using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    [HideInInspector] public float maxScale; // The Tower will tell the pulse how big to get
    public float expandSpeed = 10f;
    public float fadeSpeed = 2f;

    private SpriteRenderer sr;
    private Color pulseColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        pulseColor = sr.color;
    }

    void Update()
    {
        // 1. Grow the circle
        transform.localScale += Vector3.one * expandSpeed * Time.deltaTime;

        // 2. Fade the circle out
        pulseColor.a -= fadeSpeed * Time.deltaTime;
        sr.color = pulseColor;

        // 3. Destroy it when it is completely invisible or reaches max size
        if (pulseColor.a <= 0f || transform.localScale.x >= maxScale)
        {
            Destroy(gameObject);
        }
    }
}