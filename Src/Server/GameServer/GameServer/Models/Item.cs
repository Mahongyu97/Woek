using System;
namespace GameServer.Models
{
    public class Item
    {
        public int ItemId;
        public int Count;

        public Item()
        {
        }

        public void Add(int count)
        {
            this.Count += count;
        }

        public void Remove(int count)
        {
            this.Count -= count;
        }

        public bool Use(int count = 1)
        {
            return false;
        }

        public override string ToString()
        {
            return string.Format("ItemID:{0} Count:{1}", this.ItemId, this.Count);
        }
    }
}
