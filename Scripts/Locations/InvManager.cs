
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

//COMMENTED BY FARAMARZ HOSSEINI

namespace Sands
{
    public class InvManager : MonoBehaviour
    {
        private GameObject[] heroPrefabs = new GameObject[5];
        private Transform instantiatedCurrentHero;
        private Vehicle playerVehicle;
        private Transform[] instantiatedHeroes = new Transform[5];
        private GameObject heroVehiclePrefab;
        private Transform instantiatedHeroVehicle;

        [SerializeField] private Transform HeroVehicleBS;////////////////////////////////////////////
        [SerializeField] private Transform[] heroStations = new Transform[5];                      //
        [SerializeField] private GameObject[] heroButtons = new GameObject[5];                     //
        [SerializeField] private Transform currentHeroStation;     //position of current hero      //
        [SerializeField] private Text currentHeroHP;                                               //
        [SerializeField] private Text currentHeroAttack;   //reference to the objects in the scene //
        [SerializeField] private GameObject fireButton;                                            //
        [SerializeField] private GameObject firePopUp;                                             //
        [SerializeField] private Text vehicleName;                                                 //
        [SerializeField] private Text heroLevel;                                                   //
        [SerializeField] private Text partyHealth;                                                 //
        [SerializeField] private Text partyDamage;///////////////////////////////////////////////////

        private int selectedIndex;           //index of the selected hero
        private int pHealth = 0;             //sum of party's health
        private int pDamage = 0;             //sum of party's damage
        int health = 0;                      //health of individual heroes
        int damage = 0;                      //damage of individual heroes


        //Inventory
        
        [SerializeField] Text fullGaugeText;
        [SerializeField] Text emptyGaugeText;

        [SerializeField] GameObject invCanvas;
        [SerializeField] GameObject questCanvas;

        void Start()
        {
            //necessary text control on start of scene
            fullGaugeText.gameObject.SetActive(false);
            emptyGaugeText.gameObject.SetActive(false);

            invCanvas.gameObject.SetActive(false);
            questCanvas.gameObject.SetActive(false);



            if (Player.HasVehicle)
                vehicleName.text = Player.CurrentVehicle.Name;
            else
                vehicleName.gameObject.SetActive(false);

            foreach (var button in heroButtons)
            {
                button.SetActive(false);
            }

            //if player has a vehicle load it in
            if (SaveSystem.Pdata.HasVehicle)
            {

                Player.LoadPlayer();
                playerVehicle = Player.CurrentVehicle;

                switch (playerVehicle.Name)
                {
                    case "Scout":
                        heroVehiclePrefab = (GameObject)Resources.Load("Scout2Vehicle", typeof(GameObject));
                        instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                        instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                        instantiatedHeroVehicle.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                        heroStations[0].transform.position = new Vector3(3.75f, 9.4f, 0f);
                        heroStations[1].transform.position = new Vector3(-1.53f, 9.4f, 0f);

                        heroButtons[0].transform.position = new Vector3(3.75f, 12.4f, 0f);
                        heroButtons[1].transform.position = new Vector3(-1.53f, 12.4f, 0f);



                        break;
                    case "Warthog":
                        heroVehiclePrefab = (GameObject)Resources.Load("Warthog3Vehicle", typeof(GameObject));
                        instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                        instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                        instantiatedHeroVehicle.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                        heroStations[1].transform.position = new Vector3(1.89f, 9.59f, 0f);
                        heroStations[2].transform.position = new Vector3(-3.5f, 9.59f, 0f);
                        heroStations[0].transform.position = new Vector3(8.08f, 12.81f, 0f);

                        heroButtons[1].transform.position = new Vector3(1.89f, 12.59f, 0f);
                        heroButtons[2].transform.position = new Vector3(-3.5f, 12.59f, 0f);
                        heroButtons[0].transform.position = new Vector3(8.08f, 15.81f, 0f);
                        break;
                    case "Goliath":
                        heroVehiclePrefab = (GameObject)Resources.Load("Goliath4Vehicle", typeof(GameObject));
                        instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                        instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                        instantiatedHeroVehicle.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                        heroStations[0].transform.position = new Vector3(8.5f, 12.42f, 0f);
                        heroStations[1].transform.position = new Vector3(3.83f, 12.42f, 0f);
                        heroStations[2].transform.position = new Vector3(-0.5f, 14.07f, 0f);
                        heroStations[3].transform.position = new Vector3(-4.890064f, 14.07f, 0f);

                        heroButtons[0].transform.position = new Vector3(8.6f, 15.42f, 0f);
                        heroButtons[1].transform.position = new Vector3(3.83f, 15.42f, 0f);
                        heroButtons[2].transform.position = new Vector3(-0.5f, 17.07f, 0f);
                        heroButtons[3].transform.position = new Vector3(-4.890064f, 17.07f, 0f);
                        break;
                    case "Leviathan":
                        heroVehiclePrefab = (GameObject)Resources.Load("Leviathan5Vehicle", typeof(GameObject));
                        instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                        instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                        instantiatedHeroVehicle.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                        heroStations[0].transform.position = new Vector3(2.5f, 13.24f, 0f);
                        heroStations[1].transform.position = new Vector3(7.08f, 9.03f, 0f);
                        heroStations[2].transform.position = new Vector3(12.46f, 9.03f, 0f);
                        heroStations[3].transform.position = new Vector3(-1.2f, 13.24f, 0f);
                        heroStations[4].transform.position = new Vector3(-5.6f, 13.24f, 0f);

                        heroButtons[0].transform.position = new Vector3(2.5f, 16.24f, 0f);
                        heroButtons[1].transform.position = new Vector3(7.08f, 12.3f, 0f);
                        heroButtons[2].transform.position = new Vector3(12.46f, 12.3f, 0f);
                        heroButtons[3].transform.position = new Vector3(-1.6f, 16.24f, 0f);
                        heroButtons[4].transform.position = new Vector3(-5.6f, 16.24f, 0f);
                        break;

                    default:
                        break;
                }

                for (int i = 0; i < HeroPartyDB.getHeroList().Count; i++)
                {
                    heroButtons[i].SetActive(true);
                }

                //if there is more than one hero make fire button interactable
                if (HeroPartyDB.getHeroList().Count == 1)
                    fireButton.GetComponent<Selectable>().interactable = false;

                InstantiateHeroes();
                InstantiateCurrentHero(0);
                SetCurrentHeroStats(0);
            }
            else {

                fireButton.GetComponent<Selectable>().interactable = false;

                heroStations[0].transform.position = new Vector3(5f, 4.76f, 0f);
                heroButtons[0].transform.position = new Vector3(5f, 5.76f, 0f);
                heroPrefabs = new GameObject[1];


                heroPrefabs[0] = (GameObject)Resources.Load(HeroPartyDB.getHero(0).GetType().Name, typeof(GameObject));
                HeroPartyDB.getHero(0).setSkin(heroPrefabs[0]);
                instantiatedHeroes = new Transform[1];

                instantiatedHeroes[0] = Instantiate(heroPrefabs[0].transform, heroStations[0].position, Quaternion.identity);
                instantiatedHeroes[0].transform.localScale = new Vector3(5.0f, 5.0f, 0.0f);

                InstantiateCurrentHero(0);
                SetCurrentHeroStats(0);
            }

            partyStats();
        }

