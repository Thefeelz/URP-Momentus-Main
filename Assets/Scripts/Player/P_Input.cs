using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Input : MonoBehaviour
{
    P_CoolDownManager coolDownManager;
    P_GroundSlide groundSlide;
    P_Movement movement;

    A_BladeDance bladeDance;
    A_AirDash airDash;
    // Start is called before the first frame update
    void Awake()
    {
        coolDownManager = GetComponent<P_CoolDownManager>();
        groundSlide = GetComponent<P_GroundSlide>();
        movement = GetComponent<P_Movement>();
        bladeDance = GetComponent<A_BladeDance>();
        airDash = GetComponent<A_AirDash>();
    }

    // Update is called once per frame
    void Update()
    {
        GetUserInput();
    }

    void GetUserInput()
    {
        // ==========OVERCHARGE ABILITIES==========
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        { 
            if(bladeDance.Ability_BladeDance()) 
                coolDownManager.AddCooldownToList(bladeDance); 
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (airDash.UseAirDash())
                coolDownManager.AddCooldownToList(airDash);
        }
        // ======================================
        // ==========CHARACTER MOVEMENT==========
        // ======================================
        
        // ==========Move Forward==========
        if(Input.GetKey(KeyCode.W)) { movement.MoveForward(); }
        // ==========Move Backwards==========
        else if (Input.GetKey(KeyCode.S)) { movement.MoveBackwards(); }
        // ==========Strafe Right==========
        if(Input.GetKey(KeyCode.D)) { movement.StrafeCharacter(1); }
        // ==========Strafe Left==========
        else if (Input.GetKey(KeyCode.A)) { movement.StrafeCharacter(-1); }
        // ==========Jump==========
        if(Input.GetKeyDown(KeyCode.Space)) { movement.Jump(); }
        // ==========Ground Dash==========
        if (Input.GetKeyDown(KeyCode.LeftShift)) { groundSlide.UseGroundDash(0.5f); }


        // ==========MENU / UI THANGS==========
    }
}
