using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBad : MonoBehaviour
{
    [SerializeField] GameObject gammaExplosionPrefab;
    public Animator myAnim;
    GameObject gammaExplosionObject;
    bool beenAttacked = false;
    Vector3 initialPosition;
    Quaternion initialRotation;
    GameManager gameManager;
    MeshRenderer mesh;
    bool stunned = false;
    bool gammaSetExploded = false;
    bool gammaExploding = false;
    float sizeOfGammaExplosion;
    float speedOfGammaExplosion;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddEnemyToList(this);
        mesh = GetComponent<MeshRenderer>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (gammaExploding)
        {
            ExpandGammaExplosion();
        }
    }
    public bool GetBeenAttacked()
    {
        return beenAttacked;
    }
   public void SetBeenAttacked(bool value)
    {
        beenAttacked = value;
        if(value)
        {
            GetComponent<EnemyChaseState>().dead = true;
            myAnim.SetBool("dead", true);
        }
        else
        {
            GetComponent<EnemyChaseState>().dead = false;
            myAnim.SetBool("dead", false);
        }
    }
    public bool GetStunned()
    {
        return stunned;
    }
    public void SetStunned(bool value)
    {
        if (value)
            mesh.material.color = Color.green;
        else
            mesh.material.color = Color.red;
        stunned = value;
    }
    public void ResetEnemy()
    {
        beenAttacked = false;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        //GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void StartGammaExplosion(float delayTime, float sizeOfExplosion, float gammaExplosionSpeed)
    {
        GetComponent<Rigidbody>().detectCollisions = false;
        GetComponent<Rigidbody>().useGravity = false;
        sizeOfGammaExplosion = sizeOfExplosion;
        speedOfGammaExplosion = gammaExplosionSpeed;
        StartCoroutine(StartCountdown(delayTime));
    }
    IEnumerator StartCountdown(float countDown)
    {
        yield return new WaitForSeconds(countDown);
        gammaExploding = true;
        gammaExplosionObject = Instantiate(gammaExplosionPrefab, transform);
    }
    void ExpandGammaExplosion()
    {
        if (gammaExplosionObject.transform.localScale.x < sizeOfGammaExplosion)
            gammaExplosionObject.transform.localScale += gammaExplosionObject.transform.localScale * Time.deltaTime * speedOfGammaExplosion;
        else
        {
            gammaExploding = false;
            Destroy(gammaExplosionObject);
            GetComponent<Rigidbody>().detectCollisions = true;
            GetComponent<Rigidbody>().useGravity = true;
        }

    }
}
