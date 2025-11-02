using UnityEngine;

// Teletransporta al jugador al punto de respawn cuando entra en el trigger
public class Respawn_New : MonoBehaviour
{
    [Header("Referencia al punto de respawn")]
    public Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        // Solo activa si el objeto tiene la etiqueta "Player" y el respawnPoint está asignado
        if (other.CompareTag("Player") && respawnPoint != null)
        {
            // Intenta conseguir el CharacterController del objeto
            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null)
            {
                // Deshabilita el CharacterController para mover el transform (evita conflictos de física)
                controller.enabled = false;
                other.transform.position = respawnPoint.position;
                controller.enabled = true;

                Debug.Log("Jugador llegó al punto de respawn.");
            }
            else
            {
                // Si el objeto no tiene CharacterController, simplemente mueve el transform
                other.transform.position = respawnPoint.position;
                Debug.Log("Jugador sin CharacterController fue movido al respawn.");
            }
        }
    }
}

