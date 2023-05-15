using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrowdCapacityUpgrade : MonoBehaviour
{
    public GameObject player;
    public GameObject gameManager;
    public LevelData levelData;
    public TextMeshProUGUI textPro;

    public bool canUpgrade;

    void Start()
    {
        canUpgrade = true;
    }

    void Update()
    {
        textPro.text = gameManager.GetComponent<Upgrades>().crowdCapacityUpgradePrice.ToString();
    }

    public void Clicked()
    {
        if (canUpgrade)
        {
            gameManager.GetComponent<Upgrades>().CrowdCapacityUpgrade();
        }
        
    }


}
