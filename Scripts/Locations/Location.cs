using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    [System.Serializable]
    public class Location
    {

        private int id;
        private string locationName;
        private double latitude;
        private double longitude;
        private int[] nearbyTowns;
        private int territory;
        private List<double> tradePrices;

        //default constructor
        public Location() { }

        //6 argument constructor
        public Location(int id, string locationName, double latitude, double longitude, int territory, int[] nearbyTowns)
        {
            this.id = id;
            this.locationName = locationName;
            this.latitude = latitude;
            this.longitude = longitude;
            this.territory = territory;
            this.nearbyTowns = nearbyTowns;
            this.tradePrices = new List<double>();
        }

        //memento copy constructor
        public Location(LocationMemento locationMemento)
        {
            this.id = locationMemento.Id;
            this.locationName = locationMemento.LocationName;
            this.latitude = locationMemento.Latitude;
            this.longitude = locationMemento.Longitude;
            this.nearbyTowns = locationMemento.NearbyTowns;
            this.territory = locationMemento.Territory;
        }
        
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public string LocationName
        {
            get
            {
                return locationName;
            }
            set
            {
                locationName = value;
            }
        }

        public double Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
            }
        }

        public double Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
            }
        }

        public int Territory
        {
            get
            {
                return territory;
            }
            set
            {
                territory = value;
            }
        }

        public int[] NearbyTowns
        {
            get
            {
                return nearbyTowns;
            }
            set
            {
                nearbyTowns = value;
            }
        }

        public List<double> TradePrices{
            get
            {
                return tradePrices;
            }
            set
            {
                tradePrices = value;
            }
        }
    }
}