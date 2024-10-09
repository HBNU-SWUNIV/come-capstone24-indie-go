using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElementalComponent : CoreComponent
{
    [SerializeField] public GameObject[] damageParticles;
    [SerializeField] public Element Element;
    private ElementalManager elementalManager;
    private IElementalEffect currentEffect;

    public Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent(ref particleManager);

    Movement movement;
    ParticleManager particleManager;

    private ICharacterStats stats;



    protected override void Awake()
    {
        base.Awake();
        elementalManager = GameManager.ElementalManager;

        stats = transform.root.GetComponentInChildren<ICharacterStats>();
        if (stats == null)
            Debug.Log("stats 빔");

    }

    private void Start()
    {
        Element = stats.Element;
        ApplyPassiveEffect();
    }

    public void ChangeElement(Element newElement, int level)
    {
        RemovePassiveEffect();
        Element = newElement;
        Debug.Log($"Element changed to {Element}");
        elementalManager.UpdateEffectValues(Element, level);
        ApplyPassiveEffect();
    }

    public float GetDamageMultiplier(Element attackerElement)
    {
        Element = stats.Element;
        return elementalManager.GetDamageMultiplier(attackerElement, Element);
    }

    public void ApplyElementalEffect(Element attackerElement, GameObject target, float attackerAttackStat, ElementalComponent attackerComponent)
    {
        var targetComponent = target.transform.parent.GetComponentInChildren<ElementalComponent>();
        if (targetComponent == null) Debug.Log("targetComponent null");
        elementalManager.ApplyEffect(attackerElement, attackerComponent, targetComponent, attackerAttackStat); // 공격자와 피격자 구분
    }

    public float CalculateDamage(Element element, float baseDamage, float attackerAttackStat)
    {
        return elementalManager.CalculateDamage(element, baseDamage, attackerAttackStat);
    }

    public void UpdateEffectValues(Element element, int level)
    {
        RemovePassiveEffect();
        elementalManager.UpdateEffectValues(element, level);
        ApplyPassiveEffect();

    }

    public Movement GetMovement() => Movement;
    public ICharacterStats GetStats() => stats;
    public ParticleManager GetParticle() => ParticleManager;
    public Coroutine StartEffectCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    public void StopEffectCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    public void DestroyObj(GameObject obj)
    {
        Destroy(obj);
    }
    private void ApplyPassiveEffect()
    {
        if (elementalManager.GetEffect(Element) is IElementalEffect effect)
        {
            effect.ApplyPassiveEffect(this);
            currentEffect = effect;
        }
        else
        {
            currentEffect = null;
        }
    }

    private void RemovePassiveEffect()
    {
        if (currentEffect != null)
        {
            currentEffect.RemovePassiveEffect(this);
            currentEffect = null;
        }
    }


   
}
