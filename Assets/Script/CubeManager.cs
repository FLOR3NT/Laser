using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    private float maxHealth = 50;
    private float currentHealth = 50;
    private float maxVelocity = 3;
    private float addedScore = 1;
    [SerializeField] private Material material;
    [SerializeField] private Color baseColor;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Boost boost = Boost.None;
    [SerializeField] private Spawner currentSpawner;

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

    public Material Material
    {
        get
        {
            if (material == null)
            {
                material = GetComponent<MeshRenderer>().material;
            }
            return material;
        }
        set => material = value;
    }

    public Spawner CurrentSpawner { get => currentSpawner; set => currentSpawner = value; }
    public Boost Boost { get => boost; set => boost = value; }
    public GameManager GameManager { get => gameManager; set => gameManager = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public float MaxVelocity { get => maxVelocity; set => maxVelocity = value; }
    public float AddedScore { get => addedScore; set => addedScore = value; }

    void Update()
    {
        if (!gameManager.IsPlaying)
        {
            return;
        }

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
        Material.color = Color.Lerp(baseColor, Color.black, (maxHealth - currentHealth) / maxHealth);
        if (currentHealth <= 0)
        {
            if (currentSpawner != null)
            {
                currentSpawner.IsUse = false;
            }
            gameManager.Score += addedScore;
            gameManager.Player.AddBoost(boost);            
            gameManager.DisableCube(this);
        }
    }

    public void SetColor(Color color)
    {
        Material.color = baseColor = color;
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
            currentSpawner.IsUse = false;
        }
    }
}

public enum Boost
{
    None,
    AttackSpeed,
    Damage
} 