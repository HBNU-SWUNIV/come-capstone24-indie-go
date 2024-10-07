using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollisionHandler : MonoBehaviour
{
    public event Action<Collider2D> OnColliderDetected;
    //public LayerMask targetLayer;
    private List<Collider2D> detectedColliders = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer).Equals(LayerMasks.Enemy))
        {
            if (!detectedColliders.Contains(collision))
            {
                detectedColliders.Add(collision);
                OnColliderDetected?.Invoke(collision);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (detectedColliders.Contains(collision))
        {
            detectedColliders.Remove(collision);
        }
    }
}