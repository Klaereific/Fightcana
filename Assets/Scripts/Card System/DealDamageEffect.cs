using UnityEngine;

[CreateAssetMenu(fileName = "DealDamageEffect", menuName = "Card Effects/Deal Damage")]
public class DealDamageEffect : CardEffect
{
    public float damage;
    public int hitstun = 0;
    public float hitForce = 0f;

    public override void Execute(Player user, Player target)
    {
        if (target == null) return;
        target.TakeDamage(damage, hitstun, hitForce);
    }
}