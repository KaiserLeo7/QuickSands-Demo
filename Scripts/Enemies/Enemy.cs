using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public class Enemy : MonoBehaviour
    {
        private int id;
        private int damage;
        private int critChance;
        private int maxHP;
        private int currentHP;

        //constructor
        public Enemy(int id, int damage, int critChance, int maxHP, int currentHP) {
            this.id = id;
            this.damage = damage;
            this.critChance = critChance;
            this.maxHP = maxHP;
            this.currentHP = currentHP;
        }

        /////////// GETTERS AND SETTERS //////////

        public bool TakeDamage(int dmg)
        {
            CurrentHP -= dmg;

            if (CurrentHP <= 0)
            {
                CurrentHP = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Damage
        {
            get
            {
                return damage;
            }
            set
            {
                damage = value;
            }
        }

        public int CritChance
            {
                get
                {
                    return critChance;
                }
                set
                {
                    critChance = value;
                }
        }


        public int MaxHP
        {
            get
            {
                return maxHP;
            }
            set
            {
                maxHP = value;
            }
        }

        public int CurrentHP
        {
            get
            {
                return  currentHP;
            }
            set
            {
                currentHP = value;
            }
        }

        //returns damage with a chance to crit
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

    }
}