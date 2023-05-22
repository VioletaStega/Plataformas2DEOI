using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acorn : MonoBehaviour
{
    public float force;
    public float forceTorque;//para la fuerza de giro
    public LayerMask groundLayer;

    Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        AddForce();
    }

    void Update()
    {
        
    }
    public void AddForce()
    {
        rb2D.AddForce(Vector2.up * force);
        rb2D.AddForce(Random.Range(-1, 1) * Vector2.right * force);
        rb2D.AddTorque(forceTorque);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            GetComponent<CircleCollider2D>().isTrigger = false;
        }
    } 
}
