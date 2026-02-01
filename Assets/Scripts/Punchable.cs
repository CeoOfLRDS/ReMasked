using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Punchable : MonoBehaviour
{
    public int maxHits = 3;
    public float knockbackForce = 8f;
    public float spinTorque = 500f;

    private int hitCount = 0;
    private Rigidbody2D rb;
    private bool isKnocked = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Punch(Vector2 direction)
    {
        hitCount++;
        ApplyKnockback(direction);

        if (hitCount >= maxHits)
            DestroyAfterDelay();
    }

    void ApplyKnockback(Vector2 direction)
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);

        float torqueDir = Random.value < 0.5f ? -1f : 1f;
        rb.AddTorque(spinTorque * torqueDir);
        isKnocked = true;
    }

    void DestroyAfterDelay()
    {
        Destroy(gameObject, 0.5f);
    }
}
