using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class SpearmanMemento : HeroMemento
    {
        //Copy comstructor
        public SpearmanMemento(Spearman spearman) : base(spearman)
        {
            
        }
    }
}