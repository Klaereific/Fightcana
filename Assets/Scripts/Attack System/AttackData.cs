using System;
using UnityEngine;

/// <summary>
/// A ScriptableObject representing a single attack move.
/// Create via: Right-click in Project > Create > Fighting Game > Attack Data
/// Each attack is its own .asset file, fully editable in the Inspector.
/// Good for interpretability and adding attacks easier iterating will be better
/// :S
/// </summary>


[CreateAssetMenu(fileName = "NewAttack", menuName = "Fighting Game/Attack Data")]
public class AttackData : ScriptableObject
{
    [Header("Identity")]
    public string attackName = "New Attack";
    
    [Tooltip("Animator 'Form' integer value for this attack")]
    public int animationForm;

    [Header("Hitbox")]
    [Tooltip("Offset from player position where the hitbox spawns")]
    public Vector2 hitboxPosition = new Vector2(0.5f, 0.5f);
    
    [Tooltip("Size of the hitbox")]
    public Vector2 hitboxSize = new Vector2(1f, 0.3f);

    [Header("Damage & Stun")]
    public float damage = 5f;
    public int hitstun = 20;
    public int blockstun = 10;
    public float hitForce = 1f;
    public float blockForce = 0.5f;

    [Header("Frame Data")]
    [Tooltip("Frames before the hitbox becomes active")]
    public int startup = 5;
    
    [Tooltip("Frames the active hitbox is out")]
    public int active = 3;
    
    [Tooltip("Frames of recovery after active frames end")]
    public int recovery = 10;

    /// <summary>Total duration in frames (startup + active + recovery)</summary>
    public int TotalFrames => startup + active + recovery;

    [Header("Input Pattern")]
    [Tooltip("How many buffer frames to search backwards for the input pattern")]
    public int inputWindow = 3;
    
    [Tooltip("How many frames of gap are allowed between pattern steps")]
    public int inputTolerance = 3;

    [Tooltip("The motion input as numpad notation entries (e.g. 2,3,6 for quarter circle forward). " +
             "Use standard numpad: 1=DL, 2=D, 3=DR, 4=L, 5=Neutral, 6=R, 7=UL, 8=U, 9=UR. " +
             "Leave empty for simple normals (just the button press, no motion).")]
    public int[] motionInput = new int[0];

    // Runtime-only: the compiled byte pattern used by the input matcher.
    // Built from motionInput + the button slot this attack belongs to.
    [NonSerialized] public byte[] compiledInputs;

    /// <summary>
    /// Compiles the motionInput array into byte patterns for the input buffer matcher.
    /// Called at runtime by CharacterMoveset.Initialize().
    /// </summary>
    /// <param name="buttonBit">The button byte for the slot (e.g. InputButtons.LIGHT)</param>
    public void Compile(byte buttonBit)
    {
        if (motionInput == null || motionInput.Length == 0)
        {
            // Simple normal: just the button press, no motion
            compiledInputs = new byte[1] { buttonBit };
            return;
        }

        // Motion input: direction steps + final step includes the button
        compiledInputs = new byte[motionInput.Length];
        for (int i = 0; i < motionInput.Length; i++)
        {
            byte dirByte = NumpadToByte(motionInput[i]);
            
            // Last entry in the motion gets the button press OR'd in
            if (i == motionInput.Length - 1)
            {
                dirByte |= buttonBit;
            }

            compiledInputs[i] = dirByte;
        }
    }

    /// <summary>
    /// Converts numpad notation (1-9) to directional byte bits.
    /// 7=UL  8=U  9=UR
    /// 4=L   5=N  6=R
    /// 1=DL  2=D  3=DR
    /// </summary>
    private static byte NumpadToByte(int numpad)
    {
        switch (numpad)
        {
            case 1: return InputButtons.DOWN | InputButtons.LEFT;       // down-left
            case 2: return InputButtons.DOWN;                            // down
            case 3: return InputButtons.DOWN | InputButtons.RIGHT;      // down-right
            case 4: return InputButtons.LEFT;                            // left
            case 5: return 0;                                            // neutral
            case 6: return InputButtons.RIGHT;                           // right
            case 7: return InputButtons.UP | InputButtons.LEFT;         // up-left
            case 8: return InputButtons.UP;                              // up
            case 9: return InputButtons.UP | InputButtons.RIGHT;        // up-right
            default:
                Debug.LogWarning($"AttackData: Unknown numpad direction {numpad}, treating as neutral");
                return 0;
        }
    }
}
