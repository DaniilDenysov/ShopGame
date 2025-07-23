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

        [SerializeField] private bool randomizeContents;
        [SerializeField, Range(2, 1000)] private int itemLimit = 8;

        [SerializeField] private InventoryItemDTO[] inventoryItemDTOs;
        
        private Dictionary<InventoryItemSO, uint> inventoryItems = new Dictionary<InventoryItemSO, uint>();

        public IReadOnlyDictionary<InventoryItemSO, uint> InventoryItems
        {
            get => inventoryItems;
        }

        //TODO: encapsulate to separate models, add absruction
        public void Initialize()
        {
            inventoryItems.Clear();
            List<InventoryItemDTO> dtos = new List<InventoryItemDTO>();

            if (!randomizeContents)
            {
                dtos.AddRange(inventoryItemDTOs);
            }
            else
            {
                InventoryItemSO[] allItems = Resources.LoadAll<InventoryItemSO>("");

                foreach (var item in allItems)
                {
                    int rarityWeight = (int)item.Rarity;
                    float roll = UnityEngine.Random.Range(0, 101);

                    if (roll <= rarityWeight)
                    {
                        float amountScale = rarityWeight / 100f;
                        uint maxAmount = (uint)Mathf.Max(1, Mathf.RoundToInt(itemLimit * amountScale));
                        uint amount = (uint)UnityEngine.Random.Range(1, (int)maxAmount + 1);

                        dtos.Add(new InventoryItemDTO
                        {
                            InventoryItemSO = item,
                            Amount = amount
                        });
                    }
                }
            }

            foreach (var item in dtos)
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
