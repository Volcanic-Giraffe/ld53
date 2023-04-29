using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sounds : Singleton<Sounds>
{
    public AudioSource aSource;

    public List<AudioClip> sounds;

    private Dictionary<string, AudioClip> _soundsByName;
    private Dictionary<string, List<AudioClip>> _soundsByPrefix;

    private Transform _hero;
    
    private void Awake()
    {
        _soundsByPrefix = new Dictionary<string, List<AudioClip>>();
        _soundsByName = new Dictionary<string, AudioClip>();

        foreach (var sound in sounds)
        {
            _soundsByName.Add(sound.name, sound);
        }
    }

    public void PlayExact(string soundName, float volumeScale = 1)
    {
        if (string.IsNullOrEmpty(soundName)) return;
        
        var clip = _soundsByName[soundName];

        aSource.PlayOneShot(clip, volumeScale);
    }

    public void PlayRandom(Vector3 point, string soundPrefix, float volumeScale = 1)
    {
        if (string.IsNullOrEmpty(soundPrefix)) return;
        
        if (!_soundsByPrefix.ContainsKey(soundPrefix))
        {
            var list = new List<AudioClip>();
            foreach (var sound in sounds)
            {
                if (sound.name.StartsWith(soundPrefix))
                {
                    list.Add(sound);
                }
            }

            _soundsByPrefix.Add(soundPrefix, list);
        }

        var clips = _soundsByPrefix[soundPrefix];
        var clip = clips[Random.Range(0, clips.Count)];

        aSource.PlayOneShot(clip, volumeScale);
    }

}