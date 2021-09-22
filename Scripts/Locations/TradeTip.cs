using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    //shows the prices of each town in a pop up
    public class TradeTip : MonoBehaviour
    {
        private GameObject tradeTip;
        private Location hoveredLocation;                           //the location player hovers over
        [SerializeField] private Text locationNameText;
        [SerializeField] private Text[] tradePrices = new Text[10]; //the prices as text in the scene
        private Vector3 position;
        [SerializeField] private RectTransform TradeTipWindow;      //the position of the TradeTip window
        [SerializeField] private Text factionName;

        void Start()
        {
            //find tradeTip from the scene
            tradeTip = GameObject.FindGameObjectWithTag("tradeTip");
        }

        //when the player hovers over a town move tradeTip to the position
        public void OnMouseOver()
        {
            tradeTip.SetActive(true);
            locationNameText.text = name;
            foreach (var location in LocationDB.getLocationList())
            {
                if (location.LocationName == name)
                    hoveredLocation = location;
            }


            var locationName = name;
            tradeTip.SetActive(true);
            
            position = new Vector3(190f, -50f, 0f);
            TradeTipWindow.localPosition = position;

            for (int i = 0; i < hoveredLocation.TradePrices.Count; i++)
            {
                tradePrices[i].text = System.Convert.ToString((int)(hoveredLocation.TradePrices[i] - hoveredLocation.TradePrices[i] * 15/100));
            }

            //set the faction of tradeTip
            if (hoveredLocation.Territory == 1)
            {
                factionName.text = "Republic of Veden";
                factionName.color = Color.blue;
            }
            else if (hoveredLocation.Territory == 2)
            {
                factionName.text = "Fara Empire";
                factionName.color = Color.green;
            }
            else if (hoveredLocation.Territory == 3)
            {
                factionName.text = "The Kaiserreich";
                factionName.color = Color.red;
            }
            else
                factionName.text = "123 Fakestreet";
        }

        //when mouse exits the town deactivate tradeTip
        public void OnMouseExit()
        {
            tradeTip.SetActive(false);
        }


    }

}