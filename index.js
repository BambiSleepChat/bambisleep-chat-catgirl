#!/usr/bin/env node
/**
 * 🌸 BambiSleep™ CatGirl Avatar System - Main Entry Point
 *
 * This is the main entry point for the Node.js application layer that
 * communicates with Unity via IPC (Inter-Process Communication).
 *
 * @module index
 * @see docs/architecture/UNITY_IPC_PROTOCOL.md for IPC protocol details
 */

'use strict';

const path = require('path');
const { version } = require('./package.json');

/**
 * Main application entry point
 */
function main() {
  console.log('🌸✨ BambiSleep™ CatGirl Avatar System ✨🌸');
  console.log(`Version: ${version}`);
  console.log('Node.js:', process.version);
  console.log('');

  console.log('📋 Available Commands:');
  console.log('  npm start          - Display this message');
  console.log('  npm test           - Run test suite');
  console.log('  npm run build      - Build for production');
  console.log('  npm run mcp:setup  - Configure MCP servers');
  console.log('  npm run dev        - Start development server');
  console.log('');

  console.log('📚 Documentation:');
  console.log('  Unity Setup:  docs/development/UNITY_SETUP_GUIDE.md');
  console.log('  Architecture: docs/architecture/CATGIRL.md');
  console.log('  Build Guide:  docs/guides/build.md');
  console.log('');

  console.log('🎮 Unity Project Location:');
  console.log('  ' + path.resolve(__dirname, 'catgirl-avatar-project'));
  console.log('');

  console.log('💎 Ready for development! Nyan nyan nyan! 💎');
}

// Only run if executed directly (not required as module)
if (require.main === module) {
  main();
}

module.exports = { main };
