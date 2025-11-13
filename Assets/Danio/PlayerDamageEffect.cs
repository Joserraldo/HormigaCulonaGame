using UnityEngine;
using System.Collections;

public class PlayerDamageEffect : MonoBehaviour
{
    [Header("Efecto Visual de Daño")]
    public float shrinkScale = 0.8f; // Escala reducida (80% del tamaño original)
    public float effectDuration = 0.5f; // Duración del efecto en segundos
    public Color damageColor = new Color(1f, 0.3f, 0.3f); // Color rojizo
    
    private Vector3 originalScale;
    private bool isEffectActive = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    /// <summary>
    /// Aplica el efecto de daño al jugador (reducción de escala y cambio de color).
    /// Este método será llamado por el PlayerDamageDetector.
    /// </summary>
    public void ApplyDamageEffect()
    {
        if (!isEffectActive)
        {
            // Obtener el componente PlayerHealth para reducir vida
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage();
            }

            // Iniciar la corutina del efecto visual
            StartCoroutine(DamageEffectCoroutine());
        }
    }

    /// <summary>
    /// Corutina que maneja el efecto visual de daño.
    /// </summary>
    private IEnumerator DamageEffectCoroutine()
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

        // Primera mitad: reducir escala y cambiar color
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;

            // Interpolación de escala
            transform.localScale = Vector3.Lerp(originalScale, originalScale * shrinkScale, t);

            // Interpolación de color
            foreach (Renderer renderer in renderers)
            {
                if (renderer.material.HasProperty("_Color"))
                {
                    renderer.material.color = Color.Lerp(renderer.material.color, damageColor, t);
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
            transform.localScale = Vector3.Lerp(originalScale * shrinkScale, originalScale, t);

            // Interpolación de color de vuelta
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].material.HasProperty("_Color"))
                {
                    renderers[i].material.color = Color.Lerp(damageColor, originalColors[i], t);
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