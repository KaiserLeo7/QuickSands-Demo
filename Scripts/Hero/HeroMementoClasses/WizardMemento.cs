using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class WizardMemento : HeroMemento
    {
        //Copy comstructor
        public WizardMemento(Wizard wizard) : base(wizard)
        {
            
        }
    }
}