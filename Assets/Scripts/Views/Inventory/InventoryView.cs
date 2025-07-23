using ShopGame.Presenters.Inventory;
using ShopGame.ScriptableObjects.Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.Views.Inventory
{
    public abstract class InventoryView<T> : MonoBehaviour where T : IInventoryPresenter
    {
        [SerializeField] protected Transform container;
        [SerializeField] protected InventoryItemView inventoryItemViewPrefab;

        public event Action<InventoryItemSO, uint> OnItemPurchased;

        //in this case I prefer dictionary and manual update over event as it will speed up look-up time and thus will be more efficient
        protected Dictionary<InventoryItemSO, InventoryItemView> itemViews = new Dictionary<InventoryItemSO, InventoryItemView>();

        public void Initialize(IReadOnlyDictionary<InventoryItemSO, uint> items)
        {
            itemViews.Clear();
            foreach (var item in items)
            {
                AddItem(item.Key, item.Value);
            }
        }

        private void AddItem(InventoryItemSO itemSO, uint amount = 1)
        {
            if (itemViews.TryGetValue(itemSO, out var itemView))
            {
                itemView.Initialize(itemSO, amount);
            }
            else
            {
                //TODO: use pool instead
                itemView = Instantiate(inventoryItemViewPrefab, container);
                itemView.Initialize(itemSO, amount);
                itemView.OnPurchased += OnItemPurchased;
                itemViews.Add(itemSO, itemView);
            }
        }

        public void OnItemUpdated(InventoryItemSO itemSO, uint amount = 1)
        {
            AddItem(itemSO,amount);
        }
    }
}
