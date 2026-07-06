using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Tracking Target")]
    [SerializeField] private Transform targetTruck;

    [Header("Position Tweaks")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -10f);
    [SerializeField] private float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (targetTruck == null) return;

        Vector3 desiredPosition = targetTruck.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(targetTruck);
    }
}
