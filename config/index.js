/**
 * 🌸 Configuration Module - Centralized Configuration Management
 *
 * Loads and validates environment configuration for the BambiSleep™ system.
 *
 * @module config/index
 */

'use strict';

require('dotenv').config();
const path = require('path');

const config = {
  // Node.js Environment
  env: process.env.NODE_ENV || 'development',
  isDevelopment: process.env.NODE_ENV !== 'production',
  isProduction: process.env.NODE_ENV === 'production',

  // Server Configuration
  server: {
    host: process.env.SERVER_HOST || 'localhost',
    port: parseInt(process.env.PORT || process.env.SERVER_PORT || '3000', 10),
    wsPort: parseInt(process.env.WS_PORT || '3001', 10),
    cors: {
      origin: process.env.CORS_ORIGIN || '*'
    }
  },

  // Unity Configuration
  unity: {
    path: process.env.UNITY_PATH || '/opt/unity/Editor/Unity',
    projectPath: path.resolve(
      __dirname,
      '..',
      process.env.UNITY_PROJECT_PATH || './catgirl-avatar-project'
    ),
    batchMode: process.env.UNITY_BATCH_MODE !== 'false',
    projectId: process.env.UNITY_PROJECT_ID || '',
    environment: process.env.UNITY_ENVIRONMENT || 'development'
  },

  // Logging Configuration
  logging: {
    level: process.env.LOG_LEVEL || 'info',
    file: process.env.LOG_FILE || './logs/bambisleep.log',
    console: process.env.NODE_ENV !== 'production'
  },

  // MCP Configuration
  mcp: {
    enabled: process.env.MCP_SERVERS_ENABLED !== 'false',
    configPath: process.env.MCP_CONFIG_PATH || '~/.config/mcp'
  },

  // Feature Flags
  features: {
    cowPowers: process.env.ENABLE_COW_POWERS !== 'false',
    butterflyFlight: process.env.ENABLE_BUTTERFLY_FLIGHT !== 'false',
    auctionHouse: process.env.ENABLE_AUCTION_HOUSE !== 'false'
  },

  // Performance Settings
  performance: {
    maxConnections: parseInt(process.env.MAX_CONNECTIONS || '100', 10),
    requestTimeout: parseInt(process.env.REQUEST_TIMEOUT || '30000', 10)
  },

  // Security
  security: {
    apiKey: process.env.API_KEY || '',
    githubToken: process.env.GITHUB_PERSONAL_ACCESS_TOKEN || ''
  },

  // BambiSleep™ Specific
  bambi: {
    cutenessLevel: process.env.CUTENESS_LEVEL || 'MAXIMUM_OVERDRIVE',
    pinkFrillyMode: process.env.PINK_FRILLY_MODE !== 'false',
    trademarkEnforcement: process.env.TRADEMARK_ENFORCEMENT !== 'false'
  }
};

/**
 * Validate required configuration
 * @throws {Error} If required configuration is missing
 */
function validate() {
  const errors = [];

  if (config.isProduction) {
    if (!config.security.apiKey) {
      errors.push('API_KEY is required in production');
    }
    if (!config.unity.projectId) {
      errors.push('UNITY_PROJECT_ID is required in production');
    }
  }

  if (errors.length > 0) {
    throw new Error(`Configuration validation failed:\n${errors.join('\n')}`);
  }
}

// Validate on load
validate();

module.exports = config;
