using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScenario : MonoBehaviour
{
    void Start()
    {
        Generator.Instance.OnGenerationDone += Setup;
    }

    private void Setup()
    {
        
    }

    void Update()
    {
        
    }
}
