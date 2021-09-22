using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


//starts the music from the town scene
public class MusicStarter : MonoBehaviour
{
    void Start()
    {
        try 
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<ContiniousMusic>().PlayMusic();
        }
        catch (System.Exception) { }
    }
}
