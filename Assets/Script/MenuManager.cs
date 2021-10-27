using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private DataManager dataManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject menuGO;

    [SerializeField] private Text maxScoreText;
    [SerializeField] private Text pointText;

    [SerializeField] private Text speedFireText;
    [SerializeField] private Text damageText;
    [SerializeField] private Text scoringText;
    [SerializeField] private Text passiveScoringText;

    [SerializeField] private Text speedFireButtonText;
    [SerializeField] private Text damageButtonText;
    [SerializeField] private Text scoringButtonText;
    [SerializeField] private Text passiveScoringButtonText;

    [SerializeField] private Button speedFireButton;
    [SerializeField] private Button damageButton;
    [SerializeField] private Button scoringButton;
    [SerializeField] private Button passiveScoringButton;

    private string speedFireString = "Speed fire level : ";
    private string damageString = "Damage level : ";
    private string scoringString = "Scoring level : ";
    private string passiveScoringString = "Passive scoring level : ";

    private void Start()
    {
        OpenMenu();
    }

    public void PlayButton()
    {
        gameManager.Initialize();
        menuGO.SetActive(false);
    }

    public void OpenMenu()
    {
        menuGO.SetActive(true);
        maxScoreText.text = dataManager.MaxScore.ToString();
        pointText.text = (dataManager.UsedPoint - dataManager.TotalPoint).ToString();

        speedFireText.text = speedFireString + dataManager.BonusAttackSpeed;
        damageText.text = damageString + dataManager.BonusDamage;
        scoringText.text = scoringString + dataManager.BonusScoring;
        passiveScoringText.text = passiveScoringString + dataManager.BonusPassiveScoring;

        speedFireButtonText.text = GetCost(dataManager.BonusAttackSpeed).ToString();
        damageButtonText.text = GetCost(dataManager.BonusDamage).ToString();
        scoringButtonText.text = GetCost(dataManager.BonusScoring).ToString();
        passiveScoringButtonText.text = GetCost(dataManager.BonusPassiveScoring).ToString();

        CheckBonusInteractible();
    }

    public float GetCost(float bonusLevel)
    {
        float value = bonusLevel * 2;
        return value;
    }

    public void ResetAllLevelsButton()
    {
        dataManager.BonusAttackSpeed = 0;
        dataManager.BonusDamage = 0;
        dataManager.BonusScoring = 0;
        dataManager.BonusPassiveScoring = 0;
        dataManager.UsedPoint = 0;
        OpenMenu();
    }

    public void SpeedFireButton()
    {
        dataManager.UsedPoint -= dataManager.BonusAttackSpeed * 2;
        dataManager.BonusAttackSpeed++;
        speedFireButtonText.text = (dataManager.BonusAttackSpeed * 2).ToString();
        speedFireText.text = speedFireString + dataManager.BonusAttackSpeed;
        pointText.text = (dataManager.UsedPoint - dataManager.TotalPoint).ToString();
        CheckBonusInteractible();
    }

    public void DamageButton()
    {
        dataManager.UsedPoint -= dataManager.BonusDamage * 2;
        dataManager.BonusDamage++;
        damageButtonText.text = (dataManager.BonusDamage * 2).ToString();
        damageText.text = damageString + dataManager.BonusDamage;
        pointText.text = (dataManager.UsedPoint - dataManager.TotalPoint).ToString();
        CheckBonusInteractible();
    }

    public void ScoringButton()
    {
        dataManager.UsedPoint -= dataManager.BonusScoring * 2;
        dataManager.BonusScoring++;
        scoringButtonText.text = (dataManager.BonusScoring * 2).ToString();
        scoringText.text = scoringString + dataManager.BonusScoring;
        pointText.text = (dataManager.UsedPoint - dataManager.TotalPoint).ToString();
        CheckBonusInteractible();
    }

    public void PassiveScoringButton()
    {
        dataManager.UsedPoint -= dataManager.BonusPassiveScoring * 2;
        dataManager.BonusPassiveScoring++;
        passiveScoringButtonText.text = (dataManager.BonusPassiveScoring * 2).ToString();
        passiveScoringText.text = passiveScoringString + dataManager.BonusPassiveScoring;
        pointText.text = (dataManager.UsedPoint - dataManager.TotalPoint).ToString();
        CheckBonusInteractible();
    }

    public void CheckBonusInteractible()
    {
        speedFireButton.interactable = GetCost(dataManager.BonusAttackSpeed) <= dataManager.UsedPoint - dataManager.TotalPoint;
        damageButton.interactable = GetCost(dataManager.BonusDamage) <= dataManager.UsedPoint - dataManager.TotalPoint;
        scoringButton.interactable = GetCost(dataManager.BonusScoring) <= dataManager.UsedPoint - dataManager.TotalPoint;
        passiveScoringButton.interactable = GetCost(dataManager.BonusPassiveScoring) <= dataManager.UsedPoint - dataManager.TotalPoint;
    }
}