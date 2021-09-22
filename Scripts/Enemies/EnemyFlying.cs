using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public class EnemyFlying : Enemy
    {
        public EnemyFlying(int id, int damage, int critChance, int maxHealth, int currentHealth) : base(id, damage, critChance, maxHealth, currentHealth) { }

        //load variables from the database
        void Awake()
        {
            this.Damage = EnemyClassDB.getEnemy(1).Damage;
            this.MaxHP = EnemyClassDB.getEnemy(1).MaxHP;
            this.CurrentHP = EnemyClassDB.getEnemy(1).CurrentHP;
            this.CritChance = EnemyClassDB.getEnemy(1).CritChance;
        }
    }
}