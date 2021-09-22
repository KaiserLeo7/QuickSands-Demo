using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public static class TradeableDatabase
    {
        private static List<Tradeable> tradeables = new List<Tradeable>();

        //get database
        public static List<Tradeable> getTradeableList()
        {
            return tradeables;
        }

        static TradeableDatabase()
        {
            tradeables = new List<Tradeable>() {
                //string itemName, int weight, double price

                //30-60
                new Tradeable(1, "Cloth", 2, 30.0),
                new Tradeable(2, "Rations", 3, 40.0),
                new Tradeable(3, "Leather", 1, 50.0),
                new Tradeable(4, "Spice", 2, 60.0),

                //100 - 250
                new Tradeable(5, "Coal", 5, 100.0),
                new Tradeable(6, "Steel", 7, 150.0),
                new Tradeable(7, "Tools", 5, 250.0),

                //400-1500
                new Tradeable(8, "Silver", 6, 400.0),
                new Tradeable(9, "Gold", 6, 700.0),
                new Tradeable(10, "Diamond", 5, 1500.0)
            };
        }

        //get Hero at position
        public static Tradeable getTradeable(int position)
        {
            return tradeables[position];
        }

        public static Tradeable getTradeable(string name)
        {
            Tradeable tradeable = null;
            foreach (var item in tradeables)
            {
                if(item.ItemName == name){
                    tradeable = item;
                }
            }

            return tradeable;
        }

        //clear
        public static void clearTradeableList()
        {
            tradeables.Clear();
        }
    }
}
