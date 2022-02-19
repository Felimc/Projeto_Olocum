using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baforada : MonoBehaviour
{
    public float speed;
    GameObject player;
    bool paraDeSeguir;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        paraDeSeguir = false;
        StartCoroutine(Para_Seguir());
    }

    // Update is called once per frame
    void Update()
    {
        //Deslocamento da baforada
        if (!paraDeSeguir) 
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), Time.deltaTime);
        }
        else
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

    }

    IEnumerator Para_Seguir() 
    {
        yield return new WaitForSeconds(2.25f);
        paraDeSeguir = true;
    }
}
