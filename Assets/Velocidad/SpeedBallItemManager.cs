using UnityEngine;
using System.Collections.Generic;

public class SpeedBallItemManager : MonoBehaviour
{
    [Header("Movimiento del Item")]
    public float amplitud = 0.25f;
    public float speed = 2f;
    public float rotationSpeed = 90f; // Rotación más rápida para dar sensación de velocidad

    [Header("Configuración")]
    public string speedBallItemTag = "SpeedBallItem";

    private List<Transform> speedBallItems;
    private Dictionary<Transform, Vector3> startPositions = new Dictionary<Transform, Vector3>();

    void Start()
    {
        speedBallItems = new List<Transform>();

        // Encontrar todos los items con el tag "SpeedBallItem"
        GameObject[] itemObjects = GameObject.FindGameObjectsWithTag(speedBallItemTag);

        foreach (GameObject go in itemObjects)
        {
            speedBallItems.Add(go.transform);
        }

        // Configurar cada item
        foreach (var item in speedBallItems)
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
                if (item.GetComponent<PlayerSpeedBallDetector>() == null)
                {
                    item.gameObject.AddComponent<PlayerSpeedBallDetector>().Init(this);
                }
            }
        }
    }

    void Update()
    {
        // Aplicar movimiento flotante y rotación rápida
        foreach (var item in speedBallItems)
        {
            if (item == null) continue;

            Vector3 startPos = startPositions[item];
            float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitud;
            item.position = new Vector3(startPos.x, newY, startPos.z);
            
            // Rotar en múltiples ejes para efecto más dinámico
            item.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
            item.Rotate(Vector3.right, rotationSpeed * 0.5f * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Remueve y destruye un item de velocidad de la escena.
    /// </summary>
    public void CollectSpeedBallItem(Transform item)
    {
        if (!speedBallItems.Contains(item)) return;

        speedBallItems.Remove(item);
        Destroy(item.gameObject);
    }
}