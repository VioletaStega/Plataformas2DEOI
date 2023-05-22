using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector2 moveSpeed;

    void Update()
    {
        transform.Translate(Vector3.right * moveSpeed.x * Time.deltaTime);
        transform.Translate(Vector3.up * moveSpeed.y * Time.deltaTime);
    }
}
