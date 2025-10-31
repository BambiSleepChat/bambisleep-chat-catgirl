/**
 * 🌸 Server Tests
 *
 * Comprehensive tests for HTTP/WebSocket server
 */

'use strict';

const request = require('supertest');
const { createApp } = require('../src/server');

describe('Server', () => {
  let app;

  beforeEach(() => {
    app = createApp();
  });

  describe('GET /health', () => {
    it('should return healthy status', async () => {
      const response = await request(app)
        .get('/health')
        .expect(200);

      expect(response.body.status).toBe('healthy');
      expect(response.body.timestamp).toBeDefined();
      expect(response.body.cuteness).toBeDefined();
    });
  });

  describe('GET /api/status', () => {
    it('should return service status', async () => {
      const response = await request(app)
        .get('/api/status')
        .expect(200);

      expect(response.body.service).toContain('BambiSleep');
      expect(response.body.version).toBeDefined();
      expect(response.body.unity).toBeDefined();
      expect(response.body.features).toBeDefined();
    });
  });

  describe('POST /api/unity/message', () => {
    it('should accept Unity messages', async () => {
      const message = {
        type: 'initialize',
        data: { scene: 'MainMenu' }
      };

      const response = await request(app)
        .post('/api/unity/message')
        .send(message)
        .expect(200);

      expect(response.body.success).toBe(true);
      expect(response.body.type).toBe('initialize');
    });

    it('should handle missing type', async () => {
      const response = await request(app)
        .post('/api/unity/message')
        .send({ data: {} })
        .expect(200);

      expect(response.body.success).toBe(true);
    });
  });

  describe('GET /api/mcp/status', () => {
    it('should return MCP status', async () => {
      const response = await request(app)
        .get('/api/mcp/status')
        .expect(200);

      expect(response.body.enabled).toBeDefined();
      expect(response.body.configPath).toBeDefined();
      expect(response.body.servers).toBe(8);
    });
  });

  describe('CORS', () => {
    it('should set CORS headers', async () => {
      const response = await request(app)
        .get('/health')
        .expect(200);

      expect(response.headers['access-control-allow-origin']).toBeDefined();
    });

    it('should handle OPTIONS requests', async () => {
      await request(app)
        .options('/api/status')
        .expect(200);
    });
  });

  describe('Error handling', () => {
    it('should return 404 for unknown routes', async () => {
      const response = await request(app)
        .get('/unknown')
        .expect(404);

      expect(response.body.error).toBe('Not Found');
    });
  });
});
