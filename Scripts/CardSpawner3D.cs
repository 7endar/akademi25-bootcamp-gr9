using UnityEngine;

public class CardSpawner3D : MonoBehaviour
{
    public GameObject cardPrefab3D;
    public Transform spawnParent;
    public GameTurnManager turnManager;
    [SerializeField] GameObject CardConfirmButton;
    [SerializeField] GameObject CardHideButton;
    // Kameraya göre offset verilecek pozisyonlar
    public Vector3[] localOffsetsFromCamera = new Vector3[]
    {
        new Vector3(-4f, 0f, 8f),
        new Vector3(0f, 0f, 8f),
        new Vector3(4f, 0f, 8f)
    };

    public void SpawnCards(BaseCard[] cardsToSpawn)
    {
       
        
        CardConfirmButton.SetActive(true);
        
        CardHideButton.SetActive(true);
        for (int i = 0; i < cardsToSpawn.Length; i++)
        {
            // Kartı spawnParent altında oluştur
            GameObject newCard = Instantiate(cardPrefab3D, spawnParent);

            // Kameraya göre pozisyon alması için offset ver
            AnimEffectManager anim = newCard.GetComponent<AnimEffectManager>();
            if (anim != null)
            {
                // Offsets dizisinin dışına çıkmamak için % operatörü
                anim.localOffset = localOffsetsFromCamera[i % localOffsetsFromCamera.Length];
            }

            // Kart verisini karta uygula
            CardDisplay3D display = newCard.GetComponent<CardDisplay3D>();
            if (display != null)
            {
                display.Setup(cardsToSpawn[i]);
            }

            // Seçim davranışını ata
            CardSelector selector = newCard.AddComponent<CardSelector>();
            selector.turnManager = turnManager;
            selector.cardData = cardsToSpawn[i];
        }
    }
}
