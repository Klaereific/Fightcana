using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewAttack", menuName = "FightingGame/AttackData")]
public class AttackSO : ScriptableObject
{
    [Header("Animation")]
    public string animationTrigger; 
    public int animationForm;       
    [Header("Frame Data (60 FPS)")]
    public int startupFrames; 
    public int activeFrames;   
    public int recoveryFrames; 

    public int TotalFrames => startupFrames + activeFrames + recoveryFrames;

    [Header("Hitbox Properties")]
    public Vector2 hitBoxOffset;
    public Vector2 hitBoxSize;

    [Header("Combat Stats")]
    public float damage;
    public int hitStun;      
    public int blockStun;    
    public float hitForce;   
    public float blockForce;

    [Header("Input Requirements")]
    
    public IntPair[] inputSequence; 
    public int inputWindow = 15;
    public int inputTolerance = 2;

    [Header("Gatling / Cancels")]
    public List<AttackSO> cancelList;

    [Header("Movement")]
    public float forwardImpulse; 
    public float jumpImpulse; 
}