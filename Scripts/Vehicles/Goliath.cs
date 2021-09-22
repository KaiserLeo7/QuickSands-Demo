using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands

{
    public class Goliath : Vehicle    //3 seater trade vehicle
    {
        public Goliath(string name, int id, int maxHP, int currentHP, int speed, int buttspace, int capacity, int price, int fuelAmount) : base(name, id, maxHP, currentHP, speed, buttspace, capacity, price, fuelAmount) { }

        //memento copy constructor
        public Goliath(VehicleMemento vehicleMemento) : base(vehicleMemento) { }

        void Awake()
        {
            this.Id = VehicleClassDB.getVehicle(2).Id;
            this.VehicleHP = VehicleClassDB.getVehicle(2).VehicleHP;
            this.CurrentHP = VehicleClassDB.getVehicle(2).CurrentHP;
            this.Speed = VehicleClassDB.getVehicle(2).Speed;
            this.PartySize = VehicleClassDB.getVehicle(2).PartySize;
            this.Capacity = VehicleClassDB.getVehicle(2).Capacity;
            this.Price = VehicleClassDB.getVehicle(2).Price;
            this.SumHP = VehicleClassDB.getVehicle(2).CurrentHP;
            this.Fuel = VehicleClassDB.getVehicle(2).Fuel;
        }

    }

}
