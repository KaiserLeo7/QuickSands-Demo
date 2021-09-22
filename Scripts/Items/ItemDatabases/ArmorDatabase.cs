using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sands
{
    public static class ArmorDatabase
    {

        private static List<Armor> armors = new List<Armor>();

        //get database
        public static List<Armor> getArmorList()
        {
            return armors;
        }

        static ArmorDatabase()
        {
            armors = new List<Armor>() {
                //int id, int health, double defence, string itemName, int weight, double price, int hero
                new Armor(1, 10, "Chainmail Vest", 15, 30.0, 0),
                new Armor(2, 20, "Iron Gamberson", 20, 50.0, 0),
                new Armor(3, 30, "Steel Gamberson", 25, 70.0, 0),
                new Armor(4, 40, "Full Plate Armour", 40, 200.0, 0),
                new Armor(5, 80, "Mobile Fortress", 50, 500.0, 0),

                new Armor(6, 5, "Leather Armour", 10, 20.0, 1),
                new Armor(7, 10, "Hardened Leather Armour", 10, 40.0, 1),
                new Armor(8, 20, "Leather Cuirass", 10, 50.0, 1),
                new Armor(9, 30, "Chitin Armour", 15, 80.0, 1),
                new Armor(10, 40, "Vestments of the Unseen", 20, 500.0, 1),

                new Armor(11, 1, "Torn Doublet", 1, 10.0, 2),
                new Armor(12, 5, "Hermit's Old Robes", 1, 20.0, 2),
                new Armor(13, 10, "Cloth Robes", 1, 50.0, 2),
                new Armor(14, 15, "Voluminous Robes", 5, 80.0, 2),
                new Armor(15, 20, "Drapes of the Profligate Seer", 10, 500.0, 2),

                new Armor(16, 1, "Torn Doublet", 1, 10.0, 2),
                new Armor(17, 5, "Hermit's Old Robes", 1, 20.0, 2),
                new Armor(18, 10, "Cloth Robes", 1, 50.0, 2),
                new Armor(19, 15, "Voluminous Robes", 5, 80.0, 2),
                new Armor(20, 20, "Drapes of the Profligate Seer", 10, 500.0, 2),

                new Armor(21, 1, "Torn Doublet", 1, 10.0, 2),
                new Armor(22, 5, "Hermit's Old Robes", 1, 20.0, 2),
                new Armor(23, 10, "Cloth Robes", 1, 50.0, 2),
                new Armor(24, 15, "Voluminous Robes", 5, 80.0, 2),
                new Armor(25, 20, "Drapes of the Profligate Seer", 10, 500.0, 2),
            };
        }
        //get Armor at position
        public static Armor getArmor(int position)
        {
            return armors[position];
        }

        public static Armor getArmor(string itemName)
        {
            return armors.Single(i => i.ItemName == itemName);
        }

        //clear
        public static void clearArmorList()
        {
            armors.Clear();
        }
    }
}
