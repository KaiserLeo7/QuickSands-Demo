using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Spine.Unity;
using TMPro;

namespace Sands
{
    public class DamagePopUp : MonoBehaviour
    {
       
        //Creates a damage pop up at the given position with the amount of damage
        public static DamagePopUp Create(Vector3 position, int damageAmount, bool isCriticalHit)

        {
            GameObject damagePopUpPrefab = (GameObject)Resources.Load("DamagePopUp", typeof(GameObject));

            Transform damagePopupTransform = Instantiate(damagePopUpPrefab.transform, position, Quaternion.identity);

            DamagePopUp damagePopUp = damagePopupTransform.GetComponent<DamagePopUp>();

            //Updates how the damage pop up should look like
            damagePopUp.Setup(damageAmount, isCriticalHit);

            return damagePopUp;

        }

        private TextMeshPro textMesh;
        private float disappearTimer;   //the pop up disapears after this timer ends
        private Color textColor;

        private void Awake()
        {
            textMesh = transform.GetComponent<TextMeshPro>();
        }

        //Sets up the looks of the damage pop up
        public void Setup(int damageAmount, bool isCriticalHit) {
           
            textMesh.SetText(damageAmount.ToString());

            //if it's crit damage make it red
            if (!isCriticalHit)
            {
                //Normal hit
                textMesh.fontSize = 30;
                textColor = Color.yellow;
            }
            else {
                //Critical
                textMesh.fontSize = 36;
                textColor = Color.red;
            }

            textMesh.color = textColor;
            disappearTimer = 0.5f;
        }

        //move the damage pop up upwards and make it disapear
        private void Update()
        {
            float moveYspeed = 3.5f;
            transform.position += new Vector3(0, moveYspeed) * Time.deltaTime;

            disappearTimer -= Time.deltaTime;
            if(disappearTimer < 0)
            {
                // Start disappearing
                float disappearSpeed = 5f;
                textColor.a -= disappearSpeed * Time.deltaTime;
                textMesh.color = textColor;
                
                if (textColor.a < 0)
                {
                    Destroy(gameObject);
                }
            }
        }

    }
}