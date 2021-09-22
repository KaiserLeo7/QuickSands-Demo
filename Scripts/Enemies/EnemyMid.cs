using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public class EnemyMid : Enemy
    {
        public EnemyMid(int id, int damage, int critChance, int maxHealth, int currentHealth) : base(id, damage, critChance, maxHealth, currentHealth) { }

        //load variables from the database
        void Awake()
        {
            this.Damage = EnemyClassDB.getEnemy(5).Damage;
            this.MaxHP = EnemyClassDB.getEnemy(5).MaxHP;
            this.CurrentHP = EnemyClassDB.getEnemy(5).CurrentHP;
            this.CritChance = EnemyClassDB.getEnemy(5).CritChance;
        }
    }
}