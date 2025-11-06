using UnityEngine;

// Asegúrate de que el nombre de esta clase coincida con la referencia en ColletibleMangerS
public class PlayerCollectibleDetector : MonoBehaviour
{
    // Utilizamos ColletibleMangerS como tipo para el manager, basándonos en el script anterior.
    private ColletibleMangerS manager;

    /// <summary>
    /// Inicializa el detector, guardando una referencia al script gestor (manager).
    /// Este método es llamado por el ColletibleMangerS en el Start().
    /// </summary>
    /// <param name="manager">Referencia al script que maneja la colección.</param>
    public void Init(ColletibleMangerS manager)
    {
        this.manager = manager;
    }

    /// <summary>
    /// Se llama cuando otro Collider entra en el trigger de este objeto.
    /// Este método ahora incluye la llamada al script PlayerAbilities del jugador.
    /// </summary>
    /// <param name="other">El Collider que ha entrado en el trigger.</param>
    void OnTriggerEnter(Collider other)
    {
        // 1. Verifica si el objeto que colisionó es el "Player"
        if (other.CompareTag("Player"))
        {
            // --- Lógica de Power-Up y Recolección ---
            
            // a. Obtener el script PlayerAbilities del objeto "other" (el Player)
            // NOTA: Es crucial que el script PlayerAbilities esté asignado al GameObject que tiene el Collider.
            PlayerAbilities player = other.GetComponent<PlayerAbilities>();

            if (player != null)
            {
                // b. Activar el power-up en el script del jugador (cambia velocidad/escala)
                player.ActivatePowerUp();
            }
            // -----------------------------------------
            
            // c. Llama al método Collect del Manager para destruir este objeto recolectable
            manager.Collect(transform);
        }
    }
}