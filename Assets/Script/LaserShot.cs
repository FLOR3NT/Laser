using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShot : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private float damage = 20;
    [SerializeField] private Player player;

    public float Speed { get => speed; set => speed = value; }
    public float Damage { get => damage; set => damage = value; }
    public Player Player { get => player; set => player = value; }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, player.transform.position) > 30)
        {
            player.EndLaser(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hittable")
        {
            other.GetComponent<CubeManager>().ReceiveDamages(damage);
            player.EndLaser(this);
        }
    }   
}