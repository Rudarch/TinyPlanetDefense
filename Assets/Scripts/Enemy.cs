using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Target")]
    public Transform planetTarget;

    [Header("Movement")]
    public float jumpDistance = 1.5f;
    public float jumpInterval = 1.0f;
    public float jumpHeight = 0.5f;

    [Header("Stats")]
    public float health = 5f;
    public float damage = 1f;

    [Header("Death FX (optional)")]
    public GameObject deathEffect;

    private bool isJumping = false;
    private Vector3 jumpStart;
    private Vector3 jumpEnd;
    private float jumpProgress;

    void Start()
    {
        if (planetTarget == null)
        {
            GameObject planet = GameObject.FindWithTag("Planet");
            if (planet != null) planetTarget = planet.transform;
        }

        StartCoroutine(JumpTowardPlanet());
    }

    IEnumerator JumpTowardPlanet()
    {
        while (true)
        {
            yield return new WaitForSeconds(jumpInterval);
            if (planetTarget == null) yield break;

            Vector3 direction = (planetTarget.position - transform.position).normalized;
            jumpStart = transform.position;
            jumpEnd = jumpStart + direction * jumpDistance;
            jumpProgress = 0f;

            isJumping = true;
            while (jumpProgress < 1f)
            {
                jumpProgress += Time.deltaTime / 0.2f;
                float curved = Mathf.Sin(jumpProgress * Mathf.PI); // Arc motion
                transform.position = Vector3.Lerp(jumpStart, jumpEnd, jumpProgress) + Vector3.up * curved * jumpHeight;
                yield return null;
            }

            isJumping = false;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
            Die();
    }

    private void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Planet"))
        {
            // Deal damage to the planet (implement separately)
            Debug.Log($"Enemy hit the planet for {damage} damage");
            Destroy(gameObject);
        }
    }
}
