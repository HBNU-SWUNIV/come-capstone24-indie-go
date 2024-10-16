using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Sword")]
public class Sword_WeaponData : ScriptableObject
{
    public float[] movementSpeed = new float[] { 2, 5, 4 };

    public float[] attackDamage = new float[] { 1f, 1.1f, 1.2f };
    public int numberOfAttacks = 3;

    public float reInputTime = 0.55f;

    public Vector2 knockbackAngle = Vector2.one;

    public float knockbackStrength = 10f;
}
