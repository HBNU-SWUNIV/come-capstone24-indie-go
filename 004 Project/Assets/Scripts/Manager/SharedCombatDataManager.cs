using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedCombatDataManager : MonoBehaviour
{

    public bool IsPlayerDashing { get; private set; }
    public bool IsPlayerWithinAttackRange { get; private set; }

    public bool IsPlayerHit { get; private set; }


    public Entity Attacker { get; private set; }
    public D_MeleeAttackState AttackStateData { get; private set; }
    public Collider2D Collision { get; private set; }

    public void TakeDamage(Entity attacker, D_MeleeAttackState stateData, Collider2D collision)
    {
        Attacker = attacker;
        AttackStateData = stateData;
        Collision = collision;
    }
    public void SetPlayerDashing(bool isDashing)
    {
        IsPlayerDashing = isDashing;
    }

    public void SetPlayerWithinAttackRange(bool isWithinRange)
    {
        IsPlayerWithinAttackRange = isWithinRange;
    }

    public void SetPlayerHit(bool isHit)
    {
        IsPlayerHit = isHit;
    }

    public void ClearAttackData()
    {
        Attacker = null;
        AttackStateData = null;
        Collision = null;
    }
}
