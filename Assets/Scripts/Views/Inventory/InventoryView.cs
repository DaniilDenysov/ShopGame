using ShopGame.Managers;
using ShopGame.Presenters.Inventory;
using ShopGame.ScriptableObjects.Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ShopGame.Views.Inventory
{
    public interface IInventoryView<T> where T : InventoryItemSO
    {
        public event Action<T, uint> OnItemAmountChanged;
        public void Initialize(IReadOnlyDictionary<T, uint> items);
        public void UpdateItem(T item, uint  newAmount);
        public void Open();
        public void Close();
    }

    public abstract class InventoryView<T> : MonoBehaviour, IInventoryView<T>, IUIState where T : InventoryItemSO
    {
        [SerializeField] protected UITweener tweener;
        [SerializeField] protected RectTransform container;
        [SerializeField] protected InventoryItemView inventoryItemViewPrefab;
        public event Action<T, uint> OnItemAmountChanged;

        //in this case I prefer dictionary and manual update over event as it will speed up look-up time and thus will be more efficient
        protected Dictionary<T, InventoryItemView> itemViews = new Dictionary<T, InventoryItemView>();
        protected InputStateManager inputStateManager;
        protected UIStateManager stateManager;


        [Inject]
        private void Construct(UIStateManager stateManager, InputStateManager inputStateManager)
        {
            this.stateManager = stateManager;
            this.inputStateManager = inputStateManager;
        }

        public virtual void Initialize(IReadOnlyDictionary<T, uint> items)
        {
            foreach (var item in items)
            {
                UpdateItem(item.Key, item.Value);
            }
        }

        public virtual void UpdateItem(T itemSO, uint amount)
        {
            if (amount > 0)
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
                            if (itm is IInventoryItemView<T>) ((IInventoryItemView<T>)itm).OnValueChanged += PurchaseItem;
                            itemViews.Add(itemSO, itm);
                        }
                    });
                }
            }
            else
            {
                if (itemViews.TryGetValue(itemSO, out var itemView))
                {
                    EventBus<ReleaseRequest<InventoryItemView>>.Raise(new ReleaseRequest<InventoryItemView>()
                    {
                        PoolObject = itemView,
                        Callback = (itm) =>
                        {
                            if (itm is IInventoryItemView<T>) ((IInventoryItemView<T>)itm).OnValueChanged += PurchaseItem;
                            itemViews.Remove(itemSO);
                        }
                    });
                }
            }

        }

        private void PurchaseItem(T itm, uint amount)
        {
            OnItemAmountChanged?.Invoke(itm, amount);
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
