using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class XpDiamondPrefabManager : MonoBehaviour
{
    // Settings

    // Connections
    public GameObject player;
    public GameObject gameManager;
    public GameObject parent;

    // State Variables
    public float distanceToSuck;
    public float suckingSpeed;

    float elapsedTime;
    float duration = 2f;

    public bool canMoveToPlayer = false;

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
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToSuck && canMoveToPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, elapsedTime / duration);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.GetComponent<XpManager>().playerTotalXp += 5;
            Destroy(parent);
        }
    }
}

