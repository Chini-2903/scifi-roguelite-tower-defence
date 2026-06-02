using UnityEngine;
using TMPro;

public class NodeUI : MonoBehaviour
{
    public static NodeUI instance;

    public GameObject uiPanel;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI sellText;
    public GameObject upgradeButton;

    private Node targetNode;

    // We store references to all three possible script types
    private Tower towerScript;
    private TowerEMP empScript;
    private TowerStunner stunnerScript;

    void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    public void SetTarget(Node _target)
    {
        targetNode = _target;

        // Try to grab one of the three scripts
        towerScript = targetNode.tower.GetComponent<Tower>();
        empScript = targetNode.tower.GetComponent<TowerEMP>();
        stunnerScript = targetNode.tower.GetComponent<TowerStunner>();

        UpdateUI();
        uiPanel.SetActive(true);
    }

    public void Hide()
    {
        uiPanel.SetActive(false);
    }

    void UpdateUI()
    {
        int currentLevel = 0;
        int sellValue = 0;
        int nextCost = 0;

        // Logic for Standard Towers (Sentry/Sniper)
        if (towerScript != null)
        {
            currentLevel = towerScript.level;
            sellValue = towerScript.GetSellValue();
            nextCost = (currentLevel == 1) ? towerScript.upgradeCostLevel2 : towerScript.upgradeCostLevel3;
        }
        // Logic for EMP
        else if (empScript != null)
        {
            currentLevel = empScript.level;
            sellValue = empScript.GetSellValue();
            nextCost = (currentLevel == 1) ? empScript.upgradeCostLevel2 : empScript.upgradeCostLevel3;
        }
        // Logic for Stunner
        else if (stunnerScript != null)
        {
            currentLevel = stunnerScript.level;
            sellValue = stunnerScript.GetSellValue();
            nextCost = (currentLevel == 1) ? stunnerScript.upgradeCostLevel2 : stunnerScript.upgradeCostLevel3;
        }

        sellText.text = "SELL ($" + sellValue + ")";

        if (currentLevel < 3)
        {
            upgradeText.text = "UPGRADE ($" + nextCost + ")";
            upgradeButton.SetActive(true);
        }
        else
        {
            upgradeText.text = "MAX LEVEL";
            upgradeButton.SetActive(false);
        }
    }

    public void Upgrade()
    {
        if (towerScript != null) towerScript.UpgradeTower();
        else if (empScript != null) empScript.UpgradeTower();
        else if (stunnerScript != null) stunnerScript.UpgradeTower();

        BuildManager.instance.DeselectNode();
    }

    public void Sell()
    {
        int val = 0;
        if (towerScript != null) val = towerScript.GetSellValue();
        else if (empScript != null) val = empScript.GetSellValue();
        else if (stunnerScript != null) val = stunnerScript.GetSellValue();

        PlayerStats.Money += val;
        Destroy(targetNode.tower);
        targetNode.tower = null;
        BuildManager.instance.DeselectNode();
    }
}