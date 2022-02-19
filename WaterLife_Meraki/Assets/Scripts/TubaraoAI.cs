using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubaraoAI : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform player;
    public Transform cabecaTubarao;
    public float minSpeed, maxSpeed, persuitSpeed;
    public Canvas1 canvas;

    Animation anim;

    float speed;
    float tempoParado;
    float distanciaDoJogador; //Variável que determina uma distância mínima para começar a perseguir o jogador

    int energia;

    Vector3 nextWaypoint;

    bool movingToWaypoint;
    bool arrived; //Variável para determinar se o tubarão chegou no waypoint, usando collider trigger
    bool stateChosen; // Variável para determinar se já foi escolhido um estado 
    bool atacouJogador;
    bool dead;

    int randomState, lastState; //Variáveis para determinar se o peixe usa os waypoints dentro ou fora da água

    // Start is called before the first frame update
    void Start()
    {
        movingToWaypoint = false;
        arrived = false;
        stateChosen = false;
        atacouJogador = false;
        dead = false;

        anim = GetComponent<Animation>();

        energia = 10;
    }

    // Update is called once per frame
    void Update()
    {
        //Condição para morte do tubarão
        if (energia <= 0)
            dead = true;

        //Determinando a distância entre o jogador e o tubarão
        distanciaDoJogador = Vector3.Distance(transform.position, player.position);

        if (!dead)
        {
            // Se o tubarão não estiver no estado de perseguir o jogador, escolhe um dos outros estados
            if (Player.naAreaDoTubarao && distanciaDoJogador <= 50f && !atacouJogador)
            {
                Perseguindo();
            }
            else
            {
                // Primeiro escolhe um estado para o Tubarão
                if (!stateChosen)
                {
                    RandomState();
                    stateChosen = true;
                }
                else
                {
                    //Depois executa a ação de acordo com o estado escolhido
                    if (randomState > 2)
                    {
                        RandomWaypoint();
                        Movendo();
                    }
                    else
                    {
                        Parado();
                    }
                }
            }
        }
        else 
        {
            Canvas1.score -= 200;
            canvas.PerdePontos();
            GetComponent<Animation>().enabled = false;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
            transform.Translate(Vector3.down * 1.0f * Time.deltaTime);
            Destroy(gameObject, 3.0f);
        }
    }

    void Movendo() 
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

    void Parado() 
    {
        tempoParado -= 1 * Time.deltaTime;
        transform.Translate(Vector3.zero);
        anim.enabled = false;
        // Se acabar o tempo do tubarão ficar parado, ele volta a se mover para um waypoint
        if (tempoParado <= 0) 
        {
            stateChosen = false;
            anim.enabled = true;
        }
    }

    void Perseguindo() 
    {
        // Se move em direção ao jogador
        transform.position = Vector3.MoveTowards(transform.position, player.position, persuitSpeed * Time.deltaTime);
        // Certifica que rotaciona o NPC para a direção do waypoint
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - cabecaTubarao.position), Time.deltaTime);
    }

    //O tubarão tem 1/3 de chance de ficar parado por um tempo
    void RandomState()
    {
        lastState = randomState;
        randomState = Random.Range(1, 10);
        // Condição para evitar que a Tainha pegue dois waypoints fora da água
        while (lastState < 4 && randomState < 3)
            randomState = Random.Range(1, 10);

        // Se escolher um estado em que o tubarão fica parado, coloca um valor para o timer tempoParado
        if (randomState < 3)
            tempoParado = 10f;
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

    private void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.tag == "Player")
        {
            atacouJogador = true;
            StartCoroutine(VoltaPerseguicao());
            IEnumerator VoltaPerseguicao() 
            {
                yield return new WaitForSeconds(30.0f);
                atacouJogador = false;
            }
        }
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
            energia--;
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
