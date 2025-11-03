````instructions
# BambiSleep™ Church CatGirl Avatar System - AI Agent Guide

## 🎯 Project Overview

Unity 6.2 LTS (6000.2.11f1) XR avatar framework with advanced RPG systems, networked multiplayer, and Node.js IPC bridge. Features a playful maximalist aesthetic with emoji-driven development culture.

**Core Tech Stack**:
- Unity 6.2 (6000.2.11f1) with 7 C# subsystems (2,491 lines)
- Node.js 20.19.5 (Volta-pinned) for IPC orchestration
- 8 MCP servers for automation (filesystem, git, github, memory, etc.)
- Jest testing with enforced 80% coverage threshold

**Architecture Pattern**: Unity C# ↔ JSON stdin/stdout IPC ↔ Node.js EventEmitter ↔ MCP Servers

## 🚨 Critical Rules (Breaking These Will Cause Failures)

1. **C# Namespaces REQUIRED**: `BambiSleep.CatGirl.{Audio|Character|Economy|IPC|Networking|UI}` - NEVER use default namespace
2. **Emoji commits MANDATORY**: Start commits with `🦋 Add`, `✨ Update`, `🐛 Fix`, `💎 Refactor` (see `docs/architecture/RELIGULOUS_MANTRA.md` for full list)
3. **Jest coverage ≥80%**: All 4 metrics (branches, functions, lines, statements) - build fails below threshold
4. **Unity Gaming Services sequence**: Must call `UnityServices.InitializeAsync()` → `AuthenticationService.SignInAnonymouslyAsync()` → service-specific init
5. **NetworkBehaviour ownership**: Always `if (IsOwner)` check before state modifications, subscribe/unsubscribe events in `OnNetworkSpawn`/`OnNetworkDespawn`
6. **Humanoid avatar requirements**: T-pose base, 15+ bones mapped (green indicators in Avatar Configuration), 1m scale
7. **IPC message format**: `{ type: string, timestamp: ISO8601, data: object }` - exact schema required
8. **Trademark compliance**: Use `BambiSleep™` (with ™) in all public-facing content

## 📋 Development Checklist (Run Before Coding)

**For Unity C# changes**:
- [ ] Check existing patterns in `Assets/Scripts/{domain}/` - match coding style
- [ ] Verify correct namespace (Audio/Character/Economy/IPC/Networking/UI)
- [ ] Use ScriptableObjects for static game data (items, recipes, quests, tech nodes)
- [ ] Cache `Animator.StringToHash()` at class level - NEVER call in `Update()`
- [ ] Add `[Header("🌸 {Section Name}")]` for Inspector organization

**For Node.js changes**:
- [ ] Verify `npm test` passes with ≥80% coverage
- [ ] Check `src/unity/unity-bridge.js` for IPC patterns (EventEmitter-based)
- [ ] Ensure emoji commit message matches change type

**For both**:
- [ ] Run `./scripts/mcp-validate.sh` to verify 8 MCP servers operational
- [ ] Read relevant docs in `docs/architecture/` before major changes

## ⚡ Essential Commands (Copy-Paste Ready)

```bash
# Testing & Quality
npm test                          # Run Jest with 80% coverage requirement
npm run test:watch               # TDD mode - auto-rerun on file changes
npm run lint                     # Check code style
npm run lint:fix                 # Auto-fix linting issues

# MCP Server Management
./scripts/mcp-validate.sh        # Verify all 8 MCP servers are operational
./scripts/setup-mcp.sh           # Initial MCP configuration

# Unity Debugging
./scripts/setup-unity-debug.sh   # Configure Unity remote debugging
./scripts/launch-unity-debug.sh  # Launch Unity with debugger attached

# Build & Deploy
npm run build                    # Build universal distribution
npm run container:build          # Build Docker container
npm run docs:verify              # Validate documentation structure
```

**VS Code Tasks** (Ctrl+Shift+P → "Run Task"):
- "Run Tests" - Jest with coverage
- "Clean Unity Project" - Remove temp files
- "Build Container" - Docker build
- "Setup MCP Servers" - Initialize MCP environment

**Quick Start**: `npm install` → `npm test` → `./scripts/mcp-validate.sh` → Read `docs/development/UNITY_SETUP_GUIDE.md`

## 🔧 Unity C# Critical Patterns

### 1. Namespace Convention (REQUIRED - No Exceptions)
```csharp
// Pattern for ALL C# scripts - choose domain based on functionality
namespace BambiSleep.CatGirl.Audio       // Sound, music, voice
namespace BambiSleep.CatGirl.Character   // Avatar controller, animations
namespace BambiSleep.CatGirl.Economy     // Banking, inventory, shop
namespace BambiSleep.CatGirl.IPC         // Node.js communication
namespace BambiSleep.CatGirl.Networking  // Multiplayer, sync
namespace BambiSleep.CatGirl.UI          // Menus, HUD, inventory UI
{
    [Header("🌸 Pink Configuration")]  // Use emoji headers for Inspector sections
    public class CatgirlController : NetworkBehaviour
    {
        // Implementation
    }
}
```

### 2. NetworkBehaviour Pattern (Multiplayer Authority)
```csharp
using Unity.Netcode;

namespace BambiSleep.CatGirl.Character
{
    public class CatgirlController : NetworkBehaviour
    {
        // Network-synced variables
        private NetworkVariable<float> networkSpeed = new NetworkVariable<float>(5f);
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            // CRITICAL: Check ownership before local initialization
            if (IsOwner)
            {
                InitializeLocalSystems();
                UpdateStatsServerRpc(localPlayerData);
            }
            
            // Subscribe to network events for ALL clients
            networkSpeed.OnValueChanged += OnSpeedChanged;
        }
        
        public override void OnNetworkDespawn()
        {
            // ALWAYS cleanup to prevent memory leaks
            networkSpeed.OnValueChanged -= OnSpeedChanged;
            base.OnNetworkDespawn();
        }
        
        [ServerRpc]  // Client → Server RPC
        private void UpdateStatsServerRpc(PlayerData data) { /* ... */ }
        
        [ClientRpc]  // Server → All Clients RPC
        private void NotifyPlayersClientRpc(string message) { /* ... */ }
    }
}
```

### 3. Unity Gaming Services Initialization (MUST Follow This Order)
```csharp
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Economy;

namespace BambiSleep.CatGirl.Economy
{
    public class EconomyManager : MonoBehaviour
    {
        private async void Start()
        {
            try 
            {
                // Step 1: Initialize core Unity Services
                await UnityServices.InitializeAsync();
                
                // Step 2: Authenticate (required for all UGS services)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                
                // Step 3: Initialize service-specific features
                await EconomyService.Instance.RefreshBalancesAsync();
                
                Debug.Log("✨ Economy initialized successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"🐛 UGS initialization failed: {e.Message}");
            }
        }
    }
}
```

### 4. Animator Performance Pattern (Cache Hashes - Critical!)
```csharp
namespace BambiSleep.CatGirl.Character
{
    public class AnimationController : MonoBehaviour
    {
        private Animator animator;
        
        // Cache at CLASS LEVEL - computed once on script load
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        
        private void Update()
        {
            // CORRECT: Use cached hash (fast)
            animator.SetFloat(Speed, currentSpeed);
            animator.SetBool(IsJumping, isGrounded);
            
            // WRONG: Don't do this in Update() - creates garbage every frame!
            // animator.SetFloat("Speed", currentSpeed);  ❌
        }
        
        public void TriggerAttack()
        {
            animator.SetTrigger(AttackTrigger);  // Use cached hash
        }
    }
}
```

