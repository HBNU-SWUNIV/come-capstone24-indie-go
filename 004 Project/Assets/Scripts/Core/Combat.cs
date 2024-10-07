using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{

    [SerializeField] private GameObject damageParticles;

    private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

    private ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent(ref particleManager);

    private Movement movement;
    private CollisionSenses collisionSenses;
    private ICharacterStats stats;
    private ParticleManager particleManager;


    [SerializeField] private float maxKnockbackTime = 0.2f;

    private bool isKnockbackActive;
    private float knockbackStartTime;

    protected override void Awake()
    {
        base.Awake();
        stats = transform.root.GetComponentInChildren<ICharacterStats>();
        if (stats == null)
            Debug.Log("stats 빔");
    }

    public override void LogicUpdate()
    {
        CheckKnockback();
    }


    public void Damage(float amount)
    {
       // Debug.Log(core.transform.parent.name + " 피격");
       // 주는 피해량 계산.  방어력, 받는피해 감소 계산
        stats?.DecreaseHealth(amount);
        ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        Movement?.SetVelocity(angle, strength, direction);
        Movement.CanSetVelocity = false;
        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    private void CheckKnockback()
    {
        if(isKnockbackActive
            && ((Movement?.CurrentVelocity.y <= 0.01f && CollisionSenses.Ground)
                || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            isKnockbackActive = false;
            Movement.CanSetVelocity = true;
        }
    }

    public void DamageWithShield(float amount)
    {
        Damage(amount / 2);
    }

    public void KnockbackWithShield(Vector2 angle, float strength, int direction)
    {
        Knockback(angle, strength / 2, direction);
    }
}

