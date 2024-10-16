using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{ 
    None = 0,
    Fire = 1,
    Ice = 2,
    Land = 3,
    Lightning = 4
}

public enum ElementParticles
{
    None = 0,
    Fire = 1,
    Ice = 2,
    Land = 3,
    Lightning = 4,
    FireDot = 5,
    FireBoom = 6,
    IcePassive = 7,
    IceDot = 8,
    IceFreezeStartFront = 9,
    IceFreezeStartBack = 10,
    IceFreezeEndFront = 11,
    IceFreezeEndBack = 12,
}


public class ElementalManager
{
    private Dictionary<Element, Dictionary<Element, float>> damageMultiplier = new Dictionary<Element, Dictionary<Element, float>>
    {        {
            Element.None, new Dictionary<Element, float>
            {
                { Element.None, 1.0f },
                { Element.Fire, 1.0f },
                { Element.Ice, 1.0f },
                { Element.Land, 1.0f },
                { Element.Lightning, 1.0f }
            }
        },
        {
            Element.Fire, new Dictionary<Element, float>
            {
                { Element.None, 1.0f },
                { Element.Fire, 1.0f },
                { Element.Ice, 1.25f },
                { Element.Land, 0.5f },
                { Element.Lightning, 1.0f }
            }
        },
        {
            Element.Ice, new Dictionary<Element, float>
            {
                { Element.None, 1.0f },
                { Element.Fire, 0.5f },
                { Element.Ice, 1.0f },
                { Element.Land, 1.0f },
                { Element.Lightning, 1.25f }
            }
        },
        {
            Element.Land, new Dictionary<Element, float>
            {
                { Element.None, 1.0f },
                { Element.Fire, 1.25f },
                { Element.Ice, 1.0f },
                { Element.Land, 1.0f },
                { Element.Lightning, 0.5f }
            }
        },
        {
            Element.Lightning, new Dictionary<Element, float>
            {
                { Element.None, 1.0f },
                { Element.Fire, 1.0f },
                { Element.Ice, 0.5f },
                { Element.Land, 1.25f },
                { Element.Lightning, 1.0f }
            }
        }
    };

    private Dictionary<Element, IElementalEffect> elementalEffects = new Dictionary<Element, IElementalEffect>
    {
        { Element.None, new NoneEffect() },
        { Element.Fire, new FireEffect() },
        { Element.Ice, new IceEffect() },
        { Element.Land, new LandEffect() },
        { Element.Lightning, new LightningEffect() }
    };

    public float GetDamageMultiplier(Element attacker, Element defender)
    {
        return damageMultiplier[attacker][defender];
    }

    public void ApplyEffect(Element element, ElementalComponent attackerComponent, ElementalComponent defenderComponent, float attackerAttackStat)
    {
        if (elementalEffects.TryGetValue(element, out IElementalEffect effect))
        {
            effect.ApplyEffect(attackerComponent, defenderComponent, attackerAttackStat);
        }
    }
    public float CalculateDamage(Element element, float baseDamage, float attackerAttackStat)
    {
        if (elementalEffects.TryGetValue(element, out IElementalEffect effect))
        {
            return effect.CalculateDamage(baseDamage, attackerAttackStat);
        }
        return baseDamage;
    }

    public void UpdateEffectValues(Element element, int level)
    {
        if (elementalEffects.TryGetValue(element, out IElementalEffect effect))
        {
            var data = GameManager.Data.ElementalEffectDict[element.ToString().ToLower() + "Effect"];
            ElementalEffectLevelData levelData = null;

            switch (level)
            {
                case 1:
                    levelData = data.level1;
                    break;
                case 2:
                    levelData = data.level2;
                    break;
                case 3:
                    levelData = data.level3;
                    break;
            }

            if (levelData != null)
            {
                if (effect is FireEffect fireEffect)
                {
                    fireEffect.UpdateEffectValues(levelData.Fire_dotDamage, levelData.Fire_duration, levelData.Fire_maxStacks);
                }
                else if (effect is IceEffect iceEffect)
                {
                    iceEffect.UpdateEffectValues(levelData.Ice_slowEffect, levelData.Ice_duration);
                }
                else if (effect is LandEffect landEffect)
                {
                    landEffect.UpdateEffectValues(levelData.Land_decreaseAttackSpeed, levelData.Land_increaseAttackDamage);
                }
                else if (effect is LightningEffect lightningEffect)
                {
                    lightningEffect.UpdateEffectValues(levelData.Lightning_stunDuration, 0); // 두 번째 값은 필요하지 않음
                }
            }
        }
    }
    public IElementalEffect GetEffect(Element element)
    {
        elementalEffects.TryGetValue(element, out IElementalEffect effect);
        return effect;
    }
}
