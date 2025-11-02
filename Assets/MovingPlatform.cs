using UnityEngine;

// Clase encargada de mover una plataforma verticalmente entre dos límites
public class MovingPlatform : MonoBehaviour
{
    [Header("Configuración de la plataforma")]
    [Tooltip("Altura mínima de la plataforma en unidades del mundo")]
    [SerializeField] private float minHeight = 0f;

    [Tooltip("Altura máxima de la plataforma en unidades del mundo")]
    [SerializeField] private float maxHeight = 0f;

    [Tooltip("Velocidad de movimiento en unidades / seg")]
    [SerializeField] private float speed = 2f;

    // Para controlar la dirección del movimiento (sube/baja)
    private bool movingUp = true;

    // Guarda la posición inicial de la plataforma al iniciar
    private Vector3 initialPosition;

    // Guarda la posición inicial en Start para usar como referencia
    void Start()
    {
        initialPosition = transform.position;
    }

    // Mueve la plataforma cada frame
    void Update()
    {
        // Obtiene la altura actual (en Y) de la plataforma
        float currentHeight = transform.position.y;

        // Decide si moverse hacia arriba o abajo usando la dirección
        Vector3 movement = movingUp ? Vector3.up : Vector3.down;
        movement *= speed * Time.deltaTime;

        // Aplica el movimiento calculado
        transform.Translate(movement);

        // Calcula los límites máximos y mínimos de movimiento
        float maxY = initialPosition.y + maxHeight;
        float minY = initialPosition.y + minHeight;

        // Cambia de dirección al llegar a los límites
        if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
            movingUp = false; // Ahora baja
        }
        else if (transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
            movingUp = true; // Ahora sube
        }
    }
}
