using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileOne : MonoBehaviour
{
    [SerializeField] float projectileSpeed;
    CharacterStats ourPlayer;
    Vector3 fireDirection;
    Rigidbody rb;
    int damage;

    void Start()
    {
        ourPlayer = FindObjectOfType<CharacterStats>();
        rb = GetComponent<Rigidbody>();
        fireDirection = (ourPlayer.transform.position - transform.position).normalized * projectileSpeed;
        rb.velocity = fireDirection;
        //Vector3.MoveTowards(transform.position, ourPlayer.transform.position, 100f);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, ourPlayer.transform.rotation, 360f);
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            ourPlayer.RemoveHealth(damage);
            Destroy(gameObject);
        }
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}
