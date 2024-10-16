using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class AggressiveWeaponHitboxToWeapon : MonoBehaviour
{
    private AggressiveWeapon weapon;
    private bool isAlreadyHit;

    private void Awake()
    {
        weapon = GetComponentInParent<AggressiveWeapon>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer).Equals(LayerMasks.Enemy) && !isAlreadyHit)
        {
            isAlreadyHit = true;
            //얘도 AggressiveWeapon을 하는게 아니라, 그냥 Interface를 받아오고, 공통적으로 collider 됬을 때 실행할 함수를 실행시킨다.
            weapon.CheckAttack(collision);
        }
    }

    public void resetAlreadyHit()
    {
        isAlreadyHit = false;
    }
}

*/