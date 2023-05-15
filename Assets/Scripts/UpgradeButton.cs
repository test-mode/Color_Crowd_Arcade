using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradeButton : MonoBehaviour
{
    // Settings

    // Connections
    public GameObject gameManager;
    public GameObject upgradeButtonBackground;

    // State Variables
    int minUpgradePrice;

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

    }

    public void Clicked()
    {
        EventManager.OnUpgradeEvent();
    }

    public void BackgroundClicked()
    {
        EventManager.OnUpgradeExitEvent();
    }
}

