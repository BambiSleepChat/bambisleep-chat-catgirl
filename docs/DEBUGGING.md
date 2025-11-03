# 🐛 BambiSleep™ Unity Debugging Guide

> Complete debugging reference for Unity 6.2 LTS + VS Code development

---

## ⚡ Quick Start

### Prerequisites

- Unity Editor 6000.2.11f1 installed
- Unity project opened in Editor
- VS Code with Unity extensions active (`visualstudiotoolsforunity.vstuc`)

### 3-Step Debug Process

```bash
# 1. Launch Unity (if not running)
/opt/unity/Editor/Unity -projectPath /mnt/f/bambisleep-chat-catgirl/catgirl-avatar-project

# 2. In VS Code: Press F5
# OR: Debug Panel → "Attach to Unity Editor and Play"

# 3. Set breakpoints in C# files and debug!
```

---

## 🎯 Recommended Breakpoint Locations

### Character System - CatgirlController.cs

| Line | Method                            | Purpose                                    |
| ---- | --------------------------------- | ------------------------------------------ |
| ~76  | `Start()`                         | Component initialization, cache references |
| ~87  | `Awake()`                         | Singleton pattern setup                    |
| ~112 | `OnNetworkSpawn()`                | Network initialization, owner checks       |
| ~128 | `OnNetworkDespawn()`              | Cleanup, unsubscribe from events           |
| ~150 | `Update()`                        | Per-frame movement and input processing    |
| ~165 | `HandleMovement()`                | Movement input processing                  |
| ~180 | `ApplyGravity()`                  | Gravity calculations                       |
| ~195 | `HandleJump()`                    | Jump mechanics                             |
| ~210 | `UpdatePositionServerRpc()`       | Position sync to server                    |
| ~225 | `UpdateAnimationStateClientRpc()` | Animation sync to clients                  |

### Economy System - InventorySystem.cs

| Line | Method                  | Purpose                           |
| ---- | ----------------------- | --------------------------------- |
| ~89  | `AddItem()`             | Item addition logic, slot finding |
| ~105 | `RemoveItem()`          | Item removal, slot clearing       |
| ~120 | `TransferItem()`        | Item transfer between slots       |
| ~135 | `CanAddItem()`          | Inventory full check              |
| ~150 | `InitializeEconomy()`   | UGS Economy service setup         |
| ~170 | `LoadPlayerInventory()` | Fetch inventory from cloud        |
| ~185 | `SavePlayerInventory()` | Sync inventory to cloud           |
| ~200 | `OnItemAdded()`         | Event callback for item addition  |

### Banking System - UniversalBankingSystem.cs

| Line | Method                    | Purpose                                    |
| ---- | ------------------------- | ------------------------------------------ |
| ~144 | `AddCurrency()`           | Add currency to player balance             |
| ~160 | `RemoveCurrency()`        | Deduct currency from balance               |
| ~175 | `TransferCurrency()`      | Player-to-player currency transfer         |
| ~190 | `GetBalance()`            | Query current balance                      |
| ~210 | `PlaceBet()`              | Process gambling bet                       |
| ~230 | `ProcessGamblingResult()` | Calculate win/loss (5% house edge)         |
| ~250 | `PayoutWinnings()`        | Award winnings to player                   |
| ~270 | `CreateAuction()`         | List item for auction                      |
| ~290 | `PlaceBidServerRpc()`     | Process auction bid (server-authoritative) |
| ~310 | `EndAuction()`            | Complete auction, transfer item/currency   |
| ~330 | `SyncBalanceClientRpc()`  | Update balance on all clients              |

### Networking System - CatgirlNetworkManager.cs

| Line | Method                    | Purpose                             |
| ---- | ------------------------- | ----------------------------------- |
| ~88  | `StartHost()`             | Initialize host (server + client)   |
| ~105 | `StartClient()`           | Connect as client only              |
| ~120 | `StartServer()`           | Initialize dedicated server         |
| ~140 | `CreateRelayAllocation()` | Unity Relay setup for NAT traversal |
| ~160 | `JoinRelayAllocation()`   | Join existing Relay session         |
| ~180 | `CreateLobby()`           | Unity Lobby service integration     |
| ~200 | `JoinLobby()`             | Join existing lobby                 |
| ~220 | `OnClientConnected()`     | New client connected callback       |
| ~235 | `OnClientDisconnected()`  | Client disconnected callback        |

### UI System - InventoryUI.cs

