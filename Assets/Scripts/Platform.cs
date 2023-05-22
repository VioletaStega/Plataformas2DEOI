using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform[] positions;
    public int speed;

    Vector3[] points;
    Vector3 posToGo;
    int i;
    void Start()
    {
        points = new Vector3[positions.Length];
        for(int i=0; i< positions.Length; i++)
        {
            points[i] = positions[i].position;
        }
        posToGo = points[0];
    }
    void Update()
    {
        ChangeTargetPos();
        transform.position = Vector3.MoveTowards(transform.position, posToGo, speed * Time.deltaTime);
    }
    void ChangeTargetPos()
    {
        if (transform.position == posToGo)
        {
            if (i == points.Length - 1) i = 0;
            else i++;
            posToGo = points[i];
        }
    }
}
