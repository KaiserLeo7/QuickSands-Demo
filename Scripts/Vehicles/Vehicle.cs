using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands {
    public class Vehicle
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

  private List<Transform> passangers;

        public Vehicle(){}

        //full argument constructor
        public Vehicle(string name, int id, int vehicleHP, int currentHP, int speed, int partySize, int capacity, int price, int fuel)
        {
            this.name = name;
            this.id = id;
            this.vehicleHP = vehicleHP;
            this.currentHP = currentHP;
            this.speed = speed;
            this.partySize = partySize;
            this.capacity = capacity;
            this.price = price;
            this.sumHP = currentHP;
            this.fuel = fuel;
            this.passangers = new List<Transform>();
        }

        //copy constructor
        public Vehicle(Vehicle vehicle)
        {
            this.name = vehicle.Name;
            this.id = vehicle.Id;
            this.vehicleHP = vehicle.VehicleHP;
            this.currentHP = vehicle.CurrentHP;
            this.speed = vehicle.Speed;
            this.partySize = vehicle.PartySize;
            this.capacity = vehicle.Capacity;
            this.price = vehicle.Price;
            this.sumHP = vehicle.currentHP;
            this.fuel = vehicle.Fuel;
            this.passangers = new List<Transform>();
        }

        //memento copy constructor
        public Vehicle(VehicleMemento vehicleMemento)
        {
            this.name = vehicleMemento.Name;
            this.id = vehicleMemento.Id;
            this.vehicleHP = vehicleMemento.VehicleHP;
            this.currentHP = vehicleMemento.CurrentHP;
            this.speed = vehicleMemento.Speed;
            this.partySize = vehicleMemento.PartySize;
            this.capacity = vehicleMemento.Capacity;
            this.price = vehicleMemento.Price;
            this.sumHP = vehicleMemento.SumHP;
            this.fuel = vehicleMemento.Fuel;
            this.passangers = new List<Transform>();
        }


        //adds a hero into the vehicles to gain access to their stats
        //pools heros health then adds to vehicle
        public void addPassangers(Transform passanger){

            Passangers.Add(passanger);
            SumHP += passanger.GetComponent<Hero>().MaxHP;
            CurrentHP += passanger.GetComponent<Hero>().CurrentHP;
        }


        //use on hero or vehicle to test if still alive after attack
        //return a bool
        //also effects health
        public bool TakeDamage(int dmg)
        {
            CurrentHP -= dmg;

            if (CurrentHP <= 0)
            {
                CurrentHP = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Heal(int healAmount)
        {
            CurrentHP += healAmount;
            if (CurrentHP > SumHP)
                CurrentHP = SumHP;
        }

        public List<Transform> Passangers
        {
            get
            {
                return passangers;
            }
            set
            {
                passangers = value;
            }
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

        public int Fuel
        {
            
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