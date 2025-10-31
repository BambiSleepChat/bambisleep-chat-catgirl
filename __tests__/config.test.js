/**
 * 🌸 Configuration Tests
 *
 * Tests for configuration module
 */

'use strict';

describe('Configuration', () => {
  let originalEnv;

  beforeEach(() => {
    originalEnv = { ...process.env };
    jest.resetModules();
  });

  afterEach(() => {
    process.env = originalEnv;
  });

  it('should load default configuration', () => {
    const config = require('../config');

    expect(config.env).toBeDefined();
    expect(config.server).toBeDefined();
    expect(config.unity).toBeDefined();
    expect(config.features).toBeDefined();
    expect(config.bambi).toBeDefined();
  });

  it('should use environment variables', () => {
    process.env.NODE_ENV = 'production';
    process.env.PORT = '8080';
    process.env.LOG_LEVEL = 'error';
    process.env.API_KEY = 'test-api-key';
    process.env.UNITY_PROJECT_ID = 'test-project-id';

    const config = require('../config');

    expect(config.env).toBe('production');
    expect(config.server.port).toBe(8080);
    expect(config.logging.level).toBe('error');
  });

  it('should have correct default values', () => {
    const config = require('../config');

    expect(config.server.port).toBe(3000);
    expect(config.bambi.cutenessLevel).toBe('MAXIMUM_OVERDRIVE');
    expect(config.features.cowPowers).toBe(true);
  });

  it('should parse boolean environment variables', () => {
    process.env.ENABLE_COW_POWERS = 'false';
    process.env.PINK_FRILLY_MODE = 'false';

    const config = require('../config');

    expect(config.features.cowPowers).toBe(false);
    expect(config.bambi.pinkFrillyMode).toBe(false);
  });

  it('should identify production environment', () => {
    process.env.NODE_ENV = 'production';
    process.env.API_KEY = 'test-api-key';
    process.env.UNITY_PROJECT_ID = 'test-project-id';

    const config = require('../config');

    expect(config.isProduction).toBe(true);
    expect(config.isDevelopment).toBe(false);
  });

  it('should identify development environment', () => {
    process.env.NODE_ENV = 'development';

    const config = require('../config');

    expect(config.isProduction).toBe(false);
    expect(config.isDevelopment).toBe(true);
  });
});
