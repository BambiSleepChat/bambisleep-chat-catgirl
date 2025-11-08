# BambiSleep.Chat Analytics MCP Server

**Version**: 1.0.0  
**License**: MIT  
**Organization**: BambiSleepChat

## Overview

MCP server for tracking user engagement, session metrics, and conversion analytics for BambiSleep.Chat. Provides comprehensive analytics through the Model Context Protocol with real-time event tracking.

## Features

- **Session Tracking**: Monitor active and completed user sessions
- **Engagement Metrics**: Track messages, duration, and user activity
- **Conversion Tracking**: Record signup, subscription, and revenue events
- **Lifetime Value**: Calculate per-user lifetime value and engagement
- **Time-based Analytics**: Filter metrics by timeframe (24h, 7d, 30d, all)
- **EventEmitter Integration**: Real-time notifications for analytics events

## Installation

```bash
cd mcp-servers/chat-analytics-mcp
npm install
```

## Usage

### As MCP Server

Add to your MCP client configuration:

```json
{
  "mcpServers": {
    "chat-analytics": {
      "command": "node",
      "args": ["f:\\CATHEDRAL\\bambisleep-chat-catgirl\\mcp-servers\\chat-analytics-mcp\\index.js"]
    }
  }
}
```

## MCP Resources

### `analytics://sessions/active`

Currently active user sessions

### `analytics://sessions/completed`

Historical session data

### `analytics://users/engagement`

Per-user engagement metrics

### `analytics://conversions`

All conversion events

### `analytics://summary`

Aggregated analytics metrics

## MCP Tools

### `start_session`

Start a new user session

**Parameters**:

- `userId` (string, required): User ID
- `metadata` (object): Additional session metadata

### `end_session`

End an active session

**Parameters**:

- `sessionId` (string, required): Session ID

### `record_message`

Record a message in a session

**Parameters**:

- `sessionId` (string, required): Session ID

### `record_trigger_activation`

Record a trigger activation in a session

**Parameters**:

- `sessionId` (string, required): Session ID
- `triggerId` (string, required): Trigger ID

### `record_conversion`

Record a conversion event

**Parameters**:

- `userId` (string, required): User ID
- `eventType` (string, required): Conversion type (signup, subscription, content_unlock, referral, purchase)
- `sessionId` (string): Session ID
- `value` (number): Monetary value
- `metadata` (object): Additional event metadata

### `get_analytics`

Get aggregated analytics for a timeframe

**Parameters**:

- `timeframe` (string): Time period (all, 24h, 7d, 30d)

### `get_user_engagement`

Get engagement metrics for a specific user

**Parameters**:

- `userId` (string, required): User ID

## Example Usage

```javascript
// Start session
const session = await callTool('start_session', {
  userId: 'user-123',
  metadata: { source: 'web', device: 'desktop' }
});

// Record messages
await callTool('record_message', {
  sessionId: session.id
});

// Record trigger activation
await callTool('record_trigger_activation', {
  sessionId: session.id,
  triggerId: 'trigger-456'
});

// End session
await callTool('end_session', {
  sessionId: session.id
});

// Record conversion
await callTool('record_conversion', {
  userId: 'user-123',
  sessionId: session.id,
  eventType: 'subscription',
  value: 9.99,
  metadata: { plan: 'monthly' }
});

// Get analytics
await callTool('get_analytics', {
  timeframe: '7d'
});

// Get user engagement
await callTool('get_user_engagement', {
  userId: 'user-123'
});
```

## Architecture

### UserSession Class

```javascript
class UserSession {
  id: string
  userId: string
  startTime: ISO8601
  endTime: ISO8601 | null
  duration: number // seconds
  messageCount: number
  triggersActivated: string[]
  personalityUsed: string | null
  audioPlayed: string[]
  conversionEvents: ConversionEvent[]
  metadata: object
}
```

### ConversionEvent Class

```javascript
class ConversionEvent {
  id: string
  userId: string
  sessionId: string | null
  eventType: string // "signup" | "subscription" | "content_unlock" | "referral" | "purchase"
  timestamp: ISO8601
  value: number
  metadata: object
}
```

### UserEngagement Class

```javascript
class UserEngagement {
  userId: string
  totalSessions: number
  totalDuration: number // seconds
  totalMessages: number
  triggersActivated: string[]
  conversions: ConversionEvent[]
  firstSeen: ISO8601
  lastSeen: ISO8601
  lifetimeValue: number
}
```

### AnalyticsManager Class

EventEmitter-based manager with events:

- `sessionStarted`: New session created
- `sessionEnded`: Session completed
- `conversionRecorded`: Conversion event tracked

### Conversion Event Types

- **signup**: New user registration
- **subscription**: Paid subscription activation
- **content_unlock**: Premium content access
- **referral**: Referral link usage
- **purchase**: One-time purchase

## Analytics Metrics

### Session Metrics

- Total sessions
- Average session duration
- Average messages per session
- Active users

### Engagement Metrics

- Total messages
- Unique triggers activated
- Session frequency
- User retention

### Revenue Metrics

- Total conversions
- Total revenue
- Lifetime value per user
- Conversion rate

## Integration

Designed to work with:

- **trigger-system-mcp**: Track trigger activations in sessions
- **aigf-personality-mcp**: Monitor personality usage patterns
- **bambisleep-hypnosis-mcp**: Track audio playback analytics
- **bambisleep-church**: Web application analytics integration
- **bambisleep-chat-catgirl**: Unity avatar interaction tracking

## EventEmitter Patterns

```javascript
const { AnalyticsManager } = require('./index.js');
const manager = new AnalyticsManager();

manager.on('sessionStarted', (session) => {
  console.log(`Session started: ${session.id}`);
});

manager.on('sessionEnded', (session) => {
  console.log(`Session ended: ${session.duration}s, ${session.messageCount} messages`);
});

manager.on('conversionRecorded', (conversion) => {
  console.log(`Conversion: ${conversion.eventType} ($${conversion.value})`);
});
```

## Privacy Considerations

This server tracks user behavior and conversion metrics. Ensure compliance with:

- GDPR (EU)
- CCPA (California)
- Other applicable privacy regulations

**Recommendations**:

1. Anonymize user IDs where possible
2. Provide user data export functionality
3. Implement data retention policies
4. Obtain explicit consent for tracking
5. Document data usage in privacy policy

## Testing

```bash
npm test                  # Run tests
npm run test:coverage     # Run with coverage
npm run validate          # Validate server configuration
```

## Development

This server follows the BambiSleep™ CATHEDRAL project standards:

- **CommonJS modules**: Uses `require`/`module.exports`
- **Commentomancy**: Structured documentation with Law/Lore/Strategy/Ritual
- **EventEmitter patterns**: For real-time analytics events
- **MCP SDK ^1.0.0**: Latest Model Context Protocol

## License

MIT © BambiSleepChat

---

**Part of the CATHEDRAL Project** | [GitHub](https://github.com/BambiSleepChat)
