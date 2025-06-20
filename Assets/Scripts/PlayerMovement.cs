using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Speeds")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 10f;
    [Header("Jump")]
    public float jumpForce = 5f;
    public float gravityMultiplier = 2f;

    private WeaponShooting weaponShooting;

    private CharacterController controller;
    private float verticalVelocity;
    private Animator animator;
    

    void Start()
    {
        controller = GetComponent<CharacterController>();
        weaponShooting = GetComponent<WeaponShooting>();
        animator = GetComponentInChildren<Animator>(); 
    }

    void Update()
    {
        // --- Movement (WASD) ---
        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) input += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) input += Vector3.back;
        if (Input.GetKey(KeyCode.A)) input += Vector3.left;
        if (Input.GetKey(KeyCode.D)) input += Vector3.right;
        input = input.normalized;

        // --- Camera-relative movement (optional) ---
        Vector3 direction = input;
        if (Camera.main != null)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            direction = (input.z * camForward + input.x * camRight).normalized;
        }

        // --- Speed & Sprint ---
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float speed = isSprinting ? sprintSpeed : walkSpeed;
        float moveSpeed = direction.magnitude * speed;

        controller.Move(direction * speed * Time.deltaTime);
        animator.SetFloat("Speed", moveSpeed);

        // --- 🧭 Face movement direction ---
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // --- Jump & Gravity (Space) ---
        bool isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = jumpForce;
            animator.SetTrigger("Jump");
        }

        if (!isGrounded)
        {
            verticalVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }

        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    
        // --- Fire logic (Left Mouse Button or a specific key) ---
        if (weaponShooting != null && Input.GetMouseButton(0)) 
        {
            Debug.Log("Fire!");
            weaponShooting.TryFire();
        }
    }
}
