using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Ctrl : MonoBehaviour
{
    public int start,end = 0;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            for(int i = start;i<end;i++)
            {
                GameObject obj = ObjectPool.instance.monster[i];
                obj.SetActive(true);

                Element playerElement = other.GetComponentInChildren<PlayerStats>().Element;


                // 몬스터의 속성 설정
                EnemyStats enemyStats = obj.GetComponentInChildren<EnemyStats>();

                if (enemyStats != null)
                    enemyStats.ChangeElement(ElementRelations.GetRandomElementBasedOnPlayer(playerElement), Random.Range(1,4));

            }
        }
    }
}
