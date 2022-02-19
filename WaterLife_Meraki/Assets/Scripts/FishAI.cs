using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float minSpeed, maxSpeed;
    public Canvas1 canvas;

    float speed;

    Vector3 nextWaypoint;

    bool movingToWaypoint;
    bool arrived; //Variável para determinar se o peixe chegou no waypoint, usando collider trigger
    bool dead;

    // Start is called before the first frame update
    void Start()
    {
        movingToWaypoint = false;
        arrived = false;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            RandomWaypoint();

            //Deslocamento do peixe
            transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, speed * Time.deltaTime);
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (arrived)
                {
                    movingToWaypoint = false;
                }
                else
                    movingToWaypoint = true;

            }

            // Certifica que rotaciona o NPC para a direção do waypoint
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nextWaypoint - transform.position), Time.deltaTime);
        }
        else 
        {
            if(gameObject.name == "Bagre" || gameObject.name == "Peixe Cirurgião-Paleta")
                GetComponent<Animation>().enabled = false;
            if(gameObject.name == "Peixe-Palhaço")
                GetComponentInChildren<Animator>().enabled = false;
            if(gameObject.name == "Guaiuba prefab" || gameObject.name == "Raia prefab" || gameObject.name == "Raias prefab"
                || gameObject.name == "Peixe-trombeta")
                GetComponentInChildren<Animation>().enabled = false;

            transform.rotation = Quaternion.Euler(transform.rotation.x,transform.rotation.y,180);
            transform.Translate(Vector3.down * 1.0f * Time.deltaTime);
            Destroy(gameObject, 3.0f);
        }
        
    }

    // Método para pegar um Waypoint aleatório
    Vector3 RandomWaypoint()
    {
        int randomWP = Random.Range(0, (waypoints.Length - 1));
        Vector3 randomWaypoint = waypoints[randomWP].transform.position;
        if (!movingToWaypoint) 
        {
            speed = Random.Range(minSpeed,maxSpeed);
            nextWaypoint = randomWaypoint;
        }
        return nextWaypoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "waypoint") 
        {
            arrived = true;
        }

        if (other.gameObject.tag == "Tiro")
        {
            dead = true;
            Canvas1.score -= 200;
            canvas.PerdePontos();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "waypoint")
        {
            arrived = false;
        }
    }
}
