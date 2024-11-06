using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollisionHandler : MonoBehaviour
{
    public event Action<Collider2D> OnColliderDetected;
    private List<Collider2D> detectedColliders = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 이미 감지된 콜라이더라면 무시
        if (detectedColliders.Contains(collision))
        {
            return;
        }

        if ((1 << collision.gameObject.layer).Equals(LayerMasks.Enemy))
        {
            if (!detectedColliders.Contains(collision))
            {
                detectedColliders.Add(collision);
                OnColliderDetected?.Invoke(collision);
            }
        }
    }

    private void OnDisable()
    {
        detectedColliders.Clear(); // 공격이 끝나면 초기화
    }
}