using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class LocationMemento
    {
        private int id;
        private string locationName;
        private double latitude;
        private double longitude;
        private int[] nearbyTowns;
        private int territory;

        //copy constructor
        public LocationMemento(Location location)
        {
            this.id = location.Id;
            this.locationName = location.LocationName;
            this.latitude = location.Latitude;
            this.longitude = location.Longitude;
            this.nearbyTowns = location.NearbyTowns;
            this.territory = location.Territory;
        }

        public int Id {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        
        public string LocationName {
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

        public double Longitude {
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
    }
}