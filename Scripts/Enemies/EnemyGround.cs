using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public class EnemyGround : Enemy
    {
        public EnemyGround(int id, int damage, int critChance, int maxHealth, int currentHealth) : base(id, damage, critChance, maxHealth, currentHealth) { }

        //load variables from the database
        void Awake()
        {
            this.Damage = EnemyClassDB.getEnemy(0).Damage;
            this.MaxHP = EnemyClassDB.getEnemy(0).MaxHP;
            this.CurrentHP = EnemyClassDB.getEnemy(0).CurrentHP;
            this.CritChance = EnemyClassDB.getEnemy(0).CritChance;
        }
    }
}