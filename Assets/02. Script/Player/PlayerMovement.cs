using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move && Jump Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 8f;
    public float acceleration = 20f; // 가속도

    private Rigidbody rb;
    private Animator animator;
    private Vector3 movement;
    private Vector3 currentVelocity; // 현재 이동 속도 저장
    private bool isGrounded = true;
    private bool jumpPressed = false;

    bool isTimerSub = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 입력 받기
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
        // 목표 속도 설정
        Vector3 targetVelocity = movement * moveSpeed;

        // 현재 속도를 목표 속도로 부드럽게 가속
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        // Rigidbody에 적용 (Y는 원래 중력 사용)
        rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);

        if (jumpPressed)
        {
            Jump();
            jumpPressed = false;
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("isGrounded") || collision.gameObject.CompareTag("isWalkable"))
            isGrounded = true;

        if (collision.gameObject.CompareTag("isWalkable"))
            isTimerSub = true;
        if (collision.gameObject.CompareTag("isGrounded") && isTimerSub)
            GameManager.instance.TimerSub();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("isGrounded") && isTimerSub)
            GameManager.instance.TimerSub();
    }

}
