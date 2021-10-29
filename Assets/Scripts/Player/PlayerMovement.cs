using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerAcceleration = 10f;
    [SerializeField] float playerDeceleration = 10f;
    [SerializeField] float maxPlayerspeed = 10f;
    [SerializeField] float playerJumpPower = 10f;
    [SerializeField] float playerRotateSpeed = 60;
    [SerializeField] float playerStrafeSpeed = 10f;

    // Sliding
    [SerializeField] float slideDistance = 10f;
    [SerializeField] float slideTime = 1f;
    public float slideElapsedTime;
    public bool sliding;
    public Vector3 startingPos;
    public Vector3 endingPos;

    public bool isGrounded = true;
    float distanceToGround;

    bool returnToNormalScreen = false;
    float returnToNormalElapsedTime = 0f;
    [SerializeField] float returnToNormalScreenTime = 0.5f;
    Vector3 cameraPosStart;
    Vector3 cameraPosEnd;

    Rigidbody rb;
    OverchargeAbilities overchargeAbilities;
    CharacterStats playerStats;
    PlayerAttack playerAttack;
    Animator anim;
    Volume ppFX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distanceToGround = GetComponentInChildren<Collider>().bounds.extents.y;
        playerStats = GetComponent<CharacterStats>();
        overchargeAbilities = GetComponent<OverchargeAbilities>();
        playerAttack = GetComponent<PlayerAttack>();
        ppFX = FindObjectOfType<Volume>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForGrounded();
        GetUserInput();
        if (sliding)
            Slide();
        if (returnToNormalScreen)
            ReturnToNormalScreen();
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
        if (rb.velocity.magnitude > maxPlayerspeed)
        {
            rb.velocity = rb.velocity.normalized * maxPlayerspeed;
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
            // Move Forward
            if (Input.GetKey(KeyCode.W))
            {
                MoveForward();
            }
            // Move Backwards
            else if (Input.GetKey(KeyCode.S))
            {
                MoveBackwards();
            }
            // Decelerate the Character
            else if (rb.velocity.z != 0 && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            {
                Decelerate();
            }
            // Character Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                startingPos = transform.position;
                endingPos = transform.position + transform.forward * slideDistance;
                cameraPosStart = Camera.main.transform.localPosition;
                cameraPosEnd = Camera.main.transform.localPosition;
                cameraPosEnd.y = cameraPosStart.y - 0.5f;
                if(endingPos.y > startingPos.y) { endingPos.y = startingPos.y; }
                sliding = true;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                CalculateSlideDistance();
                Slide();
            }
            // Strafe the Character
            if (Input.GetKeyDown(KeyCode.E))
            {
                rb.AddRelativeForce(Vector3.right * playerStrafeSpeed, ForceMode.Impulse);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                rb.AddRelativeForce(Vector3.right * -playerStrafeSpeed, ForceMode.Impulse);
            }
            // strafe the Character
            if (Input.GetKey(KeyCode.D))
            {
                RotateCharacter(1);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                RotateCharacter(-1);
            }
        }

        // Ensures player is not moving faster than desired
        // LockCharacterSpeed();

        // Overcharge Abilities (In Air Allowed Abilities)
        if (!isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && (playerStats.GetPlayerOvercharge() > overchargeAbilities.Get_OverchargeAbility_AirDash_Cost()))
            {
                playerStats.RemoveHealth(overchargeAbilities.Get_OverchargeAbility_AirDash_Cost());
                overchargeAbilities.OverchargeAbility_AirDash();
            }
        }
        else /// (On Ground Allowed Abilities)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2) && (playerStats.GetPlayerOvercharge() > overchargeAbilities.Get_OverchargeAbility_SuperSlash_Cost()) && !playerAttack.GetSuperSlashStatus())
            {
                playerAttack.SetStartSuperSlash();
                // overchargeAbilities.OverchargeAbility_SuperSlash();
            }
        }
    }

    void CalculateSlideDistance()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.transform.position, transform.forward, out hit, 6);
        if(hit.collider == null) { return; }
        if(Vector3.Distance(transform.position, hit.transform.position) < slideDistance)
        {
            endingPos = hit.transform.position;
        }
    }
    void Slide()
    {
        slideElapsedTime += Time.deltaTime;
        float lerpPos = slideElapsedTime / slideTime;
        rb.MovePosition(Vector3.Lerp(startingPos, endingPos, lerpPos));
        transform.position = new Vector3(transform.position.x, startingPos.y, transform.position.z);
        ppFX.weight = Mathf.Lerp(0, 1, lerpPos);
        Camera.main.transform.localPosition = Vector3.Lerp(cameraPosStart, cameraPosEnd, lerpPos);
        if(slideElapsedTime >= slideTime)
        {
            ppFX.weight = 0;
            sliding = false;
            slideElapsedTime = 0;
            returnToNormalScreen = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
    }
    void ReturnToNormalScreen()
    {
        returnToNormalElapsedTime += Time.deltaTime;
        float lerpPos = returnToNormalElapsedTime / returnToNormalScreenTime;
        ppFX.weight = Mathf.Lerp(1, 0, lerpPos);
        Camera.main.transform.localPosition = Vector3.Lerp(cameraPosEnd, cameraPosStart, lerpPos);
        if(returnToNormalScreenTime <= returnToNormalElapsedTime)
        {
            returnToNormalElapsedTime = 0;
            returnToNormalScreen = false;
        }
    }
    private void Jump()
    {
        //Do le jump
        rb.AddForce(Vector3.up * playerJumpPower, ForceMode.VelocityChange);
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
    private void RotateCharacter(int rotationDirection)
    {
        //rb.rotation = rb.rotation * Quaternion.Euler(0, playerRotateSpeed * rotationDirection * Time.deltaTime, 0);
        rb.AddRelativeForce(Vector3.right * rotationDirection * playerAcceleration * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void MoveForward()
    {
        rb.AddRelativeForce(Vector3.forward * playerAcceleration * Time.deltaTime, ForceMode.VelocityChange);
    }
    private void MoveBackwards()
    {
        rb.AddRelativeForce(Vector3.back * playerAcceleration * Time.deltaTime, ForceMode.VelocityChange);
    }
    //Function to not allow the character to move faster than a set speed
    private void LockCharacterSpeed()
    {
        Vector3 currentSpeed = rb.velocity;
        if (!isGrounded)
            rb.velocity = currentSpeed;
    }


}