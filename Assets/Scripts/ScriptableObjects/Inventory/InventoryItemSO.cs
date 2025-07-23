using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.ScriptableObjects.Inventory
{
    public enum Rarity
    {
        Common = 100,
        Uncommon = 70,
        Rare = 40,
        Epic = 20,
        Legendary = 10
    }

    [CreateAssetMenu(fileName = "InventoryItemSO", menuName = "ShopGame/Inventory/InventoryItemSO")]
    public class InventoryItemSO : ScriptableObject
    {
        [SerializeField] protected string itemName;
        public string Name
        {
            get => itemName;
        }
        [SerializeField] protected Sprite itemIcon;
        public Sprite Icon
        {
            get => itemIcon;
        }
        [SerializeField] protected string itemDescription;
        public string Description
        {
            get => itemDescription;
        }
        [SerializeField, Range(0, 10000)] protected uint itemPrice;
        public uint Price
        {
            get => itemPrice;
        }
        [SerializeField, Range(0, 10000)] protected uint hungerPoints;
        public uint HungerPoints
        {
            get => hungerPoints;
        }
        [SerializeField] protected Rarity rarity = Rarity.Common;
        public Rarity Rarity
        {
            get => rarity;
        }
    }
}
