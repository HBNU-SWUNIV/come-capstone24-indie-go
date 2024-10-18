using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class CharacterStats<T> : MonoBehaviour, ICharacterStats where T : CharacterStatsData
{
    public event Action OnHealthZero;

    [SerializeField] protected int id;
    [SerializeField] protected int curHp;
    [SerializeField] protected int maxHp;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float defense;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Element element;
    protected Animator animator;

    public int Id { get => id; set => id = value; }

    public int CurHp { get => curHp; set => curHp = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public float AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float Defense { get => defense; set => defense = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    // 각 속성의 레벨을 저장할 변수들
    protected int fireLevel = 1;
    protected int iceLevel = 1;
    protected int landLevel = 1;
    protected int lightLevel = 1;
    protected float baseAttackSpeed;
    protected float baseMoveSpeed;

    protected float slowEffectMultiplier = 1f; // 둔화 효과의 누적 배율
    protected float attackSpeedModifier = 1f; // 공격 속도에 대한 다른 효과의 배율
    protected float moveSpeedModifier = 1f;   // 이동 속도에 대한 다른 효과의 배율
    protected float attackSpeedSlowMultiplier = 1f; // IceEffect의 누적 둔화 배율
    protected float moveSpeedSlowMultiplier = 1f;   // IceEffect의 누적 둔화 배율
    protected float adjustStatsAttackSpeed = 1f;    // playerType에 따른 공격 속도 배율
    protected float adjustStatsMoveSpeed = 1f;      // playerType에 따른 이동 속도 배율

    protected float effectAttackSpeed;
    protected float effectMoveSpeed;
    protected bool OnsetStats;
    public float TotalMoveSpeedMultiplier => moveSpeedModifier * moveSpeedSlowMultiplier;

    public Element Element { get => element; set => element = value; }

    private ElementalComponent elementalComponent;

    private float basedamage;

    protected virtual void Awake()
    {
        Element = Element.None;

        animator = transform.root.GetComponent<Animator>();

        elementalComponent = transform.parent.GetComponentInChildren<ElementalComponent>();

        OnsetStats = false;
    }

    protected abstract void SetStat();

    protected virtual void SetStatsData(T stats)
    {
        curHp = stats.curHp;
        maxHp = stats.maxHp;
        attackDamage = stats.attackDamage;
        attackSpeed = stats.attackSpeed;
        defense = stats.defense;
        moveSpeed = stats.moveSpeed;

        basedamage = attackDamage;
        baseMoveSpeed = moveSpeed;
        baseAttackSpeed = attackSpeed;
       // UpdateAnimatorSpeed();
        OnsetStats = true;
    }

    public bool DecreaseHealth(float amount)
    {
        float damage = Mathf.Max(0, amount); // Damage 부분은 따로 계산하는 로직을 구현해서 최종 데미지를 넣을 예정.
        CurHp -= (int)damage;
        Debug.Log(gameObject.transform.root.name + " 남은 체력 : " + CurHp);
        if (CurHp <= 0)
        {
            CurHp = 0;
            OnHealthZero?.Invoke();
            return false;
        }
        return true;
    }

    public void IncreaseHealth(float amount)
    {
        if (!IsHpMax(amount))
        {
            int increace = (int)(amount * maxHp);
            if (increace <= 0)
                increace = 1;
            CurHp = Mathf.Clamp(CurHp + increace, 0, maxHp);
        }
    }
    public bool IsHpMax(float amount)
    {
        if (CurHp /*+ (int)(amount * maxHp)*/ >= maxHp)
            return true;
        return false;
    }
    public void ChangeDamage(float currentDamage)
    {
        attackDamage *= (1 + currentDamage);
    }
    public void ReturnDamage()
    {
        attackDamage = basedamage;
    }

    private void UpdateAttackSpeed()
    {
        attackSpeed = baseAttackSpeed * attackSpeedModifier * adjustStatsAttackSpeed * attackSpeedSlowMultiplier;
        UpdateAnimatorAttackSpeed();
    }

    private void UpdateMoveSpeed()
    {
        moveSpeed = baseMoveSpeed * moveSpeedModifier * adjustStatsMoveSpeed * moveSpeedSlowMultiplier;
        UpdateAnimatorMoveSpeed();
    }
    // 다른 효과에 의한 공격 속도 변경 (예: Land 속성)
    public void ModifyAttackSpeed(float multiplier)
    {
        attackSpeedModifier *= multiplier;
        UpdateAttackSpeed();
    }
    // IceEffect에 의한 공격 속도 둔화 적용
    public void ApplyAttackSpeedSlow(float multiplier)
    {
        attackSpeedSlowMultiplier *= multiplier;
        UpdateAttackSpeed();

    }

    // IceEffect에 의한 공격 속도 둔화 해제
    public void ResetAttackSpeedSlow()
    {
        attackSpeedSlowMultiplier = 1f;
        UpdateAttackSpeed();
    }
    // 다른 효과에 의한 이동 속도 변경
    public void ModifyMoveSpeed(float multiplier)
    {
        moveSpeedModifier *= multiplier;
        UpdateMoveSpeed();
    }

    // IceEffect에 의한 이동 속도 둔화 적용
    public void ApplyMoveSpeedSlow(float multiplier, ElementalComponent component)
    {
        var movement = component.GetMovement();

        moveSpeedSlowMultiplier *= multiplier;
        UpdateMoveSpeed();
        if (movement != null)
        {
            movement.SetVelocityXEffect(TotalMoveSpeedMultiplier);
        }
    }

    // IceEffect에 의한 이동 속도 둔화 해제
    public void ResetMoveSpeedSlow(ElementalComponent component)
    {
        var movement = component.GetMovement();

        moveSpeedSlowMultiplier = 1f;
        UpdateMoveSpeed();
        if(movement != null)
        {
            movement.SetVelocityZeroEffect();
        }
    }
    protected void SetAdjustStatsAttackSpeed(float multiplier)
    {
        adjustStatsAttackSpeed *= multiplier;
        UpdateAttackSpeed();
    }

    protected void SetAdjustStatsMoveSpeed(float multiplier)
    {
        adjustStatsMoveSpeed *= multiplier;
        UpdateMoveSpeed();
    }

    public void ChangeAttackSpeed(float newMultiplier)
    {
        // 새로운 둔화 배율을 적용하여 누적 배율 계산
        slowEffectMultiplier *= newMultiplier;
        attackSpeed = baseAttackSpeed * slowEffectMultiplier;
        UpdateAnimatorAttackSpeed();
    }
    public void ReturnAttackSpeed()
    {
        attackSpeedModifier = 1f;
        UpdateAttackSpeed();
    }
    public void ChangeElement(Element newElement, int level = 0)
    {
        Element = newElement;
        if (elementalComponent == null)
            elementalComponent = transform.parent.GetComponentInChildren<ElementalComponent>();
        elementalComponent.ChangeElement(newElement, level);
    }

    public void UpdateElementalEffect(Element element, int level)
    {
        elementalComponent.UpdateEffectValues(element, level);
    }
    public void ResetStatsToBaseValues()
    {
        adjustStatsAttackSpeed = 1f;
        adjustStatsMoveSpeed = 1f;

        UpdateAttackSpeed();
        UpdateMoveSpeed();
    }

    protected virtual void UpdateAnimatorSpeed()
    {
        UpdateAnimatorMoveSpeed();
        UpdateAnimatorAttackSpeed();
    }
    protected virtual void UpdateAnimatorMoveSpeed()
    {
        if (animator != null)
        {
          //  Debug.Log($"UpdateAnimatorMoveSpeed : {MoveSpeed}");
            animator.SetFloat("MoveSpeed", moveSpeed);
        }
    }
    protected virtual void UpdateAnimatorAttackSpeed()
    {
        if (animator != null)
        {
      //      Debug.Log($"UpdateAnimatorAttackSpeed : {attackSpeed}");
            animator.SetFloat("AttackSpeed", attackSpeed);
        }
    }
}
