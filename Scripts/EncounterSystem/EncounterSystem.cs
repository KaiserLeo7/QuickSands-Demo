using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Spine.Unity;
using UnityEngine.Rendering;

//COMMENTED BY FARAMARZ HOSSEINI

namespace Sands
{
    public class EncounterSystem : MonoBehaviour
    {
        private int[] encounters;                                       //stores all the encounters
        [SerializeField] private BattleSystem2 battleSystem;            //a reference to BattleSystem in the scene
        [SerializeField] private EmptyIdle emptyIdle;                   //a reference to EmptyIdle in the scene
        [SerializeField] private PersonEncounter personEncounter;       //a reference to PersonEncounter in the scene
        [SerializeField] private TreasureEncounter treasureEncounter;   //a reference to TreasureEncounter in the scene
        [SerializeField] private PartyEncounter partyEncounter;         //a reference to PartyEncounter in the scene
        [SerializeField] private LevelLoader levelLoader;               //adds a fading transition to the scene
        [SerializeField] private GameObject[] backgrounds;              //stores all the background objects in the scene

        private static bool continueFunction;                           //when it becomes true the next encounter loads in
        private static bool wasInABattle;                               //checks if the game was closed during an encounter

        public static EncounterSystem instance;                         //makes EncounterSystem a singleton

        //Encounters available
        private int numberOfEncounters = 5;                             //the number of available encounters
        private int current = 0;                                        //the current running encounter
        private List<CharAction> charactersActions;                     //stores all the CharAction components of the heroes
        private static List<CharAction> playersCharActions;
        private static Vehicle playerVehicle = new Vehicle();           
        private GameObject[] heroPrefabs;                               //stores all the hero prefabs
        private GameObject heroVehiclePrefab;
        private static Transform[] instantiatedHeroes;                  //stores all the instantiated heroes
        private static Transform instantiatedHeroVehicle;               
        private static BattleChecker[] heroBattleCheckers;              //stores all the BattleChecker components pf the instantiated heroes
        //Object Transform Hero positions on Vehicles
        [SerializeField] private Transform[] heroBSList = new Transform[5];     //stores the position of all the heroes
        [SerializeField] private Transform HeroVehicleBS;               //stores the position of player's vehicle
        public static bool PersonDidHappen { get; set; } = false;       //checks if the person encounter has happened or not
        public static bool DidRob { get; set; } = false;                //checks if the player robbed someone in person encounter

        public static float fuelMaxAmount;
        public static float fuelCurrentAmount { get; set; }

        public void Start(){
            //create the singleton
            instance = this;

            wasInABattle = BattleSaver.IsInABattle;
            SetBackground();
            StartCoroutine(StartTiming());

            ResetTownMusic();
        }

        void ResetTownMusic()
        {
            try
            {
                GameObject.FindGameObjectWithTag("Music").GetComponent<ContiniousMusic>().ResetMusic();
            }
            catch (Exception)
            { }
        }

        //Sets up the Travel scene with precise timing
        private IEnumerator StartTiming()
        {
            //if the player quit the the middle of the travel scene load all the encounters and put them where they left off
            if (BattleSaver.IsInTravel)
            {
                encounters = BattleSaver.Encounters;
                current = BattleSaver.CurrentEncounter;
            }
            else
            {
                //setup all the encounters and save them
                ChooseEncounters();
                BattleSaver.Encounters = encounters;
            }

            //instantiate the player's vehicle and all the heroes
            playersCharActions = InstantiateParty();

            if (BattleSaver.IsInTravel)
                ArrangeHeroes();

            MoveHeroAimation();

            StartCoroutine(MoveHeroVehicleIn());

            yield return new WaitForSeconds(2.5f);

            continueFunction = true;
            StartCoroutine(RunCheck());
        }
        
        //run the fucntion RunEncounters countiniously every .2 seconds
        IEnumerator RunCheck() 
        {
            for(;;) 
            {
                RunEncounters();
                yield return new WaitForSeconds(.2f);
            }
        }

