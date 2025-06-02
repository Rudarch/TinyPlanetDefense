using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    private Material backgroundMaterial;
    private Vector2 offset;

    void Start()
    {
        backgroundMaterial = GetComponent<Renderer>().material;
        offset = Vector2.zero;
    }

    void Update()
    {
        offset.y += scrollSpeed * Time.deltaTime;
        backgroundMaterial.mainTextureOffset = offset;
    }
}
