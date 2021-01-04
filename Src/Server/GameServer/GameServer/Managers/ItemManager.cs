using System;
using System.Collections.Generic;
using GameServer.Entities;
using GameServer.Models;
using Common;
using GameServer.Services;
namespace GameServer.Managers
{
    class ItemManager
    {
        Character Owner;

        Dictionary<int, Item> Items = new Dictionary<int, Item>();

        public ItemManager(Character owner)
        {
            this.Owner = owner;
        }

        public bool UseItem(int id,int count = 1)
        {
            Log.InfoFormat("[{0}#ID:{1}]UseItem:Id:{2} x Count:{3}", Owner.Data.Name, Owner.entityId, id, count);
            Item item = null;
            if(Items.TryGetValue(id,out item))
            {
                if (item.Count < count)
                {
                    return false;
                }
                //todo Use

                item.Remove(count);
                return true;
            }
            return false;
        }

        public bool HasItem(int itemId)
        {
            Item item = null;
            if(Items.TryGetValue(itemId,out item))
            {
                return item.Count > 0;
            }
            return false;
        }

        public Item GetItem(int itemId)
        {
            Item item = null;
            Items.TryGetValue(itemId, out item);
            Log.InfoFormat("[{0}#ID:{1}]GetItem:{2} {3}", Owner.Data.Name, Owner.entityId, itemId, item);
            return item;
        }

        public bool AddItem(int itemId,int count)
        {
            Item item = null;
            if(Items.TryGetValue(itemId,out item))
            {
                item.Add(count);
            }
            else
            {
                //Todo Db


            }
            Log.InfoFormat("[{0}#ID:{1}]AddItem:{2} {3}", Owner.Data.Name, Owner.entityId, itemId, count);
            DBService.Instance.Save();
            return true;
        }

        public bool RemoveItem(int itemId,int count)
        {
            Item item = null;
            if(!Items.TryGetValue(itemId,out item))
            {
                return false;
            }

            if (item.Count < count)
            {
                return false;
            }
            item.Remove(count);
            Log.InfoFormat("[{0}#ID:{1}]RemoveItem:{2} {3}", Owner.Data.Name, Owner.entityId, itemId, count);
            DBService.Instance.Save();
            return true;
        }

        //todo get List NItem
        public void GetNItemInfos()
        {

        }
    }
}
