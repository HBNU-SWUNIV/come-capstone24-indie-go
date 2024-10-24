using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Movement Movement { get => movement ?? Core.GetCoreComponent(ref movement); }

    private Movement movement;
    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }
    public D_Entity entityData;
    public StunState stunState;
    private EnemyStats stats;
    public bool IsKnockbackable { get; set; } = true;

    private int currentParryStunStack;
    protected int maxParryStunStack;
    protected float parryStunTimer;
    public MonsterStateMachine stateMachine { get; protected set; }

    public int lastDamageDirection { get; private set; }

    Transform effectParticles;
    Transform elementParticles;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform groundCheck;


  
    protected bool isDead;

    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();

        Anim = GetComponent<Animator>();
        atsm = GetComponent<AnimationToStateMachine>();
        stats = GetComponentInChildren<EnemyStats>();

        stateMachine = new MonsterStateMachine();

        currentParryStunStack = 0;

        effectParticles = transform.Find("Particles");
        elementParticles = transform.Find("Core/Element");
    }

    public virtual void Update()
    {
        Core.LogicUpdate();
        stateMachine.currentState.LogicUpdate();
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

    public void AddcurrentParryStunStack(float stunTime)
    {
        ++currentParryStunStack;
        parryStunTimer = Time.time;
        StopCoroutine("CheckParryStunTimer");
        StartCoroutine("CheckParryStunTimer", stunTime);
    }

    private IEnumerator CheckParryStunTimer(float stunTime)
    {
        Debug.Log($"currentParryStunStack : {currentParryStunStack}, maxParryStunStack : {maxParryStunStack}");
        while (currentParryStunStack > 0)
        {
            if (currentParryStunStack >= maxParryStunStack)
            {
                stunState.SetParryStunTime(stunTime);
                stunState.stun = true;
                currentParryStunStack = 0;
            }
            if (parryStunTimer + entityData.parryStundurationTime <= Time.time)
                currentParryStunStack = 0;
            yield return 1f;
        }
        Debug.Log("스턴 스택 초기화");
    }

    protected void OnEnable()
    {
        if (effectParticles != null)
        {
            RemoveAllChildObjects(effectParticles);
        }
        if (elementParticles != null)
        {
            RemoveAllChildObjects(elementParticles);
        }
    }
    protected virtual void OnDisable()
    {
        Movement?.SetVelocityZero();
        IsKnockbackable = true;
        stats.ChangeElement(Element.None);


    }
    protected void RemoveAllChildObjects(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Debug.Log("child : " + child.name);
            Destroy(child.gameObject);
        }
    }
}
