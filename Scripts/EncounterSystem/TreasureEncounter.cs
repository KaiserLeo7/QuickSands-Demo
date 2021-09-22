using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    public class TreasureEncounter : MonoBehaviour
    {
        private GameObject treasurePrefab;              //the chosen prefab is stored here
        private Transform instantiatedTreasure;         //the instantiated treasure is stored here
        [SerializeField] private GameObject lootPopUp;  //the loot pop up gameObject is referenced
        private Coroutine MoveTreasureInC;              //to save the coroutine
        private Coroutine MoveTreasureOutC;             //to save the coroutine

        //starts the encounter (to be called from EncounterSystem)
        public void Begin()
        {
            Parallex.ShouldMove = true;
            MoveHeroAimation();
            InstantiateTreasure();
            StartCoroutine(TreasureMovement());
            
        }

        //chooses one random treasure and instantiates it
        private void InstantiateTreasure()
        {
            int random = UnityEngine.Random.Range(0, 3);

            switch (random)
            {
                case 0:
                    treasurePrefab = (GameObject)Resources.Load("CrateTreasure", typeof(GameObject));
                    break;
                case 1:
                    treasurePrefab = (GameObject)Resources.Load("VaseTreasure", typeof(GameObject));
                    break;
                case 2:
                    treasurePrefab = (GameObject)Resources.Load("BarrelTreasure", typeof(GameObject));
                    break;
                default:
                    break;
            }


            instantiatedTreasure = Instantiate(treasurePrefab.transform, new Vector3(11.66f, 0f, 0f), Quaternion.Euler(0, 0, 0));
        }

        //the treasure is moved in and the pop up is activated
        private IEnumerator TreasureMovement()
        {
            MoveTreasureInC = StartCoroutine(MoveTreasureIn());
            yield return new WaitForSeconds(1.6f);
            
            GenerateLootScreen();
            lootPopUp.SetActive(true);
        }

        //moves the treasure in frim the right side
        public IEnumerator MoveTreasureIn(float countdownValue = 1.5f)
        {
            Vector3 pos = new Vector3(4.56f, 0.08f, 0f);
            while (countdownValue > 0 && instantiatedTreasure.transform.position.x > pos.x)
            {
                try
                {
                    if (instantiatedTreasure.position != pos)
                    {
                        Vector3 newPos = Vector3.MoveTowards(instantiatedTreasure.transform.position, pos, 0.12f);
                        instantiatedTreasure.transform.position = newPos;
                    }
                }
                catch (Exception) { }

                yield return new WaitForSeconds(.01f);
                countdownValue -= 0.01f;
            }

            StopCoroutine(MoveTreasureInC);
            Parallex.ShouldMove = false;
            StopHeroAnimation();
        }

        //moves the treasure out to the left side
        public IEnumerator MoveTreasureOut(float countdownValue = 5f)
        {
            Vector3 pos = new Vector3(-15f, 0.059f, -10.25f);
            while (countdownValue > 0)
            {
                try
                {
                    if (instantiatedTreasure.position != pos)
                    {

                        if (Player.HasVehicle)
                        {

                            Vector3 newPos = Vector3.MoveTowards(instantiatedTreasure.transform.position, pos, 0.12f);
                            instantiatedTreasure.transform.position = newPos;
                        }
                        else
                        {
                            Vector3 newPos = Vector3.MoveTowards(instantiatedTreasure.transform.position, pos, 0.09f);
                            instantiatedTreasure.transform.position = newPos;
                        }

                    }
                }
                catch (Exception)
                {
                    StopCoroutine(MoveTreasureOutC);
                }

                yield return new WaitForSeconds(.01f);
                countdownValue -= 0.01f;
            }
        }

        //move animation based on the vehicle availability
        private void MoveHeroAimation()
        {
            if (Player.HasVehicle)
                EncounterSystem.InstantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "run";
            else
                EncounterSystem.InstantiatedHeroes[0].GetComponent<Animator>().SetBool("Running", true);
        }

        // stops move animation based on the vehicle availability
        private void StopHeroAnimation()
        {
            if (Player.HasVehicle)
                EncounterSystem.InstantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
            else
                EncounterSystem.InstantiatedHeroes[0].GetComponent<Animator>().SetBool("Running", false);
        }
        
        //accesses the lootPopUp component and generates a new loot screen
        public void GenerateLootScreen()
        {
            lootPopUp.GetComponent<LootPopUp>().GenerateLootScreen();
        }

        public void TakeLoot()
        {
            StartCoroutine(TakeLootCoroutine());
        }

        //moves the treasure out, destroys the treasure, and signals the EncounterSystem to go to the next encounter by setting ContinueFunction to true
        private IEnumerator TakeLootCoroutine()
        {
            lootPopUp.SetActive(false);

            MoveTreasureOutC = StartCoroutine(MoveTreasureOut());
            MoveHeroAimation();
            Parallex.ShouldMove = true;
            yield return new WaitForSeconds(5f);
            Destroy(instantiatedTreasure.gameObject);
            EncounterSystem.ContinueFunction = true;
        }


    }
}