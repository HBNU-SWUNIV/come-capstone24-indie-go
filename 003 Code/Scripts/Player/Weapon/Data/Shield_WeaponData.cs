using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Shield_WeaponData", menuName = "Data/Weapon Data/Shield")]
public class Shield_WeaponData : ScriptableObject
{
    public float parryStun = 0.5f;
    public float parryDamage = 2f;
    public Vector2 knockbackAngle = Vector2.one;
    public float knockbackStrength = 5f;

    public float enterTime = 0.3f;

    [Header("ShieldHold State")]
    public float holdMovementVelocity = 5f;
    
}
