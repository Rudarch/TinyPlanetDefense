using UnityEngine;

public class AutoDestroyOnAnimationEnd : MonoBehaviour
{
    private Animator animator;
    private bool animationPlayed = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator && !animationPlayed)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
                !animator.IsInTransition(0))
            {
                Destroy(gameObject);
                animationPlayed = true;
            }
        }
    }
}
