using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class NestMemento : LocationMemento
    {
        private int multiplier;
        private bool activeStatus = false;

        //memento copy constructor
        public NestMemento(Nest nest) : base(nest){
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