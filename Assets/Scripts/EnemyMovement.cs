using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] pointsObjects;   
    public int speedWalking;
    public GameObject acornPrefab;

    [Header("Player")]
    public float distanceToPlayer;//la distancia a la que va a dejar de patrullar y va a perseguir al player
    public GameObject player;
    public int speedAttack;
    public int speedAnimation;//para aumentar la velocidad de la animación

    Vector2[] points;//array con las posiciones de la patrulla
    Vector3 posToGo;//variable donde voy a guardar la posición a la que tiene que ir el enemigo
    int i;
    int speed;

    SpriteRenderer spriteRenderer;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        speed = speedWalking;
        //recorro el array de Transform y voy guardandome las posiciones en mi array de Vectores
        points = new Vector2[pointsObjects.Length];//Inicializando el array points con el tamaño que tiene
        //el array pointsObjects
        for(int i=0; i < pointsObjects.Length; i++)
        {
            points[i] = pointsObjects[i].position;
        }
        posToGo = points[0];
    }

    void Update()
    {
        Debug.DrawLine(transform.position, player.transform.position, Color.red);
        //si el player está en rango, lo persigo, si no está en rango, sigo patrullando
        if (Vector2.Distance(transform.position, player.transform.position) <= distanceToPlayer) AttackPlayer();
        else ChangeTargetPos();

        transform.position = Vector3.MoveTowards(transform.position, posToGo, speed * Time.deltaTime);
        Flip();
    }
    void ChangeTargetPos()//Patrulla del enemigo
    {
        speed = speedWalking;
        anim.speed = 1;
        if(transform.position == posToGo)//si he llegado a mi destino
        {
            if (i == points.Length - 1) i = 0;//si estoy en la última posición del array, voy a la primera posición
            else i++;
            posToGo = points[i];
        }
    }
    void Flip()
    {
        if (posToGo.x > transform.position.x) spriteRenderer.flipX = true;//va hacia la derecha
        else if (posToGo.x < transform.position.x) spriteRenderer.flipX = false;//va hacia la izquierda
    }
    void AttackPlayer()
    {
        speed = speedAttack;
        anim.speed = speedAnimation;
        posToGo = new Vector2(player.transform.position.x, posToGo.y);
    }
    public void Loot()
    {
        GetComponent<CircleCollider2D>().enabled = false;//le quito el collider a la hormiga para que
        //no choque con el de la bellota
        for(int i=0; i < Random.Range(1,5); i++)
        {
            GameObject acornClone = Instantiate(acornPrefab, transform.position, transform.rotation);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player") && 
            collision.collider.GetComponent<PlayerMovement>().isGrounded)
        {
            collision.collider.GetComponent<PlayerHealth>().TakeDamage(20);
            anim.SetTrigger("Death");
            Destroy(gameObject, 0.3f);
        }
    }
}
