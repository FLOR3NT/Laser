using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private DataManager dataManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject menuGO;
    [SerializeField] private GameObject popupGO;

    [SerializeField] private Text maxScoreText;
    [SerializeField] private Text remaininPointText;
    [SerializeField] private Text addedPointText;

    [SerializeField] private Text speedFireText;
    [SerializeField] private Text damageText;
    [SerializeField] private Text pointText;
    [SerializeField] private Text passivePointText;

    [SerializeField] private Text speedFireButtonText;
    [SerializeField] private Text damageButtonText;
    [SerializeField] private Text pointButtonText;
    [SerializeField] private Text passivePointButtonText;

    [SerializeField] private Button speedFireButton;
    [SerializeField] private Button damageButton;
    [SerializeField] private Button pointButton;
    [SerializeField] private Button passivePointButton;

    private string speedFireString = "Speed fire level : ";
    private string damageString = "Damage level : ";
    private string pointString = "Point level : ";
    private string passivePointString = "Passive point level : ";

    private void Start()
    {
        OpenPopup();
        OpenMenu();
    }

    public void OpenPopup()
    {
        if (dataManager.LastGame == "" || dataManager.BonusPassivePoint == 0)
        {
            popupGO.SetActive(false);
        }
        else
        {
            
            float newPoint = Mathf.Round(Mathf.Log((float)(DateTime.Now - DateTime.Parse(dataManager.LastGame)).TotalMinutes * Mathf.Exp(dataManager.BonusPassivePoint)));
            addedPointText.text = newPoint.ToString();
            dataManager.TotalPoint += newPoint;
            popupGO.SetActive(true);
        }
    }

    public void ClosePopup()
    {
        popupGO.SetActive(false);
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
        remaininPointText.text = (dataManager.TotalPoint - dataManager.UsedPoint).ToString();

        speedFireText.text = speedFireString + dataManager.BonusAttackSpeed;
        damageText.text = damageString + dataManager.BonusDamage;
        pointText.text = pointString + dataManager.BonusPoint;
        passivePointText.text = passivePointString + dataManager.BonusPassivePoint;

        speedFireButtonText.text = GetCost(dataManager.BonusAttackSpeed).ToString();
        damageButtonText.text = GetCost(dataManager.BonusDamage).ToString();
        pointButtonText.text = GetCost(dataManager.BonusPoint).ToString();
        passivePointButtonText.text = GetCost(dataManager.BonusPassivePoint).ToString();

        CheckBonusInteractible();
        dataManager.LastGame = DateTime.Now.ToString();
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
        dataManager.BonusPoint = 0;
        dataManager.BonusPassivePoint = 0;
        dataManager.UsedPoint = 0;
        OpenMenu();
    }

    public void SpeedFireButton()
    {
        dataManager.UsedPoint += dataManager.BonusAttackSpeed * 2;
        dataManager.BonusAttackSpeed++;
        speedFireButtonText.text = (dataManager.BonusAttackSpeed * 2).ToString();
        speedFireText.text = speedFireString + dataManager.BonusAttackSpeed;
        remaininPointText.text = (dataManager.TotalPoint - dataManager.UsedPoint).ToString();
        CheckBonusInteractible();
    }

    public void DamageButton()
    {
        dataManager.UsedPoint += dataManager.BonusDamage * 2;
        dataManager.BonusDamage++;
        damageButtonText.text = (dataManager.BonusDamage * 2).ToString();
        damageText.text = damageString + dataManager.BonusDamage;
        remaininPointText.text = (dataManager.TotalPoint - dataManager.UsedPoint).ToString();
        CheckBonusInteractible();
    }

    public void PointButton()
    {
        dataManager.UsedPoint += dataManager.BonusPoint * 2;
        dataManager.BonusPoint++;
        pointButtonText.text = (dataManager.BonusPoint * 2).ToString();
        pointText.text = pointString + dataManager.BonusPoint;
        remaininPointText.text = (dataManager.TotalPoint - dataManager.UsedPoint).ToString();
        CheckBonusInteractible();
    }

    public void PassivePointButton()
    {
        dataManager.UsedPoint += dataManager.BonusPassivePoint * 2;
        dataManager.BonusPassivePoint++;
        passivePointButtonText.text = (dataManager.BonusPassivePoint * 2).ToString();
        passivePointText.text = passivePointString + dataManager.BonusPassivePoint;
        remaininPointText.text = (dataManager.TotalPoint - dataManager.UsedPoint).ToString();
        CheckBonusInteractible();
    }

    public void CheckBonusInteractible()
    {
        speedFireButton.interactable = GetCost(dataManager.BonusAttackSpeed) <= dataManager.TotalPoint - dataManager.UsedPoint;
        damageButton.interactable = GetCost(dataManager.BonusDamage) <= dataManager.TotalPoint - dataManager.UsedPoint;
        pointButton.interactable = GetCost(dataManager.BonusPoint) <= dataManager.TotalPoint - dataManager.UsedPoint;
        passivePointButton.interactable = GetCost(dataManager.BonusPassivePoint) <= dataManager.TotalPoint - dataManager.UsedPoint;
    }
}