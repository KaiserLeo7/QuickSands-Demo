using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class Equipable : Item
    {
        private bool equipped { get; set; }

        public Equipable(int ID, string itemName, int weight, double price, int hero) : base(ID, itemName, weight, price)
        {

        }
    }
}
