using ShopGame.Models.Inventory;
using ShopGame.ScriptableObjects.Inventory;
using ShopGame.Views.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ShopGame.Installers
{
    public class PlayerInventoryInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInventoryView playerInventoryView;
        [SerializeField] private PlayerInventoryModel playerInventoryModel = new();

        public override void InstallBindings()
        {
            Container.Bind<IInventoryView<InventoryItemSO>>().FromInstance(playerInventoryView).AsSingle();
            Container.Bind<IInventoryModel<InventoryItemSO>>().FromInstance(playerInventoryModel).AsSingle();
        }
    }
}
