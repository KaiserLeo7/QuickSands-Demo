using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands {
    public static class Player {

        private static Location currentLocation;
        private static Location locationToTravelTo;                     //the location player is going towards
        private static List<Quest> acceptedQuests = new List<Quest>();

        private static Vehicle currentVehicle;
        private static bool hasVehicle;
        private static ushort[] factionReputation = new ushort[3];
        private static bool isFirstBattle;

        //audio volumes
        private static float audioVolume;
        private static float sfxVolume;
        private static float musicVolume;

        static Player() {}

       
        public static Location CurrentLocation {
            get {
                return currentLocation;
            }
            set {
                currentLocation = value;
            }
        }

        public static Location LocationToTravelTo {
            get {
                return locationToTravelTo;
            }
            set {
                locationToTravelTo = value;
            }
        }

        public static Vehicle CurrentVehicle {
            get {
                return currentVehicle;
            }
            set {
                currentVehicle = value;
            }
        }

        public static bool HasVehicle {
            get {
                return hasVehicle;
            }
            set {
                hasVehicle = value;
            }
        }

        public static List<Quest> AcceptedQuests {
            get {
                return acceptedQuests;
            }
            set {
                acceptedQuests = value;
            }
        }

        public static ushort[] FactionReputation
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

        public static bool IsFirstBattle
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

        public static float AudioVolume
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

        public static float SfxVolume
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

        public static float MusicVolume
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

        //saves all the player data into the save file
        public static void SavePlayer() {
            SaveSystem.SavePlayer();
        }

        //loads all the player data from the save file
        public static void LoadPlayer() {
            PlayerData data = SaveSystem.LoadPlayer();

            CurrentLocation = new Location(data.CurrentLocation);

            try
            {
                LocationToTravelTo = new Location(data.LocationToTravelTo);
            }
            catch (System.Exception)
            {
                locationToTravelTo = null;
            }
                
            AcceptedQuests = data.AcceptedQuests;

            if(data.HasVehicle){
                if (data.CurrentVehicle.Name.Equals("Scout"))
                {
                    CurrentVehicle = new Scout(data.CurrentVehicle);
                }
                else if (data.CurrentVehicle.Name.Equals("Warthog"))
                {
                    CurrentVehicle = new Warthog(data.CurrentVehicle);
                }
                else if (data.CurrentVehicle.Name.Equals("Goliath"))
                {
                    CurrentVehicle = new Goliath(data.CurrentVehicle);
                }
                else if (data.CurrentVehicle.Name.Equals("Leviathan"))
                {
                    CurrentVehicle = new Leviathan(data.CurrentVehicle);
                }
            }

            HasVehicle = data.HasVehicle;

            FactionReputation = data.FactionReputation;

            IsFirstBattle = data.IsFirstBattle;

            AudioVolume = data.AudioVolume;

            sfxVolume = data.SfxVolume;

            musicVolume = data.MusicVolume;
        }
    }
}  
