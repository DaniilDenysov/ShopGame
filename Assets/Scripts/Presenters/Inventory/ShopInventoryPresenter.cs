using ShopGame.EventChannel.ModelPresenter;
using ShopGame.Presenters.Inventory;
using ShopGame.ScriptableObjects.Inventory;
using UnityEngine;

namespace ShopGame.Presenters
{
    public class ShopInventoryPresenter : InventoryPresenter<ShopInventoryPresenter>
    {
        [SerializeField] private EventChannel<int> wallet;

        protected override void OnEnable()
        {
            model.OnItemUpdated += OnItemAdded;
            model.OnItemRemoved += OnItemRemoved;
            view.OnItemPurchased += OnPurchase;
        }

        protected override void OnDisable()
        {
            model.OnItemUpdated -= OnItemAdded;
            model.OnItemRemoved -= OnItemRemoved;
            view.OnItemPurchased -= OnPurchase;
        }

        private void OnPurchase(InventoryItemSO itemSO, uint amount)
        {
            if (amount == 0) return;
            uint totalPrice = itemSO.Price * amount;
            if (wallet.Value - totalPrice < 0) return;
            wallet.Value = (int)(wallet.Value - totalPrice);
            EventBus<OnSuccessfulPurchase>.Raise(new OnSuccessfulPurchase()
            {
                ItemSO = itemSO,
                Amount = amount
            });
            Remove(itemSO, amount);
        }
    }
}
