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

    }
    private void Start()
    {        // 싱글톤 패턴 적용
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        int rand = Random.Range(0, objectPrefab.Count);
        GameObject obj = Instantiate(objectPrefab[rand]);
        // 풀 초기화
        InitializePool();
    }


    // 풀 초기화 메서드
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            int rand = Random.Range(0, objectPrefab.Count);
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
            int rand = Random.Range(0,objectPrefab.Count);
            GameObject obj = Instantiate(objectPrefab[rand],transform.position, Quaternion.identity);
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

public static class ElementRelations
{
    private static Dictionary<Element, Dictionary<Element, float>> damageMultiplier = new Dictionary<Element, Dictionary<Element, float>>
    {
        // 위에서 정의한 damageMultiplier를 그대로 사용
        {
            Element.None, new Dictionary<Element, float>
            {
                { Element.None, 1.0f },
                { Element.Fire, 1.0f },
                { Element.Ice, 1.0f },
                { Element.Land, 1.0f },
                { Element.Light, 1.0f }
            }
        },
        {
            Element.Fire, new Dictionary<Element, float>
            {
                { Element.None, 1.0f },
                { Element.Fire, 1.0f },
                { Element.Ice, 1.25f },
                { Element.Land, 0.5f },
                { Element.Light, 1.0f }
            }
        },
        {
            Element.Ice, new Dictionary<Element, float>
            {
                { Element.None, 1.0f },
                { Element.Fire, 0.5f },
                { Element.Ice, 1.0f },
                { Element.Land, 1.0f },
                { Element.Light, 1.25f }
            }
        },
        {
            Element.Land, new Dictionary<Element, float>
            {
                { Element.None, 1.0f },
                { Element.Fire, 1.25f },
                { Element.Ice, 1.0f },
                { Element.Land, 1.0f },
                { Element.Light, 0.5f }
            }
        },
        {
            Element.Light, new Dictionary<Element, float>
            {
                { Element.None, 1.0f },
                { Element.Fire, 1.0f },
                { Element.Ice, 0.5f },
                { Element.Land, 1.25f },
                { Element.Light, 1.0f }
            }
        }
    };

    public static List<Element> GetOppositeElements(Element playerElement)
    {
        List<Element> oppositeElements = new List<Element>();
        var multipliers = damageMultiplier[playerElement];
        foreach (var kvp in multipliers)
        {
            if (kvp.Value == 0.5f)
            {
                oppositeElements.Add(kvp.Key);
            }
        }
        return oppositeElements;
    }
    private static List<Element> GetAdvantageousElements(Element playerElement)
    {
        List<Element> advantageousElements = new List<Element>();
        var multipliers = damageMultiplier[playerElement];
        foreach (var kvp in multipliers)
        {
            if (kvp.Value == 1.25f)
            {
                advantageousElements.Add(kvp.Key);
            }
        }
        return advantageousElements;
    }

    public static Element GetRandomElementBasedOnPlayer(Element playerElement)
    {
        float randomValue = UnityEngine.Random.Range(0f, 100f);

        if (randomValue < 40f)
        {
            // 40% 확률로 반대되는 속성 선택
            var oppositeElements = ElementRelations.GetOppositeElements(playerElement);
            if (oppositeElements.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, oppositeElements.Count);
                return oppositeElements[index];
            }
            else
            {
                // 반대되는 속성이 없으면 플레이어의 속성을 반환
                return playerElement;
            }
        }
        else if (randomValue < 70f)
        {
            // 다음 30% 확률로 동일한 속성 선택
            return playerElement;
        }
        else
        {
            // 나머지 30% 확률로 유리한 속성 선택
            var advantageousElements = ElementRelations.GetAdvantageousElements(playerElement);
            if (advantageousElements.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, advantageousElements.Count);
                return advantageousElements[index];
            }
            else
            {
                // 유리한 속성이 없으면 플레이어의 속성을 반환
                return playerElement;
            }
        }
    }
}