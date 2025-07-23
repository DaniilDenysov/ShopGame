using ShopGame.ScriptableObjects.Inventory;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Views.Inventory
{
    public class InventoryItemView : MonoBehaviour
    {
        public event Action<InventoryItemSO, uint> OnPurchased;

        [SerializeField] private TMP_Text quantityDisplay;
        [SerializeField] private TMP_Text priceDisplay;
        [SerializeField] private TMP_Text nameDisplay;
        [SerializeField] private Image icon;
        private InventoryItemSO inventoryItemSO;

        public void Initialize(InventoryItemSO inventoryItemSO, uint amount = 1)
        {
            this.inventoryItemSO = inventoryItemSO;
            if (quantityDisplay != null) quantityDisplay.text = $"x{amount}";
            priceDisplay.text = $"{inventoryItemSO.Price}$";
            nameDisplay.text = $"{inventoryItemSO.Name}";
            icon.sprite = inventoryItemSO.Icon;
        }

        private void OnDisable()
        {
            inventoryItemSO = null;
        }

        public void Purchase(uint amount)
        {
            OnPurchased?.Invoke(inventoryItemSO, amount);
        }
    }
}
