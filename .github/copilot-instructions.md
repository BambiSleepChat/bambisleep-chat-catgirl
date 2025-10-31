## Project Overview

**BambiSleep™ Church CatGirl Avatar System** - A production Unity 6.2 LTS
project with Node.js orchestration.

**Key Technologies:**

- **Unity 6.2 LTS** (6000.2.11f1): 7 complete C# systems (2,491 lines) with
  NetworkBehaviour multiplayer, Unity Gaming Services (UGS) economy, XR
  interaction toolkit
- **Node.js 20.19.5** (Volta-pinned): IPC bridge for Unity communication via
  JSON stdin/stdout
- **MCP Servers** (8 core): filesystem, git, github, memory,
  sequential-thinking, everything, brave-search, postgres
- **Jest Testing**: 80% coverage threshold with real assertions (not stubs)
- **Container-Ready**: Multi-stage Dockerfile → GHCR:
  `ghcr.io/bambisleepchat/bambisleep-church`

**Critical Project Rules:**

1. Always use `BambiSleep™` (with ™) in public-facing content
2. Follow emoji conventions in commits: `🦋 Add feature` (see
   RELIGULOUS_MANTRA.md)
3. Documentation contains **actual code** to copy verbatim, not generic advice
4. Namespace pattern: `BambiSleep.CatGirl.{Domain}` for all Unity scripts

## Quick Start Decision Tree

**Choose your workflow path:**

```
├─ Extend Unity C# → Read existing script in Assets/Scripts/{domain}/ FIRST
│  └─ Copy patterns: namespace BambiSleep.CatGirl.{Domain}, [Header("🌸 Name")], NetworkBehaviour lifecycle
│
├─ Add Node.js feature → Review src/unity/unity-bridge.js, add test in __tests__/, run `npm test`
│
├─ Debug Unity Gaming Services → Check docs/DEBUGGING.md, verify init order: UnityServices→Auth→Economy
│
├─ Use MCP automation → Run ./scripts/mcp-validate.sh (tests 8/8 servers operational)
│
└─ Build/Deploy → Update package.json version, run `npm test`, use VS Code task "Build Container"
```

**Essential Commands:**

- `npm test` - Jest with 80% coverage (fails below threshold)
- `npm run test:watch` - TDD development mode
- `./scripts/mcp-validate.sh` - Verify all 8 MCP servers work
- VS Code task "Check Unity Version" - Confirms Unity 6000.2.11f1
- VS Code task "Clean Unity Project" - Removes Library/Temp/obj directories

**Must-Read Documentation (in order):**

1. `docs/development/UNITY_SETUP_GUIDE.md` - Complete C# implementations (858
   lines)
2. `docs/architecture/CATGIRL.md` - System architecture & 16 Unity packages
3. `docs/architecture/UNITY_IPC_PROTOCOL.md` - Node.js ↔ Unity communication
   (430 lines)
4. `docs/DEBUGGING.md` - Debugging reference (522 lines)
5. `docs/guides/todo.md` - Implementation status

## Project Culture & Conventions

**Playful maximalist aesthetic** - pink frilly themes, cow powers (Diablo
references), Universal Machine Philosophy. NOT typical enterprise code.

**Emoji Conventions** (from RELIGULOUS_MANTRA.md):

- 🌸 Packages/core systems
- 🦋 Transformations/NetworkBehaviour events
- 💎 Premium features
- � Secret cow power features
- 🔥 Performance-critical code

**Commit format:** `🦋 Add feature description` (emoji prefix required)

**Key Principles:**

1. Documentation = actual code to copy (not generic advice)
2. MCP-first development (8 servers validated via `./scripts/mcp-validate.sh`)
3. 100% completion mindset (no half-implementations)
4. BambiSleep™ trademark in all public content

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

### 7. Unity Package Dependencies (16 packages)

From `Packages/manifest.json`, critical packages for development:

**Multiplayer & Networking:**

- `com.unity.netcode.gameobjects` 2.0.0 - Core multiplayer framework
- `com.unity.services.relay` 1.1.3 - NAT traversal for P2P connections
- `com.unity.services.lobby` 1.2.2 - Matchmaking and lobby system

