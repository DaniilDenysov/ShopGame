using UnityEngine;

namespace ShopGame.Interactables
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] protected Outline outline;

        public virtual void OnInteractStart()
        {
            outline.enabled = true;
        }

        public virtual void OnInteractUpdate()
        {

        }
        public virtual void OnInteractEnd()
        {
            outline.enabled = false;
        }
    }
}
