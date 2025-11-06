using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ColletibleMangerS : MonoBehaviour
{
    // Variables para el movimiento (SecuenciaMovimiento)
    [Header("SecuenciaMovimiento")]
    public float amplitud = 0.25f;
    public float speed = 2f;
    public float rotationSpeed = 45f;

    // Variables para el sistema de recolección (SistemaRecoleccion)
    [Header("SistemaRecoleccion")]
    public TextMeshProUGUI itemCounter;
    public int totalItemsScene = 2; // Asumo que este valor se establecerá en el Inspector
    public string collectableTag = "Collectible";
    
    // Lista para almacenar los objetos coleccionables y su posición inicial
    private List<Transform> collectibles;
    private Dictionary<Transform, Vector3> startposition = new Dictionary<Transform, Vector3>();
    
    // Contador estático para los ítems recolectados
    private static int itemsCollected = 0;

    // Start se llama una vez antes de la primera ejecución de Update después de que se crea el MonoBehaviour
    void Start()
    {
        // Inicializa la lista de coleccionables
        collectibles = new List<Transform>();
        
        // Encuentra todos los GameObjects con el tag "Collectible" (asumiendo que collectableTag es "Collectible")
        GameObject[] collectableObjects = GameObject.FindGameObjectsWithTag(collectableTag);
        
        // Llena la lista 'collectibles'
        foreach (GameObject go in collectableObjects)
        {
            collectibles.Add(go.transform);
        }

        // Configuración inicial de los coleccionables
        foreach (var obj in collectibles)
        {
            if (obj != null)
            {
                // Guarda la posición inicial
                startposition[obj] = obj.position;

                // Obtiene o añade el componente Collider
                Collider col = obj.GetComponent<Collider>();
                if (col == null)
                {
                    // Si no hay un Collider, añade un BoxCollider por defecto
                    col = obj.gameObject.AddComponent<BoxCollider>();
                }
                
                // Configura el Collider como trigger
                col.isTrigger = true;

                // *** Lógica de la nueva captura: Añadir e Inicializar el detector ***
                if (obj.GetComponent<PlayerCollectibleDetector>() == null)
                {
                    obj.gameObject.AddComponent<PlayerCollectibleDetector>().Init(this);
                }
            }
        }
        
        // Inicializa el contador de ítems
        UpdatedCounterUI();
    }

    // Update se llama una vez por frame
    void Update()
    {
        // Aplica el movimiento de "flote" y rotación a cada coleccionable
        foreach (var obj in collectibles)
        {
            if (obj == null) continue; // Si el objeto ha sido destruido, salta al siguiente.

            // Obtiene la posición inicial guardada
            Vector3 StartPos = startposition[obj];
            
            // Calcula la nueva posición Y usando una función seno para el flote
            float newY = StartPos.y + Mathf.Sin(Time.deltaTime * speed) * amplitud;
            
            // Asigna la nueva posición
            obj.position = new Vector3(StartPos.x, newY, StartPos.z);
            
            // Rota el objeto alrededor del eje Y (Vector3.up)
            obj.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Método llamado para recolectar un ítem.
    /// </summary>
    /// <param name="obj">Transform del objeto recolectado.</param>
    public void Collect(Transform obj)
    {
        // 1. Verifica si el objeto está en la lista de coleccionables
        if (!collectibles.Contains(obj)) return;

        // 2. Remueve el objeto de la lista
        collectibles.Remove(obj);
        
        // 3. Incrementa el contador global y local
        itemsCollected++;
        
        // 4. Actualiza la interfaz de usuario
        UpdatedCounterUI();
        
        // 5. Destruye el GameObject
        Destroy(obj.gameObject);
    }

    /// <summary>
    /// Actualiza el texto de la interfaz de usuario con el contador de ítems.
    /// </summary>
    void UpdatedCounterUI()
    {
        // Verifica si el componente TextMeshProUGUI está asignado
        if (itemCounter != null)
        {
            // Usa una cadena interpolada para mostrar ítems recolectados / total de ítems en la escena
            itemCounter.text = $"{itemsCollected} / {totalItemsScene}";
        }
    }
}