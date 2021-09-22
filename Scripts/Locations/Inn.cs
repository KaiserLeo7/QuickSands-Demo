using UnityEngine;
using UnityEngine.UI;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    public class Inn : MonoBehaviour
    {
        [SerializeField] private Transform[] heroHS = new Transform[3];         //position of each hero
        [SerializeField] private GameObject[] hireButtons = new GameObject[3];  //hero hire buttons
        [SerializeField] private GameObject hirePopUp;
        private GameObject[] heroPrefabs = new GameObject[3];
        private Transform[] heroTransforms = new Transform[3];
        private int[] price = new int[3];                                       //price of each hero
        [SerializeField] private Text heroLevel;
        [SerializeField] private Text heroPrice;
        [SerializeField] private Text errorText;
        [SerializeField] private Text money;
        [SerializeField] private Text partySize;
        [SerializeField] private Text maxPartySize;
        private int selectedHeroIndex;
        [SerializeField] private GameObject[] bannerLocations;
        private static bool[] hired = new bool[3];                              //checks if each hero is hired previously
        private static Location savedLocation;                                  //remembers the players previous location
        [SerializeField] private AudioSource hireSound;

        void Start()
        {
            PlayerInventory.LoadPlayerInventory();
            money.text = System.Convert.ToString(PlayerInventory.Money);

            //set the party size in the scene
            partySize.text = System.Convert.ToString(HeroPartyDB.getHeroList().Count);

            //set the maximum party size
            if(Player.HasVehicle)
                maxPartySize.text = System.Convert.ToString(Player.CurrentVehicle.PartySize);
            else
                maxPartySize.text = "1";

            hirePopUp.SetActive(false);

            foreach (var button in hireButtons)
            {
                button.SetActive(false);
            }

            CheckLocation();

            InstatiateHeroes();
            InstantiateBanners();
        }
        
        void CheckLocation()
        {
            try
            {
                if(savedLocation.LocationName != Player.CurrentLocation.LocationName)
                {
                    for (int i = 0; i < hired.Length; i++)
                    {
                        hired[i] = false;
                    }

                    savedLocation = Player.CurrentLocation;
                }
            }
            catch (System.Exception)
            {
                savedLocation = Player.CurrentLocation;
            }
        }

        //instantiates up to 3 heroes for the Inn
        public void InstatiateHeroes(){
            for (int i = 0; i < InnHeroes.InnHeroesList.Count; i++)
            {
                heroPrefabs[i] = (GameObject)Resources.Load(InnHeroes.InnHeroesList[i].GetType().Name, typeof(GameObject));
                InnHeroes.InnHeroesList[i].setSkin(heroPrefabs[i]);
                int skinTire = InnHeroes.InnHeroesList[i].SkinTire;
                switch (skinTire)
                {
                    case 1:
                        price[i] = 1000;
                        break;
                    case 2:
                        price[i] = 2500;
                        break;
                    case 3:
                        price[i] = 4000;
                        break;
                    default:
                    break;
                }
                heroTransforms[i] = Instantiate(heroPrefabs[i].transform, heroHS[i].position, Quaternion.identity);
                hireButtons[i].SetActive(true);
            }
        }

        //sets the values of hirePopUp
        public void HeroOnClick(){
            selectedHeroIndex = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name) - 1;
            if (!hired[selectedHeroIndex])
            {
                hirePopUp.SetActive(true);
                heroLevel.text = System.Convert.ToString(InnHeroes.InnHeroesList[selectedHeroIndex].SkinTire);
                heroPrice.text = System.Convert.ToString(price[selectedHeroIndex]);
            }
        }

        public void popUpCloseOnClick(){
            hirePopUp.SetActive(false);
            errorText.text = "";
        }

        public void popUpYesOnClick(){
            //checks if everything is ready to hire a new hero
            if(Player.HasVehicle && HeroPartyDB.getHeroList().Count < 5 && Player.CurrentVehicle.PartySize > HeroPartyDB.getHeroList().Count){
                PlayerInventory.LoadPlayerInventory();
                if(PlayerInventory.Money >= price[selectedHeroIndex]){
                    hireSound.Play();

                    //deducts the money from the player
                    PlayerInventory.Money -= price[selectedHeroIndex];
                    PlayerInventory.SavePlayerInventory();

                    //adds the hero to the party
                    switch (InnHeroes.InnHeroesList[selectedHeroIndex].GetType().Name)
                    {
                        case "Warrior":
                            HeroPartyDB.addHero(new Warrior((Warrior)InnHeroes.InnHeroesList[selectedHeroIndex]));
                            break;

                        case "Mage":
                            HeroPartyDB.addHero(new Mage((Mage)InnHeroes.InnHeroesList[selectedHeroIndex]));
                            break;

                        case "Ranger":
                            HeroPartyDB.addHero(new Ranger((Ranger)InnHeroes.InnHeroesList[selectedHeroIndex]));
                            break;

                        case "Spearman":
                            HeroPartyDB.addHero(new Spearman((Spearman)InnHeroes.InnHeroesList[selectedHeroIndex]));
                            break;

                        case "Wizard":
                            HeroPartyDB.addHero(new Wizard((Wizard)InnHeroes.InnHeroesList[selectedHeroIndex]));
                            break;

                        default:
                            break;
                    }

                    HeroPartyDB.SaveParty();

                    hired[selectedHeroIndex] = true;

                    money.text = System.Convert.ToString(PlayerInventory.Money);
                    partySize.text = System.Convert.ToString(HeroPartyDB.getHeroList().Count);
                    
                    hirePopUp.SetActive(false);
                    hireButtons[selectedHeroIndex].SetActive(false);
                }
                else{
                    errorText.text = "Not Enough Gold";
                }
            }
            else if(HeroPartyDB.getHeroList().Count == 5){
                errorText.text = "Maximum number of Heroes";
            }
            else if(!Player.HasVehicle){
                errorText.text = "You do not own a Vehicle";
            }
            else{
                errorText.text = "Vehicle is Full";
            }
            
        }

        //based on the location of the palyer sets the banners up
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
            }
        }
    }
}