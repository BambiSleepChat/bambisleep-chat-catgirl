#!/usr/bin/env node

/// Law: BambiSleep™ Hypnotic Trigger Management MCP Server
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

//<3 Lore: This MCP server manages hypnotic trigger registration,
//<3 activation patterns, and compliance tracking for BambiSleep™

//!? Guardrail: Ethical safety check for trigger management
//!? All trigger activations REQUIRE informed consent and should
//!? only be used with willing participants who understand the content

/// Law: Trigger registration structure
class HypnoticTrigger {
  constructor(data) {
    this.id = data.id;
    this.name = data.name; // e.g., "Bambi Sleep", "Good Girl"
    this.type = data.type; // "activation", "conditioning", "deepener", "amnesia"
    this.command = data.command; // Exact phrase/word
    this.effect = data.effect; // Description of intended effect
    this.duration = data.duration || null; // Duration in seconds (null = indefinite)
    this.prerequisites = data.prerequisites || []; // Required prior triggers
    this.complianceRequired = data.complianceRequired !== false; // Default true
    this.activationCount = data.activationCount || 0;
    this.lastActivated = data.lastActivated || null;
    this.created = data.created || new Date().toISOString();
  }

  activate() {
    this.activationCount++;
    this.lastActivated = new Date().toISOString();
  }
}

/// Law: Trigger activation log entry
class ActivationLog {
  constructor(data) {
    this.id = data.id;
    this.triggerId = data.triggerId;
    this.triggerName = data.triggerName;
    this.timestamp = data.timestamp || new Date().toISOString();
    this.userId = data.userId || 'anonymous';
    this.context = data.context || {};
    this.complianceAcknowledged = data.complianceAcknowledged || false;
  }
}

/// Law: Trigger state manager with event emission
class TriggerSystem extends EventEmitter {
  constructor() {
    super();
    this.triggers = new Map();
    this.activeLogs = [];
    this.maxLogSize = 1000;
  }

  registerTrigger(trigger) {
    this.triggers.set(trigger.id, trigger);
    this.emit('triggerRegistered', trigger);
    return trigger;
  }

  activateTrigger(triggerId, context = {}) {
    const trigger = this.triggers.get(triggerId);
    if (!trigger) {
      throw new Error(`Trigger not found: ${triggerId}`);
    }

    //!? Guardrail: Compliance check
    if (trigger.complianceRequired && !context.complianceAcknowledged) {
      throw new Error(`Compliance acknowledgment required for trigger: ${trigger.name}`);
    }

    // Check prerequisites
    for (const prereqId of trigger.prerequisites) {
      const prereq = this.triggers.get(prereqId);
      if (!prereq || prereq.activationCount === 0) {
        throw new Error(`Prerequisite not met: ${prereqId}`);
      }
    }

    trigger.activate();

    const log = new ActivationLog({
      id: `log-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`,
      triggerId: trigger.id,
      triggerName: trigger.name,
      userId: context.userId,
      context,
      complianceAcknowledged: context.complianceAcknowledged || false,
    });

    this.activeLogs.push(log);

    // Trim log if too large
    if (this.activeLogs.length > this.maxLogSize) {
      this.activeLogs.shift();
    }

    this.emit('triggerActivated', {
      trigger,
      log,
    });

    return { trigger, log };
  }

  searchTriggers(query) {
    const results = [];
    const lowerQuery = query.toLowerCase();

    for (const trigger of this.triggers.values()) {
      if (
        trigger.name.toLowerCase().includes(lowerQuery) ||
        trigger.command.toLowerCase().includes(lowerQuery) ||
        trigger.type.toLowerCase().includes(lowerQuery) ||
        trigger.effect.toLowerCase().includes(lowerQuery)
      ) {
        results.push(trigger);
      }
    }

    return results;
  }

  getComplianceStats() {
    const total = this.activeLogs.length;
    const compliant = this.activeLogs.filter(log => log.complianceAcknowledged).length;

    return {
      totalActivations: total,
      compliantActivations: compliant,
      complianceRate: total > 0 ? (compliant / total * 100).toFixed(2) + '%' : '0%',
    };
  }
}

//-> Strategy: Initialize trigger system
const triggerSystem = new TriggerSystem();

//-> Strategy: Event logging for security and debugging
triggerSystem.on('triggerRegistered', (trigger) => {
  console.error(`[TRIGGER-MCP] Trigger registered: ${trigger.name} (${trigger.type})`);
});

triggerSystem.on('triggerActivated', (event) => {
  console.error(`[TRIGGER-MCP] Trigger activated: ${event.trigger.name} (count: ${event.trigger.activationCount})`);
});

//-> Strategy: MCP Server initialization
const server = new Server(
  {
    name: 'trigger-system-mcp',
    version: '1.0.0',
  },
  {
    capabilities: {
      resources: {},
      tools: {},
    },
  }
);

/// Law: Resource handlers - expose triggers and logs
server.setRequestHandler(ListResourcesRequestSchema, async () => {
  const resources = [
    {
      uri: 'triggers://registry',
      name: 'Trigger Registry',
      description: 'Complete hypnotic trigger registry',
      mimeType: 'application/json',
    },
    {
      uri: 'triggers://logs',
      name: 'Activation Logs',
      description: 'Trigger activation history',
      mimeType: 'application/json',
    },
    {
      uri: 'triggers://compliance',
      name: 'Compliance Statistics',
      description: 'Compliance tracking metrics',
      mimeType: 'application/json',
    },
  ];

  return { resources };
});