        //0 -> idle
        //1 -> treasure
        //2 -> person interaction
        //3 -> faction interaction
        //4 -> party interaction

        //sets up all the encounters based on the probablity of each happening
        public void ChooseEncounters(){

            int indexNo = 8;
        
            encounters = new int[indexNo];

            int battleCounter = 0;

            for (int i = 0; i < indexNo; i++)
            {
                if (i!=0) {
                    do {
                        encounters[i] = UnityEngine.Random.Range(0, numberOfEncounters);

                        if (encounters[i] == 3)
                            battleCounter++;

                        //makes sure enough battles happen
                        if (i == 2 && battleCounter == 0) {

                            encounters[i] = 3;
                            battleCounter++;
                        }

                        else if (i == 5 && battleCounter <= 2)
                        {
                            encounters[i] = 3;
                            battleCounter++;
                        }
                        else if (i == 8 && battleCounter <= 4)
                        {
                            encounters[i] = 3;
                            battleCounter++;

                        }

                    } while (encounters[i] == encounters[i - 1]);
                } else {

                    encounters[0] = UnityEngine.Random.Range(0, numberOfEncounters);

                    if (encounters[0] == 3)
                        battleCounter++;
                }
            }
        }


        //Runs the encounters as long as it's called in RunCheck
        public void RunEncounters(){
            //as soon as continueFunction becomes true moves to the next encounter
            if (continueFunction && current < encounters.Length) {
                //saves the encounter on spicified ones
                if (encounters[current] != 1 && encounters[current] != 2 && encounters[current] != 3)
                    SaveBattleData();
                switch (encounters[current])
                {
                    case 0:
                        current++;
                        emptyIdle.Begin();
                        break;

                    case 1:
                        current++;
                        treasureEncounter.Begin();
                        break;

                    case 2:
                        current++;
                        personEncounter.Begin();
                        PersonDidHappen = true;
                        break;

                    case 3:
                        current++;
                        battleSystem.Begin();
                        break;

                    case 4:
                        current++;
                        partyEncounter.Begin();
                        break;
                }

                continueFunction = false;
                if (encounters[current - 1] == 2)
                    SaveBattleData();
            }

            //if all the encounters are done move the vehicle out of the scene
            else if(continueFunction && current == encounters.Length )
            {
                continueFunction = false;
                PersonDidHappen = false;
                DidRob = false;
                Player.LoadPlayer();
                Player.CurrentLocation = Player.LocationToTravelTo;
                Player.LocationToTravelTo = null;
                Player.SavePlayer();
                PlayerInventory.SavePlayerInventory();
                Parallex.ShouldMove = false;

                BattleSaver.IsInTravel = false;
                BattleSaver.SaveBattle();

                StartCoroutine(MoveHeroVehicleOut());
            }
        }

        //manages the health and existance if the player closed the game in the middle of an encounter
        private void ArrangeHeroes()
        {
            for (int i = 0; i < heroBattleCheckers.Length; i++)
            {
                heroBattleCheckers[i].UsedTurn = BattleSaver.HeroBattleCheckers[i].UsedTurn;
                heroBattleCheckers[i].IsDead = BattleSaver.HeroBattleCheckers[i].IsDead;

                instantiatedHeroes[i].GetComponent<Hero>().CurrentHP = BattleSaver.HeroHPs[i];

                if (heroBattleCheckers[i].IsDead)
                    Destroy(instantiatedHeroes[i].gameObject);
            }
        }

