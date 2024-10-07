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

    public int Id { get => id; set => id = value; }

    public int CurHp { get => curHp; set => curHp = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public float AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float Defense { get => defense; set => defense = value; }

    protected virtual void Start()
    {
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
    }

    public void DecreaseHealth(float amount)
    {
        float damage = Mathf.Max(0, amount - Defense); // Damage 부분은 따로 계산하는 로직을 구현해서 최종 데미지를 넣을 예정.
        CurHp -= (int)damage;
        Debug.Log(gameObject.transform.root.name + " 남은 체력 : " + CurHp);
        if (CurHp <= 0)
        {
            CurHp = 0;
            OnHealthZero?.Invoke();
            Debug.Log("사망");
        }
    }

    public void IncreaseHealth(float amount)
    {
        CurHp = Mathf.Clamp(CurHp + (int)amount, 0, maxHp);
    }
}
