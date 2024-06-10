using Glitch9.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Glitch9.Game
{
    [CreateAssetMenu(fileName = nameof(ItemSettings), menuName = UnityMenu.Game.CREATE_ITEM_SETTINGS, order = UnityMenu.Game.ORDER_CREATE_ITEM_SETTINGS)]
    public class ItemSettings : ScriptableResource<ItemSettings>
    {
        [SerializeField] private List<CurrencyEntry> currencyInfos = new();
        [SerializeField] private List<ItemType> itemTypes = new();
        [SerializeField] private List<Rarity> rarities = new();

        // Static References
        public static List<CurrencyEntry> CurrencyInfos => Instance.currencyInfos;
        public static List<Rarity> Rarities => Instance.rarities;
        public static List<ItemType> ItemTypes => Instance.itemTypes;

        public static int GetCurrencyIndex(string currencyID)
        {
            foreach (CurrencyEntry currencyInfo in CurrencyInfos)
            {
                if (currencyInfo.currencyId == currencyID) return currencyInfo.itemIndex;
            }
            return -1;
        }

        public static ItemType GetItemType(string name)
        {
            foreach (ItemType itemType in ItemTypes)
            {
                if (itemType.@class == name) return itemType;
            }
            return new ItemType() { @class = name };
        }

        public static Rarity GetRarity(int value)
        {
            foreach (Rarity rarity in Rarities)
            {
                if (rarity.value == value) return rarity;
            }
            return new Rarity() { value = value };
        }

        public static bool GetHideFromInventory(string @class)
        {
            foreach (ItemType itemType in ItemTypes)
            {
                if (itemType.@class == @class) return itemType.hideFromInventory;
            }
            return false;
        }

        public void RefreshItemTypes()
        {
#if UNITY_EDITOR
            itemTypes = new List<ItemType>();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in types)
            {
                if (type.IsInterface || type.IsAbstract) continue;
                if (type.IsSubclassOf(typeof(Item)))
                {
                    string typeName = type.Name;
                    if (!itemTypes.Contains(GetItemType(typeName)))
                    {
                        ItemType itemType = new()
                        {
                            @class = typeName
                        };
                        itemTypes.Add(itemType);
                    }
                }
            }

            // setdirty and save
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif
        }
    }
}
