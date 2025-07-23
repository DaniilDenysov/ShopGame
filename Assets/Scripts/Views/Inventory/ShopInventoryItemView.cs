using ShopGame.ScriptableObjects.Inventory;
using TMPro;
using UnityEngine;

namespace ShopGame.Views.Inventory
{
    public class ShopInventoryItemView : InventoryItemView
    {
        [SerializeField] private TMP_Text quantityDisplay;
        [SerializeField] private TMP_Text priceDisplay;

        public override void Initialize(InventoryItemSO inventoryItemSO, uint amount = 1)
        {
            this.inventoryItemSO = inventoryItemSO;
            nameDisplay.text = $"{inventoryItemSO.Name}";
            if (quantityDisplay != null) quantityDisplay.text = $"x{amount}";
            else nameDisplay.text = $"{inventoryItemSO.Name} x{amount}";
            priceDisplay.text = $"{inventoryItemSO.Price}$";
            icon.sprite = inventoryItemSO.Icon;
        }
    }
}
