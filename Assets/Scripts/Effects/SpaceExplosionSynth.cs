using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpaceExplosionSynth : MonoBehaviour
{
    [Header("Explosion Parameters")]
    [Range(0.1f, 3f)] public float duration = 1.2f;
    [Range(0f, 1f)] public float noiseIntensity = 0.5f;
    [Range(40f, 200f)] public float bassFrequency = 80f;
    [Range(0f, 1f)] public float crackleIntensity = 0.4f;
    [Range(1f, 10f)] public float decayRate = 4f;

    private AudioSource audioSource;
    private float[] explosionSamples;
    private int sampleRate;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        sampleRate = AudioSettings.outputSampleRate;
    }

    public void GenerateExplosion()
    {
        int sampleCount = Mathf.FloorToInt(sampleRate * duration);
        explosionSamples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleRate;
            float decay = Mathf.Exp(-decayRate * t);

            float noise = Random.Range(-1f, 1f) * noiseIntensity;
            float bass = Mathf.Sin(2 * Mathf.PI * bassFrequency * t) * decay;
            float crackle = Mathf.PerlinNoise(t * 30f, 0f) * crackleIntensity;

            explosionSamples[i] = (noise + bass + crackle) * decay;
        }

        AudioClip clip = AudioClip.Create("Explosion", sampleCount, 1, sampleRate, false);
        clip.SetData(explosionSamples, 0);
        audioSource.clip = clip;
    }

    public void PlayExplosion()
    {
        GenerateExplosion();
        audioSource.Play();
    }
}
