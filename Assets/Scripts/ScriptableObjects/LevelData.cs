using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New LevelData", menuName = "Level Data", order = 51)]

public class LevelData : ScriptableObject
{
    [Header("Whole numbers (ex. 2)")]
    public List<int> crowdCapacity = new List<int>();
    public List<int> crowdCapacityPrice = new List<int>();
    
    [Header("Float numbers (ex. 0.2)")]
    public List<float> fireRate = new List<float>();
    public List<float> fireRatePrice = new List<float>();

    [Header("Float numbers (ex. 0.2)")]
    public List<float> fireAngle = new List<float>();
    public List<float> fireAnglePrice = new List<float>();

    [Header("Enemies Count Per Level")]
    public List<int> enemyCount = new List<int>();

    [Header("Money")]
    public int moneyValue;
}

