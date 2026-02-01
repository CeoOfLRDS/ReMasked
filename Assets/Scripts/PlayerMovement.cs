using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float baseMoveSpeed = 5f;

    private float currentMoveSpeed;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMoveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;

        if (movement.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(movement.x);
            transform.localScale = scale;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = movement * currentMoveSpeed;
    }

    public void SetMoveSpeedMultiplier(float multiplier)
    {
        currentMoveSpeed = baseMoveSpeed * multiplier;
    }

    public void ResetMoveSpeed()
    {
        currentMoveSpeed = baseMoveSpeed;
    }
}
