using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamage : SkillComponent<SkillDamageData>, IAttackable
{
    private CollisionHandler collisionHandler;

    private Movement coreMovement;

    private Movement CoreMovement =>
        coreMovement ? coreMovement : Core.GetCoreComponent(ref coreMovement);

    protected override void Awake()
    {
        base.Awake();
        collisionHandler = transform.parent.GetComponentInChildren<CollisionHandler>();
    }
    protected override void Start()
    {
        if(collisionHandler != null)
            collisionHandler.OnColliderDetected += CheckAttack;
    }

    public void Initialize(GameObject prefab)
    {
        // 스킬 오브젝트 또는 Prefab 오브젝트에서 CollisionHandler를 찾아 참조
        if (prefab != null)
        {
            collisionHandler = prefab.GetComponent<CollisionHandler>();//transform.parent.GetComponentInChildren<CollisionHandler>();
            if (collisionHandler != null)
            {
                

                collisionHandler.OnColliderDetected += CheckAttack;
            }
            else
            {
                Debug.LogError("CollisionHandler를 찾을 수 없습니다.");
            }
        }

    }
    public void CheckAttack(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponentInChildren<IDamageable>();

        if (damageable != null)
        {
            damageable.Damage(currentSkillData.Damage * playerStats.AttackDamage);
            Debug.Log("스킬 데미지 : " + currentSkillData.Damage * playerStats.AttackDamage);
        }
        IKnockbackable knockbackable = collision.GetComponentInChildren<IKnockbackable>();
        if (knockbackable != null)
        {
            knockbackable.Knockback(currentSkillData.knockbackAngle, currentSkillData.knockbackStrength, CoreMovement.FacingDirection);
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        collisionHandler.OnColliderDetected -= CheckAttack;
    }
}