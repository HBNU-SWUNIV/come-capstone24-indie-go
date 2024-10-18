using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonsterState
{
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    protected CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

    private Movement movement;
    private CollisionSenses collisionSenses;

    protected CharacterStats<EnemyStatsData> enemyStats;

    protected Collider2D collision;
    protected bool isAnimationFinished;
    protected bool isPlayerInMinAgroRange;
    protected D_AttackState stateData;


    public AttackState(Entity entity, MonsterStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        enemyStats = entity.transform.GetComponentInChildren<EnemyStats>();

    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        entity.atsm.attackState = this;
        isAnimationFinished = false;


    }

    public override void Exit()
    {
        base.Exit();


    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement?.SetVelocityX(0f);

        if (entity.stunState.stun)
            stateMachine.ChangeState(entity.stunState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public virtual void TriggerAttack()
    {
    }
    public virtual void FinishAttack()
    {
        isAnimationFinished = true;
    }

    public virtual void TriggerCheck()
    {

    }

    public virtual void FinishCheck()
    {

    }
    public virtual void HandleAttack(Collider2D collision)
    {
        if (stateData == null)
            stateData = new D_AttackState();
        this.collision = collision;

        if (collision != null)
        {

            IDamageable damageable = collision.GetComponentInChildren<IDamageable>();
            IKnockbackable knockbackable = collision.GetComponentInChildren<IKnockbackable>();

            DefensiveWeapon defensiveWeapon = collision.GetComponentInChildren<DefensiveWeapon>();
            //패링 성공 시 패링 키 홀드하고 있으면 어떻게 할지 고민하기.
            if (defensiveWeapon != null && defensiveWeapon.isDefending && IsShieldBlockingAttack(collision.transform, defensiveWeapon.transform, defensiveWeapon.GetPlayerShieldState().Movement.FacingDirection))
            {
                // Debug.Log("방패로 막음!");
                defensiveWeapon.isDefending = false;
                defensiveWeapon.CheckShield(entity.gameObject, enemyStats.AttackDamage, stateData.knockbackAngle, stateData.knockbackStrength, Movement.FacingDirection);
                return;
            }
            else
            {
                if (!GameManager.SharedCombatDataManager.IsPlayerNotEnterHitState)
                    GameManager.SharedCombatDataManager.SetPlayerHit(true);

                if (damageable != null)
                {
                    damageable.NonElementDamage(enemyStats.AttackDamage, collision.transform);
                }

                if (!GameManager.SharedCombatDataManager.IsPlayerNotKnockback)
                {
                    if (knockbackable != null)
                    {
                        knockbackable.Knockback(stateData.knockbackAngle, stateData.knockbackStrength, Movement.FacingDirection);
                    }
                }
            }
            //SharedCombatDataManager.Instance.SetPlayerHit(true);

        }
    }
    protected bool IsShieldBlockingAttack(Transform playerTransform, Transform shieldTransform, int playerFacingDirection)
    {
        Vector2 attackDirection = entity.transform.position - playerTransform.position;
        Vector2 shieldDirection = playerFacingDirection == 1 ? Vector2.right : Vector2.left; // Assuming the shield faces up

        // Calculate the angle between the attack direction and the shield direction
        float angle = Vector2.Angle(attackDirection, shieldDirection);

        // Define a blocking angle threshold
        float blockingAngle = 90f; // Example: 45 degrees

        return angle <= blockingAngle;
    }
}
