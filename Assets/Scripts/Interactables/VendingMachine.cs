using ShopGame.Managers;
using ShopGame.Presenters;
using ShopGame.Views.Inventory;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ShopGame.Interactables
{
    [RequireComponent(typeof(ShopInventoryPresenter))]
    public class VendingMachine : Interactable
    {
        private PlayerInputActions inputActions;
        [SerializeField] private ShopInventoryView inventoryView;

        [Inject]
        private void Construct(PlayerInputActions inputActions)
        {
            this.inputActions = inputActions;
        }

        public override void OnInteractStart()
        {
            base.OnInteractStart();
            inputActions.Player.Interaction.performed += OnPlayerPressedInteraction;
        }

        private void OnPlayerPressedInteraction(InputAction.CallbackContext context)
        {
            inventoryView.Open();
        }

        public override void OnInteractEnd()
        {
            base.OnInteractEnd();
            inputActions.Player.Interaction.performed -= OnPlayerPressedInteraction;
        }
    }
}
