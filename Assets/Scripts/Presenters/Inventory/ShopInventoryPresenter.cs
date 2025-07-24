using ShopGame.EventChannel.ModelPresenter;
using ShopGame.Presenters.Inventory;
using ShopGame.ScriptableObjects.Inventory;
using UnityEngine;

namespace ShopGame.Presenters
{
    public class ShopInventoryPresenter : InventoryPresenter<InventoryItemSO>
    {
        [SerializeField] private EventChannel<int> wallet;
        [SerializeField] private UITweener errorWindowTweener;

        protected override void OnEnable()
        {
            model.OnItemUpdated += OnItemUpdated;
            view.OnItemAmountChanged += OnPurchase;
        }

        protected override void OnDisable()
        {
            model.OnItemUpdated -= OnItemUpdated;
            view.OnItemAmountChanged -= OnPurchase;
        }

        private void OnPurchase(InventoryItemSO itemSO, uint amount)
        {
            if (amount == 0) return;
            uint totalPrice = itemSO.Price * amount;
            if (wallet.Value - totalPrice < 0)
            {
                errorWindowTweener.SetActive(true);
                return;
            }
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
