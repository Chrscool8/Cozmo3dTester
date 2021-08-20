using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CozmoBotGeneral : MonoBehaviour
{
    public bool debug_mode;

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

    public bool GetDebugMode()
    {
        return debug_mode;
    }
}
