using ShopGame.Interactables;
using ShopGame.Pickables;
using ShopGame.Player.Camera;
using ShopGame.Presenters;
using ShopGame.Presenters.Inventory;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ShopGame.Player.Interactions
{
    [System.Serializable]
    public abstract class InteractionOverrideStrategy
    {
        public abstract void InteractUpdate(Interactable interactable);
        public abstract void InteractStart(Interactable interactable);
        public abstract void InteractEnd(Interactable interactable);
    }

    [System.Serializable]
    public class PickUpInteractionOverrideStrategy : InteractionOverrideStrategy
    {
        [SerializeField] private PlayerInventoryPresenter playerInventory;

        private PlayerInputActions inputActions;
        private Interactable interactable;

        [Inject]
        private void Construct(PlayerInputActions inputActions)
        {
            this.inputActions = inputActions;
        }

        public override void InteractEnd(Interactable interactable)
        {
            inputActions.Player.Interaction.performed -= OnInteraction;
            this.interactable = null;
        }

        public override void InteractStart(Interactable interactable)
        {
            this.interactable = interactable;
            inputActions.Player.Interaction.performed += OnInteraction;
        }

        private void OnInteraction(InputAction.CallbackContext context)
        {
            if (interactable == null) return;
            if (!interactable.TryGetComponent(out IPickable pickable)) return;
            if (!pickable.TryPickUp(out var pickedUpItem)) return;
            playerInventory.Add(pickedUpItem.Item1, pickedUpItem.Item2);
        }

        public override void InteractUpdate(Interactable interactable)
        {
           
        }
    }

    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private PlayerCamera playerCamera;
        [SerializeField] private float interactRange = 3f;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField, Range(0f, 5f)] private float offset = 1f;

        [Header("Overrides")]
        [SerializeReference, SubclassSelector] private InteractionOverrideStrategy [] overrides;

        private Interactable currentInteractable;
        private bool isInteracting
        {
            get => currentInteractable != null;
        }

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            foreach (var strat in overrides)
            {
                diContainer.Inject(strat);
            }
        }

        private void Update()
        {
            if (isInteracting) currentInteractable.OnInteractUpdate();
            CheckForInteractable();
        }

        private void CheckForInteractable()
        {
            Vector3 origin = playerCamera.transform.position + playerCamera.transform.forward * offset;
            Ray ray = new Ray(origin, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
            {
                if (hit.collider.TryGetComponent(out Interactable interactable))
                {
                    if (interactable == currentInteractable) return;

                    StartNewtInteraction(interactable);
                }
                else
                {
                    EndCurrentInteraction();
                }
            }
            else
            {
                EndCurrentInteraction();
            }
        }

        private void UpdateCurrentInteraction()
        {
            if (!isInteracting) return;
            currentInteractable.OnInteractUpdate();
            foreach (var strat in overrides)
            {
                strat?.InteractUpdate(currentInteractable);
            }
        }

        private void EndCurrentInteraction()
        {
            if (currentInteractable == null) return;
            currentInteractable.OnInteractEnd();
            foreach (var strat in overrides)
            {
                strat?.InteractEnd(currentInteractable);
            }
            currentInteractable = null;
        }

        private void StartNewtInteraction(Interactable interactable)
        {
            EndCurrentInteraction();
            if (interactable == null) return;
            currentInteractable = interactable;
            currentInteractable.OnInteractStart();
            foreach(var strat in overrides)
            {
                strat?.InteractStart(currentInteractable);
            }
        }

        private void OnDestroy()
        {
            EndCurrentInteraction();
        }
    }
}