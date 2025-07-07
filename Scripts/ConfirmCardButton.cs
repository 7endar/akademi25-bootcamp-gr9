using UnityEngine;

public class ConfirmCardButton : MonoBehaviour
{
    public GameTurnManager turnManager;
    [SerializeField] GameObject CardHideButton;
    public Renderer buttonRenderer;
    public Color unselectedColor = Color.gray;
    public Color selectedColor = Color.green;
    private bool isCardSelected;

    private void Start()
    {

        UpdateButtonVisual(false);
    }

    private void OnMouseDown()
    {
        if (turnManager == null) return;
        turnManager.ConfirmCardSelection();

        
        if (isCardSelected)
        {
            gameObject.SetActive(false);
            CardHideButton.SetActive(false);
            UpdateButtonVisual(false);
            isCardSelected = false;

        }
    }
    
    public void UpdateButtonVisual(bool isCardSelected)
    {
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = isCardSelected ? selectedColor : unselectedColor;
           
            if (!this.isCardSelected&& isCardSelected)
            {
                this.isCardSelected = isCardSelected;
            }
            
        }
    }
}
