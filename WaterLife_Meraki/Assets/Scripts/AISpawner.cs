using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class AIObjects 
{
    public string AIGroupName { get { return m_aiGroupName; } }
    public GameObject objectPrefab { get { return m_prefab; } }
    public int maxAI { get { return m_maxAI; } }
    public int spawnRate { get { return m_spawnRate; } }
    public int spawnAMount { get { return m_maxSpawnAmount; } }
    public bool randomizeStats { get { return m_randomizeStats; } }
    public bool enableSpawner { get { return m_enableSpawner; } }

    //Variáveis serializadas
    [Header("AI Group Stats")]
    [SerializeField]
    private string m_aiGroupName;
    [SerializeField]
    private GameObject m_prefab;
    [SerializeField]
    [Range(0f, 30f)]
    private int m_maxAI;
    [SerializeField]
    [Range(0f, 20f)]
    private int m_spawnRate;
    [SerializeField]
    [Range(0f, 10f)]
    private int m_maxSpawnAmount;

    [Header("Main Settings")]
    [SerializeField]
    private bool m_enableSpawner;
    [SerializeField]
    private bool m_randomizeStats;

    public AIObjects(string Name, GameObject Prefab, int MaxAI, int SpawnRate, int SpawnAmount, bool RandomizeStats) 
    {
        this.m_aiGroupName = Name;
        this.m_prefab = Prefab;
        this.m_maxAI = MaxAI;
        this.m_spawnRate = SpawnRate;
        this.m_maxSpawnAmount = SpawnAmount;
        this.m_randomizeStats = RandomizeStats;
    }

    public void setValues(int MaxAI, int SpawnRate, int SpawnAmount) 
    {
        this.m_maxAI = MaxAI;
        this.m_spawnRate = SpawnRate;
        this.m_maxSpawnAmount = SpawnAmount;
    }
}

public class AISpawner : MonoBehaviour
{
    // Lista para inserir os waypoints criados
    public List<Transform> Waypoints = new List<Transform>();

    public float spawnTimer { get { return m_SpawnTimer; } } // valor global para medir com qual frequência usa o spawner
    public Vector3 spawnArea { get { return m_SpawArea; } }

    // Variáveis serializadas
    [Header("Global Stats")]
    [Range(0f, 600f)]
    [SerializeField]
    private float m_SpawnTimer; // valor global para medir com qual frequência usa o spawner
    [SerializeField]
    private Color m_SpawnColor = new Color(1.0f, 0.0f, 0.3f); // usar a cor para o gizmo
    [SerializeField]
    private Vector3 m_SpawArea = new Vector3(20f, 10f, 20f);

    // Criar uma array da nova classe
    [Header("AI Groups Settings")]
    public AIObjects[] AIObject = new AIObjects[5];
    
    // Start is called before the first frame update
    void Start()
    {
        GetWaypoints();
        RandomizeGroups();
        CreateAIGroups();
        InvokeRepeating("SpawnNPC", 0.5f, spawnTimer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnNPC() 
    {
        // Loop por todos os grupos de IA
        for (int i = 0; i < AIObject.Count(); i++) 
        {
            // Verifica se o spawner está habilitado
            if (AIObject[i].enableSpawner && AIObject[i].objectPrefab != null) 
            {
                // Verifica se o grupo de IA não tem o nº máximo de NPCs
                GameObject tempGroup = GameObject.Find(AIObject[i].AIGroupName);
                if (tempGroup.GetComponentInChildren<Transform>().childCount < AIObject[i].maxAI) 
                {
                    // Número aleatório de spawn dos NPCs de 0 até quantidade máxima estabelecida
                    for (int y = 0; y < Random.Range(0, AIObject[i].spawnAMount); y++) 
                    {
                        // Pega uma rotação aleatória
                        Quaternion randomRotation = Quaternion.Euler(Random.Range(-20, 20), Random.Range(0, 360), 0);
                        // Cria o gameobject spawnado
                        GameObject tempSpawn;
                        tempSpawn = Instantiate(AIObject[i].objectPrefab, RandomPosition(), randomRotation);
                        // Coloca o NPC spawnado como um filho do grupo
                        tempSpawn.transform.parent = tempGroup.transform;
                        // Adiciona o AIMove script e classe ao novo NPC
                        tempSpawn.AddComponent<AIMove>();
                    }
                }
            }
        }
    }

    public Vector3 RandomPosition() 
    {
        // Pega uma posição aleatória dentro do espaço da Spawn Area
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            Random.Range(-spawnArea.y, spawnArea.y),
            Random.Range(-spawnArea.z, spawnArea.z));
        randomPosition = transform.TransformPoint(randomPosition * 0.5f);
        return randomPosition;
    }

    // Método para colocar valores aleatórios no AI Group setting
    void RandomizeGroups() 
    {
        for (int i = 0; i < AIObject.Count(); i++) 
        {
            if (AIObject[i].randomizeStats) 
            {
                //AIObject[i].maxAI = Random.Range(1, 30);
                //AIObject[i] = new AIObjects(AIObject[i].AIGroupName, AIObject[i].objectPrefab, Random.Range(1, 30), Random.Range(1, 20), Random.Range(1, 10), AIObject[i].randomizeStats);
                AIObject[i].setValues(Random.Range(1, 30), Random.Range(1, 20), Random.Range(1, 10));
            }
        }
    }

    // Método para pegar um Waypoint aleatório
    public Vector3 RandomWaypoint() 
    {
        int randomWP = Random.Range(0,(Waypoints.Count - 1));
        Vector3 randomWaypoint = Waypoints[randomWP].transform.position;
        return randomWaypoint;
    }

    // Método para criar os grupos de objetos vazios
    void CreateAIGroups() 
    {
        for (int i = 0; i < AIObject.Count(); i++) 
        {
            // Gameobject vazio para mantetr a IA
            GameObject m_AIGroupSpawn;

            // Cria um novo gameobject
            m_AIGroupSpawn = new GameObject(AIObject[i].AIGroupName);
            m_AIGroupSpawn.transform.parent = this.gameObject.transform;
        }
    }

    void GetWaypoints() 
    {
        //Checa os filhos acumulados nele
        Transform[] wpList = this.transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < wpList.Length; i++) 
        {
            if (wpList[i].tag == "waypoint") 
            {
                //Adiciona à lista
                Waypoints.Add(wpList[i]);
            }
        }
    }

    // Mostra os gizmos em cor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = m_SpawnColor;
        Gizmos.DrawCube(transform.position, spawnArea);
    }
}
