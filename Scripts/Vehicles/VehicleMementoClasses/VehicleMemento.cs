using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class VehicleMemento
    {
        private string name;
        private int id;
        private int vehicleHP;
        private int currentHP;
        private int speed;
        private int partySize;
        private int capacity;
        private int price;
        private int sumHP;
        private int fuel;

        public VehicleMemento(){}

        //copy constructor
        public VehicleMemento(Vehicle vehicle)
        {
            this.name = vehicle.Name;
            this.id = vehicle.Id;
            this.vehicleHP = vehicle.VehicleHP;
            this.currentHP = vehicle.CurrentHP;
            this.speed = vehicle.Speed;
            this.partySize = vehicle.PartySize;
            this.capacity = vehicle.Capacity;
            this.price = vehicle.Price;
            this.sumHP = vehicle.CurrentHP;
            this.fuel = vehicle.Fuel;
        }
        
        public int SumHP
        {
            get
            {
                return sumHP;
            }
            set
            {
                sumHP = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
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

        public int VehicleHP
        {
            get
            {
                return vehicleHP;
            }
            set
            {
                vehicleHP = value;
            }
        }

        public int CurrentHP
        {
            get
            {
                return currentHP;
            }
            set
            {
                currentHP = value;
            }
        }
        public int Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }

        public int PartySize
        {
            get
            {
                return partySize;
            }

            set
            {
                partySize = value;
            }
        }

        public int Capacity
        {
            get
            {
                return capacity;
            }

            set
            {
                capacity = value;
            }
        }

        public int Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }
        }

        public int Fuel {

            get
            {
                return fuel;
            }
            set 
            {
                fuel = value;        
            } 
        }
    }
}