using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Velocity")]
    public float speed;
    public float acceleration;

    [Header("Raycast")]
    public Transform groundCheck;//punto de origen del raycast (los pies del player)
    public LayerMask groundLayer;//capa suelo
    public float rayLenght;//longitud del raycast
    public bool isGrounded;

    [Header("Jump")]
    public float jumpForce;

    Animator anim;
    Rigidbody2D rb2D;
    SpriteRenderer spriteRenderer;
    Vector2 targetVelocity;//esta variable me representa la velocidad a la que quiero mover el personaje
    Vector3 dampVelocity;//variable donde guardo la velocidad actual

    bool jumpPressed;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
 
    void Update()
    {
        //si el player está siendo dañado no le voy a dejar que se mueva
        if (anim.GetBool("Hurt") == true) return;

        float h = Input.GetAxis("Horizontal");

        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, rayLenght, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector2.down * rayLenght, Color.red);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) jumpPressed = true;

        targetVelocity = new Vector2(h * speed, rb2D.velocity.y);
        Animating(h);
        Flip(h);
        ChangeGravity();
    }

    private void FixedUpdate()
    {
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref dampVelocity, acceleration);
        if (jumpPressed) Jump();
    }
    void Jump()
    {
        jumpPressed = false;
        rb2D.AddForce(Vector2.up * jumpForce);
    }
    void Animating(float _h)
    {
        if (_h != 0) anim.SetBool("IsRunning", true);
        else anim.SetBool("IsRunning", false);

        anim.SetBool("IsJumping", !isGrounded);//el parámetro para indicarnos que está saltando va a tener
        //un valor contrario a la variable que nos dice si está en el suelo o no
    }
    void Flip (float _h)
    {
        if (_h > 0) spriteRenderer.flipX = false;
        else if (_h < 0) spriteRenderer.flipX = true;
    }
    void ChangeGravity()
    {
        if (rb2D.velocity.y < 0) rb2D.gravityScale = 1.5f;
        else rb2D.gravityScale = 1;
    }

    #region Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ant")) AttackEnemy(collision.gameObject);
        else if (collision.collider.CompareTag("Acorn"))
        {
            Destroy(collision.collider.gameObject);
        }
        else if (collision.collider.CompareTag("Platform"))//si el player se sube a la plataforma
            //pongo al player como hijo
            transform.SetParent(collision.transform);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))//cuando el player se baja de la platorma, le pongo su 
            //padre a null
            transform.SetParent(null);
    }
    #endregion

    void AttackEnemy(GameObject enemy)
    {
        //si estoy en el suelo o la ardilla va hacia arriba no puede atacar al enemigo
        if (isGrounded) return;
        rb2D.AddForce(Vector2.up * jumpForce);
        enemy.GetComponent<Animator>().SetTrigger("Death");//reproduzco la animación de muerte de la hormiga
        enemy.GetComponent<EnemyMovement>().Loot();
        Destroy(enemy, 0.3f);//destruyo la hormiga
    }

    public void ResetVelocity()
    {
        targetVelocity = Vector2.zero;//paro al player reseteando la velocidad
    }
}
