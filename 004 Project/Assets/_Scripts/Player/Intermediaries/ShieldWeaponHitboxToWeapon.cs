using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class ShieldWeaponHitboxToWeapon : MonoBehaviour
{
    private DefensiveWeapon weapon;
    private bool isAlreadyHit;
    public bool isDefending;
    private void Awake()
    {
        weapon = GetComponentInParent<DefensiveWeapon>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer).Equals(LayerMasks.Enemy)) //&& !isAlreadyHit)
        {
            //isAlreadyHit = true;
            isDefending = true;

         //   MeleeAttackState attackState = collision.transform.root.GetComponentInChildren<MeleeAttackState>();
         // if(attackState != null)
         //   weapon.CheckShield(collision, attackState.AttackDamage, attackState.KnockbackAngle, attackState.KnockbackStrength, attackState.FacingDirection);
        }
    }

    public void ResetAlreadyHit()
    {
        isAlreadyHit = false;
    }

}
*/