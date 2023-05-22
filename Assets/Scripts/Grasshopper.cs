using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grasshopper : MonoBehaviour
{
    [Header("Force Jump")]
    public float forceUp;
    public float forceRight;
    public float timeToJump;

    [Header("Raycast")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float rayLenght;
    public bool isGrounded;

    int direction;//esta variable me va a indicar si se mueve a izquierda o derecha
    Rigidbody2D rb2D;
    Animator anim;
    SpriteRenderer spriteRenderer;
    float timer;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        direction = 1;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToJump) Jump();

        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, rayLenght, groundLayer);
        anim.SetFloat("YVelocity", rb2D.velocity.y);
        anim.SetBool("Jump", !isGrounded);
    }
    void Jump()
    {
        timer = 0;
        direction *= -1;//cambio dirección
        Flip();
        rb2D.AddForce(Vector2.up * forceUp, ForceMode2D.Impulse);
        rb2D.AddForce(Vector2.right * direction * forceRight, ForceMode2D.Impulse);
    }
    void Flip()
    {
        if (direction > 0) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
    }
}
