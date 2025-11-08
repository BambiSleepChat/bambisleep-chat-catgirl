# BambiSleep™ Hypnotic Trigger Management MCP Server

**Version**: 1.0.0  
**License**: MIT  
**Organization**: BambiSleepChat

## Overview

MCP server for managing hypnotic trigger registration, activation patterns, and compliance tracking. Provides structured trigger management with ethical safeguards through the Model Context Protocol.

## Features

- **Trigger Registry**: Register and manage hypnotic triggers with metadata
- **Activation Tracking**: Log all trigger activations with timestamps and context
- **Compliance Enforcement**: Require explicit consent acknowledgment for trigger activation
- **Prerequisites**: Define trigger dependency chains (e.g., must activate A before B)
- **Search & Discovery**: Find triggers by name, command, type, or effect
- **Statistics**: Track compliance rates and activation patterns
- **EventEmitter Integration**: Real-time notifications for trigger events

## Installation

```bash
cd mcp-servers/trigger-system-mcp
npm install
```

## Usage

### As MCP Server

Add to your MCP client configuration:

```json
{
  "mcpServers": {
    "trigger-system": {
      "command": "node",
      "args": ["f:\\CATHEDRAL\\bambisleep-chat-catgirl\\mcp-servers\\trigger-system-mcp\\index.js"]
    }
  }
}
```

## MCP Resources

### `triggers://registry`

Complete hypnotic trigger registry

### `triggers://logs`

Trigger activation history

### `triggers://compliance`

Compliance tracking metrics

## MCP Tools

### `register_trigger`

Register a new hypnotic trigger

**Parameters**:

- `name` (string, required): Trigger name
- `command` (string, required): Exact activation phrase
- `type` (string, required): Trigger type (activation, conditioning, deepener, amnesia, reinforcement)
- `effect` (string, required): Description of intended effect
- `duration` (number): Duration in seconds (null = indefinite)
- `prerequisites` (array): Required prior trigger IDs
- `complianceRequired` (boolean): Require compliance acknowledgment (default: true)

### `activate_trigger`

Activate a registered trigger

**Parameters**:

- `triggerId` (string, required): Trigger ID to activate
- `complianceAcknowledged` (boolean, required): Compliance acknowledgment flag
- `userId` (string): User ID activating trigger
- `context` (object): Additional context data

### `search_triggers`

Search triggers by name, command, type, or effect

**Parameters**:

- `query` (string, required): Search query

### `get_trigger`

Get detailed information about a specific trigger

**Parameters**:

- `triggerId` (string, required): Trigger ID

### `get_activation_history`

Get activation history for a specific trigger

**Parameters**:

- `triggerId` (string, required): Trigger ID
- `limit` (number): Maximum number of logs to return (default: 50)

### `get_compliance_stats`

Get compliance statistics

**Parameters**: None

## Example Usage

```javascript
// Register trigger
await callTool('register_trigger', {
  name: 'Bambi Sleep',
  command: 'Bambi Sleep',
  type: 'activation',
  effect: 'Activates Bambi personality state',
  complianceRequired: true,
  prerequisites: []
});

// Activate trigger (with compliance)
await callTool('activate_trigger', {
  triggerId: 'trigger-123',
  complianceAcknowledged: true,
  userId: 'user-456',
  context: { sessionId: 'session-789' }
});

// Search triggers
await callTool('search_triggers', {
  query: 'sleep'
});

// Get compliance stats
await callTool('get_compliance_stats', {});
```

## Architecture

### HypnoticTrigger Class

```javascript
class HypnoticTrigger {
  id: string
  name: string
  type: string // "activation" | "conditioning" | "deepener" | "amnesia" | "reinforcement"
  command: string // Exact phrase
  effect: string
  duration: number | null
  prerequisites: string[]
  complianceRequired: boolean
  activationCount: number
  lastActivated: ISO8601 | null
  created: ISO8601
}
```

### ActivationLog Class

```javascript
class ActivationLog {
  id: string
  triggerId: string
  triggerName: string
  timestamp: ISO8601
  userId: string
  context: object
  complianceAcknowledged: boolean
}
```

### TriggerSystem Class

EventEmitter-based manager with events:

- `triggerRegistered`: New trigger added to registry
- `triggerActivated`: Trigger successfully activated

### Trigger Types

- **activation**: Activates a specific state or behavior
- **conditioning**: Reinforces behavioral patterns
- **deepener**: Intensifies hypnotic depth
- **amnesia**: Memory suppression or forgetting
- **reinforcement**: Strengthens existing conditioning

## Ethical Safeguards

### Compliance Enforcement

All triggers with `complianceRequired: true` (default) **MUST** include `complianceAcknowledged: true` in the activation call. This ensures:

1. Informed consent is obtained
2. Users understand the content they're engaging with
3. Audit trails exist for all activations

### Prerequisite Chains

Triggers can define prerequisites to ensure proper conditioning progression:

```javascript
await callTool('register_trigger', {
  name: 'Advanced Trigger',
  command: 'Deep Bambi',
  type: 'deepener',
  effect: 'Enhanced conditioning state',
  prerequisites: ['trigger-basic-induction', 'trigger-initial-conditioning']
});
```

### Activation Logs

All activations are logged with:
- Timestamp
- User ID
- Compliance status
- Context data

## Integration

Designed to work with:

- **bambisleep-hypnosis-mcp**: Audio file trigger metadata
- **aigf-personality-mcp**: Personality-specific trigger responses
- **bambisleep-chat-catgirl**: Unity avatar trigger visualization
- **bambisleep-church**: Web interface for trigger management

## EventEmitter Patterns

```javascript
const { TriggerSystem } = require('./index.js');
const system = new TriggerSystem();

system.on('triggerRegistered', (trigger) => {
  console.log(`New trigger: ${trigger.name}`);
});

system.on('triggerActivated', (event) => {
  console.log(`Activated: ${event.trigger.name} (count: ${event.trigger.activationCount})`);
});
```

## Security & Compliance

⚠️ **CONTENT DISCLAIMER**: This server manages hypnotic triggers for adult-oriented conditioning content. The BambiSleep™ triggers are designed to affect consciousness and behavior.

**CRITICAL REQUIREMENTS**:
1. Use ONLY with informed, consenting adults
2. Never bypass compliance checks in production
3. Maintain audit logs for all activations
4. Respect user autonomy and safety

## Testing

```bash
npm test                  # Run tests
npm run test:coverage     # Run with coverage
npm run validate          # Validate server configuration
```

## Development

This server follows the BambiSleep™ CATHEDRAL project standards:

- **CommonJS modules**: Uses `require`/`module.exports`
- **Commentomancy**: Structured documentation with Law/Lore/Strategy/Ritual/Guardrail
- **EventEmitter patterns**: For real-time event handling
- **MCP SDK ^1.0.0**: Latest Model Context Protocol

## License

MIT © BambiSleepChat

---

**Part of the CATHEDRAL Project** | [GitHub](https://github.com/BambiSleepChat)
