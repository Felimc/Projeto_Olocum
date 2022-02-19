using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morte_tartarugas : MonoBehaviour
{
    bool dead;

    // Start is called before the first frame update
    void Start()
    {
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) 
        {
            GetComponent<Animation>().enabled = false;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
            if (transform.position.y < 42.8f)
                transform.Translate(Vector3.down * 1.0f * Time.deltaTime);
            Destroy(gameObject, 3.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tiro")
        {
            dead = true;
            Canvas1.score -= 200;
            print("morto");
        }
    }
}
