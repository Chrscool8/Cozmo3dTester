using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovementScript : MonoBehaviour
{
    private PlayerControls playerControls;
    public GameObject rootparent;

    public float MotorSpeed;
    public WheelCollider FL;
    public WheelCollider FR;
    public WheelCollider BL;
    public WheelCollider BR;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    int sign(float input)
    {
        if (input < 0)
            return -1;
        else if (input == 0)
            return 0;
        else
            return 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug Display
        GetComponent<MeshRenderer>().enabled = rootparent.GetComponent<CozmoBotGeneral>().GetDebugMode();

        // Movement
        float h = sign(playerControls.Player.Move.ReadValue<Vector2>().x);
        float v = sign(playerControls.Player.Move.ReadValue<Vector2>().y);

        if (h != 0)
        {
            FL.motorTorque = h * MotorSpeed * 1;
            FR.motorTorque = h * MotorSpeed * -1;
            BL.motorTorque = h * MotorSpeed * 1;
            BR.motorTorque = h * MotorSpeed * -1;
        }
        else
        {
            if (v != 0)
            {
                FL.motorTorque = v * MotorSpeed;
                FR.motorTorque = v * MotorSpeed;
                BL.motorTorque = v * MotorSpeed;
                BR.motorTorque = v * MotorSpeed;
            }
        }

        if (h == 0 && v == 0)
        {
            FL.motorTorque = 0;
            FR.motorTorque = 0;
            BL.motorTorque = 0;
            BR.motorTorque = 0;
        }

        Debug.Log(h.ToString() + ", " + v.ToString());
    }
}
