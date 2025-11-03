#!/usr/bin/env node

/// Law: AI Girlfriend Personality Management MCP Server
/// SPDX-License-Identifier: MIT

const { Server } = require('@modelcontextprotocol/sdk/server/index.js');
const { StdioServerTransport } = require('@modelcontextprotocol/sdk/server/stdio.js');
const {
  ListResourcesRequestSchema,
  ReadResourceRequestSchema,
  ListToolsRequestSchema,
  CallToolRequestSchema,
  ListPromptsRequestSchema,
  GetPromptRequestSchema,
} = require('@modelcontextprotocol/sdk/types.js');

const EventEmitter = require('events');

//<3 Lore: This MCP server manages AI girlfriend personality switching
//<3 Context preservation, mood states, and conversational history

/// Law: Personality Profile structure
class PersonalityProfile {
  constructor(data) {
    this.id = data.id;
    this.name = data.name;
    this.archetype = data.archetype; // "dominant", "submissive", "playful", "nurturing", etc.
    this.traits = data.traits || []; // Array of personality traits
    this.vocabulary = data.vocabulary || {}; // Key-value pairs of preferred phrases
    this.mood = data.mood || 'neutral'; // Current mood state
    this.conversationalStyle = data.conversationalStyle || 'balanced';
    this.triggerResponses = data.triggerResponses || {}; // Trigger -> response mapping
    this.contextMemory = data.contextMemory || []; // Recent conversation context
    this.preferences = data.preferences || {};
    this.created = data.created || new Date().toISOString();
    this.lastActive = data.lastActive || new Date().toISOString();
  }
  
  updateMood(newMood) {
    this.mood = newMood;
    this.lastActive = new Date().toISOString();
  }
  
  addContext(message) {
    this.contextMemory.push({
      timestamp: new Date().toISOString(),
      content: message,
    });
    
    // Keep only last 50 messages
    if (this.contextMemory.length > 50) {
      this.contextMemory.shift();
    }
  }
}

/// Law: Personality state manager with event emission
class PersonalityManager extends EventEmitter {
  constructor() {
    super();
    this.profiles = new Map();
    this.activeProfileId = null;
    this.moodStates = ['happy', 'playful', 'serious', 'caring', 'submissive', 'dominant', 'neutral'];
  }
  
  addProfile(profile) {
    this.profiles.set(profile.id, profile);
    this.emit('profileAdded', profile);
    return profile;
  }
  
  switchProfile(profileId) {
    const profile = this.profiles.get(profileId);
    if (!profile) {
      throw new Error(`Profile not found: ${profileId}`);
    }
    
    const previousId = this.activeProfileId;
    this.activeProfileId = profileId;
    profile.lastActive = new Date().toISOString();
    
    this.emit('profileSwitched', {
      previous: previousId,
      current: profileId,
      profile,
    });
    
    return profile;
  }
  
  getActiveProfile() {
    if (!this.activeProfileId) {
      return null;
    }
    return this.profiles.get(this.activeProfileId);
  }
  
  updateMood(profileId, mood) {
    const profile = this.profiles.get(profileId);
    if (!profile) {
      throw new Error(`Profile not found: ${profileId}`);
    }
    
    if (!this.moodStates.includes(mood)) {
      throw new Error(`Invalid mood state: ${mood}`);
    }
    
    const previousMood = profile.mood;
    profile.updateMood(mood);
    
    this.emit('moodChanged', {
      profileId,
      previousMood,
      newMood: mood,
    });
    
    return profile;
  }
}

//-> Strategy: Initialize personality manager
const personalityManager = new PersonalityManager();

//-> Strategy: Event logging for debugging
personalityManager.on('profileSwitched', (event) => {
  console.error(`[AIGF-MCP] Profile switched: ${event.previous || 'none'} -> ${event.current}`);
});

personalityManager.on('moodChanged', (event) => {
  console.error(`[AIGF-MCP] Mood changed for ${event.profileId}: ${event.previousMood} -> ${event.newMood}`);
});

