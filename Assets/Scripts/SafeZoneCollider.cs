using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SafeZoneCollider : MonoBehaviour
{
    // Settings

    // Connections
    public GameObject player;

    // State Variables
    

    void Start()
    {
        //InitConnections();
        //InitState();
        Physics.IgnoreCollision(GetComponent<BoxCollider>(), player.GetComponent<Collider>());
    }

    void InitConnections()
    {

    }
    void InitState()
    {

    }

    void Update()
    {
        
    }
}