| Line | Method                 | Purpose                                  |
| ---- | ---------------------- | ---------------------------------------- |
| ~95  | `RefreshInventory()`   | Rebuild entire inventory UI              |
| ~110 | `CreateSlotElement()`  | Generate individual slot VisualElement   |
| ~125 | `UpdateSlotDisplay()`  | Update existing slot with new item       |
| ~145 | `OnSlotClicked()`      | Handle slot click events                 |
| ~160 | `OnItemDragStarted()`  | Begin drag-and-drop operation            |
| ~175 | `OnItemDropped()`      | Complete drag-and-drop, update inventory |
| ~195 | `BindUIElements()`     | Query and cache VisualElements           |
| ~210 | `SetupEventHandlers()` | Register UI event callbacks              |

### Audio System - AudioManager.cs

| Line | Method               | Purpose                        |
| ---- | -------------------- | ------------------------------ |
| ~75  | `PlayMusic()`        | Start music track with fade-in |
| ~95  | `PlaySoundEffect()`  | Play one-shot SFX              |
| ~110 | `PlayPurringSound()` | Catgirl-specific purring audio |
| ~125 | `StopMusic()`        | Stop music with fade-out       |
| ~140 | `SetVolume()`        | Adjust volume levels           |

---

## ⌨️ Keyboard Shortcuts

### Essential Debugging

| Shortcut          | Action                | Description                                |
| ----------------- | --------------------- | ------------------------------------------ |
| **F5**            | Start Debugging       | Attach to Unity Editor and start Play mode |
| **Ctrl+F5**       | Run Without Debugging | Launch without breakpoints                 |
| **Shift+F5**      | Stop Debugging        | Detach debugger and stop Play mode         |
| **Ctrl+Shift+F5** | Restart Debugging     | Restart debug session                      |
| **F6**            | Pause Execution       | Pause the running code                     |

### Code Navigation

| Shortcut           | Action                 | Description                                   |
| ------------------ | ---------------------- | --------------------------------------------- |
| **F10**            | Step Over              | Execute current line, skip function internals |
| **F11**            | Step Into              | Step into function/method call                |
| **Shift+F11**      | Step Out               | Step out of current function                  |
| **Ctrl+Shift+F10** | Run to Cursor          | Execute until cursor position                 |
| **F9**             | Toggle Breakpoint      | Add/remove breakpoint at current line         |
| **Ctrl+Shift+F9**  | Remove All Breakpoints | Clear all breakpoints in project              |

### Breakpoint Management

| Shortcut                   | Action                    | Description                             |
| -------------------------- | ------------------------- | --------------------------------------- |
| **F9**                     | Toggle Breakpoint         | Add or remove breakpoint                |
| **Ctrl+K Ctrl+B**          | Enable/Disable Breakpoint | Keep breakpoint but disable temporarily |
| **Right-click → Edit**     | Conditional Breakpoint    | Set condition or hit count              |
| **Right-click → Logpoint** | Add Logpoint              | Log message without stopping            |

### VS Code General

| Shortcut         | Action              | Description                   |
| ---------------- | ------------------- | ----------------------------- |
| **Ctrl+P**       | Quick Open          | Open file by name             |
| **Ctrl+Shift+F** | Find in Files       | Search across entire project  |
| **Ctrl+T**       | Go to Symbol        | Jump to class/method/variable |
| **F12**          | Go to Definition    | Jump to symbol definition     |
| **Alt+F12**      | Peek Definition     | View definition inline        |
| **Shift+F12**    | Find All References | See all usages of symbol      |
| **Ctrl+G**       | Go to Line          | Jump to specific line number  |
| **Ctrl+B**       | Toggle Sidebar      | Show/hide file explorer       |
| **Ctrl+J**       | Toggle Panel        | Show/hide terminal/output     |
| **Ctrl+Shift+D** | Debug View          | Open debug panel              |
| **Ctrl+\`**      | Toggle Terminal     | Open integrated terminal      |

### Unity Editor (When Focused)

| Shortcut         | Action            | Description                      |
| ---------------- | ----------------- | -------------------------------- |
| **Ctrl+P**       | Play/Pause        | Start or pause Play mode         |
| **Ctrl+Shift+P** | Step Frame        | Advance one frame while paused   |
| **Ctrl+R**       | Recompile Scripts | Force script recompilation       |
| **F**            | Frame Selected    | Focus camera on selected object  |
| **Q/W/E/R/T**    | Tools             | Pan/Move/Rotate/Scale/Rect tools |

---

## 🚨 Troubleshooting

### Issue 1: VS Code Can't Find Unity Process

**Symptoms:**

- "No Unity process found" when pressing F5
- Debug panel shows "Could not connect to Unity"
- Breakpoints show hollow circle (not bound)

**Solutions:**

```bash
# 1. Verify Unity Editor is running
ps aux | grep Unity

# 2. Check Unity is in Play mode (CRITICAL)
# Debugger only attaches when Play mode is active

