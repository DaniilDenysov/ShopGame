using ShopGame.ScriptableObjects.Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.Models.Inventory
{
    [System.Serializable]
    public struct InventoryItemDTO
    {
         public InventoryItemSO InventoryItemSO;
         [Range(0, 10000)] public uint Amount;
    }

    [System.Serializable]
    public class InventoryModel
    {
        public event Action<InventoryItemSO, uint> OnItemUpdated;

        [SerializeField] private InventoryItemDTO[] inventoryItemDTOs;
        
        private Dictionary<InventoryItemSO, uint> inventoryItems = new Dictionary<InventoryItemSO, uint>();

        public IReadOnlyDictionary<InventoryItemSO, uint> InventoryItems
        {
            get => inventoryItems;
        }

        public void Initialize()
        {
            inventoryItems.Clear();
            foreach(var item in inventoryItemDTOs)
            {
                if (!inventoryItems.TryAdd(item.InventoryItemSO, item.Amount))
                {
                    inventoryItems[item.InventoryItemSO] += item.Amount;
                }
            }
        }

        public void Add(InventoryItemSO inventoryItemSO, uint amount = 1)
        {
            if (amount == 0) return; 
            if (!inventoryItems.TryAdd(inventoryItemSO,amount))
            {
                inventoryItems[inventoryItemSO] += amount;
            }
            OnItemUpdated?.Invoke(inventoryItemSO, amount);
        }

        public void Remove(InventoryItemSO inventoryItemSO, uint amount = 1)
        {
            if (amount == 0) return;
            uint newAmount = (uint)Mathf.Max(0, inventoryItems[inventoryItemSO] - amount);
            if (newAmount > 0) inventoryItems[inventoryItemSO] = newAmount;
            else inventoryItems.Remove(inventoryItemSO);
            OnItemUpdated?.Invoke(inventoryItemSO,amount);
        }
    }
}
