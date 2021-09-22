using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    public class QuestBoard : MonoBehaviour
    {
        private static List<Quest> quests;
        private static Location preLocation;
        private System.Random random = new System.Random();
        private BattleQuest battleQuest;
        [SerializeField] private GameObject content;
        private int selectedQuest = -1;
        private static bool[] acceptedQuests = new bool[6];

        [SerializeField] private GameObject questPopUp;//////////////////////////////////
        [SerializeField] private Text questName;                                       //
        [SerializeField] private Text questFaction;                                    //
        [SerializeField] private Text questDescription;   //references from the scene  //
        [SerializeField] private Text questReward;                                     //
        [SerializeField] private Text questDistance;                                   //
        [SerializeField] private Text questError;////////////////////////////////////////
        private GameObject[] instantiatedQuestHolder = new GameObject[6];

        private void Start()
        {
            Player.LoadPlayer();
            TryPopulateQuestList();
            AddQuestsToScene();
        }

        //makes sure we are not in the same town
        private void TryPopulateQuestList()
        {
            try
            {
                //prelocation stays null so a try catch helps with that
                if(preLocation.LocationName != Player.CurrentLocation.LocationName)
                {
                    PopulateQuestList();
                }
            }
            catch (System.Exception)
            {
                PopulateQuestList();
            }
        }   
        
        private void PopulateQuestList()
        {
            for (int i = 0; i < 6; i++)
            {
                acceptedQuests[i] = false;
            }

            preLocation = Player.CurrentLocation;

            quests = new List<Quest>();

            //generate 4 delivery quests
            for (int i = 0; i < 5; i++)
            {
                DeliveryQuest deliveryQuest = new DeliveryQuest(1);
                quests.Add(deliveryQuest);
            }


            //create a new list
            List<Nest> falseNestList = new List<Nest>();

            //get the list of nests
            foreach (Nest nest in NestDB.getNestList())
            {
                //check if any nests are false
                if (!nest.ActiveStatus)
                    //if false add to new list
                    falseNestList.Add(nest);
            }

            //so long as theres at least 1 false acviteStatus nest
            if (falseNestList.Count > 0)
            {

                //generate a quest only from the available locations
                battleQuest = new BattleQuest(falseNestList[random.Next(0, falseNestList.Count)]);
                //add new battleQuest
                quests.Add(battleQuest);
            }
            else
            {
                //otherwise nests all are active
                //no battlequests available
                //generate 6th delivery quest
                quests.Add(new DeliveryQuest(1));
            }
        }

        private void AddQuestsToScene()
        {
            GameObject questHolderPrefab = (GameObject)Resources.Load("QuestHolder", typeof(GameObject));
            for (int i = 0; i < quests.Count; i++)
            {
                instantiatedQuestHolder[i] = Instantiate(questHolderPrefab);
                instantiatedQuestHolder[i].transform.localScale = new Vector3(0.013f, 0.013f, 0.013f);
                instantiatedQuestHolder[i].transform.SetParent(content.transform);

                //fills in the variables in QuestHolder
                instantiatedQuestHolder[i].GetComponent<QuestHolder>().questNameText.text = quests[i].QuestName;
                switch (quests[i].QuestLocation.Territory)
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

                instantiatedQuestHolder[i].GetComponent<QuestHolder>().rewardText.text = "";

                instantiatedQuestHolder[i].GetComponent<QuestHolder>().distanceText.text = quests[i].DistanceNote;

                if (acceptedQuests[i]) 
                    instantiatedQuestHolder[i].GetComponent<Selectable>().interactable = false;

                //adds a listener to make questHolder clickable
                int index = i;
                instantiatedQuestHolder[i].GetComponent<Button>().onClick.AddListener(() =>
                {
                    FillInQuest(index);
                });
            }
        }

        //fills in quest pop up
        private void FillInQuest(int index)
        {
            questPopUp.SetActive(true);
            questName.text = quests[index].QuestName;
            questDescription.text = quests[index].QuestDescription;
            questReward.text = quests[index].QuestReward + " Coin";
            questDistance.text = quests[index].DistanceNote;

            if (quests[index].QuestLocation.Territory == 1)
            {
                questFaction.text = "Republic of Veden";
                questFaction.color = Color.blue;
            }
            else if (quests[index].QuestLocation.Territory == 2)
            {
                questFaction.text = "Fara Empire";
                questFaction.color = Color.green;
            }
            else if (quests[index].QuestLocation.Territory == 3)
            {
                questFaction.text = "The Kaiser Reich";
                questFaction.color = Color.red;
            }
            else
                questFaction.text = "123 Fakestreet";

            selectedQuest = index;
        }

        //accepts a quest
        public void AcceptOnClick()
        {
            if (Player.AcceptedQuests.Count < 9)
            {
                questPopUp.SetActive(false);
                Player.AcceptedQuests.Add(quests[selectedQuest]);
                Player.SavePlayer();
                acceptedQuests[selectedQuest] = true;
                instantiatedQuestHolder[selectedQuest].GetComponent<Selectable>().interactable = false;
            }
            else
            {
                questError.text = "Limit Reached";
            }
        }

        public void CloseOnClick()
        {
            questError.text = "";
        }
    }
}
