using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    [System.Serializable]
    public class DeliveryQuest : Quest
    {
        private Tradeable chosenTradeable;
        //parallel array of item delivery amounts based on tradeable id
        private int[] amount = new int[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
        private int chosenAmount = 0;

        public DeliveryQuest() { }

        public DeliveryQuest(int i)
        {
            //chooses a random tradable from the database
            chosenTradeable = TradeableDatabase.getTradeable(Random.Range(0, 10));

            chosenAmount = amount[Random.Range(0, 10)];

            //if current location is the same as the chosen one find ansother
            do
            {
                questLocation = LocationDB.getLocation(Random.Range(0, 10));
            } while (questLocation.LocationName == Player.CurrentLocation.LocationName);

            this.questName = "Delivery Quest";

            //description based on other parameters
            this.questDescription = "Deliver   " + chosenAmount + "   units of   " + chosenTradeable.ItemName + "   to   " + questLocation.LocationName;

            //checks whether our locations are connected
            bool connected = false;

            //price based on the location distance
                //checks if the locations are connected
            foreach (int location in Player.CurrentLocation.NearbyTowns)
            {
                if (questLocation.Id == location)
                {
                    connected = true;
                }
            }

            //sets the price for a CONNECTED destination location 
            if (connected)
            {
                this.questReward = Random.Range(200, 301);
                this.distanceNote = "Next Town";
            }
            //sets the price for a NOT CONNECTED destination location 
            else
            {
                //if they're NOT in the same territory
                if (Player.CurrentLocation.Territory != questLocation.Territory)
                {
                    this.questReward = Random.Range(400, 501);
                    this.distanceNote = "Far Away";
                }

                //if they ARE in the same territory
                else
                {
                    this.questReward = Random.Range(300, 401);
                    this.distanceNote = "Nearby";
                }
            }
        }

        public Tradeable ChosenTradeable
        {
            get { return chosenTradeable; }
            set { chosenTradeable = value; }
        }

        public int ChosenAmount
        {
            get { return chosenAmount; }
            set { chosenAmount = value; }
        }
    }
}
