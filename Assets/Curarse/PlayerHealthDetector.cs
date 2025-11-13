using UnityEngine;

public class PlayerHealthDetector : MonoBehaviour
{
    private HealthItemManager manager;

    /// <summary>
    /// Inicializa el detector con referencia al manager.
    /// </summary>
    public void Init(HealthItemManager manager)
    {
        this.manager = manager;
    }

    /// <summary>
    /// Detecta cuando el jugador toca el item de curación.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Obtener el componente de efecto de curación del jugador
            PlayerHealthEffect healthEffect = other.GetComponent<PlayerHealthEffect>();

            if (healthEffect != null)
            {
                // Aplicar el efecto de curación al jugador
                healthEffect.ApplyHealEffect();
            }

            // Notificar al manager para destruir este item
            manager.CollectHealthItem(transform);
        }
    }
}