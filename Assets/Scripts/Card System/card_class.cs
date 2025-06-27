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

    public List<Card> hand = new List<Card>();
    public int HandSize = 4;

    public void DrawCard()
    {
        if (hand.Count >= HandSize) return;
        Card drawn = deck.Draw();
        if (drawn != null)
        {
            hand.Add(drawn);
        }
    }
    public void DiscardCard(Card card)
    {
        if (hand.Contains(card))
        {
            hand.Remove(card);
            discard.Add(card);
        }    
    }       
}
//One bar or two bars. discard graveyard they do not come back.
//Order: Cards(total) -> Deck -> Hand -> Discard -> Graveyard