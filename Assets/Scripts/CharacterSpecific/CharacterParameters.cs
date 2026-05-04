using UnityEngine;

/// <summary>
/// Attach this to your player GameObject.
/// Assign a CharacterMoveset ScriptableObject in the Inspector.
/// This replaces the old per-character attack array setup.
/// </summary>
/// 
public class CharacterParameters : MonoBehaviour
{
    [Header("Moveset")]
    [Tooltip("Drag your CharacterMoveset ScriptableObject here")]
    public CharacterMoveset moveset;

    public AttackData[] gWest_attackDict  => moveset?.standingLight;
    public AttackData[] gNorth_attackDict => moveset?.standingMedium;
    public AttackData[] gEast_attackDict  => moveset?.standingHeavy;
    public AttackData[] gSouth_attackDict => moveset?.standingSpecial;

    public AttackData[] aWest_attackDict  => moveset?.aerialLight;
    public AttackData[] aNorth_attackDict => moveset?.aerialMedium;
    public AttackData[] aEast_attackDict  => moveset?.aerialHeavy;
    public AttackData[] aSouth_attackDict => moveset?.aerialSpecial;

    public AttackData[] cWest_attackDict  => moveset?.crouchingLight;
    public AttackData[] cNorth_attackDict => moveset?.crouchingMedium;
    public AttackData[] cEast_attackDict  => moveset?.crouchingHeavy;
    public AttackData[] cSouth_attackDict => moveset?.crouchingSpecial;

    /// <summary>
    /// Compiles all attack input patterns. Call once at startup.
    /// </summary>
    
    public void InitializeAttacks()
    {
        if (moveset == null)
        {
            Debug.LogError($"CharacterParameters on {gameObject.name}: No moveset assigned!");
            return;
        }

        moveset.Initialize();
        Debug.Log($"Moveset '{moveset.characterName}' initialized for {gameObject.name}");
    }
}