**Economy & Services:**

- `com.unity.services.core` 1.15.0 - Required base for all Unity Gaming Services
- `com.unity.services.authentication` 3.3.4 - Player identity (MUST initialize
  first)
- `com.unity.services.economy` 3.4.2 - Currency and inventory cloud storage
- `com.unity.services.analytics` 5.1.1 - Telemetry and player behavior tracking
- `com.unity.purchasing` 4.12.2 - In-app purchase integration

**XR & Advanced Features:**

- `com.unity.xr.interaction.toolkit` 3.0.5 - Eye tracking, hand gestures, VR
  input

**UI & Visuals:**

- `com.unity.ui.toolkit` 2.0.0 - Modern UI system (used by InventoryUI.cs)
- `com.unity.ugui` 2.0.0 - Legacy UI system (Canvas-based)
- `com.unity.visualeffectgraph` 16.0.6 - Particle systems and VFX

**Asset Management:**

- `com.unity.addressables` 2.3.1 - Async asset loading and memory management

**Animation & Cinematics:**

- `com.unity.animation.rigging` 1.3.1 - Procedural animation (tail physics, IK)
- `com.unity.cinemachine` 2.10.1 - Camera system with cinematic features
- `com.unity.timeline` 1.8.7 - Cutscenes and sequenced animations

### 8. Economy System Patterns (InventorySystem.cs & UniversalBankingSystem.cs)

**Item Rarity System** (5 tiers):

```csharp
public int rarity; // 1=Common, 2=Uncommon, 3=Rare, 4=Epic, 5=Diablo Secret Level
public bool isCowPowerItem = false; // Secret cow power flag
public float pinkValue = 0f; // Pink currency value
```

**Currency Management** (Multi-currency system):

```csharp
[SerializeField] private string primaryCurrencyId = "PINK_COINS";
[SerializeField] private List<CurrencyBalance> currencies;

// Load from Unity Gaming Services
var balances = await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
```

**Gambling System** (2% house edge):

```csharp
[SerializeField] private float houseEdge = 0.02f; // 2% house advantage
[SerializeField] private long minBet = 10;
[SerializeField] private long maxBet = 10000;

// Process gambling wins with rarity drops
float roll = Random.value;
if (roll < 0.001f) { // 0.1% legendary drop
    inventory.AddItem(GetLegendaryItem("divine_cow_crown_001"));
}
```

**Auction System** (5% listing fee):

```csharp
[SerializeField] private float auctionFeePercentage = 0.05f;

[System.Serializable]
public struct AuctionItem {
    public string auctionId;
    public long currentBid;
    public string highestBidder;
    public System.DateTime endTime;
}
```

**Transaction Logging**:

```csharp
[SerializeField] private List<Transaction> transactionHistory;
[SerializeField] private int maxHistoryEntries = 100;

// Track all currency movements
var transaction = new Transaction {
    transactionId = System.Guid.NewGuid().ToString(),
    type = "gambling", // deposit, withdrawal, gambling, auction
    timestamp = System.DateTime.UtcNow
};
```

### 9. Data Serialization Patterns

**[System.Serializable] for Inspector Editing**:

```csharp
[System.Serializable]
public class CatgirlStats {
    [Header("✨ Frilly Pink Configuration")]
    public float pinkIntensity = 1.0f;
    public float frillinessLevel = 100.0f;
}
```

**NetworkVariable for Multiplayer Sync**:

```csharp
private NetworkVariable<float> networkPinkIntensity = new NetworkVariable<float>(1.0f);
private NetworkVariable<bool> networkCowPowersActive = new NetworkVariable<bool>(false);

// Subscribe in OnNetworkSpawn
networkPinkIntensity.OnValueChanged += OnPinkIntensityChanged;

// Unsubscribe in OnNetworkDespawn
networkPinkIntensity.OnValueChanged -= OnPinkIntensityChanged;
```

**Unity Gaming Services Data**:

