using ShopGame.Interactables;
using ShopGame.ScriptableObjects.Inventory;
using UnityEngine;


namespace ShopGame.Pickables
{
    public interface IPickable
    {
        public bool TryPickUp(out (InventoryItemSO, uint) item);
    }

    public class Pickable : Interactable, IPickable
    {
        [SerializeField] protected InventoryItemSO inventoryItemSO;
        [SerializeField, Range(1,1000)] protected uint amount;

        public virtual bool TryPickUp(out (InventoryItemSO, uint) item)
        {
            item = default;
            if (inventoryItemSO == null) return false;
            item = new(inventoryItemSO, amount);
            EventBus<ReleaseRequest<Pickable>>.Raise(new ReleaseRequest<Pickable>()
            {
                PoolObject = this,
            });
            return true;
        }
    }
}
