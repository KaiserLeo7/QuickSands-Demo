using UnityEngine;
using UnityEngine.Audio;

//COMMENTED BY FARAMARZ HOSSEINI


//loads the volume of each mixer for any scene it's used in
public class VolumeLoader : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    void Start()
    {
        Sands.Player.LoadPlayer();
        audioMixer.SetFloat("volume", Mathf.Log10(Sands.Player.AudioVolume) * 20);
        audioMixer.SetFloat("sfxVol", Mathf.Log10(Sands.Player.SfxVolume) * 20);
        audioMixer.SetFloat("musicVol", Mathf.Log10(Sands.Player.MusicVolume) * 20);
    }
}