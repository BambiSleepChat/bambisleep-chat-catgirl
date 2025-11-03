# 🌸 BambiSleep™ Unified CatGirl Knowledge Base 🌸
*Complete Reference for Unity Avatar + MCP Control Tower Systems*

**Last Updated**: November 3, 2025  
**Systems**: bambisleep-chat-catgirl + bambisleep-church-catgirl-control-tower

---

## 📖 Table of Contents

1. [System Overview](#system-overview)
2. [Architecture Patterns](#architecture-patterns)
3. [Unity C# Development](#unity-c-development)
4. [Node.js IPC Bridge](#nodejs-ipc-bridge)
5. [MCP Control Tower](#mcp-control-tower)
6. [Testing & Quality](#testing--quality)
7. [Development Workflows](#development-workflows)
8. [Critical Conventions](#critical-conventions)
9. [Debugging & Troubleshooting](#debugging--troubleshooting)
10. [Quick Reference](#quick-reference)

---

## System Overview

### Dual-Project Architecture

```
BambiSleep™ CatGirl Ecosystem
│
├── Chat-CatGirl (Unity 6.2 + Node.js)
│   ├── Unity C# Avatar System (2,491 lines, 7 systems)
│   ├── Node.js IPC Bridge (JSON stdin/stdout)
│   └── 8 MCP Servers (Filesystem, Git, GitHub, Memory, etc.)
│
└── Control-Tower (Node.js MCP Orchestrator)
    ├── MCP Orchestrator (823 lines)
    ├── Agent Coordinator (633 lines)
    ├── Express HTTP Server + WebSocket Dashboard
    └── 3-Layer Event-Driven Architecture (2,963 lines total)
```

### Technology Stack

**Unity 6.2 LTS (6000.2.11f1)**:
- C# .NET Standard 2.1
- 16 Unity Packages (UGS Economy, Netcode, XR Toolkit)
- 7 Complete Systems: Audio, Character, Economy, IPC, Networking, UI

**Node.js 20.19.5 (LTS)**:
- Express 4.19.2 (HTTP server)
- ws 8.17.0 (WebSocket)
- EventEmitter pattern (loose coupling)
- Jest 29.7.0 (80% coverage threshold)

**MCP Servers (8)**:
- Layer 0: filesystem, memory (primitives)
- Layer 1: git, github, brave-search (foundation)
- Layer 2: sequential-thinking, postgres, everything (advanced)

---

## Architecture Patterns

### 1. Three-Layer Event-Driven Architecture (Control Tower)

```javascript
/// Law: Component dependency order (canonical)
// Layer 1: Utils (no dependencies)
//   - logger.js (358 lines) - Structured logging with emojis
//   - config.js (505 lines) - Layered configuration
//
// Layer 2: Core Services (depend on utils)
//   - orchestrator.js (823 lines) - MCP server lifecycle
//   - agent-coordinator.js (633 lines) - Multi-agent orchestration
//
// Layer 3: Application (depends on Layer 1 + 2)
//   - index.js (644 lines) - Express + WebSocket server

class MCPOrchestrator extends EventEmitter {
  //<3 Lore: EventEmitter enables loose coupling
  // Multiple components can react to server lifecycle without tight dependencies
  
  async start(serverName) {
    const process = await this._spawnProcess(serverName);
    
    //-> Strategy: Emit events instead of callbacks for multi-listener support
    this.emit('server:started', { serverName, pid: process.pid });
  }
}
```

**Benefits**:
- Loose coupling between components
- Extensible (add listeners without modifying emitters)
- Testable (easy to mock events)
- Composable (chain events into workflows)

### 2. Unity C# Namespace Pattern

```csharp
/// Law: Namespace structure (mandatory for all C# scripts)
namespace BambiSleep.CatGirl.{Domain}
{
    // Domain: Audio, Character, Economy, IPC, Networking, UI
    
    [Header("🌸 Configuration Section")]
    public class CatgirlController : NetworkBehaviour
    {
        // Component implementation
    }
}
```

**Critical**: NEVER use default namespace. All scripts MUST follow this pattern.

### 3. Node.js ↔ Unity IPC Protocol

**Communication**: JSON messages via stdin/stdout pipes (newline-delimited)

```javascript
// Node.js side (src/unity/unity-bridge.js)
class UnityBridge extends EventEmitter {
    sendMessage(type, data) {
        const message = {
            type,
            timestamp: new Date().toISOString(),
            data
        };
        this.process.stdin.write(JSON.stringify(message) + '\n');
    }
    
    _handleStdout(data) {
        const lines = this.messageBuffer.split('\n');
        for (const line of lines.filter(l => l.trim())) {
            const message = JSON.parse(line);
            this.emit(`unity:${message.type}`, message.data);
        }
    }
}
```

```csharp
// Unity side (Assets/Scripts/IPC/IPCBridge.cs)
namespace BambiSleep.CatGirl.IPC
{
    public class IPCBridge : MonoBehaviour
    {
        public void SendMessage(string type, object data)
        {
            var message = new IPCMessage {
                type = type,
                timestamp = System.DateTime.UtcNow.ToString("o"),
                data = JsonUtility.ToJson(data)
            };
            Console.WriteLine(JsonUtility.ToJson(message));
        }
        
        IEnumerator ReadStdinMessages()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    string line = Console.ReadLine();
                    ProcessMessage(line);
                }
                yield return null;
            }
        }
    }
}
```

**Message Types**:
- `initialize` - Node.js → Unity (scene setup)
- `update` - Node.js → Unity (parameter changes)
- `render` - Node.js → Unity (render request)
- `ready` - Unity → Node.js (initialization complete)
- `error` - Unity → Node.js (error reporting)

### 4. Commentomancy System (Documentation Pattern)

```javascript
/// Law: Structural truth that survives refactors
// Example: Server lifecycle states (canonical)

//<3 Lore: Emotional/intentional context explaining WHY
// Example: 30-second health check balances responsiveness vs overhead

//! Ritual: Precondition enforced by MCP
// Example: Must call initialize() before start()

//!? Guardrail: Ethics gate requiring Council approval
// Example: Destructive operations need explicit approval

//-> Strategy: Architectural Decision Record
// Example: Why EventEmitter over callbacks

//* Emergence: Revelation surfaced to Knowledge Graph
// Example: Agents spontaneously forming work groups

//~ Self-mod: Recursive awareness, system modifying itself
// Example: Agent coordinator adjusts priority weights

//+ Evolution: Performance optimization target
// Example: Hot path needing optimization
```

---

## Unity C# Development

### Critical Unity Patterns

#### 1. NetworkBehaviour Lifecycle

```csharp
public class CatgirlController : NetworkBehaviour
{
    // Network variables (synced across clients)
    private NetworkVariable<float> networkPinkIntensity = new NetworkVariable<float>(1.0f);
    
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            InitializeCatgirlSystems();
            UpdateStatsServerRpc(localStats);
        }
        
        //! Ritual: Subscribe to NetworkVariable changes
        networkPinkIntensity.OnValueChanged += OnPinkIntensityChanged;
    }
    
    public override void OnNetworkDespawn()
    {
        //! Ritual: MUST unsubscribe to prevent memory leaks
        networkPinkIntensity.OnValueChanged -= OnPinkIntensityChanged;
    }
    
    [ServerRpc]
    void UpdateStatsServerRpc(CatgirlStats stats)
    {
        //-> Strategy: Server-authoritative for anti-cheat
        // Only server can modify authoritative state
    }
    
    [ClientRpc]
    void NotifyClientsClientRpc(string message)
    {
        // Broadcast to all clients
    }
}
```

#### 2. Unity Gaming Services Initialization

```csharp
/// Law: UGS initialization order (MUST follow exactly)
async void InitializeUGS()
{
    try
    {
        // 1. Core services FIRST
        await UnityServices.InitializeAsync();
        
        // 2. Authentication SECOND
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        
        // 3. Specific service THIRD (Economy, Lobby, Relay)
        var balances = await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
    }
    catch (System.Exception e)
    {
        Debug.LogError($"UGS init failed: {e.Message}");
        //-> Strategy: Graceful fallback to offline mode
    }
}
```

**Common Failure**: Calling Economy before Auth → Always check init order!

#### 3. Animator Performance Pattern

```csharp
//<3 Lore: Cache hash IDs instead of strings for performance
// String lookups are O(n), hash lookups are O(1)
// In Update() loops, this saves significant CPU time

private static readonly int Speed = Animator.StringToHash("Speed");
private static readonly int IsJumping = Animator.StringToHash("IsJumping");
private static readonly int IsPurring = Animator.StringToHash("IsPurring");
private static readonly int CowPowerActive = Animator.StringToHash("CowPowerActive");

void Update()
{
    //+ Evolution: Hot path - optimize for performance
    animator.SetFloat(Speed, currentSpeed);
    animator.SetBool(IsPurring, isPurring);
}
```

#### 4. Singleton Services Pattern

```csharp
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<AudioManager>();
            return _instance;
        }
    }
    
    void Awake()
    {
        //! Ritual: Enforce singleton pattern
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
```

#### 5. UI Toolkit Pattern (Modern Unity UI)

```csharp
namespace BambiSleep.CatGirl.UI
{
    public class InventoryUI : MonoBehaviour
    {
        private VisualElement root;
        private VisualElement inventoryContainer;
        
        void Start()
        {
            //<3 Lore: UI Toolkit uses Flexbox-like layout
            // More performant than legacy Canvas system for dynamic UI
            
            root = GetComponent<UIDocument>().rootVisualElement;
            
            // Query cached elements
            inventoryContainer = root.Q<VisualElement>("inventory-grid");
            
            // Setup event handlers
            inventoryContainer.RegisterCallback<ClickEvent>(OnSlotClicked);
        }
        
        void RefreshInventory()
        {
            inventoryContainer.Clear();
            
            foreach (var item in inventory.GetAllItems())
            {
                var slot = CreateSlotElement(item);
                inventoryContainer.Add(slot);
            }
        }
        
        VisualElement CreateSlotElement(CatgirlItem item)
        {
            var slot = new VisualElement();
            slot.AddToClassList("inventory-slot");
            slot.style.width = 64;
            slot.style.height = 64;
            slot.style.backgroundColor = GetRarityColor(item.rarity);
            
            return slot;
        }
    }
}
```

### Unity Package Dependencies (16 Total)

**Multiplayer & Networking**:
- `com.unity.netcode.gameobjects` 2.0.0 - Core multiplayer framework
- `com.unity.services.relay` 1.1.3 - NAT traversal for P2P
- `com.unity.services.lobby` 1.2.2 - Matchmaking

**Economy & Services**:
- `com.unity.services.core` 1.15.0 - Base for all UGS
- `com.unity.services.authentication` 3.3.4 - Player identity
- `com.unity.services.economy` 3.4.2 - Currency/inventory cloud storage
- `com.unity.services.analytics` 5.1.1 - Telemetry
- `com.unity.purchasing` 4.12.2 - IAP integration

**XR & Advanced**:
- `com.unity.xr.interaction.toolkit` 3.0.5 - Eye tracking, hand gestures

**UI & Visuals**:
- `com.unity.ui.toolkit` 2.0.0 - Modern UI system
- `com.unity.ugui` 2.0.0 - Legacy Canvas UI
- `com.unity.visualeffectgraph` 16.0.6 - Particle systems

**Asset Management**:
- `com.unity.addressables` 2.3.1 - Async asset loading

**Animation & Cinematics**:
- `com.unity.animation.rigging` 1.3.1 - Procedural animation (tail physics)
- `com.unity.cinemachine` 2.10.1 - Camera system
- `com.unity.timeline` 1.8.7 - Cutscenes

---

## Node.js IPC Bridge

### UnityBridge Class (src/unity/unity-bridge.js)

```javascript
const { spawn } = require('child_process');
const EventEmitter = require('events');

class UnityBridge extends EventEmitter {
    constructor(options) {
        super();
        this.unityPath = options.unityPath;
        this.projectPath = options.projectPath;
        this.batchMode = options.batchMode !== false;
        this.process = null;
        this.messageBuffer = '';
    }
    
    async start() {
        const args = [
            '-projectPath', this.projectPath,
            '-executeMethod', 'IPCBridge.StartIPC'
        ];
        
        if (this.batchMode) {
            args.unshift('-batchmode');
        }
        
        this.process = spawn(this.unityPath, args);
        
        //-> Strategy: Line-buffered message parsing
        // Unity sends newline-delimited JSON, must reassemble partial messages
        this.process.stdout.on('data', (data) => {
            this._handleStdout(data);
        });
        
        this.process.stderr.on('data', (data) => {
            this.emit('unity:log', data.toString());
        });
        
        this.process.on('exit', (code) => {
            this.emit('unity:exit', code);
        });
        
        this.emit('ready');
    }
    
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
    
    _handleStdout(data) {
        this.messageBuffer += data.toString();
        const lines = this.messageBuffer.split('\n');
        
        //! Ritual: Process complete lines, keep incomplete in buffer
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
        
        this.messageBuffer = lines[lines.length - 1];
    }
    
    stop() {
        if (this.process) {
            this.process.kill();
            this.process = null;
        }
    }
}

module.exports = UnityBridge;
```

### Testing Pattern

```javascript
// __tests__/unity-bridge.test.js
describe('UnityBridge', () => {
    let bridge;
    
    beforeEach(() => {
        bridge = new UnityBridge({
            unityPath: '/mock/unity',
            projectPath: '/mock/project'
        });
    });
    
    afterEach(() => {
        if (bridge.process) bridge.stop();
    });
    
    it('should parse complete JSON messages', (done) => {
        const testMessage = { type: 'ready', data: { status: 'ok' } };
        
        bridge.on('unity:ready', (data) => {
            expect(data).toEqual({ status: 'ok' });
            done();
        });
        
        bridge._handleStdout(Buffer.from(JSON.stringify(testMessage) + '\n'));
    });
    
    it('should handle partial messages across buffers', () => {
        const message = { type: 'test', data: { value: 123 } };
        const json = JSON.stringify(message) + '\n';
        const part1 = json.slice(0, json.length / 2);
        const part2 = json.slice(json.length / 2);
        
        let received = false;
        bridge.on('unity:test', (data) => {
            expect(data.value).toBe(123);
            received = true;
        });
        
        bridge._handleStdout(Buffer.from(part1));
        expect(received).toBe(false); // Incomplete
        
        bridge._handleStdout(Buffer.from(part2));
        expect(received).toBe(true); // Complete
    });
});
```

---

## MCP Control Tower

### Orchestrator Patterns

#### 1. Tiered Server Initialization

```javascript
/// Law: Server tiers define initialization order
const SERVER_TIERS = {
    LAYER_0: ['filesystem', 'memory'],
    LAYER_1: ['git', 'github', 'brave-search'],
    LAYER_2: ['sequential-thinking', 'postgres', 'everything']
};

//<3 Lore: Tiered init prevents circular dependencies
// Layer 0 servers are primitives (no dependencies)
// Layer 1 depends on Layer 0 (e.g., git needs filesystem)
// Layer 2 depends on Layer 0 + 1 (e.g., sequential-thinking needs memory)

class MCPOrchestrator extends EventEmitter {
    async startAll() {
        //! Ritual: MUST start in tier order
        for (const tier of ['LAYER_0', 'LAYER_1', 'LAYER_2']) {
            for (const serverName of SERVER_TIERS[tier]) {
                await this.start(serverName);
            }
        }
    }
}
```

#### 2. State Persistence Pattern

```javascript
/// Law: State file location (canonical)
const STATE_FILE = '.mcp/state.json';

//<3 Lore: JSON chosen for simplicity and version control
// SQLite would support transactions, but JSON is simpler
// for single-node deployment. Future: Redis for distributed.

class MCPOrchestrator extends EventEmitter {
    async saveState() {
        const state = {
            timestamp: Date.now(),
            servers: this.getStatus(),
            restartCounts: Object.fromEntries(this.restartCounts)
        };
        
        //! Ritual: Create directory if doesn't exist
        await fs.mkdir(path.dirname(STATE_FILE), { recursive: true });
        
        await fs.writeFile(STATE_FILE, JSON.stringify(state, null, 2));
        this.emit('state:saved', { path: STATE_FILE });
    }
    
    async loadState() {
        try {
            const data = await fs.readFile(STATE_FILE, 'utf8');
            const state = JSON.parse(data);
            
            //<3 Lore: Attempt reconnect to running servers
            // If PID exists and responsive, reuse connection
            // If PID dead/unresponsive, mark stopped and restart
            for (const [serverName, serverState] of Object.entries(state.servers)) {
                if (serverState.pid && this._isProcessAlive(serverState.pid)) {
                    this.servers.set(serverName, { ...serverState });
                    this.emit('server:reconnected', { serverName });
                }
            }
            
            return state;
        } catch (error) {
            if (error.code === 'ENOENT') {
                return { timestamp: Date.now(), servers: {}, restartCounts: {} };
            }
            throw error;
        }
    }
}
```

#### 3. Health Check Pattern

```javascript
//<3 Lore: 30-second interval balances responsiveness vs overhead
// Faster checks (10s) caught failures quicker but increased CPU 15%
// Slower checks (60s) missed transient failures that cascaded

class MCPOrchestrator extends EventEmitter {
    constructor(config) {
        super();
        this.healthCheckInterval = config.healthCheckInterval || 30000;
        this.healthCheckTimer = null;
    }
    
    startHealthChecks() {
        //+ Evolution: Consider batch health checks for scalability
        this.healthCheckTimer = setInterval(() => {
            for (const [serverName, server] of this.servers.entries()) {
                this.checkHealth(serverName).catch((error) => {
                    this.emit('health-check-failed', { serverName, error });
                });
            }
        }, this.healthCheckInterval);
    }
    
    async checkHealth(serverName) {
        const server = this.servers.get(serverName);
        if (!server || server.state !== 'running') {
            return;
        }
        
        //-> Strategy: Multiple health indicators
        const checks = {
            processAlive: this._isProcessAlive(server.pid),
            responseTime: await this._pingServer(serverName),
            memoryUsage: await this._checkMemory(server.pid)
        };
        
        if (!checks.processAlive) {
            await this.restart(serverName);
        }
    }
}
```

#### 4. Graceful Shutdown Pattern

```javascript
/// Law: Shutdown sequence (canonical order)
// 1. Stop accepting new requests
// 2. Drain in-flight requests
// 3. Stop MCP servers (reverse tier order)
// 4. Save state
// 5. Close connections
// 6. Exit process

class ControlTowerApp {
    constructor() {
        this.shutdownInProgress = false;
        
        //! Ritual: Register shutdown handlers on startup
        process.on('SIGTERM', () => this.shutdown('SIGTERM'));
        process.on('SIGINT', () => this.shutdown('SIGINT'));
    }
    
    async shutdown(signal) {
        if (this.shutdownInProgress) {
            logger.warn('Shutdown already in progress');
            return;
        }
        
        this.shutdownInProgress = true;
        logger.info(`Graceful shutdown initiated (${signal})`);
        
        try {
            // 1. Stop accepting new requests
            this.server.close();
            
            // 2. Wait for in-flight requests (max 10s)
            await this._drainConnections(10000);
            
            // 3. Stop MCP servers (reverse tier order)
            await this.orchestrator.stopAll();
            
            // 4. Save state
            await this.orchestrator.saveState();
            
            // 5. Close WebSocket connections
            this.wss.close();
            
            logger.success('Graceful shutdown complete');
            process.exit(0);
        } catch (error) {
            logger.critical('Shutdown error', { error: error.message });
            process.exit(1);
        }
    }
}
```

### Agent Coordinator Patterns

#### 1. Capability-Based Task Matching

```javascript
class AgentCoordinator extends EventEmitter {
    constructor() {
        super();
        this.agents = new Map();
        this.taskQueue = [];
    }
    
    registerAgent(agentId, capabilities) {
        //-> Strategy: Capability-based matching for flexible scaling
        this.agents.set(agentId, {
            id: agentId,
            capabilities: new Set(capabilities),
            state: 'idle',
            tasksCompleted: 0
        });
        
        this.emit('agent:registered', { agentId, capabilities });
        this._processPendingTasks();
    }
    
    submitTask(task) {
        //! Ritual: Validate task has required capabilities
        if (!task.requiredCapabilities || task.requiredCapabilities.length === 0) {
            throw new Error('Task must specify required capabilities');
        }
        
        this.taskQueue.push({
            id: this._generateTaskId(),
            priority: task.priority || 3, // Default: normal
            requiredCapabilities: task.requiredCapabilities,
            payload: task.payload,
            timestamp: Date.now()
        });
        
        //-> Strategy: Sort by priority (1=critical, 5=low)
        this.taskQueue.sort((a, b) => a.priority - b.priority);
        
        this._processPendingTasks();
    }
    
    _processPendingTasks() {
        for (const task of this.taskQueue) {
            const agent = this._findCapableAgent(task);
            
            if (agent) {
                this._assignTask(agent, task);
                this.taskQueue = this.taskQueue.filter(t => t.id !== task.id);
            }
        }
    }
    
    _findCapableAgent(task) {
        //<3 Lore: Find idle agent with ALL required capabilities
        // Prefer agents with fewer tasks for load balancing
        
        const capableAgents = Array.from(this.agents.values())
            .filter(agent => 
                agent.state === 'idle' &&
                task.requiredCapabilities.every(cap => 
                    agent.capabilities.has(cap)
                )
            )
            .sort((a, b) => a.tasksCompleted - b.tasksCompleted);
        
        return capableAgents[0] || null;
    }
    
    _assignTask(agent, task) {
        agent.state = 'working';
        agent.currentTask = task;
        
        this.emit('task:assigned', {
            agentId: agent.id,
            taskId: task.id,
            capabilities: Array.from(agent.capabilities)
        });
    }
}
```

---

## Testing & Quality

### Jest Configuration (80% Coverage Threshold)

```javascript
// jest.config.js
module.exports = {
    testEnvironment: 'node',
    coverageDirectory: 'coverage',
    collectCoverageFrom: [
        'src/**/*.js',
        'index.js',
        '!src/**/*.test.js',
        '!**/node_modules/**'
    ],
    //! Ritual: Build FAILS if below 80% coverage
    coverageThreshold: {
        global: {
            branches: 80,
            functions: 80,
            lines: 80,
            statements: 80
        }
    },
    testMatch: [
        '**/__tests__/**/*.js',
        '**/?(*.)+(spec|test).js'
    ],
    verbose: true,
    testTimeout: 10000
};
```

### Testing Patterns

#### 1. EventEmitter Testing

```javascript
describe('MCPOrchestrator', () => {
    let orchestrator;
    
    beforeEach(() => {
        orchestrator = new MCPOrchestrator({
            workspaceRoot: '/test',
            healthCheckInterval: 1000
        });
    });
    
    afterEach(async () => {
        await orchestrator.stopAll();
        orchestrator.removeAllListeners();
    });
    
    it('should emit server:started event', (done) => {
        orchestrator.on('server:started', (data) => {
            expect(data.serverName).toBe('filesystem');
            expect(data.pid).toBeGreaterThan(0);
            done();
        });
        
        orchestrator.start('filesystem');
    });
    
    it('should handle multiple listeners', () => {
        const listener1 = jest.fn();
        const listener2 = jest.fn();
        
        orchestrator.on('server:started', listener1);
        orchestrator.on('server:started', listener2);
        
        orchestrator.emit('server:started', { serverName: 'test' });
        
        expect(listener1).toHaveBeenCalledTimes(1);
        expect(listener2).toHaveBeenCalledTimes(1);
    });
});
```

#### 2. Async Testing Pattern

```javascript
describe('UnityBridge', () => {
    it('should initialize Unity and receive ready message', async () => {
        const bridge = new UnityBridge({
            unityPath: '/usr/bin/unity',
            projectPath: '/project'
        });
        
        const readyPromise = new Promise((resolve) => {
            bridge.on('unity:ready', (data) => {
                resolve(data);
            });
        });
        
        await bridge.start();
        
        const data = await readyPromise;
        expect(data.status).toBe('initialized');
    });
});
```

#### 3. Mock Child Process Pattern

```javascript
jest.mock('child_process', () => ({
    spawn: jest.fn(() => ({
        pid: 12345,
        stdout: new EventEmitter(),
        stderr: new EventEmitter(),
        stdin: {
            write: jest.fn()
        },
        kill: jest.fn(),
        on: jest.fn()
    }))
}));

describe('UnityBridge with mocked process', () => {
    it('should spawn Unity process with correct args', async () => {
        const { spawn } = require('child_process');
        const bridge = new UnityBridge({
            unityPath: '/unity',
            projectPath: '/project'
        });
        
        await bridge.start();
        
        expect(spawn).toHaveBeenCalledWith('/unity', [
            '-batchmode',
            '-projectPath', '/project',
            '-executeMethod', 'IPCBridge.StartIPC'
        ]);
    });
});
```

---

## Development Workflows

### Unity C# Debugging (VS Code)

```bash
# 1. Setup (run once)
./scripts/setup-unity-debug.sh

# 2. Launch Unity Editor
/opt/unity/Editor/Unity -projectPath /path/to/catgirl-avatar-project

# 3. In VS Code: Press F5 or select "Attach to Unity Editor and Play"

# 4. Set breakpoints in C# files, code will break when executed
```

**VS Code Launch Configurations**:
- `Attach to Unity Editor` - Attach to running process
- `Attach to Unity Editor and Play` - Attach + enter Play mode
- `.NET Core Attach` - Manual process picker

**Critical Breakpoint Locations**:
- `CatgirlController.cs:112` - `OnNetworkSpawn()` (network init)
- `InventorySystem.cs:150` - `InitializeEconomy()` (UGS setup)
- `UniversalBankingSystem.cs:144` - `AddCurrency()` (currency ops)
- `IPCBridge.cs:100` - `ProcessMessage()` (IPC handling)

### Node.js Development Workflow

```bash
# Install dependencies
npm install

# Run tests (must pass 80% coverage)
npm test

# TDD mode (auto-run on file changes)
npm run test:watch

# Start Control Tower
npm run dev  # Development mode with hot reload
npm start    # Production mode

# Start MCP servers
npm run orchestrator:start      # All servers (tiered)
npm run orchestrator:stop       # Stop all
npm run orchestrator:status     # Show status
npm run orchestrator:health     # Run health checks

# Validate MCP servers (chat-catgirl)
./scripts/mcp-validate.sh       # Tests 8/8 servers operational

# Code quality
npm run lint                    # Check code style
npm run format                  # Format with Prettier

# Build & Deploy
npm run container:build         # Docker build with GHCR labels
```

### VS Code Tasks

```jsonc
// .vscode/tasks.json
{
  "tasks": [
    {
      "label": "Run Tests",
      "type": "shell",
      "command": "npm",
      "args": ["test", "--", "--coverage=100"],
      "group": { "kind": "test", "isDefault": true }
    },
    {
      "label": "Clean Unity Project",
      "type": "shell",
      "command": "rm",
      "args": ["-rf", "catgirl-avatar-project/{Library,Temp,obj}"]
    },
    {
      "label": "Build Container",
      "type": "shell",
      "command": "docker",
      "args": ["build", "-t", "ghcr.io/bambisleepchat/catgirl:dev", "."]
    },
    {
      "label": "MCP Validate",
      "type": "shell",
      "command": "bash",
      "args": ["./scripts/mcp-validate.sh"]
    }
  ]
}
```

---

## Critical Conventions

### Project-Wide Rules

1. **BambiSleep™ Trademark**: ALWAYS use `BambiSleep™` (with ™) in public-facing content

2. **Emoji Commit Prefixes**: REQUIRED for all commits
   - `🦋` Add feature / Transformation
   - `✨` Update / Enhancement
   - `🐛` Fix bug
   - `🔥` Performance improvement
   - `📝` Documentation
   - `🎨` Code style / formatting
   - `🧪` Tests

3. **Unity C# Namespaces**: MANDATORY pattern
   ```csharp
   namespace BambiSleep.CatGirl.{Domain}
   {
       // Domain: Audio, Character, Economy, IPC, Networking, UI
   }
   ```

4. **Jest Coverage**: 80% minimum (branches, functions, lines, statements) - build FAILS if below

5. **UGS Initialization Order**: MUST follow exactly
   ```javascript
   await UnityServices.InitializeAsync();
   await AuthenticationService.Instance.SignInAnonymouslyAsync();
   await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
   ```

6. **MCP Server Tiers**: MUST start in order (Layer 0 → Layer 1 → Layer 2)

### Naming Conventions

**JavaScript**:
- Classes: `PascalCase` (MCPOrchestrator, AgentCoordinator)
- Constants: `UPPER_SNAKE_CASE` (SERVER_TIERS, LOG_LEVELS)
- Variables: `camelCase` (serverName, healthCheckInterval)
- Private methods: `_prefixUnderscore` (_spawnProcess, _getServerConfig)
- Config keys: `camelCase` (workspaceRoot, maxRestarts)

**C#**:
- Classes: `PascalCase` (CatgirlController, InventorySystem)
- Public fields: `PascalCase` (MaxHealth, MoveSpeed)
- Private fields: `camelCase` (currentHealth, moveSpeed)
- Animator parameters: cached `StringToHash` (Speed, IsJumping)

### Logging Standards

**Never use `console.log`** - ALWAYS use structured logger:

```javascript
// ❌ Bad
console.log('Server started');

// ✅ Good
logger.info('Server started', { serverName, pid });
```

**Log Levels**:
- `debug` 🔍 - Detailed debugging (development only)
- `info` 📘 - General information (normal operation)
- `success` ✅ - Operation succeeded (confirmation)
- `warn` ⚠️ - Warning, non-fatal issue (potential problems)
- `error` ❌ - Recoverable error (logged but handled)
- `critical` 🔥 - Unrecoverable failure (requires intervention)

**Always include context**:
```javascript
// ❌ Bad (no context)
logger.error('Failed to start');

// ✅ Good (with context)
logger.error('Failed to start server', {
    serverName: 'filesystem',
    error: error.message,
    attempt: 3,
    maxAttempts: 3
});
```

---

## Debugging & Troubleshooting

### Unity Gaming Services Failures

**Symptom**: `EconomyService` fails with "Not authenticated" error

**Cause**: Authentication not initialized before Economy service

**Solution**:
```csharp
// WRONG ORDER ❌
await UnityServices.InitializeAsync();
await EconomyService.Instance.PlayerBalances.GetBalancesAsync(); // Fails!

// CORRECT ORDER ✅
await UnityServices.InitializeAsync();
await AuthenticationService.Instance.SignInAnonymouslyAsync();
await EconomyService.Instance.PlayerBalances.GetBalancesAsync(); // Works!
```

### NetworkBehaviour Ownership Issues

**Symptom**: `NetworkVariable` changes not syncing

**Cause**: Modifying NetworkVariable without `IsOwner` check

**Solution**:
```csharp
// WRONG ❌
void Update() {
    networkHealth.Value = currentHealth; // Fails on non-owner clients!
}

// CORRECT ✅
void Update() {
    if (IsOwner) {
        UpdateHealthServerRpc(currentHealth);
    }
}

[ServerRpc]
void UpdateHealthServerRpc(float health) {
    networkHealth.Value = health; // Server authoritative
}
```

### IPC Message Buffer Issues

**Symptom**: JSON parse errors in Unity bridge

**Cause**: Partial messages across buffer boundaries

**Solution**:
```javascript
// WRONG ❌
this.process.stdout.on('data', (data) => {
    const message = JSON.parse(data.toString()); // Fails on partial data!
});

// CORRECT ✅
_handleStdout(data) {
    this.messageBuffer += data.toString();
    const lines = this.messageBuffer.split('\n');
    
    // Process complete lines only
    for (let i = 0; i < lines.length - 1; i++) {
        const message = JSON.parse(lines[i]);
        this.emit('unity:message', message);
    }
    
    // Keep incomplete line in buffer
    this.messageBuffer = lines[lines.length - 1];
}
```

### MCP Server Startup Failures

**Symptom**: Layer 2 servers fail to start

**Cause**: Starting servers out of tier order

**Solution**:
```javascript
// WRONG ❌
await Promise.all([
    orchestrator.start('filesystem'),
    orchestrator.start('sequential-thinking') // Depends on filesystem!
]);

// CORRECT ✅
// Start Layer 0 first
await orchestrator.start('filesystem');
await orchestrator.start('memory');

// Then Layer 1
await orchestrator.start('git');

// Finally Layer 2
await orchestrator.start('sequential-thinking');
```

### Unity Project Corruption Recovery

```bash
# Clean build artifacts
rm -rf catgirl-avatar-project/Library
rm -rf catgirl-avatar-project/Temp
rm -rf catgirl-avatar-project/obj

# Reopen in Unity Editor to regenerate
```

### Memory Leaks in EventEmitter

**Symptom**: Node.js process memory grows unbounded

**Cause**: EventEmitter listeners not removed

**Solution**:
```javascript
// WRONG ❌
orchestrator.on('server:started', (data) => {
    // Handler registered but never removed
});

// CORRECT ✅
class MyComponent {
    constructor(orchestrator) {
        this.orchestrator = orchestrator;
        this.onServerStarted = this.onServerStarted.bind(this);
        this.orchestrator.on('server:started', this.onServerStarted);
    }
    
    destroy() {
        // MUST remove listener to prevent leak
        this.orchestrator.removeListener('server:started', this.onServerStarted);
    }
    
    onServerStarted(data) {
        // Handle event
    }
}
```

---

## Quick Reference

### Essential Commands

```bash
# Unity Development
npm test                              # Jest with 80% coverage
npm run test:watch                    # TDD mode
./scripts/setup-unity-debug.sh        # Configure VS Code debugging
./scripts/mcp-validate.sh             # Validate 8 MCP servers

# Control Tower
npm run dev                           # Start Control Tower (dev mode)
npm run orchestrator:start            # Start all MCP servers (tiered)
npm run orchestrator:status           # Show server status
npm run orchestrator:health           # Run health checks

# Build & Deploy
npm run container:build               # Docker build
npm run lint                          # Check code style
npm run format                        # Format code
```

### Key File Locations

**Unity C# Systems**:
```
catgirl-avatar-project/Assets/Scripts/
├── Audio/AudioManager.cs (341 lines)
├── Character/CatgirlController.cs (327 lines)
├── Economy/InventorySystem.cs (270 lines)
├── Economy/UniversalBankingSystem.cs (370 lines)
├── IPC/IPCBridge.cs (542 lines)
├── Networking/CatgirlNetworkManager.cs (323 lines)
└── UI/InventoryUI.cs (321 lines)
```

**Node.js Systems**:
```
src/
├── index.js (644 lines)               # Control Tower app
├── mcp/
│   ├── orchestrator.js (823 lines)   # MCP server lifecycle
│   └── agent-coordinator.js (633)    # Multi-agent orchestration
├── unity/
│   └── unity-bridge.js               # IPC bridge
└── utils/
    ├── logger.js (358 lines)         # Structured logging
    └── config.js (505 lines)         # Configuration
```

**Tests**:
```
__tests__/
├── unity-bridge.test.js (183 lines)
├── config.test.js
└── server.test.js
```

**Documentation**:
```
docs/
├── architecture/
│   ├── CATGIRL.md (683 lines)
│   ├── UNITY_IPC_PROTOCOL.md (432 lines)
│   └── RELIGULOUS_MANTRA.md (113 lines)
├── development/
│   └── UNITY_SETUP_GUIDE.md (859 lines)
└── DEBUGGING.md (558 lines)
```

### State Machine Reference

**ServerState** (6 states):
```
STOPPED → STARTING → RUNNING → STOPPING → STOPPED
             ↓           ↓
           ERROR ← RESTARTING
```

**AgentState** (7 states):
```
DISCOVERED → INITIALIZING → IDLE → WORKING → IDLE
                             ↓         ↓
                           BLOCKED   ERROR
                             ↓         ↓
                       DISCONNECTED ← ─┘
```

### MCP Server Tiers

**Layer 0 (Primitives)**:
- filesystem - File system access
- memory - In-memory key-value storage

**Layer 1 (Foundation)**:
- git - Git repository operations
- github - GitHub API integration
- brave-search - Web search capabilities

**Layer 2 (Advanced)**:
- sequential-thinking - Multi-step reasoning
- postgres - PostgreSQL database access
- everything - Test server with all capabilities

### Unity Packages (Top Priority)

1. `com.unity.netcode.gameobjects` 2.0.0 - Multiplayer framework
2. `com.unity.services.economy` 3.4.2 - Currency/inventory cloud storage
3. `com.unity.services.authentication` 3.3.4 - Player identity
4. `com.unity.ui.toolkit` 2.0.0 - Modern UI system
5. `com.unity.xr.interaction.toolkit` 3.0.5 - XR support

### Configuration Priority Order

**Node.js** (highest to lowest):
1. Runtime overrides (programmatic)
2. Environment variables (.env)
3. Workspace file (.code-workspace)
4. Defaults (config.js)

**Unity** (C# PlayerPrefs):
1. Runtime modifications
2. PlayerPrefs (persistent)
3. Inspector values (SerializeField)
4. Code defaults

### Emergency Commands

```bash
# Kill all Node.js processes
pkill -f node

# Kill specific Unity instance
pkill -f "Unity.*catgirl-avatar-project"

# Force stop all MCP servers
npm run orchestrator:stop

# Clean Unity cache
rm -rf catgirl-avatar-project/{Library,Temp,obj}

# Reset MCP state
rm .mcp/state.json

# Clear logs
rm -rf .mcp/logs/*
```

---

## Learning Path for AI Agents

### 1. Immediate Productivity (< 1 hour)

**Read these first**:
- This document (UNIFIED_CATGIRL_KNOWLEDGE_BASE.md)
- `.github/copilot-instructions.md` (in each project)
- `docs/architecture/RELIGULOUS_MANTRA.md` (emoji conventions)

**Then explore**:
- Existing C# scripts in `Assets/Scripts/{domain}/`
- Tests in `__tests__/`
- Run `npm test` to verify environment

### 2. Deep Dive (1-3 hours)

**Architecture understanding**:
- `docs/architecture/CATGIRL.md` (683 lines)
- `docs/architecture/UNITY_IPC_PROTOCOL.md` (432 lines)
- Control Tower: `.github/docs/codebase/core/architecture.md`

**Implementation patterns**:
- `docs/development/UNITY_SETUP_GUIDE.md` (859 lines)
- Control Tower: `.github/docs/codebase/implementation/patterns.md`
- `docs/DEBUGGING.md` (558 lines)

### 3. Mastery (3+ hours)

**Complete system knowledge**:
- Read all 7 Unity C# system implementations
- Study test patterns in `__tests__/`
- Review Control Tower modules documentation
- Understand commentomancy system
- Explore MCP server configurations

**Then start contributing**:
- Write tests first (TDD)
- Follow namespace patterns
- Use commentomancy sigils
- Maintain 80% coverage
- Submit with emoji commits

---

## Summary

This unified knowledge base consolidates:
- **Unity 6.2 C# Systems** (2,491 lines, 7 systems)
- **Node.js IPC Bridge** (JSON stdin/stdout protocol)
- **MCP Control Tower** (2,963 lines, event-driven architecture)
- **Testing & Quality** (Jest 80% coverage threshold)
- **Development Workflows** (VS Code debugging, npm scripts)
- **Critical Conventions** (namespaces, emojis, commentomancy)
- **Debugging Patterns** (common issues & solutions)

**Key Takeaways**:
1. Follow namespace pattern: `BambiSleep.CatGirl.{Domain}` (C#)
2. Use emoji commit prefixes (🦋, ✨, 🐛)
3. Maintain 80% test coverage (build fails below)
4. Initialize UGS in order: Core → Auth → Service
5. Start MCP servers in tiers: Layer 0 → Layer 1 → Layer 2
6. Use structured logging (never console.log)
7. Document with commentomancy sigils (///, //<3, //!, //->)
8. Test IPC with buffer-aware message parsing

**Production-Ready Status**: ✅
- 7 complete Unity C# systems (2,491 lines)
- Node.js IPC bridge with comprehensive tests
- MCP Control Tower with 3-layer architecture
- Full documentation (12+ files, ~15,000 lines)
- 80% test coverage enforced

🌸 **Welcome to the BambiSleep™ Church! Happy coding!** 🌸
