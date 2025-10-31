## Project Overview

This is a **production Unity 6.2 LTS project** combining:

- **Unity C# Systems** (1,950+ lines): Avatar controller, economy, multiplayer networking, UI
- **Node.js ↔ Unity IPC Bridge**: JSON-based stdin/stdout communication protocol
- **MCP Agent Tooling** (8 servers): Development automation via Model Context Protocol
- **Trademark Requirement**: Always use `BambiSleep™` (with ™) in public-facing content
- **Documentation as Code**: Markdown files contain canonical implementations to copy verbatim

### Quick Actions

| Task                     | Command/Location                                                                                |
| ------------------------ | ----------------------------------------------------------------------------------------------- |
| **Extend Unity systems** | Read `catgirl-avatar-project/Assets/Scripts/{domain}/{ClassName}.cs` FIRST, follow its patterns |
| **MCP setup**            | Run `./setup-mcp.sh`, test with `./mcp-validate.sh`                                             |
| **Build/Deploy**         | VS Code tasks (Ctrl+Shift+P → "Tasks: Run Task") or npm scripts                                 |
| **Architecture guide**   | `docs/development/UNITY_SETUP_GUIDE.md` (858 lines with actual C# code)                         |

## Project Structure

```
bambisleep-chat-catgirl/
├── catgirl-avatar-project/          # Unity 6.2 LTS (Unity 6000.2.11f1)
│   ├── Assets/Scripts/              # 6 complete C# systems (1,950+ lines)
│   │   ├── Audio/AudioManager.cs    # 342 lines - Singleton audio system
│   │   ├── Character/CatgirlController.cs  # 327 lines - NetworkBehaviour
│   │   ├── Economy/InventorySystem.cs      # 284 lines - UGS Economy
│   │   ├── Economy/UniversalBankingSystem.cs # 363 lines - Multi-currency
│   │   ├── Networking/CatgirlNetworkManager.cs # 324 lines - Relay + Lobby
│   │   └── UI/InventoryUI.cs        # 322 lines - UI Toolkit interface
│   ├── Packages/manifest.json       # 16 Unity packages (UGS, Netcode, XR)
│   └── ProjectSettings/ProjectVersion.txt
├── docs/                            # 4,200+ lines documentation
│   ├── architecture/                # CATGIRL.md (682), UNITY_IPC_PROTOCOL.md (430)
│   ├── development/                 # UNITY_SETUP_GUIDE.md (858 - READ THIS)
│   └── guides/                      # build.md, todo.md
├── .github/workflows/build.yml      # CI/CD with 7 jobs
├── .vscode/                         # MCP integration + 8 tasks
├── Dockerfile                       # GHCR: bambisleepchat/bambisleep-church
├── package.json                     # Node.js 20.19.5 (Volta pinned)
├── setup-mcp.sh                     # MCP server installation
└── mcp-validate.sh                  # Test all 8 MCP servers
```

**Essential Documentation** (read in this order):

1. `docs/development/UNITY_SETUP_GUIDE.md` - Complete C# implementations
2. `docs/architecture/CATGIRL.md` - System architecture & Unity packages
3. `docs/architecture/UNITY_IPC_PROTOCOL.md` - Node.js ↔ Unity communication protocol
4. `docs/architecture/RELIGULOUS_MANTRA.md` - Emoji conventions & build philosophy
5. `docs/guides/todo.md` - Implementation status (complete vs in-progress)

## Critical Unity C# Patterns

**All 6 systems are COMPLETE** (1,950+ lines). When extending, follow these patterns:

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
- `com.unity.netcode.gameobjects` 1.11.0 (multiplayer)
- `com.unity.services.lobby` 1.2.2 (matchmaking)
- `com.unity.xr.interaction.toolkit` 3.0.7 (XR support)

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

**Message Types**: initialize, update, render, camera, postprocessing, shutdown (Node→Unity); scene-loaded, render-complete, update-ack, error, heartbeat (Unity→Node)

**See**: `docs/architecture/UNITY_IPC_PROTOCOL.md` for complete protocol specification

## Development Workflows

### MCP Environment (8 Servers)

- **Setup**: Run `./setup-mcp.sh` (installs all servers)
- **Validation**: Run `./mcp-validate.sh` (tests 8/8 operational)
- **Config**: `.vscode/settings.json` (filesystem, git, github, memory, sequential-thinking, everything, brave-search, postgres)
- **Use cases**:
  - Create Unity scripts with proper namespaces (filesystem MCP)
  - Commit with emoji conventions (git MCP): `git commit -m "🦋 Add butterfly flight"`
  - Create GitHub issues linked to code (github MCP)

### Build & Deploy Commands

```bash
# Development
npm test                          # Run tests (stubs currently, see todo.md)
npm run build -- --universal      # Cross-platform build
./setup-mcp.sh                    # Install MCP servers

# Unity
# Use VS Code Task: "Clean Unity Project" (removes Library/Temp/obj)
# Use VS Code Task: "Check Unity Version" (Unity 6000.2.11f1)

# Container
docker build -t ghcr.io/bambisleepchat/bambisleep-church:latest .
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
- **Jobs**: validate-mcp → test → build-container → unity-validation → deploy → quality-check → summary
- **Artifacts**: Container images pushed to GHCR with proper labels

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

**Files to modify**: `CatgirlController.cs` (add ~30 lines), update animator controller in Unity Editor

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

**Files to modify**: `InventorySystem.cs` (~10 lines), `UniversalBankingSystem.cs` (~15 lines), UGS Dashboard config

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

**MCP servers used**: filesystem (file creation), git (commit), github (issue tracking)

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

**Debugging steps**: Check Unity logs, verify Dashboard config, test auth flow separately

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

**Files to modify**: `UniversalBankingSystem.cs` (~80 lines), `InventoryUI.cs` (~40 lines), new prefab for auction UI

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

**CI/CD result**: 7 jobs run, container deployed with all labels, semantic versioning maintained

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

**MCP servers used**: memory (context persistence), sequential-thinking (complex reasoning)

### Scenario 9: Node.js ↔ Unity IPC Communication

**Task**: Implement bidirectional communication for cathedral rendering

```javascript
// 1. Create Unity bridge in Node.js (src/unity/unity-bridge.js)
const { spawn } = require("child_process");

class UnityBridge extends EventEmitter {
  constructor(options) {
    super();
    this.unityPath = options.unityPath;
    this.projectPath = options.projectPath;
    this.process = null;
  }

  start() {
    this.process = spawn(this.unityPath, [
      "-batchmode",
      "-projectPath",
      this.projectPath,
      "-executeMethod",
      "IPCBridge.StartIPC",
    ]);

    // Parse JSON messages from Unity
    this.process.stdout.on("data", (data) => {
      const lines = data.toString().split("\n").filter(Boolean);
      lines.forEach((line) => {
        try {
          const msg = JSON.parse(line);
          this.emit(`unity:${msg.type}`, msg.data);
        } catch (e) {
          console.error("Invalid JSON from Unity:", line);
        }
      });
    });
  }

  sendMessage(type, data) {
    const message = {
      type,
      timestamp: new Date().toISOString(),
      data,
    };
    this.process.stdin.write(JSON.stringify(message) + "\n");
  }
}

// 2. Use the bridge
const bridge = new UnityBridge({
  unityPath: "/opt/unity/Editor/Unity",
  projectPath: "./catgirl-avatar-project",
});

bridge.on("unity:scene-loaded", (data) => {
  console.log("Scene loaded:", data.sceneName);
  bridge.sendMessage("update", { neonIntensity: 7.5 });
});

bridge.on("unity:render-complete", (data) => {
  console.log("Render saved:", data.outputPath);
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

**Message flow**: Node→Unity (initialize, update, render) | Unity→Node (scene-loaded, update-ack, render-complete, error, heartbeat)

**Files to create**: `src/unity/unity-bridge.js` (~200 lines), `Assets/Scripts/IPC/IPCBridge.cs` (~150 lines)

**Reference**: `docs/architecture/UNITY_IPC_PROTOCOL.md` for complete protocol spec with all message types
