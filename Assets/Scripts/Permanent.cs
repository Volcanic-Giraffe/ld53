using System;
using UnityEngine;

public class Permanent : Singleton<Permanent>
{
    private void Awake()
    {
        if (FindObjectsOfType<Permanent>().Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
