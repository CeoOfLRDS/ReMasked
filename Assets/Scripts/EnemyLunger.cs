using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyLunger : MonoBehaviour
{
    public Transform player;

    public float hoverAmplitude = 0.25f;
    public float hoverSpeed = 2f;

    public float moveSpeed = 1.2f;
    public float wanderStrength = 0.4f;
    public float wanderSpeed = 0.6f;

    public float chaseRange = 6f;

    public float attackRange = 1.5f;
    public float lungeForce = 8f;
    public float attackCooldown = 1.5f;
    public int damage = 3;

    private Rigidbody2D rb;
    private Punchable punchable;
    private Health playerHealth;

    private Vector2 basePosition;
    private bool attacking;
    private float lastAttackTime;
    private float wanderSeed;
    private bool hasAggro;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        punchable = GetComponent<Punchable>();
        basePosition = rb.position;
        wanderSeed = Random.value * 100f;

        if (player)
            playerHealth = player.GetComponent<Health>();
    }

    void FixedUpdate()
    {
        if (punchable != null && punchable.IsLaunched)
            return;

        if (player == null)
            return;

        float dist = Vector2.Distance(rb.position, player.position);

        if (!hasAggro && dist <= chaseRange)
            hasAggro = true;

        if (!hasAggro)
            return;

        Hover();
        DriftTowardPlayer();

        if (!attacking && dist <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            StartCoroutine(Lunge());
    }

    void Hover()
    {
        float offset = Mathf.Sin(Time.time * hoverSpeed + wanderSeed) * hoverAmplitude;
        rb.position = new Vector2(rb.position.x, basePosition.y + offset);
    }

    void DriftTowardPlayer()
    {
        float noise = Mathf.PerlinNoise(Time.time * wanderSpeed, wanderSeed) - 0.5f;
        float bias = Mathf.Sign(player.position.x - transform.position.x);

        float xMove = (bias + noise * wanderStrength) * moveSpeed;
        rb.velocity = new Vector2(xMove, rb.velocity.y);

        if (xMove != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(xMove) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    IEnumerator Lunge()
    {
        attacking = true;
        lastAttackTime = Time.time;

        Vector2 dir = (player.position - transform.position).normalized;

        float angle = Mathf.Sign(dir.x) * -45f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        rb.velocity = Vector2.zero;
        rb.AddForce(dir * lungeForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.15f);

        if (playerHealth != null && !playerHealth.IsDead)
            playerHealth.TakeDamage(damage);

        yield return new WaitForSeconds(0.3f);

        transform.rotation = Quaternion.identity;
        rb.velocity = Vector2.zero;

        attacking = false;
    }
}
