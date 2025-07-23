using ShopGame.Presenters;
using ShopGame.Views.Inventory;
using UnityEngine;
using Zenject;

namespace ShopGame.Installers
{
    public class InventoryInstaller : MonoInstaller
    {
        [SerializeField] private InventoryView<PlayerInventoryPresenter> playerInventoryView;
        [SerializeField] private InventoryView<ShopInventoryPresenter> shopInventoryView;

        public override void InstallBindings()
        {
            Container.Bind<InventoryView<PlayerInventoryPresenter>>().FromInstance(playerInventoryView).AsSingle();
            Container.Bind<InventoryView<ShopInventoryPresenter>>().FromInstance(shopInventoryView).AsSingle();
        }
    }
}
