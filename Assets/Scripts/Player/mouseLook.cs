using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLook : MonoBehaviour
{
    [SerializeField]
    private float mouseSens = 100f;
    private float mouseMultiplier = 10f;
    private float horizontalRotation = 0f;
    private float verticalRotation = 0f;
   
    // Start is called before the first frame update
    void Start()
    {
        //stops mouse from showing up
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //get mouse position
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * mouseMultiplier * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * mouseMultiplier * Time.deltaTime;
        
        //rotate left/right
        horizontalRotation += mouseX;
        

        //rotate up/down
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -30f, 30f);
        
        
       // transform.localRotation = Quaternion.Euler(, 0f, 0f);
        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
    }
}
