#!/bin/bash
# 🌸 BambiSleep™ Unity Debug Environment Setup Script 🌸
# Automated setup for Unity 6.2 LTS debugging with VS Code

set -e

echo "🎮✨ BambiSleep™ Unity Debug Setup ✨🎮"
echo "═══════════════════════════════════════"
echo ""

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
PURPLE='\033[0;35m'
NC='\033[0m' # No Color

# Project paths
PROJECT_ROOT="/mnt/f/bambisleep-chat-catgirl"
UNITY_PROJECT="${PROJECT_ROOT}/catgirl-avatar-project"
VSCODE_DIR="${PROJECT_ROOT}/.vscode"

# Unity version
UNITY_VERSION="6000.2.11f1"

echo "📋 Step 1: Checking Prerequisites..."
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

# Check VS Code extensions
check_vscode_extension() {
    local extension=$1
    local name=$2
    if code --list-extensions | grep -q "$extension"; then
        echo -e "${GREEN}✓${NC} $name installed"
        return 0
    else
        echo -e "${RED}✗${NC} $name NOT installed"
        return 1
    fi
}

echo ""
echo "🔍 Checking VS Code Extensions:"
check_vscode_extension "visualstudiotoolsforunity.vstuc" "Visual Studio Tools for Unity"
check_vscode_extension "ms-dotnettools.csharp" "C# Extension"
check_vscode_extension "ms-dotnettools.csdevkit" "C# Dev Kit"

echo ""
echo "📁 Checking Project Structure:"
if [ -d "$UNITY_PROJECT" ]; then
    echo -e "${GREEN}✓${NC} Unity project directory exists"
else
    echo -e "${RED}✗${NC} Unity project directory NOT found at $UNITY_PROJECT"
    exit 1
fi

if [ -f "$UNITY_PROJECT/ProjectSettings/ProjectVersion.txt" ]; then
    echo -e "${GREEN}✓${NC} Unity project settings found"
    PROJECT_VERSION=$(grep "m_EditorVersion:" "$UNITY_PROJECT/ProjectSettings/ProjectVersion.txt" | awk '{print $2}')
    echo "   Version: $PROJECT_VERSION"
else
    echo -e "${RED}✗${NC} Unity project settings NOT found"
    exit 1
fi

echo ""
echo "🎯 Step 2: Verifying Unity C# Scripts..."
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

# Check all 6 C# systems
declare -a SCRIPTS=(
    "Assets/Scripts/Character/CatgirlController.cs"
    "Assets/Scripts/Economy/InventorySystem.cs"
    "Assets/Scripts/Economy/UniversalBankingSystem.cs"
    "Assets/Scripts/Networking/CatgirlNetworkManager.cs"
    "Assets/Scripts/UI/InventoryUI.cs"
    "Assets/Scripts/Audio/AudioManager.cs"
)

SCRIPTS_FOUND=0
for script in "${SCRIPTS[@]}"; do
    if [ -f "$UNITY_PROJECT/$script" ]; then
        echo -e "${GREEN}✓${NC} $(basename "$script")"
        SCRIPTS_FOUND=$((SCRIPTS_FOUND + 1))
    else
        echo -e "${RED}✗${NC} $(basename "$script") NOT FOUND"
    fi
done

echo ""
echo "   Total: $SCRIPTS_FOUND/6 C# scripts ready"

echo ""
echo "🔧 Step 3: Checking Unity Installation..."
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

# Check for Unity Hub
UNITY_HUB_FOUND=false
if command -v unity-hub &> /dev/null; then
    echo -e "${GREEN}✓${NC} Unity Hub found in PATH"
    UNITY_HUB_FOUND=true
elif [ -f "$HOME/Unity/Hub/Editor/$UNITY_VERSION/Editor/Unity" ]; then
    echo -e "${GREEN}✓${NC} Unity Editor found: $HOME/Unity/Hub/Editor/$UNITY_VERSION/Editor/Unity"
    UNITY_HUB_FOUND=true
