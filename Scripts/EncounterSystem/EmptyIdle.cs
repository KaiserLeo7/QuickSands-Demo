using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Spine.Unity;

namespace Sands
{
    public class EmptyIdle : MonoBehaviour
    {

        private Transform instantiatedHolder;

        private GameObject spriteHolder;

        //starts the encounter  (to be called in EncounterSystem)
        public void Begin()
        {
            Parallex.ShouldMove = true;
            InstantiateTumble();
        }

        //Creates a tumble weed on the right side of the screen
        private void InstantiateTumble()
        {
            int random = UnityEngine.Random.Range(0, 2);

            switch (random)
            {
                case 0:
                    spriteHolder = (GameObject)Resources.Load("WeedHolder", typeof(GameObject));
                    instantiatedHolder = Instantiate(spriteHolder.transform, new Vector3(8.98f,0.4f, 0f), Quaternion.Euler(0, 0, 0));

                    break;
                case 1:
                    spriteHolder = (GameObject)Resources.Load("WheelHolder", typeof(GameObject));
                    instantiatedHolder = Instantiate(spriteHolder.transform, new Vector3(0f, 0f, 0f), Quaternion.Euler(0, 0, 0));

                    break;
                default:
                    break;
            }

            //start the moving animation for the hero or the vehicle
            if (SaveSystem.Pdata.HasVehicle)
            {
                EncounterSystem.InstantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "run";
            }
            else {

                EncounterSystem.InstantiatedHeroes[0].GetComponent<Animator>().SetBool("Running", true);

            }
        }

        //At the end of the tumble weed's animation signals the EncounterSystem that the encounter has finished by setting ContinueFunction to true
        private void End() {
            EncounterSystem.ContinueFunction = true;
        }
    }
}