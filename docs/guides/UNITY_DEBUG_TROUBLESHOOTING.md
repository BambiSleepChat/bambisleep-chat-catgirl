# 🐛 BambiSleep™ Unity Debugging Troubleshooting Guide

## 🚨 Common Issues & Solutions

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

**Root Cause:** Unity must be in Play mode with External Script Editor configured before VS Code can attach.

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

**Root Cause:** Script not executing, wrong line selected, or ownership/conditional logic preventing execution.

---

### Issue 3: Unity Gaming Services (UGS) Authentication Fails

**Symptoms:**
- `AuthenticationException` during SignInAnonymouslyAsync()
- Economy service returns 401 errors
- "Player not authenticated" messages in console

**Debugging Steps:**

```csharp
// Add extensive logging to UniversalBankingSystem.cs Start()
private async void Start()
{
    Debug.Log("🏦 Starting UGS initialization...");
    
    try {
        // BREAKPOINT HERE - verify initialization starts
        Debug.Log("📡 Initializing UnityServices...");
        await UnityServices.InitializeAsync();
        Debug.Log($"✅ UnityServices initialized: {UnityServices.State}");
        
        // BREAKPOINT HERE - check services state
        Debug.Log("🔐 Starting authentication...");
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log($"✅ Authenticated: {AuthenticationService.Instance.PlayerId}");
        
        // BREAKPOINT HERE - verify player ID
        Debug.Log("💰 Loading balances...");
        var balances = await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
        Debug.Log($"✅ Loaded {balances.Balances.Count} currencies");
        
        // BREAKPOINT HERE - inspect balances
        foreach (var balance in balances.Balances) {
            Debug.Log($"  - {balance.CurrencyId}: {balance.Balance}");
        }
    }
    catch (AuthenticationException e) {
        Debug.LogError($"❌ Auth failed: {e.ErrorCode} - {e.Message}");
        // BREAKPOINT HERE - inspect exception details
        Debug.LogError($"Stack: {e.StackTrace}");
    }
    catch (Exception e) {
        Debug.LogError($"❌ UGS error: {e.Message}");
        // BREAKPOINT HERE - catch-all for other errors
    }
}

// Watch expressions to add:
// - UnityServices.State (should be "Initialized")
// - AuthenticationService.Instance.IsSignedIn (should be true)
// - AuthenticationService.Instance.PlayerId (should be GUID)
```

**Common Causes:**

1. **Project ID not configured:**
   ```bash
   # Check Unity project services config
   cat catgirl-avatar-project/ProjectSettings/Services/com.unity.services.core/UserProjectSettings.asset
   # Should contain valid Project ID
   ```

2. **Network connectivity issues:**
   ```bash
   # Test Unity Services reachability
   curl -I https://services.unity.com
   # Should return 200 OK
   ```

3. **Wrong environment (dev vs prod):**
   ```csharp
   // In Unity Dashboard, ensure environment matches
   // Default: production
   // Check: Economy items exist in same environment
   ```

4. **Async timing issues:**
   ```csharp
   // ❌ WRONG - Don't call Economy before Auth completes
   async void Start() {
       _ = UnityServices.InitializeAsync();  // Fire-and-forget
       _ = AuthenticationService.Instance.SignInAnonymouslyAsync();
       await EconomyService.Instance.PlayerBalances.GetBalancesAsync();  // FAILS
   }
   
   // ✅ CORRECT - Sequential awaits
   async void Start() {
       await UnityServices.InitializeAsync();
       await AuthenticationService.Instance.SignInAnonymouslyAsync();
       await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
   }
   ```

---

### Issue 4: NetworkBehaviour RPC Not Working

**Symptoms:**
- `[ServerRpc]` methods never execute
- `[ClientRpc]` not received by clients
- "Rpc called before object was spawned" errors

**Debugging Steps:**

```csharp
// In CatgirlController.cs - Add extensive logging

[ServerRpc]
private void UpdatePositionServerRpc(Vector3 position)
{
    Debug.Log($"🦋 ServerRpc called by ClientId: {OwnerClientId}");
    // BREAKPOINT HERE - verify RPC reaches server
    
    if (!NetworkManager.Singleton.IsServer) {
        Debug.LogError("❌ ServerRpc executing on non-server!");
        return;
    }
    
    Debug.Log($"✅ Server processing position: {position}");
    networkPosition.Value = position;
    // BREAKPOINT HERE - verify NetworkVariable update
}

[ClientRpc]
private void TriggerEmoteClientRpc(string emoteName)
{
    Debug.Log($"🎭 ClientRpc received on ClientId: {NetworkManager.Singleton.LocalClientId}");
    // BREAKPOINT HERE - verify RPC reaches clients
    
    if (IsOwner) {
        Debug.Log("📍 RPC received by owner");
    } else {
        Debug.Log("👁️ RPC received by observer");
    }
    
    PlayEmoteAnimation(emoteName);
}

// Watch expressions:
// - NetworkManager.Singleton.IsServer
// - NetworkManager.Singleton.IsClient
// - NetworkManager.Singleton.IsConnectedClient
// - IsOwner
// - OwnerClientId
// - NetworkObject.IsSpawned
```

