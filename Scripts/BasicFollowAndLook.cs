using UnityEngine;
using UnityEngine.EventSystems;

public class BasicFollowAndLook : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float offsetDistance = 8f;
    public Vector3 localOffset = Vector3.zero;

    public float positionLerpSpeed = 5f;
    public float rotationLerpSpeed = 1f;

    [Header("Hover Effect")]
    public bool enableHover = true;
    public float hoverHeight = 0.2f;
    public float hoverLerpSpeed = 10f;
    private bool isHovering = false;

    private Camera mainCam;
    private Vector3 basePosition;

    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCam == null) return;

        // Ortadan çıkan ray
        Ray centerRay = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        Vector3 rayPoint = centerRay.origin + centerRay.direction * offsetDistance;

        // Local offset → world offset
        Vector3 worldOffset =
            mainCam.transform.right * localOffset.x +
            mainCam.transform.up * localOffset.y +
            mainCam.transform.forward * localOffset.z;

        Vector3 targetBase = rayPoint + worldOffset;
        basePosition = targetBase;

        // Hover varsa pozisyonu yukarı kaydır
        Vector3 hoverTarget = isHovering && enableHover ? basePosition + Vector3.up * hoverHeight : basePosition;
        transform.position = Vector3.Lerp(transform.position, hoverTarget, hoverLerpSpeed * Time.deltaTime);

        // Kameraya doğru dön
        Quaternion targetRot = mainCam.transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationLerpSpeed * Time.deltaTime);
    }

    // Hover kontrolü
    public void OnPointerEnter(PointerEventData eventData) => isHovering = true;
    public void OnPointerExit(PointerEventData eventData) => isHovering = false;
}
