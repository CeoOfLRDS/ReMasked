using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float speed = 3f;
    public Transform player;

    void Update()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }
}
