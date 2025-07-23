using ShopGame.Interactables;
using ShopGame.Player.Camera;
using UnityEngine;

namespace ShopGame.Player.Interactions
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private PlayerCamera playerCamera;
        [SerializeField] private float interactRange = 3f;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField, Range(0f, 5f)] private float offset = 1f;

        private Interactable currentInteractable;
        private bool isInteracting
        {
            get => currentInteractable != null;
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

                    if (currentInteractable != null)
                        currentInteractable.OnInteractEnd();

                    currentInteractable = interactable;
                    currentInteractable.OnInteractStart();
                }
                else if (currentInteractable != null)
                {
                    currentInteractable.OnInteractEnd();
                    currentInteractable = null;
                }
            }
            else if (currentInteractable != null)
            {
                currentInteractable.OnInteractEnd();
                currentInteractable = null;
            }
        }

        private void OnDestroy()
        {
            if (currentInteractable != null)
                currentInteractable.OnInteractEnd();
        }
    }
}