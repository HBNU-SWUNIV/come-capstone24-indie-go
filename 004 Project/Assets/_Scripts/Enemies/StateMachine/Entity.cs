using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private Movement Movement { get => movement ?? Core.GetCoreComponent(ref movement); }

    private Movement movement;
    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }
    public D_Entity entityData;
    public StunState stunState;
    public MonsterStateMachine stateMachine { get; protected set; }

    public int lastDamageDirection { get; private set; }


    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform groundCheck;


    private float currentStunResistance;
    private float lastDamageTime;


    protected bool isStunned;
    protected bool isDead;

    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();
       // entityData = new D_Entity();


        Anim = GetComponent<Animator>();
        atsm = GetComponent<AnimationToStateMachine>();

        stateMachine = new MonsterStateMachine();
    }

    public virtual void Update()
    {
        Core.LogicUpdate();
        stateMachine.currentState.LogicUpdate();
        
        if(Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    public virtual GameObject CheckPlayer()
    {
        return GameObject.FindWithTag("Player");
    }
    public virtual bool CheckPlayerInMinAgroRange()
    {
        Debug.DrawRay(playerCheck.position, transform.right * entityData.minAgroDistance, Color.red, 1.0f);
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.minAgroDistance, LayerMasks.Player);// entityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.maxAgroDistance, LayerMasks.Player);
    }
    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.closeRangeActionDistance, LayerMasks.Player);
    }
    
    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }
}
