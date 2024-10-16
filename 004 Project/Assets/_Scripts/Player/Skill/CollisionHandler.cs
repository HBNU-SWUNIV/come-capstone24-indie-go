using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollisionHandler : MonoBehaviour
{
    public event Action<Collider2D> OnColliderDetected;
    //public LayerMask targetLayer;
    private List<Collider2D> detectedColliders = new List<Collider2D>();
    private Dictionary<Collider2D, float> colliderCooldowns = new Dictionary<Collider2D, float>();

    public float collisionCooldown = 0.2f; // 중복 충돌을 무시할 시간

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 이미 감지된 콜라이더라면 무시
        if (detectedColliders.Contains(collision))
        {
            return;
        }

        // 충돌한 적이 일정 시간 내에 다시 감지되었는지 확인
        if (colliderCooldowns.ContainsKey(collision))
        {
            if (Time.time - colliderCooldowns[collision] < collisionCooldown)
            {
                Debug.Log("아직 쿨다운이 지나지 안흥ㅁ");
                // 쿨다운 시간이 아직 지나지 않음
                return;
            }
            else
            {
                // 쿨다운이 지나면 쿨다운 기록에서 제거
                colliderCooldowns.Remove(collision);
            }
        }

        if ((1 << collision.gameObject.layer).Equals(LayerMasks.Enemy))
        {
            if (!detectedColliders.Contains(collision))
            {
                detectedColliders.Add(collision);
                OnColliderDetected?.Invoke(collision);
                colliderCooldowns[collision] = Time.time; // 공격한 시간을 기록
            }
        }
    }

    // 넉백으로 인해 범위를 벗어나고 다시 한번 더 맞을 때가 있음.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (detectedColliders.Contains(collision))
        {
            detectedColliders.Remove(collision);
        }
    }
}