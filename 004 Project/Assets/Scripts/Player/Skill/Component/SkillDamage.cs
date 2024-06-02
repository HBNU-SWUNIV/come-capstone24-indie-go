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
        // ��ų ������Ʈ �Ǵ� Prefab ������Ʈ���� CollisionHandler�� ã�� ����
        if (prefab != null)
        {
            collisionHandler = prefab.GetComponent<CollisionHandler>();//transform.parent.GetComponentInChildren<CollisionHandler>();
            if (collisionHandler != null)
            {
                

                collisionHandler.OnColliderDetected += CheckAttack;
            }
            else
            {
                Debug.LogError("CollisionHandler�� ã�� �� �����ϴ�.");
            }
        }

    }
    public void CheckAttack(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponentInChildren<IDamageable>();

        if (damageable != null)
        {
            damageable.Damage(currentSkillData.Damage);
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