using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;

    [Header("In Air State")]
    public float coyotoTime = 0.2f;

    [Header("Hit State")]
    public float hitRecoveryTime = 0.25f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 2;

    [Header("Dash State")]
    public float dashCoolDown = 0.2f;
    public float dashVelocity = 20f;
    public float dashTime = 0.2f;
    public float dashEndYMultiplier = 0.2f;

}
