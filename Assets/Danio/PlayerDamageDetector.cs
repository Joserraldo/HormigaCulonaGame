using UnityEngine;

public class PlayerDamageDetector : MonoBehaviour
{
    private DamageItemManager manager;

    /// <summary>
    /// Inicializa el detector con referencia al manager.
    /// </summary>
    public void Init(DamageItemManager manager)
    {
        this.manager = manager;
    }

    /// <summary>
    /// Detecta cuando el jugador toca el item de daño.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Obtener el componente de efecto de daño del jugador
            PlayerDamageEffect damageEffect = other.GetComponent<PlayerDamageEffect>();

            if (damageEffect != null)
            {
                // Aplicar el efecto de daño al jugador
                damageEffect.ApplyDamageEffect();
            }

            // Notificar al manager para destruir este item
            manager.CollectDamageItem(transform);
        }
    }
}