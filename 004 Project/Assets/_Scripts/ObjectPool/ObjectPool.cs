using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    public List<GameObject> objectPrefab;   // 풀링할 오브젝트의 프리팹
    public List<GameObject> monster;
    public int poolSize = 350;         // 초기 풀 크기
    private Queue<GameObject> objectPool = new Queue<GameObject>();  // 오브젝트 풀

    private void Awake()
    {
        // 싱글톤 패턴 적용
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 풀 초기화
        InitializePool();
    }


    // 풀 초기화 메서드
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            int rand = Random.Range(0, 3);
            GameObject obj = Instantiate(objectPrefab[rand]);
            obj.SetActive(false); // 비활성화 상태로 초기화
            objectPool.Enqueue(obj);
        }
    }

    // 풀에서 오브젝트 가져오기
    public GameObject GetObjectFromPool(Vector3 pos)
    {
        if (objectPool.Count > 0)
        {
            GameObject obj = objectPool.Dequeue();
            monster.Add(obj);
            obj.transform.position = pos;
            // obj.SetActive(true);
            return obj;
        }
        else
        {
            // 풀에 더 이상 오브젝트가 없으면 새로운 오브젝트 생성
            int rand = Random.Range(0,3);
            GameObject obj = Instantiate(objectPrefab[rand]);
            monster.Add(obj);
            obj.transform.position = pos;
            // obj.SetActive(true);
            return obj;
        }
    }
    public void Active_Monster(int start, int end)
    {

    }
    // 오브젝트 풀에 반환하기
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }
}

