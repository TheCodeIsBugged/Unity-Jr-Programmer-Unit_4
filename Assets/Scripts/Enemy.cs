using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemyRb;
    private Transform player;

    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Vector3 lookDirection = (player.position - enemyRb.transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);
    }
}
