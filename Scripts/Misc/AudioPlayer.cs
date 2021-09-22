using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioSource attackAudioSource;     //audio of a hero or an enemy attacking
    [SerializeField] AudioSource abilityAudioSource;    //audio of a hero or an enemy using their ability

    public void PlayAttack()
    {
        attackAudioSource.Play();
    }

    public void PlayAbility()
    {
        abilityAudioSource.Play();
    }
}
