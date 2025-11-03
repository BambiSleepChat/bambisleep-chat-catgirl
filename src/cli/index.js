/**
 * 🌸 CLI Module - Command Line Interface
 *
 * Provides command-line tools for managing the BambiSleep™ system.
 * Built with Commander.js for professional CLI experience.
 *
 * @module src/cli/index
 */

'use strict';

const { Command } = require('commander');
const { version } = require('../../package.json');
const { startServer } = require('../server');
const UnityBridge = require('../unity/unity-bridge');
const config = require('../../config');
const logger = require('../utils/logger');

/**
 * Create CLI program
 * @returns {Command}
 */
function createProgram() {
  const program = new Command();

  program
    .name('bambisleep')
    .description('🌸 BambiSleep™ CatGirl Avatar System CLI')
    .version(version);

  // Server command
  program
    .command('server')
    .description('Start HTTP/WebSocket server')
    .option('-p, --port <port>', 'HTTP server port', config.server.port.toString())
    .option('-w, --ws-port <port>', 'WebSocket server port', config.server.wsPort.toString())
    .action(async (options) => {
      logger.banner();
      try {
        await startServer({
          port: parseInt(options.port, 10),
          wsPort: parseInt(options.wsPort, 10)
        });
        logger.info('✨ Server started successfully!');
      } catch (err) {
        logger.error('Failed to start server:', err);
        process.exit(1);
      }
    });

  // Unity bridge command
  program
    .command('unity')
    .description('Start Unity IPC bridge')
    .option('-u, --unity-path <path>', 'Path to Unity executable', config.unity.path)
    .option('-p, --project-path <path>', 'Path to Unity project', config.unity.projectPath)
    .option('--no-batch', 'Disable batch mode')
    .action(async (options) => {
      logger.banner();
      logger.info('🎮 Starting Unity IPC bridge...');

      const bridge = new UnityBridge({
        unityPath: options.unityPath,
        projectPath: options.projectPath,
        batchMode: options.batch
      });

      bridge.on('ready', () => {
        logger.info('✅ Unity bridge ready');
      });

      bridge.on('unity:log', (log) => {
        logger.debug('Unity:', log);
      });

      bridge.on('unity:message', (message) => {
        logger.info('Unity message:', message);
      });

      bridge.on('unity:exit', (code) => {
        logger.warn(`Unity exited with code ${code}`);
        process.exit(code);
      });

      try {
        await bridge.start();
      } catch (err) {
        logger.error('Failed to start Unity bridge:', err);
        process.exit(1);
      }
    });

  // Status command
  program
    .command('status')
    .description('Show system status')
    .action(() => {
      console.log('🌸✨ BambiSleep™ CatGirl Avatar System Status ✨🌸\n');
      console.log(`Version:         ${version}`);
      console.log(`Node.js:         ${process.version}`);
      console.log(`Environment:     ${config.env}`);
      console.log(`Unity Project:   ${config.unity.projectPath}`);
      console.log(`Cuteness Level:  ${config.bambi.cutenessLevel}`);
      console.log(`Cow Powers:      ${config.features.cowPowers ? '🐄 UNLOCKED' : '🔒 LOCKED'}`);
      console.log(`MCP Enabled:     ${config.mcp.enabled ? '✅ YES' : '❌ NO'}`);
      console.log('\n� System ready! Nyan nyan nyan! 💎\n');
    });

  // Config command
  program
    .command('config')
    .description('Show configuration')
    .option('-j, --json', 'Output as JSON')
    .action((options) => {
      if (options.json) {
        console.log(JSON.stringify(config, null, 2));
      } else {
        console.log('🌸 Configuration:');
        console.log(JSON.stringify(config, null, 2));
      }
    });

  // MCP command
  program
    .command('mcp')
    .description('MCP server management')
    .argument('[action]', 'Action to perform (setup, validate)', 'status')
    .action((action) => {
      console.log(`🌸 MCP ${action}:`);
      if (action === 'setup') {
        console.log('Run: npm run mcp:setup');
      } else if (action === 'validate') {
        console.log('Run: npm run mcp:validate');
      } else {
        console.log(`MCP Enabled: ${config.mcp.enabled}`);
        console.log(`Config Path: ${config.mcp.configPath}`);
      }
    });

  return program;
}

/**
 * Run CLI command
 * @param {string[]} args Command arguments
 * @returns {Promise<number>} Exit code
 */
async function runCLI(args) {
  const program = createProgram();

  try {
    await program.parseAsync(args);
    return 0;
  } catch (err) {
    logger.error('CLI error:', err);
    return 1;
  }
}

module.exports = { runCLI, createProgram };
