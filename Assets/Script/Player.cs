using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float lastShot = 0;
    private float attackSpeed = 0.5F;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform Cylinder;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.y = 1;
            transform.LookAt(mouseWorldPos);


        }
    }


    private IEnumerator LaserShot()
    {
        yield return 0;

        lineRenderer.SetPositions
    }
}