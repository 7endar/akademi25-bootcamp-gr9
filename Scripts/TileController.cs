using UnityEngine;

public class TileController : MonoBehaviour
{
    public bool isOccupied = false;

    private Renderer tileRenderer;
    private Color defaultColor;
    public Color canColor = Color.green;
    public Color canNotColor = Color.red;

    public bool isActive = false; // Clickable?
    public static System.Action<TileController> OnTileClicked;

    void Start()
    {
        tileRenderer = GetComponent<Renderer>();
        defaultColor = tileRenderer.material.color;
    }

    public void SetActive(bool active)
    {
        isActive = active;

        if (active)
        {
            tileRenderer.material.color = isOccupied ? canNotColor : canColor;
        }
        else
        {
            tileRenderer.material.color = defaultColor;
        }
    }

    private void OnMouseDown()
    {
        if (isActive && OnTileClicked != null)
        {
            OnTileClicked(this);
        }
    }

    public void PlaceBuilding(GameObject buildingPrefab)
    {
        if (isOccupied) return;

        Instantiate(buildingPrefab, transform.position, Quaternion.identity);
        isOccupied = true;
    }


}