**Common Causes:**

1. **NetworkObject not spawned:**
   ```csharp
   // Check in OnNetworkSpawn()
   public override void OnNetworkSpawn()
   {
       Debug.Log($"✅ NetworkObject spawned: {NetworkObjectId}");
       // BREAKPOINT HERE - verify spawn completed
       base.OnNetworkSpawn();
   }
   ```

2. **Wrong ownership for ServerRpc:**
   ```csharp
   // By default, ServerRpc requires ownership
   [ServerRpc]  // ❌ Only owner can call
   void DoSomethingServerRpc() { }
   
   [ServerRpc(RequireOwnership = false)]  // ✅ Anyone can call
   void DoSomethingServerRpc() { }
   ```

3. **Calling RPC before network spawn:**
   ```csharp
   void Start() {
       // ❌ WRONG - NetworkObject not spawned yet
       UpdatePositionServerRpc(transform.position);
   }
   
   public override void OnNetworkSpawn() {
       // ✅ CORRECT - Called after spawn
       if (IsOwner) {
           UpdatePositionServerRpc(transform.position);
       }
   }
   ```

4. **NetworkManager not started:**
   ```csharp
   // In CatgirlNetworkManager.cs - Verify network started
   void Start()
   {
       if (NetworkManager.Singleton == null) {
           Debug.LogError("❌ NetworkManager.Singleton is null!");
           return;
       }
       
       Debug.Log($"NetworkManager state: " +
                 $"Server={NetworkManager.Singleton.IsServer}, " +
                 $"Client={NetworkManager.Singleton.IsClient}");
       // BREAKPOINT HERE - check network state
   }
   ```

---

### Issue 5: Animator Not Updating

**Symptoms:**
- `SetFloat()` called but animation doesn't change
- Animator parameters show wrong values in Unity
- Character stuck in default pose

**Debugging Steps:**

```csharp
// In CatgirlController.cs - Add animator validation

private void UpdateAnimations()
{
    // BREAKPOINT HERE - start of animation update
    
    if (animator == null) {
        Debug.LogError("❌ Animator component is null!");
        return;
    }
    
    if (!animator.isInitialized) {
        Debug.LogError("❌ Animator not initialized!");
        return;
    }
    
    // Verify hash IDs are valid
    if (Speed == 0) {
        Debug.LogError("❌ Speed hash ID is 0 - parameter doesn't exist!");
    }
    
    float currentSpeed = velocity.magnitude;
    Debug.Log($"🏃 Setting Speed parameter to: {currentSpeed}");
    animator.SetFloat(Speed, currentSpeed);
    // BREAKPOINT HERE - after SetFloat
    
    // Verify parameter was set
    float actualSpeed = animator.GetFloat(Speed);
    if (Mathf.Abs(actualSpeed - currentSpeed) > 0.01f) {
        Debug.LogWarning($"⚠️ Speed mismatch! Set: {currentSpeed}, Got: {actualSpeed}");
    }
    
    Debug.Log($"😺 Setting IsPurring to: {isPurring}");
    animator.SetBool(IsPurring, isPurring);
    // BREAKPOINT HERE - after SetBool
}

// Watch expressions:
// - animator (should not be null)
// - animator.isInitialized (should be true)
// - animator.runtimeAnimatorController (should not be null)
// - Speed (hash ID should be non-zero)
// - animator.GetFloat(Speed) (verify parameter value)
```

**Common Causes:**

1. **Animator component missing:**
   ```bash
   # In Unity Inspector → CatGirl GameObject
   # Verify "Animator" component exists
   # Verify "Controller" field has animator controller assigned
   ```

2. **Wrong parameter names:**
   ```csharp
   // ❌ WRONG - Typo in parameter name
   private static readonly int Speed = Animator.StringToHash("Speeed");
   
   // ✅ CORRECT - Must match animator controller parameter exactly
   private static readonly int Speed = Animator.StringToHash("Speed");
   
   // Verify parameter exists in animator controller:
   // Unity → Project → Animator Controller → Parameters tab
   ```

