using UnityEngine;

public class PlayerSpeedBallDetector : MonoBehaviour
{
    private SpeedBallItemManager manager;

    /// <summary>
    /// Inicializa el detector con referencia al manager.
    /// </summary>
    public void Init(SpeedBallItemManager manager)
    {
        this.manager = manager;
    }

    /// <summary>
    /// Detecta cuando el jugador toca el item de velocidad.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Obtener el componente de efecto de velocidad del jugador
            PlayerSpeedBallEffect speedEffect = other.GetComponent<PlayerSpeedBallEffect>();

            if (speedEffect != null)
            {
                // Activar el efecto de velocidad
                speedEffect.ActivateSpeedBall();
            }

            // Notificar al manager para destruir este item
            manager.CollectSpeedBallItem(transform);
        }
    }
}