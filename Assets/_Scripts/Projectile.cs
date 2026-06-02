using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f;

    [HideInInspector] public int damage;
    [HideInInspector] public int pierce;

    private Vector2 startPosition;
    private float maxRange;
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

    // NEW: Receives damage and pierce from the tower!
    public void Setup(Transform target, float range, int towerDamage, int towerPierce)
    {
        startPosition = transform.position;
        maxRange = range;
        damage = towerDamage;
        pierce = towerPierce;

        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);
        foreach (Collider2D col in hitColliders)
        {
            if (col.CompareTag("Enemy"))
            {
                HitTarget(col.gameObject);
            }
        }

        if (Vector2.Distance(startPosition, transform.position) >= maxRange) Destroy(gameObject);
    }

    void HitTarget(GameObject enemyHit)
    {
        if (hitEnemies.Contains(enemyHit)) return;
        hitEnemies.Add(enemyHit);

        EnemyHealth e = enemyHit.GetComponent<EnemyHealth>();
        if (e != null) e.TakeDamage(damage);

        pierce--;
        if (pierce <= 0) Destroy(gameObject);
    }
}