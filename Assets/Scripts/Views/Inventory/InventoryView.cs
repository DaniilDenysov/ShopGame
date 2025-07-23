using ShopGame.Managers;
using ShopGame.Presenters.Inventory;
using ShopGame.ScriptableObjects.Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ShopGame.Views.Inventory
{
    public abstract class InventoryView<T> : MonoBehaviour, IUIState where T : IInventoryPresenter
    {
        [SerializeField] protected UITweener tweener;
        [SerializeField] protected RectTransform container;
        [SerializeField] protected InventoryItemView inventoryItemViewPrefab;
        public event Action<InventoryItemSO, uint> OnItemPurchased;

        //in this case I prefer dictionary and manual update over event as it will speed up look-up time and thus will be more efficient
        protected Dictionary<InventoryItemSO, InventoryItemView> itemViews = new Dictionary<InventoryItemSO, InventoryItemView>();

        protected InputStateManager inputStateManager;
        protected UIStateManager stateManager;

        [Inject]
        private void Construct(UIStateManager stateManager, InputStateManager inputStateManager)
        {
            this.stateManager = stateManager;
            this.inputStateManager = inputStateManager;
        }

        public void Initialize(IReadOnlyDictionary<InventoryItemSO, uint> items)
        {
            foreach (var item in items)
            {
                AddItem(item.Key, item.Value);
            }
        }

        public void AddItem(InventoryItemSO itemSO, uint amount = 1)
        {
            if (itemViews.TryGetValue(itemSO, out var itemView))
            {
                itemView.Initialize(itemSO, amount);
            }
            else
            {
                EventBus<PoolRequest<InventoryItemView>>.Raise(new PoolRequest<InventoryItemView>()
                {
                    Prefab = inventoryItemViewPrefab,
                    Parent = container,
                    Callback = (itm) =>
                    {
                        itm.Initialize(itemSO, amount);
                        itm.OnPurchased += PurchaseItem;
                        itemViews.Add(itemSO, itm);
                    }
                });
            }
        }

        private void PurchaseItem(InventoryItemSO itm, uint amount)
        {
            OnItemPurchased?.Invoke(itm, amount);
        }

        public void RemoveItem(InventoryItemSO itemSO)
        {
            if (itemViews.TryGetValue(itemSO, out var itemView))
            {
                EventBus<ReleaseRequest<InventoryItemView>>.Raise(new ReleaseRequest<InventoryItemView>()
                {
                    PoolObject = itemView,
                    Callback = (itm) =>
                    {
                        itm.OnPurchased -= PurchaseItem;
                        itemViews.Remove(itemSO);
                    }
                });
            }
        }

        public void Open()
        {
            stateManager.ChangeState(this);
        }

        public void Close()
        {
            stateManager.ExitState(this);
        }

        public virtual void OnEnter()
        {
            inputStateManager.ChangeState(new InventoryState());
            tweener.SetActive(true);
        }

        public virtual void OnExit()
        {
            inputStateManager.ChangeState(new DefaultState());
            tweener.SetActive(false);
        }
    }
}
