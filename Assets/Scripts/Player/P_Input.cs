using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Input : MonoBehaviour
{
    P_GroundSlide groundSlide;
    P_Movement movement;

    A_BladeDance bladeDance;
    // Start is called before the first frame update
    void Awake()
    {
        groundSlide = GetComponent<P_GroundSlide>();
        movement = GetComponent<P_Movement>();
        bladeDance = GetComponent<A_BladeDance>();
    }

    // Update is called once per frame
    void Update()
    {
        GetUserInput();
    }

    void GetUserInput()
    {
        // ==========OVERCHARGE ABILITIES==========
        if (Input.GetKeyDown(KeyCode.Alpha1)) { bladeDance.Ability_BladeDance(); }

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
