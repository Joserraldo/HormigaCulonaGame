using UnityEngine;

// Controlador básico de personaje solo con movimiento y salto
public class New_CharacterController : MonoBehaviour
{
    // AÑADIR ESTA REFERENCIA 🎯
    [Header("Referencias de Cámara")]
    public Transform cameraTransform; // Arrastra el objeto de la cámara aquí

    [Header("Referencias de Habilidades")]
    public PlayerAbilities playerAbilities; // Arrastra el script PlayerAbilities aquí

    public float gravity = -20f;
    public float SprintSpeed = 8f;
    public float WalkSpeed = 4f;
    public float jumpHeight = 2f;
    public float climbSpeed = 4f; // Velocidad de trepa

    private CharacterController characterController;
    private Vector3 velocity;
    private float currentSpeed;
    private float controllerHeight; // Altura del CharacterController para raycasts
    private float climbingTimer = 0f; // Timer para mantener estado de trepa

    public bool IsMoving { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsClimbing { get; private set; }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        controllerHeight = characterController.height; // Guardar la altura del CharacterController
        // Opcional: Si el script de la cámara está en el hijo del personaje, puedes buscarlo
        if (cameraTransform == null)
        {
            Debug.LogError("¡ERROR! Falta asignar la 'cameraTransform' en el Inspector.");
        }
        if (playerAbilities == null)
        {
            playerAbilities = GetComponent<PlayerAbilities>();
            if (playerAbilities == null)
            {
                Debug.LogError("¡ERROR! Falta asignar 'playerAbilities' o el script PlayerAbilities no está en el mismo GameObject.");
            }
        }
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        IsGrounded = characterController.isGrounded;
        IsClimbing = false;

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

        // Lógica de trepa: Si puede trepar y está tocando una pared "Climbable"
        if (playerAbilities != null && playerAbilities.CanClimb)
        {
            // Usar múltiples raycasts para detectar pared enfrente, cubriendo la altura del CharacterController
            bool wallDetected = false;
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            float rayDistance = 1.5f; // Distancia del raycast

            // Raycast desde el centro
            RaycastHit hitCenter;
            if (Physics.Raycast(transform.position, forward, out hitCenter, rayDistance) && hitCenter.collider.CompareTag("Climbable"))
            {
                wallDetected = true;
            }
            // Raycast desde arriba (mitad de altura)
            RaycastHit hitUp;
            if (!wallDetected && Physics.Raycast(transform.position + Vector3.up * (controllerHeight / 2), forward, out hitUp, rayDistance) && hitUp.collider.CompareTag("Climbable"))
            {
                wallDetected = true;
            }
            // Raycast desde abajo (mitad de altura)
            RaycastHit hitDown;
            if (!wallDetected && Physics.Raycast(transform.position - Vector3.up * (controllerHeight / 2), forward, out hitDown, rayDistance) && hitDown.collider.CompareTag("Climbable"))
            {
                wallDetected = true;
            }

            if (wallDetected)
            {
                IsClimbing = true;
                climbingTimer = 0.2f; // Mantener estado de trepa por 0.2 segundos después de perder contacto
                // Permitir movimiento vertical con input vertical
                float climbInput = Input.GetAxis("Vertical");
                velocity.y = climbInput * climbSpeed;
                // Mantener gravedad baja o cero mientras trepa
                gravity = 0f;
                // Evitar que caiga si está en la cima: si no hay input, mantener posición
                if (climbInput == 0)
                {
                    velocity.y = 0f;
                }
            }
            else if (climbingTimer > 0f)
            {
                // Mantener estado de trepa por un breve tiempo después de perder contacto
                climbingTimer -= Time.deltaTime;
                IsClimbing = true;
                float climbInput = Input.GetAxis("Vertical");
                velocity.y = climbInput * climbSpeed;
                gravity = 0f;
                if (climbInput == 0)
                {
                    velocity.y = 0f;
                }
            }
            else
            {
                gravity = -20f; // Restaurar gravedad normal
            }
        }
        else
        {
            gravity = -20f; // Restaurar gravedad normal
        }

        // El resto del código de gravedad sigue igual, pero solo si no está trepando
        if (!IsClimbing)
        {
            if (Input.GetButtonDown("Jump") && IsGrounded)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            velocity.y += gravity * Time.deltaTime;
        }

        Vector3 finalMovement = moveDirection * Time.deltaTime;
        finalMovement.y = velocity.y * Time.deltaTime; // Aplica la velocidad vertical

        characterController.Move(finalMovement);
    }
}
