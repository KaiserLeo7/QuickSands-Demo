using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class Item
    {
        int id;
        private string itemName;
        private int weight;
        private double price;

        public Item(int id, string itemName, int weight, double price)
        {
            this.id = id;
            this.itemName = itemName;
            this.weight = weight;
            this.price = price;
        }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public string ItemName
        {
            get
            {
                return itemName;
            }
            set
            {
                itemName = value;
            }
        }

        public int Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }

        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
            }
        }

      
    }
}
