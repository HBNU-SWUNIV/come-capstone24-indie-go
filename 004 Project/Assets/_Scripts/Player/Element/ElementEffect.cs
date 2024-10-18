using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoneEffect : IElementalEffect
{
    public void ApplyEffect(ElementalComponent targetComponent, ElementalComponent selfComponent, float attackerAttackStat)
    {
    }



    public float CalculateDamage(float baseDamage, float attackerAttackStat)
    {
        return baseDamage;
    }

    public void ApplyPassiveEffect(ElementalComponent component)
    {
    }
    public void RemovePassiveEffect(ElementalComponent component)
    {
    }

    public void UpdateEffectValues(float value1, float value2)
    {

    }
}
public class FireEffect : IElementalEffect
{
    private float dotDamage;
    private float duration;
    private int maxStacks;
    private float cooldown = 5.0f;
    // 타겟별 데이터를 저장하기 위한 딕셔너리
    private Dictionary<ElementalComponent, int> currentStacks = new Dictionary<ElementalComponent, int>();
    private Dictionary<ElementalComponent, Coroutine> dotCoroutines = new Dictionary<ElementalComponent, Coroutine>();


    public void ApplyEffect(ElementalComponent attackerComponent, ElementalComponent defenderComponent, float attackerAttackStat)
    {
      //  Debug.Log($"attackerAttackStat {attackerAttackStat} , dotDamage  {dotDamage}");
        int dotDamageValue = Mathf.RoundToInt(attackerAttackStat * dotDamage);

        // currentStacks에 수비자가 없으면 초기화
        if (!currentStacks.ContainsKey(defenderComponent))
        {
            currentStacks[defenderComponent] = 0;
        }
        // 기존 코루틴이 있으면 중지
        if (dotCoroutines.ContainsKey(defenderComponent))
        {
            defenderComponent.StopEffectCoroutine(dotCoroutines[defenderComponent]);
            dotCoroutines.Remove(defenderComponent);
        }

        currentStacks[defenderComponent] = Mathf.Min(currentStacks[defenderComponent] + 1, maxStacks);
        Debug.Log($"currentFireStacks for {defenderComponent.name}: {currentStacks[defenderComponent]}");


        if (currentStacks[defenderComponent] >= maxStacks)
        {
            // 최대 스택에 도달하면 폭발 피해 적용
            ApplyExplosionDamage(defenderComponent, attackerAttackStat);
            currentStacks[defenderComponent] = 0;
        }
        else
        {
            // DOT 데미지 코루틴 시작
            Coroutine newCoroutine = defenderComponent.StartEffectCoroutine(DotDamageOverTime(defenderComponent, dotDamageValue));
            dotCoroutines[defenderComponent] = newCoroutine;
        }
    }

    public void UpdateEffectValues(float newDotDamage, float newDuration, int newMaxStacks)
    {
  //      Debug.Log($"업데이트 전 dotDamage : {dotDamage}");
  //      Debug.Log($"업데이트 전 duration : {duration}");
  //      Debug.Log($"업데이트 전 maxStacks : {maxStacks}");

        dotDamage = newDotDamage;
        duration = newDuration;
        maxStacks = newMaxStacks;
   //     Debug.Log($"업데이트 후 dotDamage : {dotDamage}");
    //    Debug.Log($"업데이트 후 duration : {duration}");
   //     Debug.Log($"업데이트 후 maxStacks : {maxStacks}");
    }

    public float CalculateDamage(float baseDamage, float attackerAttackStat)
    {
        return baseDamage + attackerAttackStat * 0.2f;
    }

    private IEnumerator DotDamageOverTime(ElementalComponent target, int dotDamageValue)
    {
        float elapsedTime = 0f;
        float damageMultiplier = 1f + (currentStacks[target] - 1) * 0.1f;
        Debug.Log($" {duration} 초간 {dotDamageValue} 도트 데미지");

        while (elapsedTime < duration)
        {
            target.GetStats().DecreaseHealth(dotDamageValue * damageMultiplier);
            target.GetParticle().StartParticlesWithDesignatedRotation(target.damageParticles[(int) ElementParticles.FireDot], new Vector3(0,-0.5f,0), target.transform);
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }
        currentStacks[target] = 0; // DOT 종료 후 스택 초기화
        dotCoroutines.Remove(target); // 코루틴 참조 제거
    }
    private void ApplyExplosionDamage(ElementalComponent component, float attackerAttackStat)
    {
        float explosionDamage = attackerAttackStat * (maxStacks * 0.5f); 
        Debug.Log($"Explosion damage: {explosionDamage}");
        component.GetStats().DecreaseHealth(explosionDamage);
        // 폭발 이펙트 추가
        component.GetParticle().StartParticles(component.damageParticles[(int)ElementParticles.FireBoom], component.transform);
    }