3. **Animator culling:**
   ```csharp
   // Check if animator is culled when off-screen
   if (animator.cullingMode == AnimatorCullingMode.CullCompletely) {
       Debug.LogWarning("⚠️ Animator culled - won't update!");
       animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
   }
   ```

4. **Layer weight issues:**
   ```csharp
   // If using animator layers, check weights
   for (int i = 0; i < animator.layerCount; i++) {
       float weight = animator.GetLayerWeight(i);
       Debug.Log($"Layer {i} weight: {weight}");
       // BREAKPOINT HERE - verify layer weights
   }
   ```

---

### Issue 6: InventoryUI Not Updating

**Symptoms:**
- UI shows old item data
- AddItem() called but grid doesn't refresh
- Slots show empty when items exist

**Debugging Steps:**

```csharp
// In InventoryUI.cs - Add UI update logging

public void RefreshInventory()
{
    Debug.Log("🎨 RefreshInventory called");
    // BREAKPOINT HERE - start of refresh
    
    if (inventorySystem == null) {
        Debug.LogError("❌ InventorySystem reference is null!");
        return;
    }
    
    var items = inventorySystem.GetAllItems();
    Debug.Log($"📦 Inventory has {items.Count} items");
    // BREAKPOINT HERE - inspect items list
    
    if (itemSlots == null || itemSlots.Count == 0) {
        Debug.LogError("❌ itemSlots list is empty! UI not initialized?");
        return;
    }
    
    Debug.Log($"🎯 UI has {itemSlots.Count} slots available");
    
    for (int i = 0; i < itemSlots.Count; i++) {
        if (i < items.Count) {
            var item = items[i];
            Debug.Log($"  Slot {i}: {item.displayName} (rarity {item.rarity})");
            itemSlots[i].UpdateDisplay(item);
            // BREAKPOINT HERE - after updating slot
        } else {
            itemSlots[i].Clear();
        }
    }
    
    Debug.Log("✅ RefreshInventory complete");
}

// Watch expressions:
// - inventorySystem (should not be null)
// - inventorySystem.GetAllItems().Count
// - itemSlots.Count
// - items[0] (inspect first item details)
```

**Common Causes:**

1. **InventorySystem not assigned:**
   ```csharp
   // In Unity Inspector → Canvas → InventoryUI component
   // Verify "Inventory System" field references InventorySystem GameObject
   
   void Start() {
       if (inventorySystem == null) {
           inventorySystem = FindObjectOfType<InventorySystem>();
           if (inventorySystem == null) {
               Debug.LogError("❌ Could not find InventorySystem!");
           }
       }
   }
   ```

2. **UI Toolkit not initialized:**
   ```csharp
   private void OnEnable()
   {
       var root = GetComponent<UIDocument>()?.rootVisualElement;
       if (root == null) {
           Debug.LogError("❌ UIDocument rootVisualElement is null!");
           return;
       }
       
       BindUIElements();
       // BREAKPOINT HERE - verify UI binding
   }
   ```

3. **Event subscription issues:**
   ```csharp
   // In InventorySystem.cs - Verify event is fired
   public void AddItem(CatgirlItem item)
   {
       items.Add(item);
       Debug.Log($"🎁 Added item: {item.displayName}");
       
       OnInventoryChanged?.Invoke();  // ✅ Fire event
       // BREAKPOINT HERE - verify event invocation
       
       if (OnInventoryChanged == null) {
           Debug.LogWarning("⚠️ OnInventoryChanged has no subscribers!");
       }
   }
   
   // In InventoryUI.cs - Subscribe to event
   void Start() {
       inventorySystem.OnInventoryChanged += RefreshInventory;
       Debug.Log("✅ Subscribed to OnInventoryChanged event");
   }
   ```

4. **UI update timing:**
   ```csharp
   // ❌ WRONG - UI updated before item added
   inventoryUI.RefreshInventory();
   inventorySystem.AddItem(newItem);
   
   // ✅ CORRECT - UI updated after item added
   inventorySystem.AddItem(newItem);
   inventoryUI.RefreshInventory();
   
   // ✅ BEST - Use events (automatic timing)
   inventorySystem.OnInventoryChanged += inventoryUI.RefreshInventory;
   inventorySystem.AddItem(newItem);  // Auto-triggers refresh
   ```

---

## 🔧 Advanced Troubleshooting Techniques

### Technique 1: Conditional Breakpoints for Rare Bugs

