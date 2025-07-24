using ShopGame.EventChannel.ModelPresenter;
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
    public class Money : Interactable
    {
        [SerializeField] private int amount;
        private PlayerInputActions inputActions;
        [SerializeField] private EventChannel<int> wallet; 

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
            wallet.Value += amount;
            gameObject.SetActive(false);
            OnInteractEnd();
        }

        public override void OnInteractEnd()
        {
            base.OnInteractEnd();
            inputActions.Player.Interaction.performed -= OnPlayerPressedInteraction;
        }
    }
}