### 5. Humanoid Avatar Setup (Mecanim Configuration)
```csharp
// This is a WORKFLOW, not code - do this in Unity Editor:
// 
// FBX Import Settings:
// 1. Select FBX in Project window → Inspector → Rig tab
// 2. Animation Type: Humanoid (NOT Generic)
// 3. Avatar Definition: Create From This Model
// 4. Click "Configure..." button → Avatar Configuration window opens
// 5. Verify bone mapping (green = mapped correctly, red = unmapped/wrong)
//    - Required: Hips, Spine, Chest, Neck, Head, Shoulders, Arms, Hands, Legs, Feet (15 minimum)
//    - Optional: Fingers, Toes, Jaw, Eyes
// 6. Base pose MUST be T-pose for auto-mapping to work
// 7. Muscles & Settings tab: Adjust rotation limits for IK/procedural animations
// 8. Avatar is saved as sub-asset under FBX (MyModel.fbx/MyModelAvatar)
// 9. Scale: Set to 1m in Transform (Apply in Blender before export)

// Code usage after avatar configured:
[RequireComponent(typeof(Animator))]
public class CatgirlAvatar : MonoBehaviour
{
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        
        // Verify humanoid rig
        if (!animator.isHuman)
        {
            Debug.LogError("🐛 Avatar is not Humanoid! Check FBX import settings.");
            return;
        }
        
        // Access humanoid bones for IK or procedural animation
        Transform leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
        Transform rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
    }
}
```

### 6. IPC Bridge Pattern (Unity → Node.js Communication)
```csharp
using System;
using UnityEngine;

namespace BambiSleep.CatGirl.IPC
{
    [Serializable]
    public class IPCMessage
    {
        public string type;          // Message type identifier
        public string timestamp;     // ISO8601 format
        public string data;          // JSON string for nested objects
    }
    
    public class IPCBridge : MonoBehaviour
    {
        // Send message to Node.js via stdout (Node.js reads via readline)
        public void SendToNodeJS(string messageType, object payload)
        {
            var message = new IPCMessage 
            { 
                type = messageType,
                timestamp = DateTime.UtcNow.ToString("o"),  // ISO8601
                data = JsonUtility.ToJson(payload)
            };
            
            // Write to stdout - Node.js EventEmitter picks this up
            Console.WriteLine(JsonUtility.ToJson(message));
        }
        
        // Example usage: notify Node.js of avatar state changes
        public void NotifyAvatarStateChanged(string state)
        {
            var payload = new { avatarState = state, sceneTime = Time.time };
            SendToNodeJS("avatar:state_changed", payload);
        }
    }
}
```

## 📦 Unity Package-Specific Patterns

### XR Interaction Toolkit (v3.0.5) - VR/AR Input
```csharp
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace BambiSleep.CatGirl.Character
{
    // XR Grab Interactable - for picking up items
    public class CatgirlGrabbable : XRGrabInteractable
    {
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            // Haptic feedback when grabbed
            if (args.interactorObject is XRBaseControllerInteractor controller)
            {
                controller.SendHapticImpulse(0.5f, 0.2f);  // amplitude, duration
            }
        }
    }
    
    // Eye tracking for gaze-based interaction
    public class GazeInteractor : MonoBehaviour
    {
        private XRInputSubsystem eyeTrackingSubsystem;
        
        void Start()
        {
            // Get eye tracking subsystem
            var subsystems = new List<XRInputSubsystem>();
            SubsystemManager.GetInstances(subsystems);
            eyeTrackingSubsystem = subsystems.FirstOrDefault();
        }
        
        void Update()
        {
            if (eyeTrackingSubsystem != null && eyeTrackingSubsystem.TryGetEyeGazePosition(out var gazePosition))
            {
                // Cast ray from eye gaze position for UI selection
                RaycastHit hit;
                if (Physics.Raycast(gazePosition, transform.forward, out hit, 10f))
                {
                    // Interact with gazed object
                }
            }
        }
    }
}
```

### Animation Rigging (v1.3.1) - Procedural IK
```csharp
using UnityEngine.Animations.Rigging;

namespace BambiSleep.CatGirl.Character
{
    public class CatgirlIK : MonoBehaviour
    {
        [Header("🌸 IK Rig Components")]
        public Rig catgirlRig;
        public TwoBoneIKConstraint leftHandIK;
        public TwoBoneIKConstraint rightHandIK;
        public MultiAimConstraint headLookAt;
        
        [Header("🌸 IK Targets")]
        public Transform leftHandTarget;
        public Transform rightHandTarget;
        public Transform lookAtTarget;
        
        void Start()
        {
            // Animation Rigging workflow:
            // 1. Add Rig component to root (catgirlRig)
            // 2. Add Rig Layer with weight 1.0
            // 3. Add TwoBoneIKConstraint for each limb
            // 4. Set Source Objects (shoulder, elbow, hand) and Target
            // 5. Rig auto-evaluates every frame
            
            // Enable/disable rig at runtime
            catgirlRig.weight = 1.0f;  // 0 = animations only, 1 = full IK
        }
        
        public void ReachForObject(Transform targetObject)
        {
            // Move IK target to object position
            rightHandTarget.position = targetObject.position;
            rightHandTarget.rotation = targetObject.rotation;
            
            // Blend IK weight smoothly
            StartCoroutine(BlendIKWeight(rightHandIK, 1.0f, 0.3f));
        }
        
        IEnumerator BlendIKWeight(TwoBoneIKConstraint constraint, float targetWeight, float duration)
        {
            float startWeight = constraint.weight;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                constraint.weight = Mathf.Lerp(startWeight, targetWeight, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            constraint.weight = targetWeight;
        }
    }
}
```

### Cinemachine (v2.10.1) - Camera Control
```csharp
using Cinemachine;

namespace BambiSleep.CatGirl.Character
{
    public class CatgirlCameraController : MonoBehaviour
    {
        [Header("🌸 Virtual Cameras")]
        public CinemachineVirtualCamera thirdPersonCam;
        public CinemachineVirtualCamera firstPersonCam;
        public CinemachineVirtualCamera emoteCloseupCam;
        
        private CinemachineBrain brain;
        
        void Start()
        {
            brain = Camera.main.GetComponent<CinemachineBrain>();
            
            // Set blend style (Cut, EaseInOut, etc.)
            brain.m_DefaultBlend = new CinemachineBlendDefinition(
                CinemachineBlendDefinition.Style.EaseInOut, 1.5f
            );
        }
        
        public void SwitchToFirstPerson()
        {
            // Priority system: highest priority camera is active
            thirdPersonCam.Priority = 0;
            firstPersonCam.Priority = 10;
            emoteCloseupCam.Priority = 0;
        }
        
        public void TriggerEmoteCamera(float duration)
        {
            StartCoroutine(TemporaryCameraSwitch(emoteCloseupCam, duration));
        }
        
        IEnumerator TemporaryCameraSwitch(CinemachineVirtualCamera camera, float duration)
        {
            int originalPriority = camera.Priority;
            camera.Priority = 20;  // Highest priority
            yield return new WaitForSeconds(duration);
            camera.Priority = originalPriority;
        }
    }
}
```

