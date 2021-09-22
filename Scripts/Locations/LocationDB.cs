using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands {
    public static class LocationDB {

        private static List<Location> locations = new List<Location>();


        

        static LocationDB() 
        {
            locations = new List<Location>() {
                //string locationName, double latitude, double longitude, int territory, int[] nearbyTowns
                new Location(1, "Norwich", 100, 100, 1, new int[]{2}),
                new Location(2, "Westray", 100,100,1, new int[]{1, 3, 7}),
                new Location(3, "Veden", 100,100,1, new int[]{2, 4}),
                new Location(4, "Tunstead", 100,100,2, new int[]{3, 5}),
                new Location(5, "Gillamoor", 100,100,2, new int[]{4, 6, 10}),
                new Location(6, "Helm Crest", 100,100,2, new int[]{5, 7, 8 }),
                new Location(7, "Dalhurst", 100,100,3, new int[]{6, 8, 2}),
                new Location(8, "Kaiser", 100,100,3, new int[]{6, 7, 9}),
                new Location(9, "Braedon", 100,100,3, new int[]{8}),
                new Location(10, "Fara", 100,100,2, new int[]{5})
            };

            foreach (Location location in locations)
            {
                int amount = 21;
                for (int i = 0; i < TradeableDatabase.getTradeableList().Count; i++)
                {
                    if(UnityEngine.Random.Range(0, 101) > 40)
                        location.TradePrices.Add(TradeableDatabase.getTradeableList()[i].Price + UnityEngine.Random.Range(0, amount));
                    else
                        location.TradePrices.Add(TradeableDatabase.getTradeableList()[i].Price - UnityEngine.Random.Range(0, amount));
                    amount += 5;
                }
            }
        }
    

        //get database
        public static List<Location> getLocationList() {
            return locations;
        }

        //get Armor at position
        public static Location getLocation(int position) {
            return locations[position];
        }

        //clear
        public static void clearLocationList() {
            locations.Clear();
        }

      
    }
}
