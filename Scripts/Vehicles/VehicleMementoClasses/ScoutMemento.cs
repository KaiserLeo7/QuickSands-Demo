using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class ScoutMemento : VehicleMemento
    {
        //copy constructor
        public ScoutMemento(Vehicle scout) : base(scout)
        {
            
        }
    }
}