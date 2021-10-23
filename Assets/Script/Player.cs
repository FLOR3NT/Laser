using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float lastShot = -1;

    [SerializeField] private float attackSpeed = 1F;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform cylinder;
    [SerializeField] private Color laserOnCColor;
    [SerializeField] private Color laserOffCColor;


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


    private IEnumerator LaserShot(Vector3 mouseWorldPos)
    {
        yield return 0;

        lineRenderer.SetPosition(0, cylinder.position);
        mouseWorldPos.y = cylinder.position.y;
        Vector3 laserRay = (mouseWorldPos - cylinder.position).normalized;
        lineRenderer.SetPosition(1, cylinder.position+ laserRay*30);
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