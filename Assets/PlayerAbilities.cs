using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    // Variables para el movimiento (ajusta según tu script de movimiento)
    // Asumo que tienes una variable de velocidad que controlas en otro script
    // Reemplaza 'PlayerMovementScript' con el nombre real de tu script de movimiento.
    // Si manejas la velocidad con un Rigidbody, haz referencia a él.
    [Header("Configuración de Velocidad")]
    public float baseSpeed = 5f; // Velocidad normal del jugador
    public float speedIncreaseAmount = 2f; // Cuánto aumenta la velocidad al recolectar
    private float currentSpeed;

    [Header("Configuración de Escala")]
    public float baseScale = 1f; // Escala normal (ej. 1,1,1)
    public float scaleDecreaseFactor = 0.5f; // Factor para reducir la escala (ej. 0.5 para la mitad)

    // Variables de referencia al Rigidbody (si tu movimiento lo usa)
    private Rigidbody rb;

    void Start()
    {
        // Obtener el Rigidbody al inicio si lo usas
        rb = GetComponent<Rigidbody>();
        currentSpeed = baseSpeed;

        // Asegúrate de que el jugador inicie con la escala y velocidad base
        transform.localScale = new Vector3(baseScale, baseScale, baseScale);

        // Aquí deberías inicializar la velocidad en tu script de movimiento si es necesario
        // Ejemplo: GetComponent<PlayerMovementScript>().speed = baseSpeed;
    }

    /// <summary>
    /// Aumenta la velocidad del jugador y llama a reducir la escala.
    /// Este método será llamado por el PlayerCollectibleDetector.
    /// </summary>
    public void ActivatePowerUp()
    {
        // 1. Aumentar la Velocidad
        currentSpeed += speedIncreaseAmount;
        Debug.Log("Velocidad aumentada a: " + currentSpeed);
        
        // **IMPORTANTE:** Aquí tienes que aplicar 'currentSpeed' a tu script de movimiento real.
        // Ejemplo si usas un script 'PlayerMovement':
        // GetComponent<PlayerMovementScript>().movementSpeed = currentSpeed;
        
        // 2. Reducir la Escala
        ReduceScale();
    }

    /// <summary>
    /// Reduce la escala del GameObject del jugador.
    /// </summary>
    private void ReduceScale()
    {
        // Crea el vector de la nueva escala
        Vector3 newScale = new Vector3(baseScale, baseScale, baseScale) * scaleDecreaseFactor;

        // Aplica la nueva escala
        transform.localScale = newScale;
        Debug.Log("Escala reducida a: " + newScale);
    }
    
    // Si tu movimiento está aquí, puedes usar 'currentSpeed' para moverte.
    /* void FixedUpdate() 
    {
        // Ejemplo de movimiento simple usando Rigidbody y currentSpeed
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * currentSpeed);
    }
    */
}
