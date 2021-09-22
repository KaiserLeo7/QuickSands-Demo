using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sands {
    public static class EnemyClassDB {

        private static List<Enemy> enemies;


    static EnemyClassDB(){
        enemies = new List<Enemy>() {
           //int id, int damage, int critChance, int maxHealth, int currentHealth
        new EnemyGround(1,100, 10, 200, 200),
        new EnemyFlying(2,150, 20, 100, 100),
        new DragonBoss(3,400, 20, 1500, 1500),
        new DemonBoss(4,300, 30, 1000, 1000),
        new FireGolemBoss(5,200, 10, 2000, 2000),
        new EnemyMid(6,200, 15, 400, 400)

        };
    }
    
        //get database
        public static List<Enemy> getEnemyList() {
            return enemies;
        }

        //get Hero at position
        public static Enemy getEnemy(int position) {
            return enemies[position];
        }        

        //clear
        public static void clearEnemyList() {
            enemies.Clear();
        }

    }
}

