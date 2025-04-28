using UnityEngine;

[RequireComponent(typeof(AnimatedSprite))]
public class PlayerAnimator : MonoBehaviour
{
    private AnimatedSprite animatedSprite;
    private Player player;
    
    [Header("Animation States")]
    public bool isGrounded = true;
    public bool isDucking = false;
    public bool isDead = false;
    
    private void Awake()
    {
        animatedSprite = GetComponent<AnimatedSprite>();
        player = GetComponent<Player>();
    }
    
    private void Update()
    {
        UpdateAnimationState();
    }
    
    private void UpdateAnimationState()
    {
        // animatedSprite가 null이면 리턴
        if (animatedSprite == null)
        {
            return;
        }
        
        // 죽음 상태가 최우선
        if (isDead)
        {
            animatedSprite.ChangeState(AnimatedSprite.AnimState.Death);
            return;
        }
        
        // 숙이기 상태 확인
        if (isDucking && isGrounded)
        {
            animatedSprite.ChangeState(AnimatedSprite.AnimState.Duck);
        }
        // 공중에 있을 때 - Jump 애니메이션이 없으므로 Run 사용
        else if (!isGrounded)
        {
            animatedSprite.ChangeState(AnimatedSprite.AnimState.Run);
        }
        // 달리기 상태
        else
        {
            animatedSprite.ChangeState(AnimatedSprite.AnimState.Run);
        }
    }
    
    public void PlayDeath()
    {
        isDead = true;
        UpdateAnimationState();
    }
    
    public void ResetStates()
    {
        isDead = false;
        isDucking = false;
        isGrounded = true;
        
        // animatedSprite가 초기화되었는지 확인
        if (animatedSprite == null)
        {
            animatedSprite = GetComponent<AnimatedSprite>();
        }
        
        if (animatedSprite != null)
        {
            animatedSprite.ChangeState(AnimatedSprite.AnimState.Run);
        }
    }
}
