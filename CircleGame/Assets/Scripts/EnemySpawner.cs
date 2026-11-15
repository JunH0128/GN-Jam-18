using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float spawnInterval = 10f;

    public Vector2 spawnAreaMin = new Vector2(-8f, -3f);
    public Vector2 spawnAreaMax = new Vector2(8f, 3f);

    private Transform player;
    private float timer;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Check the Player tag.");
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        if (player == null || enemyPrefab == null) return;

        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

        Vector2 spawnPosition = new Vector2(randomX, randomY);

        GameObject enemy = Instantiate(enemyPrefab,spawnPosition,Quaternion.identity);

        AsteroidFly asteroid = enemy.GetComponent<AsteroidFly>();

        if (asteroid != null)
        {
            asteroid.player = player;
        }
        else
        {
            Debug.LogError("AsteroidFly script not found on spawned enemy!");
        }
    }
}
