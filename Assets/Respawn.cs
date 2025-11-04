using UnityEngine;

// Teletransporta al jugador al punto de respawn cuando entra en el trigger
public class Respawn_New : MonoBehaviour
{
    [Header("Referencia al punto de respawn")]
    public Transform respawnPoint;

    // ✨ Nueva variable: Altura fija de respawn
    [Header("Altura fija de respawn")]
    public float respawnHeight = 4.55f; 

    private void OnTriggerEnter(Collider other)
    {
        // Solo activa si el objeto tiene la etiqueta "Player" y el respawnPoint está asignado
        if (other.CompareTag("Player") && respawnPoint != null)
        {
            // 🆕 Creamos un nuevo Vector3 basado en la posición del respawnPoint
            // pero usamos la altura (Y) fija que definimos (4.55f).
            Vector3 newRespawnPosition = new Vector3(
                respawnPoint.position.x, 
                respawnHeight, // <<-- ¡USAMOS LA ALTURA FIJA AQUÍ!
                respawnPoint.position.z
            );
            
            // Intenta conseguir el CharacterController del objeto
            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null)
            {
                // Deshabilita el CharacterController para mover el transform (evita conflictos de física)
                controller.enabled = false;
                
                // 🔄 Asignamos la nueva posición con la altura modificada
                other.transform.position = newRespawnPosition; 
                
                controller.enabled = true;

                Debug.Log("Jugador teletransportado a X:" + newRespawnPosition.x + ", Y:" + newRespawnPosition.y + ", Z:" + newRespawnPosition.z);
            }
            else
            {
                // Si el objeto no tiene CharacterController, simplemente mueve el transform
                // 🔄 Asignamos la nueva posición con la altura modificada
                other.transform.position = newRespawnPosition; 
                
                Debug.Log("Jugador sin CharacterController fue movido al respawn.");
            }
        }
    }
}