elif [ -f "/opt/unity/Editor/Unity" ]; then
    echo -e "${GREEN}✓${NC} Unity Editor found: /opt/unity/Editor/Unity"
    UNITY_HUB_FOUND=true
else
    echo -e "${YELLOW}⚠${NC}  Unity Editor NOT found"
    echo ""
    echo "   Install Unity Hub from: https://unity.com/download"
    echo "   OR download Unity $UNITY_VERSION directly from:"
    echo "   https://unity.com/releases/editor/archive"
    echo ""
fi

# Check if Unity is currently running
if pgrep -x "Unity" > /dev/null; then
    echo -e "${GREEN}✓${NC} Unity Editor is RUNNING"
    UNITY_PID=$(pgrep -x "Unity")
    echo "   PID: $UNITY_PID"
else
    echo -e "${YELLOW}⚠${NC}  Unity Editor is NOT running"
    echo "   Launch Unity and open project: $UNITY_PROJECT"
fi

echo ""
echo "📚 Step 4: Generating Debug Helper Files..."
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

# Create breakpoint reference file
BREAKPOINTS_FILE="${PROJECT_ROOT}/BREAKPOINTS_REFERENCE.md"
cat > "$BREAKPOINTS_FILE" << 'BPEOF'
# 🐛 Recommended Breakpoints for BambiSleep™ CatGirl Systems

## Character System - CatgirlController.cs

### Initialization & Lifecycle
- **Line ~76**: `Start()` - Component initialization, cache references
- **Line ~87**: `Awake()` - Singleton pattern setup (if applicable)
- **Line ~112**: `OnNetworkSpawn()` - Network initialization, owner checks
- **Line ~128**: `OnNetworkDespawn()` - Cleanup, unsubscribe from events
- **Line ~150**: `Update()` - Per-frame movement and input processing

### Movement & Physics
- **Line ~165**: `HandleMovement()` - Movement input processing
- **Line ~180**: `ApplyGravity()` - Gravity calculations
- **Line ~195**: `HandleJump()` - Jump mechanics

### Network Synchronization
- **Line ~210**: `UpdatePositionServerRpc()` - Position sync to server
- **Line ~225**: `UpdateAnimationStateClientRpc()` - Animation sync to clients

## Economy System - InventorySystem.cs

### Item Management
- **Line ~89**: `AddItem()` - Item addition logic, slot finding
- **Line ~105**: `RemoveItem()` - Item removal, slot clearing
- **Line ~120**: `TransferItem()` - Item transfer between slots
- **Line ~135**: `CanAddItem()` - Inventory full check

### Unity Gaming Services Integration
- **Line ~150**: `InitializeEconomy()` - UGS Economy service setup
- **Line ~170**: `LoadPlayerInventory()` - Fetch inventory from cloud
- **Line ~185**: `SavePlayerInventory()` - Sync inventory to cloud

### Item Events
- **Line ~200**: `OnItemAdded()` - Event callback for item addition
- **Line ~215**: `OnInventoryChanged()` - General inventory change event

## Banking System - UniversalBankingSystem.cs

### Currency Operations
- **Line ~144**: `AddCurrency()` - Add currency to player balance
- **Line ~160**: `RemoveCurrency()` - Deduct currency from balance
- **Line ~175**: `TransferCurrency()` - Player-to-player currency transfer
- **Line ~190**: `GetBalance()` - Query current balance

### Gambling System
- **Line ~210**: `PlaceBet()` - Process gambling bet
- **Line ~230**: `ProcessGamblingResult()` - Calculate win/loss (5% house edge)
- **Line ~250**: `PayoutWinnings()` - Award winnings to player

### Auction System
- **Line ~270**: `CreateAuction()` - List item for auction
- **Line ~290**: `PlaceBidServerRpc()` - Process auction bid (server-authoritative)
- **Line ~310**: `EndAuction()` - Complete auction, transfer item/currency

### Network Synchronization
- **Line ~330**: `SyncBalanceClientRpc()` - Update balance on all clients
- **Line ~345**: `OnCurrencyChanged()` - NetworkVariable change callback

## Networking System - CatgirlNetworkManager.cs

