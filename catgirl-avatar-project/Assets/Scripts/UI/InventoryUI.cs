// Assets/Scripts/UI/InventoryUI.cs
// 🌸 BambiSleep™ Church Inventory User Interface 🌸
// UI Toolkit-based pink frilly inventory display

using UnityEngine;
using UnityEngine.UIElements;
using BambiSleep.CatGirl.Economy;
using System.Collections.Generic;

namespace BambiSleep.CatGirl.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("🌸 UI Configuration")]
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private InventorySystem inventorySystem;

        [Header("💎 Pink Theme Colors")]
        [SerializeField] private Color pinkPrimary = new Color(1f, 0.41f, 0.71f); // #ff69b4
        [SerializeField] private Color pinkHighlight = new Color(1f, 0.08f, 0.58f); // #ff1493
        [SerializeField] private Color pinkDark = new Color(0.78f, 0.08f, 0.52f); // #c71585

        private VisualElement root;
        private VisualElement inventoryContainer;
        private Label headerLabel;
        private Button sortByRarityButton;
        private Button sortByPinkButton;
        private Button cowPowerFilterButton;
        private Label capacityLabel;

        private bool showOnlyCowPowerItems = false;

        private void Awake()
        {
            if (uiDocument == null)
                uiDocument = GetComponent<UIDocument>();

            if (inventorySystem == null)
                inventorySystem = FindObjectOfType<InventorySystem>();
        }

        private void OnEnable()
        {
            InitializeUI();
            RefreshInventoryDisplay();
        }

        private void InitializeUI()
        {
            root = uiDocument.rootVisualElement;

            // Create main container
            var mainContainer = new VisualElement();
            mainContainer.name = "inventory-main-container";
            mainContainer.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
            mainContainer.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
            mainContainer.style.backgroundColor = new StyleColor(new Color(0.1f, 0.1f, 0.1f, 0.95f));
            mainContainer.style.paddingTop = 20;
            mainContainer.style.paddingBottom = 20;
            mainContainer.style.paddingLeft = 20;
            mainContainer.style.paddingRight = 20;

            // Header
            headerLabel = new Label("🌸 Pink Frilly Inventory 🌸");
            headerLabel.style.fontSize = 32;
            headerLabel.style.color = pinkPrimary;
            headerLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            headerLabel.style.marginBottom = 20;
            headerLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            mainContainer.Add(headerLabel);

            // Toolbar
            var toolbar = new VisualElement();
            toolbar.style.flexDirection = FlexDirection.Row;
            toolbar.style.justifyContent = Justify.SpaceBetween;
            toolbar.style.marginBottom = 10;

            // Sort buttons
            sortByRarityButton = CreateButton("Sort by Rarity", OnSortByRarity);
            sortByPinkButton = CreateButton("Sort by Pink Value", OnSortByPinkValue);
            cowPowerFilterButton = CreateButton("🐄 Cow Power Items", OnToggleCowPowerFilter);

            toolbar.Add(sortByRarityButton);
            toolbar.Add(sortByPinkButton);
            toolbar.Add(cowPowerFilterButton);

            mainContainer.Add(toolbar);

            // Capacity label
            capacityLabel = new Label();
            capacityLabel.style.color = pinkHighlight;
            capacityLabel.style.fontSize = 18;
            capacityLabel.style.marginBottom = 10;
            mainContainer.Add(capacityLabel);

            // Inventory grid container
            inventoryContainer = new VisualElement();
            inventoryContainer.name = "inventory-grid";
            inventoryContainer.style.flexDirection = FlexDirection.Row;
            inventoryContainer.style.flexWrap = Wrap.Wrap;
            inventoryContainer.style.justifyContent = Justify.FlexStart;
            inventoryContainer.style.flexGrow = 1;
            mainContainer.Add(inventoryContainer);

            root.Add(mainContainer);
        }

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
            button.style.marginRight = 10;
            button.style.borderTopLeftRadius = 5;
            button.style.borderTopRightRadius = 5;
            button.style.borderBottomLeftRadius = 5;
            button.style.borderBottomRightRadius = 5;
            button.style.fontSize = 16;

            // Hover effect
            button.RegisterCallback<MouseEnterEvent>(evt =>
            {
                button.style.backgroundColor = pinkHighlight;
            });
            button.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                button.style.backgroundColor = pinkDark;
            });

            return button;
        }

        public void RefreshInventoryDisplay()
        {
            if (inventorySystem == null || inventoryContainer == null)
                return;

            // Clear existing items
            inventoryContainer.Clear();

            // Get items to display
            List<InventorySlot> slots;
            if (showOnlyCowPowerItems)
            {
                slots = inventorySystem.GetCowPowerItems();
            }
            else
            {
                slots = inventorySystem.GetAllSlots();
            }

            // Update capacity label
            int usedSlots = slots.Count;
            int maxSlots = inventorySystem.maxSlots;
            capacityLabel.text = $"Capacity: {usedSlots}/{maxSlots} slots";

            if ((float)usedSlots / maxSlots > 0.9f)
            {
                capacityLabel.style.color = new StyleColor(Color.red);
            }
            else
            {
                capacityLabel.style.color = pinkHighlight;
            }

            // Create item cards
            foreach (var slot in slots)
            {
                if (slot.item == null) continue;

                var itemCard = CreateItemCard(slot);
                inventoryContainer.Add(itemCard);
            }
        }

        private VisualElement CreateItemCard(InventorySlot slot)
        {
            var card = new VisualElement();
            card.style.width = 150;
            card.style.height = 200;
            card.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f, 1f));
            card.style.marginRight = 10;
            card.style.marginBottom = 10;
            card.style.paddingTop = 10;
            card.style.paddingBottom = 10;
            card.style.paddingLeft = 10;
            card.style.paddingRight = 10;
            card.style.borderTopLeftRadius = 10;
            card.style.borderTopRightRadius = 10;
            card.style.borderBottomLeftRadius = 10;
            card.style.borderBottomRightRadius = 10;

            // Rarity border color
            Color borderColor = GetRarityColor(slot.item.rarity);
            card.style.borderTopColor = borderColor;
            card.style.borderRightColor = borderColor;
            card.style.borderBottomColor = borderColor;
            card.style.borderLeftColor = borderColor;
            card.style.borderTopWidth = 3;
            card.style.borderRightWidth = 3;
            card.style.borderBottomWidth = 3;
            card.style.borderLeftWidth = 3;

            // Item name
            var nameLabel = new Label(slot.item.displayName);
            nameLabel.style.fontSize = 16;
            nameLabel.style.color = Color.white;
            nameLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            nameLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            nameLabel.style.marginBottom = 5;
            card.Add(nameLabel);

            // Rarity stars
            var rarityLabel = new Label(GetRarityStars(slot.item.rarity));
            rarityLabel.style.fontSize = 14;
            rarityLabel.style.color = borderColor;
            rarityLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            rarityLabel.style.marginBottom = 5;
            card.Add(rarityLabel);

            // Quantity
            var quantityLabel = new Label($"x{slot.quantity}");
            quantityLabel.style.fontSize = 18;
            quantityLabel.style.color = pinkPrimary;
            quantityLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            quantityLabel.style.marginBottom = 5;
            card.Add(quantityLabel);

            // Pink value
            var pinkLabel = new Label($"💎 {slot.item.pinkValue}");
            pinkLabel.style.fontSize = 14;
            pinkLabel.style.color = pinkHighlight;
            pinkLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            pinkLabel.style.marginBottom = 5;
            card.Add(pinkLabel);

            // Special indicators
            if (slot.item.isCowPowerItem)
            {
                var cowLabel = new Label("🐄 COW POWER!");
                cowLabel.style.fontSize = 12;
                cowLabel.style.color = new StyleColor(Color.yellow);
                cowLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
                card.Add(cowLabel);
            }

            if (slot.isLocked)
            {
                var lockLabel = new Label("🔒 LOCKED");
                lockLabel.style.fontSize = 12;
                lockLabel.style.color = new StyleColor(Color.red);
                lockLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
                card.Add(lockLabel);
            }

            return card;
        }

        private Color GetRarityColor(int rarity)
        {
            return rarity switch
            {
                1 => new Color(0.5f, 0.5f, 0.5f), // Common - Gray
                2 => new Color(0.2f, 1f, 0.2f),   // Uncommon - Green
                3 => new Color(0.2f, 0.5f, 1f),   // Rare - Blue
                4 => new Color(0.8f, 0.2f, 1f),   // Epic - Purple
                5 => new Color(1f, 0.5f, 0f),     // Legendary - Orange
                _ => Color.white
            };
        }

        private string GetRarityStars(int rarity)
        {
            return rarity switch
            {
                1 => "⭐",
                2 => "⭐⭐",
                3 => "⭐⭐⭐",
                4 => "⭐⭐⭐⭐",
                5 => "⭐⭐⭐⭐⭐",
                _ => ""
            };
        }

        // Button callbacks
        private void OnSortByRarity()
        {
            inventorySystem?.SortByRarity();
            RefreshInventoryDisplay();
        }

        private void OnSortByPinkValue()
        {
            inventorySystem?.SortByPinkValue();
            RefreshInventoryDisplay();
        }

        private void OnToggleCowPowerFilter()
        {
            showOnlyCowPowerItems = !showOnlyCowPowerItems;

            if (showOnlyCowPowerItems)
            {
                cowPowerFilterButton.text = "🐄 Show All Items";
                headerLabel.text = "🐄 COW POWER ITEMS 🐄";
            }
            else
            {
                cowPowerFilterButton.text = "🐄 Cow Power Items";
                headerLabel.text = "🌸 Pink Frilly Inventory 🌸";
            }

            RefreshInventoryDisplay();
        }
    }
}
