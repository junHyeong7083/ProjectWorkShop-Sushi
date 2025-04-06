using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move && Jump")]
    public float moveSpeed;
    public float jumpForce;

    private Rigidbody rb;
    private Animator animator;
    private Vector3 movement;
    private bool isGrounded = true;
    private bool jumpPressed = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("isGrounded"))
            isGrounded = true;
    }

    private void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

   
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Camera.main.transform.right;
     
        movement = (camForward * inputZ + camRight * inputX).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpPressed = true;
        }
    }

    private void FixedUpdate()
    {
        // �̵�
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        if (jumpPressed)
        {
            Jump();
            jumpPressed = false; 
        }
    }

    private void Jump()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.y = jumpForce;
        rb.linearVelocity = velocity;
        isGrounded = false;
    }
}
