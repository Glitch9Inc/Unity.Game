using Glitch9.Database;
using Glitch9.Game;
using System.Collections.Generic;

namespace Glitch9.DB
{
    public class ItemDatabase : DatabaseBase<ItemDatabase, int, Item>
    {
        public static IEnumerable<T> GetList<T>() where T : Item
        {
            foreach (Item item in DB.Values)
            {
                //??if (item.Icon)
                if (item is T rightItem) yield return rightItem;
            }
        }

        public static Dictionary<int, T> GetDict<T>() where T : Item
        {
            Dictionary<int, T> dict = new();

            foreach (KeyValuePair<int, Item> item in DB)
            {
                if (item.Value is T rightItem) dict.Add(item.Key, rightItem);
            }

            return dict;
        }

        public static List<Item> GetPoolItems(IItemPool itemPool)
        {
            if (itemPool == null) throw new System.ArgumentNullException(nameof(itemPool));

            string[] poolIds = itemPool.GetPoolIds();
            List<Item> items = new();

            foreach (string poolId in poolIds)
            {
                foreach (Item item in DB.Values)
                {
                    if (item.PoolId == poolId) items.Add(item);
                }
            }

            if (items.IsNullOrEmpty()) GNLog.Error($"{itemPool.GetType().Name}의 아이템이 없습니다.");
            return items;
        }


        public static int GetIndex(string itemId)
        {
            foreach (Item pair in DB.Values)
            {
                if (pair.Id == itemId) return pair.Index;
            }
            GNLog.Error("Index not found with value: " + itemId);
            return -1;
        }

        public static string GetId(int index)
        {
            if (DB == null)
            {
                GNLog.Error("DB is null");
                return "Unknown";
            }
            if (DB.ContainsKey(index) == false)
            {
                GNLog.Error("Index not found: " + index);
                return "Unknown";
            }
            if (DB != null && DB.ContainsKey(index)) return DB[index].Id;
            return "Unknown";
        }

        public static T Get<T>(int index) where T : Item
        {
            if (DB != null && DB.ContainsKey(index))
            {
                if (DB[index] is T rightItem) return rightItem;
            }
            return default;
        }

        public static T Get<T>(string id) where T : Item
        {
            if (string.IsNullOrEmpty(id))
            {
                GNLog.Warning("Empty item key");
                return default;
            }

            id = id.ToLower();

            foreach (KeyValuePair<int, Item> pair in DB)
            {
                if (pair.Value.Id.ToLower() == id)
                {
                    if (pair.Value is T rightItem) return rightItem;
                }
            }

            GNLog.Error("Key not found with value: " + id);
            return default;
        }

        public static Item Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                GNLog.Warning("Empty item key");
                return null;
            }

            id = id.ToLower();

            foreach (KeyValuePair<int, Item> pair in DB)
            {
                if (pair.Value.Id.ToLower() == id) return pair.Value;
            }

            GNLog.Error("Key not found with value: " + id);
            return null;
        }

        public static bool ContainsId(string key)
        {
            key = key.Trim();
            foreach (KeyValuePair<int, Item> pair in DB)
            {
                if (pair.Value.Id.ToLower() == key.ToLower()) return true;
            }
            return false;
        }
    }
}