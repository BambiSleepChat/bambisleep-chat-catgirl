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
