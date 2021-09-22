using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using System;

namespace Sands {

    public class CharAction : MonoBehaviour {

        private CharAnimation charAnim;

        private State state;

        private Action onAttackComplete;
        private Action onHealComplete;


        private float attackEndTime;
        private float healEndTime;

        private enum State {
            Idle,
            BusyAttacking,
            BusyHealing,
        }


        private void Awake() {
            state = State.Idle;

            charAnim = GetComponent<CharAnimation>();
        }

        private void Start() {
        }

        public void PlayIdleAnim() {

            charAnim.IdleAnim();
        }

        //check every frame
        private void Update() {
            switch (state) {
                case State.Idle:
                    break;
                case State.BusyAttacking:
                    if (Time.time > attackEndTime) {
                        onAttackComplete();
                    }
                    break;
                case State.BusyHealing:
                    if (Time.time > healEndTime) {
                        onHealComplete();
                    }
                    break;
            }
        }

   




        //Allows for the execution of either a moving or still attack by either Hero Units
        public void Attack( Action onAttackComplete) {

           
            //Attack Target
            state = State.BusyAttacking;
            charAnim.AttackAnim();

            //call a function from BattleSystem to calculate damage
            StartCoroutine(BattleSystem2.instance.CalculateDamage());

            attackEndTime = Time.time + .8f;

            this.onAttackComplete = () => {
                    
                state = State.Idle;

                onAttackComplete();
            };
        }
   



        public void Heal(Action onHealComplete) {
            
            state = State.BusyHealing;
            
            //Trigger a healing animation
            charAnim.HealAnim();
        
            //calculate the healing

            healEndTime = Time.time + .5f;

            this.onHealComplete = () => {
                state = State.Idle;

            onHealComplete();
            };
        }




    }
}