using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;//distancia inicial entre c�mara y player
    public float smoothTargetTime;

    Vector3 smoothDampVelocity;
    PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Start()
    {
        offset = transform.position - player.position;//cojo la distancia inicial que hay entre c�mara y player
    }

    void Update()
    {
        if(playerHealth.currentHealth <=0)
        {
            transform.DOShakePosition(5, 0.8f, 8, 90);
            this.enabled = false;//deshabilito el script, la l�nea del dotween solo se lee una vez
        }
        transform.position = Vector3.SmoothDamp(transform.position, player.position + offset, ref
            smoothDampVelocity, smoothTargetTime);
    }
}
