## Project Overview

This is a **production Unity 6.2 LTS project** combining:

- **Unity C# Systems** (2,491 lines): Avatar controller, economy, multiplayer
  networking, UI, audio, IPC bridge
- **Node.js ↔ Unity IPC Bridge**: JSON-based stdin/stdout communication
  protocol (see `src/unity/unity-bridge.js`)
- **MCP Agent Tooling** (8 core servers + 6 optional): Development automation
  via Model Context Protocol
- **Jest Test Suite**: Real test assertions in `__tests__/` (not stubs) -
  coverage target 80%
- **Trademark Requirement**: Always use `BambiSleep™` (with ™) in
  public-facing content
- **Documentation as Code**: Markdown files contain canonical implementations to
  copy verbatim

### Quick Start: 5-Minute Productivity Guide

**Workflow Decision Tree** (choose your path):

```
Your Task → Quick Guide
├─ Extend Unity C# systems
│  └─ 1. Read existing script in catgirl-avatar-project/Assets/Scripts/{domain}/
│     2. Copy namespace pattern: BambiSleep.CatGirl.{Domain}
│     3. Follow NetworkBehaviour lifecycle if multiplayer
│     4. Add emoji headers: [Header("🌸 Section Name")]
│     5. Run "Check Unity Version" task to verify Unity 6000.2.11f1
│
├─ Add Node.js feature (IPC bridge)
│  └─ 1. Review src/unity/unity-bridge.js for IPC patterns
│     2. Add corresponding test in __tests__/unity-bridge.test.js
│     3. Follow EventEmitter pattern for async communication
│     4. Run `npm test` to verify 80% coverage maintained
│
├─ Debug Unity Gaming Services
│  └─ 1. Check docs/DEBUGGING.md for Unity-specific troubleshooting
│     2. Verify UGS initialization order: UnityServices → Auth → Economy
│     3. Check Unity Dashboard for project ID and Economy config
│     4. Use [ContextMenu] test methods for isolated debugging
│
├─ Use MCP servers for automation
│  └─ 1. Run ./scripts/mcp-validate.sh to verify 8/8 servers operational
│     2. Use filesystem MCP for creating Unity scripts with proper structure
│     3. Use git MCP for commits with emoji conventions (🦋 for features)
│     4. Use github MCP for creating issues linked to specific code lines
│
└─ Build/Deploy changes
   └─ 1. Update package.json version for semantic versioning
      2. Run `npm test` to ensure tests pass
      3. Use VS Code task "Build Container" for Docker builds
      4. CI/CD auto-deploys on git tag: v{major}.{minor}.{patch}
```

**For extending Unity systems** (most common task):

1. Read the existing script FIRST:
   `catgirl-avatar-project/Assets/Scripts/{domain}/{ClassName}.cs`
2. Follow these exact patterns: proper namespace, emoji headers, NetworkBehaviour
   lifecycle
3. Test with VS Code task "Check Unity Version" to verify Unity 6000.2.11f1
4. Run `npm test` to ensure Node.js integration still works

**For debugging issues:**

- Unity errors: `docs/DEBUGGING.md` (522 lines with Unity-specific troubleshooting)
- UGS auth failures: Check initialization order (UnityServices → Auth →
  Economy)
- Test failures: Review `__tests__/unity-bridge.test.js` for patterns
- MCP issues: Run `./scripts/mcp-validate.sh` (tests 8/8 operational)

### Quick Actions Reference

