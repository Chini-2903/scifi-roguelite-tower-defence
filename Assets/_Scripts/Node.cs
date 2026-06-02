using UnityEngine;

public class Node : MonoBehaviour
{
    [HideInInspector]
    public GameObject tower;
    private GameObject rangeIndicator;

    void Start()
    {
        GameObject indicatorPrefab = BuildManager.instance.rangeIndicatorPrefab;
        rangeIndicator = Instantiate(indicatorPrefab, transform.position, Quaternion.identity);
        rangeIndicator.transform.SetParent(transform);
        rangeIndicator.SetActive(false);
    }

    void OnMouseEnter()
    {
        float targetRange = 0f;
        int targetLevel = 0;
        string towerName = "";

        if (tower != null)
        {
            targetRange = GetRangeFromObject(tower, out targetLevel, out towerName);
        }
        else if (BuildManager.instance.GetTowerToBuild() != null)
        {
            targetRange = GetRangeFromObject(BuildManager.instance.GetTowerToBuild(), out targetLevel, out towerName);
        }

        if (targetRange > 0f)
        {
            // 1. Reset scale to 1 to get the pure, unscaled sprite bounds
            rangeIndicator.transform.localScale = Vector3.one;
            float rawSpriteSize = rangeIndicator.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

            // 2. Calculate the exact multiplier for our target diameter (range * 2)
            float targetScale = (targetRange * 2f) / rawSpriteSize;

            // 3. Apply it, but actively divide by the Node's scale so it doesn't get stretched!
            rangeIndicator.transform.localScale = new Vector3(
                targetScale / transform.localScale.x,
                targetScale / transform.localScale.y,
                1f
            );

            rangeIndicator.SetActive(true);
            UIManager.instance.ShowTooltip("Lvl " + targetLevel + " " + towerName + "\nRange: " + targetRange);
        }
    }

    void OnMouseExit()
    {
        rangeIndicator.SetActive(false);
        UIManager.instance.HideTooltip(); // Hide UI when mouse leaves
    }

    void OnMouseDown()
    {
        if (tower != null)
        {
            BuildManager.instance.SelectNode(this);
            return;
        }

        GameObject towerToBuild = BuildManager.instance.GetTowerToBuild();
        if (towerToBuild == null) return;

        int cost = BuildManager.instance.GetTowerCost();
        if (PlayerStats.Money < cost)
        {
            Debug.Log("Not enough money!");
            return;
        }

        PlayerStats.Money -= cost;
        tower = Instantiate(towerToBuild, transform.position, transform.rotation);

        BuildManager.instance.SelectTowerToBuild(null, 0);
    }

    // Helper function to grab the range, level, and name
    float GetRangeFromObject(GameObject towerObj, out int level, out string name)
    {
        name = towerObj.name.Replace("Tower_", "").Replace("(Clone)", ""); // Cleans up the name

        Tower t = towerObj.GetComponent<Tower>();
        if (t != null) { level = t.level; return t.range; }

        TowerEMP e = towerObj.GetComponent<TowerEMP>();
        if (e != null) { level = e.level; return e.range; }

        TowerStunner s = towerObj.GetComponent<TowerStunner>();
        if (s != null) { level = s.level; return s.range; }

        level = 0;
        return 0f;
    }
}