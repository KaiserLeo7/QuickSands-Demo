using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//COMMENTED BY FARAMARZ HOSSEINI


public class SaveFileCheck : MonoBehaviour
{
    //checks if there is a save file to activate the continue button
    void Start()
    {
        if(File.Exists(Application.persistentDataPath + "/player.savefile"))
            GameObject.FindGameObjectWithTag("continueBtn").SetActive(true);
        
        else
            GameObject.FindGameObjectWithTag("continueBtn").SetActive(false);
    }

    //if there is no save file deactivate save warning
    public void saveWarningTest()
    {
      if (!File.Exists(Application.persistentDataPath + "/player.savefile"))
      {
          GameObject.FindGameObjectWithTag("SaveWarning").SetActive(false);
      }
    }


}
