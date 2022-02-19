using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Morbilus : MonoBehaviour
{
    public Transform waypoint;
    public Transform hector;
    public Transform poisonSpawn;
    public GameObject olho, olho1, olho2, olho3, olho4, olho5;
    public GameObject poison;
    public GameObject explosaoVFX;
    public GameObject hand_L;
    public AudioClip monstro_1, monstro_2, monstro_3, monstro_morrendo;
    public float speed;

    public static bool rugir;
    public static bool areaAtaque1;
    public static bool areaAtaque2;
    public static bool morte;

    public static int energia = 60;
    int randomAttack;

    public static bool ataque1;
    bool ataque2;

    Animator anim;
    AudioSource au;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider>().enabled = true;
        GetComponentInChildren<Collider>().enabled = true;

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        au = new AudioSource();
        au = gameObject.AddComponent<AudioSource>();

        rugir = true;
        areaAtaque1 = false;
        areaAtaque2 = false;
        ataque1 = false;
        ataque2 = false;
        morte = false;

        hand_L.GetComponent<Collider>().enabled = false;

        explosaoVFX.SetActive(false);

        energia = 60; //Precisa de 5 tiros para tirar 1 barra de energia do Morbilus

        StartCoroutine(SomRugido());
        StartCoroutine(SomBaforada());
        StartCoroutine(SomBracada());
        StartCoroutine(SomMorte());
        StartCoroutine(Morte());
    }

    // Update is called once per frame
    void Update()
    {
        if (energia > 0)
        {
            if (!Player.sendoPerseguido)
            {
                agent.speed = 5;
                //Certificando que não mantém as animações de ataque
                ataque1 = false;
                ataque2 = false;
                anim.SetBool("mAttack2", false);

                //Desligando luz dos olhos caso não esteja perseguinhdo o jogador
                olho.SetActive(false);
                olho1.SetActive(false);
                olho2.SetActive(false);
                olho3.SetActive(false);
                olho4.SetActive(false);
                olho5.SetActive(false);

                //Deslocamento e rotação do Morbilus 
                if (Waypoint_Morbilus.posicaoValida)
                {
                    agent.destination = waypoint.position;
                    anim.SetBool("mMove", true);
                }
            }
            else
            {
                if (!ataque1 && !ataque2)
                    Perseguicao();
                Ataques();
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(hector.position - transform.position), Time.deltaTime);
            }

            Animations();
        }
        else //Sequência do monstro quando ele morre 
        {
            morte = true;
            anim.SetBool("mMove", false);
            anim.SetBool("mAttack", false);
            anim.SetBool("mRoar", false);
            anim.SetBool("mAttack2", true);
            olho.SetActive(false);
            olho1.SetActive(false);
            olho2.SetActive(false);
            olho3.SetActive(false);
            olho4.SetActive(false);
            olho5.SetActive(false);
            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<Collider>().enabled = false;
        }
       
    }

    //Estado em que o Morbilus começa a perseguir o jogador
    void Perseguicao()
    {
        agent.destination = hector.position;
        agent.speed = 7;
        anim.SetBool("mMove", true);
        olho.SetActive(true);
        olho1.SetActive(true);
        olho2.SetActive(true);
        olho3.SetActive(true);
        olho4.SetActive(true);
        olho5.SetActive(true);
    }

    void Animations()
    {
        //Animação de rugido assim que o monstro vê o jogador
        if (Player.sendoPerseguido && rugir)
        {
            anim.SetBool("mRoar", true);
            StartCoroutine(FimRugido());
            IEnumerator FimRugido()
            {
                yield return new WaitForSeconds(2.5f);
                anim.SetBool("mRoar", false);
                rugir = false;
            }
        }

        //Animação do ataque da braçada, quando o jogador está bem próximo do monstro
        anim.SetBool("mAttack", ataque1);

        //Animação da baforada, quando o jogador está numa média apra curta distância do monstro
        anim.SetBool("mAttack2", ataque2);
    }

    void Ataques()
    {
        if (areaAtaque1)
        {
            ataque1 = true;
        }
        else
        {
            ataque1 = false;
        }


        if (areaAtaque2)
        {
            randomAttack = Random.Range(1, 4);
            if (randomAttack > 2)
            {
                ataque2 = true;
            }

        }
        else
            ataque2 = false;
    }

    void Baforada()
    {
        if (!morte) 
        {
            GameObject baforada;
            baforada = Instantiate(poison, poisonSpawn.position, Quaternion.identity) as GameObject;
            Destroy(baforada, 4f);
        }
    }

    void Hand_L_Collider_On()
    {
        hand_L.GetComponent<Collider>().enabled = true;
    }

    void Hand_L_Collider_Off()
    {
        hand_L.GetComponent<Collider>().enabled = false;
    }

    IEnumerator Morte() 
    {
        yield return new WaitForSeconds(0.001f);
        if (morte) 
        {
            yield return new WaitForSeconds(2.3f);
            anim.SetBool("mAttack2", false);
            explosaoVFX.SetActive(true);
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
        StartCoroutine(Morte());
    }

    IEnumerator SomRugido() 
    {
        yield return new WaitForSeconds(0.001f);
        if (anim.GetBool("mRoar") && !morte) 
        {
            au.clip = monstro_1;
            au.Play();
            yield return new WaitForSeconds(8.5f);
        }
        StartCoroutine(SomRugido());
    }

    IEnumerator SomBaforada()
    {
        yield return new WaitForSeconds(0.001f);
        if (anim.GetBool("mAttack2") && !morte)
        {
            au.clip = monstro_3;
            au.Play();
            yield return new WaitForSeconds(14.5f);
        }
        StartCoroutine(SomBaforada());
        
    }

    IEnumerator SomBracada()
    {
        yield return new WaitForSeconds(0.001f);
        if (anim.GetBool("mAttack"))
        {
            au.clip = monstro_2;
            au.Play();
            yield return new WaitForSeconds(8.1f);
        }
        StartCoroutine(SomBracada());
    }

    IEnumerator SomMorte()
    {
        yield return new WaitForSeconds(0.001f);
        if (energia <= 0)
        {
            au.clip = monstro_morrendo;
            au.Play();
            yield break;
        }
        StartCoroutine(SomMorte());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Morbilus Waypoint") 
        {
            Waypoint_Morbilus.posicaoValida = false;
        }

        if (other.gameObject.tag == "Tiro" && !morte) 
        {
            energia--;
            if(morte)
                Canvas1.score += 5000;
            gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.red);
            StartCoroutine(VoltarCorNormal());
            Player.sendoPerseguido = true;
            Destroy(other.gameObject);
            IEnumerator VoltarCorNormal() 
            {
                yield return new WaitForSeconds(0.6f);
                gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.white);
            }
        }
    }
}
