/**
 * 🌸 Server Module - HTTP/WebSocket Server
 *
 * Provides REST API and WebSocket server for Unity integration,
 * MCP management, and real-time communication.
 *
 * @module src/server/index
 */

'use strict';

const express = require('express');
const WebSocket = require('ws');
const http = require('http');
const config = require('../../config');
const logger = require('../utils/logger').child('server');

/**
 * Create and configure Express app
 * @returns {express.Application}
 */
function createApp() {
  const app = express();

  // Middleware
  app.use(express.json());
  app.use(express.urlencoded({ extended: true }));

  // CORS
  app.use((req, res, next) => {
    res.header('Access-Control-Allow-Origin', config.server.cors.origin);
    res.header('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE, OPTIONS');
    res.header('Access-Control-Allow-Headers', 'Content-Type, Authorization');
    if (req.method === 'OPTIONS') {
      return res.sendStatus(200);
    }
    next();
  });

  // Health check endpoint
  app.get('/health', (req, res) => {
    res.json({
      status: 'healthy',
      timestamp: new Date().toISOString(),
      cuteness: config.bambi.cutenessLevel,
      cowPowers: config.features.cowPowers ? 'UNLOCKED' : 'LOCKED'
    });
  });

  // API Routes
  app.get('/api/status', (req, res) => {
    res.json({
      service: 'BambiSleep™ CatGirl Avatar System',
      version: require('../../package.json').version,
      unity: {
        projectPath: config.unity.projectPath,
        environment: config.unity.environment
      },
      features: config.features
    });
  });

  // Unity IPC endpoint
  app.post('/api/unity/message', (req, res) => {
    const { type, data } = req.body;
    logger.info(`Received Unity message: ${type}`);
    // TODO: Forward to Unity bridge
    res.json({ success: true, type });
  });

  // MCP management endpoints
  app.get('/api/mcp/status', (req, res) => {
    res.json({
      enabled: config.mcp.enabled,
      configPath: config.mcp.configPath,
      servers: 8 // TODO: Get actual server count
    });
  });

  // Error handler
  app.use((err, req, res, next) => {
    logger.error('Server error:', err);
    res.status(500).json({
      error: 'Internal Server Error',
      message: config.isDevelopment ? err.message : 'Something went wrong'
    });
  });

  // 404 handler
  app.use((req, res) => {
    res.status(404).json({ error: 'Not Found' });
  });

  return app;
}

/**
 * Start the web server with WebSocket support
 * @param {Object} options Server options
 * @returns {Promise<{httpServer: http.Server, wss: WebSocket.Server}>}
 */
async function startServer(options = {}) {
  const port = options.port || config.server.port;
  const wsPort = options.wsPort || config.server.wsPort;

  const app = createApp();
  const httpServer = http.createServer(app);

  // WebSocket server
  const wss = new WebSocket.Server({ port: wsPort });

  wss.on('connection', (ws, req) => {
    const clientId = Math.random().toString(36).substr(2, 9);
    logger.info(`🦋 WebSocket client connected: ${clientId}`);

    ws.on('message', (message) => {
      try {
        const data = JSON.parse(message);
        logger.debug(`Received WS message from ${clientId}:`, data);

        // Echo back for now (TODO: implement proper handlers)
        ws.send(JSON.stringify({
          type: 'ack',
          clientId,
          timestamp: new Date().toISOString()
        }));
      } catch (e) {
        logger.error(`Invalid WebSocket message from ${clientId}:`, e);
      }
    });

    ws.on('close', () => {
      logger.info(`WebSocket client disconnected: ${clientId}`);
    });

    ws.on('error', (error) => {
      logger.error(`WebSocket error for ${clientId}:`, error);
    });

    // Send welcome message
    ws.send(JSON.stringify({
      type: 'welcome',
      message: '🌸 Welcome to BambiSleep™ CatGirl Avatar System!',
      clientId,
      timestamp: new Date().toISOString()
    }));
  });

  // Start HTTP server
  return new Promise((resolve, reject) => {
    httpServer.listen(port, config.server.host, (err) => {
      if (err) {
        logger.error(`Failed to start server:`, err);
        return reject(err);
      }

      logger.info(`🌸 HTTP Server listening on http://${config.server.host}:${port}`);
      logger.info(`🦋 WebSocket Server listening on ws://${config.server.host}:${wsPort}`);
      logger.info(`💎 Environment: ${config.env}`);
      logger.info(`✨ API endpoints available at http://${config.server.host}:${port}/api`);

      resolve({ httpServer, wss });
    });
  });
}

/**
 * Stop the server gracefully
 * @param {http.Server} httpServer
 * @param {WebSocket.Server} wss
 * @returns {Promise<void>}
 */
async function stopServer(httpServer, wss) {
  logger.info('🛑 Shutting down servers...');

  // Close WebSocket server
  if (wss) {
    wss.clients.forEach((client) => {
      client.close();
    });
    wss.close();
  }

  // Close HTTP server
  return new Promise((resolve) => {
    if (httpServer) {
      httpServer.close(() => {
        logger.info('✅ Servers stopped');
        resolve();
      });
    } else {
      resolve();
    }
  });
}

module.exports = { startServer, stopServer, createApp };
