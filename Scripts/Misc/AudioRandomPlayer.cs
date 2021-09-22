using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


public class AudioRandomPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    //play the audio randomly from the start to second 60
    void Start()
    {
        audioSource.time = Random.Range(0.0f, 60.0f);
    }
}
