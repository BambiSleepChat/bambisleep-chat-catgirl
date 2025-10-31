/**
 * 🌸 Unity IPC Bridge - Node.js ↔ Unity Communication
 *
 * Handles bidirectional JSON-based communication between Node.js and Unity Editor
 * via stdin/stdout pipes.
 *
 * @module src/unity/unity-bridge
 * @see docs/architecture/UNITY_IPC_PROTOCOL.md
 */

'use strict';

const { spawn } = require('child_process');
const EventEmitter = require('events');

/**
 * Unity Bridge for IPC communication
 * @extends EventEmitter
 */
class UnityBridge extends EventEmitter {
  /**
   * Create a Unity bridge
   * @param {Object} options Configuration options
   * @param {string} options.unityPath Path to Unity executable
   * @param {string} options.projectPath Path to Unity project
   * @param {boolean} [options.batchMode=true] Run Unity in batch mode
   */
  constructor(options) {
    super();

    this.unityPath = options.unityPath;
    this.projectPath = options.projectPath;
    this.batchMode = options.batchMode !== false;
    this.process = null;
    this.messageBuffer = '';
  }

  /**
   * Start Unity process and establish IPC connection
   * @returns {Promise<void>}
   */
  async start() {
    const args = [
      '-projectPath', this.projectPath,
      '-executeMethod', 'IPCBridge.StartIPC'
    ];

    if (this.batchMode) {
      args.unshift('-batchmode');
    }

    this.process = spawn(this.unityPath, args);

    // Handle stdout (messages from Unity)
    this.process.stdout.on('data', (data) => {
      this._handleStdout(data);
    });

    // Handle stderr (Unity logs)
    this.process.stderr.on('data', (data) => {
      this.emit('unity:log', data.toString());
    });

    // Handle process exit
    this.process.on('exit', (code) => {
      this.emit('unity:exit', code);
    });

    this.emit('ready');
  }

  /**
   * Send message to Unity
   * @param {string} type Message type
   * @param {Object} data Message payload
   */
  sendMessage(type, data) {
    if (!this.process) {
      throw new Error('Unity process not started');
    }

    const message = {
      type,
      timestamp: new Date().toISOString(),
      data
    };

    this.process.stdin.write(JSON.stringify(message) + '\n');
  }

  /**
   * Stop Unity process
   */
  stop() {
    if (this.process) {
      this.process.kill();
      this.process = null;
    }
  }

  /**
   * Handle stdout data from Unity
   * @private
   */
  _handleStdout(data) {
    this.messageBuffer += data.toString();
    const lines = this.messageBuffer.split('\n');

    // Process all complete lines
    for (let i = 0; i < lines.length - 1; i++) {
      const line = lines[i].trim();
      if (line) {
        try {
          const message = JSON.parse(line);
          this.emit(`unity:${message.type}`, message.data);
          this.emit('unity:message', message);
        } catch (e) {
          this.emit('unity:parse-error', { line, error: e.message });
        }
      }
    }

    // Keep incomplete line in buffer
    this.messageBuffer = lines[lines.length - 1];
  }
}

module.exports = UnityBridge;
