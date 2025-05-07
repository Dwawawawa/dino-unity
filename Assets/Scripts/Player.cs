using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    // 콜라이더 관련
    public BoxCollider2D headCollider;
    public BoxCollider2D feetCollider;
    
    // 컴포넌트
    private Rigidbody2D rb;
    private PlayerAnimator playerAnimator;
    
    // 상태 변수
    private bool isJumping = false;
    private bool isDucking = false;
    private bool isLongJumpApplied = false;
    
    // 점프 관련
    private float jumpPressTime = 0f;
    private bool isJumpPressed = false;
    private float shortJumpThreshold = 0.1f;
    
    // 점프 힘
    public float addShortForce = 5.0f;
    public float addLongForce = 8.0f;
    public float gravity = 1f;
    
    // 콜라이더 위치값 (서있을 때와 숙일 때)
    private Vector2 headStandingPosition = new Vector2(0.2045271f, 0.2826734f + 0.5f); 
    private Vector2 headStandingSize = new Vector2(0.4202094f, 0.3144546f );
    private Vector2 feetStandingPosition = new Vector2(-0.02893686f, -0.3953031f + 0.5f);
    private Vector2 feetStandingSize = new Vector2(0.4470406f, 0.2143997f );

    private Vector2 headDuckingPosition = new Vector2(0.3602087f, 0.08433926f + 0.3f);
    private Vector2 headDuckingSize = new Vector2(0.3988833f, 0.3528416f);
    private Vector2 feetDuckingPosition = new Vector2(-0.2287889f, -0.2266337f + 0.3f);
    private Vector2 feetDuckingSize = new Vector2(0.308136f, 0.1548693f);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<PlayerAnimator>();
        SetupStandingColliders();
    }

    private void OnEnable()
    {
        isJumping = false;
        isDucking = false;
        rb.gravityScale = gravity;
        SetupStandingColliders();
        
        if (playerAnimator != null)
        {
            playerAnimator.ResetStates();
        }
    }

    private void Update()
    {
        HandleJumpInput();
        HandleDuckInput();
        UpdateAnimatorState();
    }

    private void HandleJumpInput()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && !isJumping && !isDucking)
        {
            isJumpPressed = true;
            jumpPressTime = 0f;
            Jump(addShortForce);
        }

        if (isJumpPressed)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
            {
                jumpPressTime += Time.deltaTime;
                if (jumpPressTime > shortJumpThreshold && !isLongJumpApplied)
                {
                    ApplyAdditionalForce(addLongForce - addShortForce);
                    isLongJumpApplied = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                isJumpPressed = false;
            }
        }
    }

    private void HandleDuckInput()
    {
        // 공중에서 다운키를 누르면 빠르게 하강
        if (Input.GetKeyDown(KeyCode.DownArrow) && isJumping)
        {
            rb.gravityScale = 7f;
            return;
        }

        // 땅에서 숙이기
        if (Input.GetKey(KeyCode.DownArrow) && !isJumping)
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

    private void Jump(float force)
    {
        if (rb != null)
        {
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            isJumping = true;
            isLongJumpApplied = false;
            PlayJumpSound();
        }
    }

    private void ApplyAdditionalForce(float extraForce)
    {
        if (rb != null)
        {
            rb.AddForce(Vector2.up * extraForce, ForceMode2D.Impulse);
        }
    }

    private void Duck()
    {
        isDucking = true;
        SetupDuckingColliders();
        PlayDuckSound();
    }

    private void StandUp()
    {
        isDucking = false;
        SetupStandingColliders();
    }

    private void SetupStandingColliders()
    {
        
        if (headCollider != null)
        {
            headCollider.offset = headStandingPosition;
            headCollider.size = headStandingSize;
        }
        
        if (feetCollider != null)
        {
            feetCollider.offset = feetStandingPosition;
            feetCollider.size = feetStandingSize;
        }
    }

    private void SetupDuckingColliders()
    {
        
        if (headCollider != null)
        {
            headCollider.offset = headDuckingPosition;
            headCollider.size = headDuckingSize;
        }
        
        if (feetCollider != null)
        {
            feetCollider.offset = feetDuckingPosition;
            feetCollider.size = feetDuckingSize;
        }
    }

    private void UpdateAnimatorState()
    {
        if (playerAnimator != null)
        {
            playerAnimator.isGrounded = !isJumping;
            playerAnimator.isDucking = isDucking;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            rb.gravityScale = gravity;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
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
