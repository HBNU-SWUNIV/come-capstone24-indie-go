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
            monsterStats.SetMonsterId(monsterId); // ���� ID�� �����ϰ� ������ �ʱ�ȭ
        }
        else
        {
            Debug.LogError("Monster prefab does not have MonsterStats component");
        }
    }
}
