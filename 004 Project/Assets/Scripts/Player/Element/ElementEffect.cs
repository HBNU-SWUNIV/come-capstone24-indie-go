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
    private int maxStacks;// = 3;
    private int currentStacks = 0;
    private Coroutine dotCoroutine;

    public void ApplyEffect(ElementalComponent attackerComponent, ElementalComponent defenderComponent, float attackerAttackStat)
    {
        Debug.Log($"attackerAttackStat {attackerAttackStat} , dotDamage  {dotDamage}");
        int dotDamageValue = Mathf.RoundToInt(attackerAttackStat * dotDamage);
        if (dotCoroutine != null)
        {
            defenderComponent.StopEffectCoroutine(dotCoroutine);
        }

        currentStacks = Mathf.Min(currentStacks + 1, maxStacks);
        Debug.Log($"currentFireStacks : {currentStacks}");

        if (currentStacks >= maxStacks)
        {
            // 스택이 최대치에 도달했을 때 폭발 효과 적용
            ApplyExplosionDamage(defenderComponent, attackerAttackStat);
            currentStacks = 0;
        }
        else
        {
            dotCoroutine = defenderComponent.StartEffectCoroutine(DotDamageOverTime(defenderComponent, dotDamageValue));
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
        float damageMultiplier = 1f + (currentStacks - 1) * 0.1f;
        Debug.Log($" {duration} 초간 {dotDamageValue} 도트 데미지");

        while (elapsedTime < duration)
        {
            target.GetStats().DecreaseHealth(dotDamageValue * damageMultiplier);
            target.GetParticle().StartParticlesWithDesignatedRotation(target.damageParticles[(int) ElementParticles.FireDot], new Vector3(0,-0.5f,0), target.transform);
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }
        currentStacks = 0; // 도트 데미지 종료 후 스택 초기화
        dotCoroutine = null; // 코루틴 참조 초기화
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
    private float slowEffect = 0.5f;
    private float duration = 4.0f;
    private int maxSlowStacks = 5;
    private int currentSlowStacks = 0;
    private float stunTime = 3.0f;
    private float cooldown = 5.0f;
    private float healthRegenRate = 5.0f;
    private Coroutine regenCoroutine; // 단일 Coroutine 변수

    private Dictionary<ElementalComponent, Coroutine> cooldowns = new Dictionary<ElementalComponent, Coroutine>();
    private GameObject debuffEffect;
    private GameObject freezeEffectFront;
    private GameObject freezeEffectBack;
    private Coroutine slowCoroutine;


    public void ApplyEffect(ElementalComponent attackerComponent, ElementalComponent defenderComponent, float attackerAttackStat)
    {
        if (cooldowns.ContainsKey(defenderComponent))
        {
            Debug.Log("Ice effect 쿨다운");
            return;
        }
        if (slowCoroutine != null)
        {
            defenderComponent.StopEffectCoroutine(slowCoroutine);
        }

        Debug.Log("currentSlowStacks : " + currentSlowStacks);
        if (currentSlowStacks == 0)
        {
            debuffEffect = (defenderComponent.GetParticle().StartParticlesWithDesignatedRotation(defenderComponent.damageParticles[(int)ElementParticles.IceDot], new Vector3(0, -0.5f, 0), defenderComponent.transform));
        }
        currentSlowStacks = Mathf.Min(currentSlowStacks + 1, maxSlowStacks);
        Debug.Log($"currentSlowStacks : {currentSlowStacks}");
        var defenderStats = defenderComponent.GetStats();
        var defenderMovement = defenderComponent.GetMovement();

        if (currentSlowStacks >= maxSlowStacks)
        {
            // Freeze the target for 1 second immediately
            Debug.Log("Ice effect: Freezing target for 3 second");
            var enemy = defenderComponent.transform.root.GetComponent<Enemy1>();
            enemy.stunState.SetStunTime(stunTime);
            freezeEffectFront = defenderComponent.GetParticle().StartParticlesWithDesignatedRotationAndDestroy(defenderComponent.damageParticles[(int)ElementParticles.IceFreezeStartFront], new Vector3(0, -0.5f, 0), defenderComponent.transform, stunTime);
            freezeEffectBack = defenderComponent.GetParticle().StartParticlesWithDesignatedRotationAndDestroy(defenderComponent.damageParticles[(int)ElementParticles.IceFreezeStartBack], new Vector3(0, -0.5f, 0), defenderComponent.transform, stunTime);
            defenderComponent.StartEffectCoroutine(StartIceFreezeEnd(defenderComponent, stunTime));
            enemy.stateMachine.ChangeState(enemy.stunState);

            // Reset the slow stacks and stop any ongoing slow effect
            currentSlowStacks = 0;
            if (defenderStats != null)
            {
                if (defenderStats.Element == Element.Land)
                {
                    defenderStats.ReturnMoveSpeed();
                    defenderStats.ReturnLandAttackSpeed();
                }
                else
                {
                    defenderStats.ReturnMoveSpeed();
                    defenderStats.ReturnAttackSpeed();
                }
            }
            if( defenderMovement != null)
                defenderMovement.SetVelocityZeroEffect();

            if (slowCoroutine != null)
            {
                defenderComponent.StopEffectCoroutine(slowCoroutine);
                slowCoroutine = null;
            }

            if (debuffEffect != null)
            {
                defenderComponent.DestroyObj(debuffEffect);
            }
            cooldowns[defenderComponent] = defenderComponent.StartEffectCoroutine(CooldownCoroutine(defenderComponent));

        }
        else
        {

            float slowMultiplier = 1f + (currentSlowStacks - 1) * 0.1f;
            if (defenderMovement != null)
            {
                
                defenderMovement.SetVelocityXEffect(1 - ((slowMultiplier) * slowEffect));
                
            }
            if (defenderStats != null)
            {
                defenderStats.ChangeMoveSpeed(1 - slowMultiplier * slowEffect);
                defenderStats.ChangeAttackSpeed(1 - slowMultiplier * slowEffect);
            }
            Debug.Log($"{duration}초간 {((slowMultiplier) * slowEffect) * 100}% 둔화 ");

            slowCoroutine = defenderComponent.StartEffectCoroutine(SlowEffectCoroutine(defenderComponent, defenderMovement, defenderStats));
        }
    }

    public void UpdateEffectValues(float newSlowEffect, float newDuration)
    {
        Debug.Log($"업데이트 전 slowEffect : {slowEffect}");
        slowEffect = newSlowEffect;
        duration = newDuration;
        Debug.Log($"업데이트 후 slowEffect : {slowEffect}");

    }

    public float CalculateDamage(float baseDamage, float attackerAttackStat)
    {
        return baseDamage;
    }

    private IEnumerator StartIceFreezeEnd(ElementalComponent component ,float time)
    {
        yield return new WaitForSeconds(time);

        if (freezeEffectFront != null)
            component.DestroyObj(freezeEffectFront);
        if (freezeEffectBack != null)
            component.DestroyObj(freezeEffectBack);
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

        // Restore original speed and attack speed
        if (defenderMovement != null)
        {
            defenderMovement.SetVelocityZeroEffect();
        }
        
        if (defenderStats != null)
        {
            if (defenderStats.Element == Element.Land)
            {
                defenderStats.ReturnMoveSpeed();
                defenderStats.ReturnLandAttackSpeed();
            }
            else
            {
                defenderStats.ReturnMoveSpeed();
                defenderStats.ReturnAttackSpeed();
            }
        }
        if (debuffEffect != null)
        {
            defenderComponent.DestroyObj(debuffEffect);
        }
        currentSlowStacks = 0; // Reset slow stacks
        slowCoroutine = null; // Clear the coroutine reference

        Debug.Log("Ice effect removed: slow effect ended");
    }

    private IEnumerator CooldownCoroutine(ElementalComponent defenderComponent)
    {
        Debug.Log("Ice effect cooldown started");
        yield return new WaitForSeconds(cooldown);
        cooldowns.Remove(defenderComponent);
        Debug.Log("Ice effect cooldown ended");
    }

    public void ApplyPassiveEffect(ElementalComponent component)
    {
        if (regenCoroutine == null)
        {
            regenCoroutine = component.StartEffectCoroutine(HealthRegenCoroutine(component));
        }
    }

    public void RemovePassiveEffect(ElementalComponent component)
    {
        if (regenCoroutine != null)
        {
            component.StopEffectCoroutine(regenCoroutine);
            regenCoroutine = null;
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
    //    Debug.Log($"Land effect applied: decreasing attack speed by {decreaseAttackSpeed} and increasing attack damage by {increaseAttackDamage}");
        var stats = component.GetStats();
        if (stats != null)
        {
            stats.ChangeLandAttackSpeed(1 - decreaseAttackSpeed);
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


public class LightningEffect : IElementalEffect
{
    private float stunDuration = 2.0f;
    private float increaseAttackSpeed = 0.3f;

    public void ApplyEffect(ElementalComponent targetComponent, ElementalComponent selfComponent, float attackerAttackStat)
    {
     /*   Debug.Log($"Lightning effect applied: stunning target for {stunDuration} seconds");
        var targetMovement = targetComponent.GetMovement();
        if (targetMovement != null)
        {
            targetMovement.SetVelocityZero();
        }
        targetComponent.StartEffectCoroutine(RemoveStunEffect(targetMovement));*/
    }

    public void UpdateEffectValues(float newStunDuration, float unused)
    {

        stunDuration = newStunDuration;
    }

    public float CalculateDamage(float baseDamage, float attackerAttackStat)
    {
        return baseDamage;
    }

    private IEnumerator RemoveStunEffect(Movement targetMovement)
    {
        yield return new WaitForSeconds(stunDuration);
        Debug.Log("Lightning effect removed: stun effect ended");
    }

    public void ApplyPassiveEffect(ElementalComponent component)
    {
        var stats = component.GetStats();
        if (stats != null)
        {
            stats.ChangeAttackSpeed(1 + increaseAttackSpeed);
        }
    }

    public void RemovePassiveEffect(ElementalComponent component)
    {
        var stats = component.GetStats();
        if (stats != null)
        {
            stats.ReturnAttackSpeed();
        }
    }
}
