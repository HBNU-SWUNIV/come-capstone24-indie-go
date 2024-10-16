using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    public event Action OnHealthZero;

    [SerializeField] protected int id;
    [SerializeField] protected int curHp;
    [SerializeField] protected int maxHp;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float defense;
    //    public int gold;
    //    public int item;
    //    public int AttackRange;
    //    public int SearchRange;
    public int Id { get => id; set => id = value; }
    public int CurHp { get => curHp; set => curHp = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public float AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float Defense { get => defense; set => defense = value; }

    protected override void Awake()
    {
        base.Awake();

        SetStat();

    }
    protected virtual void SetStat()
    {
        CurHp = maxHp;
    }

    public void DecreaseHealth(GameObject gameObject, float amount)
    {
        float damage = Mathf.Max(0, amount - Defense);      //Damage부분은 따로 계산하는 로직을 구현해서 최종 데미지를 넣을 예정.
        CurHp -= (int)damage;
        Debug.Log(gameObject.transform.root.name + "남은 체력 : " + CurHp);
        if(CurHp <= 0)
        {
            CurHp = 0;

            OnHealthZero?.Invoke();

            Debug.Log("사망");
        }
    }

    public void IncreaseHealth(float amount)
    {
        CurHp += Mathf.Clamp(CurHp + (int)amount, 0, maxHp);
    }
}


/*
 * public class PlayerStat : Stats
{
      [SerializeField] protected int level;
  //     [SerializeField] protected int maxExp;
      [SerializeField] protected int exp;
    [SerializeField] protected int gold;



    public int Level { get => level; set => level = value; }
   //   public int MaxExp { get => maxExp; set => maxExp = value; }
    public int Exp { get => exp; set => exp = value; }
    public int Gold { get => gold; set => gold = value; }

    protected override void Awake()
    {
        base.Awake();
    
    }

    protected override void SetStat()
    {
        //초기화
        id = 0;
        level = 1;
        maxHp = 1000;
        curHp = maxHp;
        attackDamage = 10;
        AttackSpeed = 1;
        Defense = 0;
        //maxExp = 20;
        exp = 0;
        gold = 0;
    }

}
 */