| Task                     | Command/Location                                                                                |
| ------------------------ | ----------------------------------------------------------------------------------------------- |
| **Extend Unity systems** | Read `catgirl-avatar-project/Assets/Scripts/{domain}/{ClassName}.cs` FIRST, follow its patterns |
| **Run tests**            | `npm test` (Jest with 80% coverage target) or `npm run test:watch`                              |
| **MCP setup**            | Run `./scripts/setup-mcp.sh`, test with `./scripts/mcp-validate.sh`                             |
| **Build/Deploy**         | VS Code tasks (Ctrl+Shift+P → "Tasks: Run Task") or npm scripts                                 |
| **Architecture guide**   | `docs/development/UNITY_SETUP_GUIDE.md` (858 lines with actual C# code)                         |
| **Debug guide**          | `docs/DEBUGGING.md` (522 lines - breakpoints, shortcuts, troubleshooting)                       |

## Project Culture & Conventions

**This project embraces a playful, maximalist aesthetic** inspired by "pink
frilly platinum blonde" themes, cow powers, and Universal Machine Philosophy.
This is NOT typical enterprise code.

### Emoji Conventions (from `RELIGULOUS_MANTRA.md`)

- 🌸 **Cherry Blossom**: Packages, core systems, main features
- 🦋 **Butterfly**: Transformations, state changes, NetworkBehaviour events
- 💎 **Gem**: High-value features, premium systems
- 👑 **Crown**: Authority, enterprise-grade patterns
- 🐄 **Cow**: Secret/easter egg features (Diablo secret level references)
- 🔥 **Fire**: Performance-critical code, hot paths
- ✨ **Sparkles**: UI polish, visual effects, frilly details

**Commit message format**: `🦋 Add feature description` (emoji prefix required)

### Code Organization Principles

1. **Documentation as Code**: `docs/*.md` files contain **actual
   implementations** to copy verbatim
2. **MCP-First Development**: Use 8 MCP servers (filesystem, git, github,
   memory, sequential-thinking, everything, brave-search, postgres) for all
   workflows. Validate with `./scripts/mcp-validate.sh` (tests 8/8 operational)
3. **100% Completion Mindset**: Follow the "10/10 operational" philosophy - no
   half-implemented features
4. **Trademark Discipline**: Always use `BambiSleep™` (with ™) in user-facing
   content

### "Cow Powers" & Secret Features

- References to "cow powers" = easter eggs/hidden features (Diablo secret cow
  level homage)
- Gambling systems must have 5% house edge (see `UniversalBankingSystem.cs:299`)
- Item rarity: Common→Uncommon→Rare→Epic→Legendary→**Divine Cow Crown** (secret
  tier)

## Project Structure

```
bambisleep-chat-catgirl/
├── catgirl-avatar-project/          # Unity 6.2 LTS (Unity 6000.2.11f1)
│   ├── Assets/Scripts/              # 7 complete C# systems (2,491 lines)
│   │   ├── Audio/AudioManager.cs    # 341 lines - Singleton audio system
│   │   ├── Character/CatgirlController.cs  # 326 lines - NetworkBehaviour
│   │   ├── Economy/InventorySystem.cs      # 269 lines - UGS Economy
│   │   ├── Economy/UniversalBankingSystem.cs # 370 lines - Multi-currency
│   │   ├── Networking/CatgirlNetworkManager.cs # 323 lines - Relay + Lobby
│   │   ├── UI/InventoryUI.cs        # 321 lines - UI Toolkit interface
│   │   └── IPC/IPCBridge.cs         # 541 lines - Unity ↔ Node.js IPC
│   │   └── IPC/MCPAgent.cs          # MCP integration (scaffold)
│   ├── Packages/manifest.json       # 16 Unity packages (UGS, Netcode, XR)
│   └── ProjectSettings/ProjectVersion.txt
├── docs/                            # Documentation (consolidated)
│   ├── architecture/                # CATGIRL.md (682), UNITY_IPC_PROTOCOL.md (430)
│   ├── development/                 # UNITY_SETUP_GUIDE.md (858 - READ THIS)
│   ├── guides/                      # build.md, todo.md
│   └── DEBUGGING.md                 # 522 lines - complete debug reference
├── .github/workflows/build.yml      # CI/CD with 7 jobs
├── .vscode/                         # MCP integration + 8 tasks
│   ├── settings.json                # 8 core + 6 optional MCP servers configured
│   └── tasks.json                   # 8 VS Code tasks for common workflows
├── Dockerfile                       # Multi-stage build → GHCR: bambisleepchat/bambisleep-church
├── package.json                     # Node.js 20.19.5 (Volta pinned)
├── jest.config.js                   # Jest config with 80% coverage threshold
├── scripts/
│   ├── setup-mcp.sh                 # MCP server installation
│   └── mcp-validate.sh              # Test all 8 MCP servers
├── src/
│   ├── unity/unity-bridge.js        # 129 lines - Node.js ↔ Unity IPC bridge
│   ├── server/index.js              # Server implementation
│   ├── cli/index.js                 # CLI implementation
│   └── utils/logger.js              # Logging utilities
└── __tests__/                       # Jest test suite (real implementations)
    ├── unity-bridge.test.js         # 183 lines - IPC bridge tests
    ├── config.test.js               # Configuration tests
    └── server.test.js               # Server tests
```

**Essential Documentation** (read in this order):

1. `docs/development/UNITY_SETUP_GUIDE.md` - Complete C# implementations
2. `docs/architecture/CATGIRL.md` - System architecture & Unity packages
3. `docs/architecture/UNITY_IPC_PROTOCOL.md` - Node.js ↔ Unity communication
   protocol
4. `docs/architecture/RELIGULOUS_MANTRA.md` - Emoji conventions & build
   philosophy
5. `docs/guides/todo.md` - Implementation status (complete vs in-progress)
6. `docs/DEBUGGING.md` - Complete debugging reference (breakpoints, shortcuts,
   troubleshooting)

## Critical Unity C# Patterns

**All 7 systems are COMPLETE** (2,491 lines). When extending, follow these
patterns:

### 1. Namespace & Structure

```csharp
// Namespace: BambiSleep.CatGirl.{Domain}
namespace BambiSleep.CatGirl.Character
{
    [Header("🌸 Section Name")]  // Emoji headers for organization
    public class CatgirlController : NetworkBehaviour
    {
        // Component references, network variables, lifecycle methods
    }
}
```

### 2. NetworkBehaviour Lifecycle

```csharp
public override void OnNetworkSpawn()
{
    if (IsOwner)
    {
        InitializeSystems();
        UpdateNetworkStatsServerRpc(localData);
    }
    // Subscribe to NetworkVariable changes
    networkVariable.OnValueChanged += OnValueChanged;
}

public override void OnNetworkDespawn()
{
    networkVariable.OnValueChanged -= OnValueChanged;
}
```

### 3. Unity Gaming Services (Async Pattern)

```csharp
private async void ConnectToService()
{
    try
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        var result = await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
    }
    catch (System.Exception e)
    {
        Debug.LogError($"Service error: {e.Message}");
        // Graceful fallback
    }
}
```

### 4. Animator Performance Pattern

```csharp
// Cache hash IDs (not strings)
private static readonly int Speed = Animator.StringToHash("Speed");
private static readonly int IsPurring = Animator.StringToHash("IsPurring");

animator.SetFloat(Speed, currentSpeed);
animator.SetBool(IsPurring, true);
```

### 5. Singleton Services

```csharp
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

private void Awake()
{
    if (_instance != null && _instance != this)
    {
        Destroy(gameObject);
        return;
    }
    _instance = this;
    DontDestroyOnLoad(gameObject);
}
```

**Key Unity Packages** (from `Packages/manifest.json`):

- `com.unity.services.economy` 3.4.2 (currency system)
- `com.unity.services.authentication` 3.3.4 (player identity)
- `com.unity.netcode.gameobjects` 2.0.0 (multiplayer networking)
- `com.unity.services.lobby` 1.2.2 (matchmaking)
- `com.unity.services.relay` 1.1.3 (NAT traversal)
- `com.unity.xr.interaction.toolkit` 3.0.5 (XR support)
- `com.unity.addressables` 2.3.1 (asset management)
- `com.unity.visualeffectgraph` 16.0.6 (particle systems)
- `com.unity.ui.toolkit` 2.0.0 (modern UI system used by InventoryUI.cs)
- `com.unity.animation.rigging` 1.3.1 (procedural animation)
- `com.unity.cinemachine` 2.10.1 (camera system)
- `com.unity.timeline` 1.8.7 (cutscenes & animation sequences)

### 6. Unity IPC Protocol (Node.js ↔ Unity Communication)

```javascript
// Node.js side (src/unity/unity-bridge.js)
sendMessage(type, data) {
  const message = {
    type,
    timestamp: new Date().toISOString(),
    data
  };
  this.process.stdin.write(JSON.stringify(message) + '\n');
}

// Listen for Unity responses
this.process.stdout.on('data', (data) => {
  const messages = data.toString().split('\n').filter(line => line.trim());
  messages.forEach(line => {
    const message = JSON.parse(line);
    this.handleMessage(message); // Routes to event handlers
  });
});
```

```csharp
// Unity side (Assets/Scripts/IPC/IPCBridge.cs)
void Update() {
    if (Console.KeyAvailable) {
        string line = Console.ReadLine();
        if (!string.IsNullOrEmpty(line)) {
            ProcessMessage(line);
        }
    }
}

void ProcessMessage(string json) {
    IPCMessage message = JsonUtility.FromJson<IPCMessage>(json);
    switch (message.type) {
        case "initialize": InitializeScene(message.data); break;
        case "render": RenderScene(message.data); break;
        case "update": UpdateParameters(message.data); break;
    }
}

void SendMessage(string type, object data) {
    var message = new { type, timestamp = DateTime.UtcNow.ToString("o"), data };
    Console.WriteLine(JsonUtility.ToJson(message));
}
```

**Message Types**: initialize, update, render, camera, postprocessing, shutdown
(Node→Unity); scene-loaded, render-complete, update-ack, error, heartbeat
(Unity→Node)

**See**: `docs/architecture/UNITY_IPC_PROTOCOL.md` for complete protocol
specification

## Development Workflows

### Critical Gotchas & Debugging

**Unity Gaming Services (UGS) Authentication Order**

```csharp
// MUST initialize in this exact order (common failure point):
await UnityServices.InitializeAsync();         // 1. Core services first
await AuthenticationService.Instance.SignInAnonymouslyAsync();  // 2. Auth second
await EconomyService.Instance.PlayerBalances.GetBalancesAsync(); // 3. Economy last
```

**NetworkBehaviour Ownership Rules**

- `IsOwner` checks required before modifying NetworkVariables on client
- Use `[ServerRpc]` for server-authoritative actions (e.g., placing bids,
  spawning items)
- Always unsubscribe from `NetworkVariable.OnValueChanged` in
  `OnNetworkDespawn()`

**Test Framework Status**

- **Jest test framework** with real assertions (NOT stubs) in `__tests__/`
- **Coverage target**: 80% (branches, functions, lines, statements)
- **Test files**: `unity-bridge.test.js` (183 lines), `config.test.js`,
  `server.test.js`
- **Test patterns**: Use `describe/it` blocks, `expect` assertions, proper
  setup/teardown
- **Run with**: `npm test` or `npm run test:watch` (watch mode)
- **Coverage reports**: Output to `coverage/lcov.info` for Codecov integration
- **IMPORTANT**: When implementing features, add corresponding tests in
  `__tests__/` directory following existing patterns

**Jest Test Pattern Example** (from `unity-bridge.test.js`):

```javascript
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

  it('should parse complete JSON messages', (done) => {
    const testMessage = { type: 'scene-loaded', data: { sceneName: 'Test' } };

    bridge.on('unity:scene-loaded', (data) => {
      expect(data).toEqual({ sceneName: 'Test' });
      done();
    });

    bridge._handleStdout(Buffer.from(JSON.stringify(testMessage) + '\n'));
  });
});
```

**When writing new tests**:

- Mock child processes with `jest.fn()` for Unity spawning
- Use EventEmitter patterns for async IPC testing
- Test both success and error paths
- Verify JSON message formatting matches IPC protocol spec

**Unity Project Corruption Recovery**

```bash
# Use VS Code Task: "Clean Unity Project" or run:
rm -rf catgirl-avatar-project/{Library,Temp,obj}
# Then reopen project in Unity Editor to regenerate
```

**IPC Bridge Known TODOs** (in `IPCBridge.cs`)

- Line 303: Cathedral parameter application needs implementation
- Line 330: Real-time cathedral parameter updates incomplete
- Line 436: Post-processing settings application pending
- These are scaffolded for future cathedral rendering features

**Unity Editor Version Requirements**

- **Required**: Unity 6.2 LTS (tested with 6000.2.11f1)
- **Compatible**: Any Unity 6000.2.x patch version (e.g., 6000.2.12f1,
  6000.2.13f1)
- **Not compatible**: Unity 6.3+ or Unity 6000.1.x (different minor/patch
  branches)
- **Verification**: Run VS Code task "Check Unity Version" or check
  `catgirl-avatar-project/ProjectSettings/ProjectVersion.txt`
- **Download**:
  [Unity 6 LTS Download Page](https://unity.com/releases/lts/6000-2)
- Unity 6.2 LTS has multi-year support; patch updates maintain API compatibility
  for NetworkBehaviour, Unity Gaming Services, and XR packages

**Git Tracking & .gitignore Rules**

- Unity `Library/`, `Temp/`, `obj/`, `Builds/`, `Logs/`, `UserSettings/` are
  ignored
- All `.csproj`, `.sln`, `.suo` files ignored (Unity regenerates these)
- `node_modules/` and package lock files ignored
- Docker volumes and container data excluded
- Keep: `Assets/`, `Packages/`, `ProjectSettings/` tracked in git
- **Meta files**: Unity `.meta` files ARE tracked (critical for asset
  references)

### MCP Environment (8 Core + Optional Servers)

**Core MCP Servers (8 validated via `./scripts/mcp-validate.sh`):**

- **Setup**: Run `./scripts/setup-mcp.sh` (installs with UV/NPX package
  managers)
- **Validation**: Run `./scripts/mcp-validate.sh` (tests 8/8 operational, 5s
  timeout per server)
- **Core servers**: filesystem, git, github, memory, sequential-thinking,
  everything, brave-search, postgres
- **Unity Integration**: VS Code setting `"unity.projectPath"` points to
  `catgirl-avatar-project/`

**Optional MCP Integrations (configured but not validated):**

- **stripe** (HTTP): Payment processing integration via `https://mcp.stripe.com`
- **huggingface** (HTTP): ML model discovery via `https://huggingface.co/mcp`
- **mongodb** (stdio): Database operations (read-only mode)
- **playwright** (stdio): Browser automation for testing
- **clarity** (stdio): Microsoft Clarity analytics integration
- **docker** (stdio): Container management operations

**Common Use Cases:**

- Create Unity scripts with proper namespaces (filesystem MCP)
- Commit with emoji conventions (git MCP):
  `git commit -m "🦋 Add butterfly flight"`
- Create GitHub issues linked to code (github MCP)
- Remember project context across sessions (memory MCP)
- Search web for Unity API documentation (brave-search MCP)
- Database schema management (postgres MCP)

### Build & Deploy Commands

```bash
# Development
npm test                          # Run Jest tests with coverage
npm run test:watch                # Watch mode for development
npm run build -- --universal      # Cross-platform build (echo stub)
./scripts/setup-mcp.sh            # Install MCP servers
./scripts/mcp-validate.sh         # Validate all 8 MCP servers

# Unity
# Use VS Code Task: "Clean Unity Project" (removes Library/Temp/obj)
# Use VS Code Task: "Check Unity Version" (Unity 6000.2.11f1)

# Container (Multi-stage build)
docker build -t ghcr.io/bambisleepchat/bambisleep-church:latest .
# Stage 1: dependencies (production deps only)
# Stage 2: builder (includes dev deps)
# Stage 3: runtime (optimized final image)
# Registry: ghcr.io/bambisleepchat/bambisleep-church
# Tags: v{major}.{minor}.{patch}, dev-{branch}, latest
```

### VS Code Tasks (Ctrl+Shift+P → "Tasks: Run Task")

1. Build Unity Project (reminder to use Unity Editor)
2. Clean Unity Project (rm -rf Library/Temp/obj)
3. Build Container (Docker with GHCR labels)
4. Run Tests (npm test with coverage)
5. Setup MCP Servers (./setup-mcp.sh)
6. Check .NET Version / Check Unity Version

### CI/CD Pipeline (`.github/workflows/build.yml`)

- **Triggers**: Push to main/dev, PRs, releases
- **Jobs**: validate-mcp → test → build-container → unity-validation → deploy →
  quality-check → summary
- **Artifacts**: Container images pushed to GHCR with proper labels
- **Container Registry Authentication**:
  - CI/CD uses `${{ secrets.GITHUB_TOKEN }}` (automatic)
  - Manual push requires GitHub Personal Access Token with `write:packages`
    scope
  - Login:
    `echo $GITHUB_TOKEN | docker login ghcr.io -u USERNAME --password-stdin`
  - See:
    [GitHub Container Registry Docs](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-container-registry)

## Real-World Development Scenarios

### Scenario 1: Adding New Catgirl Ability

**Task**: Implement "Butterfly Flight" ability with networked synchronization

```csharp
// 1. Extend CatgirlController.cs - Add to stats class
[Header("🦋 Butterfly Powers")]
public float butterflyEnergy = 100.0f;
public bool canFly = false;

// 2. Add NetworkVariable for multiplayer sync
private NetworkVariable<bool> isFlying = new NetworkVariable<bool>(false);

// 3. Add in OnNetworkSpawn() subscription
isFlying.OnValueChanged += OnFlyingStateChanged;

// 4. Implement ability activation
[ServerRpc]
private void ActivateButterflyFlightServerRpc()
{
    if (butterflyEnergy > 20f)
    {
        isFlying.Value = true;
        butterflyEnergy -= 20f;
        animator.SetBool("IsFlying", true);
    }
}

// 5. Add to Update() for input handling
if (Input.GetKeyDown(KeyCode.Space) && IsOwner)
    ActivateButterflyFlightServerRpc();
```

**Files to modify**: `CatgirlController.cs` (add ~30 lines), update animator
controller in Unity Editor

### Scenario 2: Creating New Shop Item with Gambling Unlock

**Task**: Add "Divine Cow Crown" item available via auction or gambling

```csharp
// 1. Add item definition to InventorySystem.cs
var divineCowCrown = new CatgirlItem {
    itemId = "divine_cow_crown_001",
    displayName = "Divine Cow Crown",
    rarity = 5, // Diablo secret level tier
    isCowPowerItem = true,
    pinkValue = 5000f,
    description = "Legendary crown that channels ancient cow powers"
};

// 2. Add to UniversalBankingSystem.cs gambling rewards
private void ProcessGamblingWin(string gameType, long betAmount)
{
    float roll = Random.value;
    if (roll < 0.001f) // 0.1% chance for legendary
    {
        inventory.AddItem(GetLegendaryItem("divine_cow_crown_001"));
        TriggerWinEffectsClientRpc();
    }
}

// 3. Register item in Unity Gaming Services Economy Dashboard
// - Currency ID: "divine_cow_crown_001"
// - Category: "legendary_equipment"
// - Custom data: {"slot": "head", "cowPowerBonus": 1000}
```

**Files to modify**: `InventorySystem.cs` (~10 lines),
`UniversalBankingSystem.cs` (~15 lines), UGS Dashboard config

### Scenario 3: MCP-Assisted Development Workflow

**Task**: Use MCP servers to scaffold new Unity component

```bash
# 1. Use filesystem MCP to create new script
# AI agent creates: Assets/Scripts/Character/TailPhysicsController.cs
# With proper namespace: BambiSleep.CatGirl.Character
# Includes [Header("🌸 Tail Configuration")] attributes

# 2. Use git MCP to commit with emoji convention
git add Assets/Scripts/Character/TailPhysicsController.cs
git commit -m "🦋 Add dynamic tail physics with wind simulation

- Real-time tail movement based on velocity
- Wind zone interaction for realistic swaying
- Networked tail position synchronization
- Pink sparkle particle trail on rapid movement"

# 3. Use github MCP to create tracking issue
# Creates issue titled: "Implement tail collision detection"
# Links to TailPhysicsController.cs:45 (specific line)
# Auto-assigns labels: "enhancement", "unity", "character-system"
```

**MCP servers used**: filesystem (file creation), git (commit), github (issue
tracking)

### Scenario 4: Debugging Unity Gaming Services Integration

**Task**: Troubleshoot Economy service connection failing

```csharp
// 1. Check initialization order in UniversalBankingSystem.cs
private async void Start()
{
    try
    {
        Debug.Log("🏦 Initializing Unity Gaming Services...");

        // CRITICAL: Initialize in this exact order
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        Debug.Log($"✅ Player ID: {AuthenticationService.Instance.PlayerId}");

        // Only after auth succeeds
        await LoadPlayerBalances();
    }
    catch (AuthenticationException e)
    {
        Debug.LogError($"❌ Auth failed: {e.Message}");
        // Fallback to offline mode
        InitializeOfflineBalance();
    }
}

// 2. Verify credentials in Unity Dashboard
// - Project ID matches ProjectSettings/Services/com.unity.services.core
// - Economy items configured (pinkCoins, cowTokens)
// - Environment set to "production" or "development"

// 3. Test with manual balance fetch
[ContextMenu("Test UGS Connection")]
private async void TestEconomyConnection()
{
    var balances = await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
    Debug.Log($"Fetched {balances.Balances.Count} currency types");
}
```

**Debugging steps**: Check Unity logs, verify Dashboard config, test auth flow
separately

### Scenario 5: Implementing Multiplayer Auction House

**Task**: Create real-time auction system synchronized across clients

```csharp
// 1. Create data structure in UniversalBankingSystem.cs
[System.Serializable]
public struct AuctionListing
{
    public string itemId;
    public ulong sellerClientId;
    public long currentBid;
    public long buyoutPrice;
    public double expirationTime;
}

// 2. Use NetworkList for synchronization
private NetworkList<AuctionListing> activeAuctions;

private void Awake()
{
    activeAuctions = new NetworkList<AuctionListing>();
}

// 3. Server handles bid logic
[ServerRpc(RequireOwnership = false)]
public void PlaceBidServerRpc(string itemId, long bidAmount, ServerRpcParams rpcParams = default)
{
    var auction = activeAuctions.FirstOrDefault(a => a.itemId == itemId);

    if (bidAmount > auction.currentBid)
    {
        // Refund previous bidder
        RefundPreviousBidder(auction);

        // Update auction
        auction.currentBid = bidAmount;
        auction.buyerClientId = rpcParams.Receive.SenderClientId;

        // Notify all clients
        NotifyBidUpdateClientRpc(itemId, bidAmount);
    }
}

// 4. Update UI on all clients
[ClientRpc]
private void NotifyBidUpdateClientRpc(string itemId, long newBid)
{
    // Update InventoryUI auction display
    FindObjectOfType<InventoryUI>().RefreshAuctionListing(itemId, newBid);
}
```

**Files to modify**: `UniversalBankingSystem.cs` (~80 lines), `InventoryUI.cs`
(~40 lines), new prefab for auction UI

### Scenario 6: Optimizing Animator Performance

**Task**: Reduce animator overhead when many catgirls are on screen

```csharp
// Current pattern in CatgirlController.cs (CORRECT):
private static readonly int Speed = Animator.StringToHash("Speed");
private static readonly int IsPurring = Animator.StringToHash("IsPurring");

// ❌ DON'T DO THIS (performance killer):
animator.SetFloat("Speed", currentSpeed); // String lookup every frame

// ✅ DO THIS (cached hash):
animator.SetFloat(Speed, currentSpeed); // Direct hash lookup

// Additional optimization: Culling system
private void Update()
{
    // Only update animator if visible to camera
    if (!isVisibleToCamera)
    {
        animator.cullingMode = AnimatorCullingMode.CullCompletely;
        return;
    }

    animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
    UpdateAnimations();
}
```

**Impact**: 60+ FPS with 50 catgirls vs 20 FPS with string lookups

### Scenario 7: Container Deployment with New Features

**Task**: Build and deploy updated avatar system to GHCR

```bash
# 1. Update package.json version (semantic versioning)
# Change: "version": "1.0.0" → "1.1.0" (minor feature addition)

# 2. Build container with proper labels
docker build \
    -t ghcr.io/bambisleepchat/bambisleep-church:v1.1.0 \
    -t ghcr.io/bambisleepchat/bambisleep-church:latest \
    --label org.opencontainers.image.version="1.1.0" \
    --label org.bambi.feature="butterfly-flight-ability" \
    --label org.bambi.unity-version="6000.2.11f1" \
    .

# 3. Test container locally
docker run --rm bambisleep-church:v1.1.0 npm test

# 4. Push to registry (happens automatically via CI/CD)
# GitHub Actions workflow triggers on version tag:
git tag -a v1.1.0 -m "🦋 Add butterfly flight ability"
git push origin v1.1.0

# 5. Verify deployment
docker pull ghcr.io/bambisleepchat/bambisleep-church:v1.1.0
docker inspect ghcr.io/bambisleepchat/bambisleep-church:v1.1.0 | grep -i bambi
```

**CI/CD result**: 7 jobs run, container deployed with all labels, semantic
versioning maintained

### Scenario 8: Memory Server for Development Context

**Task**: Use MCP memory server to maintain knowledge across coding sessions

```bash
# Session 1: Store Unity package versions
# AI agent remembers: "This project uses Unity Netcode 1.11.0, UGS Economy 3.4.2"

# Session 2: Query previous decisions
# AI recalls: "CatgirlController uses NetworkBehaviour, not MonoBehaviour"

# Session 3: Reference past implementations
# AI knows: "Gambling system uses 5% house edge, defined in UniversalBankingSystem.cs:299"

# Practical usage:
# - Remembers BambiSleep™ trademark requirement
# - Recalls emoji conventions (🦋 for transformations, 🌸 for packages)
# - Maintains context about cow powers being "secret level" features
# - Tracks which Unity systems are complete vs in-progress (from todo.md)
```

**MCP servers used**: memory (context persistence), sequential-thinking (complex
reasoning)

### Scenario 9: Node.js ↔ Unity IPC Communication

**Task**: Implement bidirectional communication for cathedral rendering

```javascript
// 1. Create Unity bridge in Node.js (src/unity/unity-bridge.js)
const { spawn } = require('child_process');

class UnityBridge extends EventEmitter {
  constructor(options) {
    super();
    this.unityPath = options.unityPath;
    this.projectPath = options.projectPath;
    this.process = null;
  }

  start() {
    this.process = spawn(this.unityPath, [
      '-batchmode',
      '-projectPath',
      this.projectPath,
      '-executeMethod',
      'IPCBridge.StartIPC'
    ]);

    // Parse JSON messages from Unity
    this.process.stdout.on('data', (data) => {
      const lines = data.toString().split('\n').filter(Boolean);
      lines.forEach((line) => {
        try {
          const msg = JSON.parse(line);
          this.emit(`unity:${msg.type}`, msg.data);
        } catch (e) {
          console.error('Invalid JSON from Unity:', line);
        }
      });
    });
  }

  sendMessage(type, data) {
    const message = {
      type,
      timestamp: new Date().toISOString(),
      data
    };
    this.process.stdin.write(JSON.stringify(message) + '\n');
  }
}

// 2. Use the bridge
const bridge = new UnityBridge({
  unityPath: '/opt/unity/Editor/Unity',
  projectPath: './catgirl-avatar-project'
});

bridge.on('unity:scene-loaded', (data) => {
  console.log('Scene loaded:', data.sceneName);
  bridge.sendMessage('update', { neonIntensity: 7.5 });
});

bridge.on('unity:render-complete', (data) => {
  console.log('Render saved:', data.outputPath);
});

bridge.start();
```

```csharp
// 3. Unity IPC handler (Assets/Scripts/IPC/IPCBridge.cs)
using UnityEngine;
using System;

[Serializable]
public class IPCMessage {
    public string type;
    public string timestamp;
    public string data; // JSON string for nested object
}

public class IPCBridge : MonoBehaviour {
    void Update() {
        // Read from stdin (Node.js writes here)
        if (Console.KeyAvailable) {
            string json = Console.ReadLine();
            ProcessMessage(json);
        }
    }

    void ProcessMessage(string json) {
        try {
            IPCMessage msg = JsonUtility.FromJson<IPCMessage>(json);

            switch (msg.type) {
                case "initialize":
                    var initData = JsonUtility.FromJson<InitData>(msg.data);
                    InitializeScene(initData);
                    break;
                case "update":
                    var updateData = JsonUtility.FromJson<UpdateData>(msg.data);
                    UpdateParameters(updateData);
                    SendMessage("update-ack", new { success = true });
                    break;
                case "render":
                    var renderData = JsonUtility.FromJson<RenderData>(msg.data);
                    RenderScene(renderData);
                    break;
            }
        } catch (Exception e) {
            SendMessage("error", new {
                errorCode = "INVALID_MESSAGE",
                message = e.Message
            });
        }
    }

    void SendMessage(string type, object data) {
        var msg = new {
            type,
            timestamp = DateTime.UtcNow.ToString("o"),
            data
        };
        // Write to stdout (Node.js reads this)
        Console.WriteLine(JsonUtility.ToJson(msg));
    }

    void InitializeScene(InitData data) {
        // Load scene, configure parameters
        SendMessage("scene-loaded", new {
            sceneName = data.sceneName,
            objectCount = 156
        });
    }
}
```

**Message flow**: Node→Unity (initialize, update, render) | Unity→Node
(scene-loaded, update-ack, render-complete, error, heartbeat)

**Files to create**: `src/unity/unity-bridge.js` (~200 lines),
`Assets/Scripts/IPC/IPCBridge.cs` (~150 lines)

**Reference**: `docs/architecture/UNITY_IPC_PROTOCOL.md` for complete protocol
spec with all message types
