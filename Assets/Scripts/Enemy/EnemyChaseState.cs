using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : MonoBehaviour
{
    P_Movement player;
    [SerializeField] float maxDetectionRange;
    [SerializeField] float chaseStopRange;
    [SerializeField] Animator animController;
    [SerializeField] float enemyRunSpeed;

    bool chasing = false;
    public bool dead = false;

    public bool specialInUse = false;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<P_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!specialInUse && !dead)
        {
            animController.speed = 1;
            CheckPlayerInRange();
            if(chasing)
                ChasePlayer();
        }
        else
        {
            animController.speed = 0;
        }
        if(!specialInUse && dead)
        {
            animController.speed = 1;
        }
    }

    void CheckPlayerInRange()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < maxDetectionRange)
        {
            chasing = true;
        }
    }

    void ChasePlayer()
    {
        transform.LookAt(player.transform);
        if (Vector3.Distance(transform.position, player.transform.position) > chaseStopRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemyRunSpeed * Time.deltaTime);
            animController.SetBool("chasing", true);
        }
        else
        {
            animController.SetBool("chasing", false); 
        }
    }
}
