using UnityEngine;
using TMPro;

public class PlayerSpeedBallEffect : MonoBehaviour
{
    [Header("Configuración de Velocidad")]
    public float speedMultiplier = 2.5f; // Multiplicador de velocidad
    public float effectDuration = 10f; // Duración del efecto en segundos
    public TextMeshProUGUI timerText; // UI para mostrar el timer

    [Header("Transformación a Esfera")]
    public float ballScale = 0.6f; // Tamaño de la esfera (más pequeño)
    public bool hidePlayerModel = true; // Ocultar el modelo del jugador
    
    private bool isSpeedBallActive = false;
    private float speedTimer = 0f;
    private Vector3 originalScale;
    private GameObject ballVisual; // Esfera visual que aparece
    private New_CharacterController characterController;
    private float originalWalkSpeed;
    private float originalSprintSpeed;

    // Referencias para ocultar el modelo
    private Renderer[] playerRenderers;

    void Start()
    {
        originalScale = transform.localScale;
        characterController = GetComponent<New_CharacterController>();
        
        if (characterController != null)
        {
            originalWalkSpeed = characterController.WalkSpeed;
            originalSprintSpeed = characterController.SprintSpeed;
        }

        // Obtener los renderers del jugador
        playerRenderers = GetComponentsInChildren<Renderer>();

        // Crear la esfera visual (inicialmente desactivada)
        CreateBallVisual();

        if (timerText != null)
        {
            timerText.text = "";
        }
    }

    void Update()
    {
        if (isSpeedBallActive)
        {
            speedTimer -= Time.deltaTime;
            
            if (timerText != null)
            {
                timerText.text = "⚡ " + Mathf.Ceil(speedTimer).ToString();
            }

            if (speedTimer <= 0f)
            {
                DeactivateSpeedBall();
            }
        }
    }

    /// <summary>
    /// Crea la esfera visual que representa al jugador.
    /// </summary>
    void CreateBallVisual()
    {
        ballVisual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ballVisual.name = "SpeedBall";
        ballVisual.transform.SetParent(transform);
        ballVisual.transform.localPosition = Vector3.zero;
        ballVisual.transform.localScale = Vector3.one * ballScale;
        
        // Darle un color llamativo (azul eléctrico)
        Renderer ballRenderer = ballVisual.GetComponent<Renderer>();
        if (ballRenderer != null)
        {
            ballRenderer.material.color = new Color(0.2f, 0.5f, 1f);
            ballRenderer.material.SetFloat("_Metallic", 0.5f);
            ballRenderer.material.SetFloat("_Glossiness", 0.8f);
        }

        // Desactivar inicialmente
        ballVisual.SetActive(false);

        // Remover el collider de la esfera visual (el jugador ya tiene su collider)
        Collider ballCollider = ballVisual.GetComponent<Collider>();
        if (ballCollider != null)
        {
            Destroy(ballCollider);
        }
    }

    /// <summary>
    /// Activa el modo de bola de velocidad.
    /// Este método será llamado por el PlayerSpeedBallDetector.
    /// </summary>
    public void ActivateSpeedBall()
    {
        isSpeedBallActive = true;
        speedTimer = effectDuration;

        // Aumentar velocidad del controlador
        if (characterController != null)
        {
            characterController.WalkSpeed = originalWalkSpeed * speedMultiplier;
            characterController.SprintSpeed = originalSprintSpeed * speedMultiplier;
        }

        // Transformar visualmente
        transform.localScale = originalScale * ballScale;

        // Ocultar modelo del jugador y mostrar esfera
        if (hidePlayerModel)
        {
            foreach (Renderer renderer in playerRenderers)
            {
                renderer.enabled = false;
            }
            ballVisual.SetActive(true);
        }

        Debug.Log("Modo Bola de Velocidad activado por " + effectDuration + " segundos");
    }

    /// <summary>
    /// Desactiva el modo de bola de velocidad.
    /// </summary>
    void DeactivateSpeedBall()
    {
        isSpeedBallActive = false;
        speedTimer = 0f;

        // Restaurar velocidad normal
        if (characterController != null)
        {
            characterController.WalkSpeed = originalWalkSpeed;
            characterController.SprintSpeed = originalSprintSpeed;
        }

        // Restaurar apariencia
        transform.localScale = originalScale;

        // Mostrar modelo del jugador y ocultar esfera
        if (hidePlayerModel)
        {
            foreach (Renderer renderer in playerRenderers)
            {
                renderer.enabled = true;
            }
            ballVisual.SetActive(false);
        }

        if (timerText != null)
        {
            timerText.text = "";
        }

        Debug.Log("Modo Bola de Velocidad desactivado");
    }

    /// <summary>
    /// Propiedad para saber si el efecto está activo.
    /// </summary>
    public bool IsSpeedBallActive
    {
        get { return isSpeedBallActive; }
    }
}