using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance;

    public GameObject selectedCardPrefab;

    private void Awake()
    {
        Instance = this;
    }

   
    public void PlaceCardOnTile(Transform tile)
    {
        if (selectedCardPrefab == null) return;

        GameObject obj = Instantiate(selectedCardPrefab, tile.position, Quaternion.identity);
        obj.transform.SetParent(tile); // opsiyonel: düzenli hierarchy
        selectedCardPrefab = null;
    }
}
