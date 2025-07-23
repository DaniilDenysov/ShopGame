using ShopGame.Interactables;
using ShopGame.Pickables;
using ShopGame.Presenters;
using ShopGame.ScriptableObjects.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ShopGame.Interactables
{
    public class Money : Interactable, IPickable
    {
        private PlayerInputActions inputActions;

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
            
        }

        public override void OnInteractEnd()
        {
            base.OnInteractEnd();
            inputActions.Player.Interaction.performed -= OnPlayerPressedInteraction;
        }

        public bool TryPickUp(out (InventoryItemSO, uint) item)
        {
            item = default;

            return true;
        }
    }
}
