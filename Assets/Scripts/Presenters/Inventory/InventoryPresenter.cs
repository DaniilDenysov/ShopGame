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
        private InventoryView<T> view;
        [SerializeField] private InventoryModel model = new InventoryModel();


        [Inject]
        private void Construct(InventoryView<T> view)
        {
            this.view = view;
            model.Initialize();
            view?.Initialize(model.InventoryItems);
        }

        private void OnEnable()
        {
            model.OnItemUpdated += OnModelUpdated;
        }

        private void OnDisable()
        {
            model.OnItemUpdated -= OnModelUpdated;
        }

        public void Open()
        {
            view?.Open();
        }

        public void Close()
        {
            view?.Close();
        }

        private void OnModelUpdated(InventoryItemSO itemSO, uint amount = 1)
        {
            view?.OnItemUpdated(itemSO, amount);
        }

        public virtual void OnItemPurchased(InventoryItemSO inventoryItemSO, uint amount = 1)
        {
            //TODO: add validation for currency
            Add(inventoryItemSO, amount);
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
