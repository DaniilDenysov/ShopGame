using ShopGame.Interactables;
using ShopGame.Presenters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ShopGame.Interactables
{
    public class Money : Interactable
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
    }
}
