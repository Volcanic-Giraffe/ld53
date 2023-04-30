using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MRGroup
{
    public MeshRenderer[] Group;

}
public class BlinkLights : MonoBehaviour
{
    [SerializeField] MRGroup[] Groups;
    [SerializeField] float GroupEnabledInterval = 0.5f;
    [SerializeField] Material OnMaterial;
    [SerializeField] Material OffMaterial;

    float timer = 0;
    int group = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = GroupEnabledInterval;
            ToggleGroup();
        }
    }

    private void ToggleGroup()
    {
        foreach (var grp in Groups)
        {
            foreach (var light in grp.Group)
            {
                light.material = OffMaterial;
            }
        }

        foreach (var light in Groups[group].Group)
        {
            light.material = OnMaterial;
        }

        group += 1;
        if (group >= Groups.Length) group = 0;
    }
}
