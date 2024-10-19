using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // 몬스터 프리팹들을 저장할 배열
    public GameObject[] monsterPrefabs;

    // 현재 선택된 몬스터 인덱스와 속성
    private int selectedMonsterIndex = 5;
    private Element selectedElement = Element.None;
    private int selectedElementLevel = 1;

    void Update()
    {
        HandleMonsterSelection();
        HandleElementSelection();
        HandleElementLevelSelection();
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnSelectedMonster();
        }
    }

   
    private void HandleMonsterSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedMonsterIndex = 0;
            Debug.Log("Selected Monster: Enemy1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectedMonsterIndex = 1;
            Debug.Log("Selected Monster: Goblin");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            selectedMonsterIndex = 2;
            Debug.Log("Selected Monster: FlyingEye");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            selectedMonsterIndex = 3;
            Debug.Log("Selected Monster: Enemy2");
        }
    }

    private void HandleElementSelection()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            selectedElement = Element.Fire;
            Debug.Log("Selected Element: Fire");
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            selectedElement = Element.Ice;
            Debug.Log("Selected Element: Ice");
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            selectedElement = Element.Land;
            Debug.Log("Selected Element: Land");
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            selectedElement = Element.Light;
            Debug.Log("Selected Element: Light");
        }
    }

    private void HandleElementLevelSelection()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            selectedElementLevel = 1;
            Debug.Log("Selected ElementLevel: 1");
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            selectedElementLevel = 2;
            Debug.Log("Selected ElementLevel: 2");
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            selectedElementLevel = 3;
            Debug.Log("Selected ElementLevel: 3");
        }
    }
    private void SpawnSelectedMonster()
    {
        if (selectedMonsterIndex < 0 || selectedMonsterIndex >= monsterPrefabs.Length)
        {
            Debug.Log("몬스터 종류 선택 안함");
            return;
        }
        if (selectedElement == Element.None)
        {
            Debug.Log("몬스터 속성 선택 안함");
            return;
        }
        

        GameObject monsterPrefab = monsterPrefabs[selectedMonsterIndex];
        GameObject monsterInstance = Instantiate(monsterPrefab, transform.position, Quaternion.identity);

        monsterInstance.SetActive(true);
        // 몬스터의 속성 설정
        EnemyStats enemyStats = monsterInstance.GetComponentInChildren<EnemyStats>();
           if (enemyStats != null)
           {
               enemyStats.ChangeElement(selectedElement, selectedElementLevel);
               Debug.Log($"Spawned Monster with Element: {selectedElement}");
           }
           else
           {
               Debug.LogError("EnemyStats component not found on the monster prefab.");
           }

        selectedMonsterIndex = 5;
        selectedElement = Element.None;
        selectedElementLevel = 1;
    }
}
