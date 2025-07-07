using System.Collections.Generic;
using UnityEngine;

public class HideCardButton : MonoBehaviour
{
    [Header("Hold Settings")]
    public float holdDelay = 0.3f;

    public Transform cardsParent;
    public GameObject confirmButton;

    private float holdTimer = 0f;
    private bool isHolding = false;
    private bool canTrigger = false;

    private List<GameObject> cardObjects = new List<GameObject>();
   
    void Update()
    {
        if (!canTrigger) return;

        holdTimer += Time.deltaTime;

        if (!isHolding && holdTimer >= holdDelay)
        {
            isHolding = true;
            SetVisibility(false); // hide
        }
    }

    private void OnMouseDown()
    {
        // Güncel kartları topla
        RefreshCardList();

        canTrigger = true;
        holdTimer = 0f;
    }

    private void OnMouseUp()
    {
        if (isHolding)
        {
            SetVisibility(true); // show
        }

        isHolding = false;
        canTrigger = false;
        holdTimer = 0f;
    }

    void SetVisibility(bool visible)
    {
        foreach (GameObject obj in cardObjects)
        {
            if (obj != null) obj.SetActive(visible);
        }

        if (confirmButton != null)
            confirmButton.SetActive(visible);
    }

    void RefreshCardList()
    {
        cardObjects.Clear();

        if (cardsParent != null)
        {
            foreach (Transform card in cardsParent)
            {
                if (card != null&&!card.GetComponent<AnimEffectManager>().isInDeck)
                    cardObjects.Add(card.gameObject);
            }
        }
    }
}
