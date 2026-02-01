using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CameraBlockManager : MonoBehaviour
{
    public BlockCamera blockCamera;

    private CameraBlock currentBlock;

    void OnTriggerEnter2D(Collider2D other)
    {
        var block = other.GetComponent<CameraBlock>();
        if (block == null) return;

        if (currentBlock == null || currentBlock.IsCleared())
        {
            EnterBlock(block);
        }
        else
        {
            PushBackIntoBlock();
        }
    }

    void EnterBlock(CameraBlock block)
    {
        currentBlock = block;

        if (block.cameraSnapPoint != null)
            blockCamera.SnapTo(block.cameraSnapPoint.position);
    }

    void PushBackIntoBlock()
    {
        Vector2 pos = transform.position;
        pos.x -= Mathf.Sign(transform.localScale.x) * 0.5f;
        transform.position = pos;
    }
}
