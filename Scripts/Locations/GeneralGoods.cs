using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    public class GeneralGoods : MonoBehaviour
    {
        private int selectedItemIndex;                  //index of currenty selected item
        private string selectedItem;                    //name of currenty selected item
        [SerializeField] private GameObject buyPopUp;/////////////////////////////////////////////////
        [SerializeField] private GameObject sellPopUp;                                              //
        [SerializeField] private Text goodNameBuy;                                                  //
        [SerializeField] private Text goodNameSell;                                                 //
        [SerializeField] private Text goodPriceBuy;                                                 //
        [SerializeField] private Text goodPriceSell;                                                //
        [SerializeField] private Text goodCountBuy;                                                 //
        [SerializeField] private Text goodCountSell;                                                //
        [SerializeField] private Image goodImageBuy;      //Reference to gameobjects in the scene   //
        [SerializeField] private Image goodImageSell;                                               //
        [SerializeField] private Text townName;                                                     //
        [SerializeField] private GameObject Gauge;                                                  //
        [SerializeField] private GameObject countSlider;                                            //
        [SerializeField] private GameObject sellCountSlider;                                        //
        [SerializeField] private GameObject content;   //parent for all the itemHolders             //
        [SerializeField] GameObject emptyText;                                                      //
        [SerializeField] GameObject sellAllButton;                                                  //
        [SerializeField] Text sellAllEarningText;/////////////////////////////////////////////////////
                                                                
        private Location playerLocation;                //Player's current location
        private float capacity = 0;                     //player's party capacity
        private float carrying = 0;                     //player's carrying amount

        [SerializeField] private Text money;            //money in the scene
        [SerializeField] private Text errorTextBuy;     //used to show messages to the player
        [SerializeField] private Text errorTextSell;    //used to show messages to the player

        private Tradeable myTradeable;                  //the chosen tradeable

        private GameObject[] instantiatedItemHolder;    //stores the item holders

        private bool isBuy = true;                      //if true player is in the shop if false they are in inventory

        [SerializeField] private AudioSource buySound;

        public void Start(){
            //set the fisrt item to cloth
            selectedItem = "Cloth";
            Player.LoadPlayer();
            money.text = System.Convert.ToString(PlayerInventory.Money);

            //get player's location from the database
            foreach (var item in LocationDB.getLocationList())
            {
                if(item.LocationName == Player.CurrentLocation.LocationName){
                    playerLocation = item;
                }
            }

            townName.text = playerLocation.LocationName + " Trade Goods";

            goodNameBuy.text = selectedItem;
            goodCountBuy.text = "0";
            goodPriceBuy.text = "0";

            //calculate the capacity
            foreach (var hero in HeroPartyDB.getHeroList())
            {
                capacity += hero.Capacity;
            }

            if(Player.HasVehicle)
                capacity += Player.CurrentVehicle.Capacity;

            calculateCarrying();

            //setup the gauge
            Gauge.GetComponent<Image>().fillAmount = carrying / capacity;

            //set the chosen tradeable to be cloth
            myTradeable = TradeableDatabase.getTradeable(0);

            setCountSlider();
            AddItemsToScene();
        }

        //clicking on an item activates the proper pop up and fills it in
        public void ItemOnClick(int index){

            selectedItemIndex = index;
            selectedItem = TradeableDatabase.getTradeable(selectedItemIndex).ItemName;

            foreach (var tradeable in TradeableDatabase.getTradeableList())
            {
                if (tradeable.ItemName == selectedItem)
                {
                    myTradeable = tradeable;
                }
            }

            if (isBuy)
            {
                buyPopUp.SetActive(true);
                goodNameBuy.text = selectedItem;
                goodCountBuy.text = "0";
                goodPriceBuy.text = "0";
                goodImageBuy.sprite = Resources.Load<Sprite>(selectedItem);
                setCountSlider();
            }
            else
            {
                sellPopUp.SetActive(true);
                goodNameSell.text = selectedItem;
                goodCountSell.text = "0";
                goodPriceSell.text = "0";
                goodImageSell.sprite = Resources.Load<Sprite>(selectedItem);
                setSellCountSlider();
            }

            errorTextBuy.text = "";
        }

        //sets up the count slider of the buy pop up to never exceed the available capacity
        public void setCountSlider(){
            int maxValue = ((int)capacity - (int)carrying) / myTradeable.Weight;
            countSlider.GetComponent<Slider>().maxValue = maxValue;
            countSlider.GetComponent<Slider>().value = 0;
        }

        //sets up the count slider of the sell pop up to be the number of items player has
        public void setSellCountSlider(){
            sellCountSlider.GetComponent<Slider>().maxValue = PlayerInventory.TradeableInventory[myTradeable.Id - 1].Count;
            sellCountSlider.GetComponent<Slider>().value = 0;

            if(PlayerInventory.TradeableInventory[myTradeable.Id - 1].Count == 0)
                errorTextSell.text = "No Loot";
            else
                errorTextSell.text = "";
        }

        //calculates how much the player is carrying
        public void calculateCarrying(){
            carrying = 0;
            foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
            {
                carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;
            }
        }
        
        //brings up the shop items
        public void ShopOnClick()
        {
            isBuy = true;

            sellPopUp.SetActive(false);

            sellAllButton.SetActive(false);

            DestroyAllItems();
            AddItemsToScene();
        }

        //brings up the inventory items
        public void InventoryOnClick()
        {
            isBuy = false;

            buyPopUp.SetActive(false);

            sellAllButton.SetActive(true);

            DestroyAllItems();
            AddInventoryToScene();
        }

        //sets the number and price of the chosen tradeable after the slider changes
        public void sliderOnValueChanged()
        {
            goodCountBuy.text = System.Convert.ToString((int)countSlider.GetComponent<Slider>().value);
            goodPriceBuy.text = System.Convert.ToString(playerLocation.TradePrices[myTradeable.Id - 1] * (int)countSlider.GetComponent<Slider>().value);
        }

        //sets the number and price of the chosen tradeable after the slider changes
        public void sellsliderOnValueChanged()
        {
            goodCountSell.text = System.Convert.ToString((int)sellCountSlider.GetComponent<Slider>().value);
            goodPriceSell.text = System.Convert.ToString((int)(playerLocation.TradePrices[myTradeable.Id - 1] - playerLocation.TradePrices[myTradeable.Id - 1] * 15 / 100) * (int)sellCountSlider.GetComponent<Slider>().value);
        }

        public void confirmSellOnClick(){
            sellPopUp.SetActive(false);

            //checks if the value of the slider is 0 or not
            if((int)sellCountSlider.GetComponent<Slider>().value != 0){
                buySound.Play();

                //removes the tradeable from player's inventory
                PlayerInventory.RemoveFromInventory(myTradeable.Id, (int)(sellCountSlider.GetComponent<Slider>().value));

                //adds the money
                PlayerInventory.Money += (int)(playerLocation.TradePrices[myTradeable.Id - 1] - playerLocation.TradePrices[myTradeable.Id - 1] * 15 / 100) * (int)sellCountSlider.GetComponent<Slider>().value;
                PlayerInventory.SavePlayerInventory();

                goodCountSell.text = "0";
                goodPriceSell.text = "0";

                //sets the count of the item holder
                instantiatedItemHolder[myTradeable.Id - 1].GetComponent<ItemHolder>().countText.text = ((int)(PlayerInventory.TradeableInventory[myTradeable.Id - 1].Count)).ToString();

                calculateCarrying();
                
                setSellCountSlider();
                money.text = System.Convert.ToString(PlayerInventory.Money);
                Gauge.GetComponent<Image>().fillAmount = carrying / capacity;

                DestroyAllItems();
                AddInventoryToScene();
                ActivateEmptyText();
            }
        }

        public void confirmBuyOnClick(){

            //checks if the value of the slider is 0 or not
            if ((int)countSlider.GetComponent<Slider>().value != 0){
                //checks if the player has enough money
                if(PlayerInventory.Money >= playerLocation.TradePrices[myTradeable.Id - 1] * (int)countSlider.GetComponent<Slider>().value){
                    buySound.Play();

                    buyPopUp.SetActive(false);

                    //adds the items to inventory and deducts the money
                    PlayerInventory.AddToInventory(myTradeable.Id, (int)(countSlider.GetComponent<Slider>().value));
                    PlayerInventory.Money -= playerLocation.TradePrices[myTradeable.Id - 1] * (int)countSlider.GetComponent<Slider>().value;
                    PlayerInventory.SavePlayerInventory();

                    goodCountBuy.text = "0";
                    goodPriceBuy.text = "0";

                    //sets the count of the item holder
                    instantiatedItemHolder[myTradeable.Id - 1].GetComponent<ItemHolder>().countText.text = ((int)(PlayerInventory.TradeableInventory[myTradeable.Id - 1].Count)).ToString();

                    calculateCarrying();
                    
                    setCountSlider();
                    money.text = System.Convert.ToString(PlayerInventory.Money);
                    Gauge.GetComponent<Image>().fillAmount = carrying / capacity;
                }
                else{
                    errorTextBuy.text = "No Coin";
                }
            }
        }

        //shows how much money player makes if they sell all their items
        public void SellAllOnClick()
        {
            int coin = 0;
            foreach (var tradeable in PlayerInventory.TradeableInventory)
            {
                if (tradeable.Count > 0)
                {
                    coin += (int)(playerLocation.TradePrices[tradeable.OwnedTradeable.Id - 1] - playerLocation.TradePrices[myTradeable.Id - 1] * 15 / 100) * tradeable.Count;
                }
            }

            sellAllEarningText.text = coin.ToString() + " C";
        }

        //sells all the items in the inventory
        public void SellAllYesOnClick()
        {
            buySound.Play();

            int coin = 0;
            foreach (var tradeable in PlayerInventory.TradeableInventory)
            {
                if(tradeable.Count > 0)
                {
                    coin += (int)(playerLocation.TradePrices[tradeable.OwnedTradeable.Id - 1] - playerLocation.TradePrices[myTradeable.Id - 1] * 15 / 100) * tradeable.Count;
                }

                tradeable.Count = 0;
            }

            PlayerInventory.Money += coin;
            PlayerInventory.SavePlayerInventory();

            money.text = System.Convert.ToString(PlayerInventory.Money);
            calculateCarrying();
            Gauge.GetComponent<Image>().fillAmount = carrying / capacity;

            DestroyAllItems();
            AddInventoryToScene();
            ActivateEmptyText();
        }

        //instantiates all inventory tradeables in the scene
        private void AddItemsToScene()
        {
            GameObject itemHolderPrefab = (GameObject)Resources.Load("ItemHolder", typeof(GameObject));
            instantiatedItemHolder = new GameObject[TradeableDatabase.getTradeableList().Count];
            for (int i = 0; i < TradeableDatabase.getTradeableList().Count; i++)
            {
                instantiatedItemHolder[i] = Instantiate(itemHolderPrefab, content.transform);

                //ItemHolder prefab has a component named ItemHolder which sets its properties
                instantiatedItemHolder[i].GetComponent<ItemHolder>().itemNameText.text = TradeableDatabase.getTradeable(i).ItemName;
                instantiatedItemHolder[i].GetComponent<ItemHolder>().priceText.text = playerLocation.TradePrices[i] + " Coin";
                instantiatedItemHolder[i].GetComponent<ItemHolder>().countText.text = PlayerInventory.TradeableInventory[i].Count.ToString();
                instantiatedItemHolder[i].GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(TradeableDatabase.getTradeable(i).ItemName);

                //adding ItemOnClick to the ItemHolder
                int index = i;
                instantiatedItemHolder[i].GetComponent<Button>().onClick.AddListener(() =>
                {
                    ItemOnClick(index);
                });
            }

            emptyText.SetActive(false);
        }

        //instantiates all shop tradeables in the scene
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
                    instantiatedItemHolder[i].GetComponent<ItemHolder>().priceText.text = (int)(playerLocation.TradePrices[i] - playerLocation.TradePrices[i] * 15/100) + " Coin";
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

            ActivateEmptyText();
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

        //if player's inventory is empty show emptyText
        private void ActivateEmptyText()
        {
            int count = 0;

            foreach (var tradeable in PlayerInventory.TradeableInventory)
            {
                count += tradeable.Count;
            }

            if (count == 0)
            {
                emptyText.SetActive(true);
                sellAllButton.SetActive(false);
            }
        }
    }
}