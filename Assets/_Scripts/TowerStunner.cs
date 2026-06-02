using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStunner : MonoBehaviour
{
    [Header("Attributes")]
    public float range = 3f;
    public float fireRate = 0.8f;
    public int damage = 4;
    public float stunChance = 0.20f;
    public float stunDuration = 0.5f;
    public int bounces = 1;
    public float bounceRange = 3f;
    public int level = 1;

    [Header("Economy")]
    public int upgradeCostLevel2 = 250;
    public int upgradeCostLevel3 = 350;
    private int totalMoneySpent = 200;

    private float fireCountdown = 0f;

    [Header("Unity Setup")]
    public string enemyTag = "Enemy";
    public Transform firePoint;
    public LineRenderer lineRenderer;

    public void UpgradeTower()
    {
        if (level == 1 && PlayerStats.Money >= upgradeCostLevel2)
        {
            PlayerStats.Money -= upgradeCostLevel2;
            totalMoneySpent += upgradeCostLevel2;
            level = 2;

            damage = 6;
            stunChance = 0.25f;
            stunDuration = 0.75f;
            bounces = 2;
            range = 3.5f;
        }
        else if (level == 2 && PlayerStats.Money >= upgradeCostLevel3)
        {
            PlayerStats.Money -= upgradeCostLevel3;
            totalMoneySpent += upgradeCostLevel3;
            level = 3;

            damage = 10;
            stunChance = 0.35f;
            stunDuration = 1.0f;
            bounces = 4;
            range = 4.0f;
        }
    }

    public int GetSellValue() => Mathf.RoundToInt(totalMoneySpent * 0.65f);

    void Start() { lineRenderer.enabled = false; }

    void Update()
    {
        if (GameManager.GameIsOver) return;

        if (fireCountdown <= 0f)
        {
            if (AttemptFire()) fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    bool AttemptFire()
    {
        // Create a blank memory for this specific attack
        HashSet<GameObject> enemiesHitThisChain = new HashSet<GameObject>();

        // 1. Find the first target
        GameObject firstTarget = FindClosestEnemy(transform.position, range, enemiesHitThisChain);
        if (firstTarget == null) return false;

        // Remember it and hit it
        enemiesHitThisChain.Add(firstTarget);
        List<Transform> lightningPath = new List<Transform>();
        lightningPath.Add(firePoint);
        lightningPath.Add(firstTarget.transform);
        HitTarget(firstTarget);

        // 2. Attempt the bounces
        GameObject currentTarget = firstTarget;
        for (int i = 0; i < bounces; i++)
        {
            // Pass the memory list so it ignores EVERYONE it already struck
            GameObject nextTarget = FindClosestEnemy(currentTarget.transform.position, bounceRange, enemiesHitThisChain);
            if (nextTarget != null)
            {
                enemiesHitThisChain.Add(nextTarget); // Remember this one too!
                lightningPath.Add(nextTarget.transform);
                HitTarget(nextTarget);
                currentTarget = nextTarget;
            }
            else break; // No new enemies nearby, stop bouncing
        }

        StartCoroutine(DrawLightning(lightningPath));
        return true;
    }

    // The search function now checks against the entire memory list!
    GameObject FindClosestEnemy(Vector2 origin, float searchRadius, HashSet<GameObject> alreadyHit)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            if (alreadyHit.Contains(enemy)) continue; // Skip if we already hit them in this chain!

            float distance = Vector2.Distance(origin, enemy.transform.position);
            if (distance < shortestDistance && distance <= searchRadius)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    void HitTarget(GameObject target)
    {
        EnemyHealth eHealth = target.GetComponent<EnemyHealth>();
        EnemyMovement eMove = target.GetComponent<EnemyMovement>();
        if (eHealth != null) eHealth.TakeDamage(damage);
        if (eMove != null && Random.value <= stunChance) eMove.ApplyStun(stunDuration);
    }

    IEnumerator DrawLightning(List<Transform> path)
    {
        lineRenderer.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++) lineRenderer.SetPosition(i, path[i].position);
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.1f);
        lineRenderer.enabled = false;
    }
}