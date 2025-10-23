using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardLibrary", menuName = "CardLibrary")]

public class CardLibrary : ScriptableObject
{
    public List<CardData> cards;
    public void AddCard(CardData card)
    {
        cards.Add(card);
    }
}