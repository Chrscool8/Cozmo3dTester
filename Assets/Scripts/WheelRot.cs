using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // rpm to frame time
        transform.Rotate(GetComponentInParent<WheelCollider>().rpm / 60f * Time.deltaTime * 360f, 0, 0, Space.Self);
    }
}
