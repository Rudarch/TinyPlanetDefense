using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    public float buffer = 0.1f;

    void Start()
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            float duration = anim.GetCurrentAnimatorStateInfo(0).length;
            Destroy(gameObject, duration + buffer);
        }
    }
}
