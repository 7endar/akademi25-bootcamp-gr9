using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minZoom = 10f;
    public float maxZoom = 30f;

    [Header("Tilt Settings")]
    public float minTilt = 30f;
    public float maxTilt = 60f;

    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public Vector2 xLimits = new Vector2(-10f, 30f);
    public Vector2 zLimits = new Vector2(-10f, 30f);

    private Camera cam;
    private float targetZoom;
    private float targetTilt;
    public float TargetZoom => targetZoom;
    public float TargetTilt => targetTilt;

    void Start()
    {
        cam = Camera.main;
        targetZoom = cam.transform.position.y;
        targetTilt = cam.transform.eulerAngles.x;
    }

    void Update()
    {
        HandleZoom();
        HandleMovement();
        ApplyTransform();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetZoom -= scroll * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

            // Interpolated tilt based on zoom level
            float t = Mathf.InverseLerp(minZoom, maxZoom, targetZoom);
            targetTilt = Mathf.Lerp(minTilt, maxTilt, t);
        }
    }

    void HandleMovement()
    {
        Vector3 dir = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0f,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (dir.magnitude > 0f)
        {
            Vector3 move = Quaternion.Euler(0, 45, 0) * dir; // İzometrik düzlem
            transform.position += move * moveSpeed * Time.deltaTime;
        }

        ClampPosition();
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, xLimits.x, xLimits.y);
        pos.z = Mathf.Clamp(pos.z, zLimits.x, zLimits.y);
        transform.position = pos;
    }

    void ApplyTransform()
    {
        Vector3 pos = cam.transform.position;
        pos.y = targetZoom;
        cam.transform.position = pos;

        Vector3 rot = cam.transform.eulerAngles;
        rot.x = targetTilt;
        cam.transform.eulerAngles = rot;
    }
}
