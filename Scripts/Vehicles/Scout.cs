using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands

{
    public class Scout : Vehicle   //2 seater vehicle
    {
        public Scout(string name, int id, int maxHP, int currentHP, int speed, int buttspace, int capacity, int price, int fuelAmount) : base(name, id, maxHP, currentHP, speed, buttspace, capacity, price, fuelAmount) { }

        //memento copy constructor
        public Scout(VehicleMemento vehicleMemento) : base(vehicleMemento){ }

        void Awake()
            {
                this.Name = VehicleClassDB.getVehicle(0).Name;
                this.Id = VehicleClassDB.getVehicle(0).Id;
                this.VehicleHP = VehicleClassDB.getVehicle(0).VehicleHP;
                this.CurrentHP = VehicleClassDB.getVehicle(0).CurrentHP;
                this.Speed = VehicleClassDB.getVehicle(0).Speed;
                this.PartySize = VehicleClassDB.getVehicle(0).PartySize;
                this.Capacity = VehicleClassDB.getVehicle(0).Capacity;
                this.Price = VehicleClassDB.getVehicle(0).Price;
                this.SumHP = VehicleClassDB.getVehicle(0).CurrentHP;
                this.Fuel = VehicleClassDB.getVehicle(0).Fuel;
        }

    }

}
