using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move & Jump Settings")]
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float jumpForce = 8f;

    private Rigidbody rb;
    private Vector3 moveDir;
    private bool isGrounded = false;
    private bool jumpRequested = false;

    public bool CanMove = true;


    // 외부에서 착지 상태 설정
    public void SetGrounded(bool grounded) => isGrounded = grounded;
    // 외부에서 착지 상태 조회
    public bool GetGrounded() => isGrounded;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // 컷신/리플레이 중이거나 이동 금지 상태면 무시
        if (!CanMove || ReplayManager.Instance.IsReplaying || CutScene.instance.showCutScene) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 camF = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camR = Camera.main.transform.right;

        moveDir = (camF * v + camR * h).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            jumpRequested = true;
    }

    private void FixedUpdate()
    {
        if (!CanMove || ReplayManager.Instance.IsReplaying || CutScene.instance.showCutScene) return;

        Vector3 force = moveDir * moveSpeed;
        rb.AddForce(isGrounded ? force : force * 0.5f, ForceMode.Force);

        if (jumpRequested)
        {
            Vector3 currentVel = rb.linearVelocity;
            rb.linearVelocity = new Vector3(0f, currentVel.y, 0f); 

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            SoundManager.Instance.PlaySFXSound("jumpSfx");
            jumpRequested = false;
        }
    }

   

}
