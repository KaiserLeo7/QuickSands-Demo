using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//COMMENTED BY FARAMARZ HOSSEINI



namespace Sands
{

    public class Map : MonoBehaviour
    {
        [SerializeField] private LevelLoader levelLoader;                       //used for transition between scenes
        [SerializeField] private GameObject[] locations = new GameObject[10];   //stores all locations from the scene
        [SerializeField] private GameObject[] nests = new GameObject[3];        //stores all the nests from the scene
        private List<string> connectedLocationsNames;                           //list of all locations connected to the current location
        [SerializeField] private Text clickedLocationName;
        [SerializeField] private Text currentLocation;
        private Location clickedlocation;
        private Nest clickedNest;
        private GameObject tradeTip;
        private GameObject goButton;                                            //go button from the scene
        private bool goToLocation;                                              //if true the destination is a location if false it's a nest


        public void Start()
        {
            //finding gameObjects from the scene
            tradeTip = GameObject.FindGameObjectWithTag("tradeTip");
            goButton = GameObject.FindGameObjectWithTag("goBtn");

            //setting goButton to be uninteractable
            goButton.GetComponent<Selectable>().interactable = false;

            Player.LoadPlayer();
            
            //get all the locations that are next to the current location
            connectedLocationsNames = new List<string>();
            foreach (var location in Player.CurrentLocation.NearbyTowns)
            {
                connectedLocationsNames.Add(LocationDB.getLocation(location - 1).LocationName);
            }

            //change the color of nearby towns
            foreach (GameObject location in locations)
            {
                if (location.name == Player.CurrentLocation.LocationName)
                {
                    location.GetComponent<Image>().color = new Color(0.1f, 0.7f, 0.0f);
                }
                if (connectedLocationsNames.Contains(location.name))
                {
                    location.GetComponent<Image>().color = Color.cyan;
                }
            }

            currentLocation.text = Player.CurrentLocation.LocationName;
            clickedLocationName.text = "";
        }

        public void onLocationClick()
        {
            //destination set to a location
            goToLocation = true;

            //find the clicked location
            foreach (var item in LocationDB.getLocationList())
            {
                if (item.LocationName == EventSystem.current.currentSelectedGameObject.name)
                    clickedlocation = item;
            }

            //set in interactability of goButton
            if (!connectedLocationsNames.Contains(clickedlocation.LocationName))
                goButton.GetComponent<Selectable>().interactable = false;
            else
                goButton.GetComponent<Selectable>().interactable = true;

            clickedLocationName.text = clickedlocation.LocationName;
        }

        public void NestOnClick()
        {
            //destination set to a nest
            goToLocation = false;

            //find the clicked nest
            foreach (var item in NestDB.getNestList())
            {
                if (item.LocationName + " Nest" == EventSystem.current.currentSelectedGameObject.name)
                    clickedNest = item;
            }
            Debug.Log(clickedNest.LocationName);

            //set in interactability of goButton
            if (Player.CurrentLocation.Id != clickedNest.NearbyTowns[0])
                goButton.GetComponent<Selectable>().interactable = false;
            else
                goButton.GetComponent<Selectable>().interactable = true;


            clickedLocationName.text = clickedNest.LocationName;
        }

        public void goBtnClicked()
        {
            //set the destination type
            if (goToLocation)
                Player.LocationToTravelTo = clickedlocation;
            else
                Player.LocationToTravelTo = clickedNest;

            Player.SavePlayer();

            //generate new Inn heroes
            InnHeroes.GenerateHeroes();

            //load the scene with transition
            if (goToLocation)
                StartCoroutine(LoadLevel("Travel"));
            else
                StartCoroutine(LoadLevel("Nest"));
        }

        //transition
        IEnumerator LoadLevel(string sceneName)
        {
            levelLoader.StartTransition();
            yield return new WaitForSeconds(0.5f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

    }
}