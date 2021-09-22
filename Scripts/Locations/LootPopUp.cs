﻿using UnityEngine;
using UnityEngine.UI;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    public class LootPopUp : MonoBehaviour
    {
        [SerializeField] GameObject itemHolder1Pos, itemHolder2Pos;///////////////////////////////////
        [SerializeField] private Text CoinText;                                                     //
        [SerializeField] private GameObject Gauge;                                                  //
        [SerializeField] private GameObject takeAllButton;                                          //
        [SerializeField] private GameObject takeAllButtonBattle;                                    //
        [SerializeField] private GameObject inventoryFullText;                                      //
                                                                                                    //
                                       //reference to the gameObjects in the scene                  //
        //Take Pop UP                                                                               //
        [SerializeField] GameObject itemTakePopUp;                                                  //
        [SerializeField] GameObject countSlider;                                                    //
        [SerializeField] Text itemTakeName;                                                         //
        [SerializeField] Image itemTakeIcon;                                                        //
        [SerializeField] Text itemTakeAmount;                                                       //
        [SerializeField] GameObject itemPopUp;                                                      //
        [SerializeField] GameObject takeBtn;//////////////////////////////////////////////////////////
        private int selectedItem;

        GameObject itemHolder1, itemHolder2;                //stores instantiiated itemHolders
        static Tradeable item1, item2;                      //the 2 selected tradeables
        static int randomID1 = 0, randomID2 = 0;            //the Id of random items
        static int randomAmount1 = 0, randomAmount2 = 0;    //amount of the items
        System.Random randy = new System.Random();
        private int capacity = 0;                           //capacity of the player's party
        static private double coin;                         //amount of money in the loot
        private int carrying = 0;                           //amount player is carrying


        //randomly chooses 2 tradeables
        public void GenerateLootScreen()
        {
            coin = UnityEngine.Random.Range(100, 201);
            CoinText.text = coin.ToString();

            CalculateCarrying();

            int partySize = 1;

            do
            {
                if (Player.HasVehicle)
                {
                    partySize = Player.CurrentVehicle.PartySize;
                }
                if (partySize == 1)
                {
                    randomID1 = randy.Next(1, 3);
                    randomID2 = randy.Next(1, 3);


                }
                if (partySize == 2)
                {
                    randomID1 = randy.Next(1, 5);
                    randomID2 = randy.Next(1, 5);


                }
                if (partySize == 3)
                {
                    randomID1 = randy.Next(1, 7);
                    randomID2 = randy.Next(1, 7);


                }
                if (partySize == 4 || partySize == 5)
                {
                    randomID1 = randy.Next(1, 11);
                    randomID2 = randy.Next(1, 11);


                }
            } while (randomID1 == randomID2);



            randomAmount1 = randy.Next(1, 5);
            randomAmount2 = randy.Next(1, 5);


            //loot items and amount
            item1 = TradeableDatabase.getTradeable(randomID1 - 1);
            item2 = TradeableDatabase.getTradeable(randomID2 - 1);

            GameObject itemHolder = (GameObject)Resources.Load("ItemHolder", typeof(GameObject));

            //instantiates the 2 itemHolders
            itemHolder1 = Instantiate(itemHolder, itemHolder1Pos.transform);
            itemHolder2 = Instantiate(itemHolder, itemHolder2Pos.transform);

            //sets up the itemHolders
            itemHolder1.GetComponent<ItemHolder>().itemNameText.text = item1.ItemName;
            itemHolder1.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder1.GetComponent<ItemHolder>().countText.text = randomAmount1.ToString();
            itemHolder1.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item1.ItemName);

            itemHolder2.GetComponent<ItemHolder>().itemNameText.text = item2.ItemName;
            itemHolder2.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder2.GetComponent<ItemHolder>().countText.text = randomAmount2.ToString();
            itemHolder2.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item2.ItemName);

            //increase the scale
            itemHolder1Pos.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            itemHolder2Pos.transform.localScale = new Vector3(0.75f, 0.75f, 1f);

            CheckFullCargo();
        }

        //calculates how much the player is carrying
        public void CalculateCarrying()
        {
            capacity = 0;
            foreach (var item in HeroPartyDB.getHeroList())
            {
                capacity += item.Capacity;
            }
            carrying = 0;
            foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
            {
                carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;
            }

            if (Player.HasVehicle)
                capacity += Player.CurrentVehicle.Capacity;

            Gauge.GetComponent<Image>().fillAmount = (float)carrying / (float)capacity;
        }

        //adds both items to the inventory
        public void TakeAllLoot()
        {
            inventoryFullText.SetActive(false);

            PlayerInventory.AddToInventory(randomID1, randomAmount1);
            PlayerInventory.AddToInventory(randomID2, randomAmount2);
            PlayerInventory.Money += coin;

            Destroy(itemHolder1);
            Destroy(itemHolder2);

            EncounterSystem.instance.SaveBattleData();
        }

        public void LeaveLoot()
        {
            inventoryFullText.SetActive(false);

            Destroy(itemHolder1);
            Destroy(itemHolder2);

            EncounterSystem.instance.SaveBattleData();
        }

        //checks if the capacity is not full and if it is makes the take all button uninteractable
        public void CheckFullCargo() {

            int lootWeight = 0;

            lootWeight += item1.Weight * randomAmount1;
            lootWeight += item2.Weight * randomAmount2;

            CalculateCarrying();

            if (carrying + lootWeight > capacity)
            {
                takeAllButton.GetComponent<Selectable>().interactable = false;

                try
                {
                    takeAllButtonBattle.GetComponent<Selectable>().interactable = false;
                }
                catch (System.Exception) {  }

                inventoryFullText.SetActive(true);
            }
            else
            {
                takeAllButton.GetComponent<Selectable>().interactable = true;

                try
                {
                    takeAllButtonBattle.GetComponent<Selectable>().interactable = true;
                }
                catch (System.Exception) { }

                inventoryFullText.SetActive(false);
            }

        }

        //Loads items again into the scene
        public void LoadItems()
        {
            GameObject itemHolder = (GameObject)Resources.Load("ItemHolder", typeof(GameObject));

            itemHolder1 = Instantiate(itemHolder, itemHolder1Pos.transform);
            itemHolder2 = Instantiate(itemHolder, itemHolder2Pos.transform);

            itemHolder1.GetComponent<ItemHolder>().itemNameText.text = item1.ItemName;
            itemHolder1.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder1.GetComponent<ItemHolder>().countText.text = randomAmount1.ToString();
            itemHolder1.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item1.ItemName);

            itemHolder2.GetComponent<ItemHolder>().itemNameText.text = item2.ItemName;
            itemHolder2.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder2.GetComponent<ItemHolder>().countText.text = randomAmount2.ToString();
            itemHolder2.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item2.ItemName);

            itemHolder1Pos.transform.localScale = new Vector3(0.60f, 0.75f, 1f);
            itemHolder2Pos.transform.localScale = new Vector3(0.60f, 0.75f, 1f);


            //Making Treasure Loot Interactive

            itemHolder1.GetComponent<Button>().onClick.AddListener(() =>
            {
                ItemOnClick(0);
            });

            itemHolder2.GetComponent<Button>().onClick.AddListener(() =>
            {
                ItemOnClick(1);
            });

            CoinText.text = coin.ToString();

            CalculateCarrying();

            CheckFullCargo();
        }

        //clicking on an item activates the proper pop up and fills it in
        public void ItemOnClick(int index)
        {
            itemTakePopUp.SetActive(true);
            itemPopUp.SetActive(false);
            selectedItem = index;
            if (index == 0) {
                itemTakeName.text = item1.ItemName;
                itemTakeIcon.sprite = Resources.Load<Sprite>(item1.ItemName);
            }
            else {
                itemTakeName.text = item2.ItemName;
                itemTakeIcon.sprite = Resources.Load<Sprite>(item2.ItemName);

            }
            itemTakeAmount.text = 0.ToString();
            SetCountSlider(index);
        }

        //sets up the count slider of the pop up to never exceed the available items
        public void SetCountSlider(int index)
        {
            if (index == 0)
                countSlider.GetComponent<Slider>().maxValue = System.Convert.ToInt32(itemHolder1.GetComponent<ItemHolder>().countText.text);
            else
                countSlider.GetComponent<Slider>().maxValue = System.Convert.ToInt32(itemHolder2.GetComponent<ItemHolder>().countText.text);

            countSlider.GetComponent<Slider>().value = 0;
        }


        //sets the number of the chosen tradeable after the slider changes
        public void SliderOnValueChanged()
        {
            int count = (int)countSlider.GetComponent<Slider>().value;

            itemTakeAmount.text = System.Convert.ToString(count);

            if (selectedItem == 0)
            {
                if (item1.Weight * count + carrying > capacity)
                    takeBtn.GetComponent<Selectable>().interactable = false;
                else
                    takeBtn.GetComponent<Selectable>().interactable = true;
            }
            else if (selectedItem == 1)
            {
                if (item2.Weight * count + carrying > capacity)
                    takeBtn.GetComponent<Selectable>().interactable = false;
                else
                    takeBtn.GetComponent<Selectable>().interactable = true;
            }
        }

        //adds the selected amount of the chosen item to the inventory
        public void TakeOnClick()
        {
            int count = (int)countSlider.GetComponent<Slider>().value;

            if (selectedItem == 0)
            {
                PlayerInventory.AddToInventory(item1.Id, count);

                itemTakeAmount.text = "0";
                randomAmount1 -= count;
                itemHolder1.GetComponent<ItemHolder>().countText.text = (randomAmount1).ToString();

                if (itemHolder1.GetComponent<ItemHolder>().countText.text == "0")
                    Destroy(itemHolder1);
            }
            else if (selectedItem == 1)
            {
                PlayerInventory.AddToInventory(item2.Id, count);

                itemTakeAmount.text = "0";

                randomAmount2 -= count;
                itemHolder2.GetComponent<ItemHolder>().countText.text = (randomAmount2).ToString();

                if (itemHolder2.GetComponent<ItemHolder>().countText.text == "0")
                    Destroy(itemHolder2);
            }

            CalculateCarrying();
            itemTakePopUp.SetActive(false);
            if (randomAmount1 == 0 && randomAmount2 == 0)
            {
                takeAllButton.GetComponent<Selectable>().interactable = false;
                takeAllButtonBattle.GetComponent<Selectable>().interactable = false;
            }
        }
    }
}