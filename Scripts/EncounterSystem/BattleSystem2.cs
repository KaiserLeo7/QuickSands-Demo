using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Spine.Unity;
using UnityEngine.Rendering;

//COMMENTED BY ANDRIY OSTAPOVYCH

namespace Sands
{
    public enum BattleState2 { START, PLAYERTURN, PLAYERBUSY, ENEMYTURN, ENEMYBUSY, WON, LOST }

    public class BattleSystem2 : MonoBehaviour
    {
        public BattleState2 battleState;
        public static BattleSystem2 instance;

        //Audio
        [SerializeField] private AudioSource travelMusic;       //default audio track
        [SerializeField] private AudioSource battleMusic;       //battle audio track
        [SerializeField] private AudioSource defeatMusic;       //defeat audio track
        [SerializeField] private AudioSource victoryMusic;      //victory audio track

        //Scripts
        [SerializeField] private GameObject encounterSystem;    //Encounter System reference
        [SerializeField] private LevelLoader levelLoader;       //adds Fade In/Fade Out to Scene

        //UI
        [SerializeField] private GameObject defeatPopUp;        //Defeat PopUp reference
        [SerializeField] private GameObject victoryPopUp;       //Victory PopUp reference
        [SerializeField] private GameObject lootPopUp;          //Loot Manager PopUp reference
        [SerializeField] private Text dialogueText;             //Battle Text reference
        [SerializeField] private GameObject[] heroButtons = new GameObject[5];      //Hero Selection Buttons
        [SerializeField] private GameObject[] enemyButtons = new GameObject[5];     //Enemy Selection Buttons

        //UI Abilities
        [SerializeField] private GameObject healButton;             //Mage
        [SerializeField] private GameObject twoForOneButton;        //Ranger
        [SerializeField] private GameObject blockButton;            //Warrior
        [SerializeField] private GameObject spearmanAbilityButton;     //Spearman
        [SerializeField] private GameObject wizardAbilityButton;        //Spellcaster
        [SerializeField] private GameObject[] battleElements = new GameObject[3];      //Active Action Buttons
        [SerializeField] private GameObject[] tutorials = new GameObject[5];
        private bool shouldHeal = false;
        private bool shouldDefend = false;
        private bool shouldAttackTwo = false;
        private bool shouldAttackThree = false;

        //Actions
        private CharAction activeChar;                      //Selected Hero Actions
        private List<CharAction> enemysCharActions;             //Enemy Behaviour Action List
        List<CharAction> charactersActions = new List<CharAction>();     //List of all Hero Actions
        
        //Enemies
        private GameObject enemyVehiclePrefab;                  //Enemy Vehicle GameObject
        private Vehicle enemyVehicle = new Vehicle();           //Vehicle Database access
        private GameObject[] enemyPrefabs = new GameObject[1];  //Enemy Hero Prebs
        private Transform[] instantiatedEnemies;                //Enemy Transform List
        private Transform instantiatedEnemyVehicle;             //Enemy Vehicle Tranform

        //Health
        private GameObject[] heroHealthBarPrefabs = new GameObject[5];      //Hero HP Prefab List
        private GameObject[] enemyHealthBarPrefabs = new GameObject[5];       //Enemy HP Prefab List
        private Transform[] instantiatedHeroHealthBars = new Transform[5];      //Hero HP Transforms List
        private Transform[] instantiatedEnemyHealthBars = new Transform[5];      //Enemy HP Tranforms

        //Transform Enemy positions on Vehicles
        [SerializeField] private Transform EnemyVehicleBS;
        [SerializeField] private Transform EnemyBS1;
        [SerializeField] private Transform EnemyBS2;
        [SerializeField] private Transform EnemyBS3;
        [SerializeField] private Transform EnemyBS4;
        [SerializeField] private Transform EnemyBS5;
        private List<Transform> enemyBSList = new List<Transform>();

        //Transform Hero positions on Vehicles
        [SerializeField] private Transform[] heroBSList = new Transform[5];

        //Encounter Movement Couritines
        private Coroutine MoveEnemyVehicleOutC = null;
        private Coroutine MoveEnemyVehicleInC = null;

        //Selected Hero/Enemy Indicator
        private GameObject heroIndicator = null;
        private GameObject enemyIndicator = null;
        private Transform instantiatedHeroIndicator = null;
        private Transform instantiatedEnemyIndicator = null;
        private int selectedHero = 0;
        private int selectedHeroToHeal = -1;
        private int selectedEnemy = 0;
        private int enemyToAttack = 0;

        //Factions
        private int enemyFaction;
        [SerializeField] private GameObject factionPopUp;
        [SerializeField] private Text factionText;
        [SerializeField] private Text factionRequestAmount;
        [SerializeField] private Text factionDescription;
        [SerializeField] private Text factionName;
        private int factionIntAmount = 0;

        //Retreat Nitro
        [SerializeField] private GameObject nitroGauge;
        //[SerializeField] private GameObject fillCargo;
        [SerializeField] private GameObject FuelBarBs;
        [SerializeField] private GameObject FuelAmountBs;
        [SerializeField] private GameObject FuelTextBs;
        [SerializeField] private GameObject FuelBackBs;
        private double nitroHalfCheck = 0;

        private bool isPlayersTurn = false;

        private BattleChecker[] enemyBattleCheckers = new BattleChecker[5];

        public int EnemyBattleCounter { get; set; }

        public static BattleSystem2 GetInstance()
        {
            return instance;
        }

        private void Awake()
        {
            instance = this;
        }

        //Saves Encounter Position
        //Starts Battle Audio
        //Resets Battle State
        public void Begin()
        {
          
            BattleSaver.IsInABattle = true;     
            BattleSaver.SaveBattle();           

            BattleMusic();      

            battleState = BattleState2.START;   

            Parallex.ShouldMove = true;         //start background Parallex effect

            //initially hides Hero Selection buttons
            foreach (var item in heroButtons)
            {
                item.SetActive(false);
            }

            //initially hides Enemy Selection buttons
            foreach (var item in enemyButtons)
            {
                item.SetActive(false);
            }

            //Starts battle in Travel Scene
            StartCoroutine(SetupBattle());

            //Vehicle Retreat Nitro Check and Update
            if (EncounterSystem.fuelCurrentAmount == 2 || EncounterSystem.fuelMaxAmount == 1)
                nitroGauge.GetComponent<Image>().fillAmount = EncounterSystem.fuelCurrentAmount * 1;
            else if (nitroHalfCheck == 0.5)
                nitroGauge.GetComponent<Image>().fillAmount = (float)(EncounterSystem.fuelCurrentAmount * 0.5);
            else
                nitroGauge.GetComponent<Image>().fillAmount = EncounterSystem.fuelCurrentAmount * 0;
        }

        //Load Player Data
        //Decides Enemies Faction by Location or Random Rebel
        //Places Actors into Scene
        //Animates Background
        IEnumerator SetupBattle()
        {
            Player.LoadPlayer();            

            ChooseFaction();               

            enemysCharActions = SpawnEnemy();       
            
            dialogueText.text = "A caravan approaches!!";

            MoveEnemyVehicleInC = StartCoroutine(MoveEnemyVehicleIn());     

            //Vehicle Movement Animation
            MoveEnemyAnimation();     
            MoveHeroAnimation();

            yield return new WaitForSeconds(2f);

                                         

            //Display Action Buttons
            foreach (var item in battleElements)
            {
                item.SetActive(true);
            }

            //Display Retreat Nitro
            if (Player.HasVehicle)
            {
                FuelBarBs.SetActive(true);
                FuelBackBs.SetActive(true);
                FuelTextBs.SetActive(true);
                FuelAmountBs.SetActive(true);
            }

            //Hero/Enemy Movement Animation
                   //Stop background Parallex effect


            if (enemyFaction != 4)  //if enemy not a rebel
            {
                if (Player.FactionReputation[enemyFaction - 1] >= 200)      //if in position (200) reputation with faction
                    FactionPopUp();         //Display Faction Interaction Menu
                else
                {
                    PrepareBattle();        
                }
            }
            else {
                PrepareBattle();           
            }
        }


