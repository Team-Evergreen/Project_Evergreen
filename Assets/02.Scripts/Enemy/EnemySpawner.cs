using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float spawnRadius = 15f;
    [SerializeField] private float spawnTime = 5.0f;

    private float timer = 0.0f;

    private void Awake()
    {
        FindPlayer();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        if (player == null) FindPlayer();
        if (player == null || ObjectPool.Instance == null)
            return;

        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector2 spawnPos = new Vector2(
            player.position.x + Mathf.Cos(angle) * spawnRadius,
            player.position.y + Mathf.Sin(angle) * spawnRadius
        );

        EnemyController enemy = ObjectPool.Instance.GetEnemy();
        if (enemy == null) return;

        enemy.Setup(player, spawnPos);
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) player = playerObject.transform;
    }
}
