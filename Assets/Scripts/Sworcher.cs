using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Sworcher : MonoBehaviour
{
    void Start()
    {
        transform.DOScale(0.1f, 0.7f).SetLoops(-1, LoopType.Yoyo);
    }

}
