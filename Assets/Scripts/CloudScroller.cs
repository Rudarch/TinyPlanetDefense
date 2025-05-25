using UnityEngine;

public class CloudScroller : MonoBehaviour
{
    public float scrollSpeed = 0.05f;
    private Material mat;
    private Vector2 offset;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        offset = Vector2.zero;
    }

    void Update()
    {
        offset.x += scrollSpeed * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}
