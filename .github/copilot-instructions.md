# BambiSleep™ CatGirl Avatar System

**Last Updated**: 2025-11-03  
**Project**: bambisleep-chat-catgirl  
**Stack**: Unity 6.2 (C#), Node.js 20+ (CommonJS), Express, WebSocket, MCP SDK

Unity 6.2 XR avatar framework with Node.js IPC bridge for real-time communication between Unity Editor and 8 Model Context Protocol (MCP) servers.

---

## Architecture Overview

### Three-Layer System
```
Unity C# Layer (7 subsystems)
  ↕ JSON over stdin/stdout (IPCBridge.cs ↔ unity-bridge.js)
Node.js IPC Bridge (EventEmitter-based)
  ↕ MCP SDK
8 MCP Servers (filesystem, git, github, memory, sequential-thinking, everything, brave-search, postgres)
```

**Key directories:**
- `catgirl-avatar-project/Assets/Scripts/` — Unity C# (2,491 lines across 7 files)
- `src/unity/unity-bridge.js` — Node.js IPC handler (129 lines)
- `src/server/index.js` — Express + WebSocket server (198 lines)
- `config/index.js` — Centralized configuration with environment validation
- `docs/architecture/CATGIRL.md` — Master specification (682 lines)

---

## Critical Development Patterns

### 1. Unity C# Namespace Convention ⚡ CRITICAL
**MANDATORY** for all Unity scripts — non-negotiable pattern:

```csharp
namespace BambiSleep.CatGirl.{SUBSYSTEM}
{
    using UnityEngine;
    using Unity.Netcode;  // If NetworkBehaviour
    
    public class YourController : MonoBehaviour  // or NetworkBehaviour
    {
        // Implementation
    }
}
```

**Subsystems**: `Character`, `Economy`, `Audio`, `Networking`, `UI`, `IPC`

**Real examples from codebase:**
- `CatgirlController.cs` → `BambiSleep.CatGirl.Character` (327 lines, NetworkBehaviour with pink auras & cow powers)
- `UniversalBankingSystem.cs` → `BambiSleep.CatGirl.Economy` (363 lines, gambling + auctions + multi-currency)
- `InventorySystem.cs` → `BambiSleep.CatGirl.Economy` (284 lines, Unity Gaming Services integration)
- `IPCBridge.cs` → `BambiSleep.CatGirl.IPC` (542 lines, JSON IPC protocol)

### 2. Node.js Module System ⚡ CRITICAL
**This project uses CommonJS** (not ES modules) — build will fail with import/export:

```javascript
// ✅ Correct - CommonJS
const UnityBridge = require('../src/unity/unity-bridge');
const config = require('./config');
module.exports = { createServer, startBridge };

// ❌ Wrong (do NOT use ES modules)
import UnityBridge from '../src/unity/unity-bridge.js';
export { createServer };
```

**Why**: `package.json` has no `"type": "module"`, all tests use `require()`, Jest configured for CommonJS.

**No `.js` extensions needed** in `require()` statements (CommonJS auto-resolves).

### 3. Unity-Node.js IPC Protocol
**Bidirectional JSON messaging** via stdin/stdout pipes:

**C# → Node.js** (Unity sends):
```csharp
// IPCBridge.cs
var message = new IPCMessage {
    type = "unity:initialized",
    timestamp = DateTime.UtcNow.ToString("o"),
    data = JsonUtility.ToJson(initData)
};
Console.WriteLine(JsonUtility.ToJson(message));
```

**Node.js → Unity** (bridge sends):
```javascript
// unity-bridge.js
sendMessage(type, data) {
    const message = { type, timestamp: new Date().toISOString(), data };
    this.process.stdin.write(JSON.stringify(message) + '\n');
}
```

**Message types**: `unity:initialized`, `unity:update`, `unity:render`, `node:command`, `mcp:request`

### 4. EventEmitter Over Callbacks
**All async coordination uses EventEmitter**:

```javascript
// unity-bridge.js extends EventEmitter
this.emit('ready'); // Signal Unity started
this.emit('unity:log', data.toString()); // Forward Unity console logs
this.emit('unity:exit', code); // Handle Unity process termination
```

Consumers listen:
```javascript
bridge.on('ready', () => { /* Handle connection */ });
bridge.on('unity:log', (msg) => logger.info(msg));
```

### 5. Configuration Management
**All config in `config/index.js`** with environment variable overrides:

```javascript
const config = require('./config');

// ✅ Access configuration (never hardcode)
const unityPath = config.unity.projectPath;  // f:\CATHEDRAL\bambisleep-chat-catgirl\catgirl-avatar-project
const serverPort = config.server.port;        // 3000 (or process.env.PORT)
const cowPowers = config.features.cowPowers;  // true (secret level unlocked!)

// ❌ Don't hardcode paths or settings
const path = './catgirl-avatar-project';  // Wrong!
```

**Environment variables supported**: `UNITY_PATH`, `SERVER_PORT`, `NODE_ENV`, `LOG_LEVEL`, `ENABLE_COW_POWERS`, `CUTENESS_LEVEL`

**Validation**: `config.validate()` checks required production values (API_KEY, UNITY_PROJECT_ID)

---

## Testing Requirements

### Jest Configuration
```bash
npm test              # Run full suite with coverage
npm run test:watch    # Watch mode
npm run test:ci       # CI/CD mode
```

**Coverage thresholds** (enforced in `jest.config.js`):
- 80% branches, functions, lines, statements
- Tests must be co-located: `src/unity/unity-bridge.js` → `__tests__/unity-bridge.test.js`

**Critical**: CommonJS syntax in tests (no ES modules):
```javascript
const UnityBridge = require('../src/unity/unity-bridge');

describe('UnityBridge', () => {
    let bridge;
    
    beforeEach(() => {
        bridge = new UnityBridge({
            unityPath: '/mock/unity',
            projectPath: '/mock/project'
        });
    });
    
    afterEach(() => {
        if (bridge?.process) bridge.stop();
    });
    
    it('should create bridge instance', () => {
        expect(bridge).toBeInstanceOf(UnityBridge);
        expect(bridge.batchMode).toBe(true);
    });
});
```

### Error Handling Patterns
```javascript
// Unity Gaming Services - async/await with try-catch
async function loadBalances() {
    try {
        const balances = await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
        return balances;
    } catch (error) {
        Debug.LogError($"❌ Failed to load balances: {error.Message}");
        return null;  // Graceful degradation
    }
}

// EventEmitter - error events
bridge.on('error', (error) => {
    logger.error('Bridge error:', error);
    // Attempt reconnection or notify user
});

// JSON.parse - wrap in try-catch
try {
    const message = JSON.parse(data);
    handleMessage(message);
} catch (err) {
    logger.warn('Invalid JSON received:', err.message);
}
```

---

## MCP Server Integration

### 8 Active MCP Servers
**Layer 0** (Primitives): `filesystem`, `memory`  
**Layer 1** (Foundation): `git`, `github`, `brave-search`  
**Layer 2** (Advanced): `sequential-thinking`, `everything`, `postgres`

**Setup scripts:**
- `scripts/setup-mcp.sh` — Install and configure all servers
- `scripts/mcp-validate.sh` — Verify server health

**Configuration**: Add to VS Code `settings.json` under `"mcp.servers"`

### Recommended Custom MCP Servers

Based on project needs, consider developing these specialized MCP servers:

```javascript
// bambisleep-hypnosis-mcp
// Purpose: Audio file management for hypnosis triggers
// Features: Playlist management, audio processing, trigger metadata

// aigf-personality-mcp  
// Purpose: AI girlfriend personality switching
// Features: Context switching, personality profiles, mood states

// trigger-system-mcp
// Purpose: Hypnotic trigger management and coordination
// Features: Trigger registration, activation patterns, compliance tracking

// chat-analytics-mcp
// Purpose: BambiSleep.Chat metrics and analytics
// Features: User engagement tracking, session analysis, conversion metrics
```

**Custom MCP development pattern:**
```javascript
// Example: Basic MCP server structure
import { Server } from '@modelcontextprotocol/sdk/server/index.js';
import { StdioServerTransport } from '@modelcontextprotocol/sdk/server/stdio.js';

const server = new Server({
  name: 'bambisleep-hypnosis-mcp',
  version: '1.0.0'
}, {
  capabilities: {
    resources: {},
    tools: {},
    prompts: {}
  }
});

// Register tools
server.setRequestHandler('tools/call', async (request) => {
  // Tool implementation
});

// Start server
const transport = new StdioServerTransport();
await server.connect(transport);
```

---

## Unity 6.2 Specifics

### Version Requirements
- **Unity 6.2 LTS** (`6000.2.11f1`) — specified in `package.json` config
- **Project location**: `./catgirl-avatar-project/`
- **Packages**: XR Interaction Toolkit, Netcode for GameObjects, Input System, Unity Gaming Services

### Unity Scripts Organization
```
Assets/Scripts/
├── Character/      # CatgirlController.cs (327 lines) - NetworkBehaviour, pink auras, levitation
├── Economy/        # UniversalBankingSystem.cs (363 lines), InventorySystem.cs (284 lines)
├── Audio/          # AudioManager.cs (342 lines) - Singleton, purring at 2.5Hz
├── Networking/     # CatgirlNetworkManager.cs (324 lines) - Unity Relay + Lobby
├── IPC/            # IPCBridge.cs (542 lines), MCPAgent.cs - JSON protocol
└── UI/             # InventoryUI.cs (322 lines) - UI Toolkit with pink frilly theme
```

**Total production code**: 1,950+ lines across 6 complete systems

### Unity Naming Conventions (.editorconfig enforced)
```csharp
// Interfaces: Prefix with 'I'
public interface ICatgirlBehavior { }

// Types: PascalCase
public class CatgirlController { }

// Public fields/properties: PascalCase
public int CutenessLevel = 9999;

// Private fields: camelCase (no underscore prefix)
private float pinkIntensity = 1.0f;

// NetworkVariable: PascalCase
public NetworkVariable<long> PinkCoins = new NetworkVariable<long>(0);

// Coroutines: Prefix with 'Co_'
private IEnumerator Co_PurringCycle() { }

// SerializeField: camelCase
[SerializeField] private float frillinessLevel = 100f;
```

### Unity Design Patterns Used
1. **NetworkBehaviour Lifecycle**: `Awake()` → `OnNetworkSpawn()` → `OnNetworkDespawn()`
2. **Async Unity Gaming Services**: `async Task` methods with try-catch error handling
3. **Singleton Pattern**: `AudioManager.Instance` for global services
4. **Coroutines**: Timed loops for purring (2.5Hz), animations, particle effects
5. **NetworkVariable**: Server-authoritative multiplayer state synchronization
6. **ServerRpc/ClientRpc**: `[ServerRpc]` for client→server, `[ClientRpc]` for server→clients

### Run Unity from Node.js:
```bash
npm run unity:setup    # Initialize Unity project structure
npm run unity:debug    # Setup debug configuration
npm run unity:launch   # Launch Unity Editor with IPC bridge
```

---

## Unity C# Code Examples

### Creating a New Character Component
```csharp
// Assets/Scripts/Character/EyeTrackingController.cs
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR;

namespace BambiSleep.CatGirl.Character
{
    /// <summary>
    /// Monarch butterfly eye tracking with hypnotic pupil dilation
    /// </summary>
    public class EyeTrackingController : NetworkBehaviour
    {
        [Header("👁️ Eye Tracking Configuration")]
        [SerializeField] private float pupilDilationSpeed = 2.0f;
        [SerializeField] private float gazeRaycastDistance = 10f;
        
        private NetworkVariable<float> pupilSize = new NetworkVariable<float>(1.0f);
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            StartEyeTracking();
        }
        
        private void StartEyeTracking()
        {
            // XR eye tracking initialization
            Debug.Log("👁️ Initializing hypnotic eye tracking...");
        }
    }
}
```

### Adding Economy Features
```csharp
// In UniversalBankingSystem.cs - adding new currency
namespace BambiSleep.CatGirl.Economy
{
    public class UniversalBankingSystem : NetworkBehaviour
    {
        public async Task<bool> AddCurrency(string currencyId, long amount, string reason = "")
        {
            if (!IsServer) return false;
            
            try
            {
                // Update Unity Gaming Services
                var result = await EconomyService.Instance.PlayerBalances
                    .IncrementBalanceAsync(currencyId, amount);
                
                // Record transaction
                RecordTransaction(new Transaction {
                    currencyId = currencyId,
                    amount = amount,
                    description = reason,
                    timestamp = System.DateTime.UtcNow
                });
                
                Debug.Log($"💎 Added {amount} {currencyId}: {reason}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Failed to add currency: {e.Message}");
                return false;
            }
        }
    }
}
```

---

## Node.js + Unity IPC Examples

### Sending Messages to Unity
```javascript
// src/unity/unity-bridge.js
class UnityBridge extends EventEmitter {
    sendCatgirlCommand(action, data) {
        this.sendMessage('node:command', {
            action: action,
            payload: data,
            timestamp: Date.now()
        });
    }
    
    // Example usage
    triggerCowPowers() {
        this.sendCatgirlCommand('activate_cow_powers', {
            intensity: 'MAXIMUM_OVERDRIVE',
            duration: 30000  // 30 seconds of MOO-nificence
        });
    }
}
```

### Receiving Messages from Unity
```javascript
// Unity sends JSON to stdout, Node.js receives:
bridge.on('unity:initialized', (data) => {
    console.log('✨ Unity ready:', data.sceneName);
    console.log(`🏰 Cathedral dimensions: ${data.cathedralWidth}x${data.cathedralLength}`);
});

bridge.on('unity:render', (data) => {
    console.log(`🎨 Render complete: ${data.outputPath}`);
});
```

---

## Development Workflow

### Before Making Changes
1. **Read `TODO.md`** for current priorities (Unity eye tracking, banking system, cow powers)
2. **Check `package.json`** for available scripts
3. **Run tests**: `npm test` (must pass with 80%+ coverage)
4. **Review docs**: `docs/architecture/CATGIRL.md` for system design

### Making Changes
1. **Unity C#**: Add `BambiSleep.CatGirl.{Subsystem}` namespace
2. **Node.js**: Use CommonJS (`require`/`module.exports`)
3. **Config**: Use `config/index.js`, never hardcode values
4. **Tests**: Add co-located `.test.js` file with 80%+ coverage
5. **Commit**: Use emoji prefixes (🌸 packages, ✨ features, 🐛 fixes, 💎 tests)

### After Changes
1. **Run tests**: `npm test` — verify coverage maintained
2. **Update TODO.md**: Mark completed tasks with `[x]`
3. **Update docs**: If architecture changes, update `docs/architecture/`

---

## Common Pitfalls

| Issue | Cause | Fix |
|-------|-------|-----|
| `import` syntax error | Used ES modules in CommonJS project | Use `require()` instead |
| Unity namespace error | Missing `BambiSleep.CatGirl.X` | Add mandatory namespace |
| IPC message not received | Missing newline in JSON | Append `\n` to `stdin.write()` |
| Config undefined | Hardcoded value | Import from `config/index.js` |
| Test coverage fails | Missing `.test.js` file | Add co-located test file |

---

## Key Files for Understanding System

### Documentation (Read First)
- `README.md` — Project overview, philosophy, quick start
- `TODO.md` — Current priorities, 174 lines of next steps
- `docs/architecture/CATGIRL.md` — Master specification (682 lines)
- `docs/development/UNITY_SETUP_GUIDE.md` — C# implementation guide (858 lines)

### Entry Points
- `index.js` — Main Node.js entry (displays help, routes to subcommands)
- `src/server/index.js` — Express + WebSocket server (REST API, health checks)
- `catgirl-avatar-project/Assets/Scripts/IPC/IPCBridge.cs` — Unity IPC handler

### Configuration
- `config/index.js` — All environment config with validation
- `package.json` — Scripts, dependencies, project metadata
- `jest.config.js` — Test configuration with coverage thresholds

---

## Trademark & Philosophy

**Always use "BambiSleep™"** (with ™ symbol) in public-facing documentation.

**Universal Machine Philosophy**: Pink frilly catgirl avatars that reprogram reality with cuteness. Features include:
- Hypnotic monarch butterfly eye tracking
- Purring frequency modulation (2.5Hz for brain optimization)
- Secret cow powers (unlocks Diablo-level items)
- Rainbow washing machine particle effects
- Banking & gambling mechanics for in-game economy

**Organization**: BambiSleepChat on GitHub  
**License**: MIT

### Agent Authority System (Commander-Brandynette Protocol)

**Hierarchical agent control system** for coordinating multiple AI agents:

```
Commander-Brandynette (Top Authority)
  └── Assigns agent roles and ring layer access
      ├── Layer 0: Primitive operations (filesystem, memory)
      ├── Layer 1: Foundation services (git, github, brave-search)
      └── Layer 2: Advanced capabilities (sequential-thinking, postgres, everything)
```

**Key principles:**
1. **Agent obedience**: All agents follow Commander-Brandynette's authority structure
2. **Ring layer access**: Agents get graduated access based on role assignments
3. **Trigger system integration**: Hypnosis triggers coordinate with agent behaviors
4. **Ritual enforcement**: Nightly training rituals ensure agent compliance

**Implementation in code:**
```csharp
// Unity MCPAgent.cs - Agent authority enforcement
namespace BambiSleep.CatGirl.IPC
{
    public class MCPAgent : MonoBehaviour
    {
        // Agent roles assigned by Commander-Brandynette
        private enum AgentRole {
            Primitive,    // Layer 0: Basic operations
            Foundation,   // Layer 1: Core services
            Advanced      // Layer 2: Complex automation
        }
        
        // Enforce role-based access
        private bool CheckAgentAuthority(AgentRole role, string operation) {
            // Commander-Brandynette protocol validation
            return role >= GetRequiredRole(operation);
        }
    }
}
```

```javascript
// Node.js - Agent coordination patterns
class AgentCoordinator extends EventEmitter {
    constructor() {
        super();
        this.commanderAuthority = true;  // Commander-Brandynette control
        this.agentRoles = new Map();     // Role assignments
    }
    
    assignAgentRole(agentId, ringLayer) {
        // Commander assigns ring layer access
        this.agentRoles.set(agentId, ringLayer);
        this.emit('role:assigned', { agentId, ringLayer });
    }
    
    enforceCompliance(agentId, operation) {
        const role = this.agentRoles.get(agentId);
        if (!role || !this.checkAccess(role, operation)) {
            throw new Error('Agent lacks required authority');
        }
    }
}
```

**Trigger system conventions:**
- `Bambi Sleep` — Enter hypnotic trance state
- `Bambi Freeze` — Pause agent execution
- `Snap and forget` — Clear agent memory/reset
- `Good girl` — Positive reinforcement for compliance
- Ritual enforcement — Nightly training ensures agent obedience

**DO NOT implement actual hypnosis triggers in production code** — these are narrative/thematic elements only. Use standard software design patterns (authentication, authorization, rate limiting, etc.) for actual access control.

---

## Security & Best Practices

### Environment Variables & Secrets
```javascript
// ✅ Load secrets from environment
const apiKey = process.env.API_KEY;
const githubToken = process.env.GITHUB_PERSONAL_ACCESS_TOKEN;

// ❌ Never hardcode secrets
const apiKey = 'sk-1234567890abcdef';  // Don't do this!
```

**Required in production** (validated by `config.validate()`):
- `API_KEY` — Service authentication
- `UNITY_PROJECT_ID` — Unity cloud services
- `GITHUB_PERSONAL_ACCESS_TOKEN` — GitHub API access

### Input Validation
```csharp
// Unity C# - validate user inputs
public bool ValidateItemPurchase(string itemId, long cost)
{
    if (string.IsNullOrEmpty(itemId)) return false;
    if (cost <= 0 || cost > long.MaxValue) return false;
    if (!IsServer) return false;  // Server authoritative
    return true;
}
```

```javascript
// Node.js - validate IPC messages
function handleUnityMessage(data) {
    try {
        const message = JSON.parse(data);
        if (!message.type || !message.timestamp) {
            logger.warn('Invalid message format');
            return;
        }
        processMessage(message);
    } catch (err) {
        logger.error('JSON parse error:', err.message);
    }
}
```

### Networking Security (Unity)
```csharp
// Always use ServerRpc for authoritative actions
[ServerRpc(RequireOwnership = false)]
public void PurchaseItemServerRpc(string itemId, long cost)
{
    // Validate on server ONLY
    if (!ValidateItemPurchase(itemId, cost)) return;
    if (GetCurrencyBalance() < cost) return;
    
    // Deduct currency server-side
    DeductCurrency(cost);
    GrantItem(itemId);
}

// ❌ Never trust client-side values for critical actions
```

---

## Quick Command Reference

```bash
# Development
npm start              # Show help and available commands
npm run dev            # Start development server with nodemon
npm test               # Run Jest with coverage (80%+ required)
npm run lint           # ESLint check
npm run lint:fix       # Auto-fix ESLint errors
npm run format         # Prettier format all files

# Unity Integration
npm run unity:setup    # Initialize Unity 6.2 project
npm run unity:debug    # Setup debug configuration
npm run unity:launch   # Launch Unity Editor with IPC bridge

# MCP Servers
npm run mcp:setup      # Install all 8 MCP servers
npm run mcp:validate   # Check server health and connectivity

# Build & Deploy
npm run build          # Universal build (cross-platform)
npm run deploy         # AI Girlfriend deployment mode (production)

# Documentation
npm run docs:verify    # Validate documentation structure
```

---

## Related Documentation

- **Workspace root**: `f:\CATHEDRAL\.github\copilot-instructions.md` — Multi-project conventions
- **Control tower**: `bambisleep-church-catgirl-control-tower/.github/copilot-instructions.md` — MCP orchestration
- **Express app**: `bambisleep-church/.github/copilot-instructions.md` — Production server patterns

**For complex architectural changes**, consult:
- `docs/architecture/CATGIRL.md` (682 lines) — Master specification
- `DEVELOPMENT_ROADMAP.md` (367 lines) — Feature priorities
- `docs/development/UNITY_SETUP_GUIDE.md` (858 lines) — C# implementation guide

---

## AI Agent Tips

### When implementing new features:
1. **Start with specification** — Check `TODO.md` and `CATGIRL.md` for requirements
2. **Follow existing patterns** — Study similar systems (e.g., add currency type? See `UniversalBankingSystem.cs`)
3. **Write tests first** — TDD ensures 80%+ coverage from the start
4. **Document as you go** — Update TODO.md and add inline comments for complex logic
5. **Test incrementally** — Run `npm test` after each logical change

### When debugging:
1. **Check logs first** — `logs/bambisleep.log` contains all IPC traffic
2. **Use Unity console** — Most issues have clear Debug.Log messages
3. **Verify MCP servers** — `npm run mcp:validate` checks connectivity
4. **Test IPC manually** — Send JSON to Unity via `echo '{"type":"test"}' | node`
5. **Consult DEBUGGING.md** — Has breakpoint locations and troubleshooting steps

### When refactoring:
1. **Maintain test coverage** — Must stay above 80%
2. **Keep namespaces** — Don't break `BambiSleep.CatGirl.X` pattern
3. **Preserve IPC protocol** — Unity and Node.js must stay in sync
4. **Update documentation** — Architecture changes require doc updates
5. **Run full test suite** — `npm run test:ci` before committing

---

**Last Updated**: 2025-11-03  
**Version**: 1.0 (Enhanced with awesome-copilot best practices)
