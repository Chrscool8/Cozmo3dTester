using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum CameraModeOptions
{
    Static = 0,
    Track = 1,
    Follow = 2,
    Eyes = 3,
    NumCameraModes
}

public class CameraControl : MonoBehaviour
{
    Vector3 start_position;
    Quaternion start_rotation;

    Vector3 target_position;
    Quaternion target_rotation;

    [SerializeField] private GameObject CameraModeDisplay;
    [SerializeField] private GameObject FollowObj;
    [SerializeField] private GameObject EyesCamera;

    public CameraModeOptions DefaultCameraMode;
    private int CameraMode;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(DefaultCameraMode);
        CameraMode = ((int)DefaultCameraMode);
        CameraMode = (CameraMode) % ((int)CameraModeOptions.NumCameraModes);
        CameraModeDisplay.GetComponent<UnityEngine.UI.Text>().text = ((CameraModeOptions)CameraMode).ToString();
        Debug.Log(CameraMode);

        start_position = transform.position;
        start_rotation = transform.rotation;

        target_position = transform.position;
        target_rotation = transform.rotation;
    }

    float lengthdir_x(float length, float direction)
    {
        return length * Mathf.Cos((direction * Mathf.PI) / 180f);
    }

    float lengthdir_y(float length, float direction)
    {
        return length * -Mathf.Sin((direction * Mathf.PI) / 180f);
    }

    // Update is called once per frame
    void Update()
    {
        float move_rate = .05f;

        switch (CameraMode)
        {
            case (int)CameraModeOptions.Static:
                target_position = start_position;
                target_rotation = start_rotation;
                move_rate = .05f;
                break;

            case (int)CameraModeOptions.Track:
                target_position = FollowObj.transform.position + new Vector3(0, 3, -3);
                target_rotation = start_rotation;
                move_rate = .05f;
                break;

            case (int)CameraModeOptions.Follow:
                float xx = lengthdir_x(3, FollowObj.transform.eulerAngles.y + 90);
                float yy = 3;
                float zz = lengthdir_y(3, FollowObj.transform.eulerAngles.y + 90);

                target_position = FollowObj.transform.position + (new Vector3(xx, yy, zz));
                target_rotation = Quaternion.Euler(45, FollowObj.transform.eulerAngles.y, 0);
                move_rate = 1f;
                break;

            case (int)CameraModeOptions.Eyes:
                target_position = EyesCamera.transform.position;
                target_rotation = EyesCamera.transform.rotation;
                move_rate = 1f;
                break;
        }

        transform.position = Vector3.Lerp(transform.position, target_position, move_rate);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, 5f);
    }

    public string ToggleCameraMode()
    {
        CameraMode = (CameraMode + 1) % ((int)CameraModeOptions.NumCameraModes);
        Debug.Log(CameraMode);

        return ((CameraModeOptions)CameraMode).ToString();
    }
}
