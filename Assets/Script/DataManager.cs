using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DataManager", menuName = "ScriptableObjects/DataManager", order = 1)]
public class DataManager : ScriptableObject
{
    [SerializeField] private float maxScore = 0;
    [SerializeField] private float bonusAttackSpeed = 0;
    [SerializeField] private float bonusDamage = 0;
    [SerializeField] private float bonusPoint = 0;
    [SerializeField] private float bonusPassivePoint = 0;
    [SerializeField] private float totalPoint = 0;
    [SerializeField] private float usedPoint = 0;
    [SerializeField] private string lastGame = "";

    public float MaxScore { get => maxScore; set => maxScore = value; }
    public float BonusAttackSpeed { get => bonusAttackSpeed; set => bonusAttackSpeed = value; }
    public float BonusDamage { get => bonusDamage; set => bonusDamage = value; }
    public float BonusPoint { get => bonusPoint; set => bonusPoint = value; }
    public float BonusPassivePoint { get => bonusPassivePoint; set => bonusPassivePoint = value; }
    public float TotalPoint { get => totalPoint; set => totalPoint = value; }
    public float UsedPoint { get => usedPoint; set => usedPoint = value; }
    public string LastGame { get => lastGame; set => lastGame = value; }
}