using ShopGame.Models.Inventory;
using ShopGame.ScriptableObjects.Inventory;
using ShopGame.Views.Inventory;
using UnityEngine;
using Zenject;

namespace ShopGame.Installers
{
    public class VendingMachineInventoryInstaller : MonoInstaller
    {
        [SerializeField] private ShopInventoryView shopInventoryView;
        [SerializeField] private VendingMachineInventoryModel shopInventoryModel = new();

        public override void InstallBindings()
        {
            Container.Bind<IInventoryView<InventoryItemSO>>().FromInstance(shopInventoryView).AsSingle();
            Container.Bind<IInventoryModel<InventoryItemSO>>().FromInstance(shopInventoryModel).AsSingle();
        }
    }
}
