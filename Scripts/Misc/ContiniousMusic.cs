using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


//allows keeping the music in the Town Scene to continue into the other scenes
public class ContiniousMusic : MonoBehaviour
{
    private AudioSource audioSource;
    private GameObject[] other;         //the other objects with the Music tag
    private bool notFirst = false;      //if true there are other objects with the Music tag in th scene
    private void Awake()
    {
        other = GameObject.FindGameObjectsWithTag("Music");

        foreach (GameObject oneOther in other)
        {
            if (oneOther.scene.buildIndex == -1)
            {
                notFirst = true;
            }
        }

        if (notFirst == true)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (audioSource.isPlaying) return;
            audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Pause();
    }

    public void ResetMusic()
    {
        audioSource.time = 0.0f;
    }
}