    public void ApplyPassiveEffect(ElementalComponent component)
    {
    }
    public void RemovePassiveEffect(ElementalComponent component)
    {
    }
}


public class IceEffect : IElementalEffect
{
    private float slowEffect;
    private float duration;
    private int maxSlowStacks;
    private float healthRegenRate;
    private float stunTime = 3.0f;
    private float cooldown = 5.0f;

    // 타겟별 데이터를 저장하기 위한 딕셔너리
    private Dictionary<ElementalComponent, int> currentSlowStacks = new Dictionary<ElementalComponent, int>();
    private Dictionary<ElementalComponent, Coroutine> slowCoroutines = new Dictionary<ElementalComponent, Coroutine>();
    private Dictionary<ElementalComponent, Coroutine> cooldowns = new Dictionary<ElementalComponent, Coroutine>();
    private Dictionary<ElementalComponent, GameObject> debuffEffects = new Dictionary<ElementalComponent, GameObject>();
    private Dictionary<ElementalComponent, Coroutine> regenCoroutines = new Dictionary<ElementalComponent, Coroutine>();


    //    private Dictionary<ElementalComponent, Coroutine> cooldowns = new Dictionary<ElementalComponent, Coroutine>();
    private Dictionary<ElementalComponent, GameObject> freezeEffectFronts = new Dictionary<ElementalComponent, GameObject>();
    private Dictionary<ElementalComponent, GameObject> freezeEffectBacks = new Dictionary<ElementalComponent, GameObject>();


    public void ApplyEffect(ElementalComponent attackerComponent, ElementalComponent defenderComponent, float attackerAttackStat)
    {
        if (cooldowns.ContainsKey(defenderComponent))
        {
            Debug.Log("Ice effect 쿨다운");
            return;
        }

        // currentSlowStacks에 수비자가 없으면 초기화
        if (!currentSlowStacks.ContainsKey(defenderComponent))
        {
            currentSlowStacks[defenderComponent] = 0;
        }

        // 기존 슬로우 코루틴이 있으면 중지
        if (slowCoroutines.ContainsKey(defenderComponent))
        {
            defenderComponent.StopEffectCoroutine(slowCoroutines[defenderComponent]);
            slowCoroutines.Remove(defenderComponent);
        }


        if (currentSlowStacks[defenderComponent] == 0)
        {
            GameObject debuffEffect = defenderComponent.GetParticle().StartParticlesWithDesignatedRotation(defenderComponent.damageParticles[(int)ElementParticles.IceDot], new Vector3(0, -0.5f, 0), defenderComponent.transform);
            debuffEffects[defenderComponent] = debuffEffect;
        }

        currentSlowStacks[defenderComponent] = Mathf.Min(currentSlowStacks[defenderComponent] + 1, maxSlowStacks);
        Debug.Log($"currentSlowStacks for {defenderComponent.name}: {currentSlowStacks[defenderComponent]}");
        
        var defenderStats = defenderComponent.GetStats();
        var defenderMovement = defenderComponent.GetMovement();

        if (currentSlowStacks[defenderComponent] >= maxSlowStacks)
        {
            // 프리즈 효과 적용
            Debug.Log($"Ice effect: Freezing target for {stunTime} second");
            var enemy = defenderComponent.transform.root.GetComponent<Entity>();
            enemy.stunState.SetFreezeStunTime(stunTime);
            enemy.stunState.stun = true;
            freezeEffectFronts[defenderComponent] = defenderComponent.GetParticle().StartParticlesWithDesignatedRotationAndDestroy(defenderComponent.damageParticles[(int)ElementParticles.IceFreezeStartFront], new Vector3(0, -0.5f, 0), defenderComponent.transform, stunTime);
            freezeEffectBacks[defenderComponent] = defenderComponent.GetParticle().StartParticlesWithDesignatedRotationAndDestroy(defenderComponent.damageParticles[(int)ElementParticles.IceFreezeStartBack], new Vector3(0, -0.5f, 0), defenderComponent.transform, stunTime);

            defenderComponent.StartEffectCoroutine(StartIceFreezeEnd(defenderComponent, stunTime));
          //  enemy.stateMachine.ChangeState(enemy.stunState);

            // 스택 초기화 및 슬로우 효과 제거
            currentSlowStacks[defenderComponent] = 0;

            if (defenderStats != null)
            {
                defenderStats.ResetAttackSpeedSlow();
                defenderStats.ResetMoveSpeedSlow(defenderComponent);
            }

            if (slowCoroutines.ContainsKey(defenderComponent))
            {
                defenderComponent.StopEffectCoroutine(slowCoroutines[defenderComponent]);
                slowCoroutines.Remove(defenderComponent);
            }

            if (debuffEffects.ContainsKey(defenderComponent))
            {
                defenderComponent.DestroyObj(debuffEffects[defenderComponent]);
                debuffEffects.Remove(defenderComponent);
            }
            cooldowns[defenderComponent] = defenderComponent.StartEffectCoroutine(CooldownCoroutine(defenderComponent));

        }
        else
        {

            // 둔화 효과 적용
            float slowMultiplier;
            if (currentSlowStacks[defenderComponent] == 1)
            {
                slowMultiplier = 1f - slowEffect; // 첫 번째 스택은 slowEffect만큼 감소
            }
            else
            {
                slowMultiplier = 0.9f; // 이후 스택은 10%씩 추가 감소
            }

            if (defenderStats != null)
            {
                defenderStats.ApplyAttackSpeedSlow(slowMultiplier);
                defenderStats.ApplyMoveSpeedSlow(slowMultiplier, defenderComponent);
            }

            Coroutine slowCoroutine = defenderComponent.StartEffectCoroutine(SlowEffectCoroutine(defenderComponent, defenderMovement, defenderStats));
            slowCoroutines[defenderComponent] = slowCoroutine;
        }
    }

