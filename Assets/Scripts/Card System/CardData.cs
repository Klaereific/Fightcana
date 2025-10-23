using UnityEngine;

public enum CardTarget
{
    Support,
    Aggressive
}

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    [TextArea]
    public string description;
    public rarity type;
    public int id;
    public CardTarget Target;
    public CardEffect effect;
    public Sprite icon;    
}