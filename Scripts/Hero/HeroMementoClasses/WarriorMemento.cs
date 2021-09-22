using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class WarriorMemento : HeroMemento
    {
        //Copy comstructor
        public WarriorMemento(Warrior warrior) : base(warrior)
        {
            
        }
    }
}