using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    private float _volumeInitial;
    private bool _isPlaying;

    private void Awake()
    {
        _volumeInitial = source.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (_isPlaying)
            {
                source.DOKill();
                source.DOFade(0f, 0.2f);
                
                _isPlaying = false;
            }
            else
            {
                source.DOKill();
                source.DOFade(_volumeInitial, 0.2f);
                _isPlaying = true;
            }
        }
    }
}
