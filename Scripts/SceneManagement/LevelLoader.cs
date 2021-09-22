using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator animator;

    //starts the transition
    public void StartTransition()
    {
        animator.SetTrigger("Start");
    }
}
