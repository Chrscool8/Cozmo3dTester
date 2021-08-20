using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelDebugDraw : MonoBehaviour
{
    public GameObject rootparent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<MeshRenderer>().enabled = rootparent.GetComponent<CozmoBotGeneral>().GetDebugMode();
    }
}
