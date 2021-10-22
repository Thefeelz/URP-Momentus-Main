using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDeteriation : MonoBehaviour
{
    [SerializeField] float deteriationRate = 1f;

    bool safeZone = false;

    CharacterStats ourPlayer;
    void Start()
    {
        ourPlayer = FindObjectOfType<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!safeZone)
            DeteriateHealth();
    }

    void DeteriateHealth()
    {
        if(ourPlayer.GetPlayerOvercharge() <= 0)
        {
            ourPlayer.SetPlayerOvercharge(0f);
            ourPlayer.SetPlayerHealth(ourPlayer.GetPlayerHealth() - Time.deltaTime / deteriationRate);
        } else if (ourPlayer.GetPlayerOvercharge() > 0)
        {
            ourPlayer.SetPlayerOvercharge(ourPlayer.GetPlayerOvercharge() - Time.deltaTime / deteriationRate);
        }
    }

    public void setSafeZone(bool value)
    {
        safeZone = value;
    }
}
