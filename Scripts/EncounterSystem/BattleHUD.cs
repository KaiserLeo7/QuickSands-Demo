using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands
{
    public class BattleHUD : MonoBehaviour
    {

        public Slider hpSlider;        //slider for tracking hero HP
        public Slider lossSlider;       //slider for tracking hero HP Loss
        public Gradient gradient;       //hero HP Color
        public Image lossFill;          //tracks hero HP
        public Image healthBarFill;     //tracks hero HP Loss
        public static bool shouldFlash = true;  //determines if lossFill Flashes


        private void Start()
        {
            StartCoroutine(Fading());
        }

        //Update the playerHUD to be the health of a hero
        public void SetHUDUnitHPHero(Hero hero)
        {
            hpSlider.maxValue = hero.MaxHP;
            hpSlider.value = hero.CurrentHP;
            lossSlider.maxValue = hero.MaxHP;
            lossSlider.value = hero.CurrentHP;

            lossFill.color = gradient.Evaluate(hpSlider.normalizedValue);
        }

        //Update the enemyHUD to be the health of an enemy
        public void SetHUDUnitHPEnemy(Enemy enemy)
        {
            hpSlider.maxValue = enemy.MaxHP;
            hpSlider.value = enemy.CurrentHP;
            lossSlider.maxValue = enemy.MaxHP;
            lossSlider.value = enemy.CurrentHP;

            lossFill.color = gradient.Evaluate(1f);
        }

        //Updates the HP of the BattleHUD
        public void SetHP(int hp)
        {
            hpSlider.value = hp;
            lossSlider.value = hp;

            lossFill.color = gradient.Evaluate(hpSlider.normalizedValue);
        }

        //Updates the HP Loss of the BattleHUD
        public void SetLoss(int hp)
        {
            lossSlider.value = hp;
        }

        //Controls the Red Flashing of the BattleHUD
        public IEnumerator Fading()
        {
            while (true)
            {
                yield return new WaitForSeconds(.1f);
                while (true)
                {
                    yield return new WaitForSeconds(.2f);
                    if (shouldFlash)
                    {
                        if (healthBarFill.color.a > 0)
                        {
                            Color temp = healthBarFill.color;
                            temp.a -= 5f;
                            healthBarFill.color = temp;
                        }
                        else
                        {
                            Color temp = healthBarFill.color;
                            temp.a += 5f;
                            healthBarFill.color = temp;
                        }
                    }
                }
            }
        }
    }
}
