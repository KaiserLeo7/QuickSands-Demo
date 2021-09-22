using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sands{
    public static class InnHeroes
    {
        private static List<Hero> innHeroesList;

        //constructor
        static InnHeroes(){
            innHeroesList = new List<Hero>();
            GenerateHeroes();
        }

        //generates heroes for the Inn every time the game starts
        public static void GenerateHeroes(){
            innHeroesList = new List<Hero>();
            int len = UnityEngine.Random.Range(1, 4);
            for (int i = 0; i < len; i++)
            {
                int randomHero = UnityEngine.Random.Range(0, 5);
                if(randomHero == 0)
                    InnHeroesList.Add(new Warrior((Warrior)HeroClassDB.getHero(randomHero)));
                else if(randomHero == 1)
                    InnHeroesList.Add(new Mage((Mage)HeroClassDB.getHero(randomHero)));
                else if(randomHero == 2)
                    InnHeroesList.Add(new Ranger((Ranger)HeroClassDB.getHero(randomHero)));
                else if (randomHero == 4)
                    InnHeroesList.Add(new Spearman((Spearman)HeroClassDB.getHero(randomHero)));
                else if (randomHero == 3)
                    InnHeroesList.Add(new Wizard((Wizard)HeroClassDB.getHero(randomHero)));

                InnHeroesList[i].SkinTire = UnityEngine.Random.Range(1, 4);

                if (len == 1)
                    InnHeroesList[i].SkinTire = 1;
            }
        }

        public static List<Hero> InnHeroesList{
            get{
                return innHeroesList;
            }
            set{
                innHeroesList = value;
            }
        }
    }
}