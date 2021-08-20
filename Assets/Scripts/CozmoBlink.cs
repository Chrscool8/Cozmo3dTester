using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CozmoBlink : MonoBehaviour
{
    // time in seconds til blinking
    double time_til_blink = 0;

    // Start is called before the first frame update
    void Start()
    {
        time_til_blink = Random.Range(1, 5);
    }

    // Update is called once per frame
    void Update()
    {
        time_til_blink -= Time.deltaTime;

        // Show Eye Model
        GetComponent<MeshRenderer>().enabled = true;

        // Hide Eye Model
        if (time_til_blink < 0)
            GetComponent<MeshRenderer>().enabled = false;

        if (time_til_blink < -.15)
            time_til_blink = Random.Range(1, 5);
    }
}
