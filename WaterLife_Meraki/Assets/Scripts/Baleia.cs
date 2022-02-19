using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baleia : MonoBehaviour
{
    public Transform waypoint;
    public GameObject mesh;
    public float speed;

    bool podeMovimentar;

    Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        podeMovimentar = false;
        anim = GetComponentInChildren<Animation>();
        anim.enabled = false;

        //Corrotina para fazer a baleia se mover ap�s o jogador v�-la na �rea secreta
        StartCoroutine(PodeMovimentar());
    }

    // Update is called once per frame
    void Update()
    {

        //Anima��o e movimento da baleia � acionada assim que o jogador entra na �rea secreta
        if (podeMovimentar) 
        {
            // Certifica que rotaciona o NPC para a dire��o do waypoint
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(waypoint.position - transform.position), Time.deltaTime);//Deslocamento do peixe
            transform.position = Vector3.MoveTowards(transform.position, waypoint.position, speed * Time.deltaTime);
            anim.enabled = true;
        }
    }

    IEnumerator PodeMovimentar() 
    {
        yield return new WaitForSeconds(0.001f);
        if (Player.limiteMapa)
        {
            yield return new WaitForSeconds(2.6f);
            podeMovimentar = true;
            yield break;
        }
        StartCoroutine(PodeMovimentar());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "waypoint") 
        {
            mesh.GetComponent<SkinnedMeshRenderer>().enabled = false;
            anim.enabled = false;
        }
    }
}
