using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : Entity
{
    public FE_IdleState idleState { get; private set; }
    public FE_MoveState moveState { get; private set; }
    public FE_PlayerDetectedState playerDetectedState { get; private set; }
    public FE_LookForPlayerState lookForPlayerState { get; private set; }
    public FE_DodgeState dodgeState { get; private set; }
    public FE_RangedAttackState rangedAttackState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerStateData;
    [SerializeField]
    private D_StunState stunStateData;

    [SerializeField]
    public D_DodgeState dodgeStateData;
    [SerializeField]
    private D_RangeAttackState rangeAttackStateData;

    [SerializeField]
    private Transform rangedAttackPosition;

    public override void Awake()
    {
        base.Awake();

        moveState = new FE_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new FE_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new FE_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);

        lookForPlayerState = new FE_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        stunState = new FE_StunState(this, stateMachine, "stun", stunStateData, this);
        dodgeState = new FE_DodgeState(this, stateMachine, "dodge", dodgeStateData, this);
        rangedAttackState = new FE_RangedAttackState(this, stateMachine, "rangedAttack", rangedAttackPosition, rangeAttackStateData, this);

        maxParryStunStack = 1;
    }

    private void Start()
    {
        stateMachine.Initialize(moveState);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        stateMachine.Initialize(idleState);
    }
}
