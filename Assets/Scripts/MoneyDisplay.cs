using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    public TextMeshProUGUI TextPro;
    public GameObject manager;
    private GameManager money;

    void Start()
    {
        money = manager.GetComponent<GameManager>();
    }


    void Update()
    {
        TextPro.text = money.playerTotalMoney.ToString();
    }
}
