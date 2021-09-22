using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands {
    public static class HeroPartyDB {

        private static List<Hero> heroParty = new List<Hero>();

        static HeroPartyDB(){
            LoadParty();
        }
        //get database
        public static List<Hero> getHeroList() {
            return heroParty;
        }

        //get Hero at position
        public static Hero getHero(int position) {
            return heroParty[position];
        }

        //clear
        public static void clearHeroList() {
            heroParty.Clear();
        }


        //add a hero to the party
        public static void addHero(Hero hero)
        {
            heroParty.Add(hero);
        }

        //saves the data into the save file
        public static void SaveParty() {
            Debug.Log("SaveParty");
            SaveSystem.SaveParty();
        }

        //loads the data from the save file into the class
        public static void LoadParty() {
            
            PlayerData data = SaveSystem.LoadParty();
            HeroPartyDB.heroParty = new List<Hero>();
            foreach (HeroMemento hero in data.HeroParty)
            {
                if (hero.GetType().Name.Equals("WarriorMemento"))
                {
                    HeroPartyDB.heroParty.Add(new Warrior((WarriorMemento)hero));
                }
                else if (hero.GetType().Name.Equals("MageMemento"))
                {
                    HeroPartyDB.heroParty.Add(new Mage((MageMemento)hero));
                }
                else if (hero.GetType().Name.Equals("RangerMemento"))
                {
                    HeroPartyDB.heroParty.Add(new Ranger((RangerMemento)hero));
                }
                else if (hero.GetType().Name.Equals("SpearmanMemento"))
                {
                    HeroPartyDB.heroParty.Add(new Spearman((SpearmanMemento)hero));
                }
                else if (hero.GetType().Name.Equals("WizardMemento"))
                {
                    HeroPartyDB.heroParty.Add(new Wizard((WizardMemento)hero));
                }
            }
        }
    }
}