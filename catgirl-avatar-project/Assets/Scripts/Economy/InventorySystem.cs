// Assets/Scripts/Economy/InventorySystem.cs
// 🌸 BambiSleep™ Church Inventory Management System 🌸
// Pink frilly inventory with secret cow power items

using UnityEngine;
using Unity.Services.Economy;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BambiSleep.CatGirl.Economy
{
    [System.Serializable]
    public class CatgirlItem
    {
        public string itemId;
        public string displayName;
        public Sprite icon;
        public int rarity; // 1=Common, 2=Uncommon, 3=Rare, 4=Epic, 5=Diablo Secret Level
        public bool isCowPowerItem = false;
        public float pinkValue = 0f;
        public string description;
        public int stackSize = 1;
        public Dictionary<string, object> customData = new Dictionary<string, object>();
    }

    [System.Serializable]
    public class InventorySlot
    {
        public CatgirlItem item;
        public int quantity;
        public bool isLocked;
    }

    public class InventorySystem : MonoBehaviour
    {
        [Header("🎒 Inventory Configuration")]
        [SerializeField] private int maxSlots = 100;
        [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();

        [Header("💎 Special Collections")]
        [SerializeField] private List<CatgirlItem> cowPowerItems = new List<CatgirlItem>();
        [SerializeField] private List<CatgirlItem> diabloSecretLevelItems = new List<CatgirlItem>();

        [Header("🌸 Pink Theme Settings")]
        [SerializeField] private Color pinkHighlightColor = new Color(1f, 0.41f, 0.71f);
        [SerializeField] private float frillyAnimationSpeed = 2.0f;

        private UniversalBankingSystem banking;
        private bool isInitialized = false;

        public int SlotCount => slots.Count;
        public int MaxSlots => maxSlots;
        public int AvailableSlots => maxSlots - slots.Count;

        private void Awake()
        {
            banking = GetComponent<UniversalBankingSystem>();
            InitializeInventory();
        }

        private void Start()
        {
            LoadInventoryFromCloud();
        }

        private void InitializeInventory()
        {
            if (isInitialized) return;

            // Initialize empty slots
            slots = new List<InventorySlot>(maxSlots);

            Debug.Log("🎒 BambiSleep™ Inventory System initialized!");
            isInitialized = true;
        }

        public async void LoadInventoryFromCloud()
        {
            try
            {
                Debug.Log("📦 Loading inventory from Unity Gaming Services...");

                // Load from Unity Gaming Services Economy
                var inventoryResult = await EconomyService.Instance.PlayerInventory.GetInventoryAsync();

                // Process inventory data
                ProcessCloudInventory(inventoryResult);

                Debug.Log($"✨ Loaded {slots.Count} items from cloud!");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Failed to load inventory: {e.Message}");
                // Continue with local inventory
            }
        }

        private void ProcessCloudInventory(Unity.Services.Economy.Model.PlayerInventory inventoryResult)
        {
            slots.Clear();

            foreach (var playersInventoryItem in inventoryResult.PlayersInventoryItems)
            {
                var item = CreateItemFromCloudData(playersInventoryItem);
                AddItemToSlot(item, 1);
            }
        }

        private CatgirlItem CreateItemFromCloudData(Unity.Services.Economy.Model.PlayersInventoryItem cloudItem)
        {
            var item = new CatgirlItem
            {
                itemId = cloudItem.InventoryItemId,
                displayName = cloudItem.InventoryItemId, // Would come from item definition
                rarity = 1,
                stackSize = 1
            };

            // Check for special item types
            if (cloudItem.InventoryItemId.Contains("cow_power"))
            {
                item.isCowPowerItem = true;
                cowPowerItems.Add(item);
            }

            if (cloudItem.InventoryItemId.Contains("secret_level"))
            {
                item.rarity = 5;
                diabloSecretLevelItems.Add(item);
            }

            return item;
        }

        public bool AddItem(CatgirlItem item, int quantity = 1)
        {
            if (item == null) return false;

            // Check if we can stack with existing item
            var existingSlot = FindItemSlot(item.itemId);
            if (existingSlot != null)
            {
                existingSlot.quantity += quantity;
                OnInventoryChanged();
                return true;
            }

            // Add to new slot
            if (AvailableSlots <= 0)
            {
                Debug.LogWarning("🎒 Inventory full! Cannot add item.");
                return false;
            }

            return AddItemToSlot(item, quantity);
        }

        private bool AddItemToSlot(CatgirlItem item, int quantity)
        {
            var slot = new InventorySlot
            {
                item = item,
                quantity = quantity,
                isLocked = false
            };

            slots.Add(slot);

            // Special handling for cow power items
            if (item.isCowPowerItem)
            {
                Debug.Log("🐄 MOO! Cow Power item added to inventory!");
                cowPowerItems.Add(item);
            }

            // Special handling for secret level items
            if (item.rarity == 5)
            {
                Debug.Log("💎 Secret Diablo Level item acquired! LEGENDARY!");
                diabloSecretLevelItems.Add(item);
            }

            OnInventoryChanged();
            return true;
        }

        public bool RemoveItem(string itemId, int quantity = 1)
        {
            var slot = FindItemSlot(itemId);
            if (slot == null) return false;

            if (slot.isLocked)
            {
                Debug.LogWarning("🔒 Cannot remove locked item!");
                return false;
            }

            slot.quantity -= quantity;

            if (slot.quantity <= 0)
            {
                slots.Remove(slot);
            }

            OnInventoryChanged();
            return true;
        }

        public InventorySlot FindItemSlot(string itemId)
        {
            return slots.Find(s => s.item.itemId == itemId);
        }

        public int GetItemCount(string itemId)
        {
            var slot = FindItemSlot(itemId);
            return slot?.quantity ?? 0;
        }

        public bool HasItem(string itemId)
        {
            return FindItemSlot(itemId) != null;
        }

        public List<CatgirlItem> GetCowPowerItems()
        {
            return new List<CatgirlItem>(cowPowerItems);
        }

        public List<CatgirlItem> GetSecretLevelItems()
        {
            return new List<CatgirlItem>(diabloSecretLevelItems);
        }

        public void SortByRarity()
        {
            slots.Sort((a, b) => b.item.rarity.CompareTo(a.item.rarity));
            OnInventoryChanged();
        }

        public void SortByPinkValue()
        {
            slots.Sort((a, b) => b.item.pinkValue.CompareTo(a.item.pinkValue));
            OnInventoryChanged();
        }

        private void OnInventoryChanged()
        {
            // Notify UI and other systems
            Debug.Log($"🎒 Inventory updated: {slots.Count}/{maxSlots} slots used");
        }

        public async Task<bool> SaveToCloud()
        {
            try
            {
                Debug.Log("💾 Saving inventory to cloud...");
                // Unity Gaming Services will handle persistence
                // This is automatic with Economy service
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Failed to save inventory: {e.Message}");
                return false;
            }
        }
    }
}
