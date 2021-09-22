using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System;
using UnityEngine.Rendering;

namespace Sands
{
    public class PersonEncounter : MonoBehaviour
    {
        //persons
        private Transform instantiatedPerson;
        [SerializeField] private Transform personStation;

        //popups
        [SerializeField] private GameObject WarriorPopUp;
        [SerializeField] private GameObject PersonEncounterFin;
        [SerializeField] private GameObject ResolutionRobText;
        [SerializeField] private GameObject ResolutionAidPopup;

        //warrior descriptions
        [SerializeField] private GameObject warriorEncounterText;
        [SerializeField] private GameObject WarriorResolutionAidText;

        //mage descriptions
        [SerializeField] private GameObject mageEncounterText;
        [SerializeField] private GameObject mageResolutionAidText;
        //ranger descriptions
        [SerializeField] private GameObject rangerEncounterText;
        [SerializeField] private GameObject rangerResolutionBuyText;

          //buttons
        [SerializeField] private GameObject warriorRobButt;
        [SerializeField] private GameObject warriorAidButt;
        [SerializeField] private GameObject warriorIgnoreButt;
        [SerializeField] private GameObject rangerRobButt;
        [SerializeField] private GameObject rangerBuyButt;
        [SerializeField] private GameObject rangerIgnoreButt;
        [SerializeField] private GameObject mageLieButt;
        [SerializeField] private GameObject mageTruthButt;
        [SerializeField] private GameObject mageRobbButt;
        
        //inventory stuff
        [SerializeField] Text goldAmount;
        [SerializeField] Text strangerFaction;
        [SerializeField] Text strangerFactionRob;
        [SerializeField] Text strangerFactionResolution;
        GameObject itemHolder1, itemHolder2, itemHolder3;
        [SerializeField] GameObject itemHolder1Pos, itemHolder2Pos, itemHolder3Pos;

        //global variables that help with functions
        Tradeable item1;
        Tradeable item2;
        Tradeable item3;
        int randomID = 0;
        int randomID2 = 0;
        int randomAmount = 0;
        int randomAmount2 = 0;
        System.Random itemRandy = new System.Random();
        int factionID = 0;
        private GameObject personPrefab;
        private Coroutine MovePersonInC = null;
        private Coroutine MovePersonOutC = null;
        int randy;
        [SerializeField] private GameObject Gauge;

        //Spawn person to be encountered and move person and hero towards each other
        public void Begin()
        {
             randy = UnityEngine.Random.Range(1, 4);

            Parallex.ShouldMove = true;
            MoveHeroAimation();
            SpawnPerson();
            StartCoroutine(PersonMovement());
            MoveEnemyAnimation();
        }

        //spawns the person encounter based on randy in begin(), stops their movement and sets the skin of the person
        private void SpawnPerson()
        {
            System.Console.WriteLine(randy);
            Parallex.ShouldMove = false;
            StopHeroAnimation();
            //instantiate a warrior 
            if (randy < 2)
            {
                //make characters move and instantiate pop-up with relevant encounter information/choices
                Hero warrior = new Warrior((Warrior)HeroClassDB.getHero(0));
                personPrefab = (GameObject)Resources.Load(warrior.GetType().Name, typeof(GameObject));
            }
            //instantiate a mage 
            else if (randy < 3)
            {
                //make characters move and instantiate pop-up with relevant encounter information/choices
                Hero mage = new Mage((Mage)HeroClassDB.getHero(1));
                personPrefab = (GameObject)Resources.Load(mage.GetType().Name, typeof(GameObject));
            }
            //instantiate a ranger
            else if (randy < 4)
            {
                //make characters move and instantiate pop-up with relevant encounter information/choices
                Hero ranger = new Ranger((Ranger)HeroClassDB.getHero(2));
                personPrefab = (GameObject)Resources.Load(ranger.GetType().Name, typeof(GameObject));
            }
            
            personPrefab.GetComponent<Hero>().SkinTire = 1;
            personPrefab.GetComponent<Hero>().setSkin(personPrefab);
            personPrefab.GetComponent<SortingGroup>().sortingOrder = 9;

            instantiatedPerson = Instantiate(personPrefab.transform, personStation.position, Quaternion.Euler(0, 180, 0));
        }