```csharp
// Load from cloud
var inventoryResult = await EconomyService.Instance.PlayerInventory.GetInventoryAsync();
var balances = await EconomyService.Instance.PlayerBalances.GetBalancesAsync();

// Save to cloud (handled automatically by UGS)
await EconomyService.Instance.PlayerBalances.IncrementBalanceAsync("PINK_COINS", 100);
```

### 10. Animation System - Mecanim State Machine Patterns

**Animator Controller Setup** (from CatgirlController.cs):

```csharp
// Cache animation parameter hashes (CRITICAL for performance)
private static readonly int Speed = Animator.StringToHash("Speed");
private static readonly int IsJumping = Animator.StringToHash("IsJumping");
private static readonly int IsPurring = Animator.StringToHash("IsPurring");
private static readonly int CowPowerActive = Animator.StringToHash("CowPowerActive");

private Animator animator;

void Awake() {
    animator = GetComponent<Animator>();
}
```

**Parameter Updates** (every frame optimization):

```csharp
void Update() {
    // ❌ NEVER DO THIS (string lookup every frame = performance killer)
    // animator.SetFloat("Speed", currentSpeed);

    // ✅ DO THIS (cached hash lookup = fast)
    animator.SetFloat(Speed, currentSpeed);
    animator.SetBool(IsJumping, isJumping);
    animator.SetBool(IsPurring, isPurring);
}
```

**State Machine Transitions**:

```csharp
// Trigger-based transitions (one-time events)
animator.SetTrigger("Jump");
animator.SetTrigger("Attack");
animator.SetTrigger("Dance");

// Boolean-based transitions (state toggles)
animator.SetBool(IsPurring, true);  // Enter purring state
animator.SetBool(IsPurring, false); // Exit purring state

// Float-based blend trees (smooth transitions)
animator.SetFloat(Speed, currentSpeed); // 0.0 = idle, 1.0 = walk, 2.0 = run
```

**Animation Events** (called from animation clips):

```csharp
// These methods are invoked by Animation Events in Unity Editor
public void OnFootstep() {
    AudioManager.Instance.PlayOneShot("footstep");
}

public void OnPurrStart() {
    AudioManager.Instance.Play("purring_loop");
}

public void OnCowPowerActivated() {
    AudioManager.Instance.PlayRandomCowMoo();
    // Spawn particle effects, etc.
}
```

**Layered Animation** (multiple simultaneous animations):

```csharp
// Base layer: locomotion (walk, run, jump)
animator.SetFloat(Speed, moveSpeed);

// Upper body layer: gestures (wave, point, emote)
animator.SetLayerWeight(1, 1.0f); // Enable upper body layer
animator.SetTrigger("Wave");

// Facial layer: expressions (smile, blink, purr)
animator.SetLayerWeight(2, 1.0f);
```

**Common Mecanim State Machine Structure**:

- **Idle State** → Speed = 0
- **Walk State** → Speed > 0 && Speed < 1.5
- **Run State** → Speed >= 1.5
- **Jump State** → IsJumping = true (exit condition: OnAnimationEnd)
- **Purr State** → IsPurring = true (looping animation)
- **Special State** → CowPowerActive = true (pink frilly effects)

### 11. Audio System - AudioManager Singleton Pattern

**Singleton Implementation** (from AudioManager.cs):

```csharp
namespace BambiSleep.CatGirl.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;

        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AudioManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("AudioManager");
                        instance = go.AddComponent<AudioManager>();
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject); // Persist across scenes
            }
            else if (instance != this)
            {
                Destroy(gameObject); // Destroy duplicate
                return;
            }

            InitializeAudioSources();
        }
    }
}
```

**Audio Mixer Groups** (4-channel architecture):

```csharp
[SerializeField] private AudioMixerGroup masterMixerGroup;
[SerializeField] private AudioMixerGroup musicMixerGroup;
[SerializeField] private AudioMixerGroup sfxMixerGroup;
[SerializeField] private AudioMixerGroup voiceMixerGroup;

// Volume control (logarithmic scale for natural perception)
public void SetMasterVolume(float volume) {
    masterMixerGroup?.audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
}

public void SetMusicVolume(float volume) {
    musicMixerGroup?.audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
}
```

