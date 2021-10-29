using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private GameObject first;
    Vector3 firstSpot;
    Transform postion2;
    Transform postion3;

    public GameObject[] spot;

// Start is called before the first frame update
    void Start()
    {
        firstSpot = first.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int i=1;
        /*if(transform.position == position[0])
        {
            i = 1;
        }
        else
        {
            i = 0;
        }*/
        float temp=0;
        temp = temp + 1 * Time.deltaTime;
        this.transform.position=Vector3.Lerp(transform.position, firstSpot,temp );
        
    }
}