# 3. Verify Unity External Editor setting
# Unity → Edit → Preferences → External Tools
# External Script Editor: Visual Studio Code

# 4. Regenerate .csproj files
# Unity → Edit → Preferences → External Tools
# Check "Generate .csproj files for: Embedded packages, Local packages, Built-in packages"
# Click "Regenerate project files"

# 5. Restart Unity Editor AND VS Code
# Close both → Open Unity first → Then VS Code

# 6. Check vstuc extension is enabled
code --list-extensions | grep vstuc
# Should show: visualstudiotoolsforunity.vstuc
```

**Root Cause:** Unity must be in Play mode with External Script Editor
configured before VS Code can attach.

---

### Issue 2: Breakpoints Not Hitting

**Symptoms:**

- Breakpoints set but never trigger
- Code executes but debugger doesn't pause
- Breakpoint icon shows ⚠️ warning

**Solutions:**

```csharp
// 1. Verify script is attached to GameObject
// In Unity Hierarchy → Select GameObject → Inspector → Check script component

// 2. Check script compilation errors
// Unity → Console (Ctrl+Shift+C) → Look for errors
// Scripts with errors won't execute

// 3. Ensure breakpoint is on executable line
// ❌ DON'T set on: namespace, class declaration, comments, empty lines
// ✅ DO set on: method calls, variable assignments, conditionals

// Example - Bad breakpoint locations:
namespace BambiSleep.CatGirl.Character {  // ❌ Won't hit
    public class CatgirlController : NetworkBehaviour {  // ❌ Won't hit
        // This is a comment  // ❌ Won't hit

        private float speed = 5.0f;  // ✅ Will hit (if Start() executes)

        void Start() {  // ❌ Won't hit (method declaration)
            Debug.Log("Starting");  // ✅ Will hit (executable code)
        }
    }
}

// 4. Check script execution order
// Unity → Edit → Project Settings → Script Execution Order
// Ensure CatgirlController executes before other dependent scripts

// 5. Verify NetworkBehaviour ownership
if (!IsOwner) {
    // ❌ This code won't run if you're not the owner
    // Set breakpoint AFTER ownership check
    return;
}
Debug.Log("Owner code");  // ✅ Set breakpoint here

// 6. Check conditional breakpoints
// Right-click breakpoint → Edit → Check condition syntax
// Example: currentSpeed > 0.5f (valid)
// Example: currentSpeed > 0,5f (invalid - use . not ,)
```

**Root Cause:** Script not executing, wrong line selected, or
ownership/conditional logic preventing execution.

---

### Issue 3: Unity Gaming Services (UGS) Authentication Fails

**Symptoms:**

- `AuthenticationException` in console
- Economy/Lobby services unavailable
- "Player ID not found" errors

**Critical Order:**

```csharp
// MUST initialize in this exact order:
private async void Start()
{
    try
    {
        // 1. Core services first
        await UnityServices.InitializeAsync();

        // 2. Auth second
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log($"✅ Player ID: {AuthenticationService.Instance.PlayerId}");

        // 3. Economy/Lobby last
        await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
    }
    catch (AuthenticationException e)
    {
        Debug.LogError($"❌ Auth failed: {e.Message}");
        // Fallback to offline mode
        InitializeOfflineBalance();
    }
}
```

**Verification:**

- Unity → Edit → Project Settings → Services
- Check Project ID matches dashboard
- Verify Environment set to "production" or "development"
- Confirm Economy items configured in UGS Dashboard

---

### Issue 4: NetworkBehaviour Ownership Issues

**Symptoms:**

- NetworkVariables not updating
- ServerRpc not executing
- "Not owner" errors in console

**Ownership Rules:**

```csharp
// ✅ Correct pattern
public override void OnNetworkSpawn()
{
    if (IsOwner)
    {
        // Only owner can modify NetworkVariables on client
        localSpeed.Value = 5.0f;
    }

    // Subscribe to NetworkVariable changes (any client)
    networkSpeed.OnValueChanged += OnSpeedChanged;
}

// ✅ Server-authoritative actions
[ServerRpc]
private void ProcessBidServerRpc(long amount, ServerRpcParams rpcParams = default)
{
    // Server validates and processes
    if (amount > playerBalance)
    {
        Debug.LogWarning("Insufficient funds");
        return;
    }

    // Update NetworkVariable (server has authority)
    playerBalance.Value -= amount;
}

