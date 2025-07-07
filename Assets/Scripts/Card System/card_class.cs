using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum rarity
{
    common,
    legendary
}


[Serializable]
public class Card
{
    public string name;
    public string description;
    public rarity type;
}
public class CardCollection
{
    public int MaxDeck = 15;

    public Queue<Card> cards = new Queue<Card>();
    public bool Add(Card card)
    {
        if (cards.Count >= MaxDeck)
        {
            return false;
        }
        cards.Enqueue(card);
        return true; 
    }
    public void Remove(Card card) => cards.Dequeue();

    public Card Draw()
    {
        if (cards.Count == 0)
        {
            return null;
        }
        return cards.Dequeue();
    }
}

public class CardManager : MonoBehaviour
{
    public CardCollection deck = new CardCollection();
    public CardCollection discard = new CardCollection();
    public Queue<Card> graveyard = new Queue<Card>();

    public List<Card> hand = new List<Card>();
    public int HandSize = 4;
    public int DiscardSize = 4;

    void Start()
    {
        // Hardcoded deck: 15 cards named and described 1 to 15
        List<Card> allCards = new List<Card>();
        for (int i = 1; i <= 15; i++)
        {
            allCards.Add(new Card { name = i.ToString(), description = i.ToString(), type = rarity.common });
        }

        // Shuffle with a fixed seed for determinism
        System.Random rng = new System.Random(12345); // Use any fixed seed
        int n = allCards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = allCards[k];
            allCards[k] = allCards[n];
            allCards[n] = value;
        }
        foreach (var card in allCards)
            deck.Add(card);

        // Draw initial hand
        for (int i = 0; i < HandSize; i++)
            DrawCard();
    }

    public void DrawCard()
    {
        if (hand.Count >= HandSize) return;
        Card drawn = deck.Draw();
        if (drawn != null)
            hand.Add(drawn);
    }

    public void DiscardCard(Card card)
    {
        if (hand.Contains(card))
        {
            hand.Remove(card);

            // FIFO discard logic
            if (discard.cards.Count >= DiscardSize)
            {
                Card oldest = discard.cards.Dequeue();
                graveyard.Enqueue(oldest);
            }
            discard.Add(card);
        }
    }
}
//One bar or two bars. discard graveyard they do not come back.
//Order: Cards(total) -> Deck -> Hand -> Discard -> Graveyard