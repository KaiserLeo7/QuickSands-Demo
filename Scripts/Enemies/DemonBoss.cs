using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public class DemonBoss : Enemy
    {


        public DemonBoss(int id, int damage, int critChance, int maxHealth, int currentHealth) : base(id, damage, critChance, maxHealth, currentHealth) { }

        //load variables from the database
        void Awake()
        {
            this.Damage = EnemyClassDB.getEnemy(3).Damage;
            this.MaxHP = EnemyClassDB.getEnemy(3).MaxHP;
            this.CurrentHP = EnemyClassDB.getEnemy(3).CurrentHP;
            this.CritChance = EnemyClassDB.getEnemy(3).CritChance;
        }

    }
}