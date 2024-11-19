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
    public Transform playerTransform;

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

    protected virtual void Update()
    {
        Core.LogicUpdate();
        stateMachine.currentState.LogicUpdate();


    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    // 플레이어 방향 갱신 메서드
    public int GetPlayerRelativePosition()
    {
        if (CheckPlayer() != null)
        {
            playerTransform = CheckPlayer();
            float direction = playerTransform.position.x - transform.position.x;

            // 오른쪽에 있으면 1, 왼쪽에 있으면 -1
            return direction > 0 ? 1 : -1;
        }

        return 0; // 플레이어가 감지되지 않을 경우 0 반환
    }

    public Transform CheckPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, 15f, LayerMasks.Player);
        return player != null ? player.transform : null;
    }
    /// <summary>
    /// 플레이어가 최대 탐지 범위 내에 있는지 확인합니다.
    /// </summary>
    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return CheckPlayerInBox(entityData.maxAgroDistance, entityData.agroHeight, entityData.maxForwardBias, Color.green);
    }

    /// <summary>
    /// 플레이어가 최소 탐지 범위 내에 있는지 확인합니다.
    /// </summary>
    public virtual bool CheckPlayerInMinAgroRange()
    {
        return CheckPlayerInBox(entityData.minAgroDistance, entityData.agroHeight, entityData.minForwardBias, Color.yellow);
    }

    /// <summary>
    /// 플레이어가 근접 공격 범위 내에 있는지 확인합니다.
    /// </summary>
    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return CheckPlayerInBox(entityData.closeRangeActionDistance, entityData.agroHeight, entityData.closeForwardBias, Color.red);
    }
    public virtual bool CheckPlayerInMeleeAttackRangeAction()
    {
        return CheckPlayerInBox(entityData.closeRangeActionDistance - 0.3f, entityData.agroHeight, entityData.closeForwardBias, Color.red);
    }

    /// <summary>
    /// 사각형 범위 내에서 플레이어를 탐지합니다.
    /// </summary>
    private bool CheckPlayerInBox(float range, float height, float forwardBias, Color debugColor)
    {
        // 사각형의 중심 위치를 몬스터의 앞쪽으로 이동
        Vector2 boxCenter = playerCheck.position + transform.right * (range / 2 + forwardBias);

        // 사각형의 크기 (가로: 탐지 거리, 세로: 감지 높이)
        Vector2 boxSize = new Vector2(range, height);

        // 감지
        Collider2D player = Physics2D.OverlapBox(boxCenter, boxSize, 0f, LayerMasks.Player);

        // 사각형의 네 모서리 계산
        Vector2 topLeft = boxCenter + new Vector2(-boxSize.x / 2, boxSize.y / 2);
        Vector2 topRight = boxCenter + new Vector2(boxSize.x / 2, boxSize.y / 2);
        Vector2 bottomLeft = boxCenter + new Vector2(-boxSize.x / 2, -boxSize.y / 2);
        Vector2 bottomRight = boxCenter + new Vector2(boxSize.x / 2, -boxSize.y / 2);

        // 사각형을 구성하는 네 개의 선을 그림
        Debug.DrawLine(topLeft, topRight, debugColor, 0.1f); // 상단
        Debug.DrawLine(topRight, bottomRight, debugColor, 0.1f); // 우측
        Debug.DrawLine(bottomRight, bottomLeft, debugColor, 0.1f); // 하단
        Debug.DrawLine(bottomLeft, topLeft, debugColor, 0.1f); // 좌측

        return player != null;
    }


    /// <summary>
    /// 디버그용으로 탐지 영역을 시각화합니다.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (playerCheck == null) return;

        // 최대 탐지 영역
        Gizmos.color = Color.green;
        DrawGizmoBox(entityData.maxAgroDistance, entityData.agroHeight, entityData.maxForwardBias);

        // 최소 탐지 영역
        Gizmos.color = Color.yellow;
        DrawGizmoBox(entityData.minAgroDistance, entityData.agroHeight, entityData.minForwardBias);

        // 근접 탐지 영역
        Gizmos.color = Color.red;
        DrawGizmoBox(entityData.closeRangeActionDistance, entityData.agroHeight, entityData.closeForwardBias);
    }

    /// <summary>
    /// 사각형 영역을 그립니다.
    /// </summary>
    private void DrawGizmoBox(float range, float height, float forwardBias)
    {
        Vector2 boxCenter = playerCheck.position + transform.right * (range / 2 + forwardBias);
        Vector2 boxSize = new Vector2(range, height);
        Gizmos.DrawWireCube(boxCenter, boxSize);
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
