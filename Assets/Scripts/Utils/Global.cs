using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : Singleton<Global>
{
    [SerializeField] private Sounds sounds;

    public static Sounds Sounds => Instance.sounds;
}
