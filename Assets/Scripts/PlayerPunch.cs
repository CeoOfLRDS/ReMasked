using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    public float punchRange = 1.2f;
    public float punchAngle = 45f;
    public LayerMask punchableLayers;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoPunch();
        }
    }

    void DoPunch()
    {
        Vector2 origin = transform.position;
        Vector2 forward = transform.right * transform.localScale.x;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, punchRange, punchableLayers);

        foreach (var col in hits)
        {
            Vector2 toTarget = (col.transform.position - transform.position);
            float angle = Vector2.Angle(forward, toTarget);

            if (angle <= punchAngle)
            {
                var punchable = col.GetComponent<Punchable>();
                if (punchable != null)
                {
                    punchable.Punch(toTarget);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 origin = transform.position;
        Vector3 forward = transform.right * transform.localScale.x;
        Gizmos.DrawWireSphere(origin, punchRange);
        Gizmos.DrawLine(origin, origin + Quaternion.Euler(0, 0, punchAngle) * forward * punchRange);
        Gizmos.DrawLine(origin, origin + Quaternion.Euler(0, 0, -punchAngle) * forward * punchRange);
    }
}
