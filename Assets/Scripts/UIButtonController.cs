using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonController : MonoBehaviour
{
    public GameObject CozmoParent;

    public void ToggleDebugMode()
    {
        CozmoParent.GetComponent<CozmoBotGeneral>().ToggleDebugMode();
        Debug.Log("Click!");
    }

    public void ToggleCozmoCam()
    {
        CozmoParent.GetComponent<CozmoBotGeneral>().ToggleCozmoCam();
        Debug.Log("Click!");
    }
}