        //Hero in Frame is instantiated
        public void InstantiateCurrentHero(int index)
        {

            try
            {
                Destroy(instantiatedCurrentHero.gameObject);
            }
            catch (System.Exception)
            {

            }

            HeroPartyDB.getHero(index).setSkin(heroPrefabs[index]);
            instantiatedCurrentHero = Instantiate(heroPrefabs[index].transform, currentHeroStation.position, Quaternion.identity);
            instantiatedCurrentHero.transform.localScale = new Vector3(7.0f, 7.0f, 0.0f);
            heroLevel.text = System.Convert.ToString(HeroPartyDB.getHero(index).SkinTire);
        }

        //instantiates all the heroes and sets their skin
        public void InstantiateHeroes()
        {
            if (HeroPartyDB.getHeroList().Count == 1)
                fireButton.GetComponent<Selectable>().interactable = false;
            try
            {
                for (int i = 0; i < heroPrefabs.Length && i < HeroPartyDB.getHeroList().Count; i++)
                {
                    Destroy(instantiatedHeroes[i].gameObject);
                }
            }
            catch (System.Exception)
            {

            }

            for (int i = 0; i < heroPrefabs.Length && i < HeroPartyDB.getHeroList().Count; i++)
            {
                heroButtons[i].SetActive(true);
                heroPrefabs[i] = (GameObject)Resources.Load(HeroPartyDB.getHero(i).GetType().Name, typeof(GameObject));

                HeroPartyDB.getHero(i).setSkin(heroPrefabs[i]);
                instantiatedHeroes[i] = Instantiate(heroPrefabs[i].transform, heroStations[i].position, Quaternion.identity);
                instantiatedHeroes[i].transform.localScale = new Vector3(5.0f, 5.0f, 0.0f);
            }
           
        }

        //activates the right vehicle, destroys all the heroes and reinstantiates them
        public void ReInstantiateHeroes()
        {

            if (HeroPartyDB.getHeroList().Count == 1)
                fireButton.GetComponent<Selectable>().interactable = false;

            //if player has a vehicle
            if (SaveSystem.Pdata.HasVehicle)
            {

                switch (playerVehicle.Name)
                {
                    case "Scout":
                        instantiatedHeroVehicle.gameObject.SetActive(true);
                        break;


                    case "Warthog":
                        instantiatedHeroVehicle.gameObject.SetActive(true);
                        break;


                    case "Goliath":
                        instantiatedHeroVehicle.gameObject.SetActive(true);
                        break;

                    case "Leviathan":
                        instantiatedHeroVehicle.gameObject.SetActive(true);
                        break;

                    default:
                        break;
                }
            }


            for (int i = 0; i < instantiatedHeroes.Length; i++)
            {

                try
                {
                    Destroy(instantiatedHeroes[i].gameObject);
                }
                catch (System.Exception)
                {
                }

            }

            for (int i = 0; i < heroPrefabs.Length && i < HeroPartyDB.getHeroList().Count; i++)
            {
                heroButtons[i].SetActive(true);
                heroPrefabs[i] = (GameObject)Resources.Load(HeroPartyDB.getHero(i).GetType().Name, typeof(GameObject));
               
                HeroPartyDB.getHero(i).setSkin(heroPrefabs[i]);
                instantiatedHeroes[i] = Instantiate(heroPrefabs[i].transform, heroStations[i].position, Quaternion.identity);
                instantiatedHeroes[i].transform.localScale = new Vector3(5.0f, 5.0f, 0.0f);
            }


            InstantiateCurrentHero(0);
        }

