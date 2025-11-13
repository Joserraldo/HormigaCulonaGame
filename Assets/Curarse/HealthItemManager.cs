using UnityEngine;
using System.Collections.Generic;

public class HealthItemManager : MonoBehaviour
{
    [Header("Movimiento del Item")]
    public float amplitud = 0.25f;
    public float speed = 2f;
    public float rotationSpeed = 45f;

    [Header("Configuración")]
    public string healthItemTag = "HealthItem";

    private List<Transform> healthItems;
    private Dictionary<Transform, Vector3> startPositions = new Dictionary<Transform, Vector3>();

    void Start()
    {
        healthItems = new List<Transform>();

        // Encontrar todos los items con el tag "HealthItem"
        GameObject[] itemObjects = GameObject.FindGameObjectsWithTag(healthItemTag);

        foreach (GameObject go in itemObjects)
        {
            healthItems.Add(go.transform);
        }

        // Configurar cada item
        foreach (var item in healthItems)
        {
            if (item != null)
            {
                // Guardar posición inicial
                startPositions[item] = item.position;

                // Configurar collider
                Collider col = item.GetComponent<Collider>();
                if (col == null)
                {
                    col = item.gameObject.AddComponent<BoxCollider>();
                }
                col.isTrigger = true;

                // Añadir detector
                if (item.GetComponent<PlayerHealthDetector>() == null)
                {
                    item.gameObject.AddComponent<PlayerHealthDetector>().Init(this);
                }
            }
        }
    }

    void Update()
    {
        // Aplicar movimiento flotante y rotación
        foreach (var item in healthItems)
        {
            if (item == null) continue;

            Vector3 startPos = startPositions[item];
            float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitud;
            item.position = new Vector3(startPos.x, newY, startPos.z);
            item.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Remueve y destruye un item de curación de la escena.
    /// </summary>
    public void CollectHealthItem(Transform item)
    {
        if (!healthItems.Contains(item)) return;

        healthItems.Remove(item);
        Destroy(item.gameObject);
    }
}