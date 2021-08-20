using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDebugButton : MonoBehaviour
{
    public GameObject CozmoParent;

    public void ToggleDebugMode()
    {
        CozmoParent.GetComponent<CozmoBotGeneral>().ToggleDebugMode();
        Debug.Log("Click!");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