        //destroys all heroes and deactivates the vehicle
        public void DeInstantiateHeroes()
        {
            foreach (var item in heroButtons)
            {
                item.SetActive(false);
            }

            for (int i = 0; i < HeroPartyDB.getHeroList().Count; i++)
            {
                heroButtons[i].SetActive(false);
            }


            try
            {
                for (int i = 0; i < HeroPartyDB.getHeroList().Count + 1; i++)
                {
                    instantiatedHeroes[i].gameObject.SetActive(false);
                }
            }
            catch (System.Exception)
            {}

            try
            {
                Destroy(instantiatedCurrentHero.gameObject);
            }
            catch (System.Exception)
            {}


            //if player has a vehicle
            if (SaveSystem.Pdata.HasVehicle)
            {

                switch (playerVehicle.Name)
                {
                    case "Scout":
                        instantiatedHeroVehicle.gameObject.SetActive(false);
                        break;


                    case "Warthog":
                        instantiatedHeroVehicle.gameObject.SetActive(false);
                        break;


                    case "Goliath":
                        instantiatedHeroVehicle.gameObject.SetActive(false);
                        break;

                    case "Leviathan":
                        instantiatedHeroVehicle.gameObject.SetActive(false);
                        break;

                    default:
                        break;
                }
            }
        }

        //clicking on a hero saves its index, instantiates it in the frame and sets the stats for it
        public void HeroOnClick()
        {
            selectedIndex = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name) - 1;

            InstantiateCurrentHero(selectedIndex);
            SetCurrentHeroStats(selectedIndex);
        }

        //sets the sum of stats of the hero in the scene
        public void SetCurrentHeroStats(int index)
        {
            health = 0;
            damage = 0;

            if (HeroPartyDB.getHero(index).SkinTire < 5)
            {
                if (instantiatedCurrentHero.gameObject.name == "Warrior(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire - 1).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire - 1).Damage + HeroPartyDB.getHero(index).Damage;
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
            else
            {
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

        //sets the stats of the whole party in the scene
        public void partyStats()
        {
            pHealth = 0;
            pDamage = 0;

            if (Player.HasVehicle)
            {
                for (int i = 0; i < HeroPartyDB.getHeroList().Count; i++)
                {
                    if (HeroPartyDB.getHero(i).SkinTire < 5)
                    {
                        if (instantiatedCurrentHero.gameObject.name == "Warrior(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(HeroPartyDB.getHero(i).SkinTire - 1).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(i).SkinTire).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                        else if (instantiatedCurrentHero.gameObject.name == "Mage(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(HeroPartyDB.getHero(i).SkinTire + 4).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(i).SkinTire + 4).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                        else if (instantiatedCurrentHero.gameObject.name == "Ranger(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(HeroPartyDB.getHero(i).SkinTire + 9).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(i).SkinTire + 9).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                        else if (instantiatedCurrentHero.gameObject.name == "(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(HeroPartyDB.getHero(i).SkinTire + 14).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(i).SkinTire + 14).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                        else if (instantiatedCurrentHero.gameObject.name == "Wizard(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(HeroPartyDB.getHero(i).SkinTire + 19).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(i).SkinTire + 19).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                    }
                    else
                    {
                        if (instantiatedCurrentHero.gameObject.name == "Warrior(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(4).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(4).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                        else if (instantiatedCurrentHero.gameObject.name == "Mage(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(9).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(9).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                        else if (instantiatedCurrentHero.gameObject.name == "Ranger(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(14).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(14).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                        else if (instantiatedCurrentHero.gameObject.name == "Spearman(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(19).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(19).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                        else if (instantiatedCurrentHero.gameObject.name == "Wizard(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(24).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(24).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                    }
                }
            }
            else
            {
                pHealth = health;
                pDamage = damage;
            }
            if (Player.HasVehicle) 
            pHealth += Player.CurrentVehicle.VehicleHP;


            partyDamage.text = pDamage.ToString();
            partyHealth.text = pHealth.ToString();
        }

        public void FireOnClick() {
            firePopUp.SetActive(true);
        }

        //removes a hero from player's party and recalculates the stats
        public void YesOnClick()
        {

            HeroPartyDB.getHeroList().RemoveAt(selectedIndex);
            HeroPartyDB.SaveParty();

            heroButtons[selectedIndex].SetActive(false);

            ReInstantiateHeroes();
            InstantiateCurrentHero(0);
            partyStats();

        }

    }
}
