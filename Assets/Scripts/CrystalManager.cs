using DG.Tweening.Core.Easing;
using DG.Tweening;
using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrystalManager : MonoBehaviour
{
    // Settings

    // Connections
    [Header("Connections")]
    public GameObject gameManager;
    public GameObject player;
    public GameObject moneyMine;
    public GameObject xpMine;
    public GameObject grinder;
    public GameObject blueDiamondPrefab;
    public GameObject redDiamondPrefab;
    public GameObject moneyPrefab;

    [HideInInspector] public List<GameObject> moneyWorkers = new List<GameObject>();
    [HideInInspector] public List<GameObject> xpWorkers = new List<GameObject>();

    [HideInInspector] public List<GameObject> extraMoneyWorkers = new List<GameObject>();
    [HideInInspector] public List<GameObject> extraXpWorkers = new List<GameObject>();

    FriendsFormShield friendsFormShield;

    // State Variables
    [Header("Variables")]
    public float timeBetweenWorkerJoiningCrystal;
    public float singleWorkerProduceDiamondDuration;
    public int moneyMineWorkerCapacity;
    public int XpMineWorkerCapacity;

    [HideInInspector] public bool startMine;

    float moneyCrystalTime;
    float xpCrystalTime;

    void Start()
    {
        InitConnections();
        InitState();
    }

    void InitConnections()
    {
        friendsFormShield = player.GetComponent<FriendsFormShield>();
    }
    void InitState()
    {
        moneyCrystalTime = 0f;
        xpCrystalTime = 0f;
        startMine = true;
    }

    void Update()
    {
        if (moneyWorkers.Count > 0)
        {
            SpawnMoneyDiamond();
        }

        if (xpWorkers.Count > 0)
        {
            SpawnXpDiamond();
        }
    }

    void SpawnMoneyDiamond()
    {
        moneyCrystalTime += Time.deltaTime;
        if (moneyCrystalTime > singleWorkerProduceDiamondDuration / (moneyWorkers.Count + extraMoneyWorkers.Count))
        {
            moneyCrystalTime = 0f;
            GameObject diamond = Instantiate(blueDiamondPrefab, moneyMine.transform.position, Quaternion.identity);
            TweenSequence(diamond, moneyMine, singleWorkerProduceDiamondDuration / (moneyWorkers.Count + extraMoneyWorkers.Count));
        }
    }
    void SpawnXpDiamond()
    {
        xpCrystalTime += Time.deltaTime;
        if (xpCrystalTime > singleWorkerProduceDiamondDuration / (xpWorkers.Count + extraXpWorkers.Count))
        {
            xpCrystalTime = 0f;
            GameObject diamond = Instantiate(redDiamondPrefab, xpMine.transform.position, Quaternion.identity);
            diamond.tag = "XpDiamond";
            diamond.GetComponent<XpDiamondPrefabManager>().player = player;
            diamond.GetComponent<XpDiamondPrefabManager>().gameManager = gameManager;
            diamond.transform.DOMoveY(diamond.transform.position.y + 6, 1).OnComplete(() =>
            {
                diamond.GetComponent<XpDiamondPrefabManager>().canMoveToPlayer = true;
            });
        }
    }
    private void TweenSequence(GameObject diamond, GameObject mine, float time)
    {
        float height = 5f;

        Vector3 moneyMineUp = new Vector3(mine.transform.position.x, mine.transform.position.y + height, mine.transform.position.z);
        Vector3 grinderUp = new Vector3(grinder.transform.position.x, grinder.transform.position.y + height, grinder.transform.position.z);
        Vector3 grinderPos = grinder.transform.position;

        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(diamond.transform.DOMove(moneyMineUp, time / 3));
        mySequence.Append(diamond.transform.DOMove(grinderUp, time / 3));
        mySequence.Append(diamond.transform.DOMove(grinderPos, time / 3)).OnComplete(() =>
        {
            SpawnMoney();
            Destroy(diamond);
        });

        mySequence.Play();
    }
    public void SpawnMoney()
    {
        Vector3 moneySpawnPos = grinder.transform.position + new Vector3(-1, 0, 0);
        GameObject moneyObj = Instantiate(moneyPrefab, moneySpawnPos, Quaternion.identity);
        moneyObj.transform.Rotate(0, 90, 90);
        moneyObj.GetComponent<MoneyPrefabManager>().gameManager = gameManager;
        moneyObj.GetComponent<MoneyPrefabManager>().player = player;
        moneyObj.GetComponent<Rigidbody>().mass = 30f;
        Vector3 moneyDestination = new Vector3(moneyObj.transform.position.x + 3, moneyObj.transform.position.y, moneyObj.transform.position.z);
        moneyObj.transform.DOMove(moneyDestination, 1).OnComplete(() =>
        {
            moneyObj.GetComponent<MoneyPrefabManager>().canMoveToPlayer = true;
        });
    }
    public IEnumerator MoneyMineInitializer()
    {
        for (int i = friendsFormShield.friends.Count - 1; i >= 0; i = friendsFormShield.friends.Count - 1)
        {
            if (startMine && moneyWorkers.Count < moneyMineWorkerCapacity)
            {
                FormCircleAroundMine(friendsFormShield.friends[i], moneyWorkers, moneyMine, extraMoneyWorkers);
                yield return new WaitForSeconds(timeBetweenWorkerJoiningCrystal);
            }
            else
            {
                break;
            }
        }

    }
    public IEnumerator XpMineInitializer()
    {
        for (int i = friendsFormShield.friends.Count - 1; i >= 0; i = friendsFormShield.friends.Count - 1)
        {
            if (startMine && xpWorkers.Count < XpMineWorkerCapacity)
            {
                FormCircleAroundMine(friendsFormShield.friends[i], xpWorkers, xpMine, extraXpWorkers);
                yield return new WaitForSeconds(timeBetweenWorkerJoiningCrystal);
            }
            else
            {
                break;
            }
        }

    }
    void FormCircleAroundMine(GameObject worker, List<GameObject> mineWorkers, GameObject mine, List<GameObject> extaWorkers)
    {
        mineWorkers.Add(worker);
        for (int i = 0; i < mineWorkers.Count; i++)
        {
            var radians = 2 * Mathf.PI / mineWorkers.Count * i;
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);
            var spawnDir = new Vector3(horizontal, 0, vertical);

            Vector3 positionInsideCircle = mine.transform.position + spawnDir * 3;
            positionInsideCircle.y = 0;

            mineWorkers[i].GetComponent<FriendManager>().mineDestination = positionInsideCircle;
            mineWorkers[i].GetComponent<FriendManager>().workMine = mine;
            mineWorkers[i].GetComponent<FriendManager>().goToWork = true;
            mineWorkers[i].GetComponent<FriendManager>().atWork = false;
        }
        
        worker.tag = "Untagged";
        friendsFormShield.FormShield(worker);
    }
}

