using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Execute(Player user, Player target);
}