        //saves all the battle data needed to put the player back where they left off
        public void SaveBattleData()
        {
            BattleSaver.HeroBattleCheckers = new List<BattleCheckerMemento>();
            BattleSaver.HeroHPs = new List<int>();

            for (int i = 0; i < heroBattleCheckers.Length; i++)
            {
                BattleCheckerMemento battleChecker = new BattleCheckerMemento();

                battleChecker.UsedTurn = heroBattleCheckers[i].UsedTurn;
                battleChecker.IsDead = heroBattleCheckers[i].IsDead;

                BattleSaver.HeroBattleCheckers.Add(battleChecker);

                if (!heroBattleCheckers[i].IsDead)
                    BattleSaver.HeroHPs.Add(instantiatedHeroes[i].GetComponent<Hero>().CurrentHP);
                else
                    BattleSaver.HeroHPs.Add(0);
            }

            BattleSaver.IsInTravel = true;
            BattleSaver.CurrentEncounter = current;
            BattleSaver.SaveBattle();
            print("Battle Saved");
        }
        
        //moves the player's hero or vehicle out of the scene to the right at the proper speed and loads the town scene
        public IEnumerator MoveHeroVehicleOut(float countdownValue = 5f)
        {
            Vector3 pos = new Vector3(10.5f, 0.059f, 0f);
            while (countdownValue > 0)
            {
                try
                {
                    if (Player.HasVehicle)
                    {
                        if (instantiatedHeroVehicle.position != pos)
                        {
                            Vector3 newPos = Vector3.MoveTowards(instantiatedHeroVehicle.transform.position, pos, 0.12f);
                            instantiatedHeroVehicle.transform.position = newPos;
                        }
                                
                    }
                    else
                    {
                        if (instantiatedHeroes[0].position != pos)
                        {
                            Vector3 newPos = Vector3.MoveTowards(instantiatedHeroes[0].transform.position, pos, 0.09f);
                            instantiatedHeroes[0].transform.position = newPos;
                        }
                    }
                }
                catch (Exception) { }

                yield return new WaitForSeconds(.01f);

                if (Player.HasVehicle)
                    countdownValue -= 0.03f;
                else
                    countdownValue -= 0.015f;
            }

            levelLoader.StartTransition();
            yield return new WaitForSeconds(0.5f);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
        }

