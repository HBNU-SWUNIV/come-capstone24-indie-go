using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform을 참조할 변수
    public float followSpeed = 5f; // 따라가는 속도
    public float distanceThreshold = 0.5f; // 플레이어와의 최소 거리, 이 거리 이하로 가까워지면 멈춤

    void Update()
    {
        if (player == null)
        {
            //Debug.LogWarning("Player Transform is not assigned.");
            return;
        }

        // 플레이어와의 거리 계산
        float distance = Vector3.Distance(transform.position, player.position);

        // 플레이어와의 거리가 특정 임계값보다 클 때만 이동
        if (distance > distanceThreshold)
        {
            // 플레이어를 향한 방향 계산
            Vector3 direction = (player.position - transform.position).normalized;

            // 오브젝트를 플레이어 방향으로 이동
            transform.position += direction * followSpeed * Time.deltaTime;
        }
    }
}
