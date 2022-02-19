using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint_Morbilus : MonoBehaviour
{
    public static bool posicaoValida;

    // Start is called before the first frame update
    void Start()
    {
        posicaoValida = false;
    }

    // Update is called once per frame
    void Update()
    {
        RandomWaypoint();
    }

    //M�todo para o waypoint pegar um ponto alet�rio, desde que esteja numa �rea v�lida
    void RandomWaypoint() 
    {
        if (!posicaoValida) 
        {
            float position_x = Random.Range(-300, 300);
            float position_y = Random.Range(-50, 28);
            float position_z = Random.Range(-300, 300);
            transform.position = new Vector3(position_x,position_y,position_z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "�rea do Morbilus") 
        {
            posicaoValida = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "�rea do Morbilus")
        {
            posicaoValida = false;
        }
    }
}
