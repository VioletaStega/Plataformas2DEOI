using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//INCLUIR PARA TRABAJAR CON DOTWEEN

public class TestDOTween : MonoBehaviour
{
    public Transform point;
    void Start()
    {
        transform.DOMove(point.position, 10).SetEase(Ease.OutElastic);
    }

  
}
