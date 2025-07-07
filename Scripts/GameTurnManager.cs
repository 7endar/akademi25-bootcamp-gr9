using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTurnManager : MonoBehaviour
{
   


    [Header("Turn Settings")]
    public int cardsPerTurn = 3;

    [Header("References")]
    public CardManager cardManager;
    public PlayerDeckManager playerDeck;

    private GameObject pendingCardObject; // Seçilen kartın sahnedeki GameObject referansı
    private int stepIndex = 0;

    [Header("Step Settings")]
    public float delayBetweenSteps = 2f;

    [Header("Grid Settings")]
    public GridManager gridManager;

    private GameObject selectedCardObject;
    private BaseCard selectedPlacementCard;
    private bool isInPlacementPhase = false;

    [Header("Day/Night Settings")]
    public float dayDuration = 30f;
    public float nightDuration = 30f;
    private float turnTimer = 0f;
    private bool dayTimerRunning = false;
    private bool nightTimerRunning = false;
    public Light sceneLight;
    public Color dayLightColor = Color.white;
    public Color nightLightColor = Color.blue;
    public Vector3 dayRotation = new Vector3(50, 30, 0);
    public Vector3 nightRotation = new Vector3(340, 30, 0);

    public List<CardType> daySteps = new List<CardType> { CardType.Building, CardType.Unit,CardType.Skill };
    public List<CardType> nightSteps = new List<CardType> { CardType.Building, CardType.Unit, CardType.Skill };

    [Header("Extra Card Settings")]
    public int extraCardInterval = 5; // her 5 gün/gece
    public enum StepTarget { Day, Night }
    public StepTarget extraCardTarget = StepTarget.Day;
    public CardType extraCardType = CardType.Skill;
    

    private bool isNightPhase = false;
    private bool opponentAttacked = false;
    private bool playerAttacked = false;

    [SerializeField] private ConfirmCardButton confirmButtonObj;
    [SerializeField] private HideCardButton hideCardButtonObj;

    public static ConfirmCardButton confirmButton;
    public static HideCardButton hideCardButton;

    private int dayCount = 0;

    /// /////////////////////////


    void Awake()
    {
        confirmButton = confirmButtonObj;
        hideCardButton = hideCardButtonObj;
    }
    private void Start()
    {
        StartTurn();
    }
    private void Update()
    {
        if (dayTimerRunning)
        {
            turnTimer += Time.deltaTime;

            if (turnTimer >= dayDuration)
            {
                dayTimerRunning = false;

                if (!opponentAttacked&& !playerAttacked)
                {
                    StartNightPhase();
                }
                else
                {
                    Debug.Log("Opponent attacked → geceye geçilmeyecek.");
                    // Saldırı fazına geç gibi işlemler yapılabilir.
                }
            }
        }
        if (nightTimerRunning)
        {
            turnTimer += Time.deltaTime;

            if (turnTimer >= nightDuration)
            {
                Debug.Log("Zaman doldu, geceye geçiliyor...");
                nightTimerRunning = false;
                StartLateNightPhase();
            }
        }
    }
    public void StartTurn()
    {
        stepIndex = 0;
        pendingCardObject = null;
        isNightPhase = false;
        pendingCardObject = null;
        turnTimer = 0f;
        dayTimerRunning = true;
        dayCount++;
        if (confirmButton != null)
            confirmButton.gameObject.SetActive(true);

        if (hideCardButton != null)
            hideCardButton.gameObject.SetActive(true);
       
        if (extraCardInterval > 0 && dayCount % extraCardInterval == 0)
        {
            if (extraCardTarget == StepTarget.Day)
                InsertExtraStep(daySteps);
            else
                InsertExtraStep(nightSteps);
        }

        if (sceneLight != null)
        {
            sceneLight.color = dayLightColor;
            sceneLight.transform.rotation = Quaternion.Euler(dayRotation);
        }

        StartCoroutine(RunNextStep());
    }
    private void InsertExtraStep(List<CardType> stepList)
    {
        int insertIndex = stepList.Count;
        stepList.Insert(insertIndex, extraCardType);
        Debug.Log($"Extra card step inserted at END: {extraCardType}");
    }


    public void SetPendingCard(GameObject selectedObj)
    {
        pendingCardObject = selectedObj;
    }
    public void StartLateNightPhase()
    {
        if (confirmButton != null)
            confirmButton.gameObject.SetActive(false);

        if (hideCardButton != null)
            hideCardButton.gameObject.SetActive(false);
        GameObject[] allCards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in allCards)
            Destroy(card);

        PlayerDeckManager.Instance.ClearDeck();
        Debug.Log("Canavar saldırısı bitti!");
        // 6. Yeni gece turuna başla
        if (endNightButton != null)
            endNightButton.SetActive(true);

    }
    public void StartNightPhase()
    {
        Debug.Log("Gece başladı!");
        CardSelector.ResetSelection();
        isNightPhase = true;
        opponentAttacked = false;

        turnTimer = 0f;
        nightTimerRunning = true;
        dayTimerRunning = false;
        // Kartları temizle
        GameObject[] allCards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in allCards)
        {
            AnimEffectManager anim = card.GetComponent<AnimEffectManager>();
            
            if (anim != null && !anim.isInDeck&& !anim.isFreezed)
            {
                Destroy(card);
            }
        }

        // 2. Confirm butonunu devre dışı bırak
        if (confirmButton != null)
        {

            confirmButton.gameObject.SetActive(false);
        }
        if (hideCardButton != null)
        {
            
            hideCardButton.gameObject.SetActive(false);
        }
        // 3. Seçimi sıfırla
        

        // 4. Sahne ışığını geceye çevir
        if (sceneLight != null)
        {
            sceneLight.color = nightLightColor;
            sceneLight.transform.rotation = Quaternion.Euler(nightRotation);
        }

        // 5. Deck'i sıfırla
        PlayerDeckManager.Instance.ClearDeck();
        stepIndex = 0;
        StartCoroutine(RunNextStep());

    }

    [Header("UI Elements")]
    public GameObject endNightButton;
    public void EndLateNightPhase()
    {
        Debug.Log("☀️ Sabah oldu");

        endNightButton.SetActive(false);
        StartTurn();
    }

    public void ConfirmCardSelection()
    {
        if (pendingCardObject != null)
        {
            playerDeck.AddToDeck(pendingCardObject);

            GameObject[] allCards = GameObject.FindGameObjectsWithTag("Card");

            foreach (GameObject cardObj in allCards)
            {
                if (cardObj == pendingCardObject)
                {
                    var effect = cardObj.GetComponent<AnimEffectManager>();
                    if (effect != null)
                        effect.isInDeck = true;
                }
                else
                {
                    var effect = cardObj.GetComponent<AnimEffectManager>();
                    if (effect == null || !effect.isInDeck)
                        Destroy(cardObj);
                }
            }

            pendingCardObject = null;
            CardSelector.ResetSelection();

            stepIndex++;
            StartCoroutine(RunNextStep());
        }
        else
        {
            Debug.LogWarning("Herhangi bir kart seçilmedi.");
        }
    }

    private IEnumerator RunNextStep()
    {
        List<CardType> currentSteps = isNightPhase ? nightSteps : daySteps;

        if (stepIndex < currentSteps.Count)
        {
            yield return new WaitForSeconds(delayBetweenSteps);
            cardManager.DrawCards(currentSteps[stepIndex]);
        }
        else
        {
            EnablePlacementPhase();
        }
    }
    public void OnEndDayClicked() //will change
    {
        if (!opponentAttacked)
            StartNightPhase();
        else
            Debug.Log("Opponent attacked, proceed to battle phase...");
    }
    private void EnablePlacementPhase()
    {
        Debug.Log("Placement phase started.");
        isInPlacementPhase = true;
        TileController.OnTileClicked = OnTileSelected;
        gridManager.HighlightTiles(false);
    }

    public void OnDeckCardClicked(GameObject cardObj)
    {
        if (!isInPlacementPhase) return;

        selectedCardObject = cardObj;
        var display = cardObj.GetComponent<CardDisplay3D>();
        if (display != null)
        {
            selectedPlacementCard = display.cardData;
            gridManager.HighlightTiles(true);
        }
    }

    private void OnTileSelected(TileController tile)
    {
        if (!isInPlacementPhase || selectedPlacementCard == null) return;

        // Tile boş değilse yerleştirme
        if (tile.isOccupied)
        {
            Debug.Log("Bu tile zaten dolu.");
            return;
        }

        // Kart yerleştir
        Vector3 pos = gridManager.GetTilePosition(tile);
        Instantiate(selectedPlacementCard.cardModelPrefab, pos + Vector3.up * 0.5f, Quaternion.identity);

        tile.isOccupied = true; // Bu tile artık dolu

        if (selectedCardObject != null)
            Destroy(selectedCardObject);

        PlayerDeckManager.Instance.RemoveCardObject(selectedCardObject);

        selectedPlacementCard = null;
        selectedCardObject = null;
        gridManager.HighlightTiles(false);

        CheckIfAllCardsPlaced();
    }


    private void CheckIfAllCardsPlaced()
    {
        if (PlayerDeckManager.Instance.GetDeckCount() == 0)
        {
            Debug.Log("All deck cards placed. Ending placement phase.");
            isInPlacementPhase = false;
           
        }
    }
}
