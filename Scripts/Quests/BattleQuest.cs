using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    [System.Serializable]
    public class BattleQuest : Quest
    {
        //default constructor
        public BattleQuest() { }

        //second constructor seperated by adding an int, generates a BattleQuest by random chances
        public BattleQuest(int i)
        {
            this.questName = "Battle";
            this.questLocation = NestDB.getNest(Random.Range(0, 4));
            this.questDescription = "Defeat the " + questLocation.LocationName + " Nest";

            if (Player.CurrentLocation.Territory != questLocation.Territory)
            {
                this.questReward = Random.Range(400*((Nest)this.questLocation).Multiplier, 500*((Nest)this.questLocation).Multiplier);
                this.distanceNote = "Far Away";
            }
            else
            {
                this.questReward = Random.Range(300*((Nest)this.questLocation).Multiplier, 400*((Nest)this.questLocation).Multiplier);
                this.distanceNote = "Nearby";
            }
        }
        
        //1 argument constructor generates a quest with the given nest and random chances for the reward
        public BattleQuest(Nest questLocation)
        {
            this.questName = "Battle";
            this.questLocation = questLocation;

            Nest nest = (Nest)this.questLocation;
            this.questDescription = "Defeat the " + questLocation.LocationName + " Nest";

            if (Player.CurrentLocation.Territory != questLocation.Territory)
            {
                this.questReward = Random.Range(400*nest.Multiplier, 500*nest.Multiplier);
                this.distanceNote = "Far Away";
            }
            else
            {
                this.questReward = Random.Range(300*nest.Multiplier, 400*nest.Multiplier);
                this.distanceNote = "Nearby";
            }
        }
    }
}