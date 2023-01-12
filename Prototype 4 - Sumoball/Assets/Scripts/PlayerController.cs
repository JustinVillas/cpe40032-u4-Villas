using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject focalPoint;
    private Rigidbody playerRB;
    public float speed = 5.0f;
    public bool hasPowerup;
    private float powerUpStrength = 10.0f;
    public GameObject PowerupIndicator;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
        
    }

    // Update is called once per frame
    void Update()
    {
        //store the input conrtol of the player.
        float forwardInput = Input.GetAxis("Vertical");
        //makes the player move according to the forwrd direction of the focal point game object.
        playerRB.AddForce(focalPoint.transform.forward * speed * forwardInput);
        //follow the player around
        PowerupIndicator.transform.position = gameObject.transform.position + new Vector3(0, -0.5f, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            PowerupIndicator.gameObject.SetActive(true);
            StartCoroutine(PowerupCountdownroutine());
        }
    }

    IEnumerator PowerupCountdownroutine()
    {
        yield return new WaitForSeconds(5);
        hasPowerup = false;
        PowerupIndicator.gameObject.SetActive(false);
        Debug.Log(Time.time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRb.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);

            Debug.Log("Collided with " + collision.gameObject.name + " powerup = " + hasPowerup);

        }
    }
}

