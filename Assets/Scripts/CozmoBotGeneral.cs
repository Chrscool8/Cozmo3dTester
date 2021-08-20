using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum RoboModeOptions
{
    None = 0,
    Explorer = 1,
    WanderAI = 2,
    NumRoboModes
}

public class CozmoBotGeneral : MonoBehaviour
{
    [SerializeField] private bool debug_mode;
    [SerializeField] private Camera CozmoCam;
    [SerializeField] private GameObject TextModeDisplay;
    [SerializeField] private GameObject CozmoBody;


    public RoboModeOptions DefaultRoboMode;
    private int RoboMode;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(DefaultRoboMode);
        RoboMode = ((int)DefaultRoboMode);
        RoboMode = (RoboMode) % ((int)RoboModeOptions.NumRoboModes);
        TextModeDisplay.GetComponent<UnityEngine.UI.Text>().text = ((RoboModeOptions)RoboMode).ToString();
        Debug.Log(RoboMode);
    }

    // Update is called once per frame
    void Update()
    {
        if (CozmoBody.transform.position.y < -10)
        {
            CozmoBody.transform.position = new Vector3(0, 1, 0);
            CozmoBody.transform.rotation = new Quaternion(0, 0, 0, 0);
            CozmoBody.transform.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
            CozmoBody.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }

    public int getRoboMode()
    {
        return RoboMode;
    }

    public string ToggleAIMode()
    {
        RoboMode = (RoboMode + 1) % ((int)RoboModeOptions.NumRoboModes);
        Debug.Log(RoboMode);

        return ((RoboModeOptions)RoboMode).ToString();
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
