using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TotalEnemies : MonoBehaviour
{
    // Settings
    
    // Connections
    
    // State Variables
    [HideInInspector] public List<GameObject> totalEnemies = new List<GameObject>();
    bool startCount = false;

    void Start()
    {
        //InitConnections();
        //InitState();
    }

    void InitConnections()
    {

    }
    void InitState()
    {

    }

    void Update()
    {
        if (GetComponent<GameManager>().gameStarted && startCount)
        {
            if (totalEnemies.Count == 0)
            {
                GetComponent<GameManager>().OnFinishLevel();
                startCount = false;
            }
        }
    }

    public void RecordEnemies(GameObject enemy = null)
    {
        if (enemy != null)
        {
            if (enemy.CompareTag("Enemy"))
            {
                totalEnemies.Add(enemy);
                startCount = true;
            }
            else
            {
                totalEnemies.Remove(enemy);
            }
        }
    }
}