### Unity Services Economy (v3.4.2) - Virtual Currency
```csharp
using Unity.Services.Economy;
using Unity.Services.Economy.Model;

namespace BambiSleep.CatGirl.Economy
{
    public class VirtualCurrencyManager : MonoBehaviour
    {
        async void Start()
        {
            // MUST initialize in order: Core → Auth → Economy
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            
            // Fetch player balances from cloud
            var balances = await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
            
            foreach (var balance in balances.Balances)
            {
                Debug.Log($"💎 Currency: {balance.CurrencyId}, Amount: {balance.Balance}");
            }
        }
        
        public async Task<bool> PurchaseItem(string itemId, int cost)
        {
            try
            {
                // Virtual purchase (deducts currency, grants item)
                var options = new MakeVirtualPurchaseOptions
                {
                    PlayerId = AuthenticationService.Instance.PlayerId
                };
                
                var result = await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync(
                    itemId, 
                    options
                );
                
                Debug.Log($"✨ Purchased: {itemId}, New balance: {result.PlayerBalances}");
                return true;
            }
            catch (EconomyException e)
            {
                Debug.LogError($"🐛 Purchase failed: {e.Message}");
                return false;
            }
        }
    }
}
```

### Netcode for GameObjects (v2.0.0) - Multiplayer Sync
```csharp
using Unity.Netcode;

namespace BambiSleep.CatGirl.Networking
{
    public class NetworkedCatgirl : NetworkBehaviour
    {
        // Network variables auto-sync to all clients
        private NetworkVariable<float> networkHealth = new NetworkVariable<float>(
            100f,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner  // Only owner can write
        );
        
        private NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>();
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            // Subscribe to value changes (all clients)
            networkHealth.OnValueChanged += OnHealthChanged;
            
            if (IsOwner)
            {
                // Owner-only initialization
                InitializeLocalPlayer();
            }
            else
            {
                // Remote player initialization
                InitializeRemotePlayer();
            }
        }
        
        public override void OnNetworkDespawn()
        {
            networkHealth.OnValueChanged -= OnHealthChanged;
            base.OnNetworkDespawn();
        }
        
        void Update()
        {
            if (!IsOwner) return;
            
            // Owner updates their position
            networkPosition.Value = transform.position;
        }
        
        [ServerRpc]
        public void TakeDamageServerRpc(float damage)
        {
            // Only server can modify health
            networkHealth.Value = Mathf.Max(0, networkHealth.Value - damage);
        }
        
        [ClientRpc]
        public void PlayEffectClientRpc(string effectName)
        {
            // All clients play effect
            PlayVisualEffect(effectName);
        }
        
        private void OnHealthChanged(float oldValue, float newValue)
        {
            Debug.Log($"💖 Health changed: {oldValue} → {newValue}");
            UpdateHealthUI(newValue);
        }
    }
}
```

### Visual Effect Graph (v16.0.6) - Particle Systems
```csharp
using UnityEngine.VFX;

namespace BambiSleep.CatGirl.Character
{
    public class CatgirlVFXController : MonoBehaviour
    {
        [Header("🌸 Visual Effects")]
        public VisualEffect pinkAuraVFX;
        public VisualEffect purringParticlesVFX;
        public VisualEffect rainbowTrailVFX;
        
        void Start()
        {
            // Set VFX properties (defined in VFX Graph)
            pinkAuraVFX.SetFloat("IntensityMultiplier", 2.0f);
            pinkAuraVFX.SetVector3("EmissionPosition", transform.position);
            pinkAuraVFX.SetGradient("ColorGradient", GetPinkGradient());
        }
        
        public void TriggerPurr(float intensity)
        {
            purringParticlesVFX.SetFloat("Intensity", intensity);
            purringParticlesVFX.SendEvent("OnPurr");  // Custom event in VFX Graph
        }
        
        public void ToggleRainbowTrail(bool enabled)
        {
            if (enabled)
                rainbowTrailVFX.Play();
            else
                rainbowTrailVFX.Stop();
        }
        
        Gradient GetPinkGradient()
        {
            var gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(new Color(1f, 0.4f, 0.7f), 0f),
                    new GradientColorKey(new Color(1f, 0.8f, 0.9f), 1f)
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(0f, 1f)
                }
            );
            return gradient;
        }
    }
}
```



## 🎮 RPG Systems Patterns (Economy, Inventory, Crafting, Quests)

### ScriptableObject-Driven Design (REQUIRED)
```csharp
// Static data (items, recipes, upgrades, quests) = ScriptableObjects
// Runtime data (durability, owned enchantments) = instances
[CreateAssetMenu(menuName = "CatGirl/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int baseValue;
    public int maxDurability;
    public Dictionary<StatType, int> baseStats;  // STR, DEX, INT, etc.
    // ... static properties
}

// Runtime instance with unique state
public class ItemInstance
{
    public ItemData itemData;  // Reference to ScriptableObject
    public int currentDurability;
    public List<IEnchantment> enchantments;
    // ... instance-specific state
}
```

### Currency System (Singleton Pattern)
```csharp
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public int currentCoins = 1000;
    
    void Awake() { if (Instance == null) Instance = this; }
    
    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount) { currentCoins -= amount; return true; }
        return false;
    }
    
    public void AddCoins(int amount) { currentCoins += amount; /* Update UI */ }
    
    public void RefundPlayer(string playerID, int amount)
    {
        // Multiplayer refund logic
        AddCoins(amount);
    }
}
```

### Grid-Based Inventory (Diablo-Style)
```csharp
// Item interface for grid placement
public interface IInventoryItem
{
    string Name { get; }
    Sprite Icon { get; }
    int Width { get; }   // Grid cells width
    int Height { get; }  // Grid cells height
    int Durability { get; set; }
    List<IEnchantment> Enchantments { get; }
}

// Inventory manager maintains 2D grid
public class InventoryManager : MonoBehaviour
{
    private IInventoryItem[,] grid = new IInventoryItem[8, 4];  // 8x4 grid
    
    public bool TryPlaceItem(IInventoryItem item, int x, int y)
    {
        // Check if item fits (no overlaps), place if valid
        // Support item rotation for tetris-style fitting
    }
    
    public void AddItemToPlayer(string playerID, ItemInstance item)
    {
        // Find first available grid slot, place item
    }
}
```

### Crafting Recipes (Grid-Based)
```csharp
[System.Serializable]
public class CraftRecipe : ScriptableObject
{
    public int[] requiredItemIDs;  // 9-element array for 3x3 grid (0=empty)
    public int resultItemID;
    public bool requiresExactPositions = true;  // Minecraft-style vs unordered
}

// On crafting: check inventory for ingredients, consume, spawn output
```

### Quest System (Event-Driven)
```csharp
[System.Serializable]
public class Quest : ScriptableObject
{
    public string title;
    public string description;
    public bool isCompleted;
    public List<QuestObjective> objectives;  // kill X enemies, collect Y items
    public List<RewardData> rewards;
    
    public void CompleteQuest()
    {
        isCompleted = true;
        // Grant rewards via RewardManager
        // Fire analytics event
        AnalyticsEvents.TrackQuestCompletion(title, rewards.Count);
    }
}
```

