using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Attributes")]
    public float range = 3f;
    public float fireRate = 2f;
    public int damage = 2; // NEW
    public int pierce = 3; // NEW
    public int level = 1;
    public bool isSniper = false;

    [Header("Economy")]
    public int upgradeCostLevel2 = 75;
    public int upgradeCostLevel3 = 120;
    private int totalMoneySpent; // Used to calculate Sell Value

    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public GameObject bulletPrefab;
    public Transform partToRotate;

    public Transform baseFirePoint;
    public Transform leftBarrel;
    public Transform rightBarrel;
    public LineRenderer railgunLine;

    private Transform target;

    void Start()
    {
        // Remember base cost for selling later
        totalMoneySpent = isSniper ? 100 : 50;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        if (railgunLine != null) railgunLine.enabled = false;
    }

    // --- NEW: THE UPGRADE BRAIN ---
    public void UpgradeTower()
    {
        if (level == 1 && PlayerStats.Money >= upgradeCostLevel2)
        {
            PlayerStats.Money -= upgradeCostLevel2;
            totalMoneySpent += upgradeCostLevel2;
            level = 2;

            if (!isSniper) { pierce = 5; fireRate = 2.5f; range = 2.2f; } // Sentry Lv 2
            else { damage = 12; pierce = 7; fireRate = 0.6f; range = 7.0f; } // Sniper Lv 2
        }
        else if (level == 2 && PlayerStats.Money >= upgradeCostLevel3)
        {
            PlayerStats.Money -= upgradeCostLevel3;
            totalMoneySpent += upgradeCostLevel3;
            level = 3;

            if (!isSniper) { damage = 3; pierce = 7; fireRate = 4.0f; range = 2.5f; } // Sentry Lv 3
            else { damage = 25; pierce = 12; fireRate = 0.8f; range = 8.0f; } // Sniper Lv 3
        }
    }

    public int GetSellValue()
    {
        return Mathf.RoundToInt(totalMoneySpent * 0.65f); // 65% return!
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range) target = nearestEnemy.transform;
        else target = null;
    }

    void Update()
    {
        if (GameManager.GameIsOver || target == null) return;

        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        partToRotate.rotation = Quaternion.Euler(0, 0, angle - 90f);

        if (fireCountdown <= 0f)
        {
            if (isSniper && level >= 3) StartCoroutine(FireRailgun());
            else if (!isSniper && level >= 3) StartCoroutine(FireTwinBlaster());
            else Shoot(baseFirePoint);

            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    void Shoot(Transform fp)
    {
        if (fp == null) return;
        GameObject bulletGO = Instantiate(bulletPrefab, fp.position, fp.rotation);
        Projectile bullet = bulletGO.GetComponent<Projectile>();

        // PASS THE STATS TO THE BULLET!
        if (bullet != null) bullet.Setup(target, range, damage, pierce);
    }

    IEnumerator FireTwinBlaster()
    {
        Shoot(leftBarrel);
        yield return new WaitForSeconds(0.08f);
        Shoot(rightBarrel);
    }

    IEnumerator FireRailgun()
    {
        if (railgunLine == null) yield break;

        railgunLine.SetPosition(0, baseFirePoint.position);
        Vector2 dir = (target.position - baseFirePoint.position).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(baseFirePoint.position, dir, range);

        int pierceLeft = this.pierce; // Uses actual upgraded pierce!
        int currentDamage = this.damage; // Uses actual upgraded damage!
        Vector2 endPoint = (Vector2)baseFirePoint.position + (dir * range);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag(enemyTag))
            {
                EnemyHealth e = hit.collider.GetComponent<EnemyHealth>();
                if (e != null)
                {
                    e.TakeDamage(currentDamage);
                    pierceLeft--;
                    if (pierceLeft <= 0)
                    {
                        endPoint = hit.point;
                        break;
                    }
                }
            }
        }

        railgunLine.SetPosition(1, endPoint);
        railgunLine.enabled = true;
        yield return new WaitForSeconds(0.1f);
        railgunLine.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}