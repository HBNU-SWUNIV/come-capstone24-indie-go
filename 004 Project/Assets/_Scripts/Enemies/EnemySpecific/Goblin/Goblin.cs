using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Entity
{
    public GB_IdleState idleState { get; private set; }
    public GB_MoveState moveState { get; private set; }
    public GB_PlayerDetectedState playerDetectedState { get; private set; }
    public GB_ChargeState chargeState { get; private set; }

    public GB_MeleeAttackState1 meleeAttackState1 { get; private set; }
    public GB_MeleeAttackState2 meleeAttackState2 { get; private set; }

    public GB_LookForPlayerState lookForPlayerState { get; private set; }
    public GB_DeadState deadState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_MeleeAttackState meleeAttackStateData1;
    [SerializeField]
    private D_MeleeAttackState meleeAttackStateData2;
    [SerializeField]
    private D_LookForPlayer lookForPlayerStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;

    public override void Awake()
    {
        base.Awake();

        moveState = new GB_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new GB_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new GB_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        chargeState = new GB_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new GB_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        stunState = new GB_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new GB_DeadState(this, stateMachine, "dead", deadStateData, this);
        meleeAttackState1 = new GB_MeleeAttackState1(this, stateMachine, "meleeAttack1", meleeAttackStateData1, this);
        meleeAttackState2 = new GB_MeleeAttackState2(this, stateMachine, "meleeAttack2", meleeAttackStateData2, this);

    }

    private void Start()
    {
        stateMachine.Initialize(moveState);
    }
}
