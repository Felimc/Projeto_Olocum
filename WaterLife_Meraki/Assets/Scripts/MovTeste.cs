using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovTeste : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    public float speed;
    public float waterLevel;
    public float gravity;

    Rigidbody rb;

    private float rotY = 0.0f; // rotação ao redor eixo Y
    private float rotX = 0.0f; // rotação ao redor do eixo X

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        //Rotação do jogador
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

        //Movimentação do jogador 
        if (transform.position.y < waterLevel)
        {
            if (Input.GetAxis("Vertical") > 0)
                rb.AddRelativeForce(Vector3.forward.normalized * speed * Time.deltaTime);
            else if (Input.GetAxis("Vertical") < 0)
                rb.AddRelativeForce(Vector3.back.normalized * speed * Time.deltaTime);
        }
        else
        {
            if (Input.GetAxis("Vertical") > 0 && rotX >= 0)
                rb.AddRelativeForce(Vector3.forward.normalized * speed * Time.deltaTime);

            if (Input.GetAxis("Vertical") < 0 && rotX <= 0)
                rb.AddRelativeForce(Vector3.back.normalized * speed * Time.deltaTime);
        }

        if (Input.GetAxis("Horizontal") > 0)
            rb.AddRelativeForce(Vector3.right.normalized * speed * Time.deltaTime);
        else if (Input.GetAxis("Horizontal") < 0)
            rb.AddRelativeForce(Vector3.left.normalized * speed * Time.deltaTime);

        //Gravidade para fazer o robô afundar automaticamente na água
        if (transform.position.y < waterLevel)
            rb.AddRelativeForce(Vector3.down.normalized * gravity * Time.deltaTime);

    }
}
