using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveWeapon : Weapon
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
    }//+=������ -=�� �������.

    public override void EnterWeapon()
    {
        base.EnterWeapon();

        baseAnimator.SetInteger("Counter", CurrentAttackCounter);
        weaponAnimator.SetInteger("Counter", CurrentAttackCounter);

        resetCounter = false;

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


    public void CheckAttack(Collider2D collision)
    {

        IDamageable damageable = collision.GetComponentInChildren<IDamageable>();

        if (damageable != null)
        {
            damageable.Damage(weaponData.attackDamage[CurrentAttackCounter]); // ���� ��� * playerAttackDamage
            //detectedDamageable.Add(damageable);
        }
        IKnockbackable knockbackable = collision.GetComponentInChildren<IKnockbackable>();
        if(knockbackable != null)
        {
            knockbackable.Knockback(weaponData.knockbackAngle, weaponData.knockbackStrength, attackState.Movement.FacingDirection); // ���� ü�޿� ���� �˹� ����
        }
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        aggressiveWeaponHitboxToWeapon.resetAlreadyHit();

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

        //movement���� ���������ʿ�..
    }
    public void AnimationTurnOffFlipTrigger()
    {
        attackState.SetFilpCheck(false);
    }
}
