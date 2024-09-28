using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Transform focalPoint;
    public GameObject powerupIndicator;

    private float verticalInput;
    private float force = 5f;
    //private bool hasPowerup = false;
    private float powerupStrength = 15f;

    private PowerupType currentPowerup = PowerupType.Smash;
    public GameObject rocketPrefab;
    private GameObject tmpRocket;
    private Coroutine powerupCountdown;

    private float hangTime = 1f;
    private float smashSpeed = 5f;
    private float floorY;
    private float explosionForce = 1000f;
    private float explosionRadius = 5f;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPowerup == PowerupType.Rockets && Input.GetKeyDown(KeyCode.Space))
        {
            LaunchRockets();
        }
        else if (currentPowerup == PowerupType.Smash && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SmashCoroutine());
        }
    }

    void FixedUpdate()
    {
        verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.forward * force * verticalInput);
        powerupIndicator.transform.position = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            //hasPowerup = true;
            currentPowerup = other.GetComponent<PowerUp>().powerupType;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);

            if (powerupCountdown != null)
            {
                StopCoroutine("powerupCountdown");
            }
            powerupCountdown = StartCoroutine("PowerupCountdownRoutine");
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && currentPowerup == PowerupType.Pushback)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7f);
        //hasPowerup = false;
        powerupIndicator.SetActive(false);
        currentPowerup = PowerupType.None;
    }

    void LaunchRockets()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            tmpRocket.GetComponent<RocketBehaviour>().Fire(enemy.transform);

        }
    }

    void Smash()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (collider.CompareTag("Enemy"))
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }

    IEnumerator SmashCoroutine()
    {
        floorY = transform.position.y;
        float jumpTime = Time.time + hangTime;
        while (Time.time < jumpTime)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, smashSpeed, playerRb.velocity.z);
            yield return null;
        }
        
        while (transform.position.y > floorY)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, -smashSpeed * 4, playerRb.velocity.z);
            yield return null;
        }

        Smash();
    }
}
