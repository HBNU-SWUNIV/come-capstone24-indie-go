using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToAttackCheck : MonoBehaviour
{
    public bool isAlreadyHit { get; private set; }

    public event Action<Collider2D> OnPlayerHit;

    private bool isPlayerInvincible = false;
    public float collisionCooldown = 0.25f; // 중복 충돌을 무시할 시간

    protected CoroutineHandler coroutineHandler;
    private Coroutine checkInvincibilityCooldown;

    private void Start()
    {
        coroutineHandler = GetComponentInParent<CoroutineHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 이미 무적 상태라면 공격 무시
        if (isPlayerInvincible)
            return;

        // 몬스터가 플레이어를 공격하는 경우
        if ((1 << collision.gameObject.layer).Equals(LayerMasks.Player) && !isAlreadyHit)
        {
            isAlreadyHit = true;
            OnPlayerHit?.Invoke(collision); // 플레이어가 공격당했을 때 호출되는 이벤트
            checkInvincibilityCooldown = coroutineHandler.StartManagedCoroutine(InvincibilityCooldown());
        }
    }
    // 플레이어가 공격을 당한 후 일정 시간 무적 상태가 되는 로직
    private IEnumerator InvincibilityCooldown()
    {
        isPlayerInvincible = true;
        yield return new WaitForSeconds(collisionCooldown);
        isPlayerInvincible = false;
    }

    public void TriggerAttack()
    {
        isAlreadyHit = false;

        if (checkInvincibilityCooldown != null)
            coroutineHandler.StopCoroutine(checkInvincibilityCooldown);
    }
    public void FinishAttack()
    {
        bool isPlayerDashing = GameManager.SharedCombatDataManager.IsPlayerDashing;

        //isAlreadyHit가 false이고, isWithinAttackRange가 true일 때 플레이어가 대시를 사용했었다면

        if (isPlayerDashing)
        {
            if (!isAlreadyHit)
            {
                GameManager.PlayerManager.PlayerDataCollect.RecordAction(PlayerDataCollectName.DashSuccess);
            }
            else
            {
                GameManager.PlayerManager.PlayerDataCollect.RecordAction(PlayerDataCollectName.DashFailure);
            }
            GameManager.SharedCombatDataManager.SetPlayerDashing(false);
        }
        else
        {
            if (!isAlreadyHit)
            {
                GameManager.PlayerManager.PlayerDataCollect.RecordAction(PlayerDataCollectName.RunSuccess);
            }
        }

    }
}
