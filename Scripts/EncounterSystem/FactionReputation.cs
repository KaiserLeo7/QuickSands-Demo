using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;

namespace Sands
{
    public class FactionReputation : MonoBehaviour
    {
        [SerializeField] private Slider VedenRepGauge;
        [SerializeField] private Slider FaraRepGauge;
        [SerializeField] private Slider KaiserRepGauge;

        [SerializeField] private Text VedenStatusText;
        [SerializeField] private Text FaraStatusText;
        [SerializeField] private Text KaiserStatusText;

        private ushort[] repData;
        //Launch funtion that checks rep with each faction and updates the slider value
        //update the texts to either Ally Neutral or Hostile if between certain values

        private void Start()
        {
            repData = SaveSystem.Pdata.FactionReputation;
        }

        public void UpdateFactionReputations() {

            VedenRepGauge.GetComponent<Slider>().value = repData[0];
            FaraRepGauge.GetComponent<Slider>().value = repData[1];
            KaiserRepGauge.GetComponent<Slider>().value = repData[2];

            //Update Veden Status Text
            if (repData[0] >= 0 && repData[0] <= 149)
            {
                VedenStatusText.text = "Hostile";
                VedenStatusText.color = Color.red;
            }
            else if (repData[0] >= 150 && repData[0] <= 299)
            {
                VedenStatusText.text = "Neutral";
                VedenStatusText.color = Color.white;
            }
            else if (repData[0] >= 300 && repData[0] <= 400)
            {
                VedenStatusText.text = "Ally";
                VedenStatusText.color = Color.cyan;
            }


            //Update Fara Status Text
            if (repData[1] >= 0 && repData[1] <= 149)
            {
                FaraStatusText.text = "Hostile";
                FaraStatusText.color = Color.red;
            }
            else if (repData[1] >= 150 && repData[1] <= 299)
            {
                FaraStatusText.text = "Neutral";
                FaraStatusText.color = Color.white;
            }
            else if (repData[1] >= 250 && repData[1] <= 400)
            {
                FaraStatusText.text = "Ally";
                FaraStatusText.color = Color.cyan;
            }

            //Update Kaiser Status Text
            if (repData[2] >= 0 && repData[2] <= 149)
            {
                KaiserStatusText.text = "Hostile";
                KaiserStatusText.color = Color.red;
            }
            else if (repData[2] >= 150 && repData[2] <= 299)
            {
                KaiserStatusText.text = "Neutral";
                KaiserStatusText.color = Color.white;
            }
            else if (repData[2] >= 300 && repData[2] <= 400)
            {
                KaiserStatusText.text = "Ally";
                KaiserStatusText.color = Color.cyan;
            }
        }


    }
}