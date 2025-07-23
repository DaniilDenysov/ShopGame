using ShopGame.Managers;
using ShopGame.Models.Inventory;
using ShopGame.ScriptableObjects.Inventory;
using ShopGame.Views.Inventory;
using UnityEngine;
using Zenject;

namespace ShopGame.Presenters.Inventory
{
    public interface IInventoryPresenter
    {
        public void Add(InventoryItemSO inventoryItemSO, uint amount = 1);
        public void Remove(InventoryItemSO inventoryItemSO, uint amount = 1);
    }

    public abstract class InventoryPresenter<T> : MonoBehaviour, IInventoryPresenter where T : IInventoryPresenter
    {
        protected InventoryView<T> view;
        [SerializeReference, SubclassSelector] protected InventoryModel model;


        [Inject]
        private void Construct(InventoryView<T> view)
        {
            this.view = view;
            model.Initialize();
        }

        protected virtual void Start()
        {
            view?.Initialize(model.InventoryItems);
        }

        protected virtual void OnEnable()
        {
            model.OnItemUpdated += OnItemAdded;
            model.OnItemRemoved += OnItemRemoved;
            view.OnItemPurchased += Remove;
        }

        protected virtual void OnDisable()
        {
            model.OnItemUpdated -= OnItemAdded;
            model.OnItemRemoved -= OnItemRemoved;
            view.OnItemPurchased -= Remove;
        }

        public void Open()
        {
            view?.Open();
        }

        public void Close()
        {
            view?.Close();
        }

        protected void OnItemRemoved(InventoryItemSO itemSO)
        {
            view?.RemoveItem(itemSO);
        }

        protected void OnItemAdded(InventoryItemSO itemSO, uint amount = 1)
        {
            view?.AddItem(itemSO, amount);
        }

        public void Add(InventoryItemSO inventoryItemSO, uint amount = 1)
        {
            model.Add(inventoryItemSO,amount);
        }

        public void Remove(InventoryItemSO inventoryItemSO, uint amount = 1)
        {
            model.Remove(inventoryItemSO, amount);
        }
    }
}
