using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GlobalMusicManager : MonoBehaviour
{
    public static GlobalMusicManager Inst;

    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip bossMusic;

    public float fadeDuration = 1.5f;
    public float volume = 0.5f;

    private AudioSource audioSource;
    private Coroutine fadeRoutine;
    private AudioClip currentTrack;

    //private bool isBossMusicActive = false;

    void Awake()
    {
        if (Inst != null) { Destroy(gameObject); return; }
        Inst = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 0f;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string name = scene.name.ToLower();
        if (name.Contains("mainmenu"))
            Play(menuMusic);
        else
            Play(gameMusic);
    }

    public void Play(AudioClip clip)
    {
        if (clip == null || clip == currentTrack) return;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeToClip(clip));
    }

    public void PlayBossMusic()
    {
        //isBossMusicActive = true;
        Play(bossMusic);
    }

    public void ReturnFromBoss()
    {
        //isBossMusicActive = false;
        Play(gameMusic);
    }

    public void FadeToMenuMusic()
    {
        Play(menuMusic);
    }

    IEnumerator FadeToClip(AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        // Fade out
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        // Switch and fade in
        audioSource.Stop();
        audioSource.clip = newClip;
        currentTrack = newClip;
        audioSource.Play();

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, volume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = volume;
    }
}
