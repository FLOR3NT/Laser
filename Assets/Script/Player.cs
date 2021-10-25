using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float lastShot = -1;

    [SerializeField] private float currentHealth = 1;
    [SerializeField] private float attackSpeed = 1F;
    [SerializeField] private float damages = 20F;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform cylinder;
    [SerializeField] private Color laserOnCColor;
    [SerializeField] private Color laserOffCColor;
    [SerializeField] private GameManager gameManager = default;

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
                StartCoroutine(LaserShot(mouseWorldPos));
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


    private IEnumerator LaserShot(Vector3 mouseWorldPos)
    {
        yield return 0;

        Vector3 startPos = cylinder.position;
        startPos.y = 0.5F;
        lineRenderer.SetPosition(0, cylinder.position);
        mouseWorldPos.y = startPos.y;
        Vector3 laserRay = (mouseWorldPos - startPos).normalized;

        RaycastHit hit;
        if (Physics.Raycast(startPos, laserRay, out hit, 30, ~LayerMask.NameToLayer("Hittable")))
        {
            Vector3 endPos = hit.point;
            endPos.y = cylinder.position.y;
            lineRenderer.SetPosition(1, endPos);
            hit.collider.GetComponent<CubeManager>().ReceiveDamages(damages);
        }
        else
        {
            lineRenderer.SetPosition(1, cylinder.position + laserRay * 30);
        }

        lineRenderer.material.SetColor("_Color", laserOnCColor);

        float endTime = Time.time + 0.15F;
        while (Time.time < endTime)
        {
            lineRenderer.material.SetColor("_Color", Color.Lerp(laserOnCColor, laserOffCColor, (endTime - Time.time) / 0.1F));
            yield return 0;
        }
        lineRenderer.material.SetColor("_Color", laserOffCColor);

    }
}