using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System;

namespace Sands {
    public class CharAnimation : MonoBehaviour {


        public Animator anim;

        void Start() {
            anim = GetComponent<Animator>();
        
        }
        
        //run attack animation
        public void AttackAnim() {
            
            anim.SetTrigger("Attack");
        }

        //run skill animation
        public void HealAnim() {

            anim.SetTrigger("Skill");
        }

        //run hit animation
        public void HitAnim()
        {
            anim.SetTrigger("Hit");
        }

        //run running animation
        public void RunningAnim() {

            anim.SetBool("Running", true);
        }

        //run idle animation
        public void IdleAnim() {
            
            anim.SetBool("Running", false);
        }



    }
}