```csharp
// Set conditional breakpoint when specific condition occurs
// Right-click breakpoint → Edit Breakpoint → Expression:

// Example 1: Break only when pink coins exceed threshold
pinkCoins > 1000000

// Example 2: Break only for specific item
item.itemId == "divine_cow_crown_001"

// Example 3: Break only on network desync
Mathf.Abs(transform.position.x - networkPosition.Value.x) > 1.0f

// Example 4: Break only for specific client
OwnerClientId == 2

// Example 5: Break after N iterations
loopCounter >= 50
```

### Technique 2: Logpoints for Production Debugging

```csharp
// Right-click line → Add Logpoint → Message:

// Example 1: Log variable value without stopping
Current speed: {currentSpeed}, Is grounded: {isGrounded}

// Example 2: Log network state
ClientId: {OwnerClientId}, IsServer: {NetworkManager.Singleton.IsServer}

// Example 3: Log UGS auth state
Player ID: {AuthenticationService.Instance.PlayerId}

// Example 4: Log method entry/exit
>>> Entering UpdateAnimations(), Speed={Speed}

// Logpoints appear in Debug Console without pausing execution
```

### Technique 3: Data Breakpoints (VS Code Premium)

```csharp
// Break when variable VALUE changes (not just accessed)
// Watch panel → Right-click variable → Break on Value Change

// Example: Monitor NetworkVariable changes
private NetworkVariable<int> pinkCoins = new NetworkVariable<int>(0);
// Set data breakpoint on pinkCoins.Value
// Breaks whenever any code modifies the value

// Example: Track inventory mutations
private List<CatgirlItem> items = new List<CatgirlItem>();
// Set data breakpoint on items.Count
// Breaks when item added or removed
```

### Technique 4: Exception Breakpoints

```csharp
// Debug panel → Breakpoints section → Check exception types

// Enable "All Exceptions" to catch:
// - NullReferenceException
// - AuthenticationException
// - InvalidOperationException

// VS Code will break AS SOON AS exception thrown, even if caught

try {
    await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
} catch (Exception e) {
    // With exception breakpoint enabled, breaks HERE before catch block
    Debug.LogError(e.Message);
}
```

### Technique 5: Debug Console Evaluation

```csharp
// While paused at breakpoint, type in Debug Console:

// Evaluate expressions
> currentSpeed * 2.0f

// Call methods
> Debug.Log("Current position: " + transform.position)

// Modify variables
> currentSpeed = 10.0f

// Invoke Unity API
> GameObject.Find("CatGirl").transform.position

// Check complex conditions
> NetworkManager.Singleton?.IsServer ?? false

// Inspect collections
> items.Count
> items[0].displayName
```

---

## 🎯 BambiSleep™-Specific Debugging Patterns

### Pattern 1: Gambling System Edge Case Debugging

```csharp
// In UniversalBankingSystem.cs:299 - 5% house edge validation

[ServerRpc(RequireOwnership = false)]
public void PlaceBetServerRpc(string gameType, long betAmount, ServerRpcParams rpcParams = default)
{
    // BREAKPOINT HERE - start of gambling logic
    Debug.Log($"🎰 Bet placed: {betAmount} {gameType}");
    
    float roll = Random.value;
    Debug.Log($"🎲 Roll: {roll:F6}");
    // BREAKPOINT HERE - inspect roll value
    
    // Watch for house edge edge cases
    if (roll < 0.4750f) {  // 47.5% win rate (5% house edge)
        // BREAKPOINT HERE - player win
        Debug.Log($"✅ Win! Payout: {betAmount * 2}");
        AddCurrency("pinkCoins", betAmount);
    } else {
        // BREAKPOINT HERE - player loss
        Debug.Log($"❌ Loss! Lost: {betAmount}");
        RemoveCurrency("pinkCoins", betAmount);
    }
    
    // Verify house edge over 1000 rolls
    totalBets++;
    if (totalBets % 1000 == 0) {
        float actualWinRate = (float)totalWins / totalBets;
        Debug.Log($"📊 Win rate after {totalBets} bets: {actualWinRate:P2}");
        // BREAKPOINT HERE - validate house edge math
        // Expected: ~47.5% ±2%
    }
}
```

### Pattern 2: Cow Powers Easter Egg Debugging

