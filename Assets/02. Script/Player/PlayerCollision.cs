using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerCollision : MonoBehaviour
{
    private PlayerMovement movement;
    private bool isDead = false;
    private bool isStartPoint = false;
    private Block currentBlock;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        // 블록 위에 있고, 땅에 착지한 상태일 때만 타이머 감소
        if (currentBlock != null && movement.GetGrounded())
            GameManager.instance.TimerSub();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("isGrounded") || collision.gameObject.CompareTag("isBug"))
        {
            isDead = true;
            GameManager.instance.GameOver();
            return;
        }

        if (collision.gameObject.CompareTag("StartPoint"))
        {
            isStartPoint = true;
            movement.SetGrounded(true);
            GameManager.instance.TimerInit();
        }
        else
        {
            isStartPoint = false;
        }

        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f) // 위쪽을 향한 충돌
            {
                Block block = collision.gameObject.GetComponent<Block>();
                if (block != null)
                {
                    if (currentBlock == null || currentBlock != block)
                    {
                        currentBlock = block;
                        GameManager.instance.TimerInit();
                    }

                    movement.SetGrounded(true);
                }
                break; // 하나라도 OK면 더 체크 안 함
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Goal"))
        {
            GameManager.instance.GameClear();
        }

        if (other.CompareTag("isBug"))
        {
            isDead = true;
            GameManager.instance.GameOver();
        }
    }
}
