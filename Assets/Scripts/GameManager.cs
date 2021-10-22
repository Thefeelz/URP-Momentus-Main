using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<EnemyBad> enemiesInLevel = new List<EnemyBad>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddEnemyToList(EnemyBad newEnemy)
    {
        enemiesInLevel.Add(newEnemy);
    }
    public List<EnemyBad> GetActiveEnemies()
    {
        return enemiesInLevel;
    }
    public void ResetAllEnemies()
    {
        foreach (var enemy in enemiesInLevel)
        {
            enemy.ResetEnemy();
        }
    }
    public void SetActiveSpecialAbility(bool value)
    {
        foreach (var enemy in enemiesInLevel)
        {
            enemy.GetComponent<EnemyChaseState>().specialInUse = value;
        }
    }
}