//-> Strategy: MCP Server initialization
const server = new Server(
  {
    name: 'aigf-personality-mcp',
    version: '1.0.0',
  },
  {
    capabilities: {
      resources: {},
      tools: {},
      prompts: {},
    },
  }
);

/// Law: Resource handlers - expose personality profiles
server.setRequestHandler(ListResourcesRequestSchema, async () => {
  const resources = [];
  
  // Active profile resource
  const activeProfile = personalityManager.getActiveProfile();
  if (activeProfile) {
    resources.push({
      uri: 'aigf://profile/active',
      name: 'Active Personality Profile',
      description: `Currently active: ${activeProfile.name}`,
      mimeType: 'application/json',
    });
  }
  
  // All profiles
  for (const [id, profile] of personalityManager.profiles.entries()) {
    resources.push({
      uri: `aigf://profiles/${id}`,
      name: profile.name,
      description: `${profile.archetype} personality`,
      mimeType: 'application/json',
    });
  }
  
  return { resources };
});

server.setRequestHandler(ReadResourceRequestSchema, async (request) => {
  const uri = request.params.uri;
  
  if (uri === 'aigf://profile/active') {
    const profile = personalityManager.getActiveProfile();
    if (!profile) {
      throw new Error('No active profile');
    }
    return {
      contents: [{
        uri,
        mimeType: 'application/json',
        text: JSON.stringify(profile, null, 2),
      }],
    };
  }
  
  const profileMatch = uri.match(/^aigf:\/\/profiles\/(.+)$/);
  if (profileMatch) {
    const profileId = profileMatch[1];
    const profile = personalityManager.profiles.get(profileId);
    if (!profile) {
      throw new Error(`Profile not found: ${profileId}`);
    }
    return {
      contents: [{
        uri,
        mimeType: 'application/json',
        text: JSON.stringify(profile, null, 2),
      }],
    };
  }
  
  throw new Error(`Unknown resource: ${uri}`);
});

