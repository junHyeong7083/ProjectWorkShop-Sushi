using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    [Header("Move && Jump Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 8f;
    public float acceleration = 20f; // 가속도

    private Rigidbody rb;
    private Animator animator;
    private Vector3 movement;
    private Vector3 currentVelocity; // 현재 이동 속도 저장

    [SerializeField] float jumpForceValue;
    private bool isGrounded = true;
    private bool jumpPressed = false;
    private bool isJump = false;
    bool canMoveing = true;
    private Block currentBlock = null; // 발판 블럭 체크용
    public bool isReplayMode = false;
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
        if (ReplayManager.Instance.IsReplaying || !canMoveing) return;
        // 목표 힘 설정
        Vector3 targetForce = movement * moveSpeed;

        if (isGrounded)
            rb.AddForce(targetForce, ForceMode.Force);
        else
            rb.AddForce(targetForce * 0.5f, ForceMode.Force);  


        if (jumpPressed)
        {
            Jump();
            jumpPressed = false;
        }
    }

    private void Jump()
    {
        isJump = true;

        Vector3 currentVel = rb.linearVelocity;
        rb.linearVelocity = new Vector3(0f, currentVel.y, 0f);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("isBug"))
            GameManager.instance.GameOver();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 땅에 부딪히면 초기화
        if (collision.gameObject.CompareTag("isGrounded"))
            GameManager.instance.GameOver();

        if (collision.gameObject.CompareTag("deadCollision"))
            canMoveing = false;
        // 발판 감지
        Block block = collision.gameObject.GetComponent<Block>();
        if (block != null)
        {
            // 다른 발판으로 이동한 경우
            if (currentBlock == null || currentBlock != block)
            {
                currentBlock = block;
                GameManager.instance.TimerInit(); // 타이머 초기화
            }
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (currentBlock != null && collision.gameObject == currentBlock.gameObject)
        {
            GameManager.instance.TimerSub();
        }
    }

}
