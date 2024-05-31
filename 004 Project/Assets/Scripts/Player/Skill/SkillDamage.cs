using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamage : SkillComponent<SkillDamageData>, IAttackable
{
    private CollisionHandler collisionHandler;

    protected override void Awake()
    {
        base.Awake();
        // ��ų ������Ʈ �Ǵ� Prefab ������Ʈ���� CollisionHandler�� ã�� ����
        collisionHandler = gameObject.GetComponent<CollisionHandler>();
        Debug.Log(collisionHandler != null);
    }
    protected override void Start()
    {

        collisionHandler.OnColliderDetected += CheckAttack;
    }
    public void CheckAttack(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(currentSkillData.Damage);
        }
        /*
        if(collision.TryGetComponent(out IKnockbackable knockable))
        {
            knockable.Knockback(currentSkillData.)
        }
        */
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        collisionHandler.OnColliderDetected -= CheckAttack;
    }
}