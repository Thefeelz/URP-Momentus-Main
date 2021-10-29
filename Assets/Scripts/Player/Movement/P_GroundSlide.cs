using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class P_GroundSlide : MonoBehaviour
{
    // Last Edit: Brandon Hinz 10/26/21
    // Player Ground Slide Class

    // ==========Variables to change in the inspector==========

    // The Distance our player will slide
    [SerializeField] float slideDistance = 10f;
    // The effect that is on the screen (Multiple will be placed in the scene, drag and drop which one we want per effect)
    [SerializeField] Volume postProcessingEffects;
    // This is the return to Normal screen time from the Post Processing Effect
    [SerializeField] float returnToNormalScreenTime = 0.5f;
    // The duration it takes for the player to finish the slide (Literally is the movement speed of the player)
    [SerializeField] float slideDuration = 0.5f;
    // The amount the camera will drop during the slide (It will raise automatically on its own)
    [SerializeField] float cameraDipAmount = 1.0f;

    // ==========Variables to Cache in the Awake Function==========

    // A call to our rigid body to effect it
    Rigidbody rb;
    // A call to our player to check if we are grounded
    P_Movement player;

    // ==========Private Variables to be used in the code ==========
    
    // Bool for the transition to the normal screen (Lerps off the Post Processing Effect)
    bool returnToNormalScreen = false;
    // Bool to start the actual slide in the 'Update' Function
    bool sliding = false;
    // Elapsed time for the slide (Used for the Lerp Function)
    float elapsedTime = 0f;
    // Elapsed time for the removal of the Post Processing Effect (Used in a Lerp Function)
    float returnToNormalScreenElapsedTime = 0f;

    // Variable to store the starting position of the player
    Vector3 startingPos;
    // Variable to store the ending position of the player
    Vector3 endingPos;
    // Variable to store the starting position of the camera attached to the player
    Vector3 cameraPosStart;
    // Variable to store the ending position of the camera attached to the player
    Vector3 cameraPosEnd;

    Collider playerCollider;

    // Start is called before the first frame update
    void Awake()
    {
        // Cache our Variables
        player = GetComponent<P_Movement>();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponentInChildren<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        // This will be true if we are sliding, false if we are not
        if (sliding)
            Slide();
        // If a post processing volume is attached to this ability, this will return the screen smoothly back to normal
        if (returnToNormalScreen)
            ReturnToNormalScreen();
    }

    // =================================================================
    // ========FUNCTION CALLED IN THE P_INPUT.CS TO START SLIDE=========
    // =================================================================
    public void UseGroundDash(float _slideDuration)
    {
        // Check to make sure the player is grounded
        if(player.isGrounded)
        {
            // Sliding is set to true to allow the sliding function to be called in the 'Update' function
            sliding = true;
            // This can be removed or not depending on if we are implimenting a seperate cooldown/ability system outside of the specific component
            slideDuration = _slideDuration;
            // A helper function that will do some checks to make sure we dont bug out in the game
            SetUpSlide();
        }
    }

    void Slide()
    {
        // While sliding, since we are Lerping we need a constant number to Lerp against
        elapsedTime += Time.deltaTime;
        // Set the lerp progress to a variable so we dont have to calculate it 3 times, just once
        float lerpPos = elapsedTime / slideDuration;
        // Move the rigid body forward by the current lerp amount
        rb.MovePosition(Vector3.Lerp(startingPos, endingPos, lerpPos));
        // Lerp our camera effects as well so they increase to full intensity while we are sliding
        postProcessingEffects.weight = Mathf.Lerp(0, 1, lerpPos);
        // Lerp our cameras position so it raises and lower during the slide
        Camera.main.transform.localPosition = Vector3.Lerp(cameraPosStart, cameraPosEnd, lerpPos);
        // If our slide is finished, set everything back to default values and trigger return to normal screen for our camera position and camera effects

        RaycastHit hit;
        Vector3 newpos = (transform.forward + transform.position);
        newpos = new Vector3(newpos.x, playerCollider.bounds.center.y - playerCollider.bounds.extents.y + 0.1f, newpos.z);
        // Physics.Linecast(transform.position, newpos, out hit, 6);
        Physics.Linecast(transform.position + transform.forward, transform.position + transform.forward * 2, out hit);

        if (hit.collider)
            Debug.Log(hit.collider.name);
        if (elapsedTime >= slideDuration || (hit.collider != null))
        {
            postProcessingEffects.weight = 0;
            sliding = false;
            elapsedTime = 0;
            returnToNormalScreen = true;
        }
    }

    void SetUpSlide()
    {
        // Starting position set to our players current position
        startingPos = transform.position;
        // Ending position set to our position plus the distance forward we determine in the inspector
        endingPos = transform.position + transform.forward * slideDistance;
        // Camera starting position RELATIVE to our player (not the world)
        cameraPosStart = Camera.main.transform.localPosition;
        // Camera end position RELATIVE to our player (not the world) - the camera dip amount determined in the inspector
        cameraPosEnd = Camera.main.transform.localPosition;
        cameraPosEnd.y = cameraPosStart.y - cameraDipAmount;
        // Just in case the raycast raises in the 'Y' direction, set it so the player cannot go higher while using a ground slide
        endingPos.y = startingPos.y;
        // Function to determine if the player has a clear path or not
        CalculateSlideDistance();
    }

    void CalculateSlideDistance()
    {
        // Raycast hit to store our raycast hit information
        RaycastHit hit;
        // A raycast that shoots out from our feet forward relative to where we are facing
        Physics.Raycast(transform.position, transform.forward, out hit, slideDistance);
        
        // If the raycast hits nothing, go the full length of the slide and return
        if (hit.collider == null || hit.collider.GetComponent<Floor>() || hit.collider.GetComponentInParent<P_CoolDownManager>()) { return; }
        // If we are at this point, we HIT something with our raycast, see if the raycast hit point distance is less than our slide distance
        if (Vector3.Distance(transform.position, hit.point) < slideDistance)
        {
            Debug.Log("We hit " + hit.collider.name);
            // If it is, set our ending position to where the ray made contact
            endingPos = hit.point - transform.forward;
            endingPos = new Vector3(endingPos.x, transform.position.y, endingPos.z);
        }
    }

    void ReturnToNormalScreen()
    {
        returnToNormalScreenElapsedTime += Time.deltaTime;
        float lerpPos = returnToNormalScreenElapsedTime / returnToNormalScreenTime;
        postProcessingEffects.weight = Mathf.Lerp(1, 0, lerpPos);
        Camera.main.transform.localPosition = Vector3.Lerp(cameraPosEnd, cameraPosStart, lerpPos);
        if (returnToNormalScreenTime <= returnToNormalScreenElapsedTime)
        {
            returnToNormalScreenElapsedTime = 0;
            returnToNormalScreen = false;
        }
    }
}
