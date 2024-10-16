using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skill : MonoBehaviour
{
    public Core Core { get; private set; }
    public SkillStateMachine StateMachine { get; private set; }


    public event Action OnEnter;
    public event Action OnExit;
    public event Action OnLogicUpdate;
    public event Action OnHold;
    public bool hold;
    public SkillDataEx Data { get; private set; }
    private Animator baseAnim;
    private Animator weaponAnim;
    private bool isSkillActive;
    public SkillAnimEventHandler EventHandler { get; private set; }
    public GameObject BaseGameObject { get; private set; }
    public GameObject WeaponGameObject { get; private set; }

    private List<MonoBehaviour> initializedComponents = new List<MonoBehaviour>();


    protected virtual void Awake()
    {
        BaseGameObject = transform.Find("Base").gameObject;
        WeaponGameObject = transform.Find("Weapon").gameObject;

        baseAnim = BaseGameObject.GetComponent<Animator>();
        weaponAnim = WeaponGameObject.GetComponent<Animator>();

        EventHandler = BaseGameObject.GetComponent<SkillAnimEventHandler>();
        StateMachine = new SkillStateMachine();
    }
    private void Start()
    {
        gameObject.SetActive(false);

    }

    protected virtual void Update()
    {
        if(isSkillActive)
            OnLogicUpdate?.Invoke();
    }
    public void SetCore(Core core)
    {
        this.Core = core;
    }
    public void SetData(SkillDataEx data)
    {
        Data = data;
    }
    public void EnterSkill()
    {
        gameObject.SetActive(true);

        isSkillActive = true;
        baseAnim.SetBool("Active", true);
        weaponAnim.SetBool("Active", true);

        OnEnter.Invoke();
    }
    public void ExitSkill()
    {
        gameObject.SetActive(false);

        isSkillActive = false;
        baseAnim.SetBool("Active", false);
        weaponAnim.SetBool("Active", false);

        OnExit.Invoke();
    }

    private void OnEnable()
    {
        EventHandler.OnFinish += ExitSkill;
    }
    private void OnDisable()
    {
        EventHandler.OnFinish -= ExitSkill;
    }

    public void RegisterComponent(MonoBehaviour component)
    {
        initializedComponents.Add(component);
    }

    public void ClearComponents()
    {
        foreach (var component in initializedComponents)
        {
            Destroy(component);
        }
        initializedComponents.Clear();
    }
    public void HoldSkill()
    {
        OnHold?.Invoke();
    }
}
