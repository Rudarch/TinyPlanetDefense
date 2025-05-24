using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    public AudioClip deathSound;
    public GameObject explosionEffect;

    public void PlayDeathEffects()
    {
        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position);

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
    }
}