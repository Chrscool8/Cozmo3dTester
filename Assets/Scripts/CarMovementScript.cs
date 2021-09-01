using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovementScript : MonoBehaviour
{
    private PlayerControls playerControls;
    public GameObject rootparent;
    public GameObject BrainMap;

    public float MotorSpeed;
    public WheelCollider FL;
    public WheelCollider FR;
    public WheelCollider BL;
    public WheelCollider BR;

    private int AI_TurnDir = 1;
    private float AI_TurnDirTimerSeconds = 1;

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

    void drive_stop()
    {
        FL.motorTorque = 0;
        FR.motorTorque = 0;
        BL.motorTorque = 0;
        BR.motorTorque = 0;
    }

    void drive_forward(float speed)
    {
        FL.motorTorque = speed;
        FR.motorTorque = speed;
        BL.motorTorque = speed;
        BR.motorTorque = speed;
    }

    void drive_rotate(float speed)
    {
        FL.motorTorque = speed;
        FR.motorTorque = -speed;
        BL.motorTorque = speed;
        BR.motorTorque = -speed;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug Display
        GetComponent<MeshRenderer>().enabled = rootparent.GetComponent<CozmoBotGeneral>().GetDebugMode();


        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward) * 5f, out hit, 5))
        {
            BrainMap.GetComponent<CozmoBrainMap>().MarkPosition(hit.point.x, hit.point.y, true);
        }

        if (rootparent.GetComponent<CozmoBotGeneral>().getRoboMode() == (int)RoboModeOptions.Explorer)
        {
            // Movement
            float h = sign(playerControls.Player.Move.ReadValue<Vector2>().x);
            float v = sign(playerControls.Player.Move.ReadValue<Vector2>().y);

            if (h != 0)
            {
                drive_rotate(MotorSpeed * h);
            }
            else if (v != 0)
            {
                drive_forward(v * MotorSpeed);
            }

            if (h == 0 && v == 0)
            {
                drive_stop();
            }

            // Debug
            if (rootparent.GetComponent<CozmoBotGeneral>().GetDebugMode())
                Debug.Log(h.ToString() + ", " + v.ToString());
        }
        else if (rootparent.GetComponent<CozmoBotGeneral>().getRoboMode() == (int)RoboModeOptions.WanderAI)
        {
            ////////////
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            Vector3 left = transform.TransformDirection(Vector3.left) * .5f;

            // Look Forward
            if (rootparent.GetComponent<CozmoBotGeneral>().GetDebugMode())
            {
                Debug.DrawRay(transform.position + left * 1.5f, fwd, Color.blue, 0);
                Debug.DrawRay(transform.position - left * 1.5f, fwd, Color.blue, 0);
                Debug.DrawRay(transform.position, fwd * 2f, Color.blue, 0);
            }

            if (Physics.Raycast(transform.position, fwd * 2f, 2) || Physics.Raycast(transform.position + left * 1.5f, fwd, 2) || Physics.Raycast(transform.position - left * 1.5f, fwd, 2))
            {
                Debug.Log("There is something in front of the object!");
                drive_rotate(MotorSpeed * AI_TurnDir);
            }
            else
            {
                Debug.Log("Going Forward!");
                drive_forward(MotorSpeed);

                Vector3 down = transform.TransformDirection(Vector3.down);

                if (rootparent.GetComponent<CozmoBotGeneral>().GetDebugMode())
                {
                    Debug.DrawRay(transform.position + fwd * .5f - (down * .5f) + left * 2f, down, Color.red, 0);
                    Debug.DrawRay(transform.position + fwd * .5f - (down * .5f) - left * 2f, down, Color.red, 0);
                    Debug.DrawRay(transform.position + fwd * 2 - (down * .5f) - left * 0f, down, Color.red, 0);
                }

                if (Physics.Raycast(transform.position + fwd * .5f - (down * .5f) + left, down) && Physics.Raycast(transform.position + fwd * .5f - (down * .5f) - left, down) && Physics.Raycast(transform.position + fwd * 2 - (down * .5f), down))
                {
                    Debug.Log("Driving Safely");

                    if (AI_TurnDirTimerSeconds > 0)
                    {
                        AI_TurnDirTimerSeconds -= Time.deltaTime;
                    }
                    else
                    {
                        if (AI_TurnDir == 1)
                            AI_TurnDir = -1;
                        else
                            AI_TurnDir = 1;

                        AI_TurnDirTimerSeconds = Random.Range(10, 15);
                    }
                }
                else
                {
                    Debug.Log("There's no floor here!!!");
                    drive_stop();
                    drive_rotate(MotorSpeed * AI_TurnDir);
                }
            }







        }
    }
}
