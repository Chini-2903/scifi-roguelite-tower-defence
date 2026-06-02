using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;

    void Start()
    {
        // Find the BuildManager once at the start of the game
        buildManager = BuildManager.instance;
    }

    public void SelectSentry()
    {
        buildManager.SelectTowerToBuild(buildManager.sentryPrefab, 50);
    }

    public void SelectSniper()
    {
        buildManager.SelectTowerToBuild(buildManager.sniperPrefab, 100);
    }

    public void SelectEMP()
    {
        buildManager.SelectTowerToBuild(buildManager.empPrefab, 150);
    }

    public void SelectStunner()
    {
        buildManager.SelectTowerToBuild(buildManager.stunnerPrefab, 200);
    }
}