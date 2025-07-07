
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckManager : MonoBehaviour
{
    public static PlayerDeckManager Instance { get; private set; }

    public Vector2 yAndZ_Offset= new Vector2(-4.5f, 4f); 
    public float xOffsetStep = 2f;

    private List<GameObject> deckObjects = new List<GameObject>();

    
    

   
    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddToDeck(GameObject cardObject)
    {
        if (!deckObjects.Contains(cardObject))
        {
            deckObjects.Add(cardObject);

            int index = deckObjects.Count - 1;

            float xOffset = 0f;

            if (index != 0)
            {
                int direction = (index % 2 == 1) ? -1 : 1;
                int offsetIndex = (index + 1) / 2;
                xOffset = direction * offsetIndex * xOffsetStep;
            }

            AnimEffectManager effect = cardObject.GetComponent<AnimEffectManager>();
            if (effect != null)
            {
               
                effect.deckOffset = new Vector3(xOffset, yAndZ_Offset.x, yAndZ_Offset.y);
                
                effect.isInDeck = true;
            }
        }
    }


    public int GetDeckCount()
    {
        return deckObjects.Count;
    }

    public void ClearDeck()
    {
        foreach (var obj in deckObjects)
        {
            if (obj != null)
                Destroy(obj);
        }

        deckObjects.Clear();
    }

    public List<GameObject> GetDeckObjects()
    {
        return deckObjects;
    }
    public void RemoveCardObject(GameObject cardObject)
    {
        if (deckObjects.Contains(cardObject))
        {
            deckObjects.Remove(cardObject);
            Destroy(cardObject);
        }
    }

}