        //Function Displays Health Bars, Buttons, and Starts Battle with Players Turn
        private void PrepareBattle() {

            factionPopUp.SetActive(false);
            EnemyBattleCounter++;

            InstantiateHeroHealthBars();
            InstantiateEnemyHealthBars();

            SpawnHeroButtons();
            SpawnEnemyButtons();

            SetActiveChar(EncounterSystem.PlayersCharActions[0]);
            StartCoroutine(PlayerTurn());
        }

        //Function handles Enemy Faction, Dialog, and Toll Fee
        private void FactionPopUp()
        {
            //generate a fee Amount requested by faction 
            factionIntAmount = UnityEngine.Random.Range(100, 501);

            factionPopUp.SetActive(true);   //faction window made visible

            //adds randomness to Faction Encounter Dialog
            int factionDialogue = UnityEngine.Random.Range(1,4);

            //Depending on Faction and Faction Dialog variable get different Dialog Output
            switch (enemyFaction)
            {
                case 1:
                    factionDescription.text = "Robbert Barons";
                    factionDescription.color = Color.blue;
                    factionName.text = "The Veden Republic";
                    factionName.color = Color.blue;
                    if(factionDialogue == 1)
                        factionText.text = "The Veden corp owns this road. Payup or we will be forced to evict you.";
                    if (factionDialogue == 2)
                        factionText.text = "Oh a party of murder hobo-err.. I mean adventurers! Pay the toll or scram";
                    if (factionDialogue == 3)
                        factionText.text = "The great merchant republic requires your money to... maintain these roads!";
                    break;
                case 2:
                    factionDescription.text = "Cultured Cannibals";
                    factionDescription.color = Color.green;
                    factionName.text = "Fara Empire";
                    factionName.color = Color.green;
                    if (factionDialogue == 1)
                        factionText.text = "You look... delicious. Give us your money or your thighs!";
                    if (factionDialogue == 2)
                        factionText.text = "En garde vagrants! Unless you want to pay us of course...";
                    if (factionDialogue == 3)
                        factionText.text = "Give us your money or I will be enjoying your liver with some fava beans come supper!";
                    break;
                case 3:
                    factionDescription.text = "Kaiser's Militia";
                    factionDescription.color = Color.red;
                    factionName.text = "The Kaiserreich";
                    factionName.color = Color.red;
                    if (factionDialogue == 1)
                        factionText.text = "Halt! Render onto Kaiser that which is Kaisers";
                    if (factionDialogue == 2)
                        factionText.text = "The Kaiser's army wants you! I could be persuaded to look the other way...";
                    if (factionDialogue == 3)
                        factionText.text = "Our Glorious Overlord has levied a tax upon these roads. Pay us or die.";
                    break;
                default:
                    break;
            }
            //the money faction requests
            factionRequestAmount.text = factionIntAmount + " G";
        }

        //Ends the Faction Encounter
        //Resumes Travel Music
        //Saves Player Choice
        //Cleans Up Scene After
        public void PayOnClick()
        {
            if(PlayerInventory.Money >= factionIntAmount)
            {
                TravelMusic();      

                factionPopUp.SetActive(false);      

                //Move the Encounter out of the Scene
                MoveEnemyAnimation();           
                MoveEnemyVehicleOutC = StartCoroutine(MoveEnemyVehicleOut());
                MoveHeroAnimation();
                Parallex.ShouldMove = true;

                PlayerInventory.Money -= factionIntAmount;      //Faction Toll

                selectedEnemy = 0;      
                Player.FactionReputation[enemyFaction - 1] += 15;   //Faction Favor

                Player.SavePlayer();          
                BattleSaver.IsInABattle = false;
                EncounterSystem.instance.SaveBattleData();

                HideBattleUI();

                StartCoroutine(DestroyVehicle());    
            }
            else
            {
                //No money text
            }
        }

        //Starts a Battle w/ Encountered Faction 
        public void FightOnClick()
        {
            factionPopUp.SetActive(false);

            //Health
            InstantiateHeroHealthBars();
            InstantiateEnemyHealthBars();

            //Selection Buttons
            SpawnHeroButtons();
            SpawnEnemyButtons();

            SetActiveChar(EncounterSystem.PlayersCharActions[0]);
            Player.FactionReputation[enemyFaction - 1] -= 15;       //Fighting a faction reduces Reputation
            Player.SavePlayer();       
            StartCoroutine(PlayerTurn());       
        }

        //Function Generates a random number from which the Faction Encounter
        //can be either the Destination Towns Faction, Current Towns Faction, or Bandits
        public void ChooseFaction()
        {
            if (EncounterSystem.WasInABattle)
            {
                enemyFaction = BattleSaver.EnemyFaction;
            }
            else
            {
                switch (UnityEngine.Random.Range(0, 3))
                {
                    case 0:
                        enemyFaction = Player.LocationToTravelTo.Territory;         //Destination Town Faction
                        break;

                    case 1:
                        enemyFaction = Player.CurrentLocation.Territory;            //Current Town Faction
                        break;
                    case 2:
                        enemyFaction = 4;       //Bandits
                        break;

                }
                BattleSaver.EnemyFaction = enemyFaction;        //Save Faction to Prevent Rerolling
            }
        }

