using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float distanceX;//variable que representa a que "distancia" se encuentra el fondo
    public float smoothingX;//velocidad

    Transform cam;
    Vector3 previousCamPos;//variable para guardarme la posición de la cámara en el frame anterior

    void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;

    }
    void Update()
    {
        if(distanceX !=0)
        {
            float parallaxX = (previousCamPos.x - cam.position.x) * distanceX;
            Vector3 backgroundTargetPosX = new Vector3(transform.position.x + parallaxX, transform.position.y,
                transform.position.z);
            transform.position = Vector3.Lerp(transform.position, backgroundTargetPosX, smoothingX *
                Time.deltaTime);

            previousCamPos = cam.position;
        }

    }
}
