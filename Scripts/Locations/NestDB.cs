using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands {
    public static class NestDB {

        private static List<Nest> nests;
        private static bool[] hasBeatenBoss;        //used for the buffs, saves whether the player has beaten each of the bosses

        static NestDB() 
        {
            //int id, string name, double lattitude, double longtitude, int territory, int[] nearbyTowns //multiplier
            nests = new List<Nest>() {
                //Demon
                new Nest(1, "Infernal Maw", 100, 100, 2, new int[]{4},1,false),
                //Dragon
                new Nest(2, "Wyrms Peak", 100,100, 3, new int[]{8},2,false),               
                //Golem
                new Nest(3, "Seared Rock", 100,100, 1, new int[]{2},3,false)
            };
        }
    

        //get database
        public static List<Nest> getNestList() {
            return nests;
        }

        public static Nest getNest(int position) {
            return nests[position];
        }

        //clear
        public static void clearNestList() {
            nests.Clear();
        }

        //save data to the save file
        public static void SaveNests() {

            SaveSystem.SaveNests();
        }

        //load data from the save file
        public static void LoadNests() {

            PlayerData data = SaveSystem.LoadNests();
            nests = new List<Nest>();
            foreach (NestMemento nest in data.Nests)
            {
                nests.Add(new Nest(nest));
            }

            hasBeatenBoss = data.HasBeatenBoss;
        }

        public static bool[] HasBeatenBoss
        {
            get { return hasBeatenBoss; }
            set { hasBeatenBoss = value; }
        }
    }
}