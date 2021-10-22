using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    int damage;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Enemy"))
        {
            other.transform.GetComponentInParent<EnemyThang>().TakeDamage(damage);
        }
        else if (other.transform.GetComponentInParent<ProjectileOne>())
        {
            other.transform.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
            other.transform.GetComponentInParent<Rigidbody>().useGravity = true;
        }
    }
    public void SetProjectileDamage(int damage)
    {
        this.damage = damage;
    }
}
