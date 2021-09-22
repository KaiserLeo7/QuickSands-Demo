using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

//COMMENTED BY FARAMARZ HOSSEINI

namespace Sands
{
    public class BlackSmith : MonoBehaviour
    {
        private GameObject[] heroPrefabs = new GameObject[5];                   //hero prefabs are stored here
        private GameObject[] upgradePrefabs = new GameObject[5];                //upgraded prefabs are stored here
        private Transform[] instantiatedHeroes = new Transform[5];              //instantiated heroes are stored here
        [SerializeField] private Transform[] heroStations = new Transform[5];   //positions of every hero is stored here
        [SerializeField] private Transform currentHeroStation;                  //the position of clicked hero
        [SerializeField] private Transform upgradeHeroStation;                  //the position of upgraded version of clicked hero
        [SerializeField] private GameObject[] heroButtons = new GameObject[5];  //all the hero buttons are stored here
        private Transform instantiatedCurrentHero;                              //the instantiated clicked hero
        private Transform instantiatedUpgradeHero;                              //the instantiated upgraded clicked hero
        private int selectedIndex;                                              //selected index of hero
        private int price;                                                      //the price of the upgrtade
        
        [SerializeField] private Text money;                                    //reference to the money in the scene
        [SerializeField] private Text currentHeroHP;                            //reference to the clicked hero's health text
        [SerializeField] private Text currentHeroAttack;                        //reference to the clicked hero's damage text
        [SerializeField] private Text upgradeHeroHP;
        [SerializeField] private Text upgradeHeroAttack;
        [SerializeField] private Text upgradeHeroPrice;
        [SerializeField] private GameObject[] upgradeStats = new GameObject[7]; //reference to all the stats
        [SerializeField] private Text errorText;                                //to show if player has no money or has max level
        [SerializeField] private AudioSource upgradeSound;                      //upgrading sound

        void Start()
        {
            //deactivate all the buttons
            foreach (var item in heroButtons)
            {
                item.SetActive(false);
            }

            InstantiateHeroes();

            //set money in the scene
            money.text = System.Convert.ToString(PlayerInventory.Money);
        }

        //instantiates all the heroes the player has in the party
        public void InstantiateHeroes()
        {
            for (int i = 0; i < heroPrefabs.Length && i < HeroPartyDB.getHeroList().Count; i++)
            {
                heroButtons[i].SetActive(true);
                heroPrefabs[i] = (GameObject)Resources.Load(HeroPartyDB.getHero(i).GetType().Name, typeof(GameObject));
                upgradePrefabs[i] = (GameObject)Resources.Load(HeroPartyDB.getHero(i).GetType().Name, typeof(GameObject));
                HeroPartyDB.getHero(i).setSkin(heroPrefabs[i]);
                instantiatedHeroes[i] = Instantiate(heroPrefabs[i].transform, heroStations[i].position, Quaternion.identity);
                instantiatedHeroes[i].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
            InstantiateCurrentHero(0);
            InstantiateUpgradeHero(0);
            SetUpgradeHeroStats(0);
            SetCurrentHeroStats(0);
        }

        //Destroys all the heroes and reinstantiates them
        public void ReInstantiateHeroes()
        {
            for (int i = 0; i < heroPrefabs.Length && i < HeroPartyDB.getHeroList().Count; i++)
            {
                Destroy(instantiatedHeroes[i].gameObject);
            }

            for (int i = 0; i < heroPrefabs.Length && i < HeroPartyDB.getHeroList().Count; i++)
            {
                heroButtons[i].SetActive(true);
                heroPrefabs[i] = (GameObject)Resources.Load(HeroPartyDB.getHero(i).GetType().Name, typeof(GameObject));
                upgradePrefabs[i] = (GameObject)Resources.Load(HeroPartyDB.getHero(i).GetType().Name, typeof(GameObject));
                HeroPartyDB.getHero(i).setSkin(heroPrefabs[i]);
                instantiatedHeroes[i] = Instantiate(heroPrefabs[i].transform, heroStations[i].position, Quaternion.identity);
                instantiatedHeroes[i].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
           
        }


        //clicking on a hero calls this function
        public void HeroOnClick()
        {
            errorText.text = "";

            //destroys previous instantiated hero and upgraded hero if possible
            Destroy(instantiatedCurrentHero.gameObject);
            if (instantiatedUpgradeHero != null)
                Destroy(instantiatedUpgradeHero.gameObject);

            //sets the selected index to the name of the button which is a number
            selectedIndex = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name) - 1;

            //sets the skin of the hero
            HeroPartyDB.getHero(selectedIndex).setSkin(heroPrefabs[selectedIndex]);

            //instantiates the two current and upgraded heroes
            InstantiateCurrentHero(selectedIndex);
            InstantiateUpgradeHero(selectedIndex);
            //sets the stats
            SetUpgradeHeroStats(selectedIndex);
            SetCurrentHeroStats(selectedIndex);
        }

        //instantiates the clicked hero based on the index passed to it
        public void InstantiateCurrentHero(int index)
        {
            HeroPartyDB.getHero(index).setSkin(heroPrefabs[index]);
            instantiatedCurrentHero = Instantiate(heroPrefabs[index].transform, currentHeroStation.position, Quaternion.identity);
            instantiatedCurrentHero.transform.localScale = new Vector3(2, 2, 2);
        }

        //instantiates the upgraded version of clicked hero based on the index passed to it
        public void InstantiateUpgradeHero(int index)
        {
            bool instantiate = true;  //checks if it is the last skin the hero is wearing

            var skeletonMecanim = upgradePrefabs[index].GetComponent<SkeletonMecanim>();

            //if last skin don't instantiate
            if(skeletonMecanim.initialSkinName == "p6")
                instantiate = false;

            //based on the skin set the upgraded skin and price
            switch (skeletonMecanim.initialSkinName)
            {
                case "p2":
                    skeletonMecanim.initialSkinName = "p3";
                    price = 1500;
                    break;
                case "p3":
                    skeletonMecanim.initialSkinName = "p4";
                    price = 3000;
                    break;
                case "p4":
                    skeletonMecanim.initialSkinName = "p5";
                    price = 5000;
                    break;
                case "p5":
                    skeletonMecanim.initialSkinName = "p6";
                    price = 7000;
                    break;
                case "p6":
                    skeletonMecanim.initialSkinName = "p6";
                    break;
                default:
                    break;
            }

            //set the stats to active and instantiate the upgraded hero
            if (instantiate)
            {
                foreach (var item in upgradeStats)
                {
                    item.SetActive(true);
                }

                instantiatedUpgradeHero = Instantiate(upgradePrefabs[index].transform, upgradeHeroStation.position, Quaternion.identity);
                instantiatedUpgradeHero.transform.localScale = new Vector3(2, 2, 2);
            }
            //deactivate the stats
            else{
                foreach (var item in upgradeStats)
                {
                    item.SetActive(false);
                }
            }
        }

        //based on the type of the clicked hero sets its health and damage from the database
        public void SetCurrentHeroStats(int index)
        {
            int health = 0;
            int damage = 0;
            if (HeroPartyDB.getHero(index).SkinTire < 5)
            {
                if (instantiatedCurrentHero.gameObject.name == "Warrior(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire - 1).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire -1 ).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedCurrentHero.gameObject.name == "Mage(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire + 4).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire + 4).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedCurrentHero.gameObject.name == "Ranger(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire + 9).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire + 9).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedCurrentHero.gameObject.name == "Spearman(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire + 14).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire + 14).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedCurrentHero.gameObject.name == "Wizard(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire + 19).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire + 19).Damage + HeroPartyDB.getHero(index).Damage;
                }
            }
            else{
                if (instantiatedCurrentHero.gameObject.name == "Warrior(Clone)")
                {
                    health += ArmorDatabase.getArmor(4).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(4).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedCurrentHero.gameObject.name == "Mage(Clone)")
                {
                    health += ArmorDatabase.getArmor(9).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(9).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedCurrentHero.gameObject.name == "Ranger(Clone)")
                {
                    health += ArmorDatabase.getArmor(14).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(14).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedCurrentHero.gameObject.name == "Spearman(Clone)")
                {
                    health += ArmorDatabase.getArmor(19).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(19).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedCurrentHero.gameObject.name == "Wizard(Clone)")
                {
                    health += ArmorDatabase.getArmor(24).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(24).Damage + HeroPartyDB.getHero(index).Damage;
                }
            }

