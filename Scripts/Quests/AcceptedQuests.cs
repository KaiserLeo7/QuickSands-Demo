using UnityEngine;
using UnityEngine.UI;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    public class AcceptedQuests : MonoBehaviour
    {
        [SerializeField] private GameObject questHolder;////////////////////////////////////
        [SerializeField] private GameObject content;                                      //
        [SerializeField] private GameObject questPopUp;                                   //
        [SerializeField] private Text questName;                                          //
        [SerializeField] private Text questFaction;                                       //
        [SerializeField] private Text questDescription;     //references from the scene   //
        [SerializeField] private Text questReward;                                        //
        [SerializeField] private Text questDistance;                                      //
        [SerializeField] private GameObject completeButton;                               //
        [SerializeField] private GameObject emptyQuestText;/////////////////////////////////

        private GameObject[] instantiatedQuestHolder = new GameObject[1];
        private bool[] completedQuests;           //a list of all the quests that are completed
        private int selctedQuest;                 //the quest player clicks on

        public void QuestsOnClick()
        {
            //checks if there are no quests activates the empty text
            ActivateEmptyText();

            //if there are any questHolders destroy them all
            if (instantiatedQuestHolder[0])
            {
                DestroyAllQuests();
            }

            Player.LoadPlayer();

            if(Player.AcceptedQuests.Count > 0)
            {
                instantiatedQuestHolder = new GameObject[Player.AcceptedQuests.Count];
                completedQuests = new bool[Player.AcceptedQuests.Count];

                //check what quests are complete
                CheckQuests();

                questHolder.SetActive(true);

                //load the two prefabs
                GameObject questHolderPrefab = (GameObject)Resources.Load("QuestHolder", typeof(GameObject));
                GameObject finishedQuestPrefab = (GameObject)Resources.Load("QuestHolderFinished", typeof(GameObject));

                //instantiate and populate quests
                for (int i = 0; i < Player.AcceptedQuests.Count; i++)
                {
                    if(completedQuests[i])
                        instantiatedQuestHolder[i] = Instantiate(finishedQuestPrefab);
                    else
                        instantiatedQuestHolder[i] = Instantiate(questHolderPrefab);

                    instantiatedQuestHolder[i].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
                    instantiatedQuestHolder[i].transform.SetParent(content.transform);

                    instantiatedQuestHolder[i].GetComponent<QuestHolder>().questNameText.text = Player.AcceptedQuests[i].QuestName;

                    switch (Player.AcceptedQuests[i].QuestLocation.Territory)
                    {
                        case 1:
                            instantiatedQuestHolder[i].GetComponent<QuestHolder>().factionText.text = "Republic of Veden";
                            instantiatedQuestHolder[i].GetComponent<QuestHolder>().factionText.color = Color.blue;
                            break;
                        case 2:
                            instantiatedQuestHolder[i].GetComponent<QuestHolder>().factionText.text = "Fara Empire";
                            instantiatedQuestHolder[i].GetComponent<QuestHolder>().factionText.color = Color.green;
                            break;
                        case 3:
                            instantiatedQuestHolder[i].GetComponent<QuestHolder>().factionText.text = "The Kaiserreich";
                            instantiatedQuestHolder[i].GetComponent<QuestHolder>().factionText.color = Color.red;
                            break;
                        default:
                            break;
                    }

                    instantiatedQuestHolder[i].GetComponent<QuestHolder>().rewardText.text = Player.AcceptedQuests[i].QuestReward + " C";
                    instantiatedQuestHolder[i].GetComponent<QuestHolder>().distanceText.text = "";

                    //add a listener to the quest
                    int index = i;
                    instantiatedQuestHolder[i].GetComponent<Button>().onClick.AddListener(() =>
                    {
                        FillInQuest(index);
                    });
                }
            }
        }

        //checks if a quest is complete puts it in completedQuests
        private void CheckQuests()
        {
            for (int i = 0; i < Player.AcceptedQuests.Count; i++)
            {
                try
                {
                    //try fails if it's a battleQuest
                    DeliveryQuest deliveryQuest = (DeliveryQuest)Player.AcceptedQuests[i];
                    if (Player.CurrentLocation.Id == Player.AcceptedQuests[i].QuestLocation.Id)
                    {
                        foreach (var tradeableInv in PlayerInventory.TradeableInventory)
                        {
                            //player should have the quest's requirement
                            if(tradeableInv.OwnedTradeable.Id == deliveryQuest.ChosenTradeable.Id)
                            {
                                if (deliveryQuest.ChosenAmount - tradeableInv.Count <= 0)
                                    completedQuests[i] = true;

                                break;
                            }
                        }
                    }
                }
                catch (System.Exception) 
                {
                    if(Player.AcceptedQuests[i].Completed)
                        completedQuests[i] = true;
                }
            }
        }

        //fills all the variables in a quest pop up
        private void FillInQuest(int index)
        {
            selctedQuest = index;
            questPopUp.SetActive(true);

            //activates the complete button if the quest is completed
            if (completedQuests[index])
                completeButton.SetActive(true);

            questName.text = Player.AcceptedQuests[index].QuestName;
            questDescription.text = Player.AcceptedQuests[index].QuestDescription;
            questReward.text = Player.AcceptedQuests[index].QuestReward + " Coin";
            questDistance.text = "";

            //setting the faction
            if (Player.AcceptedQuests[index].QuestLocation.Territory == 1)
            {
                questFaction.text = "Republic of Veden";
                questFaction.color = Color.blue;
            }
            else if (Player.AcceptedQuests[index].QuestLocation.Territory == 2)
            {
                questFaction.text = "Fara Empire";
                questFaction.color = Color.green;
            }
            else if (Player.AcceptedQuests[index].QuestLocation.Territory == 3)
            {
                questFaction.text = "The Kaiserreich";
                questFaction.color = Color.red;
            }
            else
                questFaction.text = "123 Fakestreet";
        }

        public void CloseQuestOnClick()
        {
            completeButton.SetActive(false);
        }

        public void DestroyAllQuests()
        {
            for (int i = 0; i < instantiatedQuestHolder.Length; i++)
            {
                Destroy(instantiatedQuestHolder[i]);
            }
        }

        public void CompleteQuestOnClick()
        {
            //tries to cast the quest to a deliveryQuest and remove the items from inventory
            try
            {
                PlayerInventory.RemoveFromInventory(((DeliveryQuest)Player.AcceptedQuests[selctedQuest]).ChosenTradeable.Id, ((DeliveryQuest)Player.AcceptedQuests[selctedQuest]).ChosenAmount);
                PlayerInventory.Money += Player.AcceptedQuests[selctedQuest].QuestReward;
            }
            catch (System.Exception)
            {
                PlayerInventory.Money += Player.AcceptedQuests[selctedQuest].QuestReward;
            }

            PlayerInventory.SavePlayerInventory();

            //removes the quest
            Player.AcceptedQuests.RemoveAt(selctedQuest);
            Player.SavePlayer();
            QuestsOnClick();

            completeButton.SetActive(false);
            questPopUp.SetActive(false);

            if (Player.AcceptedQuests.Count == 0)
                questHolder.SetActive(false);
        }

        //removes a quest that player wants to abandon
        public void AbandonQuestOnClick()
        {
            Player.AcceptedQuests.RemoveAt(selctedQuest);
            Player.SavePlayer();
            QuestsOnClick();
            completeButton.SetActive(false);
            questPopUp.SetActive(false);

            if (Player.AcceptedQuests.Count == 0)
                questHolder.SetActive(false);
        }

        //checks if there are no quests activates the empty text
        private void ActivateEmptyText()
        {
            if (Player.AcceptedQuests.Count == 0)
                emptyQuestText.SetActive(true);
            else
                emptyQuestText.SetActive(false);
        }

    }

}