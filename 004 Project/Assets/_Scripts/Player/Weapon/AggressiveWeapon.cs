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
    private bool setAttackSpeed = false;

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

        // 공격속도가 1.0보다 크면 reInputTime이 감소하고, 1.0보다 작으면 증가
        // 하지만 변화 폭을 제한해서 너무 급격한 변화를 방지함
        float minAttackSpeed = 0.2f; // 공격 속도가 너무 느려지지 않도록 제한
        float maxAttackSpeed = 3.0f; // 공격 속도가 너무 빨라지지 않도록 제한

        // playerStats.AttackSpeed 값을 클램핑(최소와 최대값 사이로 제한)
        float clampedAttackSpeed = Mathf.Clamp(playerStats.AttackSpeed, minAttackSpeed, maxAttackSpeed);

        // 새로운 reInputTime 계산: 공격 속도가 빠를수록 시간은 줄어들고, 느릴수록 증가
        float adjustedReInputTime = reInputTime * (1 + (1 - clampedAttackSpeed) + 0.15f);

        // 코루틴 시작
        checkAttackReInputCor = coroutineHandler.StartManagedCoroutine(CheckAttackReInputCoroutine(adjustedReInputTime), ResetAttackCounter);
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
     //   Debug.Log("ResetAttackCounter");
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
        attackState.Movement?.SetVelocityZero();
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
            float baseDamage = weaponData.attackDamage[CurrentAttackCounter] * playerStats.AttackDamage;
            Element attackerElement = playerStats.Element; // 플레이어의 속성
            float attackerAttackStat = playerStats.AttackDamage;
            damageable.Damage(baseDamage, attackerElement, attackerAttackStat, gameObject, collision.transform); // 기본 데미지와 공격자의 속성 및 공격력 스탯 전달
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

    private void OnEnable()
    {
        if (setAttackSpeed)
        {
            baseAnimator.SetFloat("AttackSpeed", playerStats.AttackSpeed);
            weaponAnimator.SetFloat("AttackSpeed", playerStats.AttackSpeed);
        }
        setAttackSpeed = true;
    }
}
