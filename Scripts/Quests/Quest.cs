using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands {
    [System.Serializable]
    public class Quest {
        protected string questName;
        protected string questDescription;
        protected Location questLocation;

        protected int questReward;
        protected string distanceNote;
        protected bool completed;

        public Quest() {}


        /////////// GETTERS AND SETTERS //////////

        public string QuestName
        {
            get
            {
                return questName;
            }
            set
            {
                questName = value;
            }
        }

        public string QuestDescription {
            get {
                return questDescription;
            }
            set {
                questDescription = value;
            }
        }

        public Location QuestLocation {
            get {
                return questLocation;
            }
            set {
                questLocation = value;
            }
        }

        public int QuestReward {
            get {
                return questReward;
            }
            set {
                questReward = value;
            }
        }

        public string DistanceNote
        {
            get
            {
                return distanceNote;
            }
            set
            {
                distanceNote = value;
            }
        }

        public bool Completed
        {
            get
            {
                return completed;
            }
            set
            {
                completed = value;
            }
        }
    }
}  