    public void UpdateEffectValues(float newSlowEffect, float newDuration, float newHealthRegenRate, int newMaxSlowStacks)
    {
        Debug.Log($"업데이트 전 slowEffect : {slowEffect}");
        slowEffect = newSlowEffect;
        duration = newDuration;
        healthRegenRate = newHealthRegenRate;
        maxSlowStacks = newMaxSlowStacks;
        Debug.Log($"업데이트 후 slowEffect : {slowEffect}");

    }

    public float CalculateDamage(float baseDamage, float attackerAttackStat)
    {
        return baseDamage;
    }

    private IEnumerator StartIceFreezeEnd(ElementalComponent component ,float time)
    {
        yield return new WaitForSeconds(time);

        if (freezeEffectFronts.ContainsKey(component))
            component.DestroyObj(freezeEffectFronts[component]);
        if (freezeEffectBacks.ContainsKey(component))
            component.DestroyObj(freezeEffectBacks[component]);
        component.GetParticle().StartParticlesWithDesignatedRotation(component.damageParticles[(int)ElementParticles.IceFreezeEndFront], new Vector3(0, -0.5f, 0), component.transform);
        component.GetParticle().StartParticlesWithDesignatedRotation(component.damageParticles[(int)ElementParticles.IceFreezeEndBack], new Vector3(0, -0.5f, 0), component.transform);

    }
    private IEnumerator SlowEffectCoroutine(ElementalComponent defenderComponent, Movement defenderMovement, ICharacterStats defenderStats)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;

        }

        if (defenderStats != null)
        {
            defenderStats.ResetAttackSpeedSlow();
            defenderStats.ResetMoveSpeedSlow(defenderComponent);
        }

        if (debuffEffects.ContainsKey(defenderComponent))
        {
            defenderComponent.DestroyObj(debuffEffects[defenderComponent]);
            debuffEffects.Remove(defenderComponent);
        }
        currentSlowStacks[defenderComponent] = 0; // 해당 수비자의 슬로우 스택 초기화
        slowCoroutines.Remove(defenderComponent); // 코루틴 참조 제거

        Debug.Log("Ice effect removed: slow effect ended for " + defenderComponent.name);
    }

    private IEnumerator CooldownCoroutine(ElementalComponent defenderComponent)
    {
        Debug.Log("Ice effect cooldown started for " + defenderComponent.name);
        yield return new WaitForSeconds(cooldown);
        cooldowns.Remove(defenderComponent);
        Debug.Log("Ice effect cooldown ended for " + defenderComponent.name);
    }

    public void ApplyPassiveEffect(ElementalComponent component)
    {
        if(!regenCoroutines.ContainsKey(component))
        {
            regenCoroutines[component] = component.StartEffectCoroutine(HealthRegenCoroutine(component));
        }
    }

    public void RemovePassiveEffect(ElementalComponent component)
    {
        if (regenCoroutines.ContainsKey(component))
        {
            component.StopEffectCoroutine(regenCoroutines[component]);
            regenCoroutines.Remove(component);
        }
    }

    private IEnumerator HealthRegenCoroutine(ElementalComponent component)
    {
        var stats = component.GetStats();
        while (true)
        {
            if (!stats.IsHpMax(healthRegenRate))
                component.GetParticle().StartParticlesWithDesignatedRotation(component.damageParticles[(int)ElementParticles.IcePassive], new Vector3(0, -0.85f, 0), component.transform);
            stats.IncreaseHealth(healthRegenRate);
            yield return new WaitForSeconds(1f);
        }
    }
}


