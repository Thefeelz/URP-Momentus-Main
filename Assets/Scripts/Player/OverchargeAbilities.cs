using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverchargeAbilities : MonoBehaviour
{
    Rigidbody myRigidbody;
    [SerializeField] int airDashPower;
    [SerializeField] int airDashCost;

    [SerializeField] int superSlashDamage;
    [SerializeField] int superSlashCost;
    [SerializeField] int superSlashSpeed;
    [SerializeField] GameObject superSlashObject;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();   
    }

    public void OverchargeAbility_AirDash()
    {
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.AddRelativeForce(new Vector3(0, 5, airDashPower), ForceMode.VelocityChange);
    }
    public int Get_OverchargeAbility_AirDash_Cost()
    {
        return airDashCost;
    }
    public void OverchargeAbility_SuperSlash()
    {
        GameObject swordSlash = Instantiate(superSlashObject, transform.position, transform.rotation);
        swordSlash.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * superSlashSpeed, ForceMode.VelocityChange);
        swordSlash.GetComponent<PlayerProjectile>().SetProjectileDamage(superSlashDamage);
        Destroy(swordSlash, 5f);
    }
    public int Get_OverchargeAbility_SuperSlash_Cost()
    {
        return superSlashCost;
    }
}
