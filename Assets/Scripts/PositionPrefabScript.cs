using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionPrefabScript : MonoBehaviour
{
    public Vector3 offsetFromPlayer;
    public Rigidbody playerRB;

    void Start()
    {
        
    }


    void Update()
    {
        if (playerRB != null && offsetFromPlayer != null)
        {
            transform.position = playerRB.transform.position - offsetFromPlayer;
        }
    }
}
