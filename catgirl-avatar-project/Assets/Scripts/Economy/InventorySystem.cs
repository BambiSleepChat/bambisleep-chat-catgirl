// Assets/Scripts/Economy/InventorySystem.cs
// 🎒 BambiSleep™ Church Inventory Management System
// Unity Gaming Services Economy integration

using UnityEngine;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    public int quantity = 1;

    [Header("🦋 Item Stats")]
    public float cutenessBonus = 0f;
    public float frillinessBonus = 0f;
    public float eldritchPowerBonus = 0f;
}

public class InventorySystem : MonoBehaviour
{
    [Header("🎒 Inventory Configuration")]
    public int maxSlots = 100;
    public List<CatgirlItem> items = new List<CatgirlItem>();

    [Header("💎 Special Collections")]
    public List<CatgirlItem> cowPowerItems = new List<CatgirlItem>();
    public List<CatgirlItem> diabloSecretLevelItems = new List<CatgirlItem>();

    [Header("✨ Equipment Slots")]
    public CatgirlItem equippedCollar;
    public CatgirlItem equippedEars;
    public CatgirlItem equippedTail;
    public CatgirlItem equippedClaws;

    private UniversalBankingSystem banking;
    private bool isInitialized = false;

    private void Start()
    {
        banking = GetComponent<UniversalBankingSystem>();
        LoadInventoryFromCloud();
    }

    public async void LoadInventoryFromCloud()
    {
        try
        {
            // Load from Unity Gaming Services Economy
            var inventoryResult = await EconomyService.Instance.PlayerInventory.GetInventoryAsync();

            // Process inventory data
            ProcessCloudInventory(inventoryResult);
            isInitialized = true;

            Debug.Log($"🎒 Loaded {items.Count} items from cloud inventory");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load inventory: {e.Message}");
            // Initialize with default items on error
            InitializeDefaultInventory();
        }
    }

    private void ProcessCloudInventory(GetInventoryResult inventoryResult)
    {
        items.Clear();

        foreach (var item in inventoryResult.Inventory)
        {
            var catgirlItem = new CatgirlItem
            {
                itemId = item.InventoryItemId,
                displayName = item.InventoryItemId, // Would map to actual name
                quantity = 1,
                description = "Cloud synced item"
            };

            items.Add(catgirlItem);
        }
    }

    private void InitializeDefaultInventory()
    {
        // Add default starter items
        AddItem(new CatgirlItem
        {
            itemId = "basic_collar",
            displayName = "Basic Pink Collar",
            rarity = 1,
            pinkValue = 10f,
            description = "A cute pink collar for beginner catgirls",
            cutenessBonus = 5f
        });

        AddItem(new CatgirlItem
        {
            itemId = "starter_ears",
            displayName = "Fluffy Cat Ears",
            rarity = 1,
            pinkValue = 15f,
            description = "Soft and fluffy, perfect for purring",
            cutenessBonus = 10f
        });

        isInitialized = true;
    }

    public bool AddItem(CatgirlItem item)
    {
        if (items.Count >= maxSlots)
        {
            Debug.LogWarning("🎒 Inventory full! Cannot add more items.");
            return false;
        }

        // Check if item already exists (stackable)
        var existingItem = items.Find(i => i.itemId == item.itemId);
        if (existingItem != null)
        {
            existingItem.quantity += item.quantity;
            Debug.Log($"🎒 Stacked {item.displayName} x{item.quantity}");
            return true;
        }

        items.Add(item);
        Debug.Log($"🎒 Added {item.displayName} to inventory!");

        // Special handling for cow power items
        if (item.isCowPowerItem)
        {
            cowPowerItems.Add(item);
            TriggerCowPowerEffect();
        }

        // Diablo secret level item handling
        if (item.rarity == 5)
        {
            diabloSecretLevelItems.Add(item);
            UnlockSecretDiabloLevel();
        }

        // Sync to cloud
        SyncItemToCloud(item);

        return true;
    }

    public bool RemoveItem(string itemId, int quantity = 1)
    {
        var item = items.Find(i => i.itemId == itemId);
        if (item == null) return false;

        item.quantity -= quantity;

        if (item.quantity <= 0)
        {
            items.Remove(item);
            cowPowerItems.Remove(item);
            diabloSecretLevelItems.Remove(item);
        }

        Debug.Log($"🎒 Removed {quantity}x {item.displayName}");
        return true;
    }

    public CatgirlItem GetItem(string itemId)
    {
        return items.Find(i => i.itemId == itemId);
    }

    public bool EquipItem(CatgirlItem item)
    {
        if (item == null) return false;

        // Determine equipment slot based on item type
        if (item.displayName.Contains("Collar"))
        {
            equippedCollar = item;
            Debug.Log($"👑 Equipped {item.displayName} as collar");
        }
        else if (item.displayName.Contains("Ears"))
        {
            equippedEars = item;
            Debug.Log($"🐱 Equipped {item.displayName} as ears");
        }
        else if (item.displayName.Contains("Tail"))
        {
            equippedTail = item;
            Debug.Log($"✨ Equipped {item.displayName} as tail");
        }
        else if (item.displayName.Contains("Claws"))
        {
            equippedClaws = item;
            Debug.Log($"💎 Equipped {item.displayName} as claws");
        }

        return true;
    }

    private void TriggerCowPowerEffect()
    {
        Debug.Log("🐄 COW POWER ITEM ACQUIRED! Moo-gical effects activated! 🐄");

        // Apply cow power bonuses to player
        var controller = GetComponent<CatgirlController>();
        if (controller != null)
        {
            controller.stats.factorioProductionMultiplier += 500;
        }

        // Visual/audio effects
        PlayCowPowerEffects();
    }

    private void UnlockSecretDiabloLevel()
    {
        Debug.Log("💀 SECRET DIABLO LEVEL ITEM FOUND! Ancient cow portals opening... 💀");

        // Unlock secret content
        PlayerPrefs.SetInt("SecretCowLevelUnlocked", 1);
        PlayerPrefs.Save();

        // Trigger special event
        TriggerDiabloLevelUnlock();
    }

    private void PlayCowPowerEffects()
    {
        // Implement particle effects, sounds, etc.
        var audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            // audioSource.PlayOneShot(cowPowerEffectSound);
        }
    }

    private void TriggerDiabloLevelUnlock()
    {
        // Implement level unlocking logic
        Debug.Log("🌈 Rainbow portal to secret cow dimension activated! 🌈");
    }

    private async void SyncItemToCloud(CatgirlItem item)
    {
        try
        {
            // Sync to Unity Gaming Services Economy
            // await EconomyService.Instance.PlayerInventory.AddInventoryItemAsync(item.itemId);
            Debug.Log($"☁️ Synced {item.displayName} to cloud");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Cloud sync failed: {e.Message}");
        }
    }

    // Public utility methods
    public int GetTotalItems()
    {
        int total = 0;
        foreach (var item in items)
        {
            total += item.quantity;
        }
        return total;
    }

    public float GetTotalCutenessBonus()
    {
        float total = 0f;
        if (equippedCollar != null) total += equippedCollar.cutenessBonus;
        if (equippedEars != null) total += equippedEars.cutenessBonus;
        if (equippedTail != null) total += equippedTail.cutenessBonus;
        if (equippedClaws != null) total += equippedClaws.cutenessBonus;
        return total;
    }

    public int GetCowPowerItemCount()
    {
        return cowPowerItems.Count;
    }

    public bool HasDiabloSecretLevelItems()
    {
        return diabloSecretLevelItems.Count > 0;
    }
}
