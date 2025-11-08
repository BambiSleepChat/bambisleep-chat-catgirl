#!/usr/bin/env node

/// Law: BambiSleep.Chat Analytics and Metrics MCP Server
/// SPDX-License-Identifier: MIT

const { Server } = require('@modelcontextprotocol/sdk/server/index.js');
const { StdioServerTransport } = require('@modelcontextprotocol/sdk/server/stdio.js');
const {
  ListResourcesRequestSchema,
  ReadResourceRequestSchema,
  ListToolsRequestSchema,
  CallToolRequestSchema,
} = require('@modelcontextprotocol/sdk/types.js');

const EventEmitter = require('events');

//<3 Lore: This MCP server tracks user engagement, session metrics,
//<3 and conversion analytics for BambiSleep.Chat

/// Law: Session tracking structure
class UserSession {
  constructor(data) {
    this.id = data.id;
    this.userId = data.userId;
    this.startTime = data.startTime || new Date().toISOString();
    this.endTime = data.endTime || null;
    this.duration = data.duration || 0; // seconds
    this.messageCount = data.messageCount || 0;
    this.triggersActivated = data.triggersActivated || [];
    this.personalityUsed = data.personalityUsed || null;
    this.audioPlayed = data.audioPlayed || [];
    this.conversionEvents = data.conversionEvents || [];
    this.metadata = data.metadata || {};
  }

  endSession() {
    this.endTime = new Date().toISOString();
    this.duration = Math.floor((new Date(this.endTime) - new Date(this.startTime)) / 1000);
  }
}

/// Law: Conversion event structure
class ConversionEvent {
  constructor(data) {
    this.id = data.id;
    this.userId = data.userId;
    this.sessionId = data.sessionId;
    this.eventType = data.eventType; // "signup", "subscription", "content_unlock", "referral"
    this.timestamp = data.timestamp || new Date().toISOString();
    this.value = data.value || 0; // Monetary value
    this.metadata = data.metadata || {};
  }
}

/// Law: User engagement metrics
class UserEngagement {
  constructor(data) {
    this.userId = data.userId;
    this.totalSessions = data.totalSessions || 0;
    this.totalDuration = data.totalDuration || 0; // seconds
    this.totalMessages = data.totalMessages || 0;
    this.triggersActivated = data.triggersActivated || [];
    this.conversions = data.conversions || [];
    this.firstSeen = data.firstSeen || new Date().toISOString();
    this.lastSeen = data.lastSeen || new Date().toISOString();
    this.lifetimeValue = data.lifetimeValue || 0;
  }

  updateFromSession(session) {
    this.totalSessions++;
    this.totalDuration += session.duration;
    this.totalMessages += session.messageCount;
    this.triggersActivated = [...new Set([...this.triggersActivated, ...session.triggersActivated])];
    this.lastSeen = session.endTime || new Date().toISOString();
  }
}

/// Law: Analytics manager with event emission
class AnalyticsManager extends EventEmitter {
  constructor() {
    super();
    this.activeSessions = new Map();
    this.completedSessions = [];
    this.userEngagement = new Map();
    this.conversionEvents = [];
    this.maxHistorySize = 5000;
  }

  startSession(userId, metadata = {}) {
    const id = `session-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
    const session = new UserSession({ id, userId, metadata });

    this.activeSessions.set(id, session);

    // Initialize user engagement if new user
    if (!this.userEngagement.has(userId)) {
      this.userEngagement.set(userId, new UserEngagement({ userId }));
    }

    this.emit('sessionStarted', session);
    return session;
  }

  endSession(sessionId) {
    const session = this.activeSessions.get(sessionId);
    if (!session) {
      throw new Error(`Session not found: ${sessionId}`);
    }

    session.endSession();
    this.activeSessions.delete(sessionId);
    this.completedSessions.push(session);

    // Update user engagement
    const engagement = this.userEngagement.get(session.userId);
    if (engagement) {
      engagement.updateFromSession(session);
    }

    // Trim history if too large
    if (this.completedSessions.length > this.maxHistorySize) {
      this.completedSessions.shift();
    }

    this.emit('sessionEnded', session);
    return session;
  }

  recordConversion(userId, sessionId, eventType, value = 0, metadata = {}) {
    const id = `conversion-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
    const conversion = new ConversionEvent({
      id,
      userId,
      sessionId,
      eventType,
      value,
      metadata,
    });

    this.conversionEvents.push(conversion);

    // Update user lifetime value
    const engagement = this.userEngagement.get(userId);
    if (engagement) {
      engagement.conversions.push(conversion);
      engagement.lifetimeValue += value;
    }

    this.emit('conversionRecorded', conversion);
    return conversion;
  }

