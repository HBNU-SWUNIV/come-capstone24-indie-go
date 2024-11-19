using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
//    public float maxHealth = 30f;

//    public float damageHopSpeed = 3f;

    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.4f;
    public float groundCheckRadius = 0.3f;

    public float minAgroDistance = 3f;
    public float maxAgroDistance = 4f;
    public float agroHeight = 2f; // 감지 높이

    public float parryStundurationTime = 7f;
//    public float stunResistance = 3f;
 //   public float stunRecoveryTime = 2f;

    public float closeRangeActionDistance = 1f;

    public float maxForwardBias = 1.0f; // 최대 탐지의 전방 이동 정도
    public float minForwardBias = 0.5f; // 최소 탐지의 전방 이동 정도
    public float closeForwardBias = 0.2f; // 근접 탐지의 전방 이동 정도
   
    // public GameObject hitParticle;

    //  public LayerMask whatIsGround;
    //   public LayerMask whatIsPlayer;
}
