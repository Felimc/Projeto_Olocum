using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    // Variável para o script AISpawner manager
    private AISpawner m_AIManager;

    // Variáveis para movimentação e mudar de direção
    private bool m_hasTarget = false;

    // Variáveis para o atual waypoint
    private Vector3 m_wayPoint;
    private Vector3 m_lastWaypoint;

    // Variáveis para configurar a velocidade de animação
    private Animator m_animator;
    private float m_speed;


    private Collider m_collider;

    public bool useRandomTarget;

    // Start is called before the first frame update
    void Start()
    {
        m_AIManager = transform.parent.GetComponentInParent<AISpawner>();
        m_animator = GetComponent<Animator>();

        //SetUpNPC();
    }

    void SetUpNPC() 
    {
        // Dimensiona o tamanho de cada NPC aleatoriamente
        float m_scale = Random.Range(0f, 1.25f);
        transform.localScale += new Vector3(m_scale * 1.5f, m_scale, m_scale);

        if (transform.GetComponent<Collider>() != null && transform.GetComponent<Collider>().enabled == true)
        {
            m_collider = transform.GetComponent<Collider>();
        }
        else if (transform.GetComponentInChildren<Collider>() != null && transform.GetComponentInChildren<Collider>().enabled == true) 
        {
            m_collider = transform.GetComponentInChildren<Collider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_hasTarget)
        {
            m_hasTarget = CanFindTarget();
        }
        else 
        {
            // Certifica que rotaciona o NPC para a direção do seu waypoint
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_wayPoint - transform.position), Time.deltaTime);
            // Movimenta o NPC numa linha reta em direção ao waypoint
            transform.position = Vector3.MoveTowards(transform.position, m_wayPoint, m_speed * Time.deltaTime);

            // Checa se há colisão
            ColliderNPC();
        }

        // Condição em que se o NPC atingir o waypoint reseta o alvo
        if (transform.position == m_wayPoint) 
        {
            m_hasTarget = false;
        }
    }

    //Método para mudar a direção do NPC se ele colidir com algo
    void ColliderNPC() 
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, transform.localScale.z)) 
        {
            // Se o collider atinge um waypoint ou registra ele mesmo ignora o raycast hit
            if (hit.collider == m_collider || hit.collider.tag == "waypoint") 
            {
                return;
            }
            //Se não, tem uma chance aleatória do NPC mude de direção
            int randomNum = Random.Range(1, 100);
            if (randomNum < 40)
                m_hasTarget = false;
        }
    }

    //Pega o waypoint
    Vector3 GetWaypoint(bool isRandom) 
    {
        //se isRandom for verdadeiro, então pega uma posição aleatória
        if (isRandom)
        {
            return m_AIManager.RandomPosition();
        }
        else 
        {
            return m_AIManager.RandomWaypoint();
        }
    }

    bool CanFindTarget(float start = 1f, float end = 7f) 
    {
        m_wayPoint = m_AIManager.RandomWaypoint();
        // Certifica que não usamos o mesmo waypoint duas vezes
        if (m_lastWaypoint == m_wayPoint)
        {
            // Pega um novo waypoint
            m_wayPoint = GetWaypoint(true);
            return false;
        }
        else 
        {
            // Deixa o novo waypoint como o último waypoint
            m_lastWaypoint = m_wayPoint;
            // Pega uma velocidade aleatória para o movimento e a animação
            m_speed = Random.Range(start, end);
            m_animator.speed = m_speed;
            // Deixa o bool como verdadeiro para dizer que achou o waypoint
            return true;
        }
    }

}
