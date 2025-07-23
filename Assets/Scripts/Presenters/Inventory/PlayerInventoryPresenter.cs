using ShopGame.Presenters.Inventory;
using ShopGame.ScriptableObjects.Inventory;
using ShopGame.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ShopGame.Presenters
{
    public struct OnSuccessfulPurchase : IEvent
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
            base.OnEnable();
        }

        private void OnInventoryOpened(InputAction.CallbackContext context)
        {
            view?.Open();
        }

        protected override void OnDisable()
        {
            EventBus<OnSuccessfulPurchase>.Deregister(purchaseBinding);
            inputActions.Player.Invetory.performed -= OnInventoryOpened;
            base.OnDisable();
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
