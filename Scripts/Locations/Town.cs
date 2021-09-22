using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    public class Town : MonoBehaviour
    {
        [SerializeField] private Text locationName;
        [SerializeField] private Text Money;
        [SerializeField] private GameObject[] bannerLocations;
        [SerializeField] private Transform vehicleBS;           //location of the vehicle in the scene

        void Start()
        {
            Player.LoadPlayer();
            locationName.text = Player.CurrentLocation.LocationName;
            Money.text = System.Convert.ToString(PlayerInventory.Money);
            InstantiateBanners();
            InstantiatePlayerVehicle();
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
                instantiatedBanner.transform.localScale = new Vector3(2f, 2f, 2f);
            }
        }

        //instantiates the player's vehicle into the scene
        void InstantiatePlayerVehicle()
        {
            GameObject vehiclePrefab;
            GameObject instantiatedVehicle = null;

            if (Player.HasVehicle)
            {
                switch (Player.CurrentVehicle.Name)
                {
                    case "Scout":
                        vehiclePrefab = (GameObject)Resources.Load("Scout2Vehicle", typeof(GameObject));
                        instantiatedVehicle = Instantiate(vehiclePrefab, vehicleBS.position, Quaternion.Euler(0, 180, 0));

                        break;

                    case "Warthog":
                        vehiclePrefab = (GameObject)Resources.Load("Warthog3Vehicle", typeof(GameObject));
                        instantiatedVehicle = Instantiate(vehiclePrefab, vehicleBS.position, Quaternion.Euler(0, 180, 0));

                        break;

                    case "Goliath":
                        vehiclePrefab = (GameObject)Resources.Load("Goliath4Vehicle", typeof(GameObject));
                        instantiatedVehicle = Instantiate(vehiclePrefab, vehicleBS.position, Quaternion.Euler(0, 180, 0));

                        break;

                    case "Leviathan":
                        vehiclePrefab = (GameObject)Resources.Load("Leviathan5Vehicle", typeof(GameObject));
                        instantiatedVehicle = Instantiate(vehiclePrefab, vehicleBS.position, Quaternion.Euler(0, 180, 0));

                        break;
                    default:
                        break;
                }

                instantiatedVehicle.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                instantiatedVehicle.GetComponent<SkeletonAnimation>().AnimationName = "";
            }
        }

        //adding money for the demo
        public void AddMoney()
        {
            PlayerInventory.LoadPlayerInventory();
            PlayerInventory.Money += 100000;
            Money.text = System.Convert.ToString(PlayerInventory.Money);
            PlayerInventory.SavePlayerInventory();
        }
    }
}