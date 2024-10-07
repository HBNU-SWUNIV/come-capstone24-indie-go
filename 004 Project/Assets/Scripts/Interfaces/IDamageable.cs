using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void NonElementDamage(float amount, Transform defender);
    void SkillDamage(float baseDamage, Element attackerElement, float attackerDamageStats, GameObject attacker, Transform defender);
    void Damage(float baseDamage, Element attackerElement, float attackerDamageStats, GameObject attacker, Transform defender);
    void DamageWithShield(float amount, Transform defender);
}
