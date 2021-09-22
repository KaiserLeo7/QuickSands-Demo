using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class HeroMemento
    {
        private int damage;
        private int critChance;
        private int maxHP;
        private int currentHP;
        private int capacity;
        private int skinTire;

        //Copy comstructor
        public HeroMemento(Hero hero)
        {
            this.damage = hero.Damage;
            this.critChance = hero.CritChance;
            this.maxHP = hero.MaxHP;
            this.currentHP = hero.CurrentHP;
            this.capacity = hero.Capacity;
            this.skinTire = hero.SkinTire;
        }

        public int Damage {
            get {
                return damage;
            }
            set {
                damage = value;
            }
        }

        public int CritChance {
            get {
                return critChance;
            }
            set {
                critChance = value;
            }
        }

        public int MaxHP {
            get {
                return maxHP;
            }
            set {
                maxHP = value;
            }
        }

        public int CurrentHP {
            get {
                return currentHP;
            }
            set {
                currentHP = value;
            }
        }

        public int Capacity {
            get {
                return capacity;
            }
            set {
                capacity = value;
            }
        }

        public int SkinTire{
            get {
                return skinTire;
            }
            set {
                skinTire = value;
            }
        }
    }
}