using ShopGame.EventChannel.ModelPresenter;
using ShopGame.ScriptableObjects.Inventory;
using TMPro;
using UnityEngine;

namespace ShopGame.Views.Inventory
{
    public class ShopInventoryItemView : InventoryItemView
    {
        [SerializeField] private TMP_Text priceDisplay;
        [SerializeField] private TMP_Text caloriesDisplay;
        [SerializeField] private EventChannel<int> wallet;
        [SerializeField] private UITweener available;
        [SerializeField] private UITweener notAvailable;

        public override void Initialize(InventoryItemSO inventoryItemSO, uint amount = 1)
        {
            this.inventoryItemSO = inventoryItemSO;
            UpdateStatus(wallet.Value);
            nameDisplay.text = $"{inventoryItemSO.Name}";
            quantityDisplay.text = $"{amount}";
            caloriesDisplay.text = $"{inventoryItemSO.HungerPoints}";
            priceDisplay.text = $"{inventoryItemSO.Price}$";
            icon.sprite = inventoryItemSO.Icon;
        }

        private void UpdateStatus(int currency)
        {
            if (inventoryItemSO == null) return;
            if (inventoryItemSO.Price < currency)
            {
                notAvailable.SetActive(false);
                available.SetActive(true);
            }
            else
            {
                notAvailable.SetActive(true);
                available.SetActive(false);
            }

        }

        private void OnEnable()
        {
            wallet.Register(UpdateStatus);
            UpdateStatus(wallet.Value);
        }

        private void OnDisable()
        {
            wallet.Deregister(UpdateStatus);
        }
    }
}
