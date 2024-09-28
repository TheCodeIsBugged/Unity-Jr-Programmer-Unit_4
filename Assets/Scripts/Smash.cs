using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : MonoBehaviour
{
    private Rigidbody playerRb;
    private float explosionForce = 1000f;
    private float explosionRadius = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SmashPowerup()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (collider.CompareTag("Enemy") && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log(colliders.Length);
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }


}
