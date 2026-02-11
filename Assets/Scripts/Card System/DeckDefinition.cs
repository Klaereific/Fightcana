using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "Cards/Deck Definition")]
public class DeckDefinition : ScriptableObject
{
    public List<CardData> cards;
}