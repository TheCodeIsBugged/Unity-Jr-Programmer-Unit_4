using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    private Transform target;
    private float speed = 15f;
    private float rocketStrength = 15f;
    private float aliveTimer = 5f;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 moveDirection = (target.position - transform.position).normalized;
            transform.Translate(moveDirection * speed * Time.deltaTime);
            transform.LookAt(target);
        }
    }

    public void Fire(Transform newTarget)
    {
        target = newTarget;
        Destroy(gameObject, aliveTimer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody targetRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 away = -collision.GetContact(0).normal;

            targetRb.AddForce(away * rocketStrength, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
}
