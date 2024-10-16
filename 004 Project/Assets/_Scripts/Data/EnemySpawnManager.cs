using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;

    public void SpawnMonster(int monsterId, Vector3 spawnPosition)
    {
        GameObject monsterObject = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        EnemyStats monsterStats = monsterObject.GetComponent<EnemyStats>();

        if (monsterStats != null)
        {
            monsterStats.SetMonsterId(monsterId); // 몬스터 ID를 설정하고 스탯을 초기화
        }
        else
        {
            Debug.LogError("Monster prefab does not have MonsterStats component");
        }
    }
}
