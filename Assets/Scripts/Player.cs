using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;

    public float jumpForce = 8f;
    public float gravity = 9.81f * 2f;

    // 숙이기 관련 변수
    private bool isDucking = false;
    public Transform standingCollider;
    public Transform duckingCollider;
    
    // 애니메이션 관련
    private PlayerAnimator playerAnimator;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
        if (playerAnimator != null)
        {
            playerAnimator.ResetStates();
        }
    }

    private void Update()
    {
        direction += gravity * Time.deltaTime * Vector3.down;

        if (character.isGrounded)
        {
            direction = Vector3.down;

            // 점프 입력 처리
            if (Input.GetButton("Jump") && !isDucking)
            {
                direction = Vector3.up * jumpForce;
                PlayJumpSound();
            }

            // 숙이기 입력 처리
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                if (!isDucking)
                {
                    Duck();
                }
            }
            else if (isDucking)
            {
                StandUp();
            }
        }

        // 애니메이터 상태 업데이트
        if (playerAnimator != null)
        {
            playerAnimator.isGrounded = character.isGrounded;
            playerAnimator.isDucking = isDucking;
        }

        character.Move(direction * Time.deltaTime);
    }

    private void Duck()
    {
        isDucking = true;

        // 숙이기 콜라이더로 전환
        if (standingCollider != null && duckingCollider != null)
        {
            standingCollider.gameObject.SetActive(false);
            duckingCollider.gameObject.SetActive(true);
        }

        PlayDuckSound();
    }

    private void StandUp()
    {
        isDucking = false;

        // 서있는 콜라이더로 전환
        if (standingCollider != null && duckingCollider != null)
        {
            standingCollider.gameObject.SetActive(true);
            duckingCollider.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            // 죽음 애니메이션 재생
            if (playerAnimator != null)
            {
                playerAnimator.PlayDeath();
            }
            
            PlayDeathSound();
            GameManager.Instance.GameOver();
        }
    }

    // 소리 재생 함수들
    private void PlayJumpSound()
    {
        AudioManager.Instance?.PlaySound("Jump");
    }

    private void PlayDuckSound()
    {
        AudioManager.Instance?.PlaySound("Duck");
    }

    private void PlayDeathSound()
    {
        AudioManager.Instance?.PlaySound("GameOver");
    }
}
