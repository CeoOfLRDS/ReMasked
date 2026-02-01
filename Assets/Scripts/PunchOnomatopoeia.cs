using UnityEngine;

public class PunchOnomatopoeia : MonoBehaviour
{
    public float lifetime = 0.5f;
    public float floatUpSpeed = 1f;
    public float spinSpeed = 90f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += Vector3.up * floatUpSpeed * Time.deltaTime;
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }
}
