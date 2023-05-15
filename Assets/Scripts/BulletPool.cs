using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{

    public static BulletPool SharedInstance;
    public List<GameObject> pooledObjects;
    public List<BulletManager> bulletManagers;
    public GameObject objectToPool;
    public int amountToPool;
    public GameObject player;

    void Awake()
    {
        SharedInstance = this;
        bulletManagers = new List<BulletManager>();
    }

    void Start()
    {
        
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    public List<BulletManager> PoolObjects()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
            bulletManagers.Add(tmp.GetComponent<BulletManager>());

            tmp.GetComponent<BulletManager>().player = player;
        }

        return bulletManagers;
    }
}