        //creates first person encounter pop-up and activate texts based on the randy variable
        private void activatePopup()
        {
            //spawns warrior encounter pop up, activates a aid option if player has the money
            if (randy < 2)
            {
                WarriorPopUp.SetActive(true);
                warriorEncounterText.SetActive(true);
                factionSetter();
                if (PlayerInventory.Money >= 1000)
                    warriorAidButt.SetActive(true);
                warriorIgnoreButt.SetActive(true);
                warriorRobButt.SetActive(true);
            }
            //spawns mage encounter pop up
            else if (randy < 3)
            {
                WarriorPopUp.SetActive(true);
                mageEncounterText.SetActive(true);
                factionSetter();
                mageLieButt.SetActive(true);
                mageRobbButt.SetActive(true);
                mageTruthButt.SetActive(true);
            }
            //spawns ranger encounter pop up activates a aid option if player has the money
            else if (randy < 4)
            {
                WarriorPopUp.SetActive(true);
                rangerEncounterText.SetActive(true);
                factionSetter();
                rangerRobButt.SetActive(true);
                if (PlayerInventory.Money >= 3000)
                    rangerBuyButt.SetActive(true);
                rangerIgnoreButt.SetActive(true);
            }
        }

        //generate loot from robbing person encounter, subtract reputation of faction in nearby town
        public void robHimButton()
        {
            WarriorPopUp.SetActive(false);

            int num = UnityEngine.Random.Range(10, 21);
            int faction = Player.LocationToTravelTo.Territory - 1;
            Player.FactionReputation[faction] -= (ushort)num;          
            PersonEncounterFin.SetActive(true);
            generateLoot();
            ResolutionRobText.SetActive(true);
            strangerFactionRob.text = factionName(factionID) + " - " + num;
            EncounterSystem.DidRob = true;

        }

        //lose loot from aiding warrior encounter, gain reputation with faction in nearby town
        public void aidWarriorButton()
        {
            WarriorPopUp.SetActive(false);

            int num = UnityEngine.Random.Range(10, 21);
            int faction = Player.LocationToTravelTo.Territory - 1;
            Player.FactionReputation[faction] += (ushort)num;

            ResolutionAidPopup.SetActive(true);
            WarriorResolutionAidText.SetActive(true);
            strangerFactionResolution.text = factionName(factionID) + " + " + num;
            if (PlayerInventory.Money >= 1000)
            {
                PlayerInventory.Money -= 1000;
                goldAmount.text = "-1000";

            }   
        }

        //A loot box system that spawns loot based on rng, 1/20 chance of making value
        public void aidRangerButton()
        {     
            WarriorPopUp.SetActive(false);
            int num = UnityEngine.Random.Range(10, 21);
            int faction = Player.LocationToTravelTo.Territory - 1;
            Player.FactionReputation[faction] += (ushort)num;

            int diamondChance = UnityEngine.Random.Range(1, 21);

            if (diamondChance <= 5)
                lootBox(1);
            else if (diamondChance <= 10)
                lootBox(3);
            else if (diamondChance <= 15)
                lootBox(5);
            else if (diamondChance <= 19)
                lootBox(7);
            else
                lootBox(9);

            ResolutionAidPopup.SetActive(true);
            rangerResolutionBuyText.SetActive(true);
            strangerFactionResolution.text = factionName(factionID) + " + " + num;
            PlayerInventory.Money -= 1000;
            goldAmount.text = " -3000";
        }

        //gives player money and adds reputation to relevant faction
        public void tellMageButton()
        {
            WarriorPopUp.SetActive(false);
            int num = UnityEngine.Random.Range(10, 21);
            int faction = Player.LocationToTravelTo.Territory - 1;
            Player.FactionReputation[faction] += (ushort)num;
            ResolutionAidPopup.SetActive(true);
            mageResolutionAidText.SetActive(true);
            strangerFactionResolution.text = factionName(factionID) + " + " + num;
            PlayerInventory.Money += 2000;
            goldAmount.text = " + 2000";
        }

        //encounter ends without any change, next party encounter will have the party reacting to this descision
        public void leaveHimButton()
        {
            EncounterSystem.DidRob = true;
            WarriorPopUp.SetActive(false);
            moveOn();

        }

        //The lootbox system called by the aidrangerbutton, checks if player has capacity for the new items. adds specified items to inventory and loads the picture/number into the scene 
        public void lootBox(int id)
        {
            int capacity = 0;
            foreach (var item in HeroPartyDB.getHeroList())
            {
                capacity += item.Capacity;
            }
            int carrying = 0;
            foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
            {
                carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;
            }

            if (Player.HasVehicle)
                capacity += Player.CurrentVehicle.Capacity;

            item3 = TradeableDatabase.getTradeable(id);

            if (carrying + item3.Weight * 5 <= capacity)
            {
                GameObject itemHolder = (GameObject)Resources.Load("ItemHolder", typeof(GameObject));
                //put loot from loot screen in player inventory
                PlayerInventory.AddToInventory(id + 1, 5);

                itemHolder3 = Instantiate(itemHolder, itemHolder3Pos.transform);

                itemHolder3.GetComponent<ItemHolder>().itemNameText.text = item3.ItemName;
                itemHolder3.GetComponent<ItemHolder>().priceText.text = "";
                itemHolder3.GetComponent<ItemHolder>().countText.text = "5";
                itemHolder3.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item3.ItemName);
                itemHolder3Pos.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
                carrying = 0;
                foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
                    carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;              
            }
            else
                PlayerInventory.Money += 1000;
        }


