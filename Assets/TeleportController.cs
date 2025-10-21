using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TeleportController : MonoBehaviour
{
    
    [Header("Referencias de UI")]
    [Tooltip("Arrastra el componente Button de tu UI aquí.")]
    [SerializeField] private Button actionButton;
    [Tooltip("Arrastra el componente TextMeshProUGUI del texto del botón aquí.")]
    [SerializeField] private TextMeshProUGUI buttonText;
    
    
    [Header("Parámetros de Teletransporte")]
    [Tooltip("Define el área máxima (radio) para el teletransporte.")]
    [SerializeField] private float teleportRadius = 5f;
    [Tooltip("La posición central desde donde se calcula el radio.")]
    [SerializeField] private Vector3 centerOffset = Vector3.zero;

    private Vector3 initialPosition; 
    private bool teleported = false; 

    void Start()
    {
        
        initialPosition = transform.position;

        
        if (actionButton != null)
        {
            actionButton.onClick.AddListener(TeleportAndControlUI);
        }
        
        
        if (buttonText != null)
        {
            buttonText.text = "*se tepea epicamente 👻";
        }
    }

    
    public void TeleportAndControlUI()
    {
        if (teleported)
        {
            
            TeleportTo(initialPosition);
            teleported = false;
            
            
            if (buttonText != null)
            {
                buttonText.text = "se tepea epicamente";
            }
        }
        else
        {
            
            Vector3 randomPos = GetRandomTeleportPosition();
            TeleportTo(randomPos);
            teleported = true;
            
            
            if (buttonText != null)
            {
                buttonText.text = "¡es un enderman!";
            }
        }
    }

    
    private Vector3 GetRandomTeleportPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * teleportRadius;
        
        
        Vector3 newPosition = transform.position + centerOffset;
        newPosition.x += randomCircle.x;
        newPosition.z += randomCircle.y;
        
        
        newPosition.y = initialPosition.y; 
        
        return newPosition;
    }

    
    private void TeleportTo(Vector3 targetPosition)
    {
        transform.position = targetPosition;
        
        Debug.Log($"Personaje teletransportado a: {targetPosition}");
    }
}