### Durability & Repair System
```csharp
// Items degrade on use; broken items unusable until repaired
public class ItemInstance
{
    public int currentDurability;
    
    public void ReduceDurability(int amount)
    {
        currentDurability = Mathf.Max(0, currentDurability - amount);
        if (currentDurability == 0) { /* Mark broken */ }
    }
    
    public int GetRepairCost()
    {
        // Calculate based on missing durability and rarity
        return (itemData.maxDurability - currentDurability) * itemData.repairCostMultiplier;
    }
}
```

### Tech Tree System (Unlock Dependencies)
```csharp
[CreateAssetMenu(menuName = "CatGirl/TechNode")]
public class TechNode : ScriptableObject
{
    public string nodeName;
    public string description;
    public List<TechNode> prerequisites;  // Must unlock these first
    public int unlockCost;
    public bool isUnlocked;
    
    public bool CanUnlock()
    {
        return prerequisites.All(prereq => prereq.isUnlocked) 
            && CurrencyManager.Instance.currentCoins >= unlockCost;
    }
    
    public void Unlock()
    {
        if (CanUnlock())
        {
            CurrencyManager.Instance.SpendCoins(unlockCost);
            isUnlocked = true;
            AnalyticsEvents.TrackTechUnlock(nodeName, unlockCost);
        }
    }
}
```

### Enchanting/Imbuing System (Slot Limits, Stat Modifiers)
```csharp
public interface IEnchantment
{
    string Name { get; }
    EnchantmentSlotType SlotType { get; }  // Prefix, Suffix, Unique
    Dictionary<StatType, float> StatModifiers { get; }  // +10 STR, +5% Crit
}

public class ItemInstance
{
    public List<IEnchantment> enchantments = new List<IEnchantment>();
    private const int MAX_PREFIX = 1;
    private const int MAX_SUFFIX = 1;
    private const int MAX_UNIQUE = 1;
    
    public bool CanAddEnchantment(IEnchantment enchant)
    {
        int count = enchantments.Count(e => e.SlotType == enchant.SlotType);
        return enchant.SlotType switch
        {
            EnchantmentSlotType.Prefix => count < MAX_PREFIX,
            EnchantmentSlotType.Suffix => count < MAX_SUFFIX,
            EnchantmentSlotType.Unique => count < MAX_UNIQUE,
            _ => false
        };
    }
    
    public int GetTotalStat(StatType stat)
    {
        // Sum base + all enchantment modifiers
        float baseValue = itemData.baseStats.GetValueOrDefault(stat, 0);
        float enchantBonus = enchantments.Sum(e => e.StatModifiers.GetValueOrDefault(stat, 0));
        return Mathf.RoundToInt(baseValue + enchantBonus);
    }
}
```

### Analytics/Telemetry Events (Crafting, Purchases, Retention)
```csharp
public static class AnalyticsEvents
{
    // Fire on item crafted
    public static void TrackCrafting(string recipeName, int ingredientCount)
    {
        var parameters = new Dictionary<string, object>
        {
            { "recipe_name", recipeName },
            { "ingredient_count", ingredientCount },
            { "timestamp", DateTime.UtcNow.ToString("o") }
        };
        // Unity Analytics or custom backend
        // AnalyticsService.Instance.CustomData("item_crafted", parameters);
    }
    
    // Fire on shop purchase
    public static void TrackPurchase(string itemName, int price, string currency)
    {
        var parameters = new Dictionary<string, object>
        {
            { "item_name", itemName },
            { "price", price },
            { "currency", currency },  // coins, gems, etc.
            { "player_level", PlayerManager.Instance.level }
        };
        // AnalyticsService.Instance.CustomData("shop_purchase", parameters);
    }
    
    // Fire on daily login (retention tracking)
    public static void TrackDailyLogin(int consecutiveDays)
    {
        var parameters = new Dictionary<string, object>
        {
            { "consecutive_days", consecutiveDays },
            { "total_playtime_minutes", PlayerPrefs.GetInt("TotalPlaytime") }
        };
        // AnalyticsService.Instance.CustomData("daily_login", parameters);
    }
    
    // Fire on quest completion
    public static void TrackQuestCompletion(string questName, int rewardCount)
    {
        var parameters = new Dictionary<string, object>
        {
            { "quest_name", questName },
            { "reward_count", rewardCount }
        };
        // AnalyticsService.Instance.CustomData("quest_completed", parameters);
    }
    
    // Fire on tech tree unlock
    public static void TrackTechUnlock(string nodeName, int cost)
    {
        var parameters = new Dictionary<string, object>
        {
            { "node_name", nodeName },
            { "unlock_cost", cost }
        };
        // AnalyticsService.Instance.CustomData("tech_unlocked", parameters);
    }
}
```

### Auction House System (Bidding, Listings, Expiration)
```csharp
[System.Serializable]
public class AuctionListing
{
    public string listingID;
    public ItemInstance item;
    public int startingBid;
    public int currentBid;
    public string highestBidder;  // Player ID
    public DateTime expirationTime;
    public bool isActive;
    
    public bool IsExpired() => DateTime.UtcNow > expirationTime;
    
    public bool PlaceBid(string playerID, int bidAmount)
    {
        if (!isActive || IsExpired() || bidAmount <= currentBid) return false;
        
        // Refund previous bidder
        if (!string.IsNullOrEmpty(highestBidder))
        {
            CurrencyManager.Instance.RefundPlayer(highestBidder, currentBid);
        }
        
        currentBid = bidAmount;
        highestBidder = playerID;
        AnalyticsEvents.TrackAuctionBid(item.itemData.itemName, bidAmount);
        return true;
    }
    
    public void Complete()
    {
        isActive = false;
        if (!string.IsNullOrEmpty(highestBidder))
        {
            // Transfer item to winner, coins to seller (minus auction house fee)
            InventoryManager.Instance.AddItemToPlayer(highestBidder, item);
            int sellerPayout = Mathf.RoundToInt(currentBid * 0.95f);  // 5% house fee
            // CurrencyManager.Instance.AddCoinsToPlayer(sellerID, sellerPayout);
        }
    }
}

public class AuctionHouseManager : MonoBehaviour
{
    public static AuctionHouseManager Instance;
    private List<AuctionListing> activeListings = new List<AuctionListing>();
    
    void Awake() { if (Instance == null) Instance = this; }
    
    void Update()
    {
        // Check for expired auctions every frame (consider optimization)
        foreach (var listing in activeListings.Where(l => l.IsExpired() && l.isActive))
        {
            listing.Complete();
        }
    }
    
    public void CreateListing(ItemInstance item, int startingBid, int durationHours)
    {
        var listing = new AuctionListing
        {
            listingID = System.Guid.NewGuid().ToString(),
            item = item,
            startingBid = startingBid,
            currentBid = startingBid,
            expirationTime = DateTime.UtcNow.AddHours(durationHours),
            isActive = true
        };
        activeListings.Add(listing);
    }
}
```

## 🟢 Node.js Patterns

