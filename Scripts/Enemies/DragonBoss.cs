using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public class DragonBoss : Enemy
    {


        public DragonBoss(int id, int damage, int critChance, int maxHealth, int currentHealth) : base(id, damage, critChance, maxHealth, currentHealth) { }

        //load variables from the database
        void Awake()
        {
            this.Damage = EnemyClassDB.getEnemy(2).Damage;
            this.MaxHP = EnemyClassDB.getEnemy(2).MaxHP;
            this.CurrentHP = EnemyClassDB.getEnemy(2).CurrentHP;
            this.CritChance = EnemyClassDB.getEnemy(2).CritChance;
        }

    }
}