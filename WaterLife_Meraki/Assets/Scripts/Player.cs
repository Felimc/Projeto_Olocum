using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    public float speed;
    public float waterLevel;
    public float gravity;

    public GameObject bubblesVFX;
    public GameObject magnetismoVFX;
    public GameObject spotLight, spotLight2;
    public GameObject lightningBall;
    public GameObject sonarVFX;
    public GameObject damageVFX;
    public GameObject smokeVFX;
    public GameObject sugadorVFX;
    public GameObject bracoEsquerdo;
    public GameObject musicaDentroAgua; 
    public GameObject musicaForaAgua;
    public GameObject musicaChefe;

    public Canvas1 canvas;

    public AudioClip luzLiga, luzDesliga, somRobo, bateriaFraca, bateriaRecarregando, somBotao3;

    public static bool limiteMapa; // Variável para criar efeito de neblina no napa quando o jogador acha o local da baleia
    public static bool naAreaDoTubarao; // Variável para determinar se o jogador entrou na área do tubarão
    public static bool sendoPerseguido; //Variável para o monstro saber que o jogador entrou na sua zona de perseguição
    public static bool gameOver; //Variável ativada quando a energia chega a zero
    //Variáveis para detectar os lixos e usar no script "Lixo"
    public static bool lixoMetalico, lixoNaoMetalico;

    public static int energia = 10;
    public static float bateria = 600;

    Rigidbody rb;
    AudioSource au;
    AudioSource auMusica_1;
    AudioSource auMusica_2;

    public static bool lanternasLigadas = false;
    bool limiteTerreno; // Variável para evitar que o jogador suba sobre o terreno do jogo
    bool invunerabilidade; //Variável para deixar o jogador por um tempo invunerável após tomar um dano

    // Variáveis para determinar se o jogador usará o imã ou sugador
    public static bool ima;
    bool trasicaoMusicas;
    int cliquesImaSugador;

    int cliquesSonar;

    private float rotY = 0.0f; // rotação ao redor eixo Y
    private float rotX = 0.0f; // rotação ao redor do eixo X
    float InstantiationTimer;

    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        au = GetComponent<AudioSource>();

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

        lanternasLigadas = false;
        limiteTerreno = false;
        limiteMapa = false;
        naAreaDoTubarao = false;
        lixoMetalico = false;
        lixoNaoMetalico = false;
        ima = true;
        sendoPerseguido = false;
        invunerabilidade = false;
        trasicaoMusicas = false;
        gameOver = false;

        cliquesSonar = 0;
        energia = 10;
        bateria = 600;

        // Configurando para controlar as músicas fora e dentro da água
        auMusica_1 = musicaDentroAgua.GetComponent<AudioSource>();
        auMusica_2 = musicaForaAgua.GetComponent<AudioSource>();
        auMusica_2.volume = 0f;

        //Configurando música do chefe
        musicaChefe.SetActive(false);

        smokeVFX.SetActive(false);

        //Som do robô quando perde toda a energia
        StartCoroutine(SomDestruido());

        //Sons relacionados à bateria do robô
        StartCoroutine(SomBateriaFraca());
        StartCoroutine(SomBateriaRecarregando());

        //Corrotina do Game Over
        StartCoroutine(GameOver());
    }

    void Update()
    {
        if (energia <= 0) // Ações de Game Over
        {
            gameOver = true;
            energia = 0;
            anim.SetBool("mOff", true);
            smokeVFX.SetActive(true);
        }
        else 
        {
            if (bateria > 0)
            {
                Animation();

                //Trocar com a tecla Tab se o jogador poderá usar o imã ou o sugador
                if (Input.GetKeyDown(KeyCode.Tab)) 
                {
                    cliquesImaSugador++;
                    au.clip = somBotao3;
                    au.Play();
                }
                if (cliquesImaSugador % 2 == 0)
                    ima = true;
                else
                    ima = false;

                //Ativar e desativar lanterna dos olhos
                if (Input.GetKeyDown(KeyCode.Space) && !lanternasLigadas)
                {
                    spotLight.SetActive(true);
                    spotLight2.SetActive(true);
                    lanternasLigadas = true;
                    au.clip = luzLiga;
                    au.Play();
                }
                else if (Input.GetKeyDown(KeyCode.Space) && lanternasLigadas)
                {
                    spotLight.SetActive(false);
                    spotLight2.SetActive(false);
                    lanternasLigadas = false;
                    au.clip = luzDesliga;
                    au.Play();
                }

            }
            else 
            {
                anim.SetBool("mOff", true);
            }

        }
        
        // Configurando para controlar as músicas fora e dentro da água
        if (!sendoPerseguido && !trasicaoMusicas) 
        {
            if (transform.position.y < waterLevel)
            {
                auMusica_1.volume = 100f;
                auMusica_2.volume = 0f;
            }
            else
            {
                auMusica_1.volume = 0f;
                auMusica_2.volume = 100f;
            }
        }

        //Para música do chefe ao matá-lo e outras configurações
        if (Morbilus.morte) 
        {
            musicaChefe.SetActive(false);
            sendoPerseguido = false;
        }

        //Condição para recarregar a bateria: quando o robô sobe até a superfície
        if (transform.position.y >= waterLevel && bateria <= 600) 
        {
            bateria += 200 * Time.deltaTime;
            
        }
        //Outras configurações da bateria
        if (bateria > 600)
            bateria = 600;
        if (lanternasLigadas) 
        {
            bateria -= 2 * Time.deltaTime;
        }
        if (bateria <= 0) 
        {
            lanternasLigadas = false;
            spotLight.SetActive(false);
            spotLight2.SetActive(false);
            bubblesVFX.SetActive(false);
        }

    }

    void Animation() 
    {
        // Animações dos propulsores
        anim.SetFloat("mMoveY", Input.GetAxis("Vertical"));
        anim.SetFloat("mMoveX", Input.GetAxis("Horizontal"));

        //Deixa a animação desligada se estiver com energia e bateria maior que 0
        anim.SetBool("mOff", false);

        //Animação do gancho magnético 
        if (Input.GetButton("Fire2") && ima) 
        {
            anim.SetBool("mMagnet", true);
            StartCoroutine(AtivaMagnetismo());
            bateria -= 3 * Time.deltaTime;
            IEnumerator AtivaMagnetismo() 
            {
                yield return new WaitForSeconds(1.05f);
                if (anim.GetBool("mMagnet")) 
                {
                    magnetismoVFX.SetActive(true);
                    lixoMetalico = true;
                }           
            }
        }
        if (Input.GetButtonUp("Fire2")) 
        {
            anim.SetBool("mMagnet", false);
            magnetismoVFX.SetActive(false);
            lixoMetalico = false;
        }

        //Animação do sugador
        if (Input.GetButton("Fire2") && !ima)
        {
            anim.SetBool("mSuck", true);
            StartCoroutine(AtivaSugador());
            bateria -= 3 * Time.deltaTime;
            IEnumerator AtivaSugador()
            {
                yield return new WaitForSeconds(0.2f);
                if (anim.GetBool("mSuck")) 
                {
                    sugadorVFX.SetActive(true);
                    lixoNaoMetalico = true;
                }
            }
        }
        if (Input.GetButtonUp("Fire2"))
        {
            anim.SetBool("mSuck", false);
            sugadorVFX.SetActive(false);
            lixoNaoMetalico = false;
        }

        // Animação do tiro
        if (Input.GetButton("Fire1")) 
        {
            anim.SetBool("mShoot", true);
            InstantiationTimer -= Time.deltaTime;
            GameObject tiro;
            StartCoroutine(Atirar());
            bateria -= 5 * Time.deltaTime;
            IEnumerator Atirar() 
            {
                yield return new WaitForSeconds(1.0f);
                if (anim.GetBool("mShoot") && InstantiationTimer <= 0)
                {
                    tiro = Instantiate(lightningBall, bracoEsquerdo.transform.position,transform.rotation) as GameObject;
                    InstantiationTimer = 0.6f;
                }
            }
           
        }
        if (Input.GetButtonUp("Fire1")) 
        {
            anim.SetBool("mShoot", false);
        }

    }

    void FixedUpdate()
    {
        //Movimentação do jogador
        if (energia > 0 && bateria > 0) 
        {
            //Rotação do jogador
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");

            rotY += mouseX * mouseSensitivity * Time.deltaTime;
            rotX += mouseY * mouseSensitivity * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

            Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            transform.rotation = localRotation;

            if (!limiteTerreno)
            {
                if (transform.position.y < waterLevel)
                {
                    if (Input.GetAxis("Vertical") > 0) // Frente
                    {
                        rb.AddRelativeForce(Vector3.forward.normalized * speed * Time.deltaTime);
                        bubblesVFX.SetActive(true);
                        bubblesVFX.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                        bateria -= 1 * Time.deltaTime;
                    }
                    else if (Input.GetAxis("Vertical") < 0) // Trás
                    {
                        rb.AddRelativeForce(Vector3.back.normalized * speed * Time.deltaTime);
                        bubblesVFX.SetActive(true);
                        bubblesVFX.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                        bateria -= 1 * Time.deltaTime;
                    }

                }
                else
                {
                    if (Input.GetAxis("Vertical") > 0 && rotX >= 0)
                    {
                        rb.AddRelativeForce(Vector3.forward.normalized * speed * Time.deltaTime);
                        bubblesVFX.SetActive(true);
                        bubblesVFX.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                        bateria -= 1 * Time.deltaTime;
                    }

                    if (Input.GetAxis("Vertical") < 0 && rotX <= 0)
                    {
                        rb.AddRelativeForce(Vector3.back.normalized * speed * Time.deltaTime);
                        bubblesVFX.SetActive(true);
                        bubblesVFX.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                        bateria -= 1 * Time.deltaTime;
                    }
                }

                if (Input.GetAxis("Horizontal") > 0) // Direita
                {
                    rb.AddRelativeForce(Vector3.right.normalized * speed * Time.deltaTime);
                    bubblesVFX.SetActive(true);
                    bubblesVFX.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
                    bateria -= 1 * Time.deltaTime;
                }

                else if (Input.GetAxis("Horizontal") < 0) // Esquerda
                {
                    rb.AddRelativeForce(Vector3.left.normalized * speed * Time.deltaTime);
                    bubblesVFX.SetActive(true);
                    bubblesVFX.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
                    bateria -= 1 * Time.deltaTime;
                }
            }
            else
            {
                rb.AddRelativeForce(Vector3.down.normalized * gravity * Time.deltaTime);
            }
        }
 
        // Desligar efeito dos propulsores se o jogador não estiver se movimentando
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            bubblesVFX.SetActive(false);

        //Gravidade para fazer o robô afundar automaticamente na água
        if (transform.position.y < waterLevel && bateria > 0)
            rb.AddRelativeForce(Vector3.down.normalized * gravity * Time.deltaTime);

        //Efeito para o robô subir até a superfície quando termina a bateria
        if (transform.position.y < waterLevel && bateria <= 0)
            rb.AddRelativeForce(Vector3.up.normalized * gravity * 5 * Time.deltaTime);

    }

    //Corrotina de quando o robô é destruído
    IEnumerator GameOver() 
    {
        yield return new WaitForSeconds(0.001f);
        if (gameOver) 
        {
            yield return new WaitForSeconds(5.0f);
            gameOver = false;
            sendoPerseguido = false;
            SceneManager.LoadScene("Menu Inicial");
        }
        StartCoroutine(GameOver());
    }

    IEnumerator SomDestruido() 
    {
        yield return new WaitForSeconds(0.001f);
        if (energia <= 0) 
        {
            au.clip = somRobo;
            au.Play();
        }
        StartCoroutine(SomDestruido());
    }

    IEnumerator SomBateriaFraca()
    {
        yield return new WaitForSeconds(0.001f);
        if (bateria <= 0)
        {
            au.clip = bateriaFraca;
            au.Play();
            yield return new WaitForSeconds(10.0f);
        }
        StartCoroutine(SomBateriaFraca());
    }

    IEnumerator SomBateriaRecarregando() 
    {
        yield return new WaitForSeconds(0.001f);
        if (bateria < 600 && transform.position.y >= waterLevel)
        {
            anim.SetBool("mOff", false);
            au.clip = bateriaRecarregando;
            au.Play();
            yield return new WaitForSeconds(3.0f);
        }
        StartCoroutine(SomBateriaRecarregando());
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Terrain" && transform.position.y > 28f)
        {
            limiteTerreno = true;
        }
        else 
        {
            limiteTerreno = false;
        }

        //Quando um lixo colide com o robô ao sugar/atrai-lo
        if (c.gameObject.tag == "Lixo Metálico" && lixoMetalico) 
        {
            Canvas1.score += 100;
            canvas.GanhaPontos();
            anim.SetBool("mCollected", true);
            Destroy(c.gameObject);
            StartCoroutine(Coletado());
            IEnumerator Coletado() 
            {
                yield return new WaitForSeconds(0.51f);
                anim.SetBool("mCollected", false);
            }
        }

        if (c.gameObject.tag == "Lixo Não Metálico" && lixoNaoMetalico)
        {
            Canvas1.score += 100;
            canvas.GanhaPontos();
            anim.SetBool("mCollected", true);
            Destroy(c.gameObject);
            StartCoroutine(Coletado());
            IEnumerator Coletado()
            {
                yield return new WaitForSeconds(0.51f);
                anim.SetBool("mCollected", false);
            }
        }

        //Recarregar bateria com baterias e similares
        if (c.gameObject.name == "Pilha") 
        {
            bateria += 10;
        }

        if (c.gameObject.name == "Painel de satélite")
        {
            bateria += 150;
        }

        if (c.gameObject.name == "Bateria de Carro")
        {
            bateria += 200;
        }

        if (c.gameObject.tag == "Tubarão" && !invunerabilidade && energia > 0) 
        {
            damageVFX.SetActive(true);
            StartCoroutine(FimDano());
            energia--;
            invunerabilidade = true;
            IEnumerator FimDano() 
            {
                yield return new WaitForSeconds(2.0f);
                damageVFX.SetActive(false);
                invunerabilidade = false;
            }
        }

        if (c.gameObject.tag == "Morbilus" && !invunerabilidade && energia > 0)
        {
            damageVFX.SetActive(true);
            StartCoroutine(FimDano());
            energia--;
            invunerabilidade = true;
            IEnumerator FimDano()
            {
                yield return new WaitForSeconds(2.0f);
                damageVFX.SetActive(false);
                invunerabilidade = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Limite")
        {
            StartCoroutine(Voltar());
            IEnumerator Voltar() 
            {
                limiteMapa = true;
                yield return new WaitForSeconds(10.0f);
                transform.position = new Vector3(59.46f, 25.95f, -210.24f);
                limiteMapa = false;
            }
        }

        if (other.gameObject.tag == "Area do Tubarão") 
        {
            naAreaDoTubarao = true;
        }

        if (other.gameObject.tag == "Área de perseguição" && !Morbilus.morte)
        {
            sendoPerseguido = true;
            auMusica_1.volume = 0f;
            auMusica_2.volume = 0f;
            musicaChefe.SetActive(true);
        }

        if (other.gameObject.tag == "Área de Ataque")
        {
            Morbilus.areaAtaque1 = true;
        }

        if (other.gameObject.tag == "Área de Ataque 2")
        {
            Morbilus.areaAtaque2 = true;
        }

        if (other.gameObject.tag == "Fumaça Negra" && !invunerabilidade && energia > 0) 
        {
            damageVFX.SetActive(true);
            StartCoroutine(FimDano());
            energia--;
            invunerabilidade = true;
            IEnumerator FimDano()
            {
                yield return new WaitForSeconds(2.0f);
                damageVFX.SetActive(false);
                invunerabilidade = false;
            }
        }

        if (other.gameObject.tag == "Braçada" && !invunerabilidade && energia > 0)
        {
            damageVFX.SetActive(true);
            StartCoroutine(FimDano());
            energia -= 2;
            invunerabilidade = true;
            IEnumerator FimDano()
            {
                yield return new WaitForSeconds(2.0f);
                damageVFX.SetActive(false);
                invunerabilidade = false;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Area do Tubarão")
        {
            naAreaDoTubarao = false;
        }

        if (other.gameObject.tag == "Área de perseguição")
        {
            sendoPerseguido = false;
            Morbilus.rugir = true;
            musicaChefe.SetActive(false);
            trasicaoMusicas = true;
            StartCoroutine(Transicao());
            IEnumerator Transicao() 
            {
                yield return new WaitForSeconds(3.0f);
                trasicaoMusicas = false;
            }  
        }

        if (other.gameObject.tag == "Área de Ataque")
        {
            Morbilus.areaAtaque1 = false;
        }

        if (other.gameObject.tag == "Área de Ataque 2")
        {
            Morbilus.areaAtaque2 = false;
        }
    }
}
