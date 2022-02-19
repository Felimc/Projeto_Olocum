using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Canvas1 : MonoBehaviour
{
    public Image neblina;
    //Imagens da UI
    public Image lanternaDesligadaImg, lanternaLigadaImg, barraVida1, barraVida2, barraVida3, barraVida4, barraVida5, barraVida6,barraVida7, 
                 barraVida8, barraVida9, barraVida10, bateriaCheia, bateria5, bateria4, bateria3, bateria2, bateria1, bateriaVazia, imaImg, 
                 sugadorImg, barraMorbilus1, barraMorbilus2, barraMorbilus3, barraMorbilus4, barraMorbilus5, barraMorbilus6,
                 barraMorbilus7, barraMorbilus8, barraMorbilus9, barraMorbilus10, barraMorbilus11, barraMorbilus12, controlesImg, telaVitoriaImg,
                 telaVitoriaImg2;

    public Button controlesBtn, sairBtn, voltarBtn, retornarBtn;

    public Text pontuacaoTxt, pontosNumeroTxt, morbilusTxt, pausaTxt, gameOverTxt;

    public static int score;

    public AudioClip somScorePlus, somNegativo;

    AudioSource au;
    float alphaNeblina = 0;

    // Start is called before the first frame update
    void Start()
    {
        au = GetComponent<AudioSource>();
        score = 0;
        morbilusTxt.enabled = false;
        pausaTxt.enabled = false;
        gameOverTxt.enabled = false;
        controlesImg.enabled = false;

        telaVitoriaImg.enabled = false;
        telaVitoriaImg2.enabled = false;

        //Configurando os botões
        Button btnControles = controlesBtn.GetComponent<Button>();
        Button btnSair = sairBtn.GetComponent<Button>();
        Button btnVoltar = voltarBtn.GetComponent<Button>();
        Button btnRetornar = retornarBtn.GetComponent<Button>();

        btnControles.onClick.AddListener(Controles);
        btnSair.onClick.AddListener(Sair);
        btnVoltar.onClick.AddListener(Voltar);
        btnRetornar.onClick.AddListener(Retornar);

        btnControles.enabled = false;
        btnControles.image.enabled = false;
        btnSair.enabled = false;
        btnSair.image.enabled = false;
        btnVoltar.enabled = false;
        btnVoltar.image.enabled = false;
        btnRetornar.enabled = false;
        btnRetornar.image.enabled = false;

        // Corrotina para criar neblina caso o jogador ache o local da baleia, para evitar q ele continue em região fora do mapa
        StartCoroutine(Neblina());
    }

    // Update is called once per frame
    void Update()
    {
        //Configurando os botões
        Button btnControles = controlesBtn.GetComponent<Button>();
        Button btnSair = sairBtn.GetComponent<Button>();
        Button btnVoltar = voltarBtn.GetComponent<Button>();
        Button btnRetornar = retornarBtn.GetComponent<Button>();

        //Código da Pausa
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            if (Time.timeScale > 0.0f)
            {
                Time.timeScale = 0.0f;
                pausaTxt.enabled = true;
                btnControles.enabled = true;
                btnControles.image.enabled = true;
                btnSair.enabled = true;
                btnSair.image.enabled = true;
                btnVoltar.enabled = true;
                btnVoltar.image.enabled = true;
                btnRetornar.enabled = false;
                btnRetornar.image.enabled = false;

                pontosNumeroTxt.enabled = false;
                pontuacaoTxt.enabled = false;
            }
            else 
            {
                Time.timeScale = 1.0f;
                pausaTxt.enabled = false;
                btnControles.enabled = false;
                btnControles.image.enabled = false;
                btnSair.enabled = false;
                btnSair.image.enabled = false;
                btnVoltar.enabled = false;
                btnVoltar.image.enabled = false;
                btnRetornar.enabled = false;
                btnRetornar.image.enabled = false;
                controlesImg.enabled = false;

                pontosNumeroTxt.enabled = true;
                pontuacaoTxt.enabled = true;
            }
        }

        pontosNumeroTxt.text = score.ToString();

        //Métodos das imagens da UI/HUD
        ImaSugadorImg();
        LanternasImg();
        BarrasEnergia();
        BateriaImg();
        MorbilusUI();
        if (Morbilus.morte) 
        {
            Vitorias();
        }

        if (Player.gameOver) 
        {
            gameOverTxt.enabled = true;
        }
    }


    //Método para determinar se o ícone do imã ou do sugador estarão ativos
    void ImaSugadorImg() 
    {
        if (Player.ima)
        {
            imaImg.enabled = true;
            sugadorImg.enabled = false;
        }
        else 
        {
            imaImg.enabled = false;
            sugadorImg.enabled = true;
        }
    }

    //Método para determinar imagem do robô conforme ele liga e desliga a lanterna
    void LanternasImg() 
    {
        if (Player.lanternasLigadas) 
        {
            lanternaLigadaImg.enabled = true;
            lanternaDesligadaImg.enabled = false;
        }
        else 
        {
            lanternaLigadaImg.enabled = false;
            lanternaDesligadaImg.enabled = true;
        }
    }

    //Método das barras de energia do jogador
    void BarrasEnergia() 
    {
        switch (Player.energia) 
        {
            case 0:
                barraVida1.enabled = false;
                barraVida2.enabled = false;
                barraVida3.enabled = false;
                barraVida4.enabled = false;
                barraVida5.enabled = false;
                barraVida6.enabled = false;
                barraVida7.enabled = false;
                barraVida8.enabled = false;
                barraVida9.enabled = false;
                barraVida10.enabled = false;
                break;
            case 1:
                barraVida1.enabled = true;
                barraVida2.enabled = false;
                barraVida3.enabled = false;
                barraVida4.enabled = false;
                barraVida5.enabled = false;
                barraVida6.enabled = false;
                barraVida7.enabled = false;
                barraVida8.enabled = false;
                barraVida9.enabled = false;
                barraVida10.enabled = false;
                break;
            case 2:
                barraVida1.enabled = true;
                barraVida2.enabled = true;
                barraVida3.enabled = false;
                barraVida4.enabled = false;
                barraVida5.enabled = false;
                barraVida6.enabled = false;
                barraVida7.enabled = false;
                barraVida8.enabled = false;
                barraVida9.enabled = false;
                barraVida10.enabled = false;
                break;
            case 3:
                barraVida1.enabled = true;
                barraVida2.enabled = true;
                barraVida3.enabled = true;
                barraVida4.enabled = false;
                barraVida5.enabled = false;
                barraVida6.enabled = false;
                barraVida7.enabled = false;
                barraVida8.enabled = false;
                barraVida9.enabled = false;
                barraVida10.enabled = false;
                break;
            case 4:
                barraVida1.enabled = true;
                barraVida2.enabled = true;
                barraVida3.enabled = true;
                barraVida4.enabled = true;
                barraVida5.enabled = false;
                barraVida6.enabled = false;
                barraVida7.enabled = false;
                barraVida8.enabled = false;
                barraVida9.enabled = false;
                barraVida10.enabled = false;
                break;
            case 5:
                barraVida1.enabled = true;
                barraVida2.enabled = true;
                barraVida3.enabled = true;
                barraVida4.enabled = true;
                barraVida5.enabled = true;
                barraVida6.enabled = false;
                barraVida7.enabled = false;
                barraVida8.enabled = false;
                barraVida9.enabled = false;
                barraVida10.enabled = false;
                break;
            case 6:
                barraVida1.enabled = true;
                barraVida2.enabled = true;
                barraVida3.enabled = true;
                barraVida4.enabled = true;
                barraVida5.enabled = true;
                barraVida6.enabled = true;
                barraVida7.enabled = false;
                barraVida8.enabled = false;
                barraVida9.enabled = false;
                barraVida10.enabled = false;
                break;
            case 7:
                barraVida1.enabled = true;
                barraVida2.enabled = true;
                barraVida3.enabled = true;
                barraVida4.enabled = true;
                barraVida5.enabled = true;
                barraVida6.enabled = true;
                barraVida7.enabled = true;
                barraVida8.enabled = false;
                barraVida9.enabled = false;
                barraVida10.enabled = false;
                break;
            case 8:
                barraVida1.enabled = true;
                barraVida2.enabled = true;
                barraVida3.enabled = true;
                barraVida4.enabled = true;
                barraVida5.enabled = true;
                barraVida6.enabled = true;
                barraVida7.enabled = true;
                barraVida8.enabled = true;
                barraVida9.enabled = false;
                barraVida10.enabled = false;
                break;
            case 9:
                barraVida1.enabled = true;
                barraVida2.enabled = true;
                barraVida3.enabled = true;
                barraVida4.enabled = true;
                barraVida5.enabled = true;
                barraVida6.enabled = true;
                barraVida7.enabled = true;
                barraVida8.enabled = true;
                barraVida9.enabled = true;
                barraVida10.enabled = false;
                break;
            case 10:
                barraVida1.enabled = true;
                barraVida2.enabled = true;
                barraVida3.enabled = true;
                barraVida4.enabled = true;
                barraVida5.enabled = true;
                barraVida6.enabled = true;
                barraVida7.enabled = true;
                barraVida8.enabled = true;
                barraVida9.enabled = true;
                barraVida10.enabled = true;
                break;
            default:
                print("Deu algum problema");
                break;
        }
    }

    //Método para ilustrar o estado da bateria do robô
    void BateriaImg() 
    {
        if (Player.bateria > 500 && Player.bateria <= 600) 
        {
            bateriaCheia.enabled = true;
            bateria5.enabled = false;
            bateria4.enabled = false;
            bateria3.enabled = false;
            bateria2.enabled = false;
            bateria1.enabled = false;
            bateriaVazia.enabled = false;
        }
        else if (Player.bateria > 400 && Player.bateria <= 500)
        {
            bateriaCheia.enabled = false;
            bateria5.enabled = true;
            bateria4.enabled = false;
            bateria3.enabled = false;
            bateria2.enabled = false;
            bateria1.enabled = false;
            bateriaVazia.enabled = false;
        }
        else if (Player.bateria > 300 && Player.bateria <= 400)
        {
            bateriaCheia.enabled = false;
            bateria5.enabled = false;
            bateria4.enabled = true;
            bateria3.enabled = false;
            bateria2.enabled = false;
            bateria1.enabled = false;
            bateriaVazia.enabled = false;
        }
        else if (Player.bateria > 200 && Player.bateria <= 300)
        {
            bateriaCheia.enabled = false;
            bateria5.enabled = false;
            bateria4.enabled = false;
            bateria3.enabled = true;
            bateria2.enabled = false;
            bateria1.enabled = false;
            bateriaVazia.enabled = false;
        }
        else if (Player.bateria > 100 && Player.bateria <= 200)
        {
            bateriaCheia.enabled = false;
            bateria5.enabled = false;
            bateria4.enabled = false;
            bateria3.enabled = false;
            bateria2.enabled = true;
            bateria1.enabled = false;
            bateriaVazia.enabled = false;
        }
        else if (Player.bateria > 0 && Player.bateria <= 100)
        {
            bateriaCheia.enabled = false;
            bateria5.enabled = false;
            bateria4.enabled = false;
            bateria3.enabled = false;
            bateria2.enabled = false;
            bateria1.enabled = true;
            bateriaVazia.enabled = false;
        }
        else if (Player.bateria <= 0)
        {
            bateriaCheia.enabled = false;
            bateria5.enabled = false;
            bateria4.enabled = false;
            bateria3.enabled = false;
            bateria2.enabled = false;
            bateria1.enabled = false;
            bateriaVazia.enabled = true;
        }
    }

    //Método para mostrar a energia do monstro quando o jogador o encontra
    void MorbilusUI() 
    {
        if (Player.sendoPerseguido)
        {
            morbilusTxt.enabled = true;
            if (Morbilus.energia <= 60 && Morbilus.energia > 55) 
            {
                barraMorbilus12.enabled = true;
                barraMorbilus11.enabled = true;
                barraMorbilus10.enabled = true;
                barraMorbilus9.enabled = true;
                barraMorbilus8.enabled = true;
                barraMorbilus7.enabled = true;
                barraMorbilus6.enabled = true;
                barraMorbilus5.enabled = true;
                barraMorbilus4.enabled = true;
                barraMorbilus3.enabled = true;
                barraMorbilus2.enabled = true;
                barraMorbilus1.enabled = true;
            }
            else if (Morbilus.energia <= 55 && Morbilus.energia > 50)
            {
                barraMorbilus12.enabled = false;
                barraMorbilus11.enabled = true;
                barraMorbilus10.enabled = true;
                barraMorbilus9.enabled = true;
                barraMorbilus8.enabled = true;
                barraMorbilus7.enabled = true;
                barraMorbilus6.enabled = true;
                barraMorbilus5.enabled = true;
                barraMorbilus4.enabled = true;
                barraMorbilus3.enabled = true;
                barraMorbilus2.enabled = true;
                barraMorbilus1.enabled = true;
            }
            else if (Morbilus.energia <= 50 && Morbilus.energia > 45)
            {
                barraMorbilus12.enabled = false;
                barraMorbilus11.enabled = false;
                barraMorbilus10.enabled = true;
                barraMorbilus9.enabled = true;
                barraMorbilus8.enabled = true;
                barraMorbilus7.enabled = true;
                barraMorbilus6.enabled = true;
                barraMorbilus5.enabled = true;
                barraMorbilus4.enabled = true;
                barraMorbilus3.enabled = true;
                barraMorbilus2.enabled = true;
                barraMorbilus1.enabled = true;
            }
            else if (Morbilus.energia <= 45 && Morbilus.energia > 40)
            {
                barraMorbilus12.enabled = false;
                barraMorbilus11.enabled = false;
                barraMorbilus10.enabled = false;
                barraMorbilus9.enabled = true;
                barraMorbilus8.enabled = true;
                barraMorbilus7.enabled = true;
                barraMorbilus6.enabled = true;
                barraMorbilus5.enabled = true;
                barraMorbilus4.enabled = true;
                barraMorbilus3.enabled = true;
                barraMorbilus2.enabled = true;
                barraMorbilus1.enabled = true;
            }
            else if (Morbilus.energia <= 40 && Morbilus.energia > 35)
            {
                barraMorbilus12.enabled = false;
                barraMorbilus11.enabled = false;
                barraMorbilus10.enabled = false;
                barraMorbilus9.enabled = false;
                barraMorbilus8.enabled = true;
                barraMorbilus7.enabled = true;
                barraMorbilus6.enabled = true;
                barraMorbilus5.enabled = true;
                barraMorbilus4.enabled = true;
                barraMorbilus3.enabled = true;
                barraMorbilus2.enabled = true;
                barraMorbilus1.enabled = true;
            }
            else if (Morbilus.energia <= 35 && Morbilus.energia > 30)
            {
                barraMorbilus12.enabled = false;
                barraMorbilus11.enabled = false;
                barraMorbilus10.enabled = false;
                barraMorbilus9.enabled = false;
                barraMorbilus8.enabled = false;
                barraMorbilus7.enabled = true;
                barraMorbilus6.enabled = true;
                barraMorbilus5.enabled = true;
                barraMorbilus4.enabled = true;
                barraMorbilus3.enabled = true;
                barraMorbilus2.enabled = true;
                barraMorbilus1.enabled = true;
            }
            else if (Morbilus.energia <= 30 && Morbilus.energia > 25)
            {
                barraMorbilus12.enabled = false;
                barraMorbilus11.enabled = false;
                barraMorbilus10.enabled = false;
                barraMorbilus9.enabled = false;
                barraMorbilus8.enabled = false;
                barraMorbilus7.enabled = false;
                barraMorbilus6.enabled = true;
                barraMorbilus5.enabled = true;
                barraMorbilus4.enabled = true;
                barraMorbilus3.enabled = true;
                barraMorbilus2.enabled = true;
                barraMorbilus1.enabled = true;
            }
            else if (Morbilus.energia <= 25 && Morbilus.energia > 20)
            {
                barraMorbilus12.enabled = false;
                barraMorbilus11.enabled = false;
                barraMorbilus10.enabled = false;
                barraMorbilus9.enabled = false;
                barraMorbilus8.enabled = false;
                barraMorbilus7.enabled = false;
                barraMorbilus6.enabled = false;
                barraMorbilus5.enabled = true;
                barraMorbilus4.enabled = true;
                barraMorbilus3.enabled = true;
                barraMorbilus2.enabled = true;
                barraMorbilus1.enabled = true;
            }
            else if (Morbilus.energia <= 20 && Morbilus.energia > 15)
            {
                barraMorbilus12.enabled = false;
                barraMorbilus11.enabled = false;
                barraMorbilus10.enabled = false;
                barraMorbilus9.enabled = false;
                barraMorbilus8.enabled = false;
                barraMorbilus7.enabled = false;
                barraMorbilus6.enabled = false;
                barraMorbilus5.enabled = false;
                barraMorbilus4.enabled = true;
                barraMorbilus3.enabled = true;
                barraMorbilus2.enabled = true;
                barraMorbilus1.enabled = true;
            }
            else if (Morbilus.energia <= 15 && Morbilus.energia > 10)
            {
                barraMorbilus12.enabled = false;
                barraMorbilus11.enabled = false;
                barraMorbilus10.enabled = false;
                barraMorbilus9.enabled = false;
                barraMorbilus8.enabled = false;
                barraMorbilus7.enabled = false;
                barraMorbilus6.enabled = false;
                barraMorbilus5.enabled = false;
                barraMorbilus4.enabled = false;
                barraMorbilus3.enabled = true;
                barraMorbilus2.enabled = true;
                barraMorbilus1.enabled = true;
            }
            else if (Morbilus.energia <= 10 && Morbilus.energia > 5)
            {
                barraMorbilus12.enabled = false;
                barraMorbilus11.enabled = false;
                barraMorbilus10.enabled = false;
                barraMorbilus9.enabled = false;
                barraMorbilus8.enabled = false;
                barraMorbilus7.enabled = false;
                barraMorbilus6.enabled = false;
                barraMorbilus5.enabled = false;
                barraMorbilus4.enabled = false;
                barraMorbilus3.enabled = false;
                barraMorbilus2.enabled = true;
                barraMorbilus1.enabled = true;
            }
            else if (Morbilus.energia <= 5 && Morbilus.energia > 0)
            {
                barraMorbilus12.enabled = false;
                barraMorbilus11.enabled = false;
                barraMorbilus10.enabled = false;
                barraMorbilus9.enabled = false;
                barraMorbilus8.enabled = false;
                barraMorbilus7.enabled = false;
                barraMorbilus6.enabled = false;
                barraMorbilus5.enabled = false;
                barraMorbilus4.enabled = false;
                barraMorbilus3.enabled = false;
                barraMorbilus2.enabled = false;
                barraMorbilus1.enabled = true;
            }
            else if (Morbilus.energia <= 0)
            {
                barraMorbilus12.enabled = false;
                barraMorbilus11.enabled = false;
                barraMorbilus10.enabled = false;
                barraMorbilus9.enabled = false;
                barraMorbilus8.enabled = false;
                barraMorbilus7.enabled = false;
                barraMorbilus6.enabled = false;
                barraMorbilus5.enabled = false;
                barraMorbilus4.enabled = false;
                barraMorbilus3.enabled = false;
                barraMorbilus2.enabled = false;
                barraMorbilus1.enabled = false;
            }
        }
        else 
        {
            morbilusTxt.enabled = false;
            barraMorbilus12.enabled = false;
            barraMorbilus11.enabled = false;
            barraMorbilus10.enabled = false;
            barraMorbilus9.enabled = false;
            barraMorbilus8.enabled = false;
            barraMorbilus7.enabled = false;
            barraMorbilus6.enabled = false;
            barraMorbilus5.enabled = false;
            barraMorbilus4.enabled = false;
            barraMorbilus3.enabled = false;
            barraMorbilus2.enabled = false;
            barraMorbilus1.enabled = false;
        }
    }

    //Métodos de efeitos quando o jogador perde ou ganha pontos
    public void GanhaPontos()
    {
        au.clip = somScorePlus;
        au.Play();
        StartCoroutine(PontuacaoEfeito());
        IEnumerator PontuacaoEfeito()
        {
            yield return new WaitForSeconds(0.15f);
            pontosNumeroTxt.color = Color.green;
            pontuacaoTxt.color = Color.green;
            pontosNumeroTxt.fontSize = 60;
            pontuacaoTxt.fontSize = 60;
            yield return new WaitForSeconds(0.15f);
            pontosNumeroTxt.fontSize = 70;
            pontuacaoTxt.fontSize = 70;
            yield return new WaitForSeconds(0.15f);
            pontosNumeroTxt.fontSize = 80;
            pontuacaoTxt.fontSize = 80;
            yield return new WaitForSeconds(0.15f);
            pontosNumeroTxt.fontSize = 70;
            pontuacaoTxt.fontSize = 70;
            yield return new WaitForSeconds(0.15f);
            pontosNumeroTxt.fontSize = 60;
            pontuacaoTxt.fontSize = 60;
            yield return new WaitForSeconds(0.15f);
            pontosNumeroTxt.color = new Color(1, 1, 1, 0.5607843f);
            pontuacaoTxt.color = new Color(1, 1, 1, 0.5607843f);
            pontosNumeroTxt.fontSize = 50;
            pontuacaoTxt.fontSize = 50;  
            
        }
    }

    public void PerdePontos() 
    {
        au.clip = somNegativo;
        au.Play();
        StartCoroutine(BaixaPontuacao());
        IEnumerator BaixaPontuacao() 
        {
            yield return new WaitForSeconds(0.15f);
            pontosNumeroTxt.color = Color.red;
            pontuacaoTxt.color = Color.red;
            pontosNumeroTxt.fontSize = 40;
            pontuacaoTxt.fontSize = 40;
            yield return new WaitForSeconds(0.15f);
            pontosNumeroTxt.fontSize = 30;
            pontuacaoTxt.fontSize = 30;
            yield return new WaitForSeconds(0.15f);
            pontosNumeroTxt.fontSize = 20;
            pontuacaoTxt.fontSize = 20;
            yield return new WaitForSeconds(0.15f);
            pontosNumeroTxt.fontSize = 30;
            pontuacaoTxt.fontSize = 30;
            yield return new WaitForSeconds(0.15f);
            pontosNumeroTxt.fontSize = 40;
            pontuacaoTxt.fontSize = 40;
            yield return new WaitForSeconds(0.15f);
            pontosNumeroTxt.color = new Color(1, 1, 1, 0.5607843f);
            pontuacaoTxt.color = new Color(1, 1, 1, 0.5607843f);
            pontosNumeroTxt.fontSize = 50;
            pontuacaoTxt.fontSize = 50;
        }
    }

    //Métodos dos botões
    void Controles() 
    {
        pausaTxt.enabled = false;
        controlesImg.enabled = true;

        //Configurando os botões
        Button btnControles = controlesBtn.GetComponent<Button>();
        Button btnSair = sairBtn.GetComponent<Button>();
        Button btnVoltar = voltarBtn.GetComponent<Button>();
        Button btnRetornar = retornarBtn.GetComponent<Button>();

        btnControles.enabled = false;
        btnControles.image.enabled = false;
        btnSair.enabled = false;
        btnSair.image.enabled = false;
        btnVoltar.enabled = false;
        btnVoltar.image.enabled = false;
        btnRetornar.enabled = true;
        btnRetornar.image.enabled = true;
    }

    void Sair() 
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu Inicial");
    }

    void Voltar() 
    {
        //Configurando os botões
        Button btnControles = controlesBtn.GetComponent<Button>();
        Button btnSair = sairBtn.GetComponent<Button>();
        Button btnVoltar = voltarBtn.GetComponent<Button>();
        Button btnRetornar = retornarBtn.GetComponent<Button>();

        pausaTxt.enabled = false;
        btnControles.enabled = false;
        btnControles.image.enabled = false;
        btnSair.enabled = false;
        btnSair.image.enabled = false;
        btnVoltar.enabled = false;
        btnVoltar.image.enabled = false;
        btnRetornar.enabled = false;
        btnRetornar.image.enabled = false;
        controlesImg.enabled = false;

        pontosNumeroTxt.enabled = true;
        pontuacaoTxt.enabled = true;

        Time.timeScale = 1.0f;
    }

    void Retornar() 
    {
        pausaTxt.enabled = true;
        controlesImg.enabled = false;

        //Configurando os botões
        Button btnControles = controlesBtn.GetComponent<Button>();
        Button btnSair = sairBtn.GetComponent<Button>();
        Button btnVoltar = voltarBtn.GetComponent<Button>();
        Button btnRetornar = retornarBtn.GetComponent<Button>();

        pausaTxt.enabled = true;
        btnControles.enabled = true;
        btnControles.image.enabled = true;
        btnSair.enabled = true;
        btnSair.image.enabled = true;
        btnVoltar.enabled = true;
        btnVoltar.image.enabled = true;
        btnRetornar.enabled = false;
        btnRetornar.image.enabled = false;
    }

    //Telass de quando o jogador vence o monstro
    void Vitorias() 
    {
        if (score > 0)
        {
            StartCoroutine(Vitoria1());
            IEnumerator Vitoria1()
            {
                yield return new WaitForSeconds(5.0f);
                telaVitoriaImg.enabled = true;
                yield return new WaitForSeconds(10.0f);
                SceneManager.LoadScene("Menu Inicial");
            }
        }
        else 
        {
            StartCoroutine(Vitoria2());
            IEnumerator Vitoria2()
            {
                yield return new WaitForSeconds(5.0f);
                telaVitoriaImg2.enabled = true;
                yield return new WaitForSeconds(10.0f);
                SceneManager.LoadScene("Menu Inicial");
            }
        }
    }

    //Controle de neblina quando o jogador sai dos limites do mapa
    IEnumerator Neblina() 
    {
        
        yield return new WaitForSeconds(0.001f);
        if (Player.limiteMapa)
        {
            yield return new WaitForSeconds(0.5f);
            alphaNeblina += 0.05f;
            neblina.color = new Color(0.4229f, 0.4654f, 0.5f, alphaNeblina);
        }
        else 
        {
            alphaNeblina = 0f;
            neblina.color = new Color(0.4229f, 0.4654f, 0.5f, 0.0f);
        }
        StartCoroutine(Neblina());
    }
}
