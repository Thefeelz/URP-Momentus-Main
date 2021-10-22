using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class PlayerAttack : MonoBehaviour
{
    CharacterStats playerStats;
    Rigidbody myRb;
    Collider weaponArea;
    [SerializeField] Image targetCrosshair;
    [SerializeField] Transform weaponRaycastTransformPosition;
    [SerializeField] Animator playerAnimator;
    [SerializeField] float dashMaxDistance;
    [SerializeField] float dashMinDistance;
    Volume ppFX;
    RaycastHit hitTarget;
    bool hitCast;
    bool dashing;
    [SerializeField] float dashTime;
    float elaspedTime = 0f;
    Color crosshairColor;
    Vector3 endingDashPosition;
    Vector3 startingDashPosition;


    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponentInParent<CharacterStats>();
        myRb = GetComponent<Rigidbody>();
        crosshairColor = targetCrosshair.color;
        ppFX = FindObjectOfType<Volume>();
    }

    private void Update()
    {
        GetPlayerInput();
        CheckEnemyInRange();
        if (dashing)
            DashToEnemy();
       
    }

    private void GetPlayerInput()
    {
        if (Input.GetMouseButtonDown(0) && !playerAnimator.GetBool("swordSwing"))
        {
            StartCoroutine(WeaponSwing());

            if (Physics.Raycast(Camera.main.transform.position, transform.forward * 10, out hitTarget))
            {
                if (hitTarget.transform.CompareTag("Enemy") && Vector3.Distance(transform.position, hitTarget.transform.position) < dashMaxDistance && Vector3.Distance(transform.position, hitTarget.transform.position) > dashMinDistance)
                {
                    endingDashPosition = hitTarget.transform.position;
                    startingDashPosition = transform.position;
                    dashing = true;
                    DashToEnemy();
                    // hitTarget.transform.GetComponentInParent<EnemyThang>().TakeDamage(playerStats.GetPlayerStrength());
                }
            }
        }
    }
    void DashToEnemy()
    {
        if (elaspedTime <= dashTime)
        {
            elaspedTime += Time.deltaTime;
            myRb.MovePosition(Vector3.Lerp(startingDashPosition, endingDashPosition, elaspedTime / dashTime));
            ppFX.weight = Mathf.Lerp(0, 1f, elaspedTime / dashTime);
        }
        else
        {
            elaspedTime = 0;
            dashing = false;
            ppFX.weight = 0;
        }
    }

    void CheckEnemyInRange()
    {
        // Debug.DrawRay(Camera.main.transform.position, transform.forward * 10, Color.red, 2f);
        hitCast = Physics.Raycast(Camera.main.transform.position, transform.forward * 10, out hitTarget); //Physics.BoxCast(weaponRaycastTransformPosition.position, new Vector3(0.5f, 0.5f, 0.5f), weaponRaycastTransformPosition.rotation * Vector3.forward, out hitTarget, Quaternion.identity, 2f);
        if (hitCast)
        {
            if (hitTarget.transform.CompareTag("Enemy") && Vector3.Distance(transform.position, hitTarget.transform.position) < dashMaxDistance && Vector3.Distance(transform.position, hitTarget.transform.position) > dashMinDistance)
            {
                targetCrosshair.color = Color.red;
            }
            else
            {
                targetCrosshair.color = crosshairColor;
            }
        }
        else
        {
            targetCrosshair.color = crosshairColor;
        }
    }
    IEnumerator WeaponSwing()
    {
        playerAnimator.SetBool("swordSwing", true);
        yield return new WaitForSeconds(.1f);
        playerAnimator.SetBool("swordSwing", false);
        StopCoroutine(WeaponSwing());
    }
    public void SetSwordSwingComplete()
    {
        playerAnimator.SetBool("swordSwing", false);
    }
    public void SetStartSuperSlash()
    {
        playerAnimator.SetBool("superSlash", true);
    }
    public void SetSuperSlashComplete()
    {
        playerAnimator.SetBool("superSlash", false);
    }
    public bool GetSuperSlashStatus()
    {
        return playerAnimator.GetBool("superSlash");
    }
}
