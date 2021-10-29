using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject headToRotate;
    [SerializeField] float detectionRange;
    [SerializeField] float rotateSpeed;
    [SerializeField] float sleepTimer;
    [SerializeField] float chargeUpTime;
    [SerializeField] float fireBurstTime;
    [SerializeField] Light firingLight;
    [SerializeField] ParticleSystem bulletsToFire;

    enum TurretState  {Waiting, Attacking, Asleep};
    TurretState state;

    P_Input myPlayer;
    Animator anim;

    float elapsedChargeTime = 0f;
    float elapsedFireTime = 0f;
    bool charged = false;
    bool firing = false;
    // Start is called before the first frame update
    void Start()
    {
        state = TurretState.Asleep;
        myPlayer = FindObjectOfType<P_Input>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == TurretState.Asleep)
        {
            CheckPlayerInRange();
        }
        else if (state == TurretState.Attacking)
        {
            CheckPlayerInRange();
            RotateTowardsPlayer();
        }
    }

    void CheckPlayerInRange()
    {
        if(Vector3.Distance(transform.position, myPlayer.transform.position) < detectionRange)
        {
            StopCoroutine(TurretToSleep(sleepTimer));
            if(state == TurretState.Asleep)
                StartCoroutine(WakeUpTurret());
        }
        else if (state == TurretState.Attacking)
        {
            StartCoroutine(TurretToSleep(sleepTimer));
        }
    }

    void RotateTowardsPlayer()
    {
        headToRotate.transform.rotation = Quaternion.RotateTowards(headToRotate.transform.rotation, 
                                                                   Quaternion.LookRotation(myPlayer.transform.position + myPlayer.transform.up - headToRotate.transform.position),
                                                                   Time.deltaTime * rotateSpeed);
        //if(Vector3.Angle(headToRotate.transform.forward, myPlayer.transform.position) < 15f || Vector3.Angle(headToRotate.transform.forward, myPlayer.transform.position) > -15f)
        //{
            if(charged && !firing)
                StartCoroutine(StartFiring());
            else
                StartCharging();
        //}
    }

    void StartCharging()
    {
        elapsedChargeTime += Time.deltaTime;
        firingLight.intensity = Mathf.Lerp(0, 2, elapsedChargeTime / chargeUpTime);
        if(elapsedChargeTime >= chargeUpTime)
        {
            elapsedChargeTime = 0;
            charged = true;
        }
    }

    IEnumerator WakeUpTurret()
    {
        anim.SetBool("awake", true);

        yield return new WaitForSeconds(1f);

        state = TurretState.Attacking;
        anim.SetBool("awake", false);
        anim.enabled = false;
    }
    IEnumerator TurretToSleep(float sleepTime)
    {
        Debug.Log("Turret to sleep");
        state = TurretState.Waiting;
        yield return new WaitForSeconds(sleepTime);
        state = TurretState.Asleep;
        anim.enabled = true;
        anim.SetBool("asleep", true);
        yield return new WaitForSeconds(.2f);
        anim.SetBool("asleep", false);
        firingLight.intensity = 0;
    }
    IEnumerator StartFiring()
    {
        firing = true;
        bulletsToFire.Play();
        while(elapsedFireTime <= fireBurstTime)
        {
            elapsedFireTime+=Time.deltaTime;
            yield return null;
        }
        charged = false;
        firing = false;
        bulletsToFire.Stop();
        firingLight.intensity = 0;
        elapsedFireTime = 0;
    }
}
