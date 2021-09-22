using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class WarthogMemento : VehicleMemento
    {
        //copy constructor
        public WarthogMemento(Vehicle warthog) : base(warthog)
        {
            
        }
    }
}