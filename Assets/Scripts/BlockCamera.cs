using UnityEngine;

public class BlockCamera : MonoBehaviour
{
    public void SnapTo(Vector3 position)
    {
        transform.position = new Vector3(
            position.x,
            position.y,
            transform.position.z
        );
    }
}
