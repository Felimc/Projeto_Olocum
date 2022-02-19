using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLogs : MonoBehaviour
{
    public AudioClip audioIntroducao;
    
    AudioSource au;
    
    static int introducao; // Variável para permitir tocar o áudio introdutório
    
    // Start is called before the first frame update
    void Start()
    {
        au = GetComponent<AudioSource>();
        if (introducao <=0 )
            StartCoroutine(Introducao());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Introducao() 
    {
        yield return new WaitForSeconds(0.001f);
        au.clip = audioIntroducao;
        au.Play();
        introducao++;
    }
}