### IPC EventEmitter Pattern (Bidirectional Unity Communication)
```javascript
// File: src/unity/unity-bridge.js (129 lines)
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
        // Spawn Unity process with IPC arguments
        const args = [
            '-projectPath', this.projectPath,
            '-executeMethod', 'IPCBridge.StartIPC'
        ];
        if (this.batchMode) args.unshift('-batchmode');
        
        this.process = spawn(this.unityPath, args);
        
        // Listen to Unity stdout (JSON messages)
        this.process.stdout.on('data', (data) => {
            this._handleStdout(data);  // Parse and emit as events
        });
        
        this.emit('ready');
    }
    
    // Send message to Unity via stdin
    sendMessage(type, data) {
        if (!this.process) throw new Error('Unity process not started');
        
        const message = {
            type,
            timestamp: new Date().toISOString(),
            data
        };
        
        // Unity reads this from stdin
        this.process.stdin.write(JSON.stringify(message) + '\n');
    }
    
    // Parse Unity stdout and emit as events
    _handleStdout(data) {
        this.messageBuffer += data.toString();
        const lines = this.messageBuffer.split('\n');
        this.messageBuffer = lines.pop();  // Keep incomplete line
        
        lines.forEach(line => {
            try {
                const message = JSON.parse(line);
                // Emit as 'unity:{type}' events
                this.emit(`unity:${message.type}`, message.data);
            } catch (e) {
                // Ignore non-JSON lines (Unity logs)
            }
        });
    }
    
    stop() {
        if (this.process) {
            this.process.kill();
            this.process = null;
        }
    }
}

// Usage example:
const bridge = new UnityBridge({
    unityPath: '/path/to/Unity',
    projectPath: '/path/to/project'
});

await bridge.start();

// Listen for Unity events
bridge.on('unity:avatar_loaded', (data) => {
    console.log('Avatar loaded:', data);
});

// Send commands to Unity
bridge.sendMessage('set_parameter', { name: 'speed', value: 5.0 });
```

### Jest Testing Pattern (80% Coverage Enforced)
```javascript
// File: __tests__/unity-bridge.test.js
// jest.config.js enforces 80% threshold on all metrics

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
        // CRITICAL: Always cleanup to prevent memory leaks
        if (bridge.process) {
            bridge.stop();
        }
        bridge = null;
    });
    
    test('should send message to Unity', () => {
        bridge.start();
        const spy = jest.spyOn(bridge.process.stdin, 'write');
        
        bridge.sendMessage('test_type', { foo: 'bar' });
        
        expect(spy).toHaveBeenCalledWith(
            expect.stringContaining('"type":"test_type"')
        );
    });
    
    test('should emit events from Unity stdout', (done) => {
        bridge._handleStdout(Buffer.from(
            '{"type":"avatar_loaded","data":{"name":"Catgirl"}}\n'
        ));
        
        bridge.on('unity:avatar_loaded', (data) => {
            expect(data.name).toBe('Catgirl');
            done();
        });
    });
});
```

### Configuration Pattern (Environment-Aware)
```javascript
// File: config/index.js
const path = require('path');

module.exports = {
    unity: {
        version: '6000.2.11f1',
        projectPath: path.resolve(__dirname, '../catgirl-avatar-project'),
        executablePath: process.env.UNITY_PATH || '/Applications/Unity/Hub/Editor/6000.2.11f1/Unity.app/Contents/MacOS/Unity'
    },
    
    mcp: {
        servers: 8,  // Expected number of operational servers
        configPath: '.vscode/settings.json',
        requiredServers: [
            'filesystem',
            'memory', 
            'git',
            'github',
            'brave-search',
            'sequential-thinking',
            'postgres',
            'everything'
        ]
};
```

## 🤖 MCP Server Usage Examples (Codebase-Specific)

### Using Filesystem Server for Code Editing
```javascript
// When adding new C# scripts, use filesystem server to:

// 1. Create new namespace-compliant script
await filesystem.writeFile(
    'catgirl-avatar-project/Assets/Scripts/Character/TailPhysics.cs',
    `using UnityEngine;

namespace BambiSleep.CatGirl.Character
{
    [Header("🌸 Tail Configuration")]
    public class TailPhysics : MonoBehaviour
    {
        // Implementation
    }
}`
);

// 2. Read existing patterns before modifying
const existingController = await filesystem.readFile(
    'catgirl-avatar-project/Assets/Scripts/Character/CatgirlController.cs'
);
// Analyze pattern, then apply edits

// 3. Update test coverage
await filesystem.writeFile(
    '__tests__/tail-physics.test.js',
    `describe('TailPhysics', () => { /* ... */ });`
);
```

### Using Git Server for Version Control
```javascript
// Before major Unity changes:

// 1. Create feature branch with emoji prefix
await git.createBranch('feature/🦋-tail-physics-system');

// 2. Stage Unity-specific files (exclude temp)
await git.stageFiles([
    'catgirl-avatar-project/Assets/Scripts/Character/TailPhysics.cs',
    'catgirl-avatar-project/Assets/Scripts/Character/TailPhysics.cs.meta'
]);

// 3. Commit with emoji convention
await git.commit('🦋 Add realistic tail physics with collision detection');

// 4. Check Unity .gitignore compliance
const status = await git.status();
// Should NOT see: Library/, Temp/, Logs/, obj/, *.csproj
```

### Using GitHub Server for PR Workflow
```javascript
// After completing Unity feature:

// 1. Create PR with detailed context
await github.createPullRequest({
    title: '🦋 Add Tail Physics System',
    body: `
## Changes
- Added TailPhysics.cs with spring-damper simulation
- Integrated with Animation Rigging constraints
- Added Jest tests for IPC tail state sync

## Testing
- \`npm test\` passes with 85% coverage
- Unity Editor: Tail responds to movement/rotation
- MCP servers: All 8 operational

## Checklist
- [x] Namespace: BambiSleep.CatGirl.Character
- [x] Emoji commit: 🦋
- [x] Coverage ≥80%
- [x] Documentation updated
    `,
    base: 'main',
    head: 'feature/🦋-tail-physics-system'
});

// 2. Request review from AI or team
await github.requestReview(prNumber, ['HarleyVader']);

// 3. Auto-merge when CI passes
if (checksPass) {
    await github.mergePullRequest(prNumber, {
        merge_method: 'squash'
    });
}
```

### Using Memory Server for Context Retention
```javascript
// Store architectural decisions for future reference

await memory.store('unity-ik-approach', {
    decision: 'Use Animation Rigging instead of custom IK',
    rationale: 'Unity 6.2 Animation Rigging package provides:\n' +
               '- Built-in constraints (TwoBoneIK, MultiAim)\n' +
               '- Better performance than custom IK calculations\n' +
               '- Visual editing in Scene view',
    implementedIn: 'Assets/Scripts/Character/CatgirlIK.cs',
    packages: ['com.unity.animation.rigging@1.3.1'],
    date: '2025-11-03'
});

// Retrieve when making related changes
const ikDecision = await memory.recall('unity-ik-approach');
console.log('Using Animation Rigging because:', ikDecision.rationale);
```

### Using Sequential Thinking for Complex Refactoring
```javascript
// When refactoring RPG systems with many dependencies:

