using DG.Tweening;
using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriendsFormShield : MonoBehaviour
{
    //Variables
    [HideInInspector] public List<GameObject> friends = new List<GameObject>();
    [HideInInspector] public List<int> crowdCapacities = new List<int>();
    List<GameObject> positions = new List<GameObject>();
    
    [HideInInspector] public int prefabIndex;
    int pointCount;



    //Connections
    [Header("Connections")]
    public GameObject crowdSpline;
    public GameObject positionInCirclePrefab;
    public GameObject player;
    public GameObject mainCamera;
    public GameObject fullText;
    public GameObject gameManager;
    public CrystalManager crystalManager;

    Rigidbody playerRB;

    void Start()
    {
        playerRB = player.GetComponent<Rigidbody>();
        pointCount = crowdSpline.GetComponent<SplineComputer>().GetPoints().Count();
    }

    void Update()
    {
        ShieldInitializer();
    }


    void ShieldInitializer()
    {
        crowdSpline.transform.position = transform.position;

        if (friends.Count != 0)
        {
            mainCamera.GetComponent<FollowPlayer>().SetOffsetMultiplier(friends.Count * .02f + 1);
        }

        if (friends.Count >= gameManager.GetComponent<Upgrades>().crowdCapacity)
        {
            fullText.SetActive(true);

            fullText.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            fullText.SetActive(false);
        }
    }
    public void FormShield(GameObject friend = null)
    {
        if (friend != null)
        {
            if (friend.CompareTag("Friend"))
            {
                friends.Add(friend);
            }
            else
            {
                friends.Remove(friend);
            }

            CalculateCircle();
        }
    }
    void CalculateCircle()
    {
        foreach (var position in positions)
        {
            Destroy(position);
        }
        positions.Clear();

        if (friends.Count > 0)
        {
            for (int i = 0; i < friends.Count; i++)
            {
                Vector3 positionInsideCircle = crowdSpline.GetComponent<SplineComputer>().GetPoint(pointCount - i - 1).position;

                GameObject position = Instantiate(positionInCirclePrefab, positionInsideCircle, Quaternion.identity);
                positions.Add(position);

                PrefabFollowPlayer(position, i);
            }
        }
    }
    void SendEnemyToPosition(int index, GameObject position)
    {
        friends[index].GetComponent<FriendManager>().positionPrefab = position;
        friends[index].GetComponent<FriendManager>().joiningPlayer = false;
        friends[index].GetComponent<FriendManager>().joinedPlayer = false;
    }
    private void PrefabFollowPlayer(GameObject position, int index)
    {
        position.GetComponent<PositionPrefabScript>().playerRB = playerRB;
        position.GetComponent<PositionPrefabScript>().offsetFromPlayer = (playerRB.transform.position - position.transform.position);
        SendEnemyToPosition(index, position);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoneyCrystal"))
        {
            crystalManager.startMine = true;
            StartCoroutine(crystalManager.MoneyMineInitializer());
        }

        if (other.CompareTag("XpCrystal"))
        {
            crystalManager.startMine = true;
            StartCoroutine(crystalManager.XpMineInitializer());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MoneyCrystal") || other.CompareTag("XpCrystal"))
        {
            crystalManager.startMine = false;
        }
    }
}
