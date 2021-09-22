using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


//stops the music from the town scene
public class MusicStopper : MonoBehaviour
{
    void Start()
    {
        try
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<ContiniousMusic>().StopMusic();
        }
        catch (System.Exception) { }
    }
}
