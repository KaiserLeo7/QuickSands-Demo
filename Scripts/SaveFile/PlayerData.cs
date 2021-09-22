using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    [System.Serializable]
    public class PlayerData
    {
        
        private LocationMemento currentLocation;
        private LocationMemento locationToTravelTo;

        private List<Quest> acceptedQuests;

        private VehicleMemento currentVehicle;

        private bool hasVehicle;

        //reputation with each faction
        private ushort[] factionReputation = new ushort[3];

        private bool isFirstBattle;

        private float audioVolume;

        private float sfxVolume;

        private float musicVolume;

        //a list of nests that tell us if theyre active or not
        private List<NestMemento> nests = new List<NestMemento>();

        private bool[] hasBeatenBoss;

        //a list of your partys heroes
        private List<HeroMemento> heroParty = new List<HeroMemento>();

        //player inventory
        private List<InventoryTradeable> playerTradeableInventory;

        private double playerMoney;

        //battle saver
        private List<BattleCheckerMemento> heroBattleCheckers = new List<BattleCheckerMemento>();

        private List<int> heroHPs = new List<int>();

        private bool isInABattle;

        private bool isInTravel;

        private int[] encounters;

        private int currentEncounter;

        private int enemyVehicle;

        private int enemyFaction;
        
        private List<int> enemyHeroes;
        
        private List<int> enemySkins;

        //default constructor
        public PlayerData(){ }

        //Getters and setters
        public LocationMemento CurrentLocation {
            get {
                return currentLocation;
            }
            set {
                currentLocation = value;
            }
        }

        public LocationMemento LocationToTravelTo {
            get {
                return locationToTravelTo;
            }
            set {
                locationToTravelTo = value;
            }
        }

        public List<Quest> AcceptedQuests {
            get {
                return acceptedQuests;
            }
            set {
                acceptedQuests = value;
            }
        }
        
        public VehicleMemento CurrentVehicle {
            get {
                return currentVehicle;
            }
            set {
                currentVehicle = value;
            }
        }

        public bool HasVehicle {
            get {
                return hasVehicle;
            }
            set {
                hasVehicle = value;
            }
        }

        public List<NestMemento> Nests {
            get {
                return nests;
            }
            set {
                nests = value;
            }
        }

        public bool[] HasBeatenBoss
        {
            get { return hasBeatenBoss; }
            set { hasBeatenBoss = value; }
        }

        public List<HeroMemento> HeroParty {
            get {
                return heroParty;
            }
            set {
                heroParty = value;
            }
        }

        public List<InventoryTradeable> PlayerTradeableInventory{
            get{
                return playerTradeableInventory;
            }
            set{
                playerTradeableInventory = value;
            }
        }

        public double PlayerMoney {
            get {
                return playerMoney;
            }
            set {
                playerMoney = value;
            }
        }

        public ushort[] FactionReputation
        {
            get
            {
                return factionReputation;
            }
            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] > 400)
                        value[i] = 400;
                }

                factionReputation = value;
            }
        }

        public bool IsFirstBattle
        {
            get
            {
                return isFirstBattle;
            }
            set
            {
                isFirstBattle = value;
            }
        }

        public List<BattleCheckerMemento> HeroBattleCheckers
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

        public List<int> HeroHPs
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

        public bool IsInABattle
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

        public bool IsInTravel
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

        public int[] Encounters
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

        public int CurrentEncounter
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

        public int EnemyVehicle
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

        public int EnemyFaction
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

        public List<int> EnemyHeroes
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

        public List<int> EnemySkins
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

        public float AudioVolume
        {
            get
            {
                return audioVolume;
            }
            set
            {
                if (value > 1)
                    value = 1;
                if (value < 0.0001f)
                    value = 0.0001f;
                audioVolume = value;
            }
        }

        public float SfxVolume
        {
            get
            {
                return sfxVolume;
            }
            set
            {
                if (value > 1)
                    value = 1;
                if (value < 0.0001f)
                    value = 0.0001f;
                sfxVolume = value;
            }
        }

        public float MusicVolume
        {
            get
            {
                return musicVolume;
            }
            set
            {
                if (value > 1)
                    value = 1;
                if (value < 0.0001f)
                    value = 0.0001f;
                musicVolume = value;
            }
        }
    }
}
