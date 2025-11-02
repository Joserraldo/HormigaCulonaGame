using UnityEngine;

// Controlador básico de personaje solo con movimiento y salto
public class New_CharacterController : MonoBehaviour
{
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
        Vector3 moveDirection = inputDirection;

        if (IsMoving)
        {
            bool isSprinting = Input.GetKey(KeyCode.LeftShift);
            currentSpeed = isSprinting ? SprintSpeed : WalkSpeed;
            moveDirection *= currentSpeed;
        }

        if (Input.GetButtonDown("Jump") && IsGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMovement = moveDirection * Time.deltaTime;
        finalMovement.y = velocity.y * Time.deltaTime;

        characterController.Move(finalMovement);
    }
}