```csharp
// Detecting secret cow power activation

private void CheckCowPowerActivation()
{
    // BREAKPOINT HERE - secret feature entry point
    
    if (!inventory.HasItem("divine_cow_crown_001")) {
        return;  // No cow crown, no cow powers
    }
    
    Debug.Log("🐄 COW POWERS ACTIVE");
    // BREAKPOINT HERE - cow crown detected
    
    // Check if all cow items equipped
    int cowItemCount = inventory.items.Count(i => i.isCowPowerItem);
    Debug.Log($"🐄 Cow items equipped: {cowItemCount}/5");
    // BREAKPOINT HERE - inspect cow item collection
    
    if (cowItemCount >= 5) {
        Debug.Log("🐄✨ FULL COW SET BONUS ACTIVATED!");
        // BREAKPOINT HERE - Diablo secret level homage
        UnlockSecretCowLevel();
    }
}

// Watch expressions:
// - inventory.items.Where(i => i.isCowPowerItem).ToList()
// - cowItemCount
// - inventory.HasItem("divine_cow_crown_001")
```

### Pattern 3: BambiSleep™ Trademark Verification

```csharp
// Audit trademark usage across UI text

private void ValidateTrademarkUsage()
{
    // Find all TextMeshPro components
    var allText = FindObjectsOfType<TMPro.TextMeshProUGUI>();
    
    int violationCount = 0;
    foreach (var textComponent in allText) {
        string text = textComponent.text;
        
        // BREAKPOINT HERE - inspect each text component
        if (text.Contains("BambiSleep") && !text.Contains("BambiSleep™")) {
            Debug.LogError($"❌ Trademark violation in: {textComponent.gameObject.name}");
            Debug.LogError($"   Text: '{text}'");
            violationCount++;
            // BREAKPOINT HERE - found violation
        }
    }
    
    Debug.Log($"🔍 Trademark audit: {violationCount} violations found");
}
```

---

## 📋 Debug Checklist (Print and Keep)

### Before Starting Debug Session

- [ ] Unity Editor open and project loaded
- [ ] Unity in Play mode (or will enter on F5)
- [ ] VS Code external editor configured in Unity
- [ ] .csproj files regenerated recently
- [ ] No compilation errors in Unity Console
- [ ] vstuc extension enabled in VS Code
- [ ] Correct debug configuration selected ("Attach to Unity Editor and Play")

### When Breakpoint Doesn't Hit

- [ ] Script attached to GameObject in scene
- [ ] GameObject active in Hierarchy
- [ ] Script component enabled (checkbox ticked)
- [ ] Breakpoint on executable line (not comment/declaration)
- [ ] Code path actually executes (check conditionals)
- [ ] Unity in Play mode when breakpoint set
- [ ] Debugger shows "Attached" status

### When Network Code Fails

- [ ] NetworkManager.Singleton not null
- [ ] NetworkManager started (IsServer or IsClient true)
- [ ] NetworkObject spawned (IsSpawned true)
- [ ] Correct RPC ownership settings
- [ ] Called from correct context (client/server)
- [ ] No "Rpc called before spawn" errors

### When UGS Fails

- [ ] UnityServices.InitializeAsync() completed
- [ ] Authentication completed successfully
- [ ] Player ID valid (not null/empty)
- [ ] Project ID configured in Unity Dashboard
- [ ] Economy items exist in same environment
- [ ] Network connectivity available

### When UI Doesn't Update

- [ ] InventorySystem reference assigned
- [ ] Event subscriptions registered
- [ ] RefreshInventory() actually called
- [ ] UI Toolkit UIDocument has rootVisualElement
- [ ] Item slots list initialized
- [ ] Canvas active and visible

---

## 🆘 Emergency Debug Commands

```bash
# When all else fails, nuclear options:

# 1. Clean Unity project completely
./catgirl-avatar-project/Library && ./catgirl-avatar-project/Temp
rm -rf ./catgirl-avatar-project/obj

# 2. Regenerate all .csproj files
# Unity → Edit → Preferences → External Tools → Regenerate project files

# 3. Restart Unity Editor AND VS Code (both must restart)
pkill -9 Unity
pkill -9 code
# Then reopen Unity first, VS Code second

# 4. Verify Unity version matches
cat catgirl-avatar-project/ProjectSettings/ProjectVersion.txt
# Should be: m_EditorVersion: 6000.2.11f1

# 5. Reinstall vstuc extension
code --uninstall-extension visualstudiotoolsforunity.vstuc
code --install-extension visualstudiotoolsforunity.vstuc

# 6. Check Unity debugger port
netstat -an | grep 56000
# Unity uses port 56000-56100 for debugging

# 7. Enable verbose logging
export UNITY_DEBUG_LOG=1
export VSTUC_LOG_LEVEL=verbose
```

---

🌸 **When in doubt, add more breakpoints and logging!** 🌸

💡 **Pro tip:** Set breakpoints in `OnNetworkSpawn()`, `Start()`, and first line of `Update()` - these always execute if script is working.
