using UnityEngine;

public class CardSelector : MonoBehaviour
{
    public GameTurnManager turnManager;
    public BaseCard cardData;

    private static CardSelector currentSelected;
    private AnimEffectManager animEffectManager;
    private Renderer rend;
    private Color originalColor;
    public Color selectedColor = Color.yellow;

    
    

    void Start()
    {
        animEffectManager = GetComponent<AnimEffectManager>();
        rend = GetComponentInChildren<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    private void OnMouseDown()
    {
        if (animEffectManager == null) return;

        // Eğer bu kart zaten deck'teyse, sahadaki konumlandırma için tıklanmıştır
        if (animEffectManager.isInDeck)
        {
            if (turnManager != null)
                turnManager.OnDeckCardClicked(this.gameObject);
        }
        else
        {
            // Yeni kart seçim süreci
            if (currentSelected != null && currentSelected != this)
                currentSelected.Deselect();

            currentSelected = this;
            Select();

            if (turnManager != null)
                turnManager.SetPendingCard(this.gameObject);
        }
    }

    void Select()
    {
        if (rend != null)
            rend.material.color = selectedColor;

        if (GameTurnManager.confirmButton != null)
            GameTurnManager.confirmButton.UpdateButtonVisual(true);
    }

    void Deselect()
    {
        if (rend != null)
            rend.material.color = originalColor;

        if (GameTurnManager.confirmButton != null)
            GameTurnManager.confirmButton.UpdateButtonVisual(false);
    }

    public static void ResetSelection()
    {
        if (currentSelected != null)
        {
            currentSelected.Deselect();
            currentSelected = null;
        }

        if (GameTurnManager.confirmButton != null)
            GameTurnManager.confirmButton.UpdateButtonVisual(false);
    }
}
