using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public enum AnimState
    {
        Idle = 0,
        Run,
        Jump,
        Duck,
        Death
    }

    [System.Serializable]
    public class AnimationData
    {
        public string name;
        public Sprite[] sprites;
        public float frameRate = 12f;
    }

    public AnimationData[] animations;
    private SpriteRenderer spriteRenderer;
    private int frame;
    private AnimState currentState = AnimState.Run;
    private float frameTimer;

    // 기존 sprites 필드 유지(이전 코드와의 호환성을 위해)
    public Sprite[] sprites;
    
    // 게임 속도와 관계없이 애니메이션 속도 조절할 수 있는 스케일링 값
    [Range(0.1f, 2.0f)]
    public float animationSpeedScale = 1.0f;
    
    // 게임 속도에 따라 애니메이션 속도를 조절할지 여부
    public bool useGameSpeedForAnimation = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // 기존의 단일 애니메이션 호환성 코드
        if (animations == null || animations.Length == 0)
        {
            // 기존 방식으로 애니메이션 처리
            Invoke(nameof(LegacyAnimate), 0f);
            return;
        }

        ChangeState(AnimState.Run);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        // 예외 처리: animations 배열이 비어있거나 초기화되지 않은 경우
        if (animations == null || animations.Length == 0)
        {
            // 기존 방식으로 애니메이션 처리
            return;
        }

        // 이름으로 애니메이션 찾기
        AnimationData currentAnim = FindAnimationByName(currentState.ToString());
        
        if (currentAnim == null)
        {
            Debug.LogWarning($"No animation found for state {currentState}!");
            return;
        }

        // 스프라이트 배열 확인
        if (currentAnim.sprites == null || currentAnim.sprites.Length == 0)
        {
            Debug.LogWarning($"No sprites found for animation state {currentState}!");
            return;
        }

        // 프레임 업데이트
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            float frameRate = currentAnim.frameRate;
            if (frameRate <= 0) frameRate = 12f; // 기본값
            
            // 게임 속도와 애니메이션 속도 분리
            float speed = animationSpeedScale;
            
            // 게임 속도에 비례하여 애니메이션 속도를 조절할지 여부
            if (useGameSpeedForAnimation && GameManager.Instance != null)
            {
                // 게임 속도가 너무 빠르지 않도록 제한 (최대 1.5배)
                float gameSpeedFactor = Mathf.Min(GameManager.Instance.gameSpeed / GameManager.Instance.initialGameSpeed, 1.5f);
                speed *= gameSpeedFactor > 0 ? gameSpeedFactor : 1f;
            }

            frameTimer = 1f / (frameRate * speed);
            frame = (frame + 1) % currentAnim.sprites.Length;
            spriteRenderer.sprite = currentAnim.sprites[frame];
        }
    }

    // 기존 애니메이션 메서드(이전 코드와의 호환성을 위해)
    private void LegacyAnimate()
    {
        frame++;

        if (frame >= sprites.Length)
        {
            frame = 0;
        }

        if (frame >= 0 && frame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[frame];
        }

        float speed = animationSpeedScale;
        
        // 게임 속도에 비례하여 애니메이션 속도를 조절할지 여부
        if (useGameSpeedForAnimation && GameManager.Instance != null)
        {
            // 게임 속도가 너무 빠르지 않도록 제한 (최대 1.5배)
            float gameSpeedFactor = Mathf.Min(GameManager.Instance.gameSpeed / GameManager.Instance.initialGameSpeed, 1.5f);
            speed *= gameSpeedFactor > 0 ? gameSpeedFactor : 1f;
        }

        Invoke(nameof(LegacyAnimate), 1f / (12f * speed));
    }

    private AnimationData FindAnimationByName(string animName)
    {
        foreach (AnimationData anim in animations)
        {
            if (anim.name == animName)
            {
                return anim;
            }
        }
        return null;
    }

    public void ChangeState(AnimState newState)
    {
        // 예외 처리: animations 배열이 비어있거나 초기화되지 않은 경우
        if (animations == null || animations.Length == 0)
        {
            Debug.LogWarning("Cannot change animation state: animations array is not initialized!");
            return;
        }

        // 상태가 동일하면 무시
        if (currentState == newState) return;

        currentState = newState;
        frame = 0;
        frameTimer = 0;

        // 이름으로 애니메이션 찾기
        AnimationData currentAnim = FindAnimationByName(currentState.ToString());
        
        if (currentAnim != null && currentAnim.sprites != null && currentAnim.sprites.Length > 0)
        {
            spriteRenderer.sprite = currentAnim.sprites[0];
        }
        else
        {
            Debug.LogWarning($"No animation data found for state {currentState}!");
        }
    }
}
