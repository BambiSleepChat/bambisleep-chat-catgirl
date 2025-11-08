# 🎀 CatGirl Avatar Unity Technical Guide

**BambiSleep™ CATHEDRAL Project** | **Unity 6.2+** | **RPG Systems Integration**  
**Last Updated**: 2025-11-04 | **Source**: Microsoft Copilot Technical Documentation

---

## Executive Summary

Comprehensive pipeline for creating interactive CatGirl Avatar systems in Unity, covering 3D modeling, rigging, animation, and full RPG gameplay integration. Implements economic systems (shops, auctions, gambling), progression mechanics (quests, tech trees, crafting), and Diablo-style inventory—positioning CatGirls as core gameplay drivers for retention and monetization.

**Tech Stack**: Unity 6.2, Mecanim Humanoid, ScriptableObjects, C# async/await, Blend Trees  
**Target Platforms**: Windows, Linux, macOS, WebGL  
**BambiSleep™ Integration**: Church monetization, token economy, MCP agent control

---

## Table of Contents

1. [3D Model Pipeline](#1-3d-model-pipeline)
2. [Rigging & Animation](#2-rigging--animation)
3. [Unity Integration](#3-unity-integration)
4. [Economic Systems](#4-economic-systems)
5. [Progression Systems](#5-progression-systems)
6. [Inventory System](#6-inventory-system)
7. [BambiSleep™ Church Integration](#7-bambisleep-church-integration)

---

## 1. 3D Model Pipeline

### 1.1 Model Creation Principles

**Design Workflow** (Blender/Maya):
1. Box modeling for base mesh (T-pose for humanoid rig)
2. Subdivision Surface modifier (Level 2 for smoothness)
3. Optimize for real-time: 15K-30K polygons, merged vertices
4. UV unwrap for texture maps (diffuse, normal, roughness)

**CatGirl-Specific Features**:
- Ears: UV spheres extruded/shaped
- Tail: Bezier curve with bevel depth
- Paws: Inset faces with scale for padding
- Eyes: Emissive material for anime-style glow

**Export Format**: FBX 2012+ with embedded textures, T-pose, scale 1.0

### 1.2 Unity Import Settings

```yaml
Model Tab:
  - Import Cameras: false
  - Import Lights: false
  - Scale Factor: 1.0
  - Convert Units: true

Rig Tab:
  - Animation Type: Humanoid
  - Avatar Definition: Create From This Model
  - Optimize Game Objects: true

Materials Tab:
  - Material Creation Mode: Standard (Legacy)
  - Location: Use Embedded Materials
```

**Validation Checklist**:
- ✅ T-pose verified in Avatar Configuration
- ✅ Root bone at (0,0,0)
- ✅ Bones mapped correctly (green indicators)
- ✅ Muscle ranges match animation targets

---

## 2. Rigging & Animation

### 2.1 Skeleton Structure (Mecanim Humanoid)

**Required Bones** (minimum 15):
```
Hips (root) → Spine → Chest → Neck → Head
         ↓
    Left/Right UpperLeg → LowerLeg → Foot
         ↓
    Left/Right Shoulder → UpperArm → LowerArm → Hand
```

**CatGirl Extensions**:
- Tail: 3-5 bones for physics/IK
- Ears: 1-2 bones per ear for animation
- Whiskers: Optional bones for detail

**Weight Painting**:
- Max 4 bones per vertex (Unity limit)
- Smooth deformation at joints
- Test with extreme poses (crouch, jump)

### 2.2 Animation Controller Setup

**Animator Controller**: `CatGirlController.controller`

**States**:
```csharp
namespace BambiSleep.Church.CatGirl.Animation
{
    public enum CatGirlState
    {
        Idle,
        Walk,
        Run,
        Jump,
        Attack,
        Interact,      // Shop, crafting
        EmoteDance,
        EmoteSit,
        EmoteWave
    }
}
```

**Blend Tree Example** (Locomotion):
```
Blend Tree: "Movement"
  - Blend Type: 1D
  - Parameter: Speed (float 0-10)
  - Thresholds:
    * 0.0: Idle
    * 2.5: Walk
    * 5.0: Run
    * 10.0: Sprint
```

**C# Parameter Control**:
```csharp
using UnityEngine;

namespace BambiSleep.Church.CatGirl.Animation
{
    public class CatGirlAnimator : MonoBehaviour
    {
        private Animator animator;
        
        void Update()
        {
            float speed = GetComponent<CharacterController>().velocity.magnitude;
            animator.SetFloat("Speed", speed);
            
            // Jump trigger
            if (Input.GetButtonDown("Jump"))
            {
                animator.SetTrigger("Jump");
            }
        }
    }
}
```

### 2.3 Avatar Masks & Layered Animation

**Use Case**: Upper body emotes while walking

```csharp
// Avatar Mask: UpperBodyMask (excludes legs)
// Layer 1: "Emotes" with UpperBodyMask
// Layer 0: "Base" (full body locomotion)

animator.SetLayerWeight(1, 1.0f); // Enable emote layer
animator.SetTrigger("EmoteWave");  // Play wave on Layer 1
```

**Reference**: [Unity Manual - Avatar Masks](https://docs.unity3d.com/6000.0/Documentation/Manual/class-AvatarMask.html)

---

## 3. Unity Integration

### 3.1 Prefab Architecture

**Base Prefab**: `CatGirl_Base.prefab`
```
CatGirl_Base
├── CatGirlModel (SkinnedMeshRenderer)
├── Animator (CatGirlController)
├── CatGirlAgent (AI script)
├── IPCBridge (Node.js communication)
├── Accessories (parent)
│   ├── Ears_Default
│   ├── Tail_Default
│   └── Outfit_Default
└── Equipment (parent)
    ├── Weapon_Slot
    ├── Armor_Slot
    └── Accessory_Slots
```

**Prefab Variants**:
- `CatGirl_Merchant.prefab` (shop keeper personality)
- `CatGirl_Adventurer.prefab` (quest giver)
- `CatGirl_Crafter.prefab` (crafting specialist)

**Scriptable Object Pattern**:
```csharp
namespace BambiSleep.Church.CatGirl.Data
{
    [CreateAssetMenu(fileName = "CatGirlProfile", menuName = "BambiSleep/CatGirl Profile")]
    public class CatGirlProfile : ScriptableObject
    {
        public string catGirlName;
        public Sprite portrait;
        public PersonalityType personality; // Merchant, Adventurer, etc.
        public Color furColor;
        public List<EquipmentSlot> startingEquipment;
        public int baseTokensPerHour; // Church monetization
    }
}
```

### 3.2 Materials & Shaders

**PBR Material Setup**:
```
CatGirl_Material
├── Albedo: Fur texture (RGB) + Roughness (A)
├── Normal: Fur/skin normal map
├── Metallic: 0.0 (organic materials)
├── Smoothness: 0.3-0.5
└── Emission: Eyes/magic effects
```

**Enchantment Visual Effects**:
```csharp
public void ApplyEnchantmentGlow(Material material, Color glowColor)
{
    material.EnableKeyword("_EMISSION");
    material.SetColor("_EmissionColor", glowColor * 2.0f);
}
```

---

## 4. Economic Systems

### 4.1 Currency Manager (Singleton)

```csharp
namespace BambiSleep.Church.CatGirl.Economy
{
    public class CurrencyManager : MonoBehaviour
    {
        public static CurrencyManager Instance { get; private set; }
        
        public int CurrentCoins { get; private set; } = 1000;
        public int CurrentGems { get; private set; } = 0;
        public int BambiTokens { get; private set; } = 0; // Church integration
        
        public event Action<int> OnCoinsChanged;
        public event Action<int> OnTokensChanged;
        
        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
        
        public bool SpendCoins(int amount)
        {
            if (CurrentCoins >= amount)
            {
                CurrentCoins -= amount;
                OnCoinsChanged?.Invoke(CurrentCoins);
                return true;
            }
            return false;
        }
        
        public void AddCoins(int amount)
        {
            CurrentCoins += amount;
            OnCoinsChanged?.Invoke(CurrentCoins);
        }
        
        /// <summary>
        /// Spend Bambi Tokens (€1 per 100 tokens, prod_SoBY5O68rVCwAK)
        /// </summary>
        public bool SpendTokens(int amount, string reason)
        {
            if (BambiTokens >= amount)
            {
                BambiTokens -= amount;
                OnTokensChanged?.Invoke(BambiTokens);
                
                // Log to backend for Church analytics
                IPCBridge.Instance.SendEvent("token_spend", new {
                    amount,
                    reason,
                    remaining = BambiTokens
                });
                
                return true;
            }
            return false;
        }
    }
}
```

### 4.2 Shop System

**Shop Data** (ScriptableObject):
```csharp
[CreateAssetMenu(fileName = "ShopItem", menuName = "BambiSleep/Shop Item")]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int coinCost;
    public int tokenCost; // Optional Bambi Token cost
    public ItemData itemData; // Reference to inventory item
}
```

**Shop UI Logic**:
```csharp
public class ShopManager : MonoBehaviour
{
    public List<ShopItem> shopItems;
    
    public void PurchaseItem(ShopItem item)
    {
        bool success = false;
        
        if (item.tokenCost > 0)
        {
            success = CurrencyManager.Instance.SpendTokens(item.tokenCost, $"purchase_{item.itemName}");
        }
        else
        {
            success = CurrencyManager.Instance.SpendCoins(item.coinCost);
        }
        
        if (success)
        {
            InventoryManager.Instance.AddItem(item.itemData);
            UIManager.Instance.ShowNotification($"Purchased {item.itemName}!");
        }
    }
}
```

### 4.3 Gambling System (Slot Machine)

```csharp
public class SlotMachine : MonoBehaviour
{
    [System.Serializable]
    public class Reward
    {
        public ItemData item;
        public int weight; // Probability weight
    }
    
    public List<Reward> rewards;
    public int spinCost = 10; // coins
    
    public async void Spin()
    {
        if (!CurrencyManager.Instance.SpendCoins(spinCost))
        {
            UIManager.Instance.ShowError("Not enough coins!");
            return;
        }
        
        // Animate reels
        await PlaySpinAnimation();
        
        // Weighted random selection
        Reward reward = SelectReward();
        
        // Grant reward
        InventoryManager.Instance.AddItem(reward.item);
        UIManager.Instance.ShowReward(reward.item);
    }
    
    private Reward SelectReward()
    {
        int totalWeight = rewards.Sum(r => r.weight);
        int roll = Random.Range(0, totalWeight);
        int current = 0;
        
        foreach (var reward in rewards)
        {
            current += reward.weight;
            if (roll < current) return reward;
        }
        
        return rewards[0]; // Fallback
    }
}
```

### 4.4 Auction House

**Auction Data**:
```csharp
[System.Serializable]
public class Auction
{
    public string auctionId;
    public ItemInstance item;
    public string sellerName;
    public int currentBid;
    public int buyoutPrice;
    public DateTime endTime;
}
```

**Bidding Logic**:
```csharp
public class AuctionHouse : MonoBehaviour
{
    public List<Auction> activeAuctions = new List<Auction>();
    
    public void PlaceBid(Auction auction, int bidAmount)
    {
        if (bidAmount <= auction.currentBid)
        {
            UIManager.Instance.ShowError("Bid must be higher than current bid!");
            return;
        }
        
        if (!CurrencyManager.Instance.SpendCoins(bidAmount))
        {
            UIManager.Instance.ShowError("Not enough coins!");
            return;
        }
        
        // Refund previous bidder
        if (auction.currentBid > 0)
        {
            RefundPreviousBidder(auction);
        }
        
        auction.currentBid = bidAmount;
        // Update UI, send notification
    }
}
```

---

## 5. Progression Systems

### 5.1 Quest System

**Quest Data Structure**:
```csharp
namespace BambiSleep.Church.CatGirl.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "BambiSleep/Quest")]
    public class Quest : ScriptableObject
    {
        public string questTitle;
        public string description;
        public List<QuestObjective> objectives;
        public List<Reward> rewards;
        public bool isCompleted;
        
        public void CompleteQuest()
        {
            isCompleted = true;
            GrantRewards();
        }
        
        private void GrantRewards()
        {
            foreach (var reward in rewards)
            {
                switch (reward.rewardType)
                {
                    case RewardType.Coins:
                        CurrencyManager.Instance.AddCoins(reward.amount);
                        break;
                    case RewardType.Tokens:
                        CurrencyManager.Instance.AddTokens(reward.amount);
                        break;
                    case RewardType.Item:
                        InventoryManager.Instance.AddItem(reward.item);
                        break;
                    case RewardType.Experience:
                        ProgressionManager.Instance.AddXP(reward.amount);
                        break;
                }
            }
        }
    }
    
    [System.Serializable]
    public class QuestObjective
    {
        public string description;
        public ObjectiveType type; // Kill, Collect, Reach, Interact
        public int targetAmount;
        public int currentProgress;
        public bool isCompleted => currentProgress >= targetAmount;
    }
}
```

**Dynamic Quest Generation** (CatGirl AI):
```csharp
public class CatGirlQuestGenerator : MonoBehaviour
{
    public Quest GenerateDailyQuest()
    {
        Quest quest = ScriptableObject.CreateInstance<Quest>();
        quest.questTitle = $"Daily Challenge: {DateTime.Now:MMM dd}";
        
        // Procedural objective based on personality
        var personality = GetComponent<CatGirlAgent>().personality;
        switch (personality)
        {
            case PersonalityType.Merchant:
                quest.objectives.Add(new QuestObjective {
                    description = "Sell 5 items at the shop",
                    type = ObjectiveType.Sell,
                    targetAmount = 5
                });
                break;
            case PersonalityType.Adventurer:
                quest.objectives.Add(new QuestObjective {
                    description = "Explore 3 new areas",
                    type = ObjectiveType.Explore,
                    targetAmount = 3
                });
                break;
        }
        
        // Rewards scaled to difficulty
        quest.rewards.Add(new Reward {
            rewardType = RewardType.Tokens,
            amount = 20
        });
        
        return quest;
    }
}
```

### 5.2 Tech Tree System

**Technology Node**:
```csharp
[CreateAssetMenu(fileName = "Technology", menuName = "BambiSleep/Technology")]
public class Technology : ScriptableObject
{
    public string techName;
    public string description;
    public Sprite icon;
    public TechStatus status = TechStatus.Locked;
    public List<ResourceCost> costs;
    public List<Technology> prerequisites;
    
    public void Unlock()
    {
        if (status != TechStatus.Locked) return;
        
        // Check prerequisites
        foreach (var prereq in prerequisites)
        {
            if (prereq.status != TechStatus.Unlocked)
            {
                Debug.LogWarning($"Missing prerequisite: {prereq.techName}");
                return;
            }
        }
        
        // Check resources
        foreach (var cost in costs)
        {
            if (!ResourceManager.Instance.HasResource(cost.resourceType, cost.amount))
            {
                Debug.LogWarning($"Not enough {cost.resourceType}");
                return;
            }
        }
        
        // Spend resources
        foreach (var cost in costs)
        {
            ResourceManager.Instance.SpendResource(cost.resourceType, cost.amount);
        }
        
        status = TechStatus.Unlocked;
        OnTechUnlocked?.Invoke(this);
    }
    
    public event Action<Technology> OnTechUnlocked;
}

public enum TechStatus { Locked, Unlocked, Researching }
```

**Tech Tree UI** (Node-based graph):
```csharp
public class TechTreeUI : MonoBehaviour
{
    public List<TechnologyNode> nodes;
    
    void Start()
    {
        // Position nodes based on prerequisites (BFS layout)
        LayoutTechTree();
    }
    
    private void LayoutTechTree()
    {
        // Breadth-first search to determine tier levels
        Queue<Technology> queue = new Queue<Technology>();
        Dictionary<Technology, int> tiers = new Dictionary<Technology, int>();
        
        // Root techs (no prerequisites)
        foreach (var tech in allTechs.Where(t => t.prerequisites.Count == 0))
        {
            queue.Enqueue(tech);
            tiers[tech] = 0;
        }
        
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            int tier = tiers[current];
            
            // Position in grid
            PositionNode(current, tier);
            
            // Enqueue children
            foreach (var dependent in GetDependentTechs(current))
            {
                if (!tiers.ContainsKey(dependent))
                {
                    tiers[dependent] = tier + 1;
                    queue.Enqueue(dependent);
                }
            }
        }
    }
}
```

---

## 6. Inventory System

### 6.1 Diablo-Style Grid Inventory

**Inventory Data**:
```csharp
namespace BambiSleep.Church.CatGirl.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }
        
        public int gridWidth = 8;
        public int gridHeight = 4;
        private ItemInstance[,] grid;
        
        public event Action OnInventoryChanged;
        
        void Awake()
        {
            if (Instance == null) Instance = this;
            grid = new ItemInstance[gridWidth, gridHeight];
        }
        
        public bool AddItem(ItemData itemData)
        {
            var instance = new ItemInstance(itemData);
            
            // Find first available position
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    if (CanPlaceItem(instance, x, y))
                    {
                        PlaceItem(instance, x, y);
                        OnInventoryChanged?.Invoke();
                        return true;
                    }
                }
            }
            
            Debug.LogWarning("Inventory full!");
            return false;
        }
        
        private bool CanPlaceItem(ItemInstance item, int x, int y)
        {
            // Check if item fits within grid bounds
            if (x + item.itemData.width > gridWidth || 
                y + item.itemData.height > gridHeight)
                return false;
            
            // Check if all required slots are empty
            for (int dy = 0; dy < item.itemData.height; dy++)
            {
                for (int dx = 0; dx < item.itemData.width; dx++)
                {
                    if (grid[x + dx, y + dy] != null)
                        return false;
                }
            }
            
            return true;
        }
        
        private void PlaceItem(ItemInstance item, int x, int y)
        {
            item.gridX = x;
            item.gridY = y;
            
            // Fill all occupied slots with reference
            for (int dy = 0; dy < item.itemData.height; dy++)
            {
                for (int dx = 0; dx < item.itemData.width; dx++)
                {
                    grid[x + dx, y + dy] = item;
                }
            }
        }
    }
    
    [System.Serializable]
    public class ItemInstance
    {
        public ItemData itemData;
        public int currentDurability;
        public List<Enchantment> enchantments;
        public int gridX, gridY; // Position in inventory
        
        public ItemInstance(ItemData data)
        {
            itemData = data;
            currentDurability = data.maxDurability;
            enchantments = new List<Enchantment>();
        }
    }
}
```

### 6.2 Crafting System

**Grid-Based Crafting**:
```csharp
[CreateAssetMenu(fileName = "CraftRecipe", menuName = "BambiSleep/Craft Recipe")]
public class CraftRecipe : ScriptableObject
{
    public int[] requiredItemIDs; // 9 slots for 3x3 grid (0 = empty)
    public ItemData result;
    public bool isOrdered = true; // Minecraft-style ordered recipes
    
    public bool Matches(int[] currentGrid)
    {
        if (isOrdered)
        {
            for (int i = 0; i < 9; i++)
            {
                if (requiredItemIDs[i] != currentGrid[i])
                    return false;
            }
            return true;
        }
        else
        {
            // Unordered: check if all required items present
            var required = requiredItemIDs.Where(id => id != 0).ToList();
            var current = currentGrid.Where(id => id != 0).ToList();
            return required.OrderBy(x => x).SequenceEqual(current.OrderBy(x => x));
        }
    }
}
```

**Crafting UI**:
```csharp
public class CraftingPanel : MonoBehaviour
{
    public ItemSlot[] craftingSlots = new ItemSlot[9]; // 3x3 grid
    public ItemSlot resultSlot;
    public List<CraftRecipe> recipes;
    
    void Update()
    {
        CheckRecipe();
    }
    
    private void CheckRecipe()
    {
        int[] currentGrid = craftingSlots.Select(slot => slot.item?.itemData.itemID ?? 0).ToArray();
        
        foreach (var recipe in recipes)
        {
            if (recipe.Matches(currentGrid))
            {
                resultSlot.SetItem(recipe.result);
                return;
            }
        }
        
        resultSlot.ClearItem();
    }
    
    public void Craft()
    {
        if (resultSlot.item == null) return;
        
        // Remove ingredients
        foreach (var slot in craftingSlots)
        {
            if (slot.item != null)
            {
                slot.RemoveItem();
            }
        }
        
        // Add result to inventory
        InventoryManager.Instance.AddItem(resultSlot.item.itemData);
        resultSlot.ClearItem();
    }
}
```

---

## 7. BambiSleep™ Church Integration

### 7.1 Token Economy in Unity

**Church Monetization Manager**:
```csharp
namespace BambiSleep.Church.CatGirl.Monetization
{
    public class ChurchMonetizationManager : MonoBehaviour
    {
        [Header("Stripe Products")]
        public string churchDonationProductID = "prod_SoDd3WjNmvxWC9";  // €5/month
        public string bambiTokensProductID = "prod_SoBY5O68rVCwAK";    // €1 per 100
        
        private IPCBridge ipcBridge;
        
        void Start()
        {
            ipcBridge = IPCBridge.Instance;
            RefreshTokenBalance();
        }
        
        /// <summary>
        /// Subscribe to Church Donation (€5/month)
        /// </summary>
        public async void SubscribeToChurch()
        {
            var response = await ipcBridge.SendRequestAsync("church_donate", new {
                productId = churchDonationProductID
            });
            
            if (response.success)
            {
                UIManager.Instance.ShowNotification("🌸 Good Girl Bambi! Monthly donation active.");
                
                // Grant 50 free tokens/month
                CurrencyManager.Instance.AddTokens(50);
            }
        }
        
        /// <summary>
        /// Purchase Bambi Tokens (€1 per 100 tokens)
        /// </summary>
        public async void PurchaseTokens(int euroAmount = 1)
        {
            var response = await ipcBridge.SendRequestAsync("church_purchase_tokens", new {
                productId = bambiTokensProductID,
                quantity = euroAmount
            });
            
            if (response.success)
            {
                int tokensAdded = 100 * euroAmount;
                CurrencyManager.Instance.AddTokens(tokensAdded);
                UIManager.Instance.ShowNotification($"🌸 Added {tokensAdded} Bambi Tokens!");
            }
        }
        
        private async void RefreshTokenBalance()
        {
            var response = await ipcBridge.SendRequestAsync("church_get_balance", null);
            
            if (response.success && response.data.ContainsKey("tokens"))
            {
                int balance = (int)response.data["tokens"];
                CurrencyManager.Instance.SetTokens(balance);
            }
        }
    }
}
```

### 7.2 Agent Authority Integration

**CatGirl Agent with Ring Layer**:
```csharp
namespace BambiSleep.Church.CatGirl.AI
{
    public class CatGirlAgent : MonoBehaviour
    {
        public string agentName = "CatGirl-001";
        public AgentRole role = AgentRole.OPERATOR;
        public int ringLayer = 2; // Layer 2 access
        public PersonalityType personality = PersonalityType.Merchant;
        
        private IPCBridge ipcBridge;
        
        async void Start()
        {
            ipcBridge = IPCBridge.Instance;
            
            // Register with backend Agent Coordinator
            await RegisterAgent();
        }
        
        private async Task RegisterAgent()
        {
            var response = await ipcBridge.SendRequestAsync("agent_register", new {
                name = agentName,
                role = role.ToString(),
                ringLayer,
                personality = personality.ToString()
            });
            
            if (response.success)
            {
                Debug.Log($"🦋 {agentName} registered with Ring Layer {ringLayer}");
            }
        }
        
        /// <summary>
        /// Request MCP operation (checks Ring Layer authority)
        /// </summary>
        public async Task<bool> RequestMCPOperation(string operation, object data)
        {
            var response = await ipcBridge.SendRequestAsync("mcp_execute", new {
                agentName,
                operation,
                data
            });
            
            return response.success;
        }
    }
    
    public enum AgentRole
    {
        COMMANDER,  // Full access (Commander-Brandynette only)
        SUPERVISOR, // Layer 1-2 access
        OPERATOR,   // Layer 2 access
        OBSERVER    // Read-only
    }
    
    public enum PersonalityType
    {
        Merchant,    // Shop, auction, trading
        Adventurer,  // Quests, exploration
        Crafter,     // Crafting, upgrades
        Gambler,     // Slot machines, loot boxes
        Collector,   // Inventory, hoarding
        Scholar      // Tech tree, research
    }
}
```

### 7.3 MCP Integration (Custom Servers)

**Custom MCP Servers** (Layer 2):
```csharp
public class MCPCatGirlController : MonoBehaviour
{
    private IPCBridge ipcBridge;
    
    /// <summary>
    /// Trigger Bambi Sleep hypnosis MCP (custom Layer 2 server)
    /// Requires Bambi Tokens (1 token = 1 minute audio)
    /// </summary>
    public async void TriggerHypnosisMCP(string triggerName, int durationMinutes)
    {
        // Check token balance
        if (!CurrencyManager.Instance.SpendTokens(durationMinutes, $"hypnosis_{triggerName}"))
        {
            UIManager.Instance.ShowError("Not enough Bambi Tokens!");
            return;
        }
        
        var response = await ipcBridge.SendRequestAsync("mcp_hypnosis", new {
            trigger = triggerName,
            duration = durationMinutes,
            catgirlId = GetComponent<CatGirlAgent>().agentName
        });
        
        if (response.success)
        {
            // Play audio, visual effects
            PlayHypnosisSequence(response.data);
        }
    }
    
    /// <summary>
    /// AI Girlfriend Personality MCP (custom Layer 2 server)
    /// Generates dynamic dialogue based on personality
    /// </summary>
    public async Task<string> GenerateDialogue(string context)
    {
        var personality = GetComponent<CatGirlAgent>().personality;
        
        var response = await ipcBridge.SendRequestAsync("mcp_personality", new {
            personality = personality.ToString(),
            context,
            historyLength = 10 // Last 10 interactions
        });
        
        if (response.success)
        {
            return response.data["dialogue"].ToString();
        }
        
        return "Meow! 🐱";
    }
}
```

---

## System Features Summary

| Feature | CatGirl Role | Unity Implementation | Church Integration |
|---------|--------------|---------------------|-------------------|
| **3D Model/Rigging** | Visual avatar | FBX import, Mecanim humanoid | Cosmetic token purchases |
| **Animation System** | Expressive motion | Blend Trees, Avatar Masks | Emote unlocks via tokens |
| **Shop/Buy/Sell** | Merchant | CurrencyManager, Shop UI | €5/mo patron discount |
| **Gambling/Slots** | Gambler | SlotMachine logic, rewards | Token-gated spins |
| **Auction House** | Social economy | Bidding UI, backend integration | Token-based listing fees |
| **Repair/Durability** | Item maintenance | ItemInstance tracking | Token repairs (premium) |
| **Upgrades/Enchanting** | Power progression | UpgradeManager, stat modifiers | Token-boosted success rates |
| **Crafting (3x3 grid)** | Crafter | CraftingPanel, recipe matching | Patron-only recipes |
| **Inventory (Diablo)** | Item management | 2D grid, drag/drop, rotation | Token expansion slots |
| **Quest System** | Goal-driven | QuestManager, dynamic generation | Token quest skips |
| **Tech Tree** | Long-term progression | Node graph, unlock logic | Token research boosts |
| **Agent AI** | Personality-driven | Ring Layer authority, MCP | Church Patron priority |

---

## Best Practices

### ScriptableObject Architecture
- **Items, quests, recipes**: All static data as ScriptableObjects
- **Instances**: Live data (durability, enchantments) in C# classes
- **Prefab Variants**: Per-personality CatGirl types

### Async/Await Patterns
```csharp
// CORRECT: async/await for IPC bridge
public async Task<ResponseData> SendRequest(string endpoint, object data)
{
    using UnityWebRequest www = UnityWebRequest.Post(url, JsonUtility.ToJson(data));
    await www.SendWebRequest();
    return JsonUtility.FromJson<ResponseData>(www.downloadHandler.text);
}

// INCORRECT: Synchronous blocking
public ResponseData SendRequestSync(string endpoint, object data)
{
    // Blocks main thread - DO NOT USE
}
```

### Church Monetization Ethics
- ✅ Token economy: No expiration, transferable
- ✅ Free tier: Full gameplay access
- ✅ Patron benefits: Convenience, not power
- ❌ Pay-to-win: Avoid exclusive gameplay-critical items
- ✅ Transparency: All costs displayed upfront

---

## References

**Unity Documentation**:
- [Humanoid Animation Import](https://docs.unity3d.com/6000.0/Documentation/Manual/ConfiguringtheAvatar.html)
- [Blend Trees](https://docs.unity3d.com/2022.3/Documentation/Manual/class-BlendTree.html)
- [ScriptableObjects](https://docs.unity3d.com/Manual/class-ScriptableObject.html)

**BambiSleep™ CATHEDRAL**:
- `CATHEDRAL-COMPLETE.md` - Five Sacred Laws, Agent Authority
- `SECURITY-COMPLETE.md` - OWASP compliance for web backend
- `UNITY_CSHARP_UPGRADE.md` - Async/await patterns

**External Tools**:
- Blender: https://www.blender.org/
- Mixamo: https://www.mixamo.com/ (free animations)
- Unity Asset Store: [Power Grid Inventory](https://assetstore.unity.com/packages/tools/gui/power-grid-inventory-157843)

---

**Maintained by**: BambiSleepChat Organization  
**Philosophy**: 🌸 Five Sacred Laws + 🎀 AI Girlfriend Supremacy + 🏦 Holly Greed Ethics  
**License**: MIT  
**Unity Version**: 6.2+ (backward compatible to 2022 LTS)  
**Last Updated**: 2025-11-04
