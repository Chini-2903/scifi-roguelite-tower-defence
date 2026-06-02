using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; // Singleton so the Node can easily find it

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI healthText;

    [Header("Tooltip UI")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;

    void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    void Update()
    {
        moneyText.text = "$ " + PlayerStats.Money.ToString();
        healthText.text = "CORE: " + PlayerStats.Lives.ToString();
    }

    // The Node will call this when hovered
    public void ShowTooltip(string text)
    {
        tooltipText.text = text;
        tooltipPanel.SetActive(true);
    }

    // The Node will call this when the mouse leaves
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}