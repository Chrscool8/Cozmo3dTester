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
    public float AI_TurnDirTimerSeconds;

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
        AI_TurnDirTimerSeconds = Random.Range(8, 10);
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

    bool SenseRay(Vector3 from, Vector3 direction, int maxdis, bool inverted = false)
    {
        bool hit_something = false;
        if (Physics.Raycast(from, direction, maxdis))
            hit_something = true;

        if (!inverted)
        {
            RaycastHit hit;
            if (Physics.Raycast(from, direction, out hit, maxdis * 2))
            {
                BrainMap.GetComponent<CozmoBrainMap>().MarkPosition(hit.point.x, hit.point.z, true, 0, true);
            }
        }
        else
        {
            RaycastHit hit;
            if (!Physics.Raycast(from, direction, out hit, maxdis * 2))
            {
                BrainMap.GetComponent<CozmoBrainMap>().MarkPosition(from.x, from.z, true, 0, true);
            }
        }

        if (inverted)
            return !hit_something;
        else
            return hit_something;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug Display
        GetComponent<MeshRenderer>().enabled = rootparent.GetComponent<CozmoBotGeneral>().GetDebugMode();

        SenseRay(transform.position, transform.TransformDirection(Vector3.forward) * 5f, 5);

        BrainMap.GetComponent<CozmoBrainMap>().MarkPosition(transform.position.x, transform.position.z, false, 2);

        if (rootparent.GetComponent<CozmoBotGeneral>().getRoboMode() == (int)RoboModeOptions.None)
        {
            drive_stop();
        }
        else if (rootparent.GetComponent<CozmoBotGeneral>().getRoboMode() == (int)RoboModeOptions.Explorer)
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
            Vector3 down = transform.TransformDirection(Vector3.down);

            bool a = SenseRay(transform.position, fwd * 2f, 2);
            bool b = SenseRay(transform.position + left * 1.25f, fwd, 2);
            bool c = SenseRay(transform.position - left * 1.25f, fwd, 2);

            bool d = SenseRay(transform.position + fwd * 1.85f - (down * .5f), down, 2, true);
            bool e = SenseRay(transform.position + fwd * 1.25f - (down * .5f) + left, down, 2, true);
            bool f = SenseRay(transform.position + fwd * 1.25f - (down * .5f) - left, down, 2, true);

            bool front_occupied = a || d;
            bool left_occupied = b || e;
            bool right_occupied = c || f;

            if (front_occupied || left_occupied || right_occupied)
            {
                AI_TurnDirTimerSeconds -= Time.deltaTime;
            }

            if (left_occupied && !right_occupied)
                drive_rotate(MotorSpeed);
            else if (!left_occupied && right_occupied)
                drive_rotate(-MotorSpeed);
            else if (front_occupied && left_occupied && right_occupied)
            {
                if (Random.value < .25f)
                    drive_forward(-MotorSpeed * .5f);
                else
                    drive_rotate(MotorSpeed * AI_TurnDir * .5f);
            }
            else if (front_occupied)
                drive_rotate(MotorSpeed * AI_TurnDir);
            else
            {
                drive_forward(MotorSpeed);

                if (AI_TurnDirTimerSeconds < 0)
                {
                    if (Random.value < .5f)
                        AI_TurnDir = -1;
                    else
                        AI_TurnDir = 1;

                    AI_TurnDirTimerSeconds = Random.Range(6, 8);
                }
            }
        }
    }
}
