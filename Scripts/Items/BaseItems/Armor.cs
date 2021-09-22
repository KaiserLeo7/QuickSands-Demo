using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class Armor : Equipable
    {
        private int health;

        public Armor(int ID, int health, string itemName, int weight, double price, int hero) : base(ID, itemName, weight, price, hero)
        {
            this.health = health;
        }

        public int Health{
            get{
                return health;
            }
            set{
                health = value;
            }
        }
    }
}
