using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

//Script sets the values of the unit and its animation
namespace Sands
{
    public class EnemyRanger : Hero
    {
        public EnemyRanger(int damage, int critChance, int maxHP, int currentHP, int capacity, int skinTire) : base(damage, critChance, maxHP, currentHP, capacity, skinTire) { }
        void Awake()
        {

            this.Damage = HeroClassDB.heroes[7].Damage;
            this.CritChance = HeroClassDB.heroes[7].CritChance;
            this.MaxHP = HeroClassDB.heroes[7].MaxHP + 200;
            this.CurrentHP = HeroClassDB.heroes[7].CurrentHP + 200;
            this.Capacity = HeroClassDB.heroes[7].Capacity;
        }


        public override bool TakeDamage(int dmg)
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

        public override void Heal(int healAmount)
        {
            CurrentHP += healAmount;
            if (CurrentHP > MaxHP)
                CurrentHP = MaxHP;
        }

        //sets the skin of the recieved prefab of the hero
        public override void setSkin(GameObject prefab)
        {
            var skeletonMecanim = prefab.GetComponent<SkeletonMecanim>();
            switch (SkinTire)
            {
                case 1:
                    skeletonMecanim.initialSkinName = "e2";
                    break;
                case 2:
                    skeletonMecanim.initialSkinName = "e3";
                    break;
                case 3:
                    skeletonMecanim.initialSkinName = "e4";
                    break;
                case 4:
                    skeletonMecanim.initialSkinName = "e5";
                    break;
                case 5:
                    skeletonMecanim.initialSkinName = "e6";
                    break;
                default:
                break;
            }
        }

        public int GetHealth()
        {
            return CurrentHP;
        }

    }
}