using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float lastShot = -1;
    

    [SerializeField] private float currentHealth = 1;
    [SerializeField] private float baseAttackSpeed = 1F;
    private float boostAttackSpeed = 0F;
    private float currentAttackSpeed = 2F;
    [SerializeField] private float baseDamage = 20F;
    private float boostDamage = 0F;
    private float currentDamage = 20F;
    [SerializeField] private Transform cylinder;
    [SerializeField] private DataManager dataManager = default;
    [SerializeField] private GameManager gameManager = default;
    private List<LaserShot> usedLaserShots = new List<LaserShot>();
    [SerializeField] private Transform usedLaserShotParent;  
    private List<LaserShot> unusedLaserShots = new List<LaserShot>();
    [SerializeField] private Transform unusedLaserShotParent;
    [SerializeField] private LaserShot laserPrefab;

    [SerializeField] private Text powerText;
    [SerializeField] private Text speedText;

    private void Update()
    {
        if (!gameManager.IsPlaying)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.y = 1;
            transform.LookAt(mouseWorldPos);

            if (Time.time - lastShot > 1/currentAttackSpeed)
            {
                LaserShot(mouseWorldPos);
                lastShot = Time.time;
            }
        }
    }

    public void Initialize()
    {
        currentHealth = 1;
        boostAttackSpeed = 0F;
        boostDamage = 0F;
        currentAttackSpeed = baseAttackSpeed + baseAttackSpeed * dataManager.BonusAttackSpeed / 10;
        currentDamage = baseDamage + baseDamage * dataManager.BonusDamage / 10;
        powerText.text = "Power level : " + boostDamage;
        speedText.text = "Speed level : " + boostAttackSpeed;
    }

    public void Stop()
    {
        for (int id = 0; id > usedLaserShots.Count; id++)
        {
            EndLaser(usedLaserShots[id]);
            id--;
        }
    }

    public void ReceiveDamages(float damages)
    {
        if (!gameManager.IsPlaying)
        {
            return;
        }
        currentHealth -= damages;
        if (currentHealth <= 0)
        {
            gameManager.Lose();
        }
    }

    private void LaserShot(Vector3 mouseWorldPos)
    {

        Vector3 startPos = cylinder.position;
        startPos.y = 0.5F;
        mouseWorldPos.y = startPos.y;
        Vector3 laserRay = (mouseWorldPos - startPos).normalized;

        LaserShot newLaserShot;
        if (unusedLaserShots.Count > 0)
        {
            newLaserShot = unusedLaserShots[0];
            unusedLaserShots.Remove(newLaserShot);
            newLaserShot.transform.parent = usedLaserShotParent;
            newLaserShot.gameObject.SetActive(true);
        }
        else
        {
            newLaserShot = Instantiate(laserPrefab, usedLaserShotParent);
            newLaserShot.Player = this;
        }

        usedLaserShots.Add(newLaserShot);
        newLaserShot.Damage = currentDamage;
        newLaserShot.transform.position = cylinder.position;
        newLaserShot.transform.forward = laserRay;
    }  

    public void EndLaser(LaserShot laserShot)
    {
        usedLaserShots.Remove(laserShot);
        unusedLaserShots.Add(laserShot);
        laserShot.transform.parent = unusedLaserShotParent;
        laserShot.gameObject.SetActive(false);
    }

    public void AddBoost(Boost boost)
    {
        switch (boost)
        {
            case Boost.Damage:
                boostDamage++;
                currentDamage = baseDamage + baseDamage * boostDamage * 0.25F;
                currentDamage += currentDamage * dataManager.BonusDamage / 10;
                powerText.text = "Power level : " + boostDamage;
                break;

            case Boost.AttackSpeed:
                boostAttackSpeed++;
                currentAttackSpeed = baseAttackSpeed + baseAttackSpeed * boostAttackSpeed * 0.25F;
                currentAttackSpeed += currentAttackSpeed * dataManager.BonusAttackSpeed / 10;
                speedText.text = "Speed level : " + boostAttackSpeed;
                break;

            case Boost.None:
                break;
        }
    }
}