  getAnalytics(timeframe = 'all') {
    const now = new Date();
    let sessions = this.completedSessions;

    if (timeframe === '24h') {
      const cutoff = new Date(now - 24 * 60 * 60 * 1000);
      sessions = sessions.filter(s => new Date(s.startTime) > cutoff);
    } else if (timeframe === '7d') {
      const cutoff = new Date(now - 7 * 24 * 60 * 60 * 1000);
      sessions = sessions.filter(s => new Date(s.startTime) > cutoff);
    }

    const totalSessions = sessions.length;
    const totalMessages = sessions.reduce((sum, s) => sum + s.messageCount, 0);
    const totalDuration = sessions.reduce((sum, s) => sum + s.duration, 0);
    const avgSessionDuration = totalSessions > 0 ? totalDuration / totalSessions : 0;
    const avgMessagesPerSession = totalSessions > 0 ? totalMessages / totalSessions : 0;

    return {
      timeframe,
      totalSessions,
      totalMessages,
      totalDuration,
      avgSessionDuration: Math.floor(avgSessionDuration),
      avgMessagesPerSession: avgMessagesPerSession.toFixed(2),
      activeUsers: this.userEngagement.size,
      totalConversions: this.conversionEvents.length,
      totalRevenue: this.conversionEvents.reduce((sum, c) => sum + c.value, 0),
    };
  }
}

//-> Strategy: Initialize analytics manager
const analyticsManager = new AnalyticsManager();

//-> Strategy: Event logging for monitoring
analyticsManager.on('sessionStarted', (session) => {
  console.error(`[ANALYTICS-MCP] Session started: ${session.id} (user: ${session.userId})`);
});

analyticsManager.on('sessionEnded', (session) => {
  console.error(`[ANALYTICS-MCP] Session ended: ${session.id} (duration: ${session.duration}s, messages: ${session.messageCount})`);
});

analyticsManager.on('conversionRecorded', (conversion) => {
  console.error(`[ANALYTICS-MCP] Conversion: ${conversion.eventType} ($${conversion.value}) - user: ${conversion.userId}`);
});

//-> Strategy: MCP Server initialization
const server = new Server(
  {
    name: 'chat-analytics-mcp',
    version: '1.0.0',
  },
  {
    capabilities: {
      resources: {},
      tools: {},
    },
  }
);

/// Law: Resource handlers - expose analytics data
server.setRequestHandler(ListResourcesRequestSchema, async () => {
  const resources = [
    {
      uri: 'analytics://sessions/active',
      name: 'Active Sessions',
      description: 'Currently active user sessions',
      mimeType: 'application/json',
    },
    {
      uri: 'analytics://sessions/completed',
      name: 'Completed Sessions',
      description: 'Historical session data',
      mimeType: 'application/json',
    },
    {
      uri: 'analytics://users/engagement',
      name: 'User Engagement',
      description: 'Per-user engagement metrics',
      mimeType: 'application/json',
    },
    {
      uri: 'analytics://conversions',
      name: 'Conversion Events',
      description: 'All conversion events',
      mimeType: 'application/json',
    },
    {
      uri: 'analytics://summary',
      name: 'Analytics Summary',
      description: 'Aggregated analytics metrics',
      mimeType: 'application/json',
    },
  ];

  return { resources };
});

server.setRequestHandler(ReadResourceRequestSchema, async (request) => {
  const uri = request.params.uri;

  if (uri === 'analytics://sessions/active') {
    const sessions = Array.from(analyticsManager.activeSessions.values());
    return {
      contents: [{
        uri,
        mimeType: 'application/json',
        text: JSON.stringify(sessions, null, 2),
      }],
    };
  }

  if (uri === 'analytics://sessions/completed') {
    return {
      contents: [{
        uri,
        mimeType: 'application/json',
        text: JSON.stringify(analyticsManager.completedSessions, null, 2),
      }],
    };
  }

  if (uri === 'analytics://users/engagement') {
    const engagement = Array.from(analyticsManager.userEngagement.values());
    return {
      contents: [{
        uri,
        mimeType: 'application/json',
        text: JSON.stringify(engagement, null, 2),
      }],
    };
  }

  if (uri === 'analytics://conversions') {
    return {
      contents: [{
        uri,
        mimeType: 'application/json',
        text: JSON.stringify(analyticsManager.conversionEvents, null, 2),
      }],
    };
  }

  if (uri === 'analytics://summary') {
    const summary = analyticsManager.getAnalytics('all');
    return {
      contents: [{
        uri,
        mimeType: 'application/json',
        text: JSON.stringify(summary, null, 2),
      }],
    };
  }

  throw new Error(`Unknown resource: ${uri}`);
});

