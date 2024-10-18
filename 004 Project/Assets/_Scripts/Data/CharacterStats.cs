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

    // �� �Ӽ��� ������ ������ ������
    protected int fireLevel = 1;
    protected int iceLevel = 1;
    protected int landLevel = 1;
    protected int lightLevel = 1;
    protected float baseAttackSpeed;
    protected float baseMoveSpeed;

    protected float slowEffectMultiplier = 1f; // ��ȭ ȿ���� ���� ����
    protected float attackSpeedModifier = 1f; // ���� �ӵ��� ���� �ٸ� ȿ���� ����
    protected float moveSpeedModifier = 1f;   // �̵� �ӵ��� ���� �ٸ� ȿ���� ����
    protected float attackSpeedSlowMultiplier = 1f; // IceEffect�� ���� ��ȭ ����
    protected float moveSpeedSlowMultiplier = 1f;   // IceEffect�� ���� ��ȭ ����
    protected float adjustStatsAttackSpeed = 1f;    // playerType�� ���� ���� �ӵ� ����
    protected float adjustStatsMoveSpeed = 1f;      // playerType�� ���� �̵� �ӵ� ����

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
        float damage = Mathf.Max(0, amount); // Damage �κ��� ���� ����ϴ� ������ �����ؼ� ���� �������� ���� ����.
        CurHp -= (int)damage;
        Debug.Log(gameObject.transform.root.name + " ���� ü�� : " + CurHp);
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
    // �ٸ� ȿ���� ���� ���� �ӵ� ���� (��: Land �Ӽ�)
    public void ModifyAttackSpeed(float multiplier)
    {
        attackSpeedModifier *= multiplier;
        UpdateAttackSpeed();
    }
    // IceEffect�� ���� ���� �ӵ� ��ȭ ����
    public void ApplyAttackSpeedSlow(float multiplier)
    {
        attackSpeedSlowMultiplier *= multiplier;
        UpdateAttackSpeed();

    }

    // IceEffect�� ���� ���� �ӵ� ��ȭ ����
    public void ResetAttackSpeedSlow()
    {
        attackSpeedSlowMultiplier = 1f;
        UpdateAttackSpeed();
    }
    // �ٸ� ȿ���� ���� �̵� �ӵ� ����
    public void ModifyMoveSpeed(float multiplier)
    {
        moveSpeedModifier *= multiplier;
        UpdateMoveSpeed();
    }

    // IceEffect�� ���� �̵� �ӵ� ��ȭ ����
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

    // IceEffect�� ���� �̵� �ӵ� ��ȭ ����
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
        // ���ο� ��ȭ ������ �����Ͽ� ���� ���� ���
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
