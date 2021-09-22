using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public static class WeaponDatabase
    {

        private static List<Weapon> weapons = new List<Weapon>();

        //get database
        public static List<Weapon> getWeaponList()
        {
            return weapons;
        }

        static WeaponDatabase()
        {
            weapons = new List<Weapon>() {
                //int ID, int damage, string itemName, double price, int hero
                new Weapon(1, 10, "Iron Sword", 10, 20.0, 0),
                new Weapon(2, 20, "Steel Sword", 10, 40.0, 0),
                new Weapon(3, 30, "Damascus Steel Sword", 10, 80.0, 0),
                new Weapon(4, 40, "Sky Steel Sword", 10, 160.0, 0),
                new Weapon(5, 80, "Ashen Remeberance", 25, 500.0, 0),

                new Weapon(6, 5, "Hunting Bow", 5, 20.0, 2),
                new Weapon(7, 10, "Longbow", 5, 40.0, 2),
                new Weapon(8, 15, "CrossBow", 5, 80.0, 2),
                new Weapon(9, 20, "Compound Bow", 5, 160.0, 2),
                new Weapon(10, 50, "Desert's Call", 10, 500.0, 2),

                new Weapon(11, 10, "Mage's Orb", 1, 10.0, 1),
                new Weapon(12, 15, "Elm Wand", 1, 20.0, 1),
                new Weapon(13, 20, "Elm Staff", 5, 40.0, 1),
                new Weapon(14, 25, "Oak Wand", 1, 80.0, 1),
                new Weapon(15, 50, "Obsidian Staff", 5, 160.0, 1),

                new Weapon(16, 10, "Mage's Orb", 1, 10.0, 1),
                new Weapon(17, 15, "Elm Wand", 1, 20.0, 1),
                new Weapon(18, 20, "Elm Staff", 5, 40.0, 1),
                new Weapon(19, 25, "Oak Wand", 1, 80.0, 1),
                new Weapon(20, 50, "Obsidian Staff", 5, 160.0, 1),

                new Weapon(21, 10, "Mage's Orb", 1, 10.0, 1),
                new Weapon(22, 15, "Elm Wand", 1, 20.0, 1),
                new Weapon(23, 20, "Elm Staff", 5, 40.0, 1),
                new Weapon(24, 25, "Oak Wand", 1, 80.0, 1),
                new Weapon(25, 50, "Obsidian Staff", 5, 160.0, 1),
            };
        }

        //get Hero at position
        public static Weapon getWeapon(int position)
        {
            return weapons[position];
        }
   
    }
}