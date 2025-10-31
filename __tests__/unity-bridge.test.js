/**
 * 🌸 Unity Bridge Tests
 *
 * Comprehensive tests for Unity IPC bridge functionality
 */

'use strict';

const UnityBridge = require('../src/unity/unity-bridge');
const { EventEmitter } = require('events');

describe('UnityBridge', () => {
  let bridge;

  beforeEach(() => {
    bridge = new UnityBridge({
      unityPath: '/mock/unity',
      projectPath: '/mock/project',
      batchMode: true
    });
  });

  afterEach(() => {
    if (bridge && bridge.process) {
      bridge.stop();
    }
  });

  describe('constructor', () => {
    it('should create a bridge instance', () => {
      expect(bridge).toBeInstanceOf(UnityBridge);
      expect(bridge).toBeInstanceOf(EventEmitter);
    });

    it('should set configuration options', () => {
      expect(bridge.unityPath).toBe('/mock/unity');
      expect(bridge.projectPath).toBe('/mock/project');
      expect(bridge.batchMode).toBe(true);
    });

    it('should default batchMode to true', () => {
      const defaultBridge = new UnityBridge({
        unityPath: '/mock/unity',
        projectPath: '/mock/project'
      });
      expect(defaultBridge.batchMode).toBe(true);
    });

    it('should respect batchMode=false', () => {
      const noBatchBridge = new UnityBridge({
        unityPath: '/mock/unity',
        projectPath: '/mock/project',
        batchMode: false
      });
      expect(noBatchBridge.batchMode).toBe(false);
    });
  });

  describe('sendMessage', () => {
    it('should throw error if process not started', () => {
      expect(() => {
        bridge.sendMessage('test', { foo: 'bar' });
      }).toThrow('Unity process not started');
    });

    it('should format message correctly', () => {
      // Mock process with kill method
      bridge.process = {
        stdin: {
          write: jest.fn()
        },
        kill: jest.fn()
      };

      bridge.sendMessage('initialize', { scene: 'MainMenu' });

      expect(bridge.process.stdin.write).toHaveBeenCalled();
      const call = bridge.process.stdin.write.mock.calls[0][0];
      const message = JSON.parse(call.trim());

      expect(message.type).toBe('initialize');
      expect(message.data).toEqual({ scene: 'MainMenu' });
      expect(message.timestamp).toBeDefined();
    });
  });

  describe('_handleStdout', () => {
    it('should parse complete JSON messages', (done) => {
      const testMessage = { type: 'scene-loaded', data: { sceneName: 'Test' } };

      bridge.on('unity:scene-loaded', (data) => {
        expect(data).toEqual({ sceneName: 'Test' });
        done();
      });

      bridge._handleStdout(Buffer.from(JSON.stringify(testMessage) + '\n'));
    });

    it('should handle multiple messages', (done) => {
      const messages = [
        { type: 'msg1', data: { value: 1 } },
        { type: 'msg2', data: { value: 2 } }
      ];

      let count = 0;
      bridge.on('unity:message', (msg) => {
        count++;
        expect(msg.data.value).toBe(count);
        if (count === 2) {
          done();
        }
      });

      const data = messages.map((m) => JSON.stringify(m)).join('\n') + '\n';
      bridge._handleStdout(Buffer.from(data));
    });

    it('should buffer incomplete messages', () => {
      const partialMessage = '{"type":"test",';
      bridge._handleStdout(Buffer.from(partialMessage));

      expect(bridge.messageBuffer).toBe(partialMessage);
    });

    it('should handle parse errors gracefully', (done) => {
      bridge.on('unity:parse-error', (error) => {
        expect(error.line).toBe('invalid json');
        expect(error.error).toBeDefined();
        done();
      });

      bridge._handleStdout(Buffer.from('invalid json\n'));
    });

    it('should emit typed events', (done) => {
      const message = { type: 'render-complete', data: { path: '/output.png' } };

      bridge.on('unity:render-complete', (data) => {
        expect(data.path).toBe('/output.png');
        done();
      });

      bridge._handleStdout(Buffer.from(JSON.stringify(message) + '\n'));
    });
  });

  describe('stop', () => {
    it('should kill process if running', () => {
      const mockKill = jest.fn();
      bridge.process = { kill: mockKill };

      bridge.stop();

      expect(mockKill).toHaveBeenCalled();
      expect(bridge.process).toBeNull();
    });

    it('should handle missing process gracefully', () => {
      bridge.process = null;
      expect(() => bridge.stop()).not.toThrow();
    });
  });

  describe('event handling', () => {
    it('should emit ready event', (done) => {
      bridge.on('ready', () => {
        done();
      });

      bridge.emit('ready');
    });

    it('should emit unity:exit event', (done) => {
      bridge.on('unity:exit', (code) => {
        expect(code).toBe(0);
        done();
      });

      bridge.emit('unity:exit', 0);
    });
  });
});
