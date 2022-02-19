using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderWater : MonoBehaviour
{
    public float waterLevel;
    public Transform player;
    public float fogRate;

    bool isUnderwater;

    private Color normalColor;
    private Color underwaterColor;

    Rigidbody playerRigidBody;
    
    // Start is called before the first frame update
    void Start()
    {
        normalColor = new Color(0.5f, 0.5f, 0.5f);
        underwaterColor = new Color(0.343f, 0.451f, 0.537f, 0.5f);
        playerRigidBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if ((transform.position.y < waterLevel) != isUnderwater) 
        {
            isUnderwater = transform.position.y < waterLevel;
            if (isUnderwater)
                SetUnderwater();
            else if (!isUnderwater)
                SetNormal();
        }*/

        if (player.position.y >= 28.1)
            SetNormal();
        else if (player.position.y < 28.1 && transform.position.y >= 18.1)
            SetUnderwater1();
        else if (player.position.y < 18.1 && transform.position.y >= 8.1)
            SetUnderwater2();
        else if (player.position.y < 8.1 && transform.position.y >= -2.1)
            SetUnderwater3();
        else if (player.position.y < -2.1 && transform.position.y >= -12.1)
            SetUnderwater4();
        else if (player.position.y < -12.1)
            SetUnderwater5();
    }

    void SetNormal()
    {
        if (RenderSettings.fogDensity > 0.0035f)
            RenderSettings.fogDensity -= fogRate * Time.deltaTime;
        if(transform.position.y > waterLevel)
            RenderSettings.fogColor = normalColor;
        else
            RenderSettings.fogColor = underwaterColor;
    }

    void SetUnderwater1()
    {
        RenderSettings.fogColor = underwaterColor;
        if (playerRigidBody.velocity.y < 0 && RenderSettings.fogDensity < 0.0096f)
            RenderSettings.fogDensity += fogRate * Time.deltaTime;

        if (playerRigidBody.velocity.y > 0 && RenderSettings.fogDensity > 0.0096f)
            RenderSettings.fogDensity -= fogRate * Time.deltaTime;
    }

    void SetUnderwater2()
    {
        RenderSettings.fogColor = underwaterColor;
        if (playerRigidBody.velocity.y < 0 && RenderSettings.fogDensity < 0.0172f)
            RenderSettings.fogDensity += fogRate * Time.deltaTime;

        if (playerRigidBody.velocity.y > 0 && RenderSettings.fogDensity > 0.0172f)
            RenderSettings.fogDensity -= fogRate * Time.deltaTime;
    }

    void SetUnderwater3()
    {
        RenderSettings.fogColor = underwaterColor;
        if (playerRigidBody.velocity.y < 0 && RenderSettings.fogDensity < 0.0348f)
            RenderSettings.fogDensity += fogRate * Time.deltaTime;

        if (playerRigidBody.velocity.y > 0 && RenderSettings.fogDensity > 0.0348f)
            RenderSettings.fogDensity -= fogRate * Time.deltaTime;
    }

    void SetUnderwater4()
    {
        RenderSettings.fogColor = underwaterColor;
        if (playerRigidBody.velocity.y < 0 && RenderSettings.fogDensity < 0.0524f)
            RenderSettings.fogDensity += fogRate * Time.deltaTime;

        if (playerRigidBody.velocity.y > 0 && RenderSettings.fogDensity > 0.0524f)
            RenderSettings.fogDensity -= fogRate * Time.deltaTime;
    }

    void SetUnderwater5()
    {
        RenderSettings.fogColor = underwaterColor;
        if(playerRigidBody.velocity.y < 0 && RenderSettings.fogDensity < 0.07f)
            RenderSettings.fogDensity += fogRate*Time.deltaTime;

    }
}
