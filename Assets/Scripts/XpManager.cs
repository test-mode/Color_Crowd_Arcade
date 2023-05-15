using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpManager : MonoBehaviour
{
    // Settings

    // Connections
    public GameObject player;
    public GameObject upgradeButton;
    public Slider xpSlider;

    // State Variables
    [HideInInspector] public bool XpFull = false;
    [HideInInspector] public float playerTotalXp = 0;


    void InitConnections()
    {

    }
    void InitState()
    {

    }

    void Start()
    {
        //InitConnections();
        //InitState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerTotalXp = 100;
            XpFull = true;
        }

        xpSlider.value = playerTotalXp;

        if (playerTotalXp == 100)
        {
            XpFull = true;
        }
        else
        {
            XpFull = false;
        }
    }
}

