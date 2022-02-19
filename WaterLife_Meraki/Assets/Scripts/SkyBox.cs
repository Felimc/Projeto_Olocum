using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBox : MonoBehaviour
{
    public float speed;

    private void FixedUpdate()
    {
        RenderSettings.skybox.SetFloat("_Rotation",Time.time*speed);
    }
}
