using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Punchable : MonoBehaviour
{
    public float upwardForce = 30f;
    public float sidewaysForce = 12f;
    public float baseTorque = 2500f;
    public float torqueRamp = 2.5f;
    public float gravityScale = 0f;
    public float lifetime = 1.8f;
    public Vector2 screenCenterBias = new Vector2(0f, 2f);
    public float centerPullStrength = 4f;

    private Rigidbody2D rb;
    private bool launched;
    private float launchTime;
    private Vector3 startScale;

    public bool IsLaunched => launched;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startScale = transform.localScale;
    }

    public void Punch(Vector2 direction)
    {
        if (launched) return;
        launched = true;

        foreach (var script in GetComponents<MonoBehaviour>())
            if (script != this)
                script.enabled = false;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = gravityScale;
        rb.freezeRotation = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.drag = 0f;
        rb.angularDrag = 0f;

        Vector2 impulse = new Vector2(
            Mathf.Sign(direction.x) * sidewaysForce,
            upwardForce
        );

        rb.AddForce(impulse, ForceMode2D.Impulse);

        float spinDir = Random.value < 0.5f ? -1f : 1f;
        rb.AddTorque(baseTorque * spinDir, ForceMode2D.Impulse);

        launchTime = Time.time;
        StartCoroutine(ShrinkAndDie());
    }

    void FixedUpdate()
    {
        if (!launched) return;

        rb.AddTorque(baseTorque * torqueRamp * Time.fixedDeltaTime);

        Vector2 target = (Vector2)Camera.main.transform.position + screenCenterBias;
        Vector2 toCenter = target - rb.position;
        rb.AddForce(toCenter * centerPullStrength * Time.fixedDeltaTime, ForceMode2D.Force);

        if (Time.time - launchTime >= lifetime)
            Destroy(gameObject);
    }

    IEnumerator ShrinkAndDie()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / lifetime;
            float eased = Mathf.Pow(t, 2.5f);
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, eased);
            yield return null;
        }
    }
}
