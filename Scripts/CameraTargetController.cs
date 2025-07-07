using UnityEngine;
using Cinemachine;

public class CameraTargetController : MonoBehaviour
{
    [Header("Zoom")]
    public float zoomSpeed = 10f;
    public float minZoom = 10f;
    public float maxZoom = 30f;
    public float zoomLerpSpeed = 10f;

    [Header("Tilt")]
    public float minTilt = 30f;
    public float maxTilt = 60f;
    public float tiltLerpSpeed = 10f;

    [Header("Movement")]
    public float moveSpeed = 10f;
    public Vector2 xLimits = new Vector2(-10f, 30f);
    public Vector2 zLimits = new Vector2(-10f, 30f);

    public CinemachineVirtualCamera cineCam;

    private CinemachineFramingTransposer framing;
    private float currentZoom;
    private float targetZoom;
    private float targetTilt;

    void Start()
    {
        framing = cineCam.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (framing == null)
        {
            Debug.LogError("Framing Transposer bulunamadı! Cinemachine ayarlarını kontrol et.");
            return;
        }

        currentZoom = framing.m_CameraDistance;
        targetZoom = currentZoom;

        float t = Mathf.InverseLerp(minZoom, maxZoom, currentZoom);
        targetTilt = Mathf.Lerp(minTilt, maxTilt, t);
    }

    void Update()
    {
        HandleZoom();
        HandleMovement();
        SmoothApplyZoomAndTilt();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetZoom -= scroll * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

            float t = Mathf.InverseLerp(minZoom, maxZoom, targetZoom);
            targetTilt = Mathf.Lerp(minTilt, maxTilt, t);
        }
    }

    void SmoothApplyZoomAndTilt()
    {
        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomLerpSpeed);
        framing.m_CameraDistance = currentZoom;

        float currentX = cineCam.transform.rotation.eulerAngles.x;
        float newTilt = Mathf.LerpAngle(currentX, targetTilt, Time.deltaTime * tiltLerpSpeed);
        Vector3 rot = cineCam.transform.rotation.eulerAngles;
        cineCam.transform.rotation = Quaternion.Euler(newTilt, rot.y, rot.z);
    }

    void HandleMovement()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if (dir.magnitude > 0f)
        {
            Vector3 move = dir;
            transform.position += move * moveSpeed * Time.deltaTime;
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, xLimits.x, xLimits.y);
        pos.z = Mathf.Clamp(pos.z, zLimits.x, zLimits.y);
        transform.position = pos;
    }
}