const plan = await sequentialThinking.analyze({
    task: 'Migrate inventory system from MonoBehaviour to ScriptableObject architecture',
    constraints: [
        'Must maintain 80% test coverage',
        'Cannot break existing save/load system',
        'NetworkBehaviour sync must still work'
    ],
    steps: [
        '1. Create ItemData ScriptableObject schema',
        '2. Migrate static item properties',
        '3. Update InventoryManager to use ItemData references',
        '4. Refactor serialization for save system',
        '5. Update NetworkVariable to sync item IDs instead of instances',
        '6. Update tests for new architecture',
        '7. Verify MCP IPC still functions'
    ]
});

// Execute each step with validation
for (const step of plan.steps) {
    await executeStep(step);
    await runTests();  // Ensure coverage stays ≥80%
}
```

### Using Brave Search for Unity Package Documentation
```javascript
// When working with unfamiliar Unity packages:

// 1. Search for official Unity documentation
const vfxDocs = await braveSearch.search(
    'Unity Visual Effect Graph 16.0 spawn event documentation site:docs.unity3d.com'
);

// 2. Find version-specific API changes
const netcodeChanges = await braveSearch.search(
    'Unity Netcode GameObjects 2.0.0 migration guide NetworkVariable'
);

// 3. Search for community solutions
const ik Solutions = await braveSearch.search(
    'Unity Animation Rigging TwoBoneIKConstraint runtime weight blending'
);
```



## 🤖 MCP Server Environment

**8 Required Servers** (validate with `./scripts/mcp-validate.sh`):

| Server | Purpose | Critical? | Layer |
|--------|---------|-----------|-------|
| `filesystem` | File operations, code editing | ✅ Yes | 0 (Primitive) |
| `memory` | Persistent context, knowledge retention | ✅ Yes | 0 (Primitive) |
| `git` | Version control, branch operations | ✅ Yes | 1 (Foundation) |
| `github` | Repository mgmt, PRs, issues | ✅ Yes | 1 (Foundation) |
| `brave-search` | Web search, real-time info | No | 1 (Foundation) |
| `sequential-thinking` | Complex reasoning, problem solving | No | 2 (Advanced) |
| `postgres` | Database operations, SQL queries | No | 2 (Advanced) |
| `everything` | Comprehensive testing, MCP demos | No | 2 (Advanced) |

**Tiered Bootstrap**: Servers start in dependency order (Layer 0 → Layer 1 → Layer 2) to prevent circular dependencies.

**Configuration Location**: `.vscode/settings.json` - MCP servers auto-activate in VS Code when workspace opens.

**Health Monitoring**: Each server checked every 30s with 3-restart resilience before marking failed.

## 📁 Repository Structure & Key Files

```
bambisleep-chat-catgirl/
├── catgirl-avatar-project/          # Unity 6.2 project root
│   ├── Assets/
│   │   ├── Scripts/                 # All C# code (2,491 lines)
│   │   │   ├── Audio/               # AudioManager, sound systems
│   │   │   ├── Character/           # Avatar controller, animations
│   │   │   ├── Economy/             # Banking, inventory, crafting
│   │   │   ├── IPC/                 # IPCBridge, MCP agent
│   │   │   ├── Networking/          # Multiplayer, NetworkBehaviour
│   │   │   └── UI/                  # Menus, HUD, inventory UI
│   │   ├── Animations/              # Animation clips, controllers
│   │   ├── Prefabs/                 # Reusable game objects
│   │   └── Scenes/                  # Unity scenes
│   ├── Packages/
│   │   └── manifest.json            # 16 Unity packages (UGS, Netcode, XR)
│   └── ProjectSettings/             # Unity project configuration
├── src/
│   ├── cli/                         # Command-line tools
│   ├── server/                      # Express server (if needed)
│   ├── unity/
│   │   └── unity-bridge.js          # ⭐ IPC EventEmitter (129 lines)
│   └── utils/                       # Helper functions
├── __tests__/                       # Jest test suite
│   ├── config.test.js
│   ├── server.test.js
│   └── unity-bridge.test.js
├── config/
│   └── index.js                     # ⭐ Unity paths, MCP settings
├── docs/
│   ├── architecture/                # System design specs
│   │   ├── CATGIRL.md               # ⭐ Main architecture (682 lines)
│   │   ├── RELIGULOUS_MANTRA.md     # ⭐ Emoji commits, dev culture
│   │   └── UNITY_IPC_PROTOCOL.md    # IPC message schemas
│   ├── development/
│   │   └── UNITY_SETUP_GUIDE.md     # ⭐ Copy-paste C# code (858 lines)
│   └── guides/
│       └── build.md                 # Build instructions
├── scripts/
│   ├── mcp-validate.sh              # ⭐ Check 8 MCP servers
│   ├── setup-mcp.sh
│   ├── setup-unity-debug.sh
│   └── launch-unity-debug.sh
├── .github/
│   └── copilot-instructions.md      # ⭐ This file
├── index.js                         # Main entry point (info display)
├── package.json                     # ⭐ Scripts, dependencies, config
├── jest.config.js                   # ⭐ 80% coverage threshold
└── Dockerfile                       # Container build
```

**Must-Read Files** (in priority order):
1. `docs/development/UNITY_SETUP_GUIDE.md` - Complete C# implementation guide (858 lines)
2. `docs/architecture/CATGIRL.md` - System architecture, RPG mechanics (682 lines)
3. `docs/architecture/RELIGULOUS_MANTRA.md` - Emoji commits, dev philosophy (113 lines)
4. `src/unity/unity-bridge.js` - IPC communication pattern (129 lines)
5. `jest.config.js` - Testing requirements (80% threshold)

**Unity Packages** (16 total in `Packages/manifest.json`):
- `com.unity.netcode.gameobjects` - Multiplayer networking
- `com.unity.services.economy` - Virtual currencies, inventory
- `com.unity.xr.interaction.toolkit` - VR/AR interactions
- `com.unity.animation.rigging` - Procedural animation, IK
- `com.unity.inputsystem` - New input system
- `com.unity.cinemachine` - Camera control
- And 10 more...

## 📚 Documentation Resources

**Read in this order for maximum productivity**:

1. **`docs/development/UNITY_SETUP_GUIDE.md`** (858 lines)
   - Complete C# implementations ready to copy-paste
   - Step-by-step Unity configuration
   - All 7 subsystems with full code examples

2. **`docs/architecture/CATGIRL.md`** (682 lines)
   - Overall system architecture
   - RPG mechanics (inventory, crafting, quests, tech trees, enchanting, auction house)
   - XR tracking requirements (eye, hand, mouth, finger)
   - Item quality tiers and equipment slots

3. **`docs/architecture/RELIGULOUS_MANTRA.md`** (113 lines)
   - Emoji commit conventions (MANDATORY)
   - Development philosophy
   - Machine-readable emoji mappings

4. **`docs/architecture/UNITY_IPC_PROTOCOL.md`** (432 lines estimated)
   - JSON message schemas for Unity ↔ Node.js
   - Event types and data structures
   - Error handling patterns

5. **`docs/DEBUGGING.md`** (558 lines)
   - Breakpoint locations for common issues
   - Logging patterns
   - Unity/Node.js debugging setup

**Quick Reference**:
- Package scripts: `package.json` (see `scripts` section)
- Unity packages: `catgirl-avatar-project/Packages/manifest.json`
- Test configuration: `jest.config.js`
- VS Code tasks: `.vscode/tasks.json` (if exists)

## 🎨 Project Culture & Development Philosophy

**Aesthetic**: Playful maximalist - NOT typical enterprise code. Emoji-driven commits, pink-themed documentation, "catgirl" naming throughout.

**Code Philosophy** (from `docs/architecture/RELIGULOUS_MANTRA.md`):
- **100% completion mindset** - Features are fully implemented or not started
- **8/8 server operational status** - All MCP servers must pass health checks
- **Enterprise-grade infrastructure** - Production-ready patterns despite playful theme
- **ScriptableObject-driven design** - All static game data (items, recipes, quests, tech nodes) in data files
- **Separation of concerns** - Core systems are headless, UI communicates via events
- **Analytics instrumentation** - Track all player actions (crafting, purchases, quests, etc.)

**Emoji Commit Convention** (MANDATORY - see `RELIGULOUS_MANTRA.md`):
- `🦋` - Add new features, transformations
- `✨` - Update/improve existing code
- `🐛` - Bug fixes
- `💎` - Refactoring, quality improvements
- `🌸` - Package management, dependencies
- `👑` - Architecture decisions
- `🔥` - Performance optimizations
- `🎭` - Development lifecycle, testing

**Example Commits**:
```bash
git commit -m "🦋 Add inventory grid placement system"
git commit -m "✨ Update NetworkBehaviour ownership checks"
git commit -m "🐛 Fix animator hash caching memory leak"
git commit -m "💎 Refactor IPC message serialization"
```

**Documentation Style**: 
- Code examples are COPY-PASTE READY, not pseudocode
- Extensive inline comments explaining "why" not just "what"
- References to specific line counts (e.g., "858 lines") show completeness
- Multiple emoji headers for visual organization

**Development Workflow**:
1. Run `npm test` before every commit (80% coverage enforced)
2. Validate MCP servers with `./scripts/mcp-validate.sh`
3. Read relevant `docs/architecture/` before major changes
4. Match existing patterns in `Assets/Scripts/{domain}/`
5. Use emoji commits that match the change type

**Quality Standards**:
- Jest coverage ≥80% on all metrics (branches, functions, lines, statements)
- All Unity C# must use correct namespace (`BambiSleep.CatGirl.*`)
- IPC messages must follow exact schema (type, timestamp, data)
- NetworkBehaviour must check `IsOwner` before state changes


---

## 🚀 Quick Start Checklist

**First-time setup**:
1. ✅ Run `npm install` (Node.js 20+ required, Volta-pinned)
2. ✅ Run `npm test` to verify Jest works with 80% coverage
3. ✅ Run `./scripts/mcp-validate.sh` to check all 8 MCP servers
4. ✅ Read `docs/development/UNITY_SETUP_GUIDE.md` for Unity configuration
5. ✅ Open `catgirl-avatar-project` in Unity 6.2 (6000.2.11f1)

**Before making changes**:
1. ✅ Identify which domain your change affects (Audio/Character/Economy/IPC/Networking/UI)
2. ✅ Read existing code in `Assets/Scripts/{domain}/` to match patterns
3. ✅ For Unity C#: Verify namespace convention and use `[Header("🌸 ...")]` decorators
4. ✅ For Node.js: Ensure tests maintain ≥80% coverage
5. ✅ Choose correct emoji for commit message

**Key architectural reminders**:
- **IPC**: Unity stdout → Node.js EventEmitter → Emit `unity:{type}` events
- **Networking**: Always check `IsOwner` before state modifications
- **Animations**: Cache `Animator.StringToHash()` at class level, never in `Update()`
- **UGS**: Initialize in order: Core → Auth → Service-specific
- **RPG Data**: Use ScriptableObjects for items, recipes, quests, tech nodes
- **Testing**: All code paths covered, cleanup in `afterEach()`

**Common pitfalls to avoid**:
- ❌ Default namespace instead of `BambiSleep.CatGirl.{Domain}`
- ❌ String parameters in `Animator.Set*()` calls within `Update()`
- ❌ Missing `IsOwner` checks in `NetworkBehaviour`
- ❌ Wrong Unity Gaming Services initialization order
- ❌ Generic commit messages without emoji prefixes
- ❌ Test coverage below 80% threshold

## ⚠️ Common Mistakes & Solutions (Learn From These!)

### 1. Unity Package Version Mismatches
**❌ Problem**: Installing incompatible package versions
```json
// WRONG - Unity 6.2 incompatible versions
"com.unity.netcode.gameobjects": "1.5.0",  // Too old
"com.unity.xr.interaction.toolkit": "2.5.0"  // Breaking changes in 3.x
```

**✅ Solution**: Use exact versions from `Packages/manifest.json`
```json
// CORRECT - Tested versions for Unity 6.2
"com.unity.netcode.gameobjects": "2.0.0",
"com.unity.xr.interaction.toolkit": "3.0.5"
```

**Prevention**: Always check `catgirl-avatar-project/Packages/manifest.json` before adding packages.

---

### 2. Forgetting NetworkVariable Cleanup
**❌ Problem**: Memory leaks from un-unsubscribed events
```csharp
public override void OnNetworkSpawn()
{
    networkHealth.OnValueChanged += OnHealthChanged;
    // MISSING: Cleanup in OnNetworkDespawn!
}
```

**✅ Solution**: Always pair subscribe/unsubscribe
```csharp
public override void OnNetworkSpawn()
{
    networkHealth.OnValueChanged += OnHealthChanged;
}

