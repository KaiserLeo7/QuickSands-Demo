using UnityEngine;

namespace Sands
{
    public class HeroBuff : MonoBehaviour
    {
        Hero hero;
        BattleSystem2 battleSystem;

        //adds a buff to the hero which could be an enemy too
        public void AddBuffs(bool isHero)
        {
            hero = GetComponent<Hero>();

            if (isHero)
            {
                AddVehicleBuffToHero();
                AddBossBuff();
            }

            else
            {
                battleSystem = FindObjectOfType<BattleSystem2>();
                AddVehicleBuffToEnemy();
            }
        }

        //based on the type of the vehicle grants a buff to the hero
        void AddVehicleBuffToHero()
        {
            if (Player.HasVehicle)
            {
                switch (Player.CurrentVehicle.Id)
                {
                    case 1:
                        hero.MaxHP += 25;
                        hero.CurrentHP += 25;
                        break;
                    case 2:
                        hero.MaxHP += 50;
                        hero.CurrentHP += 50;
                        break;
                    case 3:
                        hero.MaxHP += 75;
                        hero.CurrentHP += 75;
                        break;
                    case 4:
                        hero.MaxHP += 100;
                        hero.CurrentHP += 100;
                        break;
                    default:
                        break;
                }
            }
        }

        //based on the type of the enemy's vehicle in battleSystem grants a buff to the enemy
        void AddVehicleBuffToEnemy()
        {
            switch(battleSystem.EnemyVehicle.Id)
            {
                case 1:
                    hero.MaxHP += 25;
                    hero.CurrentHP += 25;
                    break;
                case 2:
                    hero.MaxHP += 50;
                    hero.CurrentHP += 50;
                    break;
                case 3:
                    hero.MaxHP += 75;
                    hero.CurrentHP += 75;
                    break;
                case 4:
                    hero.MaxHP += 100;
                    hero.CurrentHP += 100;
                    break;
                default:
                    break;
            }
        }

        //based on the bosses that the player have defeated grants buffs to the hero 
        void AddBossBuff()
        {
            if (NestDB.HasBeatenBoss[0])
            {
                hero.CritChance += 5;
            }

            if (NestDB.HasBeatenBoss[1])
            {
                hero.Damage += 50;
            }

            if (NestDB.HasBeatenBoss[2])
            {
                hero.MaxHP += 50;
                hero.CurrentHP += 50;
            }
        }
    }
}