public class LandEffect : IElementalEffect
{
    private float decreaseAttackSpeed;
    private float increaseAttackDamage;
    private float cooldown = 5.0f;

    public void ApplyEffect(ElementalComponent attackerComponent, ElementalComponent defenderComponent, float attackerAttackStat)
    {
        // LandEffect는 직접적인 ApplyEffect를 가지지 않음
    }

    public void UpdateEffectValues(float newDecreaseAttackSpeed, float newIncreaseAttackDamage)
    {
  //      Debug.Log($"업데이트 전 decreaseAttackSpeed : {decreaseAttackSpeed}");
 //       Debug.Log($"업데이트 전 increaseAttackDamage : {increaseAttackDamage}");

        decreaseAttackSpeed = newDecreaseAttackSpeed;
        increaseAttackDamage = newIncreaseAttackDamage;
   //     Debug.Log($"업데이트 후 decreaseAttackSpeed : {decreaseAttackSpeed}");
   //     Debug.Log($"업데이트 후 increaseAttackDamage : {increaseAttackDamage}");
    }

    public float CalculateDamage(float baseDamage, float attackerAttackStat)
    {
        return baseDamage;
    }

    public void ApplyPassiveEffect(ElementalComponent component)
    {
        var stats = component.GetStats();
        if (stats != null)
        {
            stats.ModifyAttackSpeed(1 - decreaseAttackSpeed);
            stats.ChangeDamage(increaseAttackDamage);
        }
    }

    public void RemovePassiveEffect(ElementalComponent component)
    {
  //      Debug.Log($"Land effect removed: restoring original attack speed and damage");
        var stats = component.GetStats();
        if (stats != null)
        {
            stats.ReturnAttackSpeed();
            stats.ReturnDamage();
        }
    }


}


public class LightEffect : IElementalEffect
{
    private float increaseAttackSpeed;
    private float decreaseAttackDamage;

    private float cooldown = 5.0f;

    public void ApplyEffect(ElementalComponent targetComponent, ElementalComponent selfComponent, float attackerAttackStat)
    {
        // LightEffect 직접적인 ApplyEffect를 가지지 않음

    }

    public void UpdateEffectValues(float newIncreaseAttackSpeed, float newDecreaseAttackDamage)
    {
        increaseAttackSpeed = newIncreaseAttackSpeed;
        decreaseAttackDamage = newDecreaseAttackDamage;
    }

    public float CalculateDamage(float baseDamage, float attackerAttackStat)
    {
        return baseDamage;
    }

    public void ApplyPassiveEffect(ElementalComponent component)
    {
        var stats = component.GetStats();
        if (stats != null)
        {
            stats.ModifyAttackSpeed(1 + increaseAttackSpeed);
            stats.ChangeDamage(-decreaseAttackDamage);
        }
    }

    public void RemovePassiveEffect(ElementalComponent component)
    {
        var stats = component.GetStats();
        if (stats != null)
        {
            stats.ReturnAttackSpeed();
            stats.ReturnDamage();
        }
    }
}
