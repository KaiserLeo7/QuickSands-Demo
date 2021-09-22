using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sands
{

    public static class SaveSystem
    {
        static string path = Application.persistentDataPath + "/player.savefile";
        private static PlayerData pdata;
        //The constructor calls both LoadAll and SaveAll - runs when the class is referenced
        static SaveSystem()
        {
            pdata = new PlayerData();
            LoadAll();
            SaveAll();
        }

        //Saves all the data
        public static void SaveAll()
        {
            SavePlayer();
            SaveNests();
            SaveParty();
            SavePlayerInventory();
            SaveBattle();
        }

        //load all the data
        public static void LoadAll()
        {
            Player.LoadPlayer();
            HeroPartyDB.LoadParty();
            NestDB.LoadNests();
            PlayerInventory.LoadPlayerInventory();
            BattleSaver.LoadBattle();
        }

        //Save and load functions
        public static void SavePlayer()
        {

            BinaryFormatter formatter = new BinaryFormatter();


            FileStream stream = new FileStream(path, FileMode.Create);


            pdata.AcceptedQuests = Player.AcceptedQuests;
            pdata.CurrentLocation = new LocationMemento(Player.CurrentLocation);
            try
            {
                pdata.LocationToTravelTo = new LocationMemento(Player.LocationToTravelTo);
            }
            catch (System.Exception)
            {
                pdata.LocationToTravelTo = null;
            }

            if(Player.HasVehicle){
                if (Player.CurrentVehicle.Name.Equals("Scout"))
                {
                    pdata.CurrentVehicle = new ScoutMemento(Player.CurrentVehicle);
                }
                else if (Player.CurrentVehicle.Name.Equals("Warthog"))
                {
                    pdata.CurrentVehicle = new WarthogMemento(Player.CurrentVehicle);
                }
                else if (Player.CurrentVehicle.Name.Equals("Goliath"))
                {
                    pdata.CurrentVehicle = new GoliathMemento(Player.CurrentVehicle);
                }
                else if (Player.CurrentVehicle.Name.Equals("Leviathan"))
                {
                    pdata.CurrentVehicle = new LeviathanMemento(Player.CurrentVehicle);
                }
            }
            else{
                pdata.CurrentVehicle = new VehicleMemento();
            }


            pdata.HasVehicle = Player.HasVehicle;
            pdata.FactionReputation = Player.FactionReputation;
            pdata.IsFirstBattle = Player.IsFirstBattle;
            pdata.AudioVolume = Player.AudioVolume;
            pdata.SfxVolume = Player.SfxVolume;
            pdata.MusicVolume = Player.MusicVolume;

            formatter.Serialize(stream, pdata);
            stream.Close();
        }

        public static PlayerData LoadPlayer()
        {

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                PlayerData data = formatter.Deserialize(stream) as PlayerData;

                pdata.CurrentLocation = data.CurrentLocation;
                pdata.LocationToTravelTo = data.LocationToTravelTo;
                pdata.HasVehicle = data.HasVehicle;
                pdata.FactionReputation = data.FactionReputation;
                pdata.IsFirstBattle = data.IsFirstBattle;
                pdata.AcceptedQuests = data.AcceptedQuests;
                pdata.AudioVolume = data.AudioVolume;

                stream.Close();
                return data;
            }
            else
            {
                Debug.Log("Save file not found in " + path);
                //Starting Location
                pdata.CurrentLocation = new LocationMemento(LocationDB.getLocation(0));
                pdata.HasVehicle = false;
                pdata.FactionReputation = new ushort[3] { 250, 150, 0 };
                pdata.IsFirstBattle = true;
                pdata.AcceptedQuests = new List<Quest>();
                pdata.AudioVolume = 1.0f;
                pdata.SfxVolume = 1.0f;
                pdata.MusicVolume = 1.0f;

                return pdata;
                
            }
            
        }

        public static void SaveNests()
        {
            BinaryFormatter formatter = new BinaryFormatter();


            FileStream stream = new FileStream(path, FileMode.Create);
            pdata.Nests = new List<NestMemento>();
            foreach (Nest nest in NestDB.getNestList())
            {
                pdata.Nests.Add(new NestMemento(nest));
            }

            pdata.HasBeatenBoss = NestDB.HasBeatenBoss;

            formatter.Serialize(stream, pdata);
            stream.Close();
        }

        public static PlayerData LoadNests()
        {

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                pdata.Nests = data.Nests;

                pdata.HasBeatenBoss = data.HasBeatenBoss;

                stream.Close();

                return pdata;

            }
            else
            {
                pdata.Nests = new List<NestMemento>()
                {
                    new NestMemento(NestDB.getNest(0)),
                    new NestMemento(NestDB.getNest(1)),
                    new NestMemento(NestDB.getNest(2))
                };

                pdata.HasBeatenBoss = new bool[3];

                return pdata;
            }
        }

        public static void SaveParty()
        {
            BinaryFormatter formatter = new BinaryFormatter();


            FileStream stream = new FileStream(path, FileMode.Create);
            
            pdata.HeroParty = new List<HeroMemento>();

            foreach (Hero hero in HeroPartyDB.getHeroList())
            {
                if (hero.GetType().Name.Equals("Warrior"))
                {
                    pdata.HeroParty.Add(new WarriorMemento((Warrior)hero));
                }
                else if (hero.GetType().Name.Equals("Mage"))
                {
                    pdata.HeroParty.Add(new MageMemento((Mage)hero));
                }
                else if (hero.GetType().Name.Equals("Ranger"))
                {
                    pdata.HeroParty.Add(new RangerMemento((Ranger)hero));
                }
                else if (hero.GetType().Name.Equals("Spearman"))
                {
                    pdata.HeroParty.Add(new SpearmanMemento((Spearman)hero));
                }
                else if (hero.GetType().Name.Equals("Wizard"))
                {
                    pdata.HeroParty.Add(new WizardMemento((Wizard)hero));
                }
            }

            formatter.Serialize(stream, pdata);
            stream.Close();


        }

        public static PlayerData LoadParty()
        {

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                pdata.HeroParty = data.HeroParty;
                stream.Close();

                return data;

            }
            else
            {
                
                pdata.HeroParty = new List<HeroMemento>();

                return pdata;
            }
        }

        public static void SavePlayerInventory()
        {

            BinaryFormatter formatter = new BinaryFormatter();


            FileStream stream = new FileStream(path, FileMode.Create);
            pdata.PlayerTradeableInventory = PlayerInventory.TradeableInventory;
            pdata.PlayerMoney = PlayerInventory.Money;
            
            formatter.Serialize(stream, pdata);
            stream.Close();


        }

        public static PlayerData LoadPlayerInventory()
        {

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                pdata.PlayerTradeableInventory = data.PlayerTradeableInventory;
                pdata.PlayerMoney = data.PlayerMoney;
                
                stream.Close();

                return pdata;
            }
            else
            {
                pdata.PlayerTradeableInventory = new List<InventoryTradeable>(){
                new InventoryTradeable(TradeableDatabase.getTradeable(0), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(1), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(2), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(3), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(4), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(5), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(6), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(7), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(8), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(9), 0)
            };
                pdata.PlayerMoney = 2000.0;
                return pdata;
            }
        }

        public static void SaveBattle()
        {

            BinaryFormatter formatter = new BinaryFormatter();


            FileStream stream = new FileStream(path, FileMode.Create);
            pdata.HeroBattleCheckers = BattleSaver.HeroBattleCheckers;
            pdata.HeroHPs = BattleSaver.HeroHPs;
            pdata.IsInABattle = BattleSaver.IsInABattle;
            pdata.IsInTravel = BattleSaver.IsInTravel;
            pdata.Encounters = BattleSaver.Encounters;
            pdata.CurrentEncounter = BattleSaver.CurrentEncounter;
            pdata.EnemyVehicle = BattleSaver.EnemyVehicle;
            pdata.EnemyFaction = BattleSaver.EnemyFaction;
            pdata.EnemyHeroes = BattleSaver.EnemyHeroes;
            pdata.EnemySkins = BattleSaver.EnemySkins;

            formatter.Serialize(stream, pdata);
            stream.Close();


        }

        public static PlayerData LoadBattle()
        {

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                pdata.HeroBattleCheckers = data.HeroBattleCheckers;
                pdata.HeroHPs = data.HeroHPs;
                pdata.IsInABattle = data.IsInABattle;
                pdata.IsInTravel = data.IsInTravel;
                pdata.Encounters = data.Encounters;
                pdata.CurrentEncounter = data.CurrentEncounter;
                pdata.EnemyVehicle = data.EnemyVehicle;
                pdata.EnemyFaction = data.EnemyFaction;
                pdata.EnemyHeroes = data.EnemyHeroes;
                pdata.EnemySkins = data.EnemySkins;

                stream.Close();
            }
            return pdata;
        }

        public static PlayerData Pdata{
            get{
                return pdata;
            }
            set{
                pdata = value;
            }
        }
    }
}