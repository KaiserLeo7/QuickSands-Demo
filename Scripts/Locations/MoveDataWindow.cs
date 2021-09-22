using System.Collections;
using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


public class MoveDataWindow : MonoBehaviour
{
    [SerializeField] private RectTransform TradeTipWindow;
    [SerializeField] private RectTransform MapWindow;
    [SerializeField] private Camera cam;
    private GameObject tradeTip;

    private void Start(){
        StartCoroutine(Deactivate());
    }

    //set tradetip inactive after 0.2 seconds to have it filled in first
    IEnumerator Deactivate(){
        tradeTip = GameObject.FindGameObjectWithTag("tradeTip");
        yield return new WaitForSeconds(0.2f);
        tradeTip.SetActive(false);
    }

   

}

