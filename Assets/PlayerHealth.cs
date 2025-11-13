using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("UI y Vidas")]
    public TextMeshProUGUI healthText; 
    public int maxHealth = 3;
    
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    /// <summary>
    /// Se llama cuando el jugador debe perder una vida.
    /// </summary>
    public void TakeDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth--;
            UpdateHealthUI();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// NUEVO: Restaura una vida al jugador (sin exceder el máximo).
    /// </summary>
    public void Heal()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            UpdateHealthUI();
            Debug.Log("¡Vida restaurada! Vida actual: " + currentHealth);
        }
        else
        {
            Debug.Log("La vida ya está al máximo.");
        }
    }

    /// <summary>
    /// Actualiza el texto de la interfaz de usuario.
    /// </summary>
    void UpdateHealthUI()
    {
        if (healthText != null)
        {
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
        // Aquí puedes añadir código para Game Over
    }
}