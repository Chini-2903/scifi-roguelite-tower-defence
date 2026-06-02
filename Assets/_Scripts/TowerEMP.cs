using UnityEngine;

public class TowerEMP : MonoBehaviour
{
    [Header("Attributes")]
    public float range = 4f;
    public float pulseRate = 1.5f;
    public float slowPercentage = 0.35f;
    public float slowDuration = 3f;
    public int level = 1;

    [Header("Economy")]
    public int upgradeCostLevel2 = 150;
    public int upgradeCostLevel3 = 200;
    private int totalMoneySpent = 150;

    private float pulseCountdown = 0f;
    private float dotCountdown = 0f;

    [Header("Unity Setup")]
    public string enemyTag = "Enemy";
    public GameObject pulsePrefab;

    public void UpgradeTower()
    {
        if (level == 1 && PlayerStats.Money >= upgradeCostLevel2)
        {
            PlayerStats.Money -= upgradeCostLevel2;
            totalMoneySpent += upgradeCostLevel2;
            level = 2;

            slowPercentage = 0.50f;
            slowDuration = 4f;
            range = 5f;
        }
        else if (level == 2 && PlayerStats.Money >= upgradeCostLevel3)
        {
            PlayerStats.Money -= upgradeCostLevel3;
            totalMoneySpent += upgradeCostLevel3;
            level = 3;

            slowPercentage = 0.65f;
            // Level 3 also unlocks the DealAreaDamage function in Update
        }
    }

    public int GetSellValue() => Mathf.RoundToInt(totalMoneySpent * 0.65f);

    void Update()
    {
        if (GameManager.GameIsOver) return;

        if (pulseCountdown <= 0f)
        {
            TriggerPulse();
            pulseCountdown = pulseRate;
        }
        pulseCountdown -= Time.deltaTime;

        if (level >= 3)
        {
            if (dotCountdown <= 0f)
            {
                DealAreaDamage();
                dotCountdown = 1f;
            }
            dotCountdown -= Time.deltaTime;
        }
    }

    void TriggerPulse()
    {
        GameObject visual = Instantiate(pulsePrefab, transform.position, transform.rotation);
        PulseEffect effect = visual.GetComponent<PulseEffect>();
        if (effect != null) effect.maxScale = range * 2f;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag(enemyTag))
            {
                EnemyMovement enemy = col.GetComponent<EnemyMovement>();
                if (enemy != null) enemy.ApplySlow(slowPercentage, slowDuration);
            }
        }
    }

    void DealAreaDamage()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag(enemyTag))
            {
                EnemyHealth e = col.GetComponent<EnemyHealth>();
                if (e != null) e.TakeDamage(1);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}