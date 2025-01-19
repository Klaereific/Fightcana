
using System;
using UnityEngine;
using System.Reflection;

public class AnimationDurationController : MonoBehaviour
{
    public Animator animator; // Assign the Animator in the Inspector
    private string animationName; // Name of the animation state
    private float desiredDuration = 1.0f; // Desired duration in seconds
    public GameObject playerGO;


    void Start()
    {
        CharacterParameters cp = playerGO.GetComponent<CharacterParameters>();
        // Get all attack arrays 
        Type type = cp.GetType();

        // Get all fields of the object
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            // Check if the field's type matches the specified type
            if (field.FieldType == typeof(Attack[]))
            {
                // Get the value of the field
                Attack[] dict = (Attack[])field.GetValue(cp);
                foreach (var attack in dict)
                {
                    SetAnimationDuration(attack._animationKey, attack._total_duration);
                }

            }
        }

        SetAnimationDuration(animationName, desiredDuration);
    }

    public void SetAnimationDuration(string stateName, float newDuration)
    {
        // Get the Animator's RuntimeAnimatorController
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

        foreach (AnimationClip clip in controller.animationClips)
        {
            if (clip.name == stateName)
            {
                // Calculate the required speed
                float originalDuration = clip.length;
                float newSpeed = originalDuration / (60/newDuration);

                // Set the Animator's speed
                animator.SetFloat("SpeedMultiplier", newSpeed);
                Debug.Log($"Set {stateName} to play in {newDuration}s with speed {newSpeed}");
                return;
            }
        }

        Debug.LogWarning($"Animation {stateName} not found!");
    }


}

