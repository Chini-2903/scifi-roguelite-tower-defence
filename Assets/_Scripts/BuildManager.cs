using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    [Header("Tower Blueprints")]
    public GameObject sentryPrefab;
    public GameObject sniperPrefab;
    public GameObject empPrefab;
    public GameObject stunnerPrefab;

    [Header("UI Elements")]
    public GameObject rangeIndicatorPrefab;
    private Node selectedNode; // Remembers which node we clicked on to upgrade

    private GameObject towerToBuild;
    private int currentTowerCost; // Remembers how much the selected tower costs

    // We removed the Start() function. No tower is selected by default now!

    public GameObject GetTowerToBuild()
    {
        return towerToBuild;
    }

    public int GetTowerCost()
    {
        return currentTowerCost;
    }

    // The Shop will call this function and pass in the specific tower and its price
    public void SelectTowerToBuild(GameObject tower, int cost)
    {
        towerToBuild = tower;
        currentTowerCost = cost;
        Debug.Log("Selected tower: " + tower.name + " | Cost: " + cost);
    }

    public void SelectNode(Node node)
    {
        selectedNode = node;
        towerToBuild = null; // Deselect shop items if we click a built tower
        Debug.Log("Node Selected! Time to open the Upgrade UI.");
        NodeUI.instance.SetTarget(node); // SHOW THE MENU!
    }

    public void DeselectNode()
    {
        selectedNode = null;
        NodeUI.instance.Hide(); // HIDE THE MENU!
    }
}