        //Funtion Instantiates Health Bars over Non Defeated Party Members
        //HP Bars are set as child to their hero
        private void InstantiateHeroHealthBars()
        {
            for (int i = 0; i < HeroPartyDB.getHeroList().Count; i++)
            {
                if (!EncounterSystem.HeroBattleCheckers[i].IsDead)
                {
                    heroHealthBarPrefabs[i] = (GameObject)Resources.Load("HealthBar", typeof(GameObject));    
                    instantiatedHeroHealthBars[i] = Instantiate(heroHealthBarPrefabs[i].transform, heroBSList[i].position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                    instantiatedHeroHealthBars[i].GetChild(0).GetComponent<BattleHUD>().SetHUDUnitHPHero(EncounterSystem.InstantiatedHeroes[i].GetComponent<Hero>());
                }
            }
        }

        //Funtion Instantiates Health Bars over all Enemies
        //HP Bars are set as child to their enemy
        private void InstantiateEnemyHealthBars()
        {
            for (int i = 0; i < enemyVehicle.PartySize; i++)
            {   
                enemyHealthBarPrefabs[i] = (GameObject)Resources.Load("HealthBar", typeof(GameObject));
                instantiatedEnemyHealthBars[i] = Instantiate(enemyHealthBarPrefabs[i].transform, enemyBSList[i].position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                instantiatedEnemyHealthBars[i].GetChild(0).GetComponent<BattleHUD>().SetHUDUnitHPHero(enemyVehicle.Passangers[i].GetComponent<Hero>());
                instantiatedEnemyHealthBars[i].SetParent(instantiatedEnemyVehicle);
            }
        }
        
        //Hero Indicator Shows Which Hero You Have Selected w/ Green Arrow
        private void InstantiateHeroIndicator()
        {
            if (instantiatedHeroIndicator)
            {
                DestroyHeroIndicator();
            }

            heroIndicator = (GameObject)Resources.Load("GreenArrow", typeof(GameObject));  
            instantiatedHeroIndicator = Instantiate(heroIndicator.transform, EncounterSystem.InstantiatedHeroes[selectedHero].transform.position + new Vector3(0, 2f, 0), Quaternion.Euler(0, 0, 0));
        }

        //Function Removes Hero Indicator
        private void DestroyHeroIndicator()
        {
            try
            {
                Destroy(instantiatedHeroIndicator.gameObject);
            }
            catch (Exception) { }
        }

        //Indicator Shows Which Enemy You Have Selected w/ Red Arrow
        private void InstantiateEnemyIndicator()
        {
            if (instantiatedEnemyIndicator)
            {
                DestroyEnemyIndicator();
            }

            enemyIndicator = (GameObject)Resources.Load("RedArrow", typeof(GameObject)); 
            instantiatedEnemyIndicator = Instantiate(enemyIndicator.transform, instantiatedEnemies[selectedEnemy].transform.position + new Vector3(0, 2f, 0), Quaternion.Euler(0, 0, 0));
        }

        //Function Removes Enemy Indicator
        private void DestroyEnemyIndicator()
        {
            try
            {
                Destroy(instantiatedEnemyIndicator.gameObject);
            }
            catch (Exception) { }
            
        }

        //Changes Active Character
        private void SetActiveChar(CharAction newChar)
        {
            activeChar = newChar;
        }

        //Function Changes the Battle State Based on Current State and Conditions
        private void chooseNextActiveChar()

        {
            
            if (battleState != BattleState2.WON && battleState != BattleState2.LOST)
            {
                if (!isPlayersTurn) 
                {
                    battleState = BattleState2.ENEMYTURN;
                }
                else     
                {
                    battleState = BattleState2.PLAYERTURN;
                }
            }
        }

        //Function Checks if Party member is Dead, IF Not Dead Spawns Selection Buttons
        private void SpawnHeroButtons()
        {
            //Battle Checker Holds party Conditions
            for (int i = 0; i < EncounterSystem.HeroBattleCheckers.Length; i++)
            {
                if (!EncounterSystem.HeroBattleCheckers[i].IsDead)
                    heroButtons[i].SetActive(true);

                heroButtons[i].transform.position = heroBSList[i].transform.position + new Vector3(0, 0.6f, 0);
            }

        }

        //Function Spawns Enemy Target Selection Buttons
        private void SpawnEnemyButtons()
        {
            for (int i = 0; i < enemyBattleCheckers.Length; i++)
            {
                enemyButtons[i].SetActive(true);
                enemyButtons[i].transform.position = enemyBSList[i].transform.position + new Vector3(0, 0.6f, 0);
            }
        }

        //Function handles Spawning Enemies, Battle Positions and Enemy Vehicle
        private List<CharAction> SpawnEnemy()
        {
            charactersActions = new List<CharAction>();

            //Adds Battle Positions to a single managable List
            enemyBSList.Add(EnemyBS1);
            enemyBSList.Add(EnemyBS2);
            enemyBSList.Add(EnemyBS3);
            enemyBSList.Add(EnemyBS4);
            enemyBSList.Add(EnemyBS5);

            //If you were in a battle before the game closed set enemy vehicle as that Vehicle
            if(EncounterSystem.WasInABattle)
            {
                enemyVehicle = new Vehicle(VehicleClassDB.getVehicle(BattleSaver.EnemyVehicle));
            }
            else
            {
                int partySize = HeroPartyDB.getHeroList().Count;
                int enemyVehicleIndex = 0;
                switch (partySize)
                {
                    case 1:
                        enemyVehicleIndex = UnityEngine.Random.Range(0, 1);
                        break;
                    case 2:
                        enemyVehicleIndex = UnityEngine.Random.Range(0, 2);
                        break;
                    case 3:
                        enemyVehicleIndex = UnityEngine.Random.Range(0, 3);
                        break;
                    case 4:
                        enemyVehicleIndex = UnityEngine.Random.Range(0, 4);
                        break;
                    case 5:
                        enemyVehicleIndex = UnityEngine.Random.Range(0, 4);
                        break;
                    default:
                        break;
                }
                //else pick a random Vehicle of 4 available (0-3)
                 
                
                enemyVehicle = new Vehicle(VehicleClassDB.getVehicle((enemyVehicleIndex)));

                BattleSaver.EnemyVehicle = enemyVehicleIndex;
                BattleSaver.SaveBattle();
            }

            //Based on Selected Vehicle Name Load the Proper Prefab, Instantiate it, Set its Animation to Idle,
            //place Battle Stations at specified positions, make them children to Vehicle
            //Change Prefab Sorting Layer
            switch (enemyVehicle.Name)
            {
                case "Scout":
                    enemyVehiclePrefab = (GameObject)Resources.Load("Scout2Vehicle", typeof(GameObject));  

                    enemyVehiclePrefab.GetComponent<SortingGroup>().sortingOrder = 9;

                    instantiatedEnemyVehicle = Instantiate(enemyVehiclePrefab.transform, EnemyVehicleBS.position, Quaternion.Euler(0, 180, 0));
                   
                    instantiatedEnemyVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                    //Scout has 2 Battle Stations
                    enemyBSList[0].transform.position = new Vector3(10.45f, 1.39f, 0f);
                    enemyBSList[1].transform.position = new Vector3(11.5f, 1.39f, 0f);
                   
                    enemyBSList[0].transform.SetParent(instantiatedEnemyVehicle);
                    enemyBSList[1].transform.SetParent(instantiatedEnemyVehicle);
                    break;

                case "Warthog":
                    enemyVehiclePrefab = (GameObject)Resources.Load("Warthog3Vehicle", typeof(GameObject)); 

                    enemyVehiclePrefab.GetComponent<SortingGroup>().sortingOrder = 9;     

                    instantiatedEnemyVehicle = Instantiate(enemyVehiclePrefab.transform, EnemyVehicleBS.position, Quaternion.Euler(0, 180, 0));
                  
                    instantiatedEnemyVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";

                    //Warthog has 3 Battle Stations
                    enemyBSList[0].transform.position = new Vector3(9.58f, 2.04f, 0f);
                    enemyBSList[1].transform.position = new Vector3(10.78f, 1.39f, 0f);
                    enemyBSList[2].transform.position = new Vector3(11.85f, 1.39f, 0f);
                    
                    enemyBSList[0].transform.SetParent(instantiatedEnemyVehicle);
                    enemyBSList[1].transform.SetParent(instantiatedEnemyVehicle);
                    enemyBSList[2].transform.SetParent(instantiatedEnemyVehicle);
                    break;

                case "Goliath":
                    enemyVehiclePrefab = (GameObject)Resources.Load("Goliath4Vehicle", typeof(GameObject));        

                    enemyVehiclePrefab.GetComponent<SortingGroup>().sortingOrder = 9;      
                    
                   
                    instantiatedEnemyVehicle = Instantiate(enemyVehiclePrefab.transform, EnemyVehicleBS.position, Quaternion.Euler(0, 180, 0));
                    
                    instantiatedEnemyVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                    //Goliath has 4 Battle Stations
                    enemyBSList[0].transform.position = new Vector3(9.49f, 1.9683f, 0f);
                    enemyBSList[1].transform.position = new Vector3(10.34f, 1.9683f, 0f);
                    enemyBSList[2].transform.position = new Vector3(11.39f, 2.2939f, 0f);
                    enemyBSList[3].transform.position = new Vector3(12.24f, 2.2939f, 0f);
                    
                    enemyBSList[0].transform.SetParent(instantiatedEnemyVehicle);
                    enemyBSList[1].transform.SetParent(instantiatedEnemyVehicle);
                    enemyBSList[2].transform.SetParent(instantiatedEnemyVehicle);
                    enemyBSList[3].transform.SetParent(instantiatedEnemyVehicle);
                    break;

                case "Leviathan":
                    enemyVehiclePrefab = (GameObject)Resources.Load("Leviathan5Vehicle", typeof(GameObject));    

                    enemyVehiclePrefab.GetComponent<SortingGroup>().sortingOrder = 9;      

                    instantiatedEnemyVehicle = Instantiate(enemyVehiclePrefab.transform, EnemyVehicleBS.position, Quaternion.Euler(0, 180, 0));
                    
                    instantiatedEnemyVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                    //Leviathan has 5 Battle Stations
                    enemyBSList[0].transform.position = new Vector3(8.745f, 1.274f, 0f);
                    enemyBSList[1].transform.position = new Vector3(9.79f, 1.274f, 0f);
                    enemyBSList[2].transform.position = new Vector3(10.54f, 2.11f, 0f);
                    enemyBSList[3].transform.position = new Vector3(11.42f, 2.11f, 0f);
                    enemyBSList[4].transform.position = new Vector3(12.27f, 2.11f, 0f);
                   
                    enemyBSList[0].transform.SetParent(instantiatedEnemyVehicle);
                    enemyBSList[1].transform.SetParent(instantiatedEnemyVehicle);
                    enemyBSList[2].transform.SetParent(instantiatedEnemyVehicle);
                    enemyBSList[3].transform.SetParent(instantiatedEnemyVehicle);
                    enemyBSList[4].transform.SetParent(instantiatedEnemyVehicle);
                    break;

                default:
                    break;
            }
            //set Enemy Prefab holder to size of Enemy Vehicle (Always holds max Enemy Party Members)
            enemyPrefabs = new GameObject[enemyVehicle.PartySize];      
            instantiatedEnemies = new Transform[enemyPrefabs.Length];  
            enemyBattleCheckers = new BattleChecker[enemyPrefabs.Length];   //Status Script added

            if (!EncounterSystem.WasInABattle)
            {
                BattleSaver.EnemyHeroes = new List<int>();
                BattleSaver.EnemySkins = new List<int>();
            }

            for (int i = 0; i < instantiatedEnemies.Length; i++)
            {
                //if there was an interupted battle
                //Load enemy heroes from BattleSaver
                //update their apperance
                if (EncounterSystem.WasInABattle)
                {
                    enemyPrefabs[i] = (GameObject)Resources.Load(HeroClassDB.getHero(BattleSaver.EnemyHeroes[i]).GetType().Name, typeof(GameObject));
                    enemyPrefabs[i].GetComponent<Hero>().SkinTire = BattleSaver.EnemySkins[i];
                    enemyPrefabs[i].GetComponent<Hero>().setSkin(enemyPrefabs[i]);
                }
                else
                {
             
                    int enemy;
                    if (enemyFaction == 4) //Bandits
                    {
                        //update their apperance to "Shadow Skins" Tiers 1 to 6
                        enemy = UnityEngine.Random.Range(5, 10);
                        enemyPrefabs[i] = (GameObject)Resources.Load(HeroClassDB.getHero(enemy).GetType().Name, typeof(GameObject));
                    }
                    else
                    {
                        
                        //if the reputation with that faction is greater then 200 (Neutral or Friendly)
                        if (Player.FactionReputation[enemyFaction - 1] >= 200)
                        {
                            //load enemy with random skin tier "Regular" (1-4)
                            enemy = UnityEngine.Random.Range(1, 5);
                            enemyPrefabs[i] = (GameObject)Resources.Load(HeroClassDB.getHero(enemy).GetType().Name, typeof(GameObject));
                        }
                        else
                        {
                            //load enemy with random skin tier "Shadow" (5-9)
                            enemy = UnityEngine.Random.Range(5, 10);    
                            enemyPrefabs[i] = (GameObject)Resources.Load(HeroClassDB.getHero(enemy).GetType().Name, typeof(GameObject));
                        }
                    }
                    //save Enemy
                    BattleSaver.EnemyHeroes.Add(enemy);


                    int skinTire = UnityEngine.Random.Range(1, 5);

                    enemyPrefabs[i].GetComponent<Hero>().SkinTire = skinTire;
                    enemyPrefabs[i].GetComponent<Hero>().setSkin(enemyPrefabs[i]);

                    enemyPrefabs[i].GetComponent<SortingGroup>().sortingOrder = 8;

                    BattleSaver.EnemySkins.Add(skinTire);
                    BattleSaver.SaveBattle();
                }                

                instantiatedEnemies[i] = Instantiate(enemyPrefabs[i].transform, enemyBSList[i].position, Quaternion.Euler(0, 180, 0));
                //adds arnors value to the HP
                instantiatedEnemies[i].GetComponent<Hero>().MaxHP += ArmorDatabase.getArmor(enemyPrefabs[i].GetComponent<Hero>().SkinTire - 1).Health;
                instantiatedEnemies[i].GetComponent<Hero>().CurrentHP += ArmorDatabase.getArmor(enemyPrefabs[i].GetComponent<Hero>().SkinTire - 1).Health;
                //Adds weapon damage value to the Attack/Damage
                instantiatedEnemies[i].GetComponent<Hero>().Damage += WeaponDatabase.getWeapon(enemyPrefabs[i].GetComponent<Hero>().SkinTire - 1).Damage;
                //vehicles have a Passanger Variable for easier Enemy Hero Access
                enemyVehicle.addPassangers(instantiatedEnemies[i]);
                //Access all Enemy Actions
                charactersActions.Add(instantiatedEnemies[i].GetComponent<CharAction>());
                charactersActions[i].PlayIdleAnim();
                //Save
                enemyBattleCheckers[i] = instantiatedEnemies[i].GetComponent<BattleChecker>();
                instantiatedEnemies[i].SetParent(instantiatedEnemyVehicle);
                //Removes Boss Buffs from enemies
                instantiatedEnemies[i].GetComponent<HeroBuff>().AddBuffs(false);
            }

            if (EncounterSystem.WasInABattle)
                EncounterSystem.WasInABattle = false;

            return charactersActions;
        }


        //Changes Selected Hero
        //If Mage Allows To Select Friendly Members
        public void HeroOnClick()
        {

            if (battleState != BattleState2.PLAYERTURN)
            {
                //Mage Heal Ability
                if (shouldHeal)
                {
                    shouldHeal = false;

                    selectedHeroToHeal = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name) - 6;
                    EncounterSystem.HeroBattleCheckers[selectedHero].UsedTurn = true;
                    for (int i = 0; i < EncounterSystem.HeroBattleCheckers.Length; i++)
                    {
                        if(EncounterSystem.HeroBattleCheckers[i].IsDead || EncounterSystem.HeroBattleCheckers[i].UsedTurn)
                        {
                            heroButtons[i].SetActive(false);
                        }
                    }

                    StartCoroutine(HealSelectedHero());
                }
                else
                {
                    return;
                }
            }
            else
            {
                selectedHero = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name) - 6;
                instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetLoss(instantiatedEnemies[selectedEnemy].GetComponent<Hero>().CurrentHP - EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().getDamag());
                InstantiateHeroIndicator();
                DisplayAbilityButton();
            }
        }

