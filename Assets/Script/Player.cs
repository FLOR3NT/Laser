using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float lastShot = -1;
    

    [SerializeField] private float currentHealth = 1;
    [SerializeField] private float attackSpeed = 1F;
    [SerializeField] private float damage = 20F;
    [SerializeField] private Transform cylinder;
    [SerializeField] private GameManager gameManager = default;
    private List<LaserShot> usedLaserShot = new List<LaserShot>();
    [SerializeField] private Transform usedLaserShotParent;  
    private List<LaserShot> unusedLaserShot = new List<LaserShot>();
    [SerializeField] private Transform unusedLaserShotParent;
    [SerializeField] private LaserShot laserPrefab;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.y = 1;
            transform.LookAt(mouseWorldPos);

            if (Time.time - lastShot > attackSpeed)
            {
                LaserShot(mouseWorldPos);
                lastShot = Time.time;
            }
        }
    }

    public void ReceiveDamages(float damages)
    {
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
        if (unusedLaserShot.Count > 0)
        {
            newLaserShot = unusedLaserShot[0];
            unusedLaserShot.Remove(newLaserShot);
            newLaserShot.transform.parent = usedLaserShotParent;
            newLaserShot.gameObject.SetActive(true);
        }
        else
        {
            newLaserShot = Instantiate(laserPrefab, usedLaserShotParent);
            newLaserShot.Player = this;
        }

        usedLaserShot.Add(newLaserShot);
        newLaserShot.Damage = damage;
        newLaserShot.transform.position = cylinder.position;
        newLaserShot.transform.forward = laserRay;
    }  

    public void EndLaser(LaserShot laserShot)
    {
        usedLaserShot.Remove(laserShot);
        unusedLaserShot.Add(laserShot);
        laserShot.transform.parent = unusedLaserShotParent;
        laserShot.gameObject.SetActive(false);
    }
}