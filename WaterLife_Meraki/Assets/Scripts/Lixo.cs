using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lixo : MonoBehaviour
{
    public Transform player;
    
    float speed = 10.0f;
    bool detectado;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        if (detectado) 
        {
            // Leva lixos não metálicos até o jogador 
            if (Player.lixoNaoMetalico && gameObject.tag == "Lixo Não Metálico")
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }

            // Leva lixos metálicos até o jogador 
            if (Player.lixoMetalico && gameObject.tag == "Lixo Metálico")
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            if (gameObject.tag == "Lixo Não Metálico")
            {
                detectado = true;
            }

            else if (gameObject.tag == "Lixo Metálico") 
            {
                detectado = true;
            }
        }

        if (other.gameObject.tag == "Morbilus") 
        {
            StartCoroutine(VoltaCollider());
            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<Collider>().enabled = false;
            IEnumerator VoltaCollider() 
            {
                yield return new WaitForSeconds(5.0f);
                GetComponent<Collider>().enabled = true;
                GetComponentInChildren<Collider>().enabled = true;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Lixo Não Metálico")
                detectado = false;
            else if (gameObject.tag == "Lixo Metálico")
                detectado = false;
        }

    }

    private void OnCollisionEnter(Collision c)
    {
        
    }
}