        //Function Changes Selected Enemy
        //If Heal Ability is Active Doesnt Heal
        //Displays Attacking Heroes Possible Damage
        public void EnemyOnClick()
        {
            if (shouldHeal)
            {
                shouldHeal = false;

                for (int i = 0; i < EncounterSystem.HeroBattleCheckers.Length; i++)
                {
                    if (EncounterSystem.HeroBattleCheckers[i].IsDead || EncounterSystem.HeroBattleCheckers[i].UsedTurn)
                    {
                        heroButtons[i].SetActive(false);
                    }
                }
                //Indicators
                InstantiateEnemyIndicator();
                InstantiateHeroIndicator();
                battleState = BattleState2.PLAYERTURN;
                dialogueText.text = "Choose an action!";
                //Display HP Loss on Attack
                instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetLoss(instantiatedEnemies[selectedEnemy].GetComponent<Hero>().CurrentHP);
                selectedEnemy = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name) - 1;
                instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetLoss(instantiatedEnemies[selectedEnemy].GetComponent<Hero>().CurrentHP - EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().getDamag());
                InstantiateEnemyIndicator();
            }
            else if (battleState != BattleState2.PLAYERTURN)
            {
                return;
            }
            else
            {
                //Display HP Loss on Attack
                instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetLoss(instantiatedEnemies[selectedEnemy].GetComponent<Hero>().CurrentHP);
                selectedEnemy = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name) - 1;
                instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetLoss(instantiatedEnemies[selectedEnemy].GetComponent<Hero>().CurrentHP - EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().getDamag());
                InstantiateEnemyIndicator();
            }
        }

        //Based on the selected hero changes the ability button
        private void DisplayAbilityButton()
        {
            switch (EncounterSystem.InstantiatedHeroes[selectedHero].name)
            {
                case "Warrior(Clone)":
                    blockButton.SetActive(true);
                    healButton.SetActive(false);
                    twoForOneButton.SetActive(false);
                    spearmanAbilityButton.SetActive(false);
                    wizardAbilityButton.SetActive(false);
                    break;
                case "Mage(Clone)":
                    blockButton.SetActive(false);
                    healButton.SetActive(true);
                    twoForOneButton.SetActive(false);
                    spearmanAbilityButton.SetActive(false);
                    wizardAbilityButton.SetActive(false);
                    break;
                case "Ranger(Clone)":
                    blockButton.SetActive(false);
                    healButton.SetActive(false);
                    twoForOneButton.SetActive(true);
                    spearmanAbilityButton.SetActive(false);
                    wizardAbilityButton.SetActive(false);
                    break;
                case "Spearman(Clone)":
                    blockButton.SetActive(false);
                    healButton.SetActive(false);
                    twoForOneButton.SetActive(false);
                    spearmanAbilityButton.SetActive(true);
                    wizardAbilityButton.SetActive(false);
                    break;
                case "Wizard(Clone)":
                    blockButton.SetActive(false);
                    healButton.SetActive(false);
                    twoForOneButton.SetActive(true);
                    spearmanAbilityButton.SetActive(false);
                    wizardAbilityButton.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        //Lets the player play their turn
        IEnumerator PlayerTurn()
        {
            bool allHeroesUsedTurns = true;     //checks if all heroes used their turn

            foreach (var hero in EncounterSystem.HeroBattleCheckers)
            {
                if (!hero.IsDead)
                {
                    if (!hero.UsedTurn)
                        allHeroesUsedTurns = false;
                }
            }

            //Reactivates all hero buttons 
            if (allHeroesUsedTurns)
            {
                for (int i = 0; i < EncounterSystem.HeroBattleCheckers.Length; i++)
                {
                    if (!EncounterSystem.HeroBattleCheckers[i].IsDead)
                    {
                        heroButtons[i].SetActive(true);
                    }
                    EncounterSystem.HeroBattleCheckers[i].UsedTurn = false;
                }

            }

            //Selects a random hero for the player
            do
            {
                selectedHero = UnityEngine.Random.Range(0, EncounterSystem.InstantiatedHeroes.Length);
            } while (EncounterSystem.HeroBattleCheckers[selectedHero].IsDead || EncounterSystem.HeroBattleCheckers[selectedHero].UsedTurn);

            instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetLoss(instantiatedEnemies[selectedEnemy].GetComponent<Hero>().CurrentHP - EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().getDamag());
            InstantiateHeroIndicator();
            InstantiateEnemyIndicator();
            DisplayAbilityButton();

            yield return new WaitForSeconds(0.5f);
            battleState = BattleState2.PLAYERTURN;
            //Damage Over Time if Applicable
            DotDamageToEnemies();
            dialogueText.text = "Choose an action:";
        }

        //Function Handles Special Ability and Regular Attack Damage
        //Targers for Abilities are Alive
        //Creates A Damage PopUp Based on Attacking Heroes Damage
        //Updates Targets HP, Animates when Damaged
        public IEnumerator CalculateDamage()
        {
            
            if (battleState == BattleState2.PLAYERBUSY)
            {
                bool isEnemyDead;
                bool isEnemyTwoDead = false;
                bool isEnemyThreeDead = false;
                int secondAttackedEnemy = -1;
                int thirdAttackedEnemy = -1;
                bool isCrit = false;

                //Spearman Ability
                if (shouldAttackThree)
                {
                    shouldAttackThree = false;
                    do
                    {
                        secondAttackedEnemy = UnityEngine.Random.Range(0, instantiatedEnemies.Length);
                    } while (enemyBattleCheckers[secondAttackedEnemy].IsDead || secondAttackedEnemy == selectedEnemy);

                    do
                    {
                        thirdAttackedEnemy = UnityEngine.Random.Range(0, instantiatedEnemies.Length);
                    } while (enemyBattleCheckers[thirdAttackedEnemy].IsDead || thirdAttackedEnemy == selectedEnemy || thirdAttackedEnemy == secondAttackedEnemy);

                    //Ability Targers
                    isEnemyDead = instantiatedEnemies[selectedEnemy].GetComponent<Hero>().TakeDamage(EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 3);
                    isEnemyTwoDead = instantiatedEnemies[secondAttackedEnemy].GetComponent<Hero>().TakeDamage(EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 3);
                    isEnemyThreeDead = instantiatedEnemies[thirdAttackedEnemy].GetComponent<Hero>().TakeDamage(EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 3);
                    
                    DamagePopUp.Create(instantiatedEnemies[selectedEnemy].position, EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 3, false);
                    DamagePopUp.Create(instantiatedEnemies[secondAttackedEnemy].position, EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 3, false);
                    DamagePopUp.Create(instantiatedEnemies[thirdAttackedEnemy].position, EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 3, false);

                    //HP Update
                    instantiatedEnemyHealthBars[secondAttackedEnemy].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedEnemies[secondAttackedEnemy].GetComponent<Hero>().CurrentHP);
                    instantiatedEnemyHealthBars[thirdAttackedEnemy].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedEnemies[thirdAttackedEnemy].GetComponent<Hero>().CurrentHP);

                    instantiatedEnemies[secondAttackedEnemy].GetComponent<CharAnimation>().HitAnim();
                    instantiatedEnemies[thirdAttackedEnemy].GetComponent<CharAnimation>().HitAnim();
                }
                else if (shouldAttackTwo)   //Ranger Ability
                {
                    shouldAttackTwo = false;
                    do
                    {
                        secondAttackedEnemy = UnityEngine.Random.Range(0, instantiatedEnemies.Length);
                    } while (enemyBattleCheckers[secondAttackedEnemy].IsDead || secondAttackedEnemy == selectedEnemy);

                    //Ability Targers
                    isEnemyDead = instantiatedEnemies[selectedEnemy].GetComponent<Hero>().TakeDamage(EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 2);
                    isEnemyTwoDead = instantiatedEnemies[secondAttackedEnemy].GetComponent<Hero>().TakeDamage(EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 2);
                    
                    DamagePopUp.Create(instantiatedEnemies[selectedEnemy].position, EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 2, false);

                    instantiatedEnemyHealthBars[secondAttackedEnemy].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedEnemies[secondAttackedEnemy].GetComponent<Hero>().CurrentHP);
                    DamagePopUp.Create(instantiatedEnemies[secondAttackedEnemy].position, EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 2, false);
                    
                    instantiatedEnemies[secondAttackedEnemy].GetComponent<CharAnimation>().HitAnim();
                }
                else    //Regular Attack
                {
                    Vector3 selectedEnemyPos = instantiatedEnemies[selectedEnemy].position;
                    int damage = EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().getDamageWithCrit(ref isCrit);

                    isEnemyDead = instantiatedEnemies[selectedEnemy].GetComponent<Hero>().TakeDamage(damage);

                    DamagePopUp.Create(selectedEnemyPos, damage, isCrit);
                }

                //HP Update
                instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedEnemies[selectedEnemy].GetComponent<Hero>().CurrentHP);
                instantiatedEnemies[selectedEnemy].GetComponent<CharAnimation>().HitAnim();

                yield return new WaitForSeconds(0.5f);

                EncounterSystem.HeroBattleCheckers[selectedHero].UsedTurn = true;
                heroButtons[selectedHero].SetActive(false);

            //Enemy Target Clean Up
                if (isEnemyTwoDead)
                {
                    enemyBattleCheckers[secondAttackedEnemy].IsDead = true;
                    enemyButtons[secondAttackedEnemy].SetActive(false);
                    Destroy(instantiatedEnemies[secondAttackedEnemy].gameObject);
                    Destroy(instantiatedEnemyHealthBars[secondAttackedEnemy].gameObject);
                }

                if (isEnemyThreeDead)
                {
                    enemyBattleCheckers[thirdAttackedEnemy].IsDead = true;
                    enemyButtons[thirdAttackedEnemy].SetActive(false);
                    Destroy(instantiatedEnemies[thirdAttackedEnemy].gameObject);
                    Destroy(instantiatedEnemyHealthBars[thirdAttackedEnemy].gameObject);
                }

                if (isEnemyDead)
                {
                    bool allEnemiesAreDead = true;
                    enemyBattleCheckers[selectedEnemy].IsDead = true;

                    enemyButtons[selectedEnemy].SetActive(false);

                    foreach (var enemy in enemyBattleCheckers)
                    {
                        if (!enemy.IsDead)
                            allEnemiesAreDead = false;
                    }

                    if (allEnemiesAreDead)
                    {
                        Destroy(instantiatedEnemies[selectedEnemy].gameObject);
                        Destroy(instantiatedEnemyHealthBars[selectedEnemy].gameObject);
                        battleState = BattleState2.WON;
                        DeathEnemyAnimation();
                        DestroyAllHealthBars();
                        EndBattle();
                    }
                    else
                    {
                        Destroy(instantiatedEnemies[selectedEnemy].gameObject);
                        Destroy(instantiatedEnemyHealthBars[selectedEnemy].gameObject);
                        battleState = BattleState2.ENEMYTURN;
                        instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetLoss(instantiatedEnemies[selectedEnemy].GetComponent<Hero>().CurrentHP);

                        do
                        {
                            selectedEnemy = UnityEngine.Random.Range(0, instantiatedEnemies.Length);
                        } while (enemyBattleCheckers[selectedEnemy].IsDead);
                        DestroyEnemyIndicator();

                        StartCoroutine(EnemyTurn());        
                    }
                }
                else
                {
                    battleState = BattleState2.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
            }
            else if (battleState == BattleState2.ENEMYBUSY)     //Enemy Attack Logic
            {
                int attackedHero;
                bool isCrit = false;

                do
                {
                    attackedHero = UnityEngine.Random.Range(0, EncounterSystem.InstantiatedHeroes.Length);
                } while (EncounterSystem.HeroBattleCheckers[attackedHero].IsDead);

                for (int i = 0; i < EncounterSystem.InstantiatedHeroes.Length; i++)
                {

                    if (EncounterSystem.HeroBattleCheckers[i].IsDead == false)
                    {
                        if (EncounterSystem.InstantiatedHeroes[i].GetComponent<Hero>().CurrentHP < EncounterSystem.InstantiatedHeroes[attackedHero].GetComponent<Hero>().CurrentHP &&
                        EncounterSystem.InstantiatedHeroes[i].GetComponent<Hero>().MaxHP != EncounterSystem.InstantiatedHeroes[i].GetComponent<Hero>().CurrentHP)
                            attackedHero = i;
                    }
                }
                
                int damage = instantiatedEnemies[enemyToAttack].GetComponent<Hero>().getDamageWithCrit(ref isCrit);
                bool isPlayerDead = EncounterSystem.InstantiatedHeroes[attackedHero].GetComponent<Hero>().TakeDamage(damage);
                

                DamagePopUp.Create(EncounterSystem.InstantiatedHeroes[attackedHero].position, damage, isCrit);
                instantiatedHeroHealthBars[attackedHero].GetChild(0).GetComponent<BattleHUD>().SetHP(EncounterSystem.InstantiatedHeroes[attackedHero].GetComponent<Hero>().CurrentHP);
                EncounterSystem.InstantiatedHeroes[attackedHero].GetComponent<CharAnimation>().HitAnim();

                yield return new WaitForSeconds(0.5f);

                //if dead you lost, if not enemy Turn
                if (isPlayerDead)
                {
                    bool allHeroesAreDead = true;
                    EncounterSystem.HeroBattleCheckers[attackedHero].IsDead = true;

                    heroButtons[attackedHero].SetActive(false);
                    Destroy(EncounterSystem.InstantiatedHeroes[attackedHero].gameObject);
                    Destroy(instantiatedHeroHealthBars[attackedHero].gameObject);
                    DestroyHeroIndicator();

                    foreach (var hero in EncounterSystem.HeroBattleCheckers)
                    {
                        if (!hero.IsDead)
                            allHeroesAreDead = false;
                    }

                    if (allHeroesAreDead)
                    {
                        battleState = BattleState2.LOST;
                        DeathHeroAnimation();
                        EndBattle();
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.8f);
                        StartCoroutine(PlayerTurn());
                    }
                }
                else
                {
                    yield return new WaitForSeconds(0.8f);
                    StartCoroutine(PlayerTurn());
                }
            }
        }

        //Function Handles DoT Damage to Heroes, to be used with more Complex Battle Behavior 
        private void DotDamageToHeroes()
        {
            bool isHeroDeadDOT;
            int casterDOTDamage = HeroClassDB.getHero(3).Damage / 3;

            //if player turn
            if (battleState == BattleState2.PLAYERTURN)
            {
                //go thought list of heroes
                for (int i = 0; i < EncounterSystem.InstantiatedHeroes.Length; i++)
                {
                    if (!EncounterSystem.HeroBattleCheckers[i].IsDead)
                    {
                        //check if hero has DOT counter > 0
                        if (EncounterSystem.HeroBattleCheckers[i].DOTCounter > 0)
                        {
                            //deal spellcaster damage to hero 
                            isHeroDeadDOT = EncounterSystem.InstantiatedHeroes[i].GetComponent<Hero>().TakeDamage(casterDOTDamage);
                            DamagePopUp.Create(EncounterSystem.InstantiatedHeroes[i].position, casterDOTDamage, false);

                            //Update HP
                            instantiatedHeroHealthBars[i].GetChild(0).GetComponent<BattleHUD>().SetHP(EncounterSystem.InstantiatedHeroes[i].GetComponent<Hero>().CurrentHP);
                            EncounterSystem.InstantiatedHeroes[i].GetComponent<CharAnimation>().HitAnim();

                            EncounterSystem.HeroBattleCheckers[i].DOTCounter -= 1;

                            //if all dead
                            if (isHeroDeadDOT)
                            {
                                bool allHeroesAreDead = true;
                                EncounterSystem.HeroBattleCheckers[i].IsDead = true;

                                heroButtons[i].SetActive(false);

                                foreach (var hero in EncounterSystem.HeroBattleCheckers)
                                {
                                    if (!hero.IsDead)
                                        allHeroesAreDead = false;
                                }

                                if (allHeroesAreDead)
                                {
                                    Destroy(EncounterSystem.InstantiatedHeroes[i].gameObject);
                                    Destroy(instantiatedHeroHealthBars[i].gameObject);
                                    battleState = BattleState2.LOST;
                                    DeathHeroAnimation();
                                    DestroyAllHealthBars();
                                    EndBattle();
                                }
                            }
                        }
                    }
                }
            }
        }


        private void DestroyAllHealthBars()
        {
            for (int i = 0; i < EncounterSystem.HeroBattleCheckers.Length; i++)
            {
                if (!EncounterSystem.HeroBattleCheckers[i].IsDead)
                {
                    Destroy(instantiatedHeroHealthBars[i].gameObject);
                }
            }
        }

        public void OnAttackButton()
        {

            if (battleState != BattleState2.PLAYERTURN)
            {
                return;
            }
            else
            {
                battleState = BattleState2.PLAYERBUSY;

                EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<AudioPlayer>().PlayAttack();

                EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<CharAction>().Attack(() =>
                {
                    chooseNextActiveChar();
                });
            }
        }

        //Function Handles Enemy Behavior
        //Enemy Targets Heroes with lowest HP to reduce numbers faster
        IEnumerator EnemyTurn()
        {
            while (battleState == BattleState2.ENEMYTURN)
            {
                yield return new WaitForSeconds(1f);
                dialogueText.text = "The enemy is attacking!";

                yield return new WaitForSeconds(1f);

                //HERE YOU CAN EXPAND THE BEHAVIOUR OF THE ENEMY BASED ON % OF HEALTH OR OTHER FACTORS
                battleState = BattleState2.ENEMYBUSY;

                bool allEnemiesUsedTurn = true;

                foreach (var enemy in enemyBattleCheckers)
                {
                    if (!enemy.UsedTurn && !enemy.IsDead)
                        allEnemiesUsedTurn = false;
                }

                if (allEnemiesUsedTurn)
                {
                    foreach (var enemy in enemyBattleCheckers)
                    {
                        enemy.UsedTurn = false;
                    }
                }

                do
                {
                    enemyToAttack = UnityEngine.Random.Range(0, instantiatedEnemies.Length);
                } while (enemyBattleCheckers[enemyToAttack].IsDead || enemyBattleCheckers[enemyToAttack].UsedTurn);

                instantiatedEnemies[enemyToAttack].GetComponent<BattleChecker>().UsedTurn = true;
                if (shouldDefend)
                {
                    shouldDefend = false;
                    EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<CharAnimation>().HealAnim();
                    EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<AudioPlayer>().PlayAbility();

                    instantiatedEnemies[enemyToAttack].GetComponent<CharAnimation>().AttackAnim();
                    yield return new WaitForSeconds(1.2f);
                    StartCoroutine(PlayerTurn());
                }

                else
                {
                    instantiatedEnemies[enemyToAttack].GetComponent<AudioPlayer>().PlayAttack();

                    instantiatedEnemies[enemyToAttack].GetComponent<CharAction>().Attack(() =>
                    {
                        chooseNextActiveChar();
                    });
                }
             
            }
        }
        //Heal Ability Logic
        private IEnumerator HealSelectedHero()
        {
            if (battleState == BattleState2.PLAYERBUSY)
            {
                EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<CharAnimation>().HealAnim();
                EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<AudioPlayer>().PlayAbility();

                EncounterSystem.InstantiatedHeroes[selectedHeroToHeal].GetComponent<Hero>().Heal(EncounterSystem.InstantiatedHeroes[selectedHeroToHeal].GetComponent<Hero>().MaxHP / 2);
                instantiatedHeroHealthBars[selectedHeroToHeal].GetChild(0).GetComponent<BattleHUD>().SetHP(EncounterSystem.InstantiatedHeroes[selectedHeroToHeal].GetComponent<Hero>().CurrentHP);
                dialogueText.text = "They feel stronger!";
                yield return new WaitForSeconds(0.8f);
                battleState = BattleState2.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }

        //Spellcasters Dot Ability Logic
        //Deals Damage to Selected Enemy for 3 Turns a 3rd of Spellcasters Regular Damage
        //Resets Counter if used on same Enemy
        private void DotDamageToEnemies()
        {
            bool isEnemyDeadDOT;
            int casterDOTDamage = HeroClassDB.getHero(3).Damage / 3;

            //if enemy turn
            if (battleState == BattleState2.PLAYERTURN)
            {
                for (int i = 0; i < instantiatedEnemies.Length; i++)
                {
                    if (!enemyBattleCheckers[i].IsDead)
                    {
                        //check if enemy has a DOT Counter
                        if (enemyBattleCheckers[i].DOTCounter > 0)
                        {
                            //deal damage from Spellcaster
                            isEnemyDeadDOT = instantiatedEnemies[i].GetComponent<Hero>().TakeDamage(casterDOTDamage);
                            DamagePopUp.Create(instantiatedEnemies[i].position, casterDOTDamage, false);

                            //Update HP
                            instantiatedEnemyHealthBars[i].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedEnemies[i].GetComponent<Hero>().CurrentHP);
                            instantiatedEnemies[i].GetComponent<CharAnimation>().HitAnim();

                            enemyBattleCheckers[i].DOTCounter -= 1;

                            //check if all dead
                            if (isEnemyDeadDOT)
                            {
                                enemyBattleCheckers[i].IsDead = true;
                                Destroy(instantiatedEnemies[i].gameObject);
                                Destroy(instantiatedEnemyHealthBars[i].gameObject);
                                enemyButtons[i].SetActive(false);
                                DestroyEnemyIndicator();

                                bool allEnemiesAreDead = true;

                                foreach (var enemy in enemyBattleCheckers)
                                {
                                    if (!enemy.IsDead)
                                        allEnemiesAreDead = false;
                                }

                                if (allEnemiesAreDead)
                                {

                                    battleState = BattleState2.WON;
                                    DeathEnemyAnimation();
                                    DestroyAllHealthBars();
                                    EndBattle();
                                }
                                else
                                {
                                    do
                                    {
                                        selectedEnemy = UnityEngine.Random.Range(0, instantiatedEnemies.Length);
                                    } while (enemyBattleCheckers[selectedEnemy].IsDead);
                                }
                            }
                        }
                    }
                }
            }
        }

        //Based on Ability Used Adjusts the Scene
        public void OnAbilityButton()
        {
            if (battleState != BattleState2.PLAYERTURN)
                return;
            else
            {
                battleState = BattleState2.PLAYERBUSY;

                string name = EncounterSystem.InstantiatedHeroes[selectedHero].name;
                switch (name)
                {
                    case "Warrior(Clone)":
                        shouldDefend = true;
                        DestroyHeroIndicator();
                        DestroyEnemyIndicator();
                        EncounterSystem.HeroBattleCheckers[selectedHero].UsedTurn = true;
                        heroButtons[selectedHero].SetActive(false);
                        battleState = BattleState2.ENEMYTURN;
                        StartCoroutine(EnemyTurn());
                        break;

                    case "Mage(Clone)":
                        shouldHeal = true;
                        dialogueText.text = "Choose a hero to heal";
                        DestroyHeroIndicator();
                        DestroyEnemyIndicator();
                        

                        for (int i = 0; i < EncounterSystem.HeroBattleCheckers.Length; i++)
                        {
                            if (!EncounterSystem.HeroBattleCheckers[i].IsDead)
                            {
                                heroButtons[i].SetActive(true);
                            }
                        }

                        break;

                    case "Ranger(Clone)":
                        EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<CharAnimation>().HealAnim();
                        EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<AudioPlayer>().PlayAbility();

                        int enemiesAlive = 0;

                        foreach (var item in enemyBattleCheckers)
                        {
                            if (!item.IsDead)
                                enemiesAlive++;
                        }

                        if(enemiesAlive > 1)
                            shouldAttackTwo = true;

                        StartCoroutine(CalculateDamage());
                        break;

                    case "Spearman(Clone)":
                        EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<CharAnimation>().HealAnim();
                        EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<AudioPlayer>().PlayAbility();

                        int allEnemiesAlive = 0;

                        foreach (var item in enemyBattleCheckers)
                        {
                            if (!item.IsDead)
                                allEnemiesAlive++;
                        }

                        if (allEnemiesAlive > 2)
                            shouldAttackThree = true;
                        else if (allEnemiesAlive > 1)
                            shouldAttackTwo = true;

                        StartCoroutine(CalculateDamage());
                        break;

                    case "Wizard(Clone)":
                        EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<CharAnimation>().HealAnim();
                        EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<AudioPlayer>().PlayAbility();

                        enemyBattleCheckers[selectedEnemy].DOTCounter = 2;

                        bool isEnemyDead = instantiatedEnemies[selectedEnemy].GetComponent<Hero>().TakeDamage(HeroClassDB.getHero(3).Damage / 3);
                        DamagePopUp.Create(instantiatedEnemies[selectedEnemy].position, HeroClassDB.getHero(3).Damage / 3, false);

                        instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedEnemies[selectedEnemy].GetComponent<Hero>().CurrentHP);
                        instantiatedEnemies[selectedEnemy].GetComponent<CharAnimation>().HitAnim();

                        if (isEnemyDead)
                        {

                            enemyBattleCheckers[selectedEnemy].IsDead = true;
                            Destroy(instantiatedEnemies[selectedEnemy].gameObject);
                            Destroy(instantiatedEnemyHealthBars[selectedEnemy].gameObject);
                            enemyButtons[selectedEnemy].SetActive(false);

                            bool allEnemiesAreDead = true;

                            foreach (var enemy in enemyBattleCheckers)
                            {
                                if (!enemy.IsDead)
                                    allEnemiesAreDead = false;
                            }

                            if (allEnemiesAreDead)
                            {
                                battleState = BattleState2.WON;
                                DeathEnemyAnimation();
                                DestroyAllHealthBars();
                                EndBattle();
                            }
                            else
                            {
                                do
                                {
                                    selectedEnemy = UnityEngine.Random.Range(0, instantiatedEnemies.Length);
                                } while (enemyBattleCheckers[selectedEnemy].IsDead);
                                battleState = BattleState2.ENEMYTURN;
                                StartCoroutine(EnemyTurn());

                            }

                        }
                        else
                        {
                            battleState = BattleState2.ENEMYTURN;
                            StartCoroutine(EnemyTurn());
                        }

                        DestroyHeroIndicator();
                        DestroyEnemyIndicator();
                        EncounterSystem.HeroBattleCheckers[selectedHero].UsedTurn = true;
                        heroButtons[selectedHero].SetActive(false);
                        break;

                    default:
                        break;
                }
            }
        }

        //Retreat Logic
        //Uses Vehicle Speed and Fuel to Decide on Successful Retreat
        public void runAway()
        {
            if (EncounterSystem.fuelCurrentAmount > 0)
            {
                if (battleState != BattleState2.PLAYERTURN)
                    return;
                else
                {
                    int chance = EncounterSystem.PlayerVehicle.Speed / 2;
                    int random = UnityEngine.Random.Range(1, 100);
                    if (random <= chance)
                    {
                        TravelMusic();
;
                        dialogueText.text = "You got away safely";
                        battleState = BattleState2.ENEMYTURN;
                        Debug.Log("Success");

                        EncounterSystem.fuelCurrentAmount -= 1;

                        if (EncounterSystem.fuelMaxAmount == 2)
                        {
                            nitroGauge.GetComponent<Image>().fillAmount = (float)(EncounterSystem.fuelCurrentAmount * 0.5);
                            nitroHalfCheck = 0.5;
                        }
                        else if (EncounterSystem.fuelMaxAmount == 1)
                            nitroGauge.GetComponent<Image>().fillAmount = EncounterSystem.fuelCurrentAmount * 1;

                        for (int i = 0; i < enemyBattleCheckers.Length; i++)
                        {
                            heroButtons[i].SetActive(false);
                            enemyButtons[i].SetActive(false);
                            enemyButtons[i].transform.position = enemyBSList[i].transform.position + new Vector3(0, 0.6f, 0);
                            heroButtons[i].transform.position = heroBSList[i].transform.position + new Vector3(0, 0.6f, 0);
                        }

                        MoveEnemyVehicleOutC = StartCoroutine(MoveEnemyVehicleOut());
                        MoveHeroAnimation();

                        Parallex.ShouldMove = true;

                        DestroyHeroIndicator();
                        DestroyEnemyIndicator();

                        DestroyAllHealthBars();
                        HideBattleUI();

                        clickNextBattle();

                    }
                    else   //Fail Retreating
                    {
                        battleState = BattleState2.ENEMYTURN;
                        Debug.Log("Fail");
                        StartCoroutine(EnemyTurn());
                        dialogueText.text = "There is no escape!";
                    }
                }
            }
            else
            {
                dialogueText.text = "Not enough Nitro to escape!";
                return;
            }

        }


        /// /// /// /// /// /// /// END /// /// /// /// /// /// /// ///
        //Battle WON or LOST Logic
        void EndBattle()
        {
            DestroyHeroIndicator();
            DestroyEnemyIndicator();

            HideBattleUI();

            if (battleState == BattleState2.WON)
            {
                victoryMusic.Play();
                victoryPopUp.SetActive(true);
                dialogueText.text = "You won the battle!";

            }
            else if (battleState == BattleState2.LOST)
            {
                defeatMusic.Play();
                defeatPopUp.SetActive(true);
                dialogueText.text = "You were defeated.";
            }
        }

        //Function Hides Buttons, Dialog Panel, Nitro
        private void HideBattleUI()
        {
            foreach (var item in battleElements)
            {
                item.SetActive(false);
            }
            if (Player.HasVehicle)
            {
                FuelBarBs.SetActive(false);
                FuelBackBs.SetActive(false);
                FuelTextBs.SetActive(false);
                FuelAmountBs.SetActive(false);
            }
            twoForOneButton.SetActive(false);
            blockButton.SetActive(false);
            healButton.SetActive(false);
            spearmanAbilityButton.SetActive(false);
            wizardAbilityButton.SetActive(false);
        }

        public void OpenLootPopUp()
        {
            lootPopUp.SetActive(true);
            victoryPopUp.SetActive(false);
            lootPopUp.GetComponent<LootPopUp>().GenerateLootScreen();
        }

        public void clickNextBattle()
        {
            lootPopUp.SetActive(false);

            StartCoroutine(DestroyVehicle());
        }

        IEnumerator DestroyVehicle()
        {
            enemyBSList[0].transform.SetParent(null);
            enemyBSList[1].transform.SetParent(null);
            enemyBSList[2].transform.SetParent(null);
            enemyBSList[3].transform.SetParent(null);
            enemyBSList[4].transform.SetParent(null);
            yield return new WaitForSeconds(5f);
            Destroy(instantiatedEnemyVehicle.gameObject);
            EncounterSystem.ContinueFunction = true;
        }

        //Quickly Grab All Reward Loot, Proceed to Next Encounter
        public void takeLoot()
        {
            BattleSaver.IsInABattle = false;
            BattleSaver.SaveBattle();

            MoveEnemyVehicleOutC = StartCoroutine(MoveEnemyVehicleOut());
            MoveHeroAnimation();
            Parallex.ShouldMove = true;

            selectedEnemy = 0;
            TravelMusic();
            clickNextBattle();
        }

        public IEnumerator MoveEnemyVehicleOut(float countdownValue = 5f)
        {
            Vector3 pos = new Vector3(-15f, 0.059f, -10.25f);
            while (countdownValue > 0)
            {
                try
                {
                    if (instantiatedEnemyVehicle.position != pos)
                    {

                        if (Player.HasVehicle) {

                            Vector3 newPos = Vector3.MoveTowards(instantiatedEnemyVehicle.transform.position, pos, 0.12f);
                            instantiatedEnemyVehicle.transform.position = newPos;
                        }
                        else {

                            Vector3 newPos = Vector3.MoveTowards(instantiatedEnemyVehicle.transform.position, pos, 0.09f);
                            instantiatedEnemyVehicle.transform.position = newPos;
                        }
                    }
                    else
                    {
                        countdownValue = 0;
                    }
                }
                catch (Exception) {
                    StopCoroutine(MoveEnemyVehicleOutC);
                }
                
                yield return new WaitForSeconds(.01f);
                countdownValue -= 0.01f;
            }
            StopCoroutine(MoveEnemyVehicleOutC);
        }

        public IEnumerator MoveEnemyVehicleIn(float countdownValue = 5f)
        {
            Vector3 pos = new Vector3(4.56f, 0.08f, 0f);
            while (countdownValue > 0 && instantiatedEnemyVehicle.transform.position.x > pos.x)
            {
                try
                {
                    Vector3 newPos = Vector3.MoveTowards(instantiatedEnemyVehicle.transform.position, pos, 0.12f);
                    instantiatedEnemyVehicle.transform.position = newPos;
                }
                catch (Exception) { }

                yield return new WaitForSeconds(.01f);
                countdownValue -= 0.01f;
            }

            StopCoroutine(MoveEnemyVehicleInC);
            StopHeroAimation();
            StopEnemyAnimation();

            Parallex.ShouldMove = false;
        }

        //Defeat Logic
        //Return Player to Last Town
        //Clears Inventory
        public void clickAcceptDefeat()
        {
            EncounterSystem.PersonDidHappen = false;
            EncounterSystem.DidRob = false;

            Player.LocationToTravelTo = null;
            Player.SavePlayer();

            foreach (var item in PlayerInventory.TradeableInventory)
            {
                item.Count = 0;
            }

            PlayerInventory.SavePlayerInventory();

            BattleSaver.IsInABattle = false;
            BattleSaver.IsInTravel = false;
            BattleSaver.SaveBattle();

            StartCoroutine(LoadLevel("Town"));
        }

        IEnumerator LoadLevel(string sceneName)
        {
            levelLoader.StartTransition();
            yield return new WaitForSeconds(0.5f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        private void MoveHeroAnimation()
        {
            if (Player.HasVehicle)
                EncounterSystem.InstantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "run";
            else
                EncounterSystem.InstantiatedHeroes[0].GetComponent<Animator>().SetBool("Running", true);
        }

        private void MoveEnemyAnimation()
        {
            instantiatedEnemyVehicle.GetComponent<SkeletonAnimation>().AnimationName = "run";
        }

        private void StopHeroAimation()
        {
            if (Player.HasVehicle)
                EncounterSystem.InstantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
            else
                EncounterSystem.InstantiatedHeroes[0].GetComponent<Animator>().SetBool("Running", false);
        }

        private void StopEnemyAnimation()
        {
            instantiatedEnemyVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
        }

        private void DeathEnemyAnimation()
        {
            instantiatedEnemyVehicle.GetComponent<SkeletonAnimation>().loop = false;
            instantiatedEnemyVehicle.GetComponent<SkeletonAnimation>().AnimationName = "death";
        }

        private void DeathHeroAnimation()
        {
            if (Player.HasVehicle)
            {
                EncounterSystem.InstantiatedHeroVehicle.GetComponent<SkeletonAnimation>().loop = false;
                EncounterSystem.InstantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "death";
            }
        }

        public Vehicle EnemyVehicle
        {
            get { return enemyVehicle; }
        }

        public void TravelMusic()
        {
            if (battleMusic.isPlaying)
                battleMusic.Stop();
            if (!travelMusic.isPlaying)
                travelMusic.Play();
        }

        public void BattleMusic()
        {
            if (travelMusic.isPlaying)
                travelMusic.Pause();
            if (!battleMusic.isPlaying)
                battleMusic.Play();
        }
    }
}