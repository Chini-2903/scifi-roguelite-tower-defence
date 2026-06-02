using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float baseSpeed = 3f;
    private float currentSpeed;
    private float slowTimer = 0f;
    private float stunTimer = 0f; // NEW: The stun clock

    private Transform[] waypoints;
    private int waypointIndex = 0;

    void Start()
    {
        currentSpeed = baseSpeed;

        GameObject pathObject = GameObject.Find("Enemy_Path");
        waypoints = new Transform[pathObject.transform.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathObject.transform.GetChild(i);
        }
    }

    void Update()
    {
        // 1. STUN CHECK (Highest Priority)
        if (stunTimer > 0f)
        {
            stunTimer -= Time.deltaTime;
            return; // If stunned, we use 'return' to instantly stop running the rest of the Update code. They completely freeze!
        }

        // 2. SLOW CHECK
        if (slowTimer > 0f)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0f)
            {
                currentSpeed = baseSpeed;
            }
        }

        // 3. MOVEMENT
        if (waypointIndex < waypoints.Length)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, currentSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, waypoints[waypointIndex].position) < 0.1f)
            {
                waypointIndex++;
            }
        }
        else
        {
            // The satellite takes damage equal to how many layers the enemy has left!
            PlayerStats.Lives -= GetComponent<EnemyHealth>().currentLayers;
            Destroy(gameObject);
        }
    }

    public void ApplySlow(float slowPercentage, float duration)
    {
        currentSpeed = baseSpeed * (1f - slowPercentage);
        slowTimer = duration;
    }

    // NEW: The Stunner Tower will call this!
    public void ApplyStun(float duration)
    {
        stunTimer = duration;
    }
}