using UnityEngine;

/// <summary>
/// A ScriptableObject that defines a character's complete moveset.
/// Create via: Right-click in Project > Create > Fighting Game > Character Moveset
/// 
/// SETUP:
///   1. Create AttackData assets for each move (Create > Fighting Game > Attack Data)
///   2. Create a CharacterMoveset asset (Create > Fighting Game > Character Moveset)
///   3. Drag your AttackData assets into the appropriate slots below
///   4. Assign this moveset to your CharacterParameters component
///
/// ORDERING MATTERS:
///   Within each array, attacks are checked top-to-bottom.
///   Put complex motion inputs FIRST (e.g. 236+Light before standing Light)
///   so they get priority. The simple normal should always be LAST in the array.
/// 
/// </summary>


[CreateAssetMenu(fileName = "NewMoveset", menuName = "Fighting Game/Character Moveset")]
public class CharacterMoveset : ScriptableObject
{
    [Header("Character Info")]
    public string characterName = "New Character";

    [Header("Standing Attacks (Ground + Button)")]
    [Tooltip("Light button attacks while standing. Order: specials first, normal last.")]
    public AttackData[] standingLight;

    [Tooltip("Medium button attacks while standing.")]
    public AttackData[] standingMedium;

    [Tooltip("Heavy button attacks while standing.")]
    public AttackData[] standingHeavy;

    [Tooltip("Special button attacks while standing.")]
    public AttackData[] standingSpecial;

    [Header("Crouching Attacks (Crouch + Button)")]
    public AttackData[] crouchingLight;
    public AttackData[] crouchingMedium;
    public AttackData[] crouchingHeavy;
    public AttackData[] crouchingSpecial;

    [Header("Aerial Attacks (Air + Button)")]
    public AttackData[] aerialLight;
    public AttackData[] aerialMedium;
    public AttackData[] aerialHeavy;
    public AttackData[] aerialSpecial;

    /// <summary>
    /// Compiles all attack input patterns at runtime.
    /// Must be called once during initialization (e.g. in CharacterParameters.InitializeAttacks).
    /// Each attack's motionInput gets compiled into byte patterns with the correct button bit.
    /// </summary>
    
    public void Initialize()
    {
        CompileSlot(standingLight,    InputButtons.LIGHT);
        CompileSlot(standingMedium,   InputButtons.MEDIUM);
        CompileSlot(standingHeavy,    InputButtons.HEAVY);
        CompileSlot(standingSpecial,  InputButtons.SPECIAL);

        CompileSlot(crouchingLight,   InputButtons.LIGHT);
        CompileSlot(crouchingMedium,  InputButtons.MEDIUM);
        CompileSlot(crouchingHeavy,   InputButtons.HEAVY);
        CompileSlot(crouchingSpecial, InputButtons.SPECIAL);

        CompileSlot(aerialLight,      InputButtons.LIGHT);
        CompileSlot(aerialMedium,     InputButtons.MEDIUM);
        CompileSlot(aerialHeavy,      InputButtons.HEAVY);
        CompileSlot(aerialSpecial,    InputButtons.SPECIAL);
    }

    private void CompileSlot(AttackData[] attacks, byte buttonBit)
    {
        if (attacks == null) return;
        foreach (var attack in attacks)
        {
            if (attack != null)
            {
                attack.Compile(buttonBit);
            }
        }
    }

    /// <summary>
    /// Gets the attack array for a given button, based on stance.
    /// </summary>
    
    
    public AttackData[] GetAttacksForButton(byte buttonBit, string stance)
    {
        switch (stance)
        {
            case "Crouching":
                if (buttonBit == InputButtons.LIGHT)   return crouchingLight;
                if (buttonBit == InputButtons.MEDIUM)  return crouchingMedium;
                if (buttonBit == InputButtons.HEAVY)   return crouchingHeavy;
                if (buttonBit == InputButtons.SPECIAL) return crouchingSpecial;
                break;

            case "InAir":
                if (buttonBit == InputButtons.LIGHT)   return aerialLight;
                if (buttonBit == InputButtons.MEDIUM)  return aerialMedium;
                if (buttonBit == InputButtons.HEAVY)   return aerialHeavy;
                if (buttonBit == InputButtons.SPECIAL) return aerialSpecial;
                break;

            default: // "Grounded", "Idle", standing
                if (buttonBit == InputButtons.LIGHT)   return standingLight;
                if (buttonBit == InputButtons.MEDIUM)  return standingMedium;
                if (buttonBit == InputButtons.HEAVY)   return standingHeavy;
                if (buttonBit == InputButtons.SPECIAL) return standingSpecial;
                break;
        }

        return null;
    }
}