            currentHeroHP.text = System.Convert.ToString(health);
            currentHeroAttack.text = System.Convert.ToString(damage);
        }

        //based on the type of the clicked hero sets its upgraded health and damage
        public void SetUpgradeHeroStats(int index)
        {
            int health = 0;
            int damage = 0;
            if (HeroPartyDB.getHero(index).SkinTire < 5 && instantiatedUpgradeHero != null)
            {
                if (instantiatedUpgradeHero.gameObject.name == "Warrior(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedUpgradeHero.gameObject.name == "Mage(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire + 5).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire + 5).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedUpgradeHero.gameObject.name == "Ranger(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire + 10).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire + 10).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedUpgradeHero.gameObject.name == "Spearman(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire + 15).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire + 15).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedUpgradeHero.gameObject.name == "Wizard(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire + 20).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire + 20).Damage + HeroPartyDB.getHero(index).Damage;
                }
            }
            upgradeHeroHP.text = System.Convert.ToString(health);

            upgradeHeroAttack.text = System.Convert.ToString(damage);

            upgradeHeroPrice.text = System.Convert.ToString(price);
        }

        //clicking upgarde in the scene calls this function
        public void upgradeBtnOnClick()
        {
            //checks if the player has enough money and the hero is not maxed out
            if (PlayerInventory.Money >= price && HeroPartyDB.getHero(selectedIndex).SkinTire < 5)
            {
                //deducts the money
                PlayerInventory.Money -= price;
                money.text = System.Convert.ToString(PlayerInventory.Money);
                PlayerInventory.SavePlayerInventory();

                //destroys the heroes
                Destroy(instantiatedCurrentHero.gameObject);
                if (instantiatedUpgradeHero != null) 
                Destroy(instantiatedUpgradeHero.gameObject);

                //adds a level to the hero and instantiates them
                HeroPartyDB.getHero(selectedIndex).SkinTire++;
                HeroPartyDB.getHero(selectedIndex).setSkin(heroPrefabs[selectedIndex]);
                InstantiateCurrentHero(selectedIndex);
                InstantiateUpgradeHero(selectedIndex);
                ReInstantiateHeroes();
                //saves the party
                HeroPartyDB.SaveParty();

                //sets the new stats
                SetCurrentHeroStats(selectedIndex);
                SetUpgradeHeroStats(selectedIndex);

                upgradeSound.Play();
            }

            else if(HeroPartyDB.getHero(selectedIndex).SkinTire == 5){
                errorText.text = "Already Maxed Out";
            }
            else{
                errorText.text = "Not Enough Gold";
            }
        }

    }
}