using UnityEngine;
using System.Collections.Generic;

public class DamageItemManager : MonoBehaviour
{
    [Header("Movimiento del Item")]
    public float amplitud = 0.25f;
    public float speed = 2f;
    public float rotationSpeed = 45f;

    [Header("Configuración")]
    public string damageItemTag = "DamageItem";

    private List<Transform> damageItems;
    private Dictionary<Transform, Vector3> startPositions = new Dictionary<Transform, Vector3>();

    void Start()
    {
        damageItems = new List<Transform>();

        // Encontrar todos los items con el tag "DamageItem"
        GameObject[] itemObjects = GameObject.FindGameObjectsWithTag(damageItemTag);

        foreach (GameObject go in itemObjects)
        {
            damageItems.Add(go.transform);
        }

        // Configurar cada item
        foreach (var item in damageItems)
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
                if (item.GetComponent<PlayerDamageDetector>() == null)
                {
                    item.gameObject.AddComponent<PlayerDamageDetector>().Init(this);
                }
            }
        }
    }

    void Update()
    {
        // Aplicar movimiento flotante y rotación
        foreach (var item in damageItems)
        {
            if (item == null) continue;

            Vector3 startPos = startPositions[item];
            float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitud;
            item.position = new Vector3(startPos.x, newY, startPos.z);
            item.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Remueve y destruye un item de daño de la escena.
    /// </summary>
    public void CollectDamageItem(Transform item)
    {
        if (!damageItems.Contains(item)) return;

        damageItems.Remove(item);
        Destroy(item.gameObject);
    }
}