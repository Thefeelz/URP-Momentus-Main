using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] float playerMaxHealth = 100;
    [SerializeField] float playerCurrentHealth;
    [SerializeField] float playerMaxOvercharge = 100;
    [SerializeField] float playerCurrentOvercharge = 0;

    [SerializeField] int playerStrength = 10;
    [SerializeField] int playerDefense = 10;

    [SerializeField] Camera firstPersonCam;
    [SerializeField] Camera thirdPersonCam;
    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReplenishHealth(float amountToRestore)
    {
        float difference = 0f;
        // Check to see if health is full with the restore
        if(playerCurrentHealth + amountToRestore >= playerMaxHealth)
        {
            difference = playerCurrentHealth + amountToRestore - playerMaxHealth;
            // Set Player Current Health to Max Health
            playerCurrentHealth = playerMaxHealth;

            // Check to see if Overcharge wil be full
            if((playerCurrentOvercharge + difference) >= playerMaxOvercharge)
            {
                // Set overcharge to max amount
                playerCurrentOvercharge = playerMaxOvercharge;
                return;
            }
            else /// Player Overcharge will not overfill
            {
                playerCurrentOvercharge += difference;
                return;
            }
        } 
        else
        {
            playerCurrentHealth += amountToRestore;
        }
        // Check to see if amountToRestore will exceed full health

        // Add to overcharge
    }
    public void RemoveHealth(float amountToRemove)
    {
        float difference = 0f;
        if(playerCurrentOvercharge > 0)
        {
            if(playerCurrentOvercharge - amountToRemove > 0)
            {
                playerCurrentOvercharge -= amountToRemove;
            }
            else
            {
                difference = playerCurrentOvercharge - amountToRemove;
                playerCurrentOvercharge = 0;
                playerCurrentHealth += difference;
            }
        }
        else
        {
            playerCurrentHealth -= amountToRemove;
        }
    }
    public void SetPlayerHealth(float newPlayerHealth)
    {
        playerCurrentHealth = newPlayerHealth;
    }
    public float GetPlayerHealth()
    {
        return playerCurrentHealth;
    }
    public void SetPlayerOvercharge(float newPlayerOvercharge)
    {
        playerCurrentOvercharge = newPlayerOvercharge;
    }
    public float GetPlayerOvercharge()
    {
        return playerCurrentOvercharge;
    }
    public void SetPlayerMaxHealth(float newPlayerMaxHealth)
    {
        playerMaxHealth = newPlayerMaxHealth;
    }
    public float GetPlayerMaxHealth()
    {
        return playerMaxHealth;
    }
    public void SetPlayerMaxOvercharge(float newPlayerMaxOvercharge)
    {
        playerMaxOvercharge = newPlayerMaxOvercharge;
    }
    public float GetPlayerMaxOvercharge()
    {
        return playerMaxOvercharge;
    }
    public void SetPlayerStrength(int newPlayerStrength)
    {
        playerStrength = newPlayerStrength;
    }
    public int GetPlayerStrength()
    {
        return playerStrength;
    }
    public void SetFirstPersonCam()
    {
        firstPersonCam.gameObject.SetActive(true);
        thirdPersonCam.gameObject.SetActive(false);
    }
    public void SetThirdPersonCam()
    {
        thirdPersonCam.gameObject.SetActive(true);
        firstPersonCam.gameObject.SetActive(false);
    }
}