### Connection Management
- **Line ~88**: `StartHost()` - Initialize host (server + client)
- **Line ~105**: `StartClient()` - Connect as client only
- **Line ~120**: `StartServer()` - Initialize dedicated server

### Relay & Lobby
- **Line ~140**: `CreateRelayAllocation()` - Unity Relay setup for NAT traversal
- **Line ~160**: `JoinRelayAllocation()` - Join existing Relay session
- **Line ~180**: `CreateLobby()` - Unity Lobby service integration
- **Line ~200**: `JoinLobby()` - Join existing lobby

### Connection Events
- **Line ~220**: `OnClientConnected()` - New client connected callback
- **Line ~235**: `OnClientDisconnected()` - Client disconnected callback
- **Line ~250**: `OnServerStarted()` - Server initialization complete

## UI System - InventoryUI.cs

### UI Initialization
- **Line ~95**: `RefreshInventory()` - Rebuild entire inventory UI
- **Line ~110**: `CreateSlotElement()` - Generate individual slot VisualElement
- **Line ~125**: `UpdateSlotDisplay()` - Update existing slot with new item

### User Interaction
- **Line ~145**: `OnSlotClicked()` - Handle slot click events
- **Line ~160**: `OnItemDragStarted()` - Begin drag-and-drop operation
- **Line ~175**: `OnItemDropped()` - Complete drag-and-drop, update inventory

### UI Toolkit Integration
- **Line ~195**: `BindUIElements()` - Query and cache VisualElements
- **Line ~210**: `SetupEventHandlers()` - Register UI event callbacks
- **Line ~225**: `UpdateCurrencyDisplay()` - Refresh currency text fields

## Audio System - AudioManager.cs

### Audio Playback
- **Line ~75**: `PlayMusic()` - Start music track with fade-in
- **Line ~95**: `PlaySoundEffect()` - Play one-shot SFX
- **Line ~110**: `PlayPurringSound()` - Catgirl-specific purring audio

### Audio Management
- **Line ~130**: `StopMusic()` - Stop current music with fade-out
- **Line ~145**: `SetMusicVolume()` - Adjust music volume
- **Line ~160**: `SetSFXVolume()` - Adjust sound effects volume

### Playlist System
- **Line ~180**: `NextTrack()` - Advance to next music track
- **Line ~195**: `ShufflePlaylist()` - Randomize playlist order
- **Line ~210**: `OnTrackCompleted()` - Handle track end event

## Cross-System Breakpoints

### Authentication Flow
1. `UniversalBankingSystem.cs:150` - UGS Auth initialization
2. `InventorySystem.cs:150` - Economy service initialization
3. `CatgirlNetworkManager.cs:88` - Network connection with auth

### Multiplayer Spawn Sequence
1. `CatgirlNetworkManager.cs:88` - Host starts
2. `CatgirlController.cs:112` - Player spawns (OnNetworkSpawn)
3. `InventorySystem.cs:170` - Load player inventory from cloud
4. `UniversalBankingSystem.cs:144` - Initialize player balance

### Item Purchase Flow
1. `InventoryUI.cs:145` - User clicks "Buy" button
2. `UniversalBankingSystem.cs:160` - Deduct currency
3. `InventorySystem.cs:89` - Add item to inventory
4. `InventoryUI.cs:95` - Refresh UI display

## Conditional Breakpoint Examples

```csharp
// Break only for server
IsServer == true

// Break only for specific item rarity (Legendary = 5)
item.rarity == 5

// Break only when low on currency
currentBalance < 100

// Break only for specific network client
OwnerClientId == 1

// Break when inventory full
items.Count >= maxSlots

// Break on high pink intensity
stats.pinkIntensity > 7.5f
```

## Watch Expression Examples

```csharp
// Character state
transform.position
stats.pinkIntensity
IsOwner
NetworkObject.NetworkObjectId

// Inventory
inventory.items.Count
inventory.items[0].displayName

// Currency
currentBalance["pinkCoins"]
currentBalance["cowTokens"]

// Network
NetworkManager.Singleton.IsServer
NetworkManager.Singleton.ConnectedClientsList.Count

// Audio
audioSource.isPlaying
currentTrackIndex
```

