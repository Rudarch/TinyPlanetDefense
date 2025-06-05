using System.Collections;
using UnityEngine;

public class OrbitalWingSystem : MonoBehaviour
{
    public GameObject interceptorPrefab;
    public Transform planet;
    public float launchDelay = 0.5f;
    public float spawnOffset = 0.5f;
    public int interceptorCount = 0;

    private int activeInterceptors = 0;
    private bool isLaunching = false;

    public void AddInterceptor()
    {
        interceptorCount++;

        if (!isLaunching)
            StartCoroutine(LaunchLoop());
    }

    IEnumerator LaunchLoop()
    {
        isLaunching = true;

        while (true)
        {
            if (interceptorPrefab == null)
            {
                Debug.Log("Interceptor prefab is missing.");
                yield break;
            }

            if (planet == null)
            {
                Debug.Log("Planet is not assigned.");
                yield break;
            }

            if (activeInterceptors < interceptorCount)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;

                float planetRadius = 1f;
                CircleCollider2D collider = planet.GetComponent<CircleCollider2D>();
                if (collider != null)
                {
                    planetRadius = collider.radius * Mathf.Max(planet.localScale.x, planet.localScale.y);
                }
                else
                {
                    Debug.Log("Planet collider is missing, using default radius 1f.");
                }

                Vector3 spawnPosition = planet.position + (Vector3)(randomDir * (planetRadius + spawnOffset));

                GameObject drone = Instantiate(interceptorPrefab, spawnPosition, Quaternion.identity);
                var behavior = drone.GetComponent<InterceptorDrone>();

                if (behavior != null)
                {
                    behavior.Initialize();
                    activeInterceptors++;
                    StartCoroutine(TrackInterceptorLifecycle(behavior));
                }
                else Debug.Log("InterceptorDrone component is missing on prefab.");

                yield return new WaitForSeconds(launchDelay);
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator TrackInterceptorLifecycle(InterceptorDrone drone)
    {
        while (drone != null)
        {
            yield return null;
        }

        float reload = interceptorPrefab.GetComponent<InterceptorDrone>()?.reloadTime ?? 5f;
        yield return new WaitForSeconds(reload);
        activeInterceptors--;
    }
}
