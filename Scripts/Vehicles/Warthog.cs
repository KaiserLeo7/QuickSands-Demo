using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands

{
    public class Warthog : Vehicle    //4 seat vehicle
    {
        public Warthog(string name, int id, int maxHP, int currentHP, int speed, int buttspace, int capacity, int price, int fuelAmount) : base(name, id, maxHP, currentHP, speed, buttspace, capacity, price, fuelAmount) { }

        //copy constructor
        public Warthog(VehicleMemento vehicleMemento) : base(vehicleMemento){ }

        void Awake()
            {
                this.Name = VehicleClassDB.getVehicle(1).Name;    
                this.Id = VehicleClassDB.getVehicle(1).Id;
                this.VehicleHP = VehicleClassDB.getVehicle(1).VehicleHP;
                this.CurrentHP = VehicleClassDB.getVehicle(1).CurrentHP;
                this.Speed = VehicleClassDB.getVehicle(1).Speed;
                this.PartySize = VehicleClassDB.getVehicle(1).PartySize;
                this.Capacity = VehicleClassDB.getVehicle(1).Capacity;
                this.Price = VehicleClassDB.getVehicle(1).Price;
                this.SumHP = VehicleClassDB.getVehicle(1).CurrentHP;
                this.Fuel = VehicleClassDB.getVehicle(1).Fuel;
        }
    }

}




