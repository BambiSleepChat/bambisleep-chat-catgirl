# BambiSleep™ Hypnosis Audio Management MCP Server

**Version**: 1.0.0  
**License**: MIT  
**Organization**: BambiSleepChat

## Overview

MCP server for managing BambiSleep™ hypnosis audio files, playlists, and trigger metadata. Provides structured access to audio library management through the Model Context Protocol.

## Features

- **Audio Library Management**: Add, search, and organize hypnosis audio files
- **Trigger Tracking**: Associate hypnotic triggers with specific audio tracks
- **Playlist Creation**: Build custom playlists with automatic duration calculation
- **Series Organization**: Group audio files by series (Bambi Sleep, Training, etc.)
- **Tag System**: Flexible tagging for advanced categorization

## Installation

```bash
cd mcp-servers/bambisleep-hypnosis-mcp
npm install
```

## Usage

### As MCP Server

Add to your MCP client configuration:

```json
{
  "mcpServers": {
    "bambisleep-hypnosis": {
      "command": "node",
      "args": ["f:\\CATHEDRAL\\bambisleep-chat-catgirl\\mcp-servers\\bambisleep-hypnosis-mcp\\index.js"],
      "env": {
        "BAMBISLEEP_AUDIO_PATH": "path/to/audio/files"
      }
    }
  }
}
```

### Environment Variables

- `BAMBISLEEP_AUDIO_PATH`: Base directory for audio file storage (default: `./audio`)

## MCP Resources

### `bambisleep://audio/library`
Complete audio library with all metadata

### `bambisleep://playlists/{id}`
Individual playlist with track list

## MCP Tools

### `add_audio_file`
Add new audio file to library

**Parameters**:
- `title` (string, required): Track title
- `filename` (string, required): Filename with extension
- `duration` (number, required): Duration in seconds
- `triggers` (array): Trigger names used in audio
- `series` (string, required): Series name
- `trackNumber` (number): Track number in series
- `tags` (array): Additional tags

### `search_audio`
Search audio library by criteria

**Parameters**:
- `trigger` (string): Filter by trigger name
- `series` (string): Filter by series name
- `tags` (array): Filter by tags

### `create_playlist`
Create new playlist from tracks

**Parameters**:
- `name` (string, required): Playlist name
- `trackIds` (array, required): Audio IDs to include
- `description` (string): Playlist description

### `get_playlist`
Get playlist with full track details

**Parameters**:
- `playlistId` (string, required): Playlist ID

### `list_triggers`
List all unique triggers in library

**Parameters**: None

## Example Usage

```javascript
// Add audio file
await callTool('add_audio_file', {
  title: 'Bambi Sleep 1 - Induction',
  filename: 'bambi-sleep-01.mp3',
  duration: 3600,
  triggers: ['Bambi Sleep', 'Good Girl'],
  series: 'Bambi Sleep',
  trackNumber: 1,
  tags: ['induction', 'conditioning']
});

// Search by trigger
await callTool('search_audio', {
  trigger: 'Bambi Sleep'
});

// Create playlist
await callTool('create_playlist', {
  name: 'Full Session',
  trackIds: ['audio-123', 'audio-456', 'audio-789'],
  description: 'Complete conditioning session'
});
```

## Security

⚠️ **CONTENT DISCLAIMER**: This server manages adult-oriented hypnosis content. The BambiSleep™ files contain conditioning material designed to affect consciousness and behavior. Use responsibly and with informed consent only.

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
- **EventEmitter patterns**: For extensibility
- **MCP SDK ^1.0.0**: Latest Model Context Protocol

## Integration

Designed to work with:
- **bambisleep-chat-catgirl**: Unity avatar system
- **bambisleep-church-catgirl-control-tower**: MCP orchestrator
- **bambisleep-church**: Express.js web application

## License

MIT © BambiSleepChat

---

**Part of the CATHEDRAL Project** | [GitHub](https://github.com/BambiSleepChat)
