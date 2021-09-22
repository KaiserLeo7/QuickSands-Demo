using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

//COMMENTED BY FARAMARZ HOSSEINI

namespace Sands
{
    public class PartyEncounter : MonoBehaviour
    {
        [SerializeField] private Camera cam;    //a reference to the scene's main camera

        //starts the encounter (to be called from EncounterSystem)
        public void Begin()
        {
            MoveHeroAimation();
            Parallex.ShouldMove = true;
            StartCoroutine(HandleTiming());
        }

        //by precise timing it zooms the camera in and moves it to the left and then shows a text on top of a hero
        private IEnumerator HandleTiming()
        {
            yield return new WaitForSeconds(.5f);
            StartCoroutine(ZoomCameraIn());
            StartCoroutine(MoveCameraLeft());
            yield return new WaitForSeconds(2f);

            //selects a random hero from the party who is not dead
            int selectedHero;
            do
            {
                selectedHero = UnityEngine.Random.Range(0, EncounterSystem.InstantiatedHeroes.Length);
            } while (EncounterSystem.HeroBattleCheckers[selectedHero].IsDead);

            //if person encounter has happened
            if (EncounterSystem.PersonDidHappen)
            {
                EncounterSystem.PersonDidHappen = false;
                //if the player robbed the person
                if (EncounterSystem.DidRob)
                {
                    EncounterSystem.DidRob = false;
                    switch (UnityEngine.Random.Range(0, 4))
                    {
                        case 0:
                            StartCoroutine(DialoguePopUp("Did we do the right thing", EncounterSystem.InstantiatedHeroes[selectedHero].position));
                            break;
                        case 1:
                            StartCoroutine(DialoguePopUp("That guy deserved to be robbed!", EncounterSystem.InstantiatedHeroes[selectedHero].position));
                            break;
                        case 2:
                            StartCoroutine(DialoguePopUp("I feel bad about what we did", EncounterSystem.InstantiatedHeroes[selectedHero].position));
                            break;
                        case 3:
                            StartCoroutine(DialoguePopUp("Leaving people to die is dope", EncounterSystem.InstantiatedHeroes[selectedHero].position));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (UnityEngine.Random.Range(0, 4))
                    {
                        case 0:
                            StartCoroutine(DialoguePopUp("Feels good to help people", EncounterSystem.InstantiatedHeroes[selectedHero].position));
                            break;
                        case 1:
                            StartCoroutine(DialoguePopUp("Why help people", EncounterSystem.InstantiatedHeroes[selectedHero].position));
                            break;
                        case 2:
                            StartCoroutine(DialoguePopUp("Should have robed him", EncounterSystem.InstantiatedHeroes[selectedHero].position));
                            break;
                        case 3:
                            StartCoroutine(DialoguePopUp("Happy to help", EncounterSystem.InstantiatedHeroes[selectedHero].position));
                            break;
                        default:
                            break;
                    }
                }
            }
            //regular scripts
            else
            {
                switch (UnityEngine.Random.Range(0, 4))
                {
                    case 0:
                        StartCoroutine(DialoguePopUp("I hate sand!", EncounterSystem.InstantiatedHeroes[selectedHero].position));
                        break;
                    case 1:
                        StartCoroutine(DialoguePopUp("I like Andrew!", EncounterSystem.InstantiatedHeroes[selectedHero].position));
                        break;
                    case 2:
                        StartCoroutine(DialoguePopUp("My back hurts!", EncounterSystem.InstantiatedHeroes[selectedHero].position));
                        break;
                    case 3:
                        StartCoroutine(DialoguePopUp("I feel like a hero!", EncounterSystem.InstantiatedHeroes[selectedHero].position));
                        break;
                    default:
                        break;
                }
            }            
            
            //after 3 seconds zoom the camera out and continue the EncounterSystem's RunEncounters
            yield return new WaitForSeconds(3f);
            StartCoroutine(ZoomCameraOut());
            StartCoroutine(MoveCameraRight());
            yield return new WaitForSeconds(3f);
            EncounterSystem.ContinueFunction = true;
        }

        public IEnumerator ZoomCameraIn(float countdownValue = 1.7f)
        {
            while (countdownValue > 0)
            {
                cam.orthographicSize -= 0.04f;

                yield return new WaitForSeconds(.01f);
                countdownValue -= 0.04f;
            }
        }

        public IEnumerator MoveCameraLeft(float countdownValue = 3f)
        {
            while (countdownValue > 0)
            {
                cam.transform.position += new Vector3(-0.07f, 0, 0);

                yield return new WaitForSeconds(.01f);
                countdownValue -= 0.07f;
            }
        }

        public IEnumerator ZoomCameraOut(float countdownValue = 1.7f)
        {
            while (countdownValue > 0)
            {
                cam.orthographicSize += 0.04f;

                yield return new WaitForSeconds(.01f);
                countdownValue -= 0.04f;
            }
        }

        public IEnumerator MoveCameraRight(float countdownValue = 3f)
        {
            while (countdownValue > 0)
            {
                cam.transform.position += new Vector3(0.07f, 0, 0);

                yield return new WaitForSeconds(.01f);
                countdownValue -= 0.07f;
            }
        }

        //Instantiates a dialogue at the given position with the text and destroys it after 3 seconds
        public IEnumerator DialoguePopUp(string text, Vector3 position)
        {
            GameObject dialoguePrefab = (GameObject)Resources.Load("ChatBubbleHolder", typeof(GameObject));
            Transform instantiatedDialogue = Instantiate(dialoguePrefab.transform, position + new Vector3(0.7f, 1.4f, 0), Quaternion.Euler(0, 0, 0));
            instantiatedDialogue.GetChild(0).GetChild(0).GetComponent<Text>().text = text;

            yield return new WaitForSeconds(3f);
            Destroy(instantiatedDialogue.gameObject);
        }

        //move animation based on the vehicle availability
        private void MoveHeroAimation()
        {
            if (Player.HasVehicle)
                EncounterSystem.InstantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "run";
            else
                EncounterSystem.InstantiatedHeroes[0].GetComponent<Animator>().SetBool("Running", true);
        }
    }
}
