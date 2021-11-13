//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;


//namespace Sands
//{

//    public class grayscaleIcons : MonoBehaviour
//    {
//        // Start is called before the first frame update
//        private SpriteRenderer spriteRenderer;
//        private List<string> connectedLocationsNames;                           //list of all locations connected to the current location
//        [SerializeField] private GameObject[] locations = new GameObject[10];   //stores all locations from the scene
//        private List<Location> allLocationsNames;

//        void Start()
//        {
//            spriteRenderer = GetComponent<SpriteRenderer>();


//            Player.LoadPlayer();

//            //get all location names
//            allLocationsNames = LocationDB.getLocationList();

//            //all locations are grayscale by default

//            //get all the locations that are next to the current location
//            connectedLocationsNames = new List<string>();
//            foreach (var location in Player.CurrentLocation.NearbyTowns)
//            {
//                connectedLocationsNames.Add(LocationDB.getLocation(location - 1).LocationName);
//            }

//            //change the saturation/color of nearby towns
//            foreach (GameObject location in locations)
//            {

//                //current location
//                if (location.name == Player.CurrentLocation.LocationName)
//                {


//                    location.GetComponent<Image>().color = Color.cyan;
//                }
//                //connected location
//                if (connectedLocationsNames.Contains(location.name))
//                {
//                    //change grayscale to 0


//                    SetGrayscale(0);


//                }
//            }

//            currentLocation.text = Player.CurrentLocation.LocationName;
//            clickedLocationName.text = "";





//        }







//        public void SetGrayscale(float amount = 0)
//        {

//            spriteRenderer.material.SetFloat("_GrayscaleAmount", amount);
//        }

//    }

//}
