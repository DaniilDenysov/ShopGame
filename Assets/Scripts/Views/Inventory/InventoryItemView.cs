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
        [SerializeField] protected TMP_Text nameDisplay;
        [SerializeField] protected Image icon;
        protected InventoryItemSO inventoryItemSO;

        public virtual void Initialize(InventoryItemSO inventoryItemSO, uint amount = 1)
        {
            this.inventoryItemSO = inventoryItemSO;
            nameDisplay.text = $"{inventoryItemSO.Name}";
            icon.sprite = inventoryItemSO.Icon;
        }

        public virtual void Purchase(int amount)
        {
            if (inventoryItemSO == null) return;
            OnPurchased?.Invoke(inventoryItemSO, (uint)Mathf.Max(0, amount));
        }
    }
}
