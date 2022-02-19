using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public Button pularBtn;
    public AudioClip somBotao;

    AudioSource au;
    
    // Start is called before the first frame update
    void Start()
    {
        au = GetComponent<AudioSource>();
        
        //Configurando botão para pular vídeo de introdução
        Button btnPular = pularBtn.GetComponent<Button>();
        btnPular.onClick.AddListener(Pular);

        //Corrotina para carregar menu assim que o vídeo terminar
        StartCoroutine(FimVideo());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Pular();
        }
    }

    void Pular() 
    {
        au.clip = somBotao;
        au.Play();
        SceneManager.LoadScene("Menu Inicial");
    }

    IEnumerator FimVideo() 
    {
        yield return new WaitForSeconds(69f);
        SceneManager.LoadScene("Menu Inicial");
    }

}
