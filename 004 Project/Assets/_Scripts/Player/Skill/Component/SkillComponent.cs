using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillComponent : MonoBehaviour
{
    protected Skill skill;
    protected Core Core => skill.Core;
    protected SkillAnimEventHandler eventHandler => skill.EventHandler;

    protected bool isSkillActive;

    protected PlayerStats playerStats;
    public virtual void Init()
    {

    }

    protected virtual void Awake()
    {
        skill = GetComponent<Skill>();
        playerStats = transform.root.GetComponentInChildren<PlayerStats>();
    }

    protected virtual void OnEnable()
    {
        skill.OnEnter += HandleEnter;
        skill.OnExit += HandleExit;
    }

    protected virtual void HandleEnter()
    {
        isSkillActive = true;
    }
    protected virtual void HandleExit()
    {
        isSkillActive = false;
    }

    protected virtual void OnDisable()
    {
        skill.OnEnter += HandleEnter;
        skill.OnExit += HandleExit;
    }
}

public class SkillComponent<T> : SkillComponent where T : SkillData
{
    protected T currentSkillData;

    protected override void HandleEnter()
    {
        base.HandleEnter();
        currentSkillData = skill.Data.GetData<T>();
    }
    public override void Init()
    {
        base.Init();
        currentSkillData = skill.Data.GetData<T>();
    }
}


/*
 
public class SkillComponent<T> : SkillComponent where T : SkillData
{
    protected T currentSkillData;

    protected override void HandleEnter()
    {
        base.HandleEnter();
        currentSkillData = skill.Data.GetData<T>();
    }
    public override void Init()
    {
        base.Init();
        currentSkillData = skill.Data.GetData<T>();
    }
}*/