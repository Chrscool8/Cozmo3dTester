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

    public int AI_TurnDir;
    private float AI_TurnDirTimerSeconds;

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
        AI_TurnDirTimerSeconds = Random.Range(5, 10);
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

    bool SenseRay(Vector3 from, Vector3 direction, int maxdis)
    {
        bool hit_something = false;
        if (Physics.Raycast(from, direction, maxdis))
            hit_something = true;

        RaycastHit hit;
        if (Physics.Raycast(from, direction, out hit, maxdis * 2))
        {
            BrainMap.GetComponent<CozmoBrainMap>().MarkPosition(hit.point.x, hit.point.z, true);
        }

        return hit_something;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug Display
        GetComponent<MeshRenderer>().enabled = rootparent.GetComponent<CozmoBotGeneral>().GetDebugMode();

        SenseRay(transform.position, transform.TransformDirection(Vector3.forward) * 5f, 5);

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

            bool a = SenseRay(transform.position, fwd * 2f, 2);
            bool b = SenseRay(transform.position + left * 1.5f, fwd, 2);
            bool c = SenseRay(transform.position - left * 1.5f, fwd, 2);
            if (a || b || c)
            {
                Debug.Log("There is something in front of the object!");
                if (a)
                {
                    if (b && !c)
                        drive_rotate(MotorSpeed);
                    else if (c && !b)
                        drive_rotate(-MotorSpeed);
                    else
                        drive_rotate(MotorSpeed * AI_TurnDir);
                }
                //    drive_rotate(MotorSpeed * AI_TurnDir);
                else if (b && !c)
                    drive_rotate(MotorSpeed);
                else if (c && !b)
                    drive_rotate(-MotorSpeed);
                else
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

                bool d = Physics.Raycast(transform.position + fwd * 2 - (down * .5f), down, 2);
                bool e = Physics.Raycast(transform.position + fwd * .75f - (down * .5f) + left, down, 2);
                bool f = Physics.Raycast(transform.position + fwd * .75f - (down * .5f) - left, down, 2);

                if (d && e && f)
                {
                    Debug.Log("Driving Safely");

                    if (AI_TurnDirTimerSeconds > 0)
                    {
                        AI_TurnDirTimerSeconds -= Time.deltaTime;
                    }
                    else
                    {
                        if (Random.Range(0, 10) < 5)
                            AI_TurnDir = -1;
                        else
                            AI_TurnDir = 1;

                        AI_TurnDirTimerSeconds = Random.Range(10, 15);
                    }
                }
                else
                {
                    Debug.Log("There's no floor here!!!");

                    if (!e && f)
                        drive_rotate(MotorSpeed);
                    else if (!f && e)
                        drive_rotate(-MotorSpeed);
                    else
                        drive_rotate(MotorSpeed * AI_TurnDir);
                }
            }
        }
    }
}
