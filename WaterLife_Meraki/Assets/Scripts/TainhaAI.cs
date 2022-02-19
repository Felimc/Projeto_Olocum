using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TainhaAI : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform[] waypointsOut;
    public float minSpeed, maxSpeed;
    public Canvas1 canvas;

    float speed;

    public float speed_2; // Variávle da velocidade da Tainha quando ela pula para fora da água
    int randomState, lastState; //Variáveis para determinar se o peixe usa os waypoints dentro ou fora da água

    Vector3 nextWaypoint;

    bool movingToWaypoint;
    bool arrived; //Variável para determinar se o peixe chegou no waypoint, usando collider trigger
    bool stateChosen; // Variável para determinar se já foi escolhido estado da Tainha
    bool dead;

    // Start is called before the first frame update
    void Start()
    {
        movingToWaypoint = false;
        arrived = false;
        stateChosen = false;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            // Primeiro escolhe um estado para a Tainha
            if (!stateChosen)
            {
                RandomState();
                stateChosen = true;
            }
            else
            {
                //Depois executa a ação de acordo com o estado escolhido
                if (randomState > 3)
                {
                    RandomWaypoint();
                    DentroDagua();
                }
                else
                {
                    RandomWaypoint2();
                    ForaDagua();
                }
            }
        }
        else 
        {
            GetComponentInChildren<Animation>().enabled = false;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
            transform.Translate(Vector3.down * 1.0f * Time.deltaTime);
            Destroy(gameObject, 3.0f);
        }

    }

    void DentroDagua()
    {
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

    void ForaDagua() 
    {
        //Deslocamento do peixe
        transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, speed_2 * Time.deltaTime);
        for (int i = 0; i < waypointsOut.Length; i++)
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

    //A Tainha tem 1/3 de chance de pular para fora da água caso esteja dentro da água
    void RandomState() 
    {
        lastState = randomState;
        randomState = Random.Range(1, 10);
        // Condição para evitar que a Tainha pegue dois waypoints fora da água
        while (lastState < 4 && randomState < 4)
            randomState = Random.Range(1, 10);
    }

    // Método para pegar um Waypoint aleatório dentro da água
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

    // Método para pegar um Waypoint aleatório fora da água
    Vector3 RandomWaypoint2()
    {
        int randomWP = Random.Range(0, (waypointsOut.Length - 1));
        Vector3 randomWaypoint = waypointsOut[randomWP].transform.position;
        if (!movingToWaypoint)
        {
            nextWaypoint = randomWaypoint;
        }
        return nextWaypoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "waypoint")
        {
            arrived = true;
            stateChosen = false;
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
