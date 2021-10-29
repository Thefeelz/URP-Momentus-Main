using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_BladeDance : A_OverchargeAbilities
{
    [Header("Blade Dance")]
    [SerializeField] float dashTime;
    EnemyBad currentTarget;
    bool allEnemiesAttacked = false;
    int killCount;
    [SerializeField] int killCountMax = 7;
    bool bladeDanceReady = true;

    [Header("General Things")]
    [SerializeField] GameObject myStartingPosition;
    [SerializeField] Camera myCam;
    [SerializeField] Transform firstPersonCamPos;
    [SerializeField] Transform specialAbilityCamPos;
    [SerializeField] float camTransitionTime;
    float camElapsedTime = 0f;
    bool camTransitioning;
    bool usingSpecial;

    Vector3 startPos;
    Quaternion startRotation;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (camTransitioning)
            UpdateCameraPos(usingSpecial);
    }

    public bool Ability_BladeDance()
    {
        if (!abilityReady) { return false; }
        gameManager.SetActiveSpecialAbility(true);
        startPos = myStartingPosition.transform.position;
        startRotation = myStartingPosition.transform.rotation;
        allEnemiesAttacked = false;
        bladeDanceReady = false;
        GetComponent<P_Movement>().enabled = false;
        GetComponent<mouseLook>().enabled = false;
        camTransitioning = true;
        usingSpecial = true;
        StartCoroutine(AttackEnemy());
        return true;
    }

    void UpdateCameraPos(bool startingSpecial)
    {
        camElapsedTime += Time.deltaTime;
        if (startingSpecial)
        {
            myCam.transform.position = Vector3.Lerp(firstPersonCamPos.position, specialAbilityCamPos.position, camElapsedTime / camTransitionTime);
        }
        else
        {
            myCam.transform.position = Vector3.Lerp(specialAbilityCamPos.position, firstPersonCamPos.position, camElapsedTime / camTransitionTime);
        }
        if (camTransitionTime <= camElapsedTime)
        {
            camTransitioning = false;
            camElapsedTime = 0;
        }
    }
    EnemyBad LocateClosestEnemy()
    {
        if (killCount >= killCountMax)
        {
            return null;
        }
        EnemyBad closestEnemy = null;
        foreach (var enemy in gameManager.GetActiveEnemies())
        {
            if (!enemy.GetBeenAttacked())
            {
                if (closestEnemy == null)
                {
                    closestEnemy = enemy;
                }
                else if (Vector3.Distance(transform.position, enemy.transform.position) <= Vector3.Distance(transform.position, closestEnemy.transform.position))
                {
                    closestEnemy = enemy;
                }
            }
        }
        if (closestEnemy == null)
        {
            return null;
        }
        else
        {
            return closestEnemy;
        }
    }
    IEnumerator AttackEnemy()
    {
        if (camTransitioning)
            yield return new WaitForSeconds(camTransitionTime);
        do
        {
            yield return new WaitForSeconds(dashTime);
            currentTarget = LocateClosestEnemy();
            if (currentTarget == null)
            {
                gameManager.SetActiveSpecialAbility(false);
                allEnemiesAttacked = true;
                transform.position = startPos;
                transform.rotation = startRotation;
                bladeDanceReady = true;
                killCount = 0;
                GetComponent<P_Movement>().enabled = true;
                GetComponent<mouseLook>().enabled = true;
                usingSpecial = false;
                camTransitioning = true;
                yield break;
            }
            else
            {
                transform.position = currentTarget.transform.position;
                transform.rotation = currentTarget.transform.rotation;
                transform.position -= transform.forward;

                // currentTarget.GetComponent<MeshRenderer>().material.color = Color.black;
                currentTarget.SetBeenAttacked(true);
                killCount++;
            }
        } while (!allEnemiesAttacked);
    }
}
