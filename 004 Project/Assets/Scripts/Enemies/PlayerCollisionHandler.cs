using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public event Action OnPlayerHit;

    private bool isPlayerInvincible = false;
    public float invincibilityDuration = 1.0f; // �÷��̾ ��� ���� ���°� �Ǵ� �ð�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾ �̹� ���� ���¶�� ���� ����
        if (isPlayerInvincible)
            return;

        // ���Ͱ� �÷��̾ �����ϴ� ���
        if ((1 << collision.gameObject.layer).Equals(LayerMasks.Enemy))
        {
            OnPlayerHit?.Invoke(); // �÷��̾ ���ݴ����� �� ȣ��Ǵ� �̺�Ʈ
            StartCoroutine(InvincibilityCooldown());
        }
    }

    // �÷��̾ ������ ���� �� ���� �ð� ���� ���°� �Ǵ� ����
    private IEnumerator InvincibilityCooldown()
    {
        isPlayerInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isPlayerInvincible = false;
    }
}
