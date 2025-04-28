using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    [Header("Move && Jump Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 8f;
    public float acceleration = 20f; // 가속도

    private Rigidbody rb;
    private Vector3 movement;
    private Vector3 currentVelocity; // 현재 이동 속도 저장

    [SerializeField] float jumpForceValue;
    private bool isGrounded = true;
    private bool jumpPressed = false; // 점프 한번만 눌리도록 막기위해
    bool canMoveing = true;
    private Block currentBlock = null; // 발판 블럭 체크용
    public bool isReplayMode = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
            jumpPressed = true;
    }

    private void FixedUpdate()
    {
        // 리플레이 컷신재생중일땐 움직임 제한
        if (ReplayManager.Instance.IsReplaying || !canMoveing || CutScene.instance.showCutScene || isDead) return;
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

        if (currentBlock != null && isGrounded)
            GameManager.instance.TimerSub();
    }

    private void Jump()
    {

        Vector3 currentVel = rb.linearVelocity;
        rb.linearVelocity = new Vector3(0f, currentVel.y, 0f);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("isBug"))
            GameManager.instance.GameOver();

        if(other.gameObject.CompareTag("Goal"))
        {
            // 페이드인 페이드아웃
            // 클리어
            // 타이틀씬으로
        }
    }

    bool isBlockOn = false;
    bool isDead = false;
    private void OnCollisionEnter(Collision collision)
    {
        // 땅에 부딪히면 초기화
        if (collision.gameObject.CompareTag("isGrounded"))
            GameManager.instance.GameOver();

        if (collision.gameObject.CompareTag("StartPoint"))
            isGrounded = true;

        if (isDead) return; // 이미 죽었으면 무시

        if (collision.gameObject.CompareTag("isBug"))
        {
            isDead = true;
            GameManager.instance.GameOver();
        }

        // 발판 감지
        Block block = collision.gameObject.GetComponent<Block>();
        if (block != null)
        {
            // 다른 발판으로 이동한 경우
            if (currentBlock == null || currentBlock != block)
            {
                currentBlock = block;
                GameManager.instance.TimerInit(); // 타이머 초기화
                isBlockOn = true;
            }
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
       // GameManager.instance.TimerInit(); // 타이머 초기화
    }

}
