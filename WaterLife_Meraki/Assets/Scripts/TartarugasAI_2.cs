using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TartarugasAI_2 : MonoBehaviour
{
    public Transform[] waypoints;
    public GameObject tartaruguinha1, tartaruguinha2, tartaruguinha3, tartaruguinha4, tartaruguinha5;
    public float minSpeed, maxSpeed;
    public Canvas1 canvas;

    float speed;
    int waypointNumero;
    bool dead;
    bool desativaColliders;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointNumero].position, speed * Time.deltaTime);
            // Certifica que rotaciona o NPC para a direção do waypoint
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(waypoints[waypointNumero].position - transform.position), Time.deltaTime);
        }
        else 
        {
            GetComponentInChildren<Animation>().enabled = false;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
            if (transform.position.y < 42.8f)
                transform.Translate(Vector3.down * 1.0f * Time.deltaTime);
            Destroy(gameObject, 3.0f);
        }

        if (desativaColliders)
        {
            GetComponentInChildren<Collider>().enabled = false;
            tartaruguinha1.GetComponentInChildren<Collider>().enabled = false;
            tartaruguinha2.GetComponentInChildren<Collider>().enabled = false;
            tartaruguinha3.GetComponentInChildren<Collider>().enabled = false;
            tartaruguinha4.GetComponentInChildren<Collider>().enabled = false;
            tartaruguinha5.GetComponentInChildren<Collider>().enabled = false;
        }
        else 
        {
            GetComponentInChildren<Collider>().enabled = true;
            tartaruguinha1.GetComponentInChildren<Collider>().enabled = true;
            tartaruguinha2.GetComponentInChildren<Collider>().enabled = true;
            tartaruguinha3.GetComponentInChildren<Collider>().enabled = true;
            tartaruguinha4.GetComponentInChildren<Collider>().enabled = true;
            tartaruguinha5.GetComponentInChildren<Collider>().enabled = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "waypoint")
        {
            waypointNumero++;
            speed = Random.Range(minSpeed, maxSpeed);
            if (waypointNumero > 10)
                waypointNumero = 0;
            desativaColliders = true;
            StartCoroutine(LigaCollider());
            IEnumerator LigaCollider() 
            {
                yield return new WaitForSeconds(2.0f);
                desativaColliders = false;
            }
        }

        if (other.gameObject.tag == "Tiro")
        {
            dead = true;
            Canvas1.score -= 200;
            canvas.PerdePontos();
        }
    }

}
