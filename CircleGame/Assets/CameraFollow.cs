
using UnityEngine;

public class CameraFollow : MonoBehaviour //really cool script I found
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    public float smoothTime = 0.2f;

    private Vector3 smoothVelocity;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref smoothVelocity,
            smoothTime
        );
    }
}
