using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public event Action OnPlayerHit;

    private bool isPlayerInvincible = false;
    public float invincibilityDuration = 1.0f; // 플레이어가 잠시 무적 상태가 되는 시간

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 이미 무적 상태라면 공격 무시
        if (isPlayerInvincible)
            return;

        // 몬스터가 플레이어를 공격하는 경우
        if ((1 << collision.gameObject.layer).Equals(LayerMasks.Enemy))
        {
            OnPlayerHit?.Invoke(); // 플레이어가 공격당했을 때 호출되는 이벤트
            StartCoroutine(InvincibilityCooldown());
        }
    }

    // 플레이어가 공격을 당한 후 일정 시간 무적 상태가 되는 로직
    private IEnumerator InvincibilityCooldown()
    {
        isPlayerInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isPlayerInvincible = false;
    }
}
