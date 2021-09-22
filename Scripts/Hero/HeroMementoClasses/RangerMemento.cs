using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class RangerMemento : HeroMemento
    {
        //Copy comstructor
        public RangerMemento(Ranger ranger) : base(ranger)
        {
            
        }
    }
}