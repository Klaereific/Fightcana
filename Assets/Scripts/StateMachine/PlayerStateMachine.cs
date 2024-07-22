using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateManager<PlayerStateMachine.EPlayerState>
{
    public enum EPlayerState{
        Idle,
        Walk,
        Duck,
        Jump
    }

    [SerializeField] private CustomRigidbody2D customRb;
    [SerializeField] private float width;
    [SerializeField] private float height;
    
    private PlayerStateContext _context;

    // Start is called before the first frame update
    void Awake()
    {
        _context = new PlayerStateContext(customRb,width,height);
        InitializeStates();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeStates()
    {
        States.Add(EPlayerState.Idle, new Player_Idle(_context,EPlayerState.Idle));
        States.Add(EPlayerState.Walk, new Player_Walk(_context,EPlayerState.Walk));
        States.Add(EPlayerState.Duck, new Player_Duck(_context,EPlayerState.Duck));
        States.Add(EPlayerState.Jump, new Player_Jump(_context,EPlayerState.Jump));
        CurrentState = States[EPlayerState.Idle];
    }
}
