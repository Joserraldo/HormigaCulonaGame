using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Configuración de Trepa")]
    public float climbDuration = 10f;
    public TextMeshProUGUI timerText;

    [Header("Efecto Visual de Trepa")]
    public float glowScale = 1.15f; // Escala con brillo (115% del tamaño original)
    public float effectDuration = 0.6f; // Duración del efecto de activación
    public Color climbColor = new Color(1f, 0.8f, 0.2f); // Color amarillo/dorado
    public bool pulseEffect = true; // Efecto de pulso mientras trepa
    public float pulseSpeed = 2f; // Velocidad del pulso
    public float pulseIntensity = 0.05f; // Intensidad del pulso (5% de variación)

    private bool canClimb = false;
    private float climbTimer = 0f;
    private Vector3 originalScale;
    private bool isEffectActive = false;
    private Renderer[] playerRenderers;
    private Color[] originalColors;

    void Start()
    {
        originalScale = transform.localScale;
        playerRenderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[playerRenderers.Length];

        // Guardar colores originales
        for (int i = 0; i < playerRenderers.Length; i++)
        {
            if (playerRenderers[i].material.HasProperty("_Color"))
            {
                originalColors[i] = playerRenderers[i].material.color;
            }
        }

        if (timerText != null)
        {
            timerText.text = "";
        }
    }

    void Update()
    {
        if (canClimb)
        {
            climbTimer -= Time.deltaTime;
            if (timerText != null)
            {
                timerText.text = "🧗 " + Mathf.Ceil(climbTimer).ToString();
            }

            // Efecto de pulso mientras está activo
            if (pulseEffect && !isEffectActive)
            {
                float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseIntensity;
                transform.localScale = originalScale * pulse;

                // Pulso de color sutil
                float colorPulse = 0.7f + Mathf.Sin(Time.time * pulseSpeed) * 0.3f;
                for (int i = 0; i < playerRenderers.Length; i++)
                {
                    if (playerRenderers[i].material.HasProperty("_Color"))
                    {
                        playerRenderers[i].material.color = Color.Lerp(originalColors[i], climbColor, colorPulse * 0.4f);
                    }
                }
            }

            if (climbTimer <= 0f)
            {
                DeactivateClimb();
            }
        }
    }

    /// <summary>
    /// Activa la habilidad de trepar con efecto visual.
    /// </summary>
    public void ActivatePowerUp()
    {
        canClimb = true;
        climbTimer = climbDuration;
        Debug.Log("Habilidad de trepar activada por " + climbDuration + " segundos");

        // Iniciar efecto visual de activación
        StartCoroutine(ClimbActivationEffect());
    }

    /// <summary>
    /// Corutina que maneja el efecto visual de activación de trepa.
    /// </summary>
    private IEnumerator ClimbActivationEffect()
    {
        isEffectActive = true;

        float elapsed = 0f;
        float halfDuration = effectDuration / 2f;

        // Primera mitad: crecer y cambiar a color de trepa
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;

            // Interpolación de escala (crecer con brillo)
            transform.localScale = Vector3.Lerp(originalScale, originalScale * glowScale, t);

            // Interpolación de color a amarillo/dorado
            for (int i = 0; i < playerRenderers.Length; i++)
            {
                if (playerRenderers[i].material.HasProperty("_Color"))
                {
                    playerRenderers[i].material.color = Color.Lerp(originalColors[i], climbColor, t);
                }
            }

            yield return null;
        }

        elapsed = 0f;

        // Segunda mitad: volver a escala normal pero mantener tinte de color
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;

            // Volver a escala original
            transform.localScale = Vector3.Lerp(originalScale * glowScale, originalScale, t);

            yield return null;
        }

        // Asegurar escala original
        transform.localScale = originalScale;
        isEffectActive = false;
    }

    /// <summary>
    /// Desactiva la habilidad de trepar y restaura el aspecto normal.
    /// </summary>
    private void DeactivateClimb()
    {
        StartCoroutine(ClimbDeactivationEffect());
    }

    /// <summary>
    /// Corutina que maneja el efecto visual de desactivación.
    /// </summary>
    private IEnumerator ClimbDeactivationEffect()
    {
        canClimb = false;
        climbTimer = 0f;

        float elapsed = 0f;
        float deactivationDuration = 0.3f;

        // Fade out del color y escala
        while (elapsed < deactivationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / deactivationDuration;

            // Volver a colores originales
            for (int i = 0; i < playerRenderers.Length; i++)
            {
                if (playerRenderers[i].material.HasProperty("_Color"))
                {
                    playerRenderers[i].material.color = Color.Lerp(playerRenderers[i].material.color, originalColors[i], t);
                }
            }

            yield return null;
        }

        // Asegurar valores originales
        transform.localScale = originalScale;
        for (int i = 0; i < playerRenderers.Length; i++)
        {
            if (playerRenderers[i].material.HasProperty("_Color"))
            {
                playerRenderers[i].material.color = originalColors[i];
            }
        }

        if (timerText != null)
        {
            timerText.text = "";
        }

        Debug.Log("Habilidad de trepar desactivada");
    }

    /// <summary>
    /// Propiedad para acceder a canClimb desde otros scripts.
    /// </summary>
    public bool CanClimb
    {
        get { return canClimb; }
    }
}