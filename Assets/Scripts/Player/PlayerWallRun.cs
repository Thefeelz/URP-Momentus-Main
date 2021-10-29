using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallRun : MonoBehaviour
{
    public float minimumHeightForWallRun;
    Vector3[] directions;
    RaycastHit[] hits;

    Rigidbody playerBody;
    P_Movement playerMovement;
    bool isWallRunning;
    Vector3 lastWallPosition;
    Vector3 lastWallNormal;
    public float normalizedAngleThreshold = 0.1f;
    bool IsPlayerGrounded() => playerMovement.isGrounded;

    bool VerticalCheck()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumHeightForWallRun);
    }
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<P_Movement>();

        // Initialize Raycast Directions
        directions = new Vector3[] 
        {
            Vector3.right,
            Vector3.right + Vector3.forward,
            Vector3.forward,
            Vector3.left + Vector3.forward,
            Vector3.left
        };
    }

    // Update is called once per frame
    void LateUpdate()
    {
        isWallRunning = false;
        if (VerticalCheck())
        {
            hits = new RaycastHit[directions.Length];
            for(int i = 0; i < directions.Length; i++)
            {
                Vector3 dir = transform.TransformDirection(directions[i]);
                Physics.Raycast(transform.position, dir, out hits[i], 1);
                if(hits[i].collider !=null && hits[i].collider.GetComponent<WallRunnable>())
                {
                    Debug.DrawRay(transform.position, dir * hits[i].distance, Color.green);
                }
                else
                {
                    Debug.DrawRay(transform.position, dir * 1, Color.red);
                }
            }
            hits = hits.ToList().Where(h => h.collider != null).OrderBy(h => h.distance).ToArray();
                if(hits.Length > 0)
                {
                    OnWall(hits[0]);
                    lastWallPosition = hits[0].point;
                    lastWallNormal = hits[0].normal;
                }
        }
        if(isWallRunning){ playerMovement.isGrounded = true;}
    }

    void OnWall(RaycastHit hit){
        float d = Vector3.Dot(hit.normal, Vector3.up);
        if(d >= -normalizedAngleThreshold && d <= normalizedAngleThreshold)
        {
            // Vector3 alongWall = Vector3.Cross(hit.normal, Vector3.up);
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 alongWall = transform.TransformDirection(Vector3.forward);

            Debug.DrawRay(transform.position, alongWall.normalized * 10, Color.green);
            Debug.DrawRay(transform.position, lastWallNormal * 10, Color.magenta);

            playerBody.velocity = alongWall * vertical * 10f;
            isWallRunning = true;
        }
    }
}
