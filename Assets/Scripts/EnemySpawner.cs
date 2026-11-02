using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float jedaSpawn = 5f;
    [SerializeField] private int jumlahMaksimalEnemy = 6;

    private float waktuSpawn;

    void Update()
    {
        if (FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length >= jumlahMaksimalEnemy)
            return;

        waktuSpawn += Time.deltaTime;

        if (waktuSpawn >= jedaSpawn)
        {
            SpawnEnemy();
            waktuSpawn = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("Enemy Prefabs belum diisi!");
            return;
        }

        int percobaan = 0;
        GameObject prefabTerpilih = null;

        while (percobaan < enemyPrefabs.Length)
        {
            int index = Random.Range(0, enemyPrefabs.Length);

            if (enemyPrefabs[index] != null)
            {
                prefabTerpilih = enemyPrefabs[index];
                break;
            }

            percobaan++;
        }

        if (prefabTerpilih == null)
        {
            Debug.LogWarning("Tidak ada Enemy Prefab yang valid!");
            return;
        }

        Vector2 posisiSpawn = new Vector2(
            Random.Range(-8f, 8f),
            Random.Range(-4f, 4f)
        );

        Instantiate(prefabTerpilih, posisiSpawn, Quaternion.identity);
    }

}