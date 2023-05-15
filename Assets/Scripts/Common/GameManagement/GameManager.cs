using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Settings
    public bool spawnLevel = true;
    public int tutorialLevels;

    // Connections
    public GameObject[] levels;
    public UIManager ui;
    public GameObject[] upgradeButtons;
    public GameObject backgroundDarkener;
    public GameObject upgradeButton;
    public GameObject player;
    public GameObject safeZone;
    public TextMeshProUGUI levelCountText;

    GameObject currentLevelEnvironment;
    int currentLevelEnvironmentIndex;

    EnemySpawner enemySpawnerScript;


    // State variables
    [HideInInspector] public int currentLevel = 0;
    int score;


    [HideInInspector] public float playerTotalMoney = 0;
    [HideInInspector] public int playerLevel = 0;
    [HideInInspector] public bool gameOver = false;
    [HideInInspector] public bool gameStarted = false;
    [HideInInspector] public bool playerLevelPassed = false;



    private void Awake()
    {
        InitStates();
        InitConnections();
    }

    void InitStates()
    {
        //currentLevel = PlayerPrefs.GetInt("Level", 0);

        currentLevelEnvironmentIndex = 0;
        LoadLevel();
    }

    void LoadLevel()
    {
        if (spawnLevel)
        {
            
            foreach (var level in levels)
            {
                level.SetActive(false);
            }

            int prefabIndex = GetPrefabIndex(currentLevel, tutorialLevels, levels.Length);
            //currentLevelEnvironment = Instantiate(levels[currentLevelEnvironmentIndex], Vector3.zero, Quaternion.identity);
            levels[currentLevelEnvironmentIndex].SetActive(true);
            SetUpGame(prefabIndex);
        }
    }

    private void SetUpGame(int prefabIndex)
    {
        player.GetComponent<FriendsFormShield>().prefabIndex = prefabIndex;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerTotalMoney += 99999;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerTotalMoney = 0;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerLevel = 0;
        }

        levelCountText.text = "Level " + playerLevel.ToString();

        if (Input.GetKeyDown(KeyCode.T))
        {
            for (int i = 0; i < 50; i++)
            {
                Debug.Log("Prefab index for level " + i + ":" + GetPrefabIndex(i, tutorialLevels, levels.Length));
            }
        }

        if (player.GetComponent<PlayerController>().playerDied)
        {
            OnLevelFailed();
        }

        if (GetComponent<XpManager>().XpFull)
        {
            PlayerNextLevel();
        }

        if (GetComponent<Upgrades>().upgradePurchased)
        {
            upgradeButton.SetActive(false);
        }
        else
        {
            if (playerLevel > 0)
            {
                upgradeButton.SetActive(true);
            }
            else
            {
                upgradeButton.SetActive(false);
            }
        }
    }

    private void PlayerNextLevel()
    {
        playerLevel++;
        GetComponent<XpManager>().playerTotalXp = 0;
        playerLevelPassed = true;
        GetComponent<Upgrades>().upgradePurchased = false;
        EventManager.OnUpgradeEvent();
    }

    int GetPrefabIndex(int levelIndex, int nInitialLevels, int nLevels)
    {

        int nRepeatingLevels = nLevels - nInitialLevels;
        int prefabIndex = levelIndex;
        if (levelIndex >= nInitialLevels)
        {
            prefabIndex = ((levelIndex - nInitialLevels) % nRepeatingLevels) + nInitialLevels;
        }
        return prefabIndex;

    }

    void InitConnections()
    {
        ui.OnLevelStart += OnLevelStart;
        ui.OnNextLevel += OnNextLevel;
        ui.OnLevelRestart += OnLevelRestart;

        EventManager.OnUpgrade += OnUpgrade;
        EventManager.OnUpgradeExit += OnUpgradeExit;

        enemySpawnerScript = GetComponent<EnemySpawner>();
    }

    void OnLevelFailed()
    {
        ui.FailLevel();
        gameOver = true;
        Debug.Log("LEVEL FAILED");
        
    }

    public void OnFinishLevel()
    {
        ui.FinishLevel();
        //PlayerPrefs.SetInt("Level", currentLevel + 1);
        player.GetComponent<PlayerController>().stopMoving = true;
    }

    void OnLevelStart()
    {
        gameStarted = true;
        Debug.Log("LEVEL STARTED");
    }

    void OnLevelRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameOver = false;
    }

    public void OnNextLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //PlayerPrefs.SetInt("showStart", 0);

        levels[currentLevelEnvironmentIndex].SetActive(false);
        currentLevel++;
        currentLevelEnvironmentIndex++;
        if (currentLevelEnvironmentIndex > levels.Count() - 1)
        {
            currentLevelEnvironmentIndex = 0;
        }
        gameStarted = true;
        levels[currentLevelEnvironmentIndex].SetActive(true);

        player.transform.position = safeZone.transform.position - new Vector3(0, 0, 14);
        player.GetComponent<PlayerController>().stopMoving = false;

        enemySpawnerScript.spawnEnemies = true;
        enemySpawnerScript.spawnedEnemiesCount = 0;
    }



    #region EventAddons

    [Header("Event Variables")]
    [HideInInspector] public bool isSellStarted = false;
    [HideInInspector] public bool startSell = false;
    public GameObject moneyPrefab;

    void OnUpgrade()
    {
        foreach (var button in upgradeButtons)
        {
            if (button != null)
            {
                button.SetActive(true);
                button.GetComponent<DOTweenAnimation>().DOPlay();
                button.transform.DOKill();
                button.transform.DOScale(1f, 1);
            }
        }
        backgroundDarkener.SetActive(true);
    }
    
    void OnUpgradeExit()
    {
        foreach (var button in upgradeButtons)
        {
            if (button != null)
            {
                button.SetActive(false);
                button.transform.DOKill();
                button.transform.DOScale(0, .1f);
            }
        }
        backgroundDarkener.SetActive(false);
    }

    #endregion


}