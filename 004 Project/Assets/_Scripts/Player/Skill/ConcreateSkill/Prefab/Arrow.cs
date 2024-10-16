using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Arrow : MonoBehaviour
{
    private CollisionHandler collisionHandler;
    private Rigidbody2D rb;

    private float currentDistance; // 이동한 거리를 저장하는 변수 추가
    private float maxDistance; // 화살이 날아갈 최대 거리

    private void FixedUpdate()
    {
        currentDistance += rb.velocity.magnitude * Time.fixedDeltaTime; // 화살이 이동한 거리 계산
        if (currentDistance >= maxDistance) // 일정 거리 이상 이동하면 삭제
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        collisionHandler = GetComponent<CollisionHandler>();
        rb = GetComponent<Rigidbody2D>();
        currentDistance = 0f; // 초기화

        collisionHandler.OnColliderDetected += Detected;

    }

    private void Detected(Collider2D collision)
    {
        Debug.Log("화살삭제");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        collisionHandler.OnColliderDetected -= Detected;
    }

    public void SetThrowDistance(float distance)
    {
        maxDistance = distance;
    }
}
