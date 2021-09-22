using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public class FireGolemBoss : Enemy
    {


        public FireGolemBoss(int id, int damage, int critChance, int maxHealth, int currentHealth) : base(id, damage, critChance, maxHealth, currentHealth) { }

        //load variables from the database
        void Awake()
        {
            this.Damage = EnemyClassDB.getEnemy(4).Damage;
            this.MaxHP = EnemyClassDB.getEnemy(4).MaxHP;
            this.CurrentHP = EnemyClassDB.getEnemy(4).CurrentHP;
            this.CritChance = EnemyClassDB.getEnemy(4).CritChance;
        }

    }
}