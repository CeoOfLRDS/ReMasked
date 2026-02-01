using UnityEngine;

public class CameraBlock : MonoBehaviour
{
    public Transform cameraSnapPoint;

    private Health[] enemies;

    void Awake()
    {
        enemies = GetComponentsInChildren<Health>();
    }

    public bool IsCleared()
    {
        foreach (var e in enemies)
        {
            if (e != null && !e.IsDead)
                return false;
        }
        return true;
    }
}
