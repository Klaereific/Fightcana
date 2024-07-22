using UnityEngine;

public class Player_Jump : PlayerState
{
    public Player_Jump(PlayerStateContext context, PlayerStateMachine.EPlayerState estate) : base(context, estate)
    {
        PlayerStateContext Context = context;
    }

    public override void EnterState() {}
    public override void ExitState() {}
    public override void UpdateState() {}
    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) {}
    public override void OnTriggerStay(Collider other) {}
    public override void OnTriggerExit(Collider other) {}
}
