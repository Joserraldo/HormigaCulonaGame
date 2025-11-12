using UnityEngine;
using TMPro;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Configuración de Trepa")]
    public float climbDuration = 10f; // Duración del efecto de trepa en segundos
    public TextMeshProUGUI timerText; // Arrastra el TextMeshPro aquí desde el Inspector

    private bool canClimb = false;
    private float climbTimer = 0f;

    void Start()
    {
        // Inicializar el timer como vacío o 0
        if (timerText != null)
        {
            timerText.text = "";
        }
    }

    void Update()
    {
        if (canClimb)
        {
            climbTimer -= Time.deltaTime;
            if (timerText != null)
            {
                timerText.text = Mathf.Ceil(climbTimer).ToString();
            }

            if (climbTimer <= 0f)
            {
                DeactivateClimb();
            }
        }
    }

    /// <summary>
    /// Activa la habilidad de trepar durante climbDuration segundos.
    /// Este método será llamado por el PlayerCollectibleDetector.
    /// </summary>
    public void ActivatePowerUp()
    {
        canClimb = true;
        climbTimer = climbDuration;
        Debug.Log("Habilidad de trepar activada por " + climbDuration + " segundos");
    }

    /// <summary>
    /// Desactiva la habilidad de trepar.
    /// </summary>
    private void DeactivateClimb()
    {
        canClimb = false;
        climbTimer = 0f;
        if (timerText != null)
        {
            timerText.text = "";
        }
        Debug.Log("Habilidad de trepar desactivada");
    }

    /// <summary>
    /// Propiedad para acceder a canClimb desde otros scripts.
    /// </summary>
    public bool CanClimb
    {
        get { return canClimb; }
    }
}
