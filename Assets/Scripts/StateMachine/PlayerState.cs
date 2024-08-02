using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : BaseState<PlayerStateMachine.EPlayerState>
{
    protected PlayerStateContext Context; 
    protected PlayerStateMachine.EPlayerState nextStateKey;

    public PlayerState(PlayerStateContext context, PlayerStateMachine.EPlayerState stateKey) : base(stateKey)
    {
        Context = context;
        nextStateKey = stateKey;
    }
}