public override void OnNetworkDespawn()
{
    networkHealth.OnValueChanged -= OnHealthChanged;  // CRITICAL!
    base.OnNetworkDespawn();
}
```

**Prevention**: Search for `OnValueChanged +=` and verify matching `-=` exists.

---

### 3. Animator String Allocation in Update()
**❌ Problem**: Garbage collection spikes from string hashing every frame
```csharp
void Update()
{
    // WRONG - Creates garbage every frame!
    animator.SetFloat("Speed", currentSpeed);
    animator.SetBool("IsJumping", isJumping);
}
```

**✅ Solution**: Cache hashes at class level
```csharp
private static readonly int SpeedHash = Animator.StringToHash("Speed");
private static readonly int IsJumpingHash = Animator.StringToHash("IsJumping");

void Update()
{
    animator.SetFloat(SpeedHash, currentSpeed);  // No allocation
    animator.SetBool(IsJumpingHash, isJumping);
}
```

**Prevention**: Use `grep -r "animator.Set.*\"" Assets/Scripts/` to find violations.

---

### 4. Wrong Unity Gaming Services Init Order
**❌ Problem**: AuthenticationException or NullReferenceException
```csharp
async void Start()
{
    // WRONG - Auth before Core initialization!
    await AuthenticationService.Instance.SignInAnonymouslyAsync();
    await UnityServices.InitializeAsync();
}
```

**✅ Solution**: Always Core → Auth → Service
```csharp
async void Start()
{
    try
    {
        await UnityServices.InitializeAsync();          // 1. Core
        await AuthenticationService.Instance.SignInAnonymouslyAsync();  // 2. Auth
        await EconomyService.Instance.RefreshBalancesAsync();  // 3. Service
    }
    catch (Exception e)
    {
        Debug.LogError($"🐛 UGS init failed: {e.Message}");
    }
}
```

**Prevention**: Search for `UnityServices.InitializeAsync()` and verify it's first.

---

### 5. IPC Message Schema Violations
**❌ Problem**: Node.js can't parse Unity messages
```csharp
// WRONG - Missing required fields
var message = new { 
    messageType = "avatar_moved",  // Should be "type"
    position = transform.position   // Should be nested in "data"
};
Console.WriteLine(JsonUtility.ToJson(message));
```

**✅ Solution**: Use exact IPCMessage schema
```csharp
var message = new IPCMessage
{
    type = "avatar:moved",  // REQUIRED: type field
    timestamp = DateTime.UtcNow.ToString("o"),  // REQUIRED: ISO8601
    data = JsonUtility.ToJson(new { position = transform.position })  // REQUIRED: data
};
Console.WriteLine(JsonUtility.ToJson(message));
```

**Prevention**: Always use `IPCMessage` class, never anonymous objects.

---

### 6. Missing `IsOwner` Checks in Networked Code
**❌ Problem**: All clients modify state, causing conflicts
```csharp
void Update()
{
    // WRONG - Every client tries to move this object!
    transform.position += velocity * Time.deltaTime;
    healthServerRpc.Value -= 0.1f;
}
```

**✅ Solution**: Guard with `IsOwner` or use RPCs
```csharp
void Update()
{
    if (!IsOwner) return;  // Only owner updates
    
    transform.position += velocity * Time.deltaTime;
    
    if (shouldTakeDamage)
    {
        TakeDamageServerRpc(0.1f);  // Request server to modify
    }
}

