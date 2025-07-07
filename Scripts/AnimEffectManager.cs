using UnityEngine;
using UnityEngine.EventSystems;

public class AnimEffectManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Deck Offset")]
    public Vector3 deckOffset = new Vector3(10f, -2f, -1f);
    public bool isInDeck = false;
    public bool isFreezed = false;

    [Header(" Ray Settings")]
    public float offsetDistance = 8f;
    public Vector3 localOffset = Vector3.zero;

    [Header("Lerp Settings")]
    public float positionLerpSpeed = 5f;
    public float rotationLerpSpeed = 4f;

    [Header("Hover Effect")]
    public bool enableHover = true;
    public float hoverHeight = 0.2f;
    public float hoverLerpSpeed = 10f;
    private bool isHovering = false;

    [Header(" Wave Effect")]
    public bool enableWave = true;
    public float waveAmplitude = 0.1f;
    public float waveFrequency = 2f;

    [Header(" Flip Settings")]
    public float flipHoldDuration = 0.4f;
    private bool isFlipped = false;
    private float rightClickHoldTime = 0f;

    private Camera mainCam;
    private Vector3 basePosition;

    void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null)
            Debug.LogError("Main Camera bulunamadı. 'MainCamera' tag'i verilmiş olmalı.");
    }

    void Update()
    {
        //  Sağ tuşla basılı tutarak kartı döndür
        if (Input.GetMouseButton(1))
        {
            rightClickHoldTime += Time.deltaTime;

            if (!isFlipped && rightClickHoldTime >= flipHoldDuration)
                isFlipped = true;
        }
        else
        {
            // Tuş bırakıldığında geri dön
            if (isFlipped && rightClickHoldTime > 0f)
                isFlipped = false;

            rightClickHoldTime = 0f;
        }
    }

    void LateUpdate()
    {
        if (mainCam == null) return;

        //  Ray oluştur
        Ray centerRay = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        Vector3 rayPoint = centerRay.origin + centerRay.direction * offsetDistance;
        Vector3 offsetToApply = isInDeck ? deckOffset : localOffset;

        Vector3 worldOffset =
            mainCam.transform.right * offsetToApply.x +
            mainCam.transform.up * offsetToApply.y +
            mainCam.transform.forward * offsetToApply.z;
        

        Vector3 targetBasePos = rayPoint + worldOffset;



        float waveOffset = enableWave ? Mathf.Sin(Time.time * waveFrequency) * waveAmplitude : 0f;
        float actualDeltaY = transform.position.y - (targetBasePos.y + waveOffset);

        if (Mathf.Abs(actualDeltaY) > 0.01f)
        {
            Vector3 adjustedTarget = new Vector3(targetBasePos.x, targetBasePos.y + waveOffset, targetBasePos.z);
            transform.position = Vector3.Lerp(transform.position, adjustedTarget, positionLerpSpeed * Time.deltaTime);
        }

        basePosition = new Vector3(targetBasePos.x, targetBasePos.y + waveOffset, targetBasePos.z);

        Vector3 hoverTarget = isHovering ? basePosition + Vector3.up * hoverHeight : basePosition;
        transform.position = Vector3.Lerp(transform.position, hoverTarget, hoverLerpSpeed * Time.deltaTime);

        //  Kamera yönüne göre ön mü arka mı baksın?
        Quaternion targetRot = isFlipped ?
            Quaternion.LookRotation(-mainCam.transform.forward, mainCam.transform.up) : // arka yüz
            mainCam.transform.rotation; // ön yüz

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationLerpSpeed * Time.deltaTime);
    }

    public void OnPointerEnter(PointerEventData eventData) => isHovering = true;
    public void OnPointerExit(PointerEventData eventData) => isHovering = false;
}
