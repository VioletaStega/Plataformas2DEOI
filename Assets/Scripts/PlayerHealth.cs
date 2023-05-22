using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    [Header("UI")]
    public Image acornLife;
    public GameObject[] acorns;
    public float amountLife;

    [Header("Death")]
    public float forceJumpDeath;

    Animator anim;
    PlayerMovement playerMovement;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    //función pública que se va a encagar de gestionar el daño del player y que va a ser llamada
    //desde el script del enemigo
    public void TakeDamage(int amount)
    {
        if (anim.GetBool("Hurt") == true || currentHealth <=0) return;

        currentHealth -= amount;
        //fillamount va de 0 a 1, para que esto represente la cantidad de vida que tiene el player tengo que
        //dividir la salud actual entre la salud máxima. El valor máximo que cogerá esta división sera 1 (cuando
        //la salud actual sea igual a la máxima) y conforme vaya bajando el valor de currentHealth la división irá
        //devolviendo valores menores que 1
        acornLife.fillAmount = currentHealth / maxHealth;

        anim.SetBool("Hurt", true);
        playerMovement.ResetVelocity();
        if (currentHealth <= 0)
        {
            Death();
            return;
        }
        Invoke("HurtToFalse", 1);     
    }
    void HurtToFalse()
    {
        anim.SetBool("Hurt", false);
    }
    void Death()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * forceJumpDeath);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Acorn"))
        {
            PickAcorn();
        }
    }
    void PickAcorn()
    {
        bool checkAcorn = false;
        for (int i = 0; i < acorns.Length; i++)//recorro el array de bellotas (las 3 imágenes que tengo en la UI)
        {
            if (acorns[i].activeInHierarchy == false)//si el gameobject de la casilla i está desactivado, lo activo
            {
                acorns[i].SetActive(true);
                checkAcorn = true;
                return;//me salgo del for
            }
        }
        if (checkAcorn == false)//si no se ha metido en el if que hay dentro del for, significa que
            //todas las bellotas estaban activas, con lo cual esta sería la 4 bellota que suma vida
        {
            DesactivateAcorns();
            AddLife();
        }
    }
    void DesactivateAcorns()
    {
        for(int i=0; i < acorns.Length; i++)
        {
            acorns[i].SetActive(false);
        }
    }
    void AddLife()
    {
        if (acornLife.fillAmount == 1) return;

        acornLife.fillAmount += amountLife;
    }
}
