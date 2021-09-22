using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sands {
    public static class HeroClassDB {

        public static List<Hero> heroes = new List<Hero>();

        static HeroClassDB() 
        {
            heroes = new List<Hero>() {
                //adding all 3 types off heroes to sample class
                //  int damage, int critChance, int maxHP, int currentHP, int capacity, skinTier
                new Warrior(100, 10, 500, 500, 100, 1),
                new Mage(   250, 15, 250, 250, 60, 1),
                new Ranger( 100, 33, 350, 350, 75, 1),
                new Wizard( 150, 10, 300, 300, 70, 1),
                new Spearman(80, 20, 400, 400, 90, 1),
                
                new EnemyWarrior(40, 10, 250, 250, 100, 1),
                new EnemyMage(   70, 15, 100, 100, 60, 1),
                new EnemyRanger( 40, 33, 150, 150, 75, 1),
                new EnemyWizard( 40, 33, 150, 150, 75, 1),
                new EnemySpearman(40, 33, 150, 150, 75, 1)
            }; 
        }

        //get database
        public static List<Hero> getHeroList() {
            return heroes;
        }

        //get Hero at position
        public static Hero getHero(int position) {
          return heroes[position];
        }

        //clear
        public static void clearHeroList() {
            heroes.Clear();
        }
    }
}