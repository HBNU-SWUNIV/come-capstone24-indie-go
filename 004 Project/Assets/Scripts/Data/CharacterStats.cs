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
    protected int lightningLevel = 1;

    public Element Element { get => element; set => element = value; }
    private ElementalComponent elementalComponent;

    private float damage;
    private float aspeed;
    private float mspeed;
    private float land_aspeed;
    protected virtual void Start()
    {
        Element = Element.None;
        elementalComponent = transform.parent.GetComponentInChildren<ElementalComponent>();
        animator = transform.root.GetComponent<Animator>();

        //SetStat();
    }

    protected abstract void SetStat();

    protected void SetStatsData(T stats)
    {
        curHp = stats.curHp;
        maxHp = stats.maxHp;
        attackDamage = stats.attackDamage;
        attackSpeed = stats.attackSpeed;
        defense = stats.defense;
        moveSpeed = stats.moveSpeed;

        damage = attackDamage;
        Debug.Log("moveSpeed : " + moveSpeed);
        mspeed = moveSpeed;
        aspeed = attackSpeed;
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
            Debug.Log("사망");
            return false;
        }
        return true;
    }

    public void IncreaseHealth(float amount)
    {
        if(!IsHpMax(amount))
            CurHp = Mathf.Clamp(CurHp + (int)amount, 0, maxHp);
    }
    public bool IsHpMax(float amount)
    {
        if (CurHp + (int)amount > maxHp)
            return true;
        return false;
    }
    public void ChangeDamage(float currentDamage)
    {
        attackDamage *= (1 + currentDamage);
    }
    public void ReturnDamage()
    {
        attackDamage = damage;
    }
    public void ChangeAttackSpeed(float currentSpeed)
    {
        attackSpeed = currentSpeed;
        UpdateAnimatorAttackSpeed();
    }
    public void ChangeLandAttackSpeed(float currentSpeed)
    {
        land_aspeed = currentSpeed;
        Debug.Log(land_aspeed);

        attackSpeed = currentSpeed;
        UpdateAnimatorAttackSpeed();
    }
    public void ReturnLandAttackSpeed()
    {
        attackSpeed = land_aspeed;
        Debug.Log("aspeed" + aspeed);

        UpdateAnimatorAttackSpeed();
    }
    public void ReturnAttackSpeed()
    {
        attackSpeed = aspeed;
        Debug.Log("aspeed" + aspeed);

        UpdateAnimatorAttackSpeed();
    }
    public void ChangeMoveSpeed(float currentSpeed)
    {
        
        moveSpeed = currentSpeed;
        UpdateAnimatorMoveSpeed();
    }
    public void ReturnMoveSpeed()
    {
        moveSpeed = mspeed;
        Debug.Log("mspeed" + mspeed);
        UpdateAnimatorMoveSpeed();
    }
    public void ChangeElement(Element newElement, int level)
    {
        Element = newElement;
        elementalComponent.ChangeElement(newElement, level);
    }

    public void UpdateElementalEffect(Element element, int level)
    {
        elementalComponent.UpdateEffectValues(element, level);
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
            animator.SetFloat("MoveSpeed", moveSpeed);
        }
    }
    protected virtual void UpdateAnimatorAttackSpeed()
    {
        if (animator != null)
        {
            animator.SetFloat("AttackSpeed", attackSpeed);
        }
    }
}