        //Instantiates all the heroes and the vehicle into the scene
        private List<CharAction> InstantiateParty(){
            charactersActions = new List<CharAction>();
     
            //add Hero Battle Stations to a List

            HeroPartyDB.LoadParty();
            heroBattleCheckers = new BattleChecker[HeroPartyDB.getHeroList().Count];

            //IF PLAYER HAS VEHICLE
            if (SaveSystem.Pdata.HasVehicle)
            {
                Player.LoadPlayer();
                playerVehicle = Player.CurrentVehicle;
                fuelMaxAmount = PlayerVehicle.Fuel;
                fuelCurrentAmount = fuelMaxAmount;

                //positioning and instantiation of hero vehicle based on the type
                switch (playerVehicle.Name)
                {
                    case "Scout":
                        heroVehiclePrefab = (GameObject)Resources.Load("Scout2Vehicle", typeof(GameObject));

                        instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                        heroBSList[0].transform.position = new Vector3(-4.164998f, 1.404377f, 0f);
                        heroBSList[1].transform.position = new Vector3(-3.156f, 1.36614f, 0f);
                        break;
                    case "Warthog":
                        heroVehiclePrefab = (GameObject)Resources.Load("Warthog3Vehicle", typeof(GameObject));

                        instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                        heroBSList[0].transform.position = new Vector3(-2.3f, 2.040558f, 0f);
                        heroBSList[1].transform.position = new Vector3(-3.5688f, 1.428f, 0f);
                        heroBSList[2].transform.position = new Vector3(-4.55f, 1.395107f, 0f);
                        break;
                    case "Goliath":
                        heroVehiclePrefab = (GameObject)Resources.Load("Goliath4Vehicle", typeof(GameObject));
                        
                        instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                        heroBSList[0].transform.position = new Vector3(-2.17f, 1.963147f, 0f);
                        heroBSList[1].transform.position = new Vector3(-2.98f, 2.001312f, 0f);
                        heroBSList[2].transform.position = new Vector3(-4.1f, 2.291212f, 0f);
                        heroBSList[3].transform.position = new Vector3(-4.890064f, 2.324204f, 0f);
                        break;
                    case "Leviathan":
                        heroVehiclePrefab = (GameObject)Resources.Load("Leviathan5Vehicle", typeof(GameObject));

                        instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                        heroBSList[0].transform.position = new Vector3(-1.409f, 1.258098f, 0f);
                        heroBSList[1].transform.position = new Vector3(-2.362f, 1.3f, 0f);
                        heroBSList[2].transform.position = new Vector3(-3.44f, 2.119831f, 0f);
                        heroBSList[3].transform.position = new Vector3(-4.177f, 2.119828f, 0f);
                        heroBSList[4].transform.position = new Vector3(-4.94f, 2.119828f, 0f);
                        break;
                    
                    default:
                    break;
                }

                //set the sorting layer to 7
                heroVehiclePrefab.GetComponent<SortingGroup>().sortingOrder = 7;

                //set the animation to idle
                instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";


                heroPrefabs = new GameObject[HeroPartyDB.getHeroList().Count];
                instantiatedHeroes = new Transform[HeroPartyDB.getHeroList().Count];


                //run through prefabResources
                //load the prefabs from Resources folder into the prefabResources list based on the length of DB

                //characterTransform is an array that gives us access to the prefabs that we instantiated inside the scene
                //run through characterTransform list based on its length
                for (int i = 0; i < heroPrefabs.Length && i < playerVehicle.PartySize; i++)
                {
                    //using the name of the prefab based on name of hero inside DB
                    heroPrefabs[i] = (GameObject)Resources.Load(HeroPartyDB.getHero(i).GetType().Name, typeof(GameObject));
                    HeroPartyDB.getHero(i).setSkin(heroPrefabs[i]);

                    //cahnging the sorting layer of the heroes before instantiating them
                    heroPrefabs[i].GetComponent<SortingGroup>().sortingOrder = 6;

                    //takes prefab from prefabResources at its current iteration, that same prefabs position and rotation
                    //gives us access to those prefabs
                    instantiatedHeroes[i] = Instantiate(heroPrefabs[i].transform, heroBSList[i].position, Quaternion.identity);
                    instantiatedHeroes[i].GetComponent<Hero>().MaxHP += ArmorDatabase.getArmor(HeroPartyDB.getHero(i).SkinTire - 1).Health;
                    instantiatedHeroes[i].GetComponent<Hero>().CurrentHP += ArmorDatabase.getArmor(HeroPartyDB.getHero(i).SkinTire - 1).Health;
                    instantiatedHeroes[i].GetComponent<Hero>().Damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(i).SkinTire - 1).Damage;
                    playerVehicle.addPassangers(instantiatedHeroes[i]);

                    //Adding instantiated Characters to playerUnit list 
                    charactersActions.Add(instantiatedHeroes[i].GetComponent<CharAction>());
                    charactersActions[i].PlayIdleAnim();

                    //store all the battleCheckers
                    heroBattleCheckers[i] = instantiatedHeroes[i].GetComponent<BattleChecker>();

                    //set the parent of instantiated Heroes to the vehicle
                    instantiatedHeroes[i].SetParent(instantiatedHeroVehicle);

                    //adding achived buffs to the heroes
                    instantiatedHeroes[i].GetComponent<HeroBuff>().AddBuffs(true);
                }

                instantiatedHeroVehicle.position -= new Vector3(10, 0, 0);
            }
            //if there is no vehicle do the same for a single hero
            else 
            {
                heroBSList[0].transform.position = new Vector3(-5f, 0.1f, 0f);
                heroPrefabs = new GameObject[1];

                
                heroPrefabs[0] = (GameObject)Resources.Load(HeroPartyDB.getHero(0).GetType().Name, typeof(GameObject));
                HeroPartyDB.getHero(0).setSkin(heroPrefabs[0]);
                instantiatedHeroes = new Transform[1];
                
                instantiatedHeroes[0] = Instantiate(heroPrefabs[0].transform, heroBSList[0].position, Quaternion.identity);

                Debug.Log(instantiatedHeroes[0].GetComponent<Hero>().MaxHP  + "Max HERO HP when instantiated" );

                charactersActions.Add(instantiatedHeroes[0].GetComponent<CharAction>());
                
                charactersActions[0].PlayIdleAnim();
                heroBattleCheckers[0] = instantiatedHeroes[0].GetComponent<BattleChecker>();

                instantiatedHeroes[0].position -= new Vector3(4, 0, 0);
            }
            return charactersActions;
        }

