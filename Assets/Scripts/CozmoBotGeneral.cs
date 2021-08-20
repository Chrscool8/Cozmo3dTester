using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CozmoBotGeneral : MonoBehaviour
{
    [SerializeField] private bool debug_mode;
    [SerializeField] private Camera CozmoCam;

    // Start is called before the first frame update
    void Start()
    {
        //debug_mode = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool ToggleDebugMode()
    {
        debug_mode = !debug_mode;
        return debug_mode;
    }

    public void ToggleCozmoCam()
    {
        CozmoCam.gameObject.SetActive(!CozmoCam.isActiveAndEnabled);

    }

    public bool GetDebugMode()
    {
        return debug_mode;
    }

    public void SetDebugMode(bool setting)
    {
        debug_mode = setting;
    }
}
