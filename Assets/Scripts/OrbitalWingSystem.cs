using System.Collections;
using UnityEngine;

public class OrbitalWingSystem : MonoBehaviour
{
    public GameObject interceptorPrefab;
    public Transform spawnPoint;
    public float launchDelay = 0.5f;

    private int interceptorCount = 0;
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

            if (spawnPoint == null)
            {
                Debug.Log("Spawn point is not assigned.");
                yield break;
            }

            if (activeInterceptors < interceptorCount)
            {
                GameObject drone = Instantiate(interceptorPrefab, spawnPoint.position, Quaternion.identity);
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