// ❌ WRONG: Client trying to modify without ownership
private void Update()
{
    if (!IsOwner) // This check helps, but...
        return;

    // This only works for owner, fails for other clients observing
    someNetworkVariable.Value = newValue;
}
```

**Debugging:**

- Set breakpoint in `OnNetworkSpawn()`
- Check `IsOwner`, `IsClient`, `IsServer` values
- Verify `NetworkObject` component attached to GameObject
- Check Network Manager has correct prefabs registered

---

### Issue 5: Animator Performance Issues

**Symptoms:**

- Low FPS with many catgirls on screen
- Stuttering animation playback
- High CPU usage in profiler

**Optimization:**

```csharp
// ✅ DO THIS: Cache animator hash IDs
private static readonly int Speed = Animator.StringToHash("Speed");
private static readonly int IsPurring = Animator.StringToHash("IsPurring");

private void Update()
{
    animator.SetFloat(Speed, currentSpeed);  // Fast hash lookup
    animator.SetBool(IsPurring, true);
}

// ❌ DON'T DO THIS: String lookups every frame
private void Update()
{
    animator.SetFloat("Speed", currentSpeed);  // Slow string lookup
    animator.SetBool("IsPurring", true);
}

// ✅ Culling for distant objects
private void Update()
{
    if (!isVisibleToCamera)
    {
        animator.cullingMode = AnimatorCullingMode.CullCompletely;
        return;
    }

    animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
    UpdateAnimations();
}
```

**Profiling:**

- Window → Analysis → Profiler
- Check CPU Usage → Scripts
- Look for `Animator.Update` spikes
- Verify culling mode set correctly

---

### Issue 6: UI Toolkit Not Updating

**Symptoms:**

- InventoryUI not refreshing
- Currency display shows old values
- Drag-and-drop not working

**UI Toolkit Debugging:**

```csharp
// 1. Verify UXML/USS loaded
private void OnEnable()
{
    var root = GetComponent<UIDocument>().rootVisualElement;

    if (root == null)
    {
        Debug.LogError("❌ Root VisualElement is null!");
        return;
    }

    // 2. Query elements with null checks
    var inventoryContainer = root.Q<VisualElement>("inventory-container");
    if (inventoryContainer == null)
    {
        Debug.LogError("❌ 'inventory-container' not found in UXML!");
        return;
    }

    Debug.Log($"✅ Found inventory container with {inventoryContainer.childCount} children");
}

// 3. Force UI refresh
public void RefreshInventory()
{
    // Clear existing
    inventoryContainer.Clear();

    // Rebuild from data
    foreach (var item in inventorySystem.GetAllItems())
    {
        var slotElement = CreateSlotElement(item);
        inventoryContainer.Add(slotElement);
    }

    // Force layout recalculation
    inventoryContainer.MarkDirtyRepaint();
}
```

**Verification:**

- Check UXML file exists in `Assets/UI/` directory
- Verify element names match UXML structure
- Inspector → UIDocument component → Panel Settings configured
- Console shows no USS parsing errors

---

## 🔧 Debug Configurations

### 1. Attach to Unity Editor

- Attaches debugger to running Unity instance
- Does NOT auto-start Play mode
- **Use for:** Inspecting Editor code, custom tools

### 2. Attach to Unity Editor and Play (Recommended)

- Attaches debugger AND starts Play mode
- **Use for:** Testing runtime behavior, NetworkBehaviour

### 3. .NET Core Attach

- Manual process selection
- **Use for:** Advanced scenarios, non-Unity .NET processes

---

## 📊 Debug Panel Features

### Variables

- **Locals**: Variables in current scope
- **Statics**: Static class members (AudioManager.Instance, etc.)
- **This**: Current object instance (CatgirlController properties)

### Watch Expressions

Add expressions to monitor:

```csharp
// Example watch expressions:
currentSpeed
IsOwner
NetworkManager.Singleton.IsServer
inventorySystem.itemCount
animator.GetFloat(Speed)
```

### Call Stack

- Shows execution path to current breakpoint
- Click frames to navigate between methods
- See parameter values for each stack frame

---

## 🎯 Best Practices

1. **Set strategic breakpoints**: Focus on system boundaries (OnNetworkSpawn,
   Start, Update)
2. **Use conditional breakpoints**: `currentSpeed > 5.0f` instead of breaking
   every frame
3. **Log liberally**: `Debug.Log($"✅ {message}")` for quick traces
4. **Test one system at a time**: Isolate Character → Economy → Networking → UI
5. **Use Unity Profiler**: Window → Analysis → Profiler for performance issues
6. **Check Console first**: Many issues show error messages before debugging
   needed
7. **Restart when stuck**: Unity Editor + VS Code fresh start often resolves
   issues

---

## 📚 Additional Resources

- Unity Manual: <https://docs.unity3d.com/Manual/>
- Netcode for GameObjects: <https://docs-multiplayer.unity3d.com/>
- Unity Gaming Services: <https://docs.unity.com/ugs/>
- VS Code Unity Extension: <https://code.visualstudio.com/docs/other/unity>
