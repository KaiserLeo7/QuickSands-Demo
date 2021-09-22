using System.Collections.Generic;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    //saves the state of encounterSystem preventing players from cheating the game by closing it
    public static class BattleSaver
    {
        private static bool isInTravel;
        private static bool isInABattle;
        private static int[] encounters;
        private static int currentEncounter;
        private static List<BattleCheckerMemento> heroBattleCheckers;   //to save the state of each hero
        private static List<int> heroHPs;
        private static int enemyVehicle;
        private static int enemyFaction;
        private static List<int> enemyHeroes = new List<int>();
        private static List<int> enemySkins = new List<int>();

        static BattleSaver() { }

        //saves all the data into the save file
        public static void SaveBattle()
        {
            SaveSystem.SaveBattle();
        }

        //loads all the data from the save file
        public static void LoadBattle()
        {
            PlayerData data = SaveSystem.LoadBattle();
            heroBattleCheckers = data.HeroBattleCheckers;
            heroHPs = data.HeroHPs;
            isInTravel = data.IsInTravel;
            isInABattle = data.IsInABattle;
            encounters = data.Encounters;
            currentEncounter = data.CurrentEncounter;
            enemyVehicle = data.EnemyVehicle;
            enemyHeroes = data.EnemyHeroes;
            enemyFaction = data.EnemyFaction;
            enemySkins = data.EnemySkins;
        }

        //getters and setters

        public static List<BattleCheckerMemento> HeroBattleCheckers
        {
            get
            {
                return heroBattleCheckers;
            }
            set
            {
                heroBattleCheckers = value;
            }
        }

        public static List<int> HeroHPs
        {
            get
            {
                return heroHPs;
            }
            set
            {
                heroHPs = value;
            }
        }

        public static bool IsInABattle
        {
            get
            {
                return isInABattle;
            }
            set
            {
                isInABattle = value;
            }
        }

        public static bool IsInTravel
        {
            get
            {
                return isInTravel;
            }
            set
            {
                isInTravel = value;
            }
        }

        public static int[] Encounters
        {
            get
            {
                return encounters;
            }
            set
            {
                encounters = value;
            }
        }

        public static int CurrentEncounter
        {
            get
            {
                return currentEncounter;
            }
            set
            {
                currentEncounter = value;
            }
        }

        public static int EnemyVehicle
        {
            get
            {
                return enemyVehicle;
            }
            set
            {
                enemyVehicle = value;
            }
        }

        public static int EnemyFaction
        {
            get
            {
                return enemyFaction;
            }
            set
            {
                enemyFaction = value;
            }
        }

        public static List<int> EnemyHeroes
        {
            get
            {
                return enemyHeroes;
            }
            set
            {
                enemyHeroes = value;
            }
        }

        public static List<int> EnemySkins
        {
            get
            {
                return enemySkins;
            }
            set
            {
                enemySkins = value;
            }
        }
    }
}

