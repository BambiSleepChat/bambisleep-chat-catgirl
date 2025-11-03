/**
 * 🌸 Logging Module - Winston-based Logging System
 *
 * Provides structured logging for the BambiSleep™ system with
 * emoji-enhanced output and file rotation.
 *
 * @module src/utils/logger
 */

'use strict';

const winston = require('winston');
const path = require('path');
const fs = require('fs');
const config = require('../../config');

// Ensure logs directory exists
const logsDir = path.dirname(config.logging.file);
if (!fs.existsSync(logsDir)) {
  fs.mkdirSync(logsDir, { recursive: true });
}

// Custom format with emojis
const emojiFormat = winston.format.printf(({ level, message, timestamp, ...meta }) => {
  const emoji = {
    error: '❌',
    warn: '⚠️',
    info: '💎',
    http: '🌐',
    debug: '🔍'
  }[level] || '📝';

  const metaStr = Object.keys(meta).length > 0 ? `\n${JSON.stringify(meta, null, 2)}` : '';
  return `${emoji} [${timestamp}] ${level.toUpperCase()}: ${message}${metaStr}`;
});

// Create logger instance
const logger = winston.createLogger({
  level: config.logging.level,
  format: winston.format.combine(
    winston.format.timestamp({ format: 'YYYY-MM-DD HH:mm:ss' }),
    winston.format.errors({ stack: true }),
    winston.format.splat(),
    winston.format.json()
  ),
  defaultMeta: { service: 'bambisleep-catgirl' },
  transports: [
    // Write all logs to file
    new winston.transports.File({
      filename: config.logging.file,
      maxsize: 10485760, // 10MB
      maxFiles: 5,
      tailable: true
    })
  ]
});

// Add console transport for development
if (config.logging.console) {
  logger.add(new winston.transports.Console({
    format: winston.format.combine(
      winston.format.colorize(),
      winston.format.timestamp({ format: 'HH:mm:ss' }),
      emojiFormat
    )
  }));
}

/**
 * Create a child logger with specific context
 * @param {string} module - Module name
 * @returns {winston.Logger}
 */
const createChildLogger = function (module) {
  return logger.child({ module });
};
logger.createChild = createChildLogger;

/**
 * Log startup banner
 */
logger.banner = function () {
  if (config.logging.console) {
    console.log('\n🌸✨━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━✨🌸');
    console.log('       BambiSleep™ CatGirl Avatar System');
    console.log('🌸✨━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━✨🌸\n');
  }
  logger.info('🌸 BambiSleep™ System Starting...');
  logger.info(`📋 Environment: ${config.env}`);
  logger.info(`🎮 Unity Project: ${config.unity.projectPath}`);
  logger.info(`💎 Cuteness Level: ${config.bambi.cutenessLevel}`);
};

module.exports = logger;
