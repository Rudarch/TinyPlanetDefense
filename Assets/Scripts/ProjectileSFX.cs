using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProjectileSFX : MonoBehaviour
{
    public AudioClip fireSound;

    void Start()
    {
        if (fireSound != null)
            GetComponent<AudioSource>().PlayOneShot(fireSound);
    }
}