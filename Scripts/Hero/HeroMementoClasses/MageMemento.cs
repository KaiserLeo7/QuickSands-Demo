using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class MageMemento : HeroMemento
    {
        //Copy comstructor
        public MageMemento(Mage mage) : base(mage)
        {
            
        }
    }
}