[ServerRpc]
void TakeDamageServerRpc(float damage)
{
    healthNetworkVariable.Value -= damage;  // Server authority
}
```

**Prevention**: Search for `NetworkBehaviour` classes and verify `IsOwner` checks.

---

### 7. ScriptableObject Instance Modifications
**❌ Problem**: Changing ScriptableObject at runtime affects ALL instances
```csharp
public ItemData swordData;  // ScriptableObject reference

public void DamageSword()
{
    // WRONG - Changes the asset file, affects all swords!
    swordData.currentDurability -= 10;
}
```

**✅ Solution**: Use instance classes for runtime state
```csharp
public class ItemInstance
{
    public ItemData itemData;  // Reference to ScriptableObject (read-only)
    public int currentDurability;  // Runtime state
    
    public ItemInstance(ItemData data)
    {
        itemData = data;
        currentDurability = data.maxDurability;  // Copy initial value
    }
}

public void DamageSword(ItemInstance sword)
{
    sword.currentDurability -= 10;  // CORRECT - modifies instance
}
```

**Prevention**: Never modify ScriptableObject fields at runtime. Use instance classes.

---

### 8. Jest Test Coverage Blind Spots
**❌ Problem**: 80% coverage but critical paths untested
```javascript
// WRONG - Only tests happy path
test('should send IPC message', () => {
    bridge.sendMessage('test', { foo: 'bar' });
    expect(bridge.process.stdin.write).toHaveBeenCalled();
});
// Missing: Error handling, null process, invalid JSON
```

**✅ Solution**: Test error paths and edge cases
```javascript
test('should send IPC message', () => {
    bridge.sendMessage('test', { foo: 'bar' });
    expect(bridge.process.stdin.write).toHaveBeenCalledWith(
        expect.stringContaining('"type":"test"')
    );
});

test('should throw if process not started', () => {
    bridge.process = null;
    expect(() => bridge.sendMessage('test', {}))
        .toThrow('Unity process not started');
});

test('should handle invalid data gracefully', () => {
    const circular = {};
    circular.self = circular;
    expect(() => bridge.sendMessage('test', circular))
        .toThrow();  // JSON.stringify fails on circular refs
});
```

**Prevention**: Run `npm test -- --coverage` and review uncovered branches.

---

### 9. Avatar Rig Configuration Errors
**❌ Problem**: IK doesn't work, bones are unmapped
```
// Unity Editor errors:
"Avatar is not configured correctly"
"Required bone 'LeftHand' is not mapped"
```

**✅ Solution**: Follow exact Humanoid rig workflow
1. FBX must be in T-pose (arms horizontal, legs straight)
2. Import Settings → Rig → Animation Type: **Humanoid**
3. Click **Configure** → Verify all bones are **green**
4. Required bones (15 minimum): Hips, Spine, Chest, Neck, Head, LeftShoulder, LeftUpperArm, LeftLowerArm, LeftHand, RightShoulder, RightUpperArm, RightLowerArm, RightHand, LeftUpperLeg, LeftLowerLeg, LeftFoot, RightUpperLeg, RightLowerLeg, RightFoot
5. Click **Apply** and **Done**

**Prevention**: Always export FBX from Blender with "Armature" checked, T-pose applied.

---

### 10. MCP Server Dependency Failures
**❌ Problem**: Sequential-thinking server fails because git isn't initialized
```bash
# WRONG order - Layer 2 before Layer 0
Starting sequential-thinking... ❌ Failed (missing git dependency)
```

**✅ Solution**: Use tiered bootstrap from `./scripts/mcp-validate.sh`
```bash
# CORRECT - Layer 0 → 1 → 2
Layer 0 (Primitives): filesystem, memory
Layer 1 (Foundation): git, github, brave-search  
Layer 2 (Advanced): sequential-thinking, postgres, everything
```

**Prevention**: Always run `./scripts/mcp-validate.sh` before starting work.

---

### 11. Emoji Commit Validation Failures
**❌ Problem**: CI rejects commits without emoji prefix
```bash
git commit -m "Add tail physics"  # ❌ REJECTED
```

**✅ Solution**: Use emoji from RELIGULOUS_MANTRA.md
```bash
git commit -m "🦋 Add tail physics with spring-damper simulation"  # ✅ ACCEPTED
git commit -m "✨ Update NetworkBehaviour ownership checks"
git commit -m "🐛 Fix animator hash caching memory leak"
git commit -m "💎 Refactor IPC message serialization"
```

**Emoji Quick Reference**:
- `🦋` New features/additions
- `✨` Updates/improvements  
- `🐛` Bug fixes
- `💎` Refactoring
- `🌸` Package management
- `👑` Architecture
- `🔥` Performance

**Prevention**: Create git commit template with emoji options.

---

### 12. Visual Effect Graph Property Mismatches
**❌ Problem**: `SetFloat()` has no effect on VFX
```csharp
// WRONG - Property name doesn't match VFX Graph
vfx.SetFloat("intensity", 2.0f);  // VFX has "IntensityMultiplier"
```

**✅ Solution**: Match exact property names from VFX Graph
```csharp
// CORRECT - Open VFX Graph, check Blackboard property names
vfx.SetFloat("IntensityMultiplier", 2.0f);  // Exact match
vfx.SetVector3("EmissionPosition", transform.position);
vfx.SendEvent("OnPurr");  // Event names also case-sensitive
```

**Prevention**: Use VFX Graph Blackboard as source of truth for property names.

