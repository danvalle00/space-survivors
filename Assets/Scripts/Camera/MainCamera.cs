using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector2 offset;
    [SerializeField] private float smoothSpeed = 0.125f;
    private Vector2 velocity = Vector2.zero;

    void Awake()
    {
        playerTransform = FindFirstObjectByType<Player>().transform;
    }
    void LateUpdate()
    {
        if (playerTransform == null)
        {
            return;
        }
        Vector2 targetPos = (Vector2)playerTransform.position + offset;
        Vector2 smoothedPos = Vector2.SmoothDamp(transform.position, targetPos, ref velocity, smoothSpeed);
        transform.position = new(smoothedPos.x, smoothedPos.y, transform.position.z);
    }
}

