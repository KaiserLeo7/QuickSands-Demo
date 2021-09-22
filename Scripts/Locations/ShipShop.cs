using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    public class ShipShop : MonoBehaviour
    {
        private GameObject playerVehiclePrefab;
        private GameObject upgradeVehiclePrefab;
        private Transform instantiatedPlayerVehicle;
        private Transform instantiatedUpgradeVehicle;
        private int price;                              //price of the upgrade
        private Vehicle upgradeVehicle;                 //the next vehicle to upgrade to

        [SerializeField] private GameObject buyBtn;////////////////////////////////////////////////////////////
        [SerializeField] private GameObject upgradeBtn;                                                      //
        [SerializeField] private Transform playerVehiclePosition;                                            //
        [SerializeField] private Transform upgradeVehiclePosition;                                           //
        [SerializeField] private Text money;                                                                 //
        [SerializeField] private Text currentVehicleHp;                                                      //
        [SerializeField] private Text currentVehicleCargo;           //reference to objects in the scene     //
        [SerializeField] private Text currentVehicleName;                                                    //
        [SerializeField] private Text upgradeVehicleHp;                                                      //
        [SerializeField] private Text upgradeVehicleCargo;                                                   //
        [SerializeField] private Text upgradeVehiclePrice;                                                   //
        [SerializeField] private Text upgradeVehicleName;                                                    //
        [SerializeField] private GameObject[] currentVehicleStats = new GameObject[6];                       //
        [SerializeField] private GameObject[] upgradeVehicleStats = new GameObject[8];                       //
        [SerializeField] private GameObject[] bannerLocations;/////////////////////////////////////////////////
                   
        [SerializeField] private AudioSource buySound;

        void Start()
        {
            money.text = System.Convert.ToString(PlayerInventory.Money);

            upgradeBtn.SetActive(false);
            buyBtn.SetActive(false);
            InstantiateBanners();

            //if player has a vehicle set its stats and show the upgrade button
            if (Player.HasVehicle)
            {
                SetCurrentVehicleStats();
                upgradeBtn.SetActive(true);
                InstantiateVehicles();
                if(Player.CurrentVehicle.Name != "Leviathan")
                    SetUpgradeVehicleStats();
            }
            //if not instantiate the first vehicle and activate the buy button
            else
            {
                foreach (var item in currentVehicleStats)
                {
                    item.SetActive(false);
                }

                buyBtn.SetActive(true);
                upgradeVehiclePrefab = (GameObject)Resources.Load("Scout2Vehicle", typeof(GameObject));
                instantiatedUpgradeVehicle = Instantiate(upgradeVehiclePrefab.transform, upgradeVehiclePosition.position, Quaternion.identity);
                upgradeVehicle = new Vehicle(VehicleClassDB.getVehicle(0));
                price = VehicleClassDB.getVehicle(0).Price;
                SetUpgradeVehicleStats();
            }
        }

        
        //based on the name of player's current vehicle loads it in the scene
        public void InstantiateVehicles(){
            switch (Player.CurrentVehicle.Name)
            {
                case "Scout":
                    playerVehiclePrefab = (GameObject)Resources.Load("Scout2Vehicle", typeof(GameObject));
                    upgradeVehiclePrefab = (GameObject)Resources.Load("Warthog3Vehicle", typeof(GameObject));
                    upgradeVehicle = new Vehicle(VehicleClassDB.getVehicle(1));
                    price = VehicleClassDB.getVehicle(1).Price;
                    break;
                case "Warthog":
                    playerVehiclePrefab = (GameObject)Resources.Load("Warthog3Vehicle", typeof(GameObject));
                    upgradeVehiclePrefab = (GameObject)Resources.Load("Goliath4Vehicle", typeof(GameObject));
                    upgradeVehicle = new Vehicle(VehicleClassDB.getVehicle(2));
                    price = VehicleClassDB.getVehicle(2).Price;
                    break;
                case "Goliath":
                    playerVehiclePrefab = (GameObject)Resources.Load("Goliath4Vehicle", typeof(GameObject));
                    upgradeVehiclePrefab = (GameObject)Resources.Load("Leviathan5Vehicle", typeof(GameObject));
                    upgradeVehicle = new Vehicle(VehicleClassDB.getVehicle(3));
                    price = VehicleClassDB.getVehicle(3).Price;
                    break;
                case "Leviathan":
                    playerVehiclePrefab = (GameObject)Resources.Load("Leviathan5Vehicle", typeof(GameObject));
                    upgradeVehiclePosition.position = new Vector3(180f, 0.08f, 0f);
                    foreach (var item in upgradeVehicleStats)
                    {
                        item.SetActive(false);
                    }
                    upgradeVehiclePrefab = (GameObject)Resources.Load("Leviathan5Vehicle", typeof(GameObject));
                    upgradeBtn.SetActive(false);
                    break;
                default:
                    break;
            }
            instantiatedPlayerVehicle = Instantiate(playerVehiclePrefab.transform, playerVehiclePosition.position, Quaternion.identity);
            instantiatedUpgradeVehicle = Instantiate(upgradeVehiclePrefab.transform, upgradeVehiclePosition.position, Quaternion.identity);
        }

        public void SetUpgradeVehicleStats(){
            upgradeVehicleName.text = upgradeVehicle.Name;
            upgradeVehicleCargo.text = System.Convert.ToString(upgradeVehicle.Capacity);
            upgradeVehicleHp.text = System.Convert.ToString(upgradeVehicle.VehicleHP);
            upgradeVehiclePrice.text = System.Convert.ToString(upgradeVehicle.Price);
        }

        public void SetCurrentVehicleStats(){
            currentVehicleName.text = Player.CurrentVehicle.Name;
            currentVehicleCargo.text = System.Convert.ToString(Player.CurrentVehicle.Capacity);
            currentVehicleHp.text = System.Convert.ToString(Player.CurrentVehicle.VehicleHP);

        }

        public void upgradeBtnOnClick(){
            //player has to have enough money
            if(PlayerInventory.Money >= price)
            {
                buySound.Play();

                //deduct the money
                PlayerInventory.Money -= price;
                money.text = System.Convert.ToString(PlayerInventory.Money);
                PlayerInventory.SavePlayerInventory();
                
                //give the player their new vehicle
                if(Player.HasVehicle){
                    Destroy(instantiatedUpgradeVehicle.gameObject);
                    Destroy(instantiatedPlayerVehicle.gameObject);
                    switch (Player.CurrentVehicle.Name)
                    {
                        case "Scout":
                            Player.CurrentVehicle = new Vehicle(VehicleClassDB.getVehicle(1));
                            break;
                        case "Warthog":
                            Player.CurrentVehicle = new Vehicle(VehicleClassDB.getVehicle(2));
                            break;
                        case "Goliath":
                            Player.CurrentVehicle = new Vehicle(VehicleClassDB.getVehicle(3));
                            break;
                        default:
                            break;
                    }
                }
                else{
                    Destroy(instantiatedUpgradeVehicle.gameObject);
                    Player.HasVehicle = true;
                    Player.CurrentVehicle = new Vehicle(VehicleClassDB.getVehicle(0));
                    buyBtn.SetActive(false);
                    upgradeBtn.SetActive(true);
                    foreach (var item in currentVehicleStats)
                    {
                        item.SetActive(true);
                    }
                }
                Player.SavePlayer();

                //change the current and upgrade vehicles
                InstantiateVehicles();
                SetCurrentVehicleStats();
                SetUpgradeVehicleStats();
            }
            else{
                //no money
            }
        }

        //loads the right banners into the scene
        void InstantiateBanners()
        {
            GameObject banner = null;

            switch (Player.CurrentLocation.Territory)
            {
                case 1:
                    banner = (GameObject)Resources.Load("BannerBlue", typeof(GameObject));
                    break;
                case 3:
                    banner = (GameObject)Resources.Load("BannerRed", typeof(GameObject));
                    break;
                case 2:
                    banner = (GameObject)Resources.Load("BannerGreen", typeof(GameObject));
                    break;
                default:
                    break;
            }

            foreach (var loc in bannerLocations)
            {
                GameObject instantiatedBanner = Instantiate(banner);
                instantiatedBanner.transform.position = loc.transform.position;
                instantiatedBanner.transform.localScale = new Vector3(3f, 3f, 3f);
    
            }
        }
    }
}