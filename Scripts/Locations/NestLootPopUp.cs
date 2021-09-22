using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    public class NestLootPopUp : MonoBehaviour
    {
        [SerializeField] GameObject itemHolder1Pos, itemHolder2Pos, itemHolder3Pos, itemHolder4Pos;///////
        [SerializeField] private Text CoinText;                                                         //
        [SerializeField] private GameObject Gauge;                                                      //
        [SerializeField] private GameObject takeAllButton;                                              //
        [SerializeField] private GameObject inventoryFullText;                                          //
                                                                                                        //
                                       //reference to the gameObjects in the scene                      //
        //Take Pop UP                                                                                   //
        [SerializeField] GameObject itemTakePopUp;                                                      //
        [SerializeField] GameObject countSlider;                                                        //
        [SerializeField] Text itemTakeName;                                                             //
        [SerializeField] Image itemTakeIcon;                                                            //
        [SerializeField] Text itemTakeAmount;                                                           //
        [SerializeField] GameObject itemPopUp;                                                          //
        [SerializeField] GameObject takeBtn;//////////////////////////////////////////////////////////////
        private int selectedItem;

        GameObject itemHolder1, itemHolder2, itemHolder3, itemHolder4;                             //stores instantiiated itemHolders
        static Tradeable item1, item2, item3, item4;                                               //the 2 selected tradeables
        static int randomID1 = 0, randomID2 = 0, randomID3 = 0, randomID4 = 0;                     //the Id of random items
        static int randomAmount1 = 0, randomAmount2 = 0, randomAmount3 = 0, randomAmount4 = 0;     //amount of the items

        System.Random randy = new System.Random();                                                 
        private int capacity = 0;                                                                  //capacity of the player's party
        static private double coin;                                                                //amount of money in the loot
        private int carrying = 0;                                                                  //amount player is carrying


        //randomly chooses 4 tradeables
        public void GenerateLootScreen()
        {
            coin = UnityEngine.Random.Range(2000, 4001);
            CoinText.text = coin.ToString();

            CalculateCarrying();

            do
            {
                randomID1 = randy.Next(3, 11);
                randomID2 = randy.Next(4, 11);
            } while (randomID1 == randomID2);

            do
            {
                randomID3 = randy.Next(5, 11);
            } while (randomID3 == randomID1 || randomID3 == randomID2);

            do
            {
                randomID4 = randy.Next(5, 11);
            } while (randomID4 == randomID1 || randomID4 == randomID2 || randomID4 == randomID3);

            randomAmount1 = randy.Next(5, 10);
            randomAmount2 = randy.Next(5, 10);
            randomAmount3 = randy.Next(5, 10);
            randomAmount4 = randy.Next(5, 10);


            //loot items and amount
            item1 = TradeableDatabase.getTradeable(randomID1 - 1);
            item2 = TradeableDatabase.getTradeable(randomID2 - 1);
            item3 = TradeableDatabase.getTradeable(randomID3 - 1);
            item4 = TradeableDatabase.getTradeable(randomID4 - 1);

            GameObject itemHolder = (GameObject)Resources.Load("ItemHolder", typeof(GameObject));

            //instantiates the 2 itemHolders
            itemHolder1 = Instantiate(itemHolder, itemHolder1Pos.transform);
            itemHolder2 = Instantiate(itemHolder, itemHolder2Pos.transform);
            itemHolder3 = Instantiate(itemHolder, itemHolder3Pos.transform);
            itemHolder4 = Instantiate(itemHolder, itemHolder4Pos.transform);

            //sets up the itemHolders
            itemHolder1.GetComponent<ItemHolder>().itemNameText.text = item1.ItemName;
            itemHolder1.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder1.GetComponent<ItemHolder>().countText.text = randomAmount1.ToString();
            itemHolder1.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item1.ItemName);

            itemHolder2.GetComponent<ItemHolder>().itemNameText.text = item2.ItemName;
            itemHolder2.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder2.GetComponent<ItemHolder>().countText.text = randomAmount2.ToString();
            itemHolder2.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item2.ItemName);

            itemHolder3.GetComponent<ItemHolder>().itemNameText.text = item3.ItemName;
            itemHolder3.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder3.GetComponent<ItemHolder>().countText.text = randomAmount3.ToString();
            itemHolder3.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item3.ItemName);

            itemHolder4.GetComponent<ItemHolder>().itemNameText.text = item4.ItemName;
            itemHolder4.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder4.GetComponent<ItemHolder>().countText.text = randomAmount4.ToString();
            itemHolder4.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item4.ItemName);

            //increase the scale
            itemHolder1Pos.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            itemHolder2Pos.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            itemHolder3Pos.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            itemHolder4Pos.transform.localScale = new Vector3(0.75f, 0.75f, 1f);

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
            PlayerInventory.AddToInventory(randomID3, randomAmount3);
            PlayerInventory.AddToInventory(randomID4, randomAmount4);

            PlayerInventory.Money += coin;

            Destroy(itemHolder1);
            Destroy(itemHolder2);
            Destroy(itemHolder3);
            Destroy(itemHolder4);

            PlayerInventory.SavePlayerInventory();
        }


        public void LeaveLoot()
        {
            inventoryFullText.SetActive(false);

            Destroy(itemHolder1);
            Destroy(itemHolder2);
            Destroy(itemHolder3);
            Destroy(itemHolder4);
        }

        //checks if the capacity is not full and if it is makes the take all button uninteractable
        public void CheckFullCargo() {

            int lootWeight = 0;

            lootWeight += item1.Weight * randomAmount1;
            lootWeight += item2.Weight * randomAmount2;
            lootWeight += item3.Weight * randomAmount3;
            lootWeight += item4.Weight * randomAmount4;

            CalculateCarrying();

            if (carrying + lootWeight > capacity)
            {
                takeAllButton.GetComponent<Selectable>().interactable = false;

                inventoryFullText.SetActive(true);
            }
            else
            {
                takeAllButton.GetComponent<Selectable>().interactable = true;

                inventoryFullText.SetActive(false);
            }

        }

        //Loads items again into the scene
        public void LoadItems()
        {
            GameObject itemHolder = (GameObject)Resources.Load("ItemHolder", typeof(GameObject));

            itemHolder1 = Instantiate(itemHolder, itemHolder1Pos.transform);
            itemHolder2 = Instantiate(itemHolder, itemHolder2Pos.transform);
            itemHolder3 = Instantiate(itemHolder, itemHolder3Pos.transform);
            itemHolder4 = Instantiate(itemHolder, itemHolder4Pos.transform);

            itemHolder1.GetComponent<ItemHolder>().itemNameText.text = item1.ItemName;
            itemHolder1.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder1.GetComponent<ItemHolder>().countText.text = randomAmount1.ToString();
            itemHolder1.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item1.ItemName);

            itemHolder2.GetComponent<ItemHolder>().itemNameText.text = item2.ItemName;
            itemHolder2.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder2.GetComponent<ItemHolder>().countText.text = randomAmount2.ToString();
            itemHolder2.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item2.ItemName);

            itemHolder3.GetComponent<ItemHolder>().itemNameText.text = item3.ItemName;
            itemHolder3.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder3.GetComponent<ItemHolder>().countText.text = randomAmount3.ToString();
            itemHolder3.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item3.ItemName);

            itemHolder4.GetComponent<ItemHolder>().itemNameText.text = item4.ItemName;
            itemHolder4.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder4.GetComponent<ItemHolder>().countText.text = randomAmount4.ToString();
            itemHolder4.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item4.ItemName);

            itemHolder1Pos.transform.localScale = new Vector3(0.60f, 0.75f, 1f);
            itemHolder2Pos.transform.localScale = new Vector3(0.60f, 0.75f, 1f);
            itemHolder3Pos.transform.localScale = new Vector3(0.60f, 0.75f, 1f);
            itemHolder4Pos.transform.localScale = new Vector3(0.60f, 0.75f, 1f);

            //Making Treasure Loot Interactive

            itemHolder1.GetComponent<Button>().onClick.AddListener(() =>
            {
                ItemOnClick(0);
            });

            itemHolder2.GetComponent<Button>().onClick.AddListener(() =>
            {
                ItemOnClick(1);
            });

            itemHolder3.GetComponent<Button>().onClick.AddListener(() =>
            {
                ItemOnClick(2);
            });
            
            itemHolder4.GetComponent<Button>().onClick.AddListener(() =>
            {
                ItemOnClick(3);
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
            else if(index == 1){
                itemTakeName.text = item2.ItemName;
                itemTakeIcon.sprite = Resources.Load<Sprite>(item2.ItemName);
            }
            else if (index == 2)
            {
                itemTakeName.text = item3.ItemName;
                itemTakeIcon.sprite = Resources.Load<Sprite>(item3.ItemName);
            }
            else if (index == 3)
            {
                itemTakeName.text = item4.ItemName;
                itemTakeIcon.sprite = Resources.Load<Sprite>(item4.ItemName);
            }
            itemTakeAmount.text = 0.ToString();
            SetCountSlider(index);
        }

        //sets up the count slider of the pop up to never exceed the available items
        public void SetCountSlider(int index)
        {
            if (index == 0)
                countSlider.GetComponent<Slider>().maxValue = System.Convert.ToInt32(itemHolder1.GetComponent<ItemHolder>().countText.text);
            else if(index == 1)
                countSlider.GetComponent<Slider>().maxValue = System.Convert.ToInt32(itemHolder2.GetComponent<ItemHolder>().countText.text);
            else if (index == 2)
                countSlider.GetComponent<Slider>().maxValue = System.Convert.ToInt32(itemHolder3.GetComponent<ItemHolder>().countText.text);
            else if (index == 3)
                countSlider.GetComponent<Slider>().maxValue = System.Convert.ToInt32(itemHolder4.GetComponent<ItemHolder>().countText.text);

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
            else if (selectedItem == 2)
            {
                if (item3.Weight * count + carrying > capacity)
                    takeBtn.GetComponent<Selectable>().interactable = false;
                else
                    takeBtn.GetComponent<Selectable>().interactable = true;
            }
            else if (selectedItem == 3)
            {
                if (item4.Weight * count + carrying > capacity)
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
            else if (selectedItem == 2)
            {
                PlayerInventory.AddToInventory(item3.Id, count);

                itemTakeAmount.text = "0";

                randomAmount3 -= count;
                itemHolder3.GetComponent<ItemHolder>().countText.text = (randomAmount3).ToString();

                if (itemHolder3.GetComponent<ItemHolder>().countText.text == "0")
                    Destroy(itemHolder3);
            }
            else if (selectedItem == 3)
            {
                PlayerInventory.AddToInventory(item4.Id, count);

                itemTakeAmount.text = "0";

                randomAmount4 -= count;
                itemHolder4.GetComponent<ItemHolder>().countText.text = (randomAmount4).ToString();

                if (itemHolder4.GetComponent<ItemHolder>().countText.text == "0")
                    Destroy(itemHolder4);
            }

            CalculateCarrying();

            itemTakePopUp.SetActive(false);
            if (randomAmount1 == 0 && randomAmount2 == 0 && randomAmount3 == 0 && randomAmount4 == 0)
            {
                takeAllButton.GetComponent<Selectable>().interactable = false;
            }

            PlayerInventory.SavePlayerInventory();
        }
    }
}