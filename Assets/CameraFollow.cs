using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform target; // El personaje que la cámara sigue

    [Header("Configuración de cámara")]
    public float distance = 4f;     // Qué tan lejos está la cámara
    public float height = 2f;       // Qué tan alto está respecto al jugador
    public float sensitivity = 2f;  // Sensibilidad del mouse
    public float smoothSpeed = 10f; // Qué tan suave se mueve la cámara

    [Header("Límites verticales")]
    public float minPitch = -20f;
    public float maxPitch = 60f;

    private float yaw;   // Rotación horizontal
    private float pitch; // Rotación vertical

    void Start()
    {
        // Bloquea el cursor al centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Inicializa la rotación
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void LateUpdate()
    {
        if (!target) return;

        // Movimiento del mouse
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Calcula la rotación de la cámara
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Calcula posición deseada detrás del jugador
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance + Vector3.up * height;

        // Movimiento suave
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // La cámara siempre mira al jugador
        transform.LookAt(target.position + Vector3.up * height * 0.5f);

        // Hace que el jugador mire en la dirección de la cámara (solo en eje Y)
        Vector3 forward = transform.forward;
        forward.y = 0f; // Evita que el jugador se incline
        target.forward = Vector3.Lerp(target.forward, forward, smoothSpeed * Time.deltaTime);
    }
}
