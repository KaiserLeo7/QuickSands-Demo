using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands

{
    public class Leviathan : Vehicle    //3 seater trade vehicle
    {
        public Leviathan(string name, int id, int maxHP, int currentHP, int speed, int buttspace, int capacity, int price, int fuelAmount) : base(name, id, maxHP, currentHP, speed, buttspace, capacity, price, fuelAmount) { }

        //memento copy constructor
        public Leviathan(VehicleMemento vehicleMemento) : base(vehicleMemento){ }

        void Awake()
            {
                    
                this.Id = VehicleClassDB.getVehicle(3).Id;
                this.VehicleHP = VehicleClassDB.getVehicle(3).VehicleHP;
                this.CurrentHP = VehicleClassDB.getVehicle(3).CurrentHP;
                this.Speed = VehicleClassDB.getVehicle(3).Speed;
                this.PartySize = VehicleClassDB.getVehicle(3).PartySize;
                this.Capacity = VehicleClassDB.getVehicle(3).Capacity;
                this.Price = VehicleClassDB.getVehicle(3).Price;
                this.SumHP = VehicleClassDB.getVehicle(3).CurrentHP;
                this.Fuel = VehicleClassDB.getVehicle(3).Fuel;
        }

    }

}
