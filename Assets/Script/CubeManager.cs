using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50;
    [SerializeField] private float currentHealth = 50;
    [SerializeField] private float maxVelocity = 3;
    [SerializeField] private float addedScore = 50;
    [SerializeField] private GameManager gameManager;
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

    public GameManager GameManager { get => gameManager; set => gameManager = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public float MaxVelocity { get => maxVelocity; set => maxVelocity = value; }
    public float AddedScore { get => addedScore; set => addedScore = value; }

    void Update()
    {
        forceDir = (GameManager.Player.transform.position - transform.position).normalized;
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
            gameManager.Score += addedScore;
            gameManager.DisableCube(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            GameManager.Player.ReceiveDamages(1);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Spawner")
        {
            collision.collider.GetComponent<Spawner>().IsUse = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Spawner")
        {
            other.GetComponent<Spawner>().IsUse = false;
        }
    }
}