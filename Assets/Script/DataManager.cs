using UnityEngine;

[CreateAssetMenu(fileName = "DataManager", menuName = "ScriptableObjects/DataManager", order = 1)]
public class DataManager : ScriptableObject
{
    [SerializeField] private float maxScore = 0;
    [SerializeField] private float bonusAttackSpeed = 0;
    [SerializeField] private float bonusDamage = 0;
    [SerializeField] private float bonusScoring = 0;
    [SerializeField] private float bonusPassiveScoring = 0;
    [SerializeField] private float totalPoint = 0;
    [SerializeField] private float usedPoint = 0;

    public float MaxScore { get => maxScore; set => maxScore = value; }
    public float BonusAttackSpeed { get => bonusAttackSpeed; set => bonusAttackSpeed = value; }
    public float BonusDamage { get => bonusDamage; set => bonusDamage = value; }
    public float BonusScoring { get => bonusScoring; set => bonusScoring = value; }
    public float BonusPassiveScoring { get => bonusPassiveScoring; set => bonusPassiveScoring = value; }
    public float TotalPoint { get => totalPoint; set => totalPoint = value; }
    public float UsedPoint { get => usedPoint; set => usedPoint = value; }
}