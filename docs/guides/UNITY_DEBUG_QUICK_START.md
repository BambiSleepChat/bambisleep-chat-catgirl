# 🐛 Unity Debug Quick Start Guide

## ⚡ Quick Launch (Once Unity Installed)

### Prerequisites
- Unity Editor 6000.2.11f1 installed
- Unity project opened in Editor
- VS Code with Unity extensions active

### 3-Step Debug Process

```bash
# 1. Launch Unity (if not running)
/opt/unity/Editor/Unity -projectPath /mnt/f/bambisleep-chat-catgirl/catgirl-avatar-project

# 2. In VS Code: Press F5
# OR: Debug Panel → "Attach to Unity Editor and Play"

# 3. Set breakpoints in C# files and debug!
```

## 🎯 Recommended Breakpoint Locations

| File | Line | Method | Purpose |
|------|------|--------|---------|
| `CatgirlController.cs` | ~76 | `Start()` | Component initialization |
| `CatgirlController.cs` | ~112 | `OnNetworkSpawn()` | Network spawning |
| `CatgirlController.cs` | ~150 | `Update()` | Per-frame updates |
| `InventorySystem.cs` | ~89 | `AddItem()` | Item addition logic |
| `InventorySystem.cs` | ~120 | `RemoveItem()` | Item removal |
| `UniversalBankingSystem.cs` | ~144 | `AddCurrency()` | Economy operations |
| `UniversalBankingSystem.cs` | ~200 | `TransferCurrency()` | Player-to-player transfers |
| `CatgirlNetworkManager.cs` | ~88 | `StartHost()` | Host networking setup |
| `AudioManager.cs` | ~75 | `PlayMusic()` | Audio playback |
| `InventoryUI.cs` | ~95 | `RefreshInventory()` | UI updates |

## 🔧 Debug Configurations Available

### 1. Attach to Unity Editor
- Attaches debugger to running Unity instance
- Does NOT auto-start Play mode
- Use for: Inspecting Editor code, custom tools

### 2. Attach to Unity Editor and Play
- Attaches debugger AND starts Play mode
- **Recommended for gameplay debugging**
- Use for: Testing runtime behavior, NetworkBehaviour

### 3. .NET Core Attach
- Manual process selection
- Use for: Advanced scenarios, non-Unity .NET processes

## 🚨 Common Issues & Fixes

### Issue: "No Unity process found"
```bash
# Check if Unity is running
ps aux | grep Unity | grep -v grep

# If not running, launch Unity first
```

### Issue: "Breakpoints not hitting"
1. Verify Unity Console shows no compilation errors
2. Check Debug symbols enabled in Unity Project Settings
3. Ensure breakpoint is on executable line (not comment/blank)
4. Try: Unity → Assets → Reimport All

### Issue: "Debugger attaches but Play mode doesn't start"
1. Check Unity Console for errors
2. Verify Network Manager is in scene
3. Try: Edit → Project Settings → Player → Scripting Define Symbols

### Issue: "VS Code shows outdated code"
1. Unity → Edit → Preferences → External Tools
2. Click: "Regenerate project files"
3. Restart VS Code

## ⌨️ Keyboard Shortcuts

| Action | Shortcut | Description |
|--------|----------|-------------|
| Start Debugging | `F5` | Attach to Unity and Play |
| Stop Debugging | `Shift+F5` | Detach debugger |
| Restart | `Ctrl+Shift+F5` | Restart debug session |
| Step Over | `F10` | Execute current line |
| Step Into | `F11` | Step into method |
| Step Out | `Shift+F11` | Exit current method |
| Continue | `F5` | Resume execution |
| Toggle Breakpoint | `F9` | Add/remove breakpoint |

## 📊 Debug Panel Features

### Variables
- **Locals**: Variables in current scope
- **Statics**: Static class members (AudioManager.Instance, etc.)
- **This**: Current object instance (CatgirlController properties)

### Watch
Add expressions to monitor:
```csharp
// Examples:
stats.pinkIntensity
NetworkManager.Singleton.IsServer
inventory.items.Count
currentBalance["pinkCoins"]
```

### Call Stack
- Shows method execution hierarchy
- Click frames to navigate code
- Useful for understanding NetworkBehaviour call order

## 💡 Pro Tips

### Conditional Breakpoints
Right-click breakpoint → "Edit Breakpoint" → Add condition:
```csharp
// Only break when specific condition true
item.rarity == 5  // Legendary items only
IsServer == true  // Server-side only
pinkIntensity > 5.0f  // High intensity only
```

### Log Points
Right-click → "Add Logpoint" → Enter message:
```csharp
Player {PlayerId} spawned at position {transform.position}
```

### Data Breakpoints (Advanced)
Watch for changes to specific variable:
```csharp
// Break when variable changes, not just on line execution
```

### Debug Console Evaluation
While paused, evaluate expressions in Debug Console:
```csharp
// Check values on-the-fly
transform.position
NetworkObject.NetworkObjectId
inventory.items[0].displayName
```

## 🌸 BambiSleep™ Specific Debugging

### NetworkBehaviour Debugging
```csharp
// Check network state
NetworkManager.Singleton.IsHost
NetworkManager.Singleton.IsClient
NetworkManager.Singleton.IsServer
NetworkObject.IsOwner

// Log network events
[ServerRpc]
public void MyServerRpc() {
    Debug.Log($"ServerRpc called by client {OwnerClientId}");
    // Breakpoint here
}
```

### Unity Gaming Services Debugging
```csharp
// Check authentication
AuthenticationService.Instance.IsSignedIn
AuthenticationService.Instance.PlayerId

// Economy balance
await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
// Breakpoint after await to inspect result
```

### Async/Await Debugging
```csharp
// Set breakpoint AFTER await to see result
var result = await SomeAsyncMethod();
// Breakpoint here to inspect result
```

## 📚 Additional Resources

- [Unity C# Debugging Docs](https://docs.unity3d.com/Manual/ManagedCodeDebugging.html)
- [VS Code Unity Extension](https://marketplace.visualstudio.com/items?itemName=visualstudiotoolsforunity.vstuc)
- [Netcode for GameObjects Debugging](https://docs-multiplayer.unity3d.com/netcode/current/learn/debugging)
- Project-specific: `docs/development/UNITY_SETUP_GUIDE.md`

---

🌈 **Happy Debugging! NYAN NYAN NYAN!** 🌈
