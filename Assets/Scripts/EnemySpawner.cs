using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TheraBytes.BetterUi.LocationAnimations;

public class EnemySpawner : MonoBehaviour
{
    public bool spawnEnemies;

    public LevelData levelData;
    public GameObject enemyPrefab;
    public GameObject player;
    public CrowdCapacityUpgrade crowdCapacityUpgrade;
    public CrystalManager crystalManager;
    GameManager gameManager;

    public int amountToSpawn;
    int currentLevel;

    public Vector3 enemySpawnAreaSize;
    public Vector3 enemySpawnPosition;

    [HideInInspector] public int spawnedEnemiesCount;

    float timer = 1f;
    float time = 0f;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    void Update()
    {
        currentLevel = gameManager.currentLevel;
        if (currentLevel > levelData.enemyCount.Count - 1)
        {
            currentLevel = levelData.enemyCount.Count - 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            SpawnEnemies(enemyPrefab, levelData.enemyCount[currentLevel], enemySpawnPosition, enemySpawnAreaSize);
        }

        time += Time.deltaTime;
        if (time > timer && spawnedEnemiesCount < levelData.enemyCount[currentLevel])
        {
            SpawnEnemiesWithDelay(enemyPrefab, enemySpawnPosition, enemySpawnAreaSize);
            spawnedEnemiesCount += 1;
            time = 0f;
        }

        if (spawnEnemies)
        {
            SpawnEnemies(enemyPrefab, levelData.enemyCount[currentLevel] / 2, enemySpawnPosition, enemySpawnAreaSize);
            spawnEnemies = false;
        }
    }

    void SpawnEnemies(GameObject prefab, int spawnNumber, Vector3 placementTarget, Vector3 size)
    {

        for (int i = 0; i < spawnNumber; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-size.x / 2, size.x / 2), 0.5f, Random.Range(-size.z / 2, size.z / 2)) + placementTarget;
            
            float angle = Random.Range(0f, 360f);
            Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.up);
            newRotation.x = 0;
            newRotation.z = 0;

            GameObject enemy = Instantiate(prefab, pos, newRotation);
            EnemyConnections(enemy);

            enemy.tag = "Enemy";

            spawnedEnemiesCount++;

            GetComponent<TotalEnemies>().RecordEnemies(enemy);
        }
    }

    private void SpawnEnemiesWithDelay(GameObject prefab, Vector3 placementTarget, Vector3 size)
    {
        Vector3 pos = new Vector3(Random.Range(-size.x / 2, size.x / 2), 0.5f, Random.Range(-size.z / 2, size.z / 2)) + placementTarget;

        float angle = Random.Range(0f, 360f);
        Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.up);
        newRotation.x = 0;
        newRotation.z = 0;

        GameObject enemy = Instantiate(prefab, pos, newRotation);
        EnemyConnections(enemy);

        prefab.tag = "Enemy";

        GetComponent<TotalEnemies>().RecordEnemies(enemy);
    }

    void EnemyConnections(GameObject enemy)
    {
        enemy.GetComponent<EnemyManager>().crowdCapacityUpgrade = crowdCapacityUpgrade;
        enemy.GetComponent<EnemyManager>().gameManager = gameObject;
        enemy.GetComponent<EnemyManager>().player = player;
        enemy.GetComponent<EnemyManager>().enabled = true;

        enemy.GetComponent<FriendManager>().gameManager = gameObject;
        enemy.GetComponent<FriendManager>().player = player;
        enemy.GetComponent<FriendManager>().crystalManager = crystalManager;
        enemy.GetComponent<FriendManager>().enabled = false;
    }

}