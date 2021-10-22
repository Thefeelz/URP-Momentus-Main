using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform startingPoint;
    [SerializeField] Transform endingPoint;
    [SerializeField] GameObject door;
    [SerializeField] float doorOpenTime;
    float timeSinceStart;
    bool openDoor = false;
    bool closeDoor = false;

    void Update()
    {
        if(openDoor)
            OpenDoor();
        if (closeDoor)
            CloseDoor();

    }

    void OpenDoor()
    {
        timeSinceStart += Time.deltaTime;
        if (timeSinceStart < doorOpenTime)
        {
            door.transform.position = Vector3.Lerp(startingPoint.position, endingPoint.position, timeSinceStart / doorOpenTime);
        }
        else
        {
            timeSinceStart = 0;
            openDoor = false;
        }
    }
    void CloseDoor()
    {
        timeSinceStart += Time.deltaTime;
        if (timeSinceStart < doorOpenTime)
        {
            door.transform.position = Vector3.Lerp(endingPoint.position, startingPoint.position, timeSinceStart / doorOpenTime);
        }
        else
        {
            timeSinceStart = 0;
            closeDoor = false;
        }
    }
    public void SetOpenDoor()
    {
        openDoor = true;
        closeDoor = false;
    }
    public void SetCloseDoor()
    {
        openDoor = false;
        closeDoor = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Body"))
        {
            SetCloseDoor();
            FindObjectOfType<LevelDeteriation>().setSafeZone(true);
            timeSinceStart = 0;
            this.GetComponent<Collider>().enabled = false;
        }
    }
}
