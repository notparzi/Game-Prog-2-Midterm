using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyBase[] enemyPrefabs;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private Transform player;

    private float timer;

    void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            Vector2 circle = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 pos = player.position + new Vector3(circle.x, 0f, circle.y);
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], pos, Quaternion.identity);
        }
    }
}