/// Law: Tool handlers - personality management
server.setRequestHandler(ListToolsRequestSchema, async () => {
  return {
    tools: [
      {
        name: 'create_personality',
        description: 'Create a new personality profile',
        inputSchema: {
          type: 'object',
          properties: {
            name: { type: 'string', description: 'Personality name' },
            archetype: { type: 'string', description: 'Personality archetype', enum: ['dominant', 'submissive', 'playful', 'nurturing', 'strict', 'bratty'] },
            traits: { type: 'array', items: { type: 'string' }, description: 'Personality traits' },
            conversationalStyle: { type: 'string', description: 'Communication style' },
          },
          required: ['name', 'archetype'],
        },
      },
      {
        name: 'switch_personality',
        description: 'Switch to a different personality profile',
        inputSchema: {
          type: 'object',
          properties: {
            profileId: { type: 'string', description: 'Profile ID to switch to' },
          },
          required: ['profileId'],
        },
      },
      {
        name: 'update_mood',
        description: 'Update the mood state of a personality',
        inputSchema: {
          type: 'object',
          properties: {
            profileId: { type: 'string', description: 'Profile ID' },
            mood: { type: 'string', description: 'New mood state', enum: ['happy', 'playful', 'serious', 'caring', 'submissive', 'dominant', 'neutral'] },
          },
          required: ['profileId', 'mood'],
        },
      },
      {
        name: 'add_context',
        description: 'Add conversational context to active profile',
        inputSchema: {
          type: 'object',
          properties: {
            message: { type: 'string', description: 'Context message to add' },
          },
          required: ['message'],
        },
      },
      {
        name: 'get_trigger_response',
        description: 'Get personality-specific response to a trigger',
        inputSchema: {
          type: 'object',
          properties: {
            profileId: { type: 'string', description: 'Profile ID' },
            trigger: { type: 'string', description: 'Trigger name' },
          },
          required: ['profileId', 'trigger'],
        },
      },
      {
        name: 'list_profiles',
        description: 'List all personality profiles',
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
    case 'create_personality': {
      const id = `profile-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
      const profile = new PersonalityProfile({ id, ...args });
      personalityManager.addProfile(profile);
      
      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ success: true, id, profile }, null, 2),
        }],
      };
    }
    
    case 'switch_personality': {
      const profile = personalityManager.switchProfile(args.profileId);
      
      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ success: true, activeProfile: profile }, null, 2),
        }],
      };
    }
    
    case 'update_mood': {
      const profile = personalityManager.updateMood(args.profileId, args.mood);
      
      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ success: true, profile }, null, 2),
        }],
      };
    }
    
    case 'add_context': {
      const profile = personalityManager.getActiveProfile();
      if (!profile) {
        throw new Error('No active profile');
      }
      
      profile.addContext(args.message);
      
      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ success: true, contextCount: profile.contextMemory.length }, null, 2),
        }],
      };
    }
    
    case 'get_trigger_response': {
      const profile = personalityManager.profiles.get(args.profileId);
      if (!profile) {
        throw new Error(`Profile not found: ${args.profileId}`);
      }
      
      const response = profile.triggerResponses[args.trigger] || null;
      
      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ trigger: args.trigger, response, profile: profile.name }, null, 2),
        }],
      };
    }
    
    case 'list_profiles': {
      const profiles = Array.from(personalityManager.profiles.values());
      const activeId = personalityManager.activeProfileId;
      
      return {
        content: [{
          type: 'text',
          text: JSON.stringify({ profiles, activeProfileId: activeId }, null, 2),
        }],
      };
    }
    
    default:
      throw new Error(`Unknown tool: ${name}`);
  }
});

/// Law: Prompt handlers - personality-aware prompts
server.setRequestHandler(ListPromptsRequestSchema, async () => {
  return {
    prompts: [
      {
        name: 'personality_greeting',
        description: 'Generate a greeting in the active personality\'s style',
        arguments: [
          {
            name: 'userName',
            description: 'User\'s name',
            required: false,
          },
        ],
      },
      {
        name: 'personality_response',
        description: 'Generate a response in the active personality\'s style',
        arguments: [
          {
            name: 'userMessage',
            description: 'User\'s message',
            required: true,
          },
        ],
      },
    ],
  };
});

server.setRequestHandler(GetPromptRequestSchema, async (request) => {
  const { name, arguments: args } = request.params;
  const profile = personalityManager.getActiveProfile();
  
  if (!profile) {
    throw new Error('No active personality profile');
  }
  
  switch (name) {
    case 'personality_greeting': {
      const userName = args?.userName || 'there';
      const greeting = `[Personality: ${profile.name} (${profile.archetype}, ${profile.mood})]
Generate a greeting for ${userName} in this personality's style.
Traits: ${profile.traits.join(', ')}
Conversational style: ${profile.conversationalStyle}`;
      
      return {
        messages: [
          {
            role: 'user',
            content: {
              type: 'text',
              text: greeting,
            },
          },
        ],
      };
    }
    
    case 'personality_response': {
      const recentContext = profile.contextMemory.slice(-5).map(c => c.content).join('\n');
      const prompt = `[Personality: ${profile.name} (${profile.archetype}, ${profile.mood})]
Generate a response to: "${args.userMessage}"
Traits: ${profile.traits.join(', ')}
Conversational style: ${profile.conversationalStyle}
Recent context:
${recentContext}`;
      
      return {
        messages: [
          {
            role: 'user',
            content: {
              type: 'text',
              text: prompt,
            },
          },
        ],
      };
    }
    
    default:
      throw new Error(`Unknown prompt: ${name}`);
  }
});

//! Ritual: Server startup
async function main() {
  const transport = new StdioServerTransport();
  await server.connect(transport);
  
  //<3 Lore: Server is now ready to manage AI girlfriend personalities
  console.error('AIGF Personality MCP Server running on stdio');
}

main().catch((error) => {
  console.error('Fatal error:', error);
  process.exit(1);
});
