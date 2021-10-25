using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50;
    [SerializeField] private float currentHealth = 50;
    [SerializeField] private float maxVelocity = 3;
    [SerializeField] private Player player;
    private Rigidbody rb = null;
    private Vector3 forceDir;

    public Rigidbody Rb 
    {
        get
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
            return rb;
        }
        set => rb = value; 
    }

    public Player Player { get => player; set => player = value; }

    void Update()
    {
        forceDir = (Player.transform.position - transform.position).normalized;
        this.Rb.AddForce(forceDir, ForceMode.Force);
        if (Rb.velocity.magnitude > maxVelocity)
        {
            Rb.velocity = Rb.velocity.normalized * maxVelocity;
        }
    }

    public void ReceiveDamages(float damages)
    {
        currentHealth -= damages;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            Player.ReceiveDamages(1);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Spawner")
        {
            collision.collider.GetComponent<Spawner>().IsUse = false;
        }
    }
}