using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    [System.Serializable]
    public class Nest : Location
    {
        private int multiplier;
        private bool activeStatus = false;
        public Nest(){}
        
        //constructor
        public Nest(int id, string name, double lattitude, double longtitude, int territory, int[] nearbyTowns, int multiplier, bool activeStatus) : base(id, name, lattitude, longtitude, territory, nearbyTowns) {
            this.multiplier = multiplier;
            this.activeStatus = activeStatus;
        }

        //memento copy constructor
        public Nest(NestMemento nest) : base(nest){
            this.multiplier = nest.Multiplier;
            this.activeStatus = nest.ActiveStatus;
        }

        public int Multiplier
        {
            get{ return multiplier; }
            set{ multiplier = value; }
        }
        public bool ActiveStatus
        {
            get{ return activeStatus; }
            set{ activeStatus = value; }
        }
    }
}


