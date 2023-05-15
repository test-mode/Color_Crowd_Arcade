using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    //Settings
    public List<GameObject> enemies = new List<GameObject>();

    //Connections
    [HideInInspector] public GameObject currentTarget;
    [HideInInspector] public List<BulletManager> bulletManagers;
    public GameObject gameManager;
    public LevelData levelData;
    public BulletPool bulletPool;
    public GameObject player;

    //Variables
    [HideInInspector] public float fireRate;
    int friendsAmount;
    int crowdCapacity;
    float timer = 0f;
    float maxDistance = 10f;
    public bool fireBullets = false;
    Vector3 originalPosition;

    void Start()
    {
        bulletManagers = new List<BulletManager>();
        bulletManagers = bulletPool.PoolObjects();
    }

    public void Update()
    {
        friendsAmount = player.GetComponent<FriendsFormShield>().friends.Count;
        crowdCapacity = gameManager.GetComponent<Upgrades>().crowdCapacity;

        timer += Time.deltaTime;

        GameObject bullet = bulletPool.GetPooledObject();

        PickNearestTarget();

        if (gameManager.GetComponent<GameManager>().gameOver)
        {
            fireBullets = false;
        }

        if (bullet != null && fireBullets && timer > fireRate && !player.GetComponent<PlayerController>().onSafeZone && friendsAmount < crowdCapacity)
        {
            bullet.transform.position = transform.position;
            Vector3 bulletRotation = transform.rotation.eulerAngles;
            bulletRotation.x = 0;
            bulletRotation.z = 0;
            bullet.transform.rotation = Quaternion.Euler(bulletRotation);
            originalPosition = bullet.transform.position;
            bullet.SetActive(true);


            bullet.GetComponent<BulletManager>().originalPosition = originalPosition;
            bullet.GetComponent<BulletManager>().player = player;
            bullet.GetComponent<BulletManager>().currentTarget = currentTarget;

            timer = 0f;
        }
        
    }
    void PickNearestTarget()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null && enemy.CompareTag("Enemy"))
            {
                var distance = Vector3.Distance(player.transform.position, enemy.transform.position);
                var angle = Vector3.Angle(-player.transform.forward, player.transform.position - enemy.transform.position);
                var angleLimit = gameManager.GetComponent<Upgrades>().fireAngle;
                if (distance < maxDistance && angle < angleLimit)
                {
                    currentTarget = enemy;
                    fireBullets = true;
                    break;
                }
                else
                {
                    fireBullets = false;
                }
            }
            else
            {
                fireBullets = false;
                continue;
            }
        }
    }

    public void GetEnemies(GameObject enemy = null)
    {
        if (enemy != null)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemies.Add(enemy);
            }
            else
            {
                enemies.Remove(enemy);
            }
        }
    }

}
