using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("References")]
    public List<BaseCard> currentHand;
    public GameTurnManager turnManager;
    public CardSpawner3D spawner;
    [Header("will change")]
    public RaceType playerRace = RaceType.Elf; //ilerde başka yerden çek 


    public void DrawCards(CardType type)
    {
        currentHand.Clear();

        // Resources klasöründen tüm kartları yükle
        BaseCard[] allCards = Resources.LoadAll<BaseCard>("Cards");

        List<BaseCard> validCards = new List<BaseCard>();

        foreach (BaseCard card in allCards)
        {
            if (card.cardType == type && card.race == playerRace)
            {
                validCards.Add(card);
            }
        }
        int cardsToDraw = turnManager != null ? turnManager.cardsPerTurn : 3;
        for (int i = 0; i < cardsToDraw; i++)
        {
            if (validCards.Count == 0) break;

            int randIndex = Random.Range(0, validCards.Count);
            currentHand.Add(validCards[randIndex]);
            validCards.RemoveAt(randIndex);
        }

        spawner.SpawnCards(currentHand.ToArray());
    }
}
