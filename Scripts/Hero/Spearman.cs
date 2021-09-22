using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace Sands
{
    public class Spearman : Hero
    {
        public Spearman(int damage, int critChance, int maxHP, int currentHP, int capacity, int skinTire) : base(damage, critChance, maxHP, currentHP, capacity, skinTire) { }
        void Awake()
        {

            this.Damage = HeroClassDB.heroes[4].Damage;
            this.CritChance = HeroClassDB.heroes[4].CritChance;
            this.MaxHP = HeroClassDB.heroes[4].MaxHP;
            this.CurrentHP = HeroClassDB.heroes[4].CurrentHP;
            this.Capacity = HeroClassDB.heroes[4].Capacity;
        }

        //memento copy constructor
        public Spearman(SpearmanMemento spearmanMemento) : base(spearmanMemento) { }

        //copy constructor
        public Spearman(Spearman spearman) : base(spearman) { }

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
                    skeletonMecanim.initialSkinName = "p2";
                    break;
                case 2:
                    skeletonMecanim.initialSkinName = "p3";
                    break;
                case 3:
                    skeletonMecanim.initialSkinName = "p4";
                    break;
                case 4:
                    skeletonMecanim.initialSkinName = "p5";
                    break;
                case 5:
                    skeletonMecanim.initialSkinName = "p6";
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
