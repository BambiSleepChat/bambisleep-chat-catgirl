#!/usr/bin/env node

/// Law: BambiSleep™ Hypnosis Audio Management MCP Server
/// SPDX-License-Identifier: MIT

const { Server } = require('@modelcontextprotocol/sdk/server/index.js');
const { StdioServerTransport } = require('@modelcontextprotocol/sdk/server/stdio.js');
const {
  ListResourcesRequestSchema,
  ReadResourceRequestSchema,
  ListToolsRequestSchema,
  CallToolRequestSchema,
} = require('@modelcontextprotocol/sdk/types.js');

const fs = require('fs').promises;
const path = require('path');

//<3 Lore: This MCP server manages BambiSleep™ hypnosis audio files
//<3 including playlists, trigger metadata, and audio processing

//! Ritual: Audio file storage directory
const AUDIO_BASE_PATH = process.env.BAMBISLEEP_AUDIO_PATH || path.join(__dirname, 'audio');

/// Law: Audio file metadata structure
class AudioMetadata {
  constructor(data) {
    this.id = data.id;
    this.title = data.title;
    this.filename = data.filename;
    this.duration = data.duration; // seconds
    this.triggers = data.triggers || []; // Array of trigger names
    this.series = data.series; // "Bambi Sleep", "Bambi Training", etc.
    this.trackNumber = data.trackNumber;
    this.tags = data.tags || [];
    this.created = data.created || new Date().toISOString();
  }
}

/// Law: Playlist structure
class Playlist {
  constructor(data) {
    this.id = data.id;
    this.name = data.name;
    this.tracks = data.tracks || []; // Array of audio IDs
    this.duration = data.duration || 0;
    this.description = data.description || '';
    this.created = data.created || new Date().toISOString();
    this.modified = data.modified || new Date().toISOString();
  }
}

/// Law: In-memory storage for audio metadata and playlists
const audioLibrary = new Map();
const playlists = new Map();

//-> Strategy: MCP Server initialization
const server = new Server(
  {
    name: 'bambisleep-hypnosis-mcp',
    version: '1.0.0',
  },
  {
    capabilities: {
      resources: {},
      tools: {},
    },
  }
);

/// Law: Resource handlers - expose audio library and playlists
server.setRequestHandler(ListResourcesRequestSchema, async () => {
  const resources = [];
  
  // Audio library resource
  resources.push({
    uri: 'bambisleep://audio/library',
    name: 'Audio Library',
    description: 'Complete BambiSleep™ audio file collection',
    mimeType: 'application/json',
  });
  
  // Individual playlists
  for (const [id, playlist] of playlists.entries()) {
    resources.push({
      uri: `bambisleep://playlists/${id}`,
      name: playlist.name,
      description: `Playlist: ${playlist.description}`,
      mimeType: 'application/json',
    });
  }
  
  return { resources };
});

server.setRequestHandler(ReadResourceRequestSchema, async (request) => {
  const uri = request.params.uri;
  
  if (uri === 'bambisleep://audio/library') {
    const library = Array.from(audioLibrary.values());
    return {
      contents: [{
        uri,
        mimeType: 'application/json',
        text: JSON.stringify(library, null, 2),
      }],
    };
  }
  
  const playlistMatch = uri.match(/^bambisleep:\/\/playlists\/(.+)$/);
  if (playlistMatch) {
    const playlistId = playlistMatch[1];
    const playlist = playlists.get(playlistId);
    if (!playlist) {
      throw new Error(`Playlist not found: ${playlistId}`);
    }
    return {
      contents: [{
        uri,
        mimeType: 'application/json',
        text: JSON.stringify(playlist, null, 2),
      }],
    };
  }
  
  throw new Error(`Unknown resource: ${uri}`);
});

/// Law: Tool handlers - audio and playlist management
server.setRequestHandler(ListToolsRequestSchema, async () => {
  return {
    tools: [
      {
        name: 'add_audio_file',
        description: 'Add a new audio file to the library with metadata',
        inputSchema: {
          type: 'object',
          properties: {
            title: { type: 'string', description: 'Audio track title' },
            filename: { type: 'string', description: 'Filename with extension' },
            duration: { type: 'number', description: 'Duration in seconds' },
            triggers: { type: 'array', items: { type: 'string' }, description: 'Trigger names used in audio' },
            series: { type: 'string', description: 'Series name (e.g., "Bambi Sleep")' },
            trackNumber: { type: 'number', description: 'Track number in series' },
            tags: { type: 'array', items: { type: 'string' }, description: 'Additional tags' },
          },
          required: ['title', 'filename', 'duration', 'series'],
        },
      },
      {
        name: 'search_audio',
        description: 'Search audio library by trigger, series, or tags',
        inputSchema: {
          type: 'object',
          properties: {
            trigger: { type: 'string', description: 'Filter by trigger name' },
            series: { type: 'string', description: 'Filter by series name' },
            tags: { type: 'array', items: { type: 'string' }, description: 'Filter by tags' },
          },
        },
      },
      {
        name: 'create_playlist',
        description: 'Create a new playlist from audio tracks',
        inputSchema: {
          type: 'object',
          properties: {
            name: { type: 'string', description: 'Playlist name' },
            trackIds: { type: 'array', items: { type: 'string' }, description: 'Array of audio IDs' },
            description: { type: 'string', description: 'Playlist description' },
          },
          required: ['name', 'trackIds'],
        },
      },
      {
        name: 'get_playlist',
        description: 'Get playlist details with full track information',
        inputSchema: {
          type: 'object',
          properties: {
            playlistId: { type: 'string', description: 'Playlist ID' },
          },
          required: ['playlistId'],
        },
      },
      {
        name: 'list_triggers',
        description: 'List all unique triggers in the audio library',
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
    case 'add_audio_file': {
      const id = `audio-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
      const metadata = new AudioMetadata({ id, ...args });
      audioLibrary.set(id, metadata);
      
      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ success: true, id, metadata }, null, 2),
        }],
      };
    }
    
    case 'search_audio': {
      let results = Array.from(audioLibrary.values());
      
      if (args.trigger) {
        results = results.filter(audio => 
          audio.triggers.some(t => t.toLowerCase().includes(args.trigger.toLowerCase()))
        );
      }
      
      if (args.series) {
        results = results.filter(audio => 
          audio.series.toLowerCase().includes(args.series.toLowerCase())
        );
      }
      
      if (args.tags && args.tags.length > 0) {
        results = results.filter(audio => 
          args.tags.some(tag => audio.tags.includes(tag))
        );
      }
      
      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ results, count: results.length }, null, 2),
        }],
      };
    }
    
    case 'create_playlist': {
      const id = `playlist-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
      
      // Calculate total duration
      let totalDuration = 0;
      for (const trackId of args.trackIds) {
        const audio = audioLibrary.get(trackId);
        if (audio) {
          totalDuration += audio.duration;
        }
      }
      
      const playlist = new Playlist({
        id,
        name: args.name,
        tracks: args.trackIds,
        duration: totalDuration,
        description: args.description || '',
      });
      
      playlists.set(id, playlist);
      
      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ success: true, id, playlist }, null, 2),
        }],
      };
    }
    
    case 'get_playlist': {
      const playlist = playlists.get(args.playlistId);
      if (!playlist) {
        throw new Error(`Playlist not found: ${args.playlistId}`);
      }
      
      // Hydrate with full track information
      const tracks = playlist.tracks.map(id => audioLibrary.get(id)).filter(Boolean);
      
      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ playlist, tracks }, null, 2),
        }],
      };
    }
    
    case 'list_triggers': {
      const triggersSet = new Set();
      for (const audio of audioLibrary.values()) {
        audio.triggers.forEach(t => triggersSet.add(t));
      }
      
      const triggers = Array.from(triggersSet).sort();
      
      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ triggers, count: triggers.length }, null, 2),
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
  
  //<3 Lore: Server is now ready to manage BambiSleep™ audio files
  console.error('BambiSleep™ Hypnosis MCP Server running on stdio');
}

main().catch((error) => {
  console.error('Fatal error:', error);
  process.exit(1);
});