**Sound Dictionary Pattern** (O(1) lookup):

```csharp
[System.Serializable]
public class Sound {
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop = false;
    [HideInInspector] public AudioSource source;
}

[SerializeField] private Sound[] sounds;
private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

void InitializeAudioSources() {
    foreach (Sound sound in sounds) {
        sound.source = gameObject.AddComponent<AudioSource>();
        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
        sound.source.loop = sound.loop;
        sound.source.outputAudioMixerGroup = sfxMixerGroup;

        soundDictionary[sound.name] = sound; // Fast lookup
    }
}
```

**Common Usage Patterns**:

```csharp
// Play sound by name (fast dictionary lookup)
AudioManager.Instance.Play("footstep");
AudioManager.Instance.Play("purring_loop");
AudioManager.Instance.Stop("purring_loop");

// One-shot sounds (fire and forget)
AudioManager.Instance.PlayOneShot("nyan_sound");
AudioManager.Instance.PlayOneShot("button_click");

// Random variations (adds variety)
AudioManager.Instance.PlayRandomPurr();
AudioManager.Instance.PlayRandomNyan();
AudioManager.Instance.PlayRandomCowMoo(); // 🐄

// Music transitions (crossfade)
AudioManager.Instance.PlayMusic(backgroundMusicClip);
AudioManager.Instance.FadeMusicTo(combatMusicClip, 2.0f); // 2 second fade
```

**Best Practices**:

- **One AudioManager per scene** (DontDestroyOnLoad ensures singleton)
- **Preload sounds in dictionary** for fast runtime lookup
- **Use AudioMixerGroups** for grouped volume control (Master/Music/SFX/Voice)
- **One-shot for effects**, **looping for ambience/music**
- **Object pooling** for frequently used sounds (e.g., footsteps)

### 12. UI Toolkit Patterns - Modern UI Implementation

**UIDocument Setup** (from InventoryUI.cs):

```csharp
namespace BambiSleep.CatGirl.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        private VisualElement root;

        private void OnEnable()
        {
            root = uiDocument.rootVisualElement;
            InitializeUI();
        }
    }
}
```

**Pink Theme Colors** (project-specific palette):

```csharp
[Header("💎 Pink Theme Colors")]
[SerializeField] private Color pinkPrimary = new Color(1f, 0.41f, 0.71f);    // #ff69b4
[SerializeField] private Color pinkHighlight = new Color(1f, 0.08f, 0.58f);  // #ff1493
[SerializeField] private Color pinkDark = new Color(0.78f, 0.08f, 0.52f);    // #c71585
```

**Creating UI Elements Programmatically**:

```csharp
// Container with styling
var mainContainer = new VisualElement();
mainContainer.name = "inventory-main-container";
mainContainer.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
mainContainer.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
mainContainer.style.backgroundColor = new StyleColor(new Color(0.1f, 0.1f, 0.1f, 0.95f));
mainContainer.style.paddingTop = 20;
mainContainer.style.paddingLeft = 20;

// Styled label
var headerLabel = new Label("🌸 Pink Frilly Inventory 🌸");
headerLabel.style.fontSize = 32;
headerLabel.style.color = pinkPrimary;
headerLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
headerLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
mainContainer.Add(headerLabel);
```

**Button with Hover Effects**:

```csharp
private Button CreateButton(string text, System.Action callback)
{
    var button = new Button(callback);
    button.text = text;
    button.style.backgroundColor = pinkDark;
    button.style.color = Color.white;
    button.style.paddingTop = 10;
    button.style.paddingBottom = 10;
    button.style.paddingLeft = 20;
    button.style.paddingRight = 20;
    button.style.borderTopLeftRadius = 5;
    button.style.borderBottomRightRadius = 5;
    button.style.fontSize = 16;

    // Hover effect (interactive feedback)
    button.RegisterCallback<MouseEnterEvent>(evt => {
        button.style.backgroundColor = pinkHighlight;
    });
    button.RegisterCallback<MouseLeaveEvent>(evt => {
        button.style.backgroundColor = pinkDark;
    });

    return button;
}
```

**Flexbox Layout** (responsive grid):

```csharp
// Grid container with wrap
inventoryContainer = new VisualElement();
inventoryContainer.style.flexDirection = FlexDirection.Row;
inventoryContainer.style.flexWrap = Wrap.Wrap;
inventoryContainer.style.justifyContent = Justify.FlexStart;
inventoryContainer.style.flexGrow = 1;

// Item cards (150x200 each)
foreach (var slot in slots) {
    var card = new VisualElement();
    card.style.width = 150;
    card.style.height = 200;
    card.style.marginRight = 10;
    card.style.marginBottom = 10;
    card.style.borderTopLeftRadius = 10;
    card.style.borderBottomRightRadius = 10;

    // Rarity-based border color
    Color borderColor = GetRarityColor(slot.item.rarity);
    card.style.borderTopColor = borderColor;
    card.style.borderTopWidth = 3;

    inventoryContainer.Add(card);
}
```

**Rarity Color System**:

```csharp
private Color GetRarityColor(int rarity) {
    return rarity switch {
        1 => Color.gray,                // Common
        2 => Color.green,               // Uncommon
        3 => Color.blue,                // Rare
        4 => new Color(0.64f, 0.21f, 0.93f), // Epic (purple)
        5 => new Color(1f, 0.5f, 0f),   // Legendary (orange)
        _ => Color.white
    };
}
```

**USS (Unity Style Sheets) Alternative**:

Instead of C# styling, use USS files for cleaner separation:

```css
/* Resources/UI/InventoryStyles.uss */
.inventory-main-container {
  width: 100%;
  height: 100%;
  background-color: rgba(26, 26, 26, 0.95);
  padding: 20px;
}

.inventory-header {
  font-size: 32px;
  color: rgb(255, 105, 180);
  -unity-text-align: middle-center;
  -unity-font-style: bold;
  margin-bottom: 20px;
}

.inventory-grid {
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: flex-start;
  flex-grow: 1;
}
```

**Best Practices**:

- **USS for static styles**, **C# for dynamic styling**
- **Use flexbox** for responsive layouts (flex-direction, flex-wrap)
- **Register callbacks** for interactive elements (hover, click)
- **Name elements** for USS selector targeting
- **StyleLength** units: Percent, Pixel, Auto

### 13. XR Integration - Unity 6.2 XR Interaction Toolkit

**XR Package Requirements** (from `Packages/manifest.json`):

```json
{
  "com.unity.xr.interaction.toolkit": "3.0.5",
  "com.unity.xr.openxr": "1.11.0",
  "com.unity.xr.hands": "1.5.0",
  "com.unity.animation.rigging": "1.3.1"
}
```

**Eye Tracking Implementation** (OpenXR standard):

```csharp
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

namespace BambiSleep.CatGirl.XR
{
    public class CatgirlEyeTracking : MonoBehaviour
    {
        [Header("👁️ Eye Tracking Configuration")]
        [SerializeField] private Transform leftEyeBone;
        [SerializeField] private Transform rightEyeBone;
        [SerializeField] private float eyeRotationSpeed = 10f;

        private InputDevice eyeTrackingDevice;

        void Start()
        {
            eyeTrackingDevice = InputDevices.GetDeviceAtXRNode(XRNode.CenterEye);
        }

        void Update()
        {
            if (eyeTrackingDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 gazePosition))
            {
                Vector3 gazeDirection = (gazePosition - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(gazeDirection);

                leftEyeBone.rotation = Quaternion.Slerp(leftEyeBone.rotation, targetRotation, Time.deltaTime * eyeRotationSpeed);
                rightEyeBone.rotation = Quaternion.Slerp(rightEyeBone.rotation, targetRotation, Time.deltaTime * eyeRotationSpeed);
            }
        }
    }
}
```

**Hand Tracking with Paw Gestures**:

```csharp
using UnityEngine.XR.Hands;
using UnityEngine.Animations.Rigging;

namespace BambiSleep.CatGirl.XR
{
    public class CatgirlHandTracking : MonoBehaviour
    {
        [Header("🐾 Hand Tracking Configuration")]
        [SerializeField] private Rig animationRig;
        [SerializeField] private Transform[] clawBones;
        [SerializeField] private float kneadingThreshold = 0.7f;

        private XRHandSubsystem handSubsystem;
        private bool isKneading = false;

        void Start()
        {
            var subsystems = new List<XRHandSubsystem>();
            SubsystemManager.GetSubsystems(subsystems);
            if (subsystems.Count > 0) handSubsystem = subsystems[0];
        }

        void Update()
        {
            if (handSubsystem == null) return;

            var rightHand = handSubsystem.rightHand;
            if (rightHand.isTracked)
            {
                UpdateClawPositions(rightHand);
                DetectKneadingGesture(rightHand);
            }
        }

        void UpdateClawPositions(XRHand hand)
        {
            for (int i = 0; i < 5; i++)
            {
                var fingerTip = hand.GetJoint(XRHandJointID.ThumbTip + i);
                if (fingerTip.TryGetPose(out var pose))
                {
                    clawBones[i].position = pose.position;
                    clawBones[i].rotation = pose.rotation;
                }
            }
        }

        void DetectKneadingGesture(XRHand hand)
        {
            float curvature = CalculateFingerCurvature(hand);
            if (curvature > kneadingThreshold && !isKneading) StartKneading();
            else if (curvature < kneadingThreshold && isKneading) StopKneading();
        }

        float CalculateFingerCurvature(XRHand hand)
        {
            float totalCurvature = 0f;
            for (int i = 0; i < 5; i++)
            {
                var tip = hand.GetJoint(XRHandJointID.ThumbTip + i);
                var proximal = hand.GetJoint(XRHandJointID.ThumbProximal + i);
                if (tip.TryGetPose(out var tipPose) && proximal.TryGetPose(out var proximalPose))
                {
                    float distance = Vector3.Distance(tipPose.position, proximalPose.position);
                    totalCurvature += Mathf.Clamp01(1f - (distance / 0.1f));
                }
            }
            return totalCurvature / 5f;
        }

        void StartKneading()
        {
            isKneading = true;
            animationRig.weight = 1.0f;
            AudioManager.Instance.PlayRandomPurr();
            // Send haptic feedback
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).SendHapticImpulse(0, 0.3f, 1.0f);
        }

        void StopKneading()
        {
            isKneading = false;
            animationRig.weight = 0f;
        }
    }
}
```

**Best Practices**:

- **OpenXR** for cross-platform VR/AR compatibility
- **Animation Rigging** for dynamic IK-based hand positioning
- **Haptic feedback** enhances immersion (purring, scratching sensations)
- **Foveated rendering** for performance optimization
- **Confidence scoring** to handle tracking loss gracefully (check `isTracked`)

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

**MCP Integration in Unity** (MCPAgent.cs):

The `MCPAgent` component (in `Assets/Scripts/IPC/MCPAgent.cs`) provides
Unity-side MCP integration:

```csharp
// Enable in Unity Inspector
public bool enableFilesystemMCP = true;
public bool enableGitMCP = true;
public bool enableMemoryMCP = true;

// Send MCP request from Unity
mcpAgent.SendMCPRequest("filesystem", "create_file", new {
    path = "Assets/Scripts/Character/NewAbility.cs",
    content = generatedCSharpCode
});

// Listen for MCP responses
mcpAgent.OnMCPResponse += (server, response) => {
    Debug.Log($"MCP {server} responded: {response}");
};
```

**Common Use Cases:**

- Create Unity scripts with proper namespaces (filesystem MCP)
- Commit with emoji conventions (git MCP):
  `git commit -m "🦋 Add butterfly flight"`
- Create GitHub issues linked to code (github MCP)
- Remember project context across sessions (memory MCP)
- Search web for Unity API documentation (brave-search MCP)
- Database schema management (postgres MCP)
- AI-powered code generation (sequential-thinking MCP)

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

### Scenario Index - Quick Lookup

| #   | Scenario                                                                                    | Key Technologies                      | Files Modified                                | Complexity |
| --- | ------------------------------------------------------------------------------------------- | ------------------------------------- | --------------------------------------------- | ---------- |
| 1   | [Adding New Catgirl Ability](#scenario-1-adding-new-catgirl-ability)                        | NetworkBehaviour, ServerRpc, Animator | CatgirlController.cs                          | ⭐⭐       |
| 2   | [Creating Shop Item with Gambling](#scenario-2-creating-new-shop-item-with-gambling-unlock) | UGS Economy, Item System              | InventorySystem.cs, UniversalBankingSystem.cs | ⭐⭐⭐     |
| 3   | [MCP-Assisted Development](#scenario-3-mcp-assisted-development-workflow)                   | Filesystem/Git/GitHub MCP             | New Unity scripts                             | ⭐         |
| 4   | [Debugging UGS Integration](#scenario-4-debugging-unity-gaming-services-integration)        | Unity Services, Authentication        | UniversalBankingSystem.cs                     | ⭐⭐⭐     |
| 5   | [Multiplayer Auction House](#scenario-5-implementing-multiplayer-auction-house)             | NetworkList, ClientRpc                | UniversalBankingSystem.cs, InventoryUI.cs     | ⭐⭐⭐⭐   |
| 6   | [Optimizing Animator Performance](#scenario-6-optimizing-animator-performance)              | StringToHash, Culling                 | CatgirlController.cs                          | ⭐⭐       |
| 7   | [Container Deployment](#scenario-7-container-deployment-with-new-features)                  | Docker, GHCR, Semantic Versioning     | Dockerfile, package.json                      | ⭐⭐       |
| 8   | [Memory Server Context](#scenario-8-memory-server-for-development-context)                  | MCP Memory, Context Persistence       | N/A (MCP usage)                               | ⭐         |
| 9   | [Node.js ↔ Unity IPC](#scenario-9-nodejs--unity-ipc-communication)                         | Child Process, EventEmitter           | unity-bridge.js, IPCBridge.cs                 | ⭐⭐⭐⭐   |

**Complexity Legend**: ⭐ = Beginner | ⭐⭐ = Intermediate | ⭐⭐⭐ = Advanced |
⭐⭐⭐⭐ = Expert

---

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

---

## 🎯 Development Cheat Sheet

### Unity C# Quick Reference

**Creating a New System**:

1. Copy namespace pattern: `namespace BambiSleep.CatGirl.{Domain}`
2. Add emoji headers: `[Header("🌸 Section Name")]`
3. For multiplayer: extend `NetworkBehaviour`, implement
   `OnNetworkSpawn/OnNetworkDespawn`
4. Cache animator hashes:
   `private static readonly int Speed = Animator.StringToHash("Speed");`
5. Use async/await for UGS: `await UnityServices.InitializeAsync();`

**Common Import Statements**:

```csharp
using Unity.Netcode; // NetworkBehaviour, ServerRpc, ClientRpc
using Unity.Services.Core; // UnityServices
using Unity.Services.Authentication; // AuthenticationService
using Unity.Services.Economy; // EconomyService
using UnityEngine.XR.Interaction.Toolkit; // XR features
using UnityEngine.UI.Toolkit; // VisualElement, UIDocument
```

### MCP Command Patterns

```bash
# Create Unity script with proper structure
mcp filesystem create_file \
  --path "Assets/Scripts/Character/NewAbility.cs" \
  --namespace "BambiSleep.CatGirl.Character"

# Commit with emoji convention
git commit -m "🦋 Add butterfly flight ability"

# Create GitHub issue linked to code
mcp github create_issue \
  --title "Implement tail collision detection" \
  --file "TailPhysicsController.cs:45"

# Remember project context
mcp memory store "CatgirlController uses NetworkBehaviour pattern"

# Search Unity docs
mcp brave-search "Unity Netcode NetworkVariable synchronization"
```

### Testing Quick Commands

```bash
# Run all tests with coverage
npm test

# Watch mode for TDD
npm run test:watch

# Test specific file
npm test -- unity-bridge.test.js

# Generate coverage report
npm test -- --coverage
```

### Docker Build & Deploy

```bash
# Build with proper labels
docker build \
  -t ghcr.io/bambisleepchat/bambisleep-church:v1.0.0 \
  --label org.bambi.unity-version="6000.2.11f1" \
  .

# Test locally
docker run --rm bambisleep-church:v1.0.0 npm test

# Deploy via git tag (CI/CD auto-pushes)
git tag -a v1.0.0 -m "🌸 Production release"
git push origin v1.0.0
```

### Unity Editor Shortcuts

- `Ctrl+Shift+F`: Search across all scripts
- `Ctrl+K`: Open Console for errors
- `F5`: Play mode (test multiplayer locally)
- `Ctrl+9`: Animation window
- `Ctrl+7`: Profiler (check performance)
- `Alt+Shift+C`: Create new C# script

### Debugging Workflow

1. **Unity errors**: Check `docs/DEBUGGING.md` (522 lines)
2. **UGS auth failures**: Verify init order (UnityServices → Auth → Economy)
3. **Network sync issues**: Check `IsOwner` before modifying NetworkVariables
4. **Animation not playing**: Verify StringToHash cached, check Animator
   transitions
5. **Test failures**: Run `npm run test:watch` to see live failures
6. **MCP not working**: Run `./scripts/mcp-validate.sh` (tests 8/8 servers)

---

## 📚 Essential File References

| Category          | File                      | Lines | Purpose                                 |
| ----------------- | ------------------------- | ----- | --------------------------------------- |
| **Unity C# Core** | CatgirlController.cs      | 327   | Avatar controller with NetworkBehaviour |
|                   | AudioManager.cs           | 342   | Singleton audio system                  |
|                   | InventorySystem.cs        | 270   | UGS Economy integration                 |
|                   | UniversalBankingSystem.cs | 371   | Multi-currency gambling/auctions        |
|                   | InventoryUI.cs            | 322   | UI Toolkit interface                    |
|                   | IPCBridge.cs              | 541   | Unity ↔ Node.js IPC                    |
| **Node.js**       | unity-bridge.js           | 129   | EventEmitter-based IPC bridge           |
| **Tests**         | unity-bridge.test.js      | 183   | Jest tests with real assertions         |
| **Documentation** | UNITY_SETUP_GUIDE.md      | 858   | Complete C# implementations             |
|                   | CATGIRL.md                | 683   | System architecture & XR specs          |
|                   | UNITY_IPC_PROTOCOL.md     | 430   | IPC message protocol                    |
|                   | DEBUGGING.md              | 522   | Complete debug reference                |
|                   | RELIGULOUS_MANTRA.md      | 300+  | Emoji conventions & philosophy          |

---

## 🦋 Final Notes for AI Agents

**This project is PRODUCTION-READY** with 7 complete Unity C# systems (2,491
lines). When extending:

1. **Read existing code FIRST** - All patterns are established in current
   scripts
2. **Follow emoji conventions** - 🦋 transformations, 🌸 packages, 💎 premium
   features
3. **Test immediately** - Run `npm test` after Node.js changes
4. **Use MCP servers** - Validate with `./scripts/mcp-validate.sh` (8/8
   operational)
5. **Maintain 80% coverage** - Jest enforces thresholds
6. **Always use BambiSleep™** - Trademark required in public content
7. **Cow powers are secret** - Easter eggs for Diablo homage
8. **Pink frilly maximalism** - Not typical enterprise code, embrace the
   aesthetic

**When stuck**: Check `docs/DEBUGGING.md` → Run MCP validate → Read scenario
examples → Ask about specific patterns

**Remember**: Documentation contains ACTUAL implementations to copy verbatim,
not generic advice. This guide is designed for immediate productivity with zero
ramp-up time.

🌸 Happy coding in the BambiSleep™ Church! 🌸
