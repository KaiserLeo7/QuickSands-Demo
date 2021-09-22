using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

//COMMENTED BY FARAMARZ HOSSEINI


public class AudioFader : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        StartCoroutine(StartFade(1.5f, 1));
    }

    //fade the sound in over a duration and to a target volume
    public IEnumerator StartFade(float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        audioSource.volume = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}