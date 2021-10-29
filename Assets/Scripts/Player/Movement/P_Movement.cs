using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class P_Movement : MonoBehaviour
{
    [SerializeField] float playerAcceleration = 10f;
    [SerializeField] float playerDeceleration = 10f;
    [SerializeField] float maxPlayerspeed = 10f;
    [SerializeField] float playerJumpPower = 10f;
    [SerializeField] float playerStrafeSpeed = 10f;
    [SerializeField] float fallMultiplier = 2.5f;

    public bool isGrounded = true;
    float distanceToGround;

    Rigidbody rb;
    CharacterStats playerStats;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distanceToGround = GetComponentInChildren<Collider>().bounds.extents.y;
        playerStats = GetComponent<CharacterStats>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForGrounded();
        GetUserInput();
    }
    void FixedUpdate()
    {
        if(rb.velocity.magnitude > 2f)
        {
            anim.SetBool("running", true);
        }
        else
        {
            anim.SetBool("running", false);
        }

        float nonYMagnitude = (Mathf.Abs(rb.velocity.z) + Mathf.Abs(rb.velocity.x));
        if (nonYMagnitude > maxPlayerspeed)
        {
            rb.velocity = rb.velocity.normalized * maxPlayerspeed;
        }
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void CheckForGrounded()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f))
        {
            isGrounded = true;
        }
        else
            isGrounded = false;
    }
    void GetUserInput()
    {
        // Testing Purposes to restore health
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerStats.ReplenishHealth(10f);
        }
        // Character Movement (Checks for them to be Grounded)
        if (isGrounded)
        {
            // Decelerate the Character (Will enter the decelerate function IF the velocity is not 0 AND the user is pressing no movement keys
            if (rb.velocity.z != 0 && ((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))))
            {
                Decelerate();
            }
            // ===================================================IMPORTANT=======================================================
            // Strafe the Character (These can be not used, added to for force, etc...we will play with them and see what is good)
            if (Input.GetKeyDown(KeyCode.E))
            {
                rb.AddRelativeForce(Vector3.right * playerStrafeSpeed, ForceMode.Impulse);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                rb.AddRelativeForce(Vector3.right * -playerStrafeSpeed, ForceMode.Impulse);
            }
        }
    }
    public void Jump()
    {
        if(isGrounded && ((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))))
            rb.AddForce(Vector3.up * playerJumpPower, ForceMode.VelocityChange);
        else if (isGrounded)
            rb.AddForce(Vector3.up * playerJumpPower * 2, ForceMode.VelocityChange);
    }

    private void Decelerate()
    {
        //Store the x and z variables for ezpz access
        float currentX = rb.velocity.x;
        float currentZ = rb.velocity.z;
        //If the x direction is positive, reduce it by substraction
        if (currentX > 0)
        {
            currentX -= Mathf.Sqrt(currentX) * Time.deltaTime * playerDeceleration;
        }
        //If the x is negative, reduce it by addition
        else if (currentX < 0)
        {
            currentX += Mathf.Sqrt(-currentX) * Time.deltaTime * playerDeceleration;
        }
        //If the z is positive, reduce it by subtraction
        if (currentZ > 0)
        {
            currentZ -= Mathf.Sqrt(currentZ) * Time.deltaTime * playerDeceleration;
        }
        //If the z is negative, reduce it by addition
        else if (currentZ < 0)
        {
            currentZ += Mathf.Sqrt(-currentZ) * Time.deltaTime * playerDeceleration;
        }
        rb.velocity = new Vector3(currentX, rb.velocity.y, currentZ);
    }
    //Rotate the rigid body to be more inline with the physics system instead of rotating the transform
    public void StrafeCharacter(int rotationDirection)
    {
        //rb.rotation = rb.rotation * Quaternion.Euler(0, playerRotateSpeed * rotationDirection * Time.deltaTime, 0);
        rb.AddForce(transform.right * rotationDirection * playerAcceleration * Time.deltaTime, ForceMode.VelocityChange);
    }

    public void MoveForward()
    {
        rb.AddForce(transform.forward * playerAcceleration * Time.deltaTime, ForceMode.VelocityChange);
    }
    public void MoveBackwards()
    {
        rb.AddForce(-transform.forward * playerAcceleration * Time.deltaTime, ForceMode.VelocityChange);
    }
}