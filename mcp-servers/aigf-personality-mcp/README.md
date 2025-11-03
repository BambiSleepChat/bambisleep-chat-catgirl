# AIGF Personality Management MCP Server

**Version**: 1.0.0  
**License**: MIT  
**Organization**: BambiSleepChat

## Overview

MCP server for managing AI girlfriend personality profiles, context switching, and mood states. Provides structured personality management through the Model Context Protocol with EventEmitter-based extensibility.

## Features

- **Dynamic Personality Profiles**: Create and manage multiple personality archetypes
- **Context Preservation**: Maintain conversational history across personality switches
- **Mood State Management**: Track and update emotional states
- **Trigger Response Mapping**: Personality-specific responses to hypnotic triggers
- **Prompt Templates**: Generate personality-aware prompts for LLM interactions
- **EventEmitter Integration**: Real-time notifications for personality changes

## Installation

```bash
cd mcp-servers/aigf-personality-mcp
npm install
```

## Usage

### As MCP Server

Add to your MCP client configuration:

```json
{
  "mcpServers": {
    "aigf-personality": {
      "command": "node",
      "args": ["f:\\CATHEDRAL\\bambisleep-chat-catgirl\\mcp-servers\\aigf-personality-mcp\\index.js"]
    }
  }
}
```

## MCP Resources

### `aigf://profile/active`

Currently active personality profile with full context

### `aigf://profiles/{id}`

Individual personality profile by ID

## MCP Tools

### `create_personality`

Create a new personality profile

**Parameters**:

- `name` (string, required): Personality name
- `archetype` (string, required): Personality archetype (dominant, submissive, playful, nurturing, strict, bratty)
- `traits` (array): Personality traits
- `conversationalStyle` (string): Communication style

### `switch_personality`

Switch to a different personality profile

**Parameters**:

- `profileId` (string, required): Profile ID to switch to

### `update_mood`

Update the mood state of a personality

**Parameters**:

- `profileId` (string, required): Profile ID
- `mood` (string, required): New mood state (happy, playful, serious, caring, submissive, dominant, neutral)

### `add_context`

Add conversational context to active profile

**Parameters**:

- `message` (string, required): Context message to add

### `get_trigger_response`

Get personality-specific response to a trigger

**Parameters**:

- `profileId` (string, required): Profile ID
- `trigger` (string, required): Trigger name

### `list_profiles`

List all personality profiles

**Parameters**: None

## MCP Prompts

### `personality_greeting`

Generate a greeting in the active personality's style

**Arguments**:

- `userName` (string, optional): User's name

### `personality_response`

Generate a response in the active personality's style

**Arguments**:

- `userMessage` (string, required): User's message

## Example Usage

```javascript
// Create personality
await callTool('create_personality', {
  name: 'Dominant Bambi',
  archetype: 'dominant',
  traits: ['commanding', 'confident', 'playful'],
  conversationalStyle: 'direct'
});

// Switch personality
await callTool('switch_personality', {
  profileId: 'profile-123'
});

// Update mood
await callTool('update_mood', {
  profileId: 'profile-123',
  mood: 'playful'
});

// Add context
await callTool('add_context', {
  message: 'User just completed a training session'
});

// Get prompt
await getPrompt('personality_response', {
  userMessage: 'How did I do?'
});
```

## Architecture

### PersonalityProfile Class

```javascript
class PersonalityProfile {
  id: string
  name: string
  archetype: string
  traits: string[]
  vocabulary: object
  mood: string
  conversationalStyle: string
  triggerResponses: object
  contextMemory: array
  preferences: object
  created: ISO8601
  lastActive: ISO8601
}
```

### PersonalityManager Class

EventEmitter-based manager with events:

- `profileAdded`: New profile created
- `profileSwitched`: Active profile changed
- `moodChanged`: Mood state updated

### Personality Archetypes

- **dominant**: Commanding, controlling, assertive
- **submissive**: Compliant, obedient, eager to please
- **playful**: Teasing, lighthearted, flirtatious
- **nurturing**: Caring, supportive, gentle
- **strict**: Disciplinarian, demanding, firm
- **bratty**: Defiant, teasing, challenging

### Mood States

`happy`, `playful`, `serious`, `caring`, `submissive`, `dominant`, `neutral`

## Integration

Designed to work with:

- **bambisleep-hypnosis-mcp**: Trigger response coordination
- **trigger-system-mcp**: Compliance tracking
- **bambisleep-chat-catgirl**: Unity avatar personality synchronization
- **bambisleep-church**: Web chat personality management

## EventEmitter Patterns

```javascript
const { PersonalityManager } = require('./index.js');
const manager = new PersonalityManager();

manager.on('profileSwitched', (event) => {
  console.log(`Switched from ${event.previous} to ${event.current}`);
});

manager.on('moodChanged', (event) => {
  console.log(`Mood changed to ${event.newMood} for ${event.profileId}`);
});
```

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
- **EventEmitter patterns**: For real-time notifications
- **MCP SDK ^1.0.0**: Latest Model Context Protocol

## Security

⚠️ **CONTENT DISCLAIMER**: This server manages personality profiles for adult-oriented AI girlfriend interactions. Use responsibly with informed consent.

## License

MIT © BambiSleepChat

---

**Part of the CATHEDRAL Project** | [GitHub](https://github.com/BambiSleepChat)