        //moves the hero vehicle into the scene at the proper speed
        public IEnumerator MoveHeroVehicleIn(float countdownValue = 5f)
        {

            while (countdownValue > 0)
            {
                try
                {
                    if (Player.HasVehicle)
                    {
                        if (instantiatedHeroVehicle.position != HeroVehicleBS.position)
                        {
                            Vector3 newPos = Vector3.MoveTowards(instantiatedHeroVehicle.transform.position, HeroVehicleBS.position, 0.12f);
                            instantiatedHeroVehicle.transform.position = newPos;
                        }
                        else
                            countdownValue = 0;

                    }
                    else
                    {
                        if (instantiatedHeroes[0].position != heroBSList[0].position)
                        {
                            Vector3 newPos = Vector3.MoveTowards(instantiatedHeroes[0].transform.position, heroBSList[0].position, 0.09f);
                            instantiatedHeroes[0].transform.position = newPos;
                        }
                        else
                            countdownValue = 0;
                    }
                }
                catch (Exception) { }

                yield return new WaitForSeconds(.01f);

                if (Player.HasVehicle)
                    countdownValue -= 0.03f;
                else
                    countdownValue -= 0.015f;
            }

            Parallex.ShouldMove = true;
        }

        //based on the availability of a vehicle it runs the right animation
        private void MoveHeroAimation()
        {
            if (Player.HasVehicle)
                EncounterSystem.InstantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "run";
            else
                EncounterSystem.InstantiatedHeroes[0].GetComponent<Animator>().SetBool("Running", true);
        }
        
        //sets the background of the scene based on the current faction
        private void SetBackground() {

            switch (Player.LocationToTravelTo.Territory)
            {
                case 1:
                    backgrounds[0].SetActive(true);
                    backgrounds[1].SetActive(false);
                    backgrounds[2].SetActive(false);
                    break;

                case 2:
                    backgrounds[2].SetActive(true);
                    backgrounds[0].SetActive(false);
                    backgrounds[1].SetActive(false);
                    break;

                case 3:
                    backgrounds[1].SetActive(true);
                    backgrounds[0].SetActive(false);
                    backgrounds[2].SetActive(false);
                    break;
                default:

                    break;
            }
        }

        public static bool ContinueFunction{
            get{
                return continueFunction;
            }
            set{
                continueFunction = value;
            }
        }

        public static bool WasInABattle
        {
            get
            {
                return wasInABattle;
            }
            set
            {
                wasInABattle = value;
            }
        }

        public static Vehicle PlayerVehicle{
            get{
                return playerVehicle;
            }
            set{
                playerVehicle = value;
            }
        }

        public static Transform[] InstantiatedHeroes{
            get{
                return instantiatedHeroes;
            }
            set{
                instantiatedHeroes = value;
            }
        }

        public static List<CharAction> PlayersCharActions{
            get{
                return playersCharActions;
            }
            set{
                playersCharActions = value;
            }
        }

        public static Transform InstantiatedHeroVehicle{
        
            get{
                return instantiatedHeroVehicle;
            }
            set{
                instantiatedHeroVehicle = value;
            }

        }

        public static BattleChecker[] HeroBattleCheckers{
            get{
                return heroBattleCheckers;
            }
            set{
                heroBattleCheckers = value;
            }
        }
    }
}
