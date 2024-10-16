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

    public float collisionCooldown = 0.25f; // �ߺ� �浹�� ������ �ð�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �̹� ������ �ݶ��̴���� ����
        if (detectedColliders.Contains(collision))
        {
            return;
        }

        // �浹�� ���� ���� �ð� ���� �ٽ� �����Ǿ����� Ȯ��
        if (colliderCooldowns.ContainsKey(collision))
        {
            if (Time.time - colliderCooldowns[collision] < collisionCooldown)
            {
                // ��ٿ� �ð��� ���� ������ ����
                return;
            }
            else
            {
                // ��ٿ��� ������ ��ٿ� ��Ͽ��� ����
                colliderCooldowns.Remove(collision);
            }
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

    // �˹����� ���� ������ ����� �ٽ� �ѹ� �� ���� ���� ����.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (detectedColliders.Contains(collision))
        {
            detectedColliders.Remove(collision);
            colliderCooldowns[collision] = Time.time; // ���� �ð��� ���
        }
    }
}