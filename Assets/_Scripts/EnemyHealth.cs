using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentLayers = 10;
    public int moneyPerLayer = 1; // Earn $1 for every layer you pop!
    private SpriteRenderer sr;

    // The classic BTD color scheme (1:Red, 2:Blue, 3:Green, 4:Yellow, 5:Pink, 6:Black, 7:White, 8:Gray, 9:Purple, 10:Cyan)
    private Color[] layerColors = new Color[] {
        Color.clear,                 // 0 (Dead)
        new Color(1f, 0f, 0f),       // 1 Red
        new Color(0f, 0f, 1f),       // 2 Blue
        new Color(0f, 1f, 0f),       // 3 Green
        new Color(1f, 1f, 0f),       // 4 Yellow
        new Color(1f, 0.5f, 0.8f),   // 5 Pink
        new Color(0.2f, 0.2f, 0.2f), // 6 Black
        new Color(1f, 1f, 1f),       // 7 White
        new Color(0.5f, 0.5f, 0.5f), // 8 Gray
        new Color(0.5f, 0f, 0.5f),   // 9 Purple
        new Color(0f, 1f, 1f)        // 10 Cyan
    };

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateColor();
    }

    // Now accepts INT instead of FLOAT
    public void TakeDamage(int damageAmount)
    {
        // Don't pop more layers than the enemy actually has left
        int actualDamage = Mathf.Min(damageAmount, currentLayers);

        currentLayers -= actualDamage;
        PlayerStats.Money += (actualDamage * moneyPerLayer); // Get paid per pop!

        if (currentLayers <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            UpdateColor(); // Change color immediately when dropping layers
        }
    }

    void UpdateColor()
    {
        if (currentLayers > 0 && currentLayers < layerColors.Length)
        {
            sr.color = layerColors[currentLayers];
        }
    }
}