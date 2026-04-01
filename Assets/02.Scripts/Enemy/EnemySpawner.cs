using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyController> enemyPool;
    private Transform player;        // 플레이어 위치
    private float spawnRadius = 15f; // 스폰 거리 (카메라 밖이어야 함)
    private float spawnTime = 5.0f;  // 스폰 간격

    private float timer = 0.0f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            SpawnEnemy();
            timer = 0;
        }
    }

    private void SpawnEnemy()
    {
        if (player == null) 
            return;

        // 플레이어 주변 랜덤 각도 계산
        float angle = Random.Range(0f, Mathf.PI * 2);

        // 원의 경계 지점 좌표 계산
        Vector2 spawnPos = new Vector2(
            player.position.x + Mathf.Cos(angle) * spawnRadius,
            player.position.y + Mathf.Sin(angle) * spawnRadius
        );

        // 랜덤 적 선택 및 생성
        int randomIndex = Random.Range(0, enemyPool.Count);
        GameObject enemyObj = Instantiate(enemyPool[randomIndex].gameObject, spawnPos, Quaternion.identity);

        // AI 타겟 설정
        if (enemyObj.TryGetComponent(out EnemyController enemy))
        {
            enemy.Setup(player);
        }
    }
}
