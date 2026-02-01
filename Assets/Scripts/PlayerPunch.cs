using System.Collections;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    public Transform punchPoint;
    public Vector2 punchBoxSize = new Vector2(1.2f, 0.8f);
    public LayerMask punchableLayers;

    public float startup = 0.05f;
    public float active = 0.08f;
    public float recovery = 0.15f;
    public float hitStopTime = 0.06f;

    public int baseDamage = 1;

    public GameObject[] onomatopoeiaPrefabs;
    public Vector2 fxOffset = new Vector2(0.2f, 0.2f);

    private int currentDamage;
    private float speedMultiplier = 1f;
    private bool isPunching;

    void Awake()
    {
        currentDamage = baseDamage;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isPunching)
            StartCoroutine(PunchRoutine());
    }

    IEnumerator PunchRoutine()
    {
        isPunching = true;

        yield return new WaitForSeconds(startup / speedMultiplier);

        float timer = 0f;
        bool hit = false;

        while (timer < active / speedMultiplier)
        {
            Collider2D col = Physics2D.OverlapBox(
                punchPoint.position,
                punchBoxSize,
                0f,
                punchableLayers
            );

            if (col && !hit)
            {
                if (col.transform.root == transform.root)
                {
                    timer += Time.deltaTime;
                    yield return null;
                    continue;
                }

                hit = true;

                Vector2 dir = (col.transform.position - transform.position).normalized;

                var health = col.GetComponent<Health>();
                if (health)
                    health.TakeDamage(currentDamage);

                var punchable = col.GetComponent<Punchable>();
                if (punchable)
                    punchable.Punch(dir);

                SpawnOnomatopoeia(col.transform.position);

                yield return StartCoroutine(HitStop());
            }

            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(recovery / speedMultiplier);
        isPunching = false;
    }

    IEnumerator HitStop()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(hitStopTime);
        Time.timeScale = 1f;
    }

    void SpawnOnomatopoeia(Vector2 hitPosition)
    {
        if (onomatopoeiaPrefabs == null || onomatopoeiaPrefabs.Length == 0)
            return;

        GameObject fx = onomatopoeiaPrefabs[Random.Range(0, onomatopoeiaPrefabs.Length)];

        Vector2 offset = new Vector2(
            Random.Range(-fxOffset.x, fxOffset.x),
            Random.Range(-fxOffset.y, fxOffset.y)
        );

        Instantiate(
            fx,
            hitPosition + offset,
            Quaternion.Euler(0f, 0f, Random.Range(-20f, 20f))
        );
    }

    public void SetAttackSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    public void ResetAttackSpeed()
    {
        speedMultiplier = 1f;
    }

    public void SetDamage(int value)
    {
        currentDamage = value;
    }

    public void ResetDamage()
    {
        currentDamage = baseDamage;
    }
}
