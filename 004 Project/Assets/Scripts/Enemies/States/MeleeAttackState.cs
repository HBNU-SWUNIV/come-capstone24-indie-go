using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    AnimationToAttackCheck attackCheck;
    AnimationToPlayerDashCheck playerDashCheck;

    private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

    private Movement movement;
    private CollisionSenses collisionSenses;
    private CharacterStats<EnemyStatsData> enemyStats;
    private D_MeleeAttackState stateData;

    public float AttackDamage => enemyStats.AttackDamage; // 공격력 프로퍼티
    public Vector2 KnockbackAngle => stateData.knockbackAngle; // 넉백 각도 프로퍼티
    public float KnockbackStrength => stateData.knockbackStrength; // 넉백 강도 프로퍼티
    public int FacingDirection => Movement.FacingDirection;

    public MeleeAttackState(Entity entity, MonsterStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        attackCheck = entity.transform.GetComponentInChildren<AnimationToAttackCheck>();
        playerDashCheck = entity.transform.GetComponentInChildren<AnimationToPlayerDashCheck>();
        enemyStats = entity.transform.GetComponentInChildren<EnemyStats>();
        this.stateData = stateData;

    }


    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        //플레이어가 공격 범위에 있는지를 체크하고, 공격 trigger가 일어나기 전에 공격 범위 내를 벗어나면 카운트 증가
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        attackCheck.TriggerAttack();
    }
    public override void FinishAttack()
    {
        base.FinishAttack();

        attackCheck.FinishAttack();
    }

    public override void TriggerCheck()
    {
        base.TriggerCheck();

        playerDashCheck.TriggerCheck();
    }

    public override void FinishCheck()
    {
        base.FinishCheck();

        playerDashCheck.FinishCheck();
    }

    //방패로직 수정할 수 있음
    public override void HandleAttack(Collider2D collision)
    {
        base.HandleAttack(collision);
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
                defensiveWeapon.CheckShield(entity.gameObject, AttackDamage, KnockbackAngle, KnockbackStrength, FacingDirection);
                return;
            }
            else
            {
                GameManager.SharedCombatDataManager.SetPlayerHit(true);

                if (damageable != null)
                {
                    damageable.Damage(AttackDamage);
                }


                if (knockbackable != null)
                {
                    knockbackable.Knockback(stateData.knockbackAngle, stateData.knockbackStrength, Movement.FacingDirection);
                }
            }
            //SharedCombatDataManager.Instance.SetPlayerHit(true);

        }
    }
    private bool IsShieldBlockingAttack(Transform playerTransform, Transform shieldTransform, int playerFacingDirection)
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
