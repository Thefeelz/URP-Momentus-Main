using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThang : MonoBehaviour
{
    [SerializeField] int enemyMaxHealth = 20;
    [SerializeField] int enemyCurrentHealth;
    [SerializeField] float movementSpeed = 1f;

    CharacterStats player;
    MasterLevel masterLevel;


    void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        player = FindObjectOfType<CharacterStats>();
        masterLevel = FindObjectOfType<MasterLevel>();
    }

    void Update()
    {
        //MoveToPlayer();
    }
    public void TakeDamage(int damage)
    {
        enemyCurrentHealth -= damage;
        if(enemyCurrentHealth <= 0)
        {
        }
    }
}
