using System;
using UnityEngine;

public class Permanent : Singleton<Permanent>
{
    [SerializeField] private Sounds sounds;
    
    private void Awake()
    {
        if (FindObjectsOfType<Permanent>().Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    
    public static Sounds Sounds => Instance.sounds;
}
