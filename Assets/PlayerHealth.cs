using UnityEngine;
using TMPro; // Necesitas esto para trabajar con TextMeshPro

public class PlayerHealth : MonoBehaviour
{
    // Asigna este texto en el Inspector de Unity (tu "texto vida")
    [Header("UI y Vidas")]
    public TextMeshProUGUI healthText; 
    public int maxHealth = 3;
    
    private int currentHealth;

    // Inicialización al empezar el juego
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    /// <summary>
    /// Se llama cuando el jugador debe perder una vida (ej. al hacer respawn).
    /// </summary>
    public void TakeDamage()
    {
        // Solo resta vida si la salud es mayor a cero
        if (currentHealth > 0)
        {
            currentHealth--;
            UpdateHealthUI();
        }

        // Verifica si el jugador ha perdido
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Actualiza el texto de la interfaz de usuario (ej: 2/3).
    /// </summary>
    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            // Formato: "2/3", "1/3", etc.
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
        else
        {
            Debug.LogWarning("El TextMeshProUGUI 'healthText' no está asignado en PlayerHealth.");
        }
    }

    /// <summary>
    /// Lógica que ocurre cuando el jugador pierde todas las vidas.
    /// </summary>
    void Die()
    {
        Debug.Log("¡El jugador ha perdido! (Llamar a pantalla de Game Over)");
        
        // Aquí puedes añadir código para:
        // 1. Mostrar la pantalla de Game Over.
        // 2. Detener el juego (Time.timeScale = 0).
        // 3. Recargar la escena.
        // gameObject.SetActive(false); // Ocultar al jugador
    }
}
