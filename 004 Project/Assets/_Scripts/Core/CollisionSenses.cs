using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectName
{
    public const string GroundCheck = "GroundCheck";
    public const string WallCheck = "WallCheck";
    public const string LedgeCheck = "LedgeCheck";

}

public class LayerMasks
{
    public const int Shield = 1 << 6;
    public const int Player = 1 << 7;
    public const int Ground = 1 << 8;
    public const int Enemy = 1 << 9;
    
}

public class CollisionSenses : CoreComponent
{

    public Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private Movement movement;

    #region Check Transforms
    public Transform GroundCheck { get => groundCheck; private set => groundCheck = value; }
    public Transform WallCheck { get => wallCheck; private set => wallCheck = value; }
    public Transform LedgeCheckVertical { get => ledgeCheckVertical; private set => ledgeCheckVertical = value; }
    public float GroundCheckRadius { get => groundCheckRadius; private set => groundCheckRadius = value; }
    public int WhatIsGround { get => whatIsGround; private set => whatIsGround = value; }


    private Transform groundCheck;
    private Transform wallCheck;
    private Transform ledgeCheckVertical;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private int whatIsGround = LayerMasks.Ground;

    //LayerMasks.Ground
    #endregion

    public bool Ground { get => Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsGround); }
    public bool WallFront { get => Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround); }
    public bool LedgeVertical { get => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallCheckDistance, whatIsGround); }

    protected override void Awake()
    {
        base.Awake();
        groundCheck = transform.Find(GameObjectName.GroundCheck);
      //  if (groundCheck == null)
            //Debug.Log(transform.root.name + "에 GroundCheck 없음.");

        wallCheck = transform.Find(GameObjectName.WallCheck);
        //if (wallCheck == null)
            //Debug.Log(transform.root.name + "에 WallCheck 없음.");
        
        ledgeCheckVertical = transform.Find(GameObjectName.LedgeCheck);
       // if (ledgeCheckVertical == null)
            //Debug.Log(transform.root.name + "에 LedgeCheck 없음.");
    }
}
