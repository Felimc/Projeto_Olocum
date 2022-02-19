using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMov : MonoBehaviour
{
    private Camera cam;
    bool firstPersonView;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        firstPersonView = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -31.5f)
        {
            cam.clearFlags = CameraClearFlags.SolidColor;
        }
        else 
        {
            cam.clearFlags = CameraClearFlags.Skybox;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (cam.fieldOfView > 1 && !firstPersonView)
            {
                cam.fieldOfView-=20;
            }

            if (cam.fieldOfView <= 1)
            {
                transform.localPosition = new Vector3(0f, 0.5f, 1f);
                cam.fieldOfView = 60;
                firstPersonView = true;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (cam.fieldOfView < 100)
            {
                if(!firstPersonView)
                    cam.fieldOfView += 20;

            }

            if (cam.fieldOfView > 1)
            {
                transform.localPosition = new Vector3(0f, 2.53f, -6.6f);
                firstPersonView = false;
            }
        }
    }

}
