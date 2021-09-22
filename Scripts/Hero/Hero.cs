using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script sets the values of the unit and its animation
namespace Sands {
    public abstract class Hero : MonoBehaviour {
        //unit values

        private string heroName;
        private int damage;
        private int critChance;
        private int maxHP;
        private int currentHP;
        private int capacity;
        private int skinTire;
        
        
        //constructor
        public Hero(int damage, int critChance, int maxHP, int currentHP, int capacity, int skinTire) {
            this.damage = damage;
            this.critChance = critChance;
            this.maxHP = maxHP;
            this.currentHP = currentHP;
            this.capacity = capacity;
            this.skinTire = skinTire;
        }

        //copy constructor
        public Hero(Hero hero) {
    
            this.damage = hero.Damage;
            this.critChance = hero.CritChance;
            this.maxHP = hero.MaxHP;
            this.currentHP = hero.CurrentHP;
            this.capacity = hero.Capacity;
            this.skinTire = hero.SkinTire;
        }

        //memento copy constructor
        public Hero(HeroMemento heroMemento)
        {
            this.damage = heroMemento.Damage;
            this.critChance = heroMemento.CritChance;
            this.maxHP = heroMemento.MaxHP;
            this.currentHP = heroMemento.CurrentHP;
            this.capacity = heroMemento.Capacity;
            this.skinTire = heroMemento.SkinTire;
        }

        //deducts from the HP of the hero
        public abstract bool TakeDamage(int dmg);
        
        //adds to the HP of the hero
        public abstract void Heal(int healAmount);

        //sets the skin of the recieved prefab of the hero
        public abstract void setSkin(GameObject prefab);
       
        /////////// GETTERS AND SETTERS //////////
        
        

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
                if (value > MaxHP)
                    value = MaxHP;
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
                if(value > 5)
                    skinTire = 5;
                else
                    skinTire = value;
            }
        }

        public int getDamageWithCrit(ref bool isCrit)
        {

            int random = UnityEngine.Random.Range(1, 100);
            if (random <= CritChance)
            {

                isCrit = true;
                return Damage * 2;
            }
            else
            {
                isCrit = false;
                return Damage;
            }

        }

        public int getDamag()
        {
                return Damage;
        }

    }
}