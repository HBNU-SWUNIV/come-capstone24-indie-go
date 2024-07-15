using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToAttackCheck : MonoBehaviour
{
    private AttackState attackState;
    public bool isAlreadyHit { get; private set; }
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log(collision.name);
        if ((1 << collision.gameObject.layer).Equals(LayerMasks.Player) && !isAlreadyHit)
        {        
            isAlreadyHit = true;
            attackState.HandleAttack(collision);
        }
    }
    
    public void TriggerAttack()
    {
        isAlreadyHit = false;
        gameObject.SetActive(true);
    }
    public void FinishAttack()
    {
        gameObject.SetActive(false);

        bool isPlayerDashing = GameManager.SharedCombatDataManager.IsPlayerDashing;

        //isAlreadyHit�� false�̰�, isWithinAttackRange�� true�� �� �÷��̾ ��ø� ����߾��ٸ�

        if(isPlayerDashing)
        {
            if(!isAlreadyHit)
            {
                GameManager.PlayerManager.PlayerDataCollect.RecordAction(PlayerDataCollectName.DashSuccess);
            }
            else
            {
                GameManager.PlayerManager.PlayerDataCollect.RecordAction(PlayerDataCollectName.DashFailure);
            }
            GameManager.SharedCombatDataManager.SetPlayerDashing(false);
        }

    }

    public void InitializedAttackCheck(AttackState attackState)
    {
        this.attackState = attackState;
    }
}
