using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private GameObject content;        //the place where all the items are instantiated in
        private int selectedItemIndex;
        private string selectedItem;
        private GameObject[] instantiatedItemHolder = new GameObject[1];
        private Tradeable myTradeable;                      //the chosen tradeable

        [SerializeField] GameObject itemPopUp;///////////////////////////////////////
        [SerializeField] GameObject itemTakePopUp;                                 //
        [SerializeField] Text itemName;                                            //
        [SerializeField] Text itemAmount;                                          //
        [SerializeField] Image itemIcon;           //references from the scene     //
        [SerializeField] GameObject countSlider;                                   //
        [SerializeField] GameObject emptyText;                                     //
                                                                                   //
        [SerializeField] Text fullGaugeText;                                       //
        [SerializeField] Text emptyGaugeText;////////////////////////////////////////

        [SerializeField] private GameObject Gauge;          //capacity fill
        private float capacity = 0;
        private float carrying = 0;

        //instantiates all the tradeables in the scene
        private void AddInventoryToScene()
        {
            GameObject itemHolderPrefab = (GameObject)Resources.Load("ItemHolder", typeof(GameObject));
            instantiatedItemHolder = new GameObject[PlayerInventory.TradeableInventory.Count];

            for (int i = 0; i < PlayerInventory.TradeableInventory.Count; i++)
            {
                if (PlayerInventory.TradeableInventory[i].Count > 0)
                {
                    instantiatedItemHolder[i] = Instantiate(itemHolderPrefab, content.transform);

                    //ItemHolder prefab has a component named ItemHolder which sets its properties
                    instantiatedItemHolder[i].GetComponent<ItemHolder>().itemNameText.text = PlayerInventory.TradeableInventory[i].OwnedTradeable.ItemName;
                    instantiatedItemHolder[i].GetComponent<ItemHolder>().priceText.text = "";
                    instantiatedItemHolder[i].GetComponent<ItemHolder>().countText.text = PlayerInventory.TradeableInventory[i].Count.ToString();
                    instantiatedItemHolder[i].GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(TradeableDatabase.getTradeable(i).ItemName);

                    //adding ItemOnClick to the ItemHolder
                    int index = i;
                    instantiatedItemHolder[i].GetComponent<Button>().onClick.AddListener(() =>
                    {
                        ItemOnClick(index);
                    });

                }
            }
        }

        private void DestroyAllItems()
        {
            foreach (var item in instantiatedItemHolder)
            {
                try
                {
                    Destroy(item);
                }
                catch (System.Exception)
                {
                    break;
                }
            }
        }

        public void InventoryOnClick()
        {
            fullGaugeText.gameObject.SetActive(true);
            emptyGaugeText.gameObject.SetActive(true);

            capacity = 0;

            foreach (var hero in HeroPartyDB.getHeroList())
            {
                capacity += hero.Capacity;
            }
            if (Player.HasVehicle)
                capacity += Player.CurrentVehicle.Capacity;

            calculateCarrying();

            Gauge.GetComponent<Image>().fillAmount = carrying / capacity;



            DestroyAllItems();
            AddInventoryToScene();
            ActivateEmptyText();
        }

        //if player's inventory is empty show emptyText
        private void ActivateEmptyText()
        {
            int count = 0;

            foreach (var tradeable in PlayerInventory.TradeableInventory)
            {
                count += tradeable.Count;
            }

            if (count == 0)
                emptyText.SetActive(true);
        }

        //clicking on an item activates the proper pop up and fills it in
        public void ItemOnClick(int index)
        {
            itemPopUp.SetActive(true);
            itemTakePopUp.SetActive(false);

            selectedItemIndex = index;
            selectedItem = TradeableDatabase.getTradeable(selectedItemIndex).ItemName;

            foreach (var tradeable in TradeableDatabase.getTradeableList())
            {
                if (tradeable.ItemName == selectedItem)
                {
                    myTradeable = tradeable;
                    break;
                }
            }

            SetCountSlider();

            itemName.text = myTradeable.ItemName;
            itemIcon.sprite = Resources.Load<Sprite>(myTradeable.ItemName);
        }

        //sets up the count slider of the pop up to never exceed the available items
        public void SetCountSlider()
        {
            countSlider.GetComponent<Slider>().maxValue = PlayerInventory.TradeableInventory[myTradeable.Id - 1].Count;
            countSlider.GetComponent<Slider>().value = 0;
        }

        //sets the number of the chosen tradeable after the slider changes
        public void SliderOnValueChanged()
        {
            itemAmount.text = System.Convert.ToString((int)countSlider.GetComponent<Slider>().value);
        }

        //drops an item from the inventory
        public void DropOnClick()
        {
            if((int)countSlider.GetComponent<Slider>().value > 0)
            {
                PlayerInventory.RemoveFromInventory(selectedItemIndex + 1, (int)countSlider.GetComponent<Slider>().value);
                itemPopUp.SetActive(false);
                DestroyAllItems();
                AddInventoryToScene();
                ActivateEmptyText();

                calculateCarrying();

                Gauge.GetComponent<Image>().fillAmount = carrying / capacity;
            }

           

        }

        //calculates how much the player is carrying
        public void calculateCarrying()
        {
            carrying = 0;
            foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
            {
                carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;
            }
        }
    }
}