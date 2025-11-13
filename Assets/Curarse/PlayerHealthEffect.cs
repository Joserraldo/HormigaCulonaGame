using UnityEngine;
using System.Collections;

public class PlayerHealthEffect : MonoBehaviour
{
    [Header("Efecto Visual de Curación")]
    public float growScale = 1.3f; // Escala aumentada (130% del tamaño original)
    public float effectDuration = 0.5f; // Duración del efecto en segundos
    public Color healColor = new Color(0.3f, 1f, 0.3f); // Color verde
    
    private Vector3 originalScale;
    private bool isEffectActive = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    /// <summary>
    /// Aplica el efecto de curación al jugador (aumento de escala y cambio de color).
    /// Este método será llamado por el PlayerHealthDetector.
    /// </summary>
    public void ApplyHealEffect()
    {
        if (!isEffectActive)
        {
            // Obtener el componente PlayerHealth para aumentar vida
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(); // Necesitarás agregar este método a PlayerHealth
            }

            // Iniciar la corutina del efecto visual
            StartCoroutine(HealEffectCoroutine());
        }
    }

    /// <summary>
    /// Corutina que maneja el efecto visual de curación.
    /// </summary>
    private IEnumerator HealEffectCoroutine()
    {
        isEffectActive = true;

        // Obtener todos los Renderer del jugador para cambiar el color
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        Color[] originalColors = new Color[renderers.Length];
        
        // Guardar colores originales
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
            {
                originalColors[i] = renderers[i].material.color;
            }
        }

        float elapsed = 0f;
        float halfDuration = effectDuration / 2f;

        // Primera mitad: aumentar escala y cambiar a color verde
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;

            // Interpolación de escala (crecer)
            transform.localScale = Vector3.Lerp(originalScale, originalScale * growScale, t);

            // Interpolación de color
            foreach (Renderer renderer in renderers)
            {
                if (renderer.material.HasProperty("_Color"))
                {
                    renderer.material.color = Color.Lerp(renderer.material.color, healColor, t);
                }
            }

            yield return null;
        }

        elapsed = 0f;

        // Segunda mitad: volver a la normalidad
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;

            // Interpolación de escala de vuelta
            transform.localScale = Vector3.Lerp(originalScale * growScale, originalScale, t);

            // Interpolación de color de vuelta
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].material.HasProperty("_Color"))
                {
                    renderers[i].material.color = Color.Lerp(healColor, originalColors[i], t);
                }
            }

            yield return null;
        }

        // Asegurar que volvemos exactamente a los valores originales
        transform.localScale = originalScale;
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
            {
                renderers[i].material.color = originalColors[i];
            }
        }

        isEffectActive = false;
    }
}