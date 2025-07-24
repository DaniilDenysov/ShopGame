using ShopGame.ScriptableObjects.Inventory;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Views.Inventory
{
    public interface IInventoryItemView<T> where T : InventoryItemSO
    {
        public event Action<T, uint> OnValueChanged;
        public void Initialize(T inventoryItemSO, uint amount = 1);
    }

    public class InventoryItemView : MonoBehaviour, IInventoryItemView<InventoryItemSO>
    {
        public event Action<InventoryItemSO, uint> OnValueChanged;

        [SerializeField] protected TMP_Text quantityDisplay;
        [SerializeField] protected TMP_Text nameDisplay;
        [SerializeField] protected Image icon;
        protected InventoryItemSO inventoryItemSO;

        public virtual void Initialize(InventoryItemSO inventoryItemSO, uint amount = 1)
        {
            this.inventoryItemSO = inventoryItemSO;
            nameDisplay.text = $"{inventoryItemSO.Name}";
            quantityDisplay.text = $"{amount}";
            icon.sprite = inventoryItemSO.Icon;
        }

        public virtual void Purchase(int amount)
        {
            if (inventoryItemSO == null) return;
            OnValueChanged?.Invoke(inventoryItemSO, (uint)Mathf.Max(0, amount));
        }
    }
}
