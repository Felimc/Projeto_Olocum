using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TartarugaAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float minSpeed, maxSpeed;
    public Canvas1 canvas;

    float speed;

    Vector3 nextWaypoint;

    bool movingToWaypoint;
    bool arrived; //Variável para determinar se a tartaruga chegou no waypoint, usando collider trigger
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

            //Deslocamento da tartaruga 
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
            GetComponentInChildren<Animation>().enabled = false;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
            if (transform.position.y < 42.8f)
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
            speed = Random.Range(minSpeed, maxSpeed);
            nextWaypoint = randomWaypoint;
        }
        return nextWaypoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "waypoint")
        {
            if (other.gameObject.name != "Cube (5)" && other.gameObject.name != "Cube (6)")
                arrived = true;
            else 
            {
                GetComponentInChildren<Animation>().enabled = false;
                StartCoroutine(Parada());
                IEnumerator Parada() 
                {
                    yield return new WaitForSeconds(35f);
                    arrived = true;
                    GetComponentInChildren<Animation>().enabled = true;
                }
            }

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
