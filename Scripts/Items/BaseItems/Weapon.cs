using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class Weapon : Equipable
    {
        private int damage { get; set; }

        public Weapon(int ID, int damage, string itemName, int weight, double price, int hero) : base(ID, itemName, weight, price, hero)
        {
            this.damage = damage;
        }

        public int Damage{
            get{
                return damage;
            }
            set{
                damage = value;
            }
        }
    }
}