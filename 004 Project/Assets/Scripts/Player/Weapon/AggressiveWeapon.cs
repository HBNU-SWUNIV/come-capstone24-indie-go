using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveWeapon : Weapon, IWeapon
{
    [SerializeField] protected Sword_WeaponData weaponData;

    private bool resetCounter;
    private Coroutine checkAttackReInputCor;

    protected int CurrentAttackCounter { get => currentAttackCounter; set => currentAttackCounter = value >= weaponData.numberOfAttacks ? 0 : value; }
    private int currentAttackCounter;

    protected CoroutineHandler coroutineHandler;

    protected override void Start()
    {
        base.Start();

        //SetWeaponData(so_weapondata);
        coroutineHandler = GetComponentInParent<CoroutineHandler>();


        weaponAnimationToWeapon.OnAction += AnimationActionTrigger;
        weaponAnimationToWeapon.OnFinish += AnimationFinishTrigger;
        weaponAnimationToWeapon.OnStartMovement += AnimationStartMovementTrigger;
        weaponAnimationToWeapon.OnStopMovement += AnimationStopMovementTrigger;
        weaponAnimationToWeapon.OnTurnOnFlip += AnimationTurnOnFlipTrigger;
        weaponAnimationToWeapon.OnTurnOffFlip += AnimationTurnOffFlipTrigger;
    }//+=했으면 -=도 해줘야함.

    public override void EnterWeapon()
    {
        base.EnterWeapon();

        baseAnimator.SetInteger("Counter", CurrentAttackCounter);
        weaponAnimator.SetInteger("Counter", CurrentAttackCounter);

        resetCounter = false;

        GameManager.PlayerManager.PlayerDataCollect.RecordAction(PlayerDataCollectName.AttackAttempt);

        CheckAttackReInput(weaponData.reInputTime);
    }

    private void CheckAttackReInput(float reInputTime)
    {
        if (checkAttackReInputCor != null)
            coroutineHandler.StopCoroutine(checkAttackReInputCor);
        checkAttackReInputCor = coroutineHandler.StartManagedCoroutine(CheckAttackReInputCoroutine(reInputTime), ResetAttackCounter);
    }

    private IEnumerator CheckAttackReInputCoroutine(float reInputTime)
    {
        float currentTime = 0f;
        while (currentTime < reInputTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    private void ResetAttackCounter()
    {
        Debug.Log("ResetAttackCounter");
        resetCounter = true;
        CurrentAttackCounter = 0;

    }

    public override void ExitWeapon()
    {
        base.ExitWeapon();
        if (!resetCounter)
        {
            CurrentAttackCounter++;
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

      //  aggressiveWeaponHitboxToWeapon.resetAlreadyHit();

    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        attackState.AnimationFinishTrigger();
    }
    public void AnimationStartMovementTrigger()
    {
        attackState.Movement?.SetVelocityX(weaponData.movementSpeed[CurrentAttackCounter] * attackState.Movement.FacingDirection);
    }
    public void AnimationStopMovementTrigger()
    {
        attackState.Movement?.SetVelocityX(0);
    }

    public void AnimationTurnOnFlipTrigger()
    {
        attackState.SetFilpCheck(true);

        //movement따로 돌릴곳이필요..
    }
    public void AnimationTurnOffFlipTrigger()
    {
        attackState.SetFilpCheck(false);
    }

    public override void HandleCollision(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponentInChildren<IDamageable>();

        if (damageable != null)
        {
            damageable.Damage(weaponData.attackDamage[CurrentAttackCounter] * playerStats.AttackDamage); // 공격 계수 * playerAttackDamage
            Debug.Log("데미지 : " + weaponData.attackDamage[CurrentAttackCounter] * playerStats.AttackDamage);
            //detectedDamageable.Add(damageable);
            GameManager.PlayerManager.PlayerDataCollect.RecordAction(PlayerDataCollectName.AttackSuccess);
        }
        IKnockbackable knockbackable = collision.GetComponentInChildren<IKnockbackable>();
        if (knockbackable != null)
        {
            knockbackable.Knockback(weaponData.knockbackAngle, weaponData.knockbackStrength, attackState.Movement.FacingDirection); // 적의 체급에 따른 넉백 정도
        }
    }
}
