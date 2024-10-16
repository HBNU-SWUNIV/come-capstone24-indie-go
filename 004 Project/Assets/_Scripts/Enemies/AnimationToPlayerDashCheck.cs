using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToPlayerDashCheck : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer).Equals(LayerMasks.Player))
        {
            GameManager.SharedCombatDataManager.SetPlayerWithinAttackRange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer).Equals(LayerMasks.Player))
        {
            GameManager.SharedCombatDataManager.SetPlayerWithinAttackRange(false);
        }
    }

    public void TriggerCheck()
    {
        gameObject.SetActive(true);
    }
    public void FinishCheck()
    {
        gameObject.SetActive(false);
    }
}
