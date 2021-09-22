using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands

{
    public class CharacterSelect : MonoBehaviour
    {
        [SerializeField] GameObject MainHeroBS;     //the position of the selected hero
        [SerializeField] GameObject Start;

        private GameObject instantiatedCurrentHero; //stores the instantiated selected hero
        private GameObject hero;

        private int selectedHero = 0;

        //selects the chosen hero as a permanent choice
        public void SelectHeroButton() {

            switch (selectedHero)
            {
                case 0:
                    addWarrior();
                    break;

                case 1:
                    addMage();
                    break;

                case 2:
                    addRanger();
                    break;

                case 3:
                    addSpellcaster();
                    break;

                case 4:
                    addSpearman();
                    break;
            }

            SetSavedVolumes();

            Player.SavePlayer();
        }

        //sets the saved volumes after creating a new game
        void SetSavedVolumes()
        {
            Player.AudioVolume = SettingsMenu.SavedVolume;
            Player.SfxVolume = SettingsMenu.SavedSfxVolume;
            Player.MusicVolume = SettingsMenu.SavedMusicVolume;
        }

        //Instantiate Selected Hero to GameObject transform
        public void InstantiateWarrior()
        {
            Start.SetActive(true);
            selectedHero = 0;
            hero = (GameObject)Resources.Load("Warrior", typeof(GameObject));
            instantiatedCurrentHero = Instantiate(hero, MainHeroBS.transform.position, Quaternion.identity);
            instantiatedCurrentHero.transform.localScale = new Vector3(150, 150, 150);
        }
        public void InstantiateMage()
        {
            Start.SetActive(true);
            selectedHero = 1;
            hero = (GameObject)Resources.Load("Mage", typeof(GameObject));
            instantiatedCurrentHero = Instantiate(hero, MainHeroBS.transform.position, Quaternion.identity);
            instantiatedCurrentHero.transform.localScale = new Vector3(150, 150, 150);
           
        }

        public void InstantiateRanger()
        {
            Start.SetActive(true);
            selectedHero = 2;
            hero = (GameObject)Resources.Load("Ranger", typeof(GameObject));
            instantiatedCurrentHero = Instantiate(hero, MainHeroBS.transform.position, Quaternion.identity);
            instantiatedCurrentHero.transform.localScale = new Vector3(150, 150, 150);

        }

        public void InstantiateSpellcaster()
        {
            Start.SetActive(true);
            selectedHero = 3;
            hero = (GameObject)Resources.Load("Wizard", typeof(GameObject));
            instantiatedCurrentHero = Instantiate(hero, MainHeroBS.transform.position, Quaternion.identity);
            instantiatedCurrentHero.transform.localScale = new Vector3(150, 150, 150);
        }

        public void InstantiateSpearman()
        {
            Start.SetActive(true);
            selectedHero = 4;
            hero = (GameObject)Resources.Load("Spearman", typeof(GameObject));
            instantiatedCurrentHero = Instantiate(hero, MainHeroBS.transform.position, Quaternion.identity);
            instantiatedCurrentHero.transform.localScale = new Vector3(150, 150, 150);
        }

        public void DestroyHero() {

            Destroy(instantiatedCurrentHero);
        }

        //add warrior class to party database
        public void addWarrior()
        {
            HeroPartyDB.addHero(new Warrior((Warrior)HeroClassDB.getHero(0)));            
            Debug.Log("Added Warrior");
            HeroPartyDB.SaveParty();
            
        }

        //add mage class to party database
        public void addMage()
        {
            HeroPartyDB.addHero(new Mage((Mage)HeroClassDB.getHero(1))); 
            Debug.Log("Added Mage");
            HeroPartyDB.SaveParty();        
        }

        // add ranger class to party database
        public void addRanger()
        {
            HeroPartyDB.addHero(new Ranger((Ranger)HeroClassDB.getHero(2))); 
            HeroPartyDB.SaveParty();
            Debug.Log("Added Ranger");
        }

        public void addSpellcaster()
        {
            HeroPartyDB.addHero(new Wizard((Wizard)HeroClassDB.getHero(3)));
            HeroPartyDB.SaveParty();
            Debug.Log("Added Spellcaster");
        }

        public void addSpearman()
        {
            HeroPartyDB.addHero(new Spearman((Spearman)HeroClassDB.getHero(4)));
            HeroPartyDB.SaveParty();
            Debug.Log("Added Spearman");
        }
    }
}