---

🌸 **Happy Debugging! Set these breakpoints and press F5!** 🌸
BPEOF

echo -e "${GREEN}✓${NC} Created: BREAKPOINTS_REFERENCE.md"

# Create quick launch script
LAUNCH_SCRIPT="${PROJECT_ROOT}/launch-unity-debug.sh"
cat > "$LAUNCH_SCRIPT" << 'LAUNCHEOF'
#!/bin/bash
# Quick launch Unity and attach debugger

echo "🎮 Launching Unity Editor..."

# Try to find Unity installation
UNITY_PATH=""

if [ -f "$HOME/Unity/Hub/Editor/6000.2.11f1/Editor/Unity" ]; then
    UNITY_PATH="$HOME/Unity/Hub/Editor/6000.2.11f1/Editor/Unity"
elif [ -f "/opt/unity/Editor/Unity" ]; then
    UNITY_PATH="/opt/unity/Editor/Unity"
else
    echo "❌ Unity Editor not found!"
    echo "   Install Unity 6000.2.11f1 first"
    exit 1
fi

# Launch Unity with project
PROJECT_PATH="/mnt/f/bambisleep-chat-catgirl/catgirl-avatar-project"

echo "   Unity: $UNITY_PATH"
echo "   Project: $PROJECT_PATH"
echo ""
echo "🚀 Starting Unity Editor..."

"$UNITY_PATH" -projectPath "$PROJECT_PATH" &

echo ""
echo "⏳ Waiting for Unity to initialize (30 seconds)..."
sleep 30

echo ""
echo "🐛 Now you can press F5 in VS Code to attach debugger!"
echo "   Or run: code . && F5"
LAUNCHEOF

chmod +x "$LAUNCH_SCRIPT"
echo -e "${GREEN}✓${NC} Created: launch-unity-debug.sh"

echo ""
echo "🎯 Step 5: Testing VS Code Configuration..."
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

if [ -f "$VSCODE_DIR/launch.json" ]; then
    echo -e "${GREEN}✓${NC} launch.json exists"

    # Check for Unity debug configurations
    if grep -q "Attach to Unity Editor and Play" "$VSCODE_DIR/launch.json"; then
        echo -e "${GREEN}✓${NC} 'Attach to Unity Editor and Play' configuration found"
    else
        echo -e "${RED}✗${NC} Missing Unity debug configuration"
    fi
else
    echo -e "${RED}✗${NC} launch.json NOT found"
fi

if [ -f "$VSCODE_DIR/tasks.json" ]; then
    echo -e "${GREEN}✓${NC} tasks.json exists"
else
    echo -e "${YELLOW}⚠${NC}  tasks.json not found (optional)"
fi

echo ""
echo "════════════════════════════════════════"
echo "✨ Setup Complete! Next Steps: ✨"
echo "════════════════════════════════════════"
echo ""
echo "1️⃣  Install Unity Editor if not installed:"
echo "   ${BLUE}https://unity.com/download${NC}"
echo ""
echo "2️⃣  Launch Unity and open project:"
echo "   ${BLUE}./launch-unity-debug.sh${NC}"
echo "   ${YELLOW}OR manually open: $UNITY_PROJECT${NC}"
echo ""
echo "3️⃣  In Unity, configure external editor:"
echo "   ${BLUE}Edit → Preferences → External Tools → Visual Studio Code${NC}"
echo ""
echo "4️⃣  In VS Code, press F5 to attach debugger"
echo "   ${BLUE}Debug Panel → 'Attach to Unity Editor and Play'${NC}"
echo ""
echo "📚 Reference files created:"
echo "   • ${GREEN}BREAKPOINTS_REFERENCE.md${NC} - All recommended breakpoints"
echo "   • ${GREEN}launch-unity-debug.sh${NC} - Quick Unity launcher"
echo "   • ${GREEN}docs/guides/UNITY_DEBUG_QUICK_START.md${NC} - Full guide"
echo ""
echo "🌸 NYAN NYAN NYAN! Happy debugging! 🌸"
