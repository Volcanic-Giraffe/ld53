using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LevelVolume : Singleton<LevelVolume>
{
    [SerializeField] private Volume vol;

    private float _fgIntensityInitial;
    private FilmGrainLookup _fgTypeInitial;

    private float _hitTimer;

    private FilmGrain _fg;
    
    private void Awake()
    {
        if(vol.profile.TryGet<FilmGrain>(out var fg))
        {
            _fg = fg;
            _fgIntensityInitial = fg.intensity.value;
            _fgTypeInitial = fg.type.value;
        }
    }

    public void HitEffect(float duration)
    {
        _hitTimer = duration;

        if (_fg != null)
        {
            _fg.intensity.Override(1f);
            _fg.type.Override(FilmGrainLookup.Large01);
        }
    }

    private void Update()
    {
        if (_hitTimer > 0)
        {
            _hitTimer -= Time.deltaTime;

            if (_hitTimer <= 0)
            {
                if (_fg != null)
                {
                    _fg.intensity.Override(_fgIntensityInitial);
                    _fg.type.Override(_fgTypeInitial);
                }
            }
        }
    }
}
