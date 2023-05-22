using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gator : MonoBehaviour
{
    public Transform[] positions;

    [Header("Speed")]
    public float speed;
    public float speedMax;
    public float speedMovement;
    public float factorSpeedAttack;

    [Header("Attack")]
    public int damage;
    public float timeToAttackPlayer;//cada cuanto tiempo va a atacar al player
    public GameObject player;

    Vector3[] wayPoints;
    Vector3 posToGo;
    SpriteRenderer spriteRenderer;
    CircleCollider2D circleCollider;
    int i;
    float timer;
    bool attacking;
    PlayerHealth playerHealth;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Start()
    {
        speed = speedMovement;
        wayPoints = new Vector3[positions.Length];
        for(int i=0; i < positions.Length; i++)
        {
            wayPoints[i] = positions[i].position;
        }
        posToGo = wayPoints[0];
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToAttackPlayer && playerHealth.currentHealth > 0) Attack();

        ChangeTargetPos();
        Flip();
        transform.position = Vector3.MoveTowards(transform.position, posToGo, speed * Time.deltaTime);
    }
    void Attack()
    {
        //establezco la posición a la que tiene que ir, LA POSICIÓN LA COJO UNA SOLA VEZ
        if (!attacking) posToGo = player.transform.position;
        circleCollider.enabled = true;
        attacking = true;

        //en cada frame calculo la distancia que hay entre enemigo y la posición de destino
        float distance = Vector2.Distance(transform.position, posToGo);
        //la velocidad va aumentado conforme nos acercamos a la posición
        speed = speedMovement * (1 / distance) * factorSpeedAttack;
        //limitamos el valor mínimo y máximo de la velocidad
        speed = Mathf.Clamp(speed, speedMovement, speedMax);

        //Cuando llegamos a la posición de destino, reseteamos el timer y ponemos la booleana a falso
        if(transform.position == posToGo)
        {
            timer = 0;
            attacking = false;
        }
    }
    void ChangeTargetPos()
    {
        if (attacking == true) return;

        speed = speedMovement;
        circleCollider.enabled = false;
        if(transform.position == posToGo)
        {
            i = Random.Range(0, wayPoints.Length);
            posToGo = wayPoints[i];
        }
    }
    void Flip()
    {
        if (posToGo.x > transform.position.x) spriteRenderer.flipX = true;//va hacia la derecha
        else if (posToGo.x < transform.position.x) spriteRenderer.flipX = false;//va hacia la izquierda
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
