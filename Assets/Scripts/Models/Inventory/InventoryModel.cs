using ShopGame.ScriptableObjects.Inventory;
using ShopGame.Utilities;
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
    public class VendingMachineInventoryModel : InventoryModel
    {
        [SerializeField] protected bool randomizeContents;
        [SerializeField, Range(2, 1000)] protected int itemLimit = 8;

        public override void Initialize()
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
                else
                {
                    DebugUtility.PrintLine($"Added {item.InventoryItemSO}");
                }
            }
        }
    }

    [System.Serializable]
    public class PlayerInventoryModel : InventoryModel
    {
 
    }

    [System.Serializable]
    public abstract class InventoryModel
    {
        public event Action<InventoryItemSO, uint> OnItemUpdated;
        public event Action<InventoryItemSO> OnItemRemoved;

        [SerializeField] protected InventoryItemDTO[] inventoryItemDTOs;
        
        protected Dictionary<InventoryItemSO, uint> inventoryItems = new Dictionary<InventoryItemSO, uint>();

        public IReadOnlyDictionary<InventoryItemSO, uint> InventoryItems
        {
            get => inventoryItems;
        }

        public virtual void Initialize()
        {
            inventoryItems.Clear();
            foreach (var item in inventoryItemDTOs)
            {
                if (!inventoryItems.TryAdd(item.InventoryItemSO, item.Amount))
                {
                    inventoryItems[item.InventoryItemSO] += item.Amount;
                }
                else
                {
                    DebugUtility.PrintLine($"Added {item.InventoryItemSO}");
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
            OnItemUpdated?.Invoke(inventoryItemSO, inventoryItems[inventoryItemSO]);
        }

        public void Remove(InventoryItemSO inventoryItemSO, uint amount = 1)
        {
            if (amount == 0 || !inventoryItems.TryGetValue(inventoryItemSO, out uint currentAmount))
                return;

            uint newAmount = (uint)Mathf.Max(0, currentAmount - amount);
            DebugUtility.PrintLine($"{newAmount}");
            if (newAmount > 0)
            {
                inventoryItems[inventoryItemSO] = newAmount;
                OnItemUpdated?.Invoke(inventoryItemSO, newAmount);
            }
            else
            {
                inventoryItems.Remove(inventoryItemSO);
                OnItemRemoved?.Invoke(inventoryItemSO);
            }
        }
    }
}