        //generates random loot, the picture/number of loot and places it in inventory. This is called by the rob function
        public void generateLoot()
        {
            int capacity = 0;
            foreach (var item in HeroPartyDB.getHeroList())
            {
                capacity += item.Capacity;
            }
            int carrying = 0;
            foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
            {
                carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;
            }

            if (Player.HasVehicle)
                capacity += Player.CurrentVehicle.Capacity;

            int partySize = 1;

            do
            {
                if (Player.HasVehicle)
                {
                    partySize = Player.CurrentVehicle.PartySize;
                }
                if (partySize == 1)
                {
                    randomID = itemRandy.Next(1, 3);
                    randomID2 = itemRandy.Next(1, 3);


                }
                if (partySize == 2)
                {
                    randomID = itemRandy.Next(1, 5);
                    randomID2 = itemRandy.Next(1, 5);


                }
                if (partySize == 3)
                {
                    randomID = itemRandy.Next(1, 7);
                    randomID2 = itemRandy.Next(1, 7);


                }
                if (partySize == 4 || partySize == 5)
                {
                    randomID = itemRandy.Next(1, 11);
                    randomID2 = itemRandy.Next(1, 11);


                }
            } while (randomID == randomID2);



            randomAmount = itemRandy.Next(1, 5);
            randomAmount2 = itemRandy.Next(1, 5);


            //loot items and amount
            item1 = TradeableDatabase.getTradeable(randomID - 1);
            item2 = TradeableDatabase.getTradeable(randomID2 - 1);

            GameObject itemHolder = (GameObject)Resources.Load("ItemHolder", typeof(GameObject));

            itemHolder1 = Instantiate(itemHolder, itemHolder1Pos.transform);
            itemHolder2 = Instantiate(itemHolder, itemHolder2Pos.transform);

            itemHolder1.GetComponent<ItemHolder>().itemNameText.text = item1.ItemName;
            itemHolder1.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder1.GetComponent<ItemHolder>().countText.text = randomAmount.ToString();
            itemHolder1.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item1.ItemName);

            itemHolder2.GetComponent<ItemHolder>().itemNameText.text = item2.ItemName;
            itemHolder2.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder2.GetComponent<ItemHolder>().countText.text = randomAmount2.ToString();
            itemHolder2.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item2.ItemName);

            itemHolder1Pos.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            itemHolder2Pos.transform.localScale = new Vector3(0.75f, 0.75f, 1f);

            if (carrying + item1.Weight*randomAmount + item2.Weight*randomAmount <= capacity)
            {
                //put loot from loot screen in player inventory
                    PlayerInventory.AddToInventory(randomID, randomAmount);
                    PlayerInventory.AddToInventory(randomID2, randomAmount);
                    carrying = 0;
                    foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
                    {
                        carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;
                    }

                
            }
        }


       //Faction name in pop up based on what faction encounter is
        public string factionName(int factNum)
        {
              if (factNum == 1)
               return "Republic of Veden";
            if (factNum == 2)
                return "Fara Empire";
            if (factNum == 3)
                return "The Kaiserreich";
            else
                return "Nowhere";
        }

        //sets the faction for the person encounter based on 50/50 chance between the towns the player is travelling between
        public int factionSetter()
        {
            int randomFaction = UnityEngine.Random.Range(1, 3);
            if (randomFaction == 1)
            {
                strangerFaction.text = factionName(Player.LocationToTravelTo.Territory);
                factionColour(Player.LocationToTravelTo.Territory);
                factionID = Player.LocationToTravelTo.Territory;
                return factionID;
            }
            else if (randomFaction == 2)
            {
                strangerFaction.text = factionName(Player.CurrentLocation.Territory);
                factionColour(Player.CurrentLocation.Territory);
                factionID = Player.CurrentLocation.Territory;
                return factionID;
            }
            else
                return factionID;
        }

        //sets faction text on pop-up to relevant faction
        public void factionColour(int factNum)
        {
            if (factNum == 1)
            {
                strangerFaction.color = Color.blue;
                strangerFactionRob.color = Color.blue;
                strangerFactionResolution.color = Color.blue;
            }
            if (factNum == 2)
            {
                strangerFaction.color = Color.green;
                strangerFactionRob.color = Color.green;
                strangerFactionResolution.color = Color.green;
            }
            if (factNum == 3)
            {
                strangerFaction.color = Color.red;
                strangerFactionRob.color = Color.red;
                strangerFactionResolution.color = Color.red;

            }
        }

        //moves person into the scene and activates the pop up
        private IEnumerator PersonMovement()
        {
            MovePersonInC = StartCoroutine(MovePersonIn());
            yield return new WaitForSeconds(0.9f);
            
            activatePopup();

        }

        //moves person into the scene and activates the pop up
        public IEnumerator MovePersonIn(float countdownValue = 1.5f)
        {
            Vector3 pos = new Vector3(6.56f, 0.08f, 0f);
            while (countdownValue > 0 && instantiatedPerson.transform.position.x > pos.x)
            {
                try
                {
                    if (instantiatedPerson.position != pos)
                    {
                        if (Player.HasVehicle)
                        {
                            Vector3 newPos = Vector3.MoveTowards(instantiatedPerson.transform.position, pos, 0.12f);
                            instantiatedPerson.transform.position = newPos;
                        }
                        else
                        {
                            Vector3 newPos = Vector3.MoveTowards(instantiatedPerson.transform.position, pos, 0.09f);
                            instantiatedPerson.transform.position = newPos;
                        }
                    }
                    StopEnemyAnimation();
                }
                catch (Exception) { }

                yield return new WaitForSeconds(.01f);
                countdownValue -= 0.01f;
            }
            StopCoroutine(MovePersonInC);
            Parallex.ShouldMove = false;
            StopHeroAnimation();

        }
        //moves person out of the scene 
        public IEnumerator MovePersonOut(float countdownValue = 5f)
        {
            MoveEnemyAnimation();
            Vector3 pos = new Vector3(-15f, 0.059f, -10.25f);
            while (countdownValue > 0)
            {
                try
                {
                    if (instantiatedPerson.position != pos)
                    {

                        if (Player.HasVehicle)
                        {

                            Vector3 newPos = Vector3.MoveTowards(instantiatedPerson.transform.position, pos, 0.12f);
                            instantiatedPerson.transform.position = newPos;
                        }
                        else
                        {
                            Vector3 newPos = Vector3.MoveTowards(instantiatedPerson.transform.position, pos, 0.09f);
                            instantiatedPerson.transform.position = newPos;
                        }

                    }
                    else
                    {
                        countdownValue = 0;
                    }
                }
                catch (Exception)
                {
                    StopCoroutine(MovePersonOutC);
                }

                yield return new WaitForSeconds(.01f);
                countdownValue -= 0.01f;
            }
            StopCoroutine(MovePersonOutC);

        }

        //makes the hero/vehicle play their move animation
        private void MoveHeroAimation()
        {
            if (Player.HasVehicle)
                EncounterSystem.InstantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "run";
            else
                EncounterSystem.InstantiatedHeroes[0].GetComponent<Animator>().SetBool("Running", true);
        }


        //makes the hero/vehicle stop their move animation
        private void StopHeroAnimation()
        {

            if (Player.HasVehicle)
                EncounterSystem.InstantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
            else
                EncounterSystem.InstantiatedHeroes[0].GetComponent<Animator>().SetBool("Running", false);
        }

        //makes the enemy play their move animation
        private void MoveEnemyAnimation()
        {
            instantiatedPerson.GetComponent<Animator>().SetBool("Running", true);
        }

        //makes the enemy stop playing their move animation
        private void StopEnemyAnimation()
        {
            instantiatedPerson.GetComponent<Animator>().SetBool("Running", false);
        }
        
        //closes all pop-ups/texts/buttons/item pictures, and begins the hero and enemies movement
        private IEnumerator moveOnCoroutine()
        {
            PersonEncounterFin.SetActive(false);
            warriorEncounterText.SetActive(false);
            rangerEncounterText.SetActive(false);
            mageEncounterText.SetActive(false);
            WarriorPopUp.SetActive(false);
            ResolutionAidPopup.SetActive(false);
            mageLieButt.SetActive(false);
            mageRobbButt.SetActive(false);
            mageTruthButt.SetActive(false);
            rangerRobButt.SetActive(false);
            rangerBuyButt.SetActive(false);
            rangerIgnoreButt.SetActive(false);
            rangerResolutionBuyText.SetActive(false);
            mageResolutionAidText.SetActive(false);
            WarriorResolutionAidText.SetActive(false);
            Destroy(itemHolder1);
            Destroy(itemHolder2);
            Destroy(itemHolder3);
            goldAmount.text = "";
            MovePersonOutC = StartCoroutine(MovePersonOut());
            MoveHeroAimation();
            Parallex.ShouldMove = true;
            yield return new WaitForSeconds(5f);
            Destroy((GameObject)instantiatedPerson.gameObject);
            EncounterSystem.ContinueFunction = true;
        }

        //calls the move on coroutine
            public void moveOn()
        {       
            StartCoroutine(moveOnCoroutine());           
        }


    }
 }
