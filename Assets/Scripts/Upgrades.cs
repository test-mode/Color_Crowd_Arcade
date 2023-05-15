using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Upgrades : MonoBehaviour
{
    // Settings

    // Connections
    [Header("Connections")]
    public GameObject player;
    public LevelData levelData;

    public FireRateUpgrade fireRateUpgrade;
    public FireAngleUpgrade fireAngleUpgrade;
    public CrowdCapacityUpgrade crowdCapacityUpgrade;

    public GameObject fireRateUpgradable;
    public GameObject fireAngleUpgradable;
    public GameObject crowdCapacityUpgradable;

    // Blend Shape
    public Image coneImage;

    // State Variables
    [Header("Variables")]

    int fireRateMaxIncrementCount;
    int fireAngleMaxIncrementCount;
    int crowdCapacityMaxIncrementCount;

    int fireRateIncrementCount;
    int fireAngleIncrementCount;
    int crowdCapacityIncrementCount;

    [HideInInspector] public float fireRateUpgradePrice;
    [HideInInspector] public float fireRate;

    [HideInInspector] public float fireAngleUpgradePrice;
    [HideInInspector] public float fireAngle;

    [HideInInspector] public float crowdCapacityUpgradePrice;
    [HideInInspector] public int crowdCapacity;

    [HideInInspector] public bool upgradePurchased;

    [HideInInspector] public float minPrice;
    List<float> upgradePrices = new List<float>();

    void Awake()
    {

    }
    void Start()
    {
        InitConnections();
        InitState();
    }

    void InitConnections()
    {

    }
    void InitState()
    {
        fireRateIncrementCount = 0;
        fireAngleIncrementCount = 0;
        crowdCapacityIncrementCount = 0;

        fireRate = levelData.fireRate[fireRateIncrementCount];
        fireAngle = levelData.fireAngle[fireAngleIncrementCount];
        coneImage.fillAmount += fireAngle / 360;
        crowdCapacity = levelData.crowdCapacity[crowdCapacityIncrementCount];

        fireRateUpgradePrice = levelData.fireRatePrice[fireRateIncrementCount];
        fireAngleUpgradePrice = levelData.fireAnglePrice[fireRateIncrementCount];
        crowdCapacityUpgradePrice = levelData.crowdCapacityPrice[crowdCapacityIncrementCount];

        upgradePrices.Add(fireRateUpgradePrice);
        upgradePrices.Add(fireAngleUpgradePrice);
        upgradePrices.Add(crowdCapacityUpgradePrice);

        fireRateMaxIncrementCount = levelData.fireRate.Count;
        fireAngleMaxIncrementCount = levelData.fireAngle.Count;
        crowdCapacityMaxIncrementCount = levelData.crowdCapacity.Count;

        player.GetComponentInChildren<BulletSpawner>().fireRate = levelData.fireRate[fireRateIncrementCount];

        upgradePurchased = false;
    }

    void Update()
    {
        minPrice = upgradePrices.Min();

        if (fireRateIncrementCount + 1 == levelData.fireRate.Count)
        {
            fireRateUpgradable.gameObject.SetActive(false);
            fireRateUpgrade.canUpgrade = false;
        }
        if (fireAngleIncrementCount == 7)
        {
            fireAngleUpgradable.gameObject.SetActive(false);
            fireAngleUpgrade.canUpgrade = false;
        }
        if (crowdCapacityIncrementCount + 1 == levelData.crowdCapacity.Count)
        {
            crowdCapacityUpgradable.gameObject.SetActive(false);
            crowdCapacityUpgrade.canUpgrade = false;
        }

        fireRateUpgradePrice = levelData.fireRatePrice[fireRateIncrementCount];
        fireAngleUpgradePrice = levelData.fireAnglePrice[fireAngleIncrementCount];
        crowdCapacityUpgradePrice = levelData.crowdCapacityPrice[crowdCapacityIncrementCount];
    }

    public void FireRateUpgrade()
    {
        if (gameObject.GetComponent<GameManager>().playerTotalMoney >= fireRateUpgradePrice)
        {
            fireRateIncrementCount++;
            upgradePrices.Add(fireRateUpgradePrice);
            gameObject.GetComponent<GameManager>().playerTotalMoney -= fireRateUpgradePrice;
            player.GetComponentInChildren<BulletSpawner>().fireRate = levelData.fireRate[fireRateIncrementCount];

            upgradePurchased = true;

            EventManager.OnUpgradeExitEvent();
        }
    }

    public void FireAngleUpgrade()
    {
        if (gameObject.GetComponent<GameManager>().playerTotalMoney >= fireAngleUpgradePrice)
        {
            fireAngleIncrementCount++;
            upgradePrices.Add(fireAngleUpgradePrice);
            gameObject.GetComponent<GameManager>().playerTotalMoney -= fireAngleUpgradePrice;
            fireAngle = levelData.fireAngle[fireAngleIncrementCount];
            coneImage.fillAmount += fireAngle /360;

            upgradePurchased = true;
            

            EventManager.OnUpgradeExitEvent();
        }
    }

    public void CrowdCapacityUpgrade()
    {
        if (gameObject.GetComponent<GameManager>().playerTotalMoney >= crowdCapacityUpgradePrice)
        {
            crowdCapacityIncrementCount++;
            upgradePrices.Add(crowdCapacityUpgradePrice);
            gameObject.GetComponent<GameManager>().playerTotalMoney -= crowdCapacityUpgradePrice;
            crowdCapacity = levelData.crowdCapacity[crowdCapacityIncrementCount];

            upgradePurchased = true;
            

            EventManager.OnUpgradeExitEvent();
        }
    }
}

