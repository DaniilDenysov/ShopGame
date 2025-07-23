using ShopGame.EventChannel.ModelPresenter;
using ShopGame.Presenters.Inventory;
using ShopGame.ScriptableObjects.Inventory;
using ShopGame.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Zenject.SpaceFighter;

namespace ShopGame.Presenters
{
    public struct OnSuccessfulPurchase : IEvent
    {
        public InventoryItemSO ItemSO;
        public uint Amount;
    }

    public struct  OnFoodConsumed : IEvent
    {
        public InventoryItemSO ItemSO;
        public uint Amount;
    }

    public class PlayerInventoryPresenter : InventoryPresenter<PlayerInventoryPresenter>
    {
        private EventBinding<OnSuccessfulPurchase> purchaseBinding;
        private PlayerInputActions inputActions;

        [Inject]
        private void Construct(PlayerInputActions inputActions)
        {
            this.inputActions = inputActions;
        }

        protected override void OnEnable()
        {
            purchaseBinding = new EventBinding<OnSuccessfulPurchase>(OnSuccesfullyPurchased);
            EventBus<OnSuccessfulPurchase>.Register(purchaseBinding);
            inputActions.Player.Invetory.performed += OnInventoryOpened;
            model.OnItemUpdated += OnItemAdded;
            model.OnItemRemoved += OnItemRemoved;
            view.OnItemPurchased += OnPurchase;
        }

        private void OnInventoryOpened(InputAction.CallbackContext context)
        {
            view?.Open();
        }

        private void OnPurchase(InventoryItemSO itemSO, uint amount)
        {
            if (amount == 0) return;
            EventBus<OnFoodConsumed>.Raise(new OnFoodConsumed()
            {
                ItemSO = itemSO,
                Amount = amount
            });
            Remove(itemSO, amount);
        }

        protected override void OnDisable()
        {
            EventBus<OnSuccessfulPurchase>.Deregister(purchaseBinding);
            inputActions.Player.Invetory.performed -= OnInventoryOpened;
            model.OnItemUpdated -= OnItemAdded;
            model.OnItemRemoved -= OnItemRemoved;
            view.OnItemPurchased -= OnPurchase;
        }

        private void OnSuccesfullyPurchased(OnSuccessfulPurchase @event)
        {
            if (@event.ItemSO == null)
            {
                DebugUtility.PrintError("Null ItemSO!");
                return;
            }
            Add(@event.ItemSO, @event.Amount);
        }
    }
}
