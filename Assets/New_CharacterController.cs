using UnityEngine;

// Controlador básico de personaje solo con movimiento y salto
public class New_CharacterController : MonoBehaviour
{
    // AÑADIR ESTA REFERENCIA 🎯
    [Header("Referencias de Cámara")]
    public Transform cameraTransform; // Arrastra el objeto de la cámara aquí

    public float gravity = -20f;
    public float SprintSpeed = 8f;
    public float WalkSpeed = 4f;
    public float jumpHeight = 2f;

    private CharacterController characterController;
    private Vector3 velocity;
    private float currentSpeed;

    public bool IsMoving { get; private set; }
    public bool IsGrounded { get; private set; }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        // Opcional: Si el script de la cámara está en el hijo del personaje, puedes buscarlo
        if (cameraTransform == null)
        {
            Debug.LogError("¡ERROR! Falta asignar la 'cameraTransform' en el Inspector.");
        }
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        IsGrounded = characterController.isGrounded;

        if (IsGrounded && velocity.y < 0)
            velocity.y = -2f; // Mantiene al personaje pegado al suelo

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        IsMoving = inputDirection.magnitude > 0.1f;
        
        // 🔑 CAMBIOS CLAVE: Rotar el vector de movimiento según la cámara.

        Vector3 moveDirection = Vector3.zero;

        if (IsMoving)
        {
            // 1. Obtener la rotación de la cámara (SOLO EJE Y)
            Quaternion cameraRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

            // 2. Rotar el vector de entrada (WASD) usando la rotación de la cámara.
            moveDirection = cameraRotation * inputDirection;

            // Mantener la dirección de movimiento horizontal
            moveDirection.y = 0; 
            moveDirection.Normalize();

            bool isSprinting = Input.GetKey(KeyCode.LeftShift);
            currentSpeed = isSprinting ? SprintSpeed : WalkSpeed;
            moveDirection *= currentSpeed;
        }
        
        // El resto del código de gravedad sigue igual
        
        if (Input.GetButtonDown("Jump") && IsGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMovement = moveDirection * Time.deltaTime;
        finalMovement.y = velocity.y * Time.deltaTime; // Aplica la velocidad vertical

        characterController.Move(finalMovement);
    }
}