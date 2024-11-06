using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElementalComponent : CoreComponent
{
    [SerializeField] public GameObject[] damageParticles;
    [SerializeField] public Element Element;
    private ElementalManager elementalManager;
    private IElementalEffect currentEffect;

    private Dictionary<IEnumerator, Coroutine> activeCoroutines = new Dictionary<IEnumerator, Coroutine>();

    public Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent(ref particleManager);

    Movement movement;
    ParticleManager particleManager;
    public bool setPassive;
    private ICharacterStats stats;



    protected override void Awake()
    {
        base.Awake();
        elementalManager = GameManager.ElementalManager;

        stats = transform.root.GetComponentInChildren<ICharacterStats>();
        if (stats == null)
            Debug.Log("stats ��");

    }
    private void OnEnable()
    {
        if(setPassive)
        {
            RemovePassiveEffect();
            ApplyPassiveEffect();
            setPassive = false;
        }
    }

    private void Start()
    {
  //      RemovePassiveEffect();
        Element = stats.Element;
   //     ApplyPassiveEffect();
    }

    public void ChangeElement(Element newElement, int level = 0)
    {
        RemovePassiveEffect();
        Element = newElement;
    //    Debug.Log($"Element changed to {Element}");
        if (gameObject.CompareTag("Player"))
        {
            // Debug.Log($"{newElement}�� �÷��̾� ��ų ����");
            GameManager.PlayerManager.ChangeSkill(Element);
        }
        if (elementalManager == null)
            elementalManager = GameManager.ElementalManager;
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
        elementalManager.ApplyEffect(attackerElement, attackerComponent, targetComponent, attackerAttackStat); // �����ڿ� �ǰ��� ����
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
        if (activeCoroutines.ContainsKey(coroutine))
        {
            StopCoroutine(activeCoroutines[coroutine]);
            activeCoroutines.Remove(coroutine);
        }

        Coroutine newCoroutine = StartCoroutine(coroutine);
        activeCoroutines[coroutine] = newCoroutine;
        return newCoroutine;
    }

    public void StopEffectCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    public void StopAllEffectCoroutines()
    {
        foreach (var entry in activeCoroutines)
        {
            StopCoroutine(entry.Value);
        }
        activeCoroutines.Clear();
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
