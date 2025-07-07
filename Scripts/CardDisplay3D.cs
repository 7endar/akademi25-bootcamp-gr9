using UnityEngine;
using TMPro;

public class CardDisplay3D : MonoBehaviour
{
    [Header("Card Faces")]
    public MeshRenderer frontRenderer;
    public MeshRenderer backRenderer;

    [Header("Text Elements")]
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    [Header("Dynamic Data")]
    public BaseCard cardData;

    public void Setup(BaseCard card)
    {
        cardData = card;

        // Set artwork as material on front face
        if (card.artwork != null && frontRenderer != null)
        {
            Material newMat = new Material(frontRenderer.material);
            newMat.mainTexture = card.artwork.texture;
            frontRenderer.material = newMat;
        }

        // Set text on back face
        if (nameText != null) nameText.text = card.cardName;
        if (descriptionText != null) descriptionText.text = card.description;
    }
}