server.setRequestHandler(ReadResourceRequestSchema, async (request) => {
  const uri = request.params.uri;

  if (uri === 'triggers://registry') {
    const registry = Array.from(triggerSystem.triggers.values());
    return {
      contents: [{
        uri,
        mimeType: 'application/json',
        text: JSON.stringify(registry, null, 2),
      }],
    };
  }

  if (uri === 'triggers://logs') {
    return {
      contents: [{
        uri,
        mimeType: 'application/json',
        text: JSON.stringify(triggerSystem.activeLogs, null, 2),
      }],
    };
  }

  if (uri === 'triggers://compliance') {
    const stats = triggerSystem.getComplianceStats();
    return {
      contents: [{
        uri,
        mimeType: 'application/json',
        text: JSON.stringify(stats, null, 2),
      }],
    };
  }

  throw new Error(`Unknown resource: ${uri}`);
});

/// Law: Tool handlers - trigger management
server.setRequestHandler(ListToolsRequestSchema, async () => {
  return {
    tools: [
      {
        name: 'register_trigger',
        description: 'Register a new hypnotic trigger',
        inputSchema: {
          type: 'object',
          properties: {
            name: { type: 'string', description: 'Trigger name' },
            command: { type: 'string', description: 'Exact activation phrase' },
            type: { type: 'string', description: 'Trigger type', enum: ['activation', 'conditioning', 'deepener', 'amnesia', 'reinforcement'] },
            effect: { type: 'string', description: 'Description of intended effect' },
            duration: { type: 'number', description: 'Duration in seconds (null = indefinite)' },
            prerequisites: { type: 'array', items: { type: 'string' }, description: 'Required prior trigger IDs' },
            complianceRequired: { type: 'boolean', description: 'Require compliance acknowledgment' },
          },
          required: ['name', 'command', 'type', 'effect'],
        },
      },
      {
        name: 'activate_trigger',
        description: 'Activate a registered trigger',
        inputSchema: {
          type: 'object',
          properties: {
            triggerId: { type: 'string', description: 'Trigger ID to activate' },
            userId: { type: 'string', description: 'User ID activating trigger' },
            complianceAcknowledged: { type: 'boolean', description: 'Compliance acknowledgment flag' },
            context: { type: 'object', description: 'Additional context data' },
          },
          required: ['triggerId', 'complianceAcknowledged'],
        },
      },
      {
        name: 'search_triggers',
        description: 'Search triggers by name, command, type, or effect',
        inputSchema: {
          type: 'object',
          properties: {
            query: { type: 'string', description: 'Search query' },
          },
          required: ['query'],
        },
      },
      {
        name: 'get_trigger',
        description: 'Get detailed information about a specific trigger',
        inputSchema: {
          type: 'object',
          properties: {
            triggerId: { type: 'string', description: 'Trigger ID' },
          },
          required: ['triggerId'],
        },
      },
      {
        name: 'get_activation_history',
        description: 'Get activation history for a specific trigger',
        inputSchema: {
          type: 'object',
          properties: {
            triggerId: { type: 'string', description: 'Trigger ID' },
            limit: { type: 'number', description: 'Maximum number of logs to return' },
          },
          required: ['triggerId'],
        },
      },
      {
        name: 'get_compliance_stats',
        description: 'Get compliance statistics',
        inputSchema: {
          type: 'object',
          properties: {},
        },
      },
    ],
  };
});

server.setRequestHandler(CallToolRequestSchema, async (request) => {
  const { name, arguments: args } = request.params;

  switch (name) {
    case 'register_trigger': {
      const id = `trigger-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
      const trigger = new HypnoticTrigger({ id, ...args });
      triggerSystem.registerTrigger(trigger);

      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ success: true, id, trigger }, null, 2),
        }],
      };
    }

    case 'activate_trigger': {
      const result = triggerSystem.activateTrigger(args.triggerId, args);

      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ success: true, ...result }, null, 2),
        }],
      };
    }

    case 'search_triggers': {
      const results = triggerSystem.searchTriggers(args.query);

      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ results, count: results.length }, null, 2),
        }],
      };
    }

    case 'get_trigger': {
      const trigger = triggerSystem.triggers.get(args.triggerId);
      if (!trigger) {
        throw new Error(`Trigger not found: ${args.triggerId}`);
      }

      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ trigger }, null, 2),
        }],
      };
    }

    case 'get_activation_history': {
      const limit = args.limit || 50;
      const logs = triggerSystem.activeLogs
        .filter(log => log.triggerId === args.triggerId)
        .slice(-limit);

      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ logs, count: logs.length }, null, 2),
        }],
      };
    }

    case 'get_compliance_stats': {
      const stats = triggerSystem.getComplianceStats();

      return {
        content: [{
          type: 'text',
          text: JSON.stringify(stats, null, 2),
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

  //<3 Lore: Server is now ready to manage hypnotic triggers
  console.error('BambiSleep™ Trigger System MCP Server running on stdio');
}

main().catch((error) => {
  console.error('Fatal error:', error);
  process.exit(1);
});