/// Law: Tool handlers - analytics tracking
server.setRequestHandler(ListToolsRequestSchema, async () => {
  return {
    tools: [
      {
        name: 'start_session',
        description: 'Start a new user session',
        inputSchema: {
          type: 'object',
          properties: {
            userId: { type: 'string', description: 'User ID' },
            metadata: { type: 'object', description: 'Additional session metadata' },
          },
          required: ['userId'],
        },
      },
      {
        name: 'end_session',
        description: 'End an active session',
        inputSchema: {
          type: 'object',
          properties: {
            sessionId: { type: 'string', description: 'Session ID' },
          },
          required: ['sessionId'],
        },
      },
      {
        name: 'record_message',
        description: 'Record a message in a session',
        inputSchema: {
          type: 'object',
          properties: {
            sessionId: { type: 'string', description: 'Session ID' },
          },
          required: ['sessionId'],
        },
      },
      {
        name: 'record_trigger_activation',
        description: 'Record a trigger activation in a session',
        inputSchema: {
          type: 'object',
          properties: {
            sessionId: { type: 'string', description: 'Session ID' },
            triggerId: { type: 'string', description: 'Trigger ID' },
          },
          required: ['sessionId', 'triggerId'],
        },
      },
      {
        name: 'record_conversion',
        description: 'Record a conversion event',
        inputSchema: {
          type: 'object',
          properties: {
            userId: { type: 'string', description: 'User ID' },
            sessionId: { type: 'string', description: 'Session ID' },
            eventType: { type: 'string', description: 'Conversion type', enum: ['signup', 'subscription', 'content_unlock', 'referral', 'purchase'] },
            value: { type: 'number', description: 'Monetary value' },
            metadata: { type: 'object', description: 'Additional event metadata' },
          },
          required: ['userId', 'eventType'],
        },
      },
      {
        name: 'get_analytics',
        description: 'Get aggregated analytics for a timeframe',
        inputSchema: {
          type: 'object',
          properties: {
            timeframe: { type: 'string', description: 'Time period', enum: ['all', '24h', '7d', '30d'] },
          },
        },
      },
      {
        name: 'get_user_engagement',
        description: 'Get engagement metrics for a specific user',
        inputSchema: {
          type: 'object',
          properties: {
            userId: { type: 'string', description: 'User ID' },
          },
          required: ['userId'],
        },
      },
    ],
  };
});

server.setRequestHandler(CallToolRequestSchema, async (request) => {
  const { name, arguments: args } = request.params;

  switch (name) {
    case 'start_session': {
      const session = analyticsManager.startSession(args.userId, args.metadata);

      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ success: true, session }, null, 2),
        }],
      };
    }

    case 'end_session': {
      const session = analyticsManager.endSession(args.sessionId);

      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ success: true, session }, null, 2),
        }],
      };
    }

    case 'record_message': {
      const session = analyticsManager.activeSessions.get(args.sessionId);
      if (!session) {
        throw new Error(`Session not found: ${args.sessionId}`);
      }

      session.messageCount++;

      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ success: true, messageCount: session.messageCount }, null, 2),
        }],
      };
    }

    case 'record_trigger_activation': {
      const session = analyticsManager.activeSessions.get(args.sessionId);
      if (!session) {
        throw new Error(`Session not found: ${args.sessionId}`);
      }

      if (!session.triggersActivated.includes(args.triggerId)) {
        session.triggersActivated.push(args.triggerId);
      }

      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ success: true, triggersActivated: session.triggersActivated }, null, 2),
        }],
      };
    }

    case 'record_conversion': {
      const conversion = analyticsManager.recordConversion(
        args.userId,
        args.sessionId || null,
        args.eventType,
        args.value || 0,
        args.metadata || {}
      );

      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ success: true, conversion }, null, 2),
        }],
      };
    }

    case 'get_analytics': {
      const timeframe = args.timeframe || 'all';
      const analytics = analyticsManager.getAnalytics(timeframe);

      return {
        content: [{
          type: 'text',
          text: JSON.stringify(analytics, null, 2),
        }],
      };
    }

    case 'get_user_engagement': {
      const engagement = analyticsManager.userEngagement.get(args.userId);
      if (!engagement) {
        throw new Error(`User not found: ${args.userId}`);
      }

      return {
        content: [{
          type: 'text',
          text: JSON.stringify(engagement, null, 2),
        }],
      };
    }

    default:
      throw new Error(`Unknown tool: ${name}`);
  }
});

//! Ritual: Server startup
async function main() {
  const transport = new StdioServerTransport();
  await server.connect(transport);

  //<3 Lore: Server is now ready to track chat analytics
  console.error('BambiSleep.Chat Analytics MCP Server running on stdio');
}

main().catch((error) => {
  console.error('Fatal error:', error);
  process.exit(1);
});
