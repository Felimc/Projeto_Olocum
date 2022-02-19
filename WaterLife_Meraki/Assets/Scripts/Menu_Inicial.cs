using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_Inicial : MonoBehaviour
{
    public Button jogarBtn, controlesBtn, voltarBtn, sairBtn;
    public AudioClip somBotao, somBotao2;
    public Image telaControles;

    AudioSource au;

    bool controlesClicado;

    // Start is called before the first frame update
    void Start()
    {
        au = GetComponent<AudioSource>();

        controlesClicado = false;

        Button jogar = jogarBtn.GetComponent<Button>();
        Button sair = sairBtn.GetComponent<Button>();
        Button controles = controlesBtn.GetComponent<Button>();
        Button voltar = voltarBtn.GetComponent<Button>();
        jogar.onClick.AddListener(LoadGame);
        sair.onClick.AddListener(Sair);
        controles.onClick.AddListener(Controles);
        voltar.onClick.AddListener(Voltar);
    }

    // Update is called once per frame
    void Update()
    {
        Button jogar = jogarBtn.GetComponent<Button>();
        Button sair = sairBtn.GetComponent<Button>();
        Button controles = controlesBtn.GetComponent<Button>();
        Button voltar = voltarBtn.GetComponent<Button>();

        if (!controlesClicado)
        {
            jogar.enabled = true;
            jogar.image.enabled = true;
            controles.enabled = true;
            controles.image.enabled = true;
            sair.enabled = true;
            sair.image.enabled = true;
            voltar.enabled = false;
            voltar.image.enabled = false;

            telaControles.enabled = false;
        }
        else 
        {
            jogar.enabled = false;
            jogar.image.enabled = false;
            controles.enabled = false;
            controles.image.enabled = false;
            sair.enabled = false;
            sair.image.enabled = false;
            voltar.enabled = true;
            voltar.image.enabled = true;

            telaControles.enabled = true;
        }
    }

    //Carrega o jogo principal
    void LoadGame() 
    {
        au.clip = somBotao;
        au.Play();
        SceneManager.LoadScene("Game");
    }

    void Sair() 
    {
        Application.Quit();
    }

    void Controles() 
    {
        controlesClicado = true;
        au.clip = somBotao2;
        au.Play();
    }

    void Voltar() 
    {
        controlesClicado = false;
        au.clip = somBotao2;
        au.Play();
    }

}
