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
            nameDisplay.text = $"{inventoryItemSO.Name}";
            if (quantityDisplay != null) quantityDisplay.text = $"x{amount}";
            else nameDisplay.text = $"{inventoryItemSO.Name} x{amount}";
            priceDisplay.text = $"{inventoryItemSO.Price}$";
            icon.sprite = inventoryItemSO.Icon;
        }

        private void OnDisable()
        {
            inventoryItemSO = null;
        }

        public void Purchase(int amount)
        {
            OnPurchased?.Invoke(inventoryItemSO, (uint)Mathf.Max(0, amount));
        }
    }
}
