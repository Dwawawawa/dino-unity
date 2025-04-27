using UnityEngine;
using UnityEditor;

public class AnimationSetup : EditorWindow
{
    [MenuItem("Tools/Setup Player Animation")]
    public static void SetupPlayerAnimation()
    {
        // Player 오브젝트 찾기
        Player player = FindObjectOfType<Player>();
        if (player == null)
        {
            Debug.LogError("Player object not found in scene!");
            return;
        }
        
        // AnimatedSprite 컴포넌트 확인
        AnimatedSprite animatedSprite = player.GetComponent<AnimatedSprite>();
        if (animatedSprite == null)
        {
            Debug.LogError("AnimatedSprite component not found on Player!");
            return;
        }
        
        // PlayerAnimator 컴포넌트 추가 또는 가져오기
        PlayerAnimator playerAnimator = player.GetComponent<PlayerAnimator>();
        if (playerAnimator == null)
        {
            playerAnimator = player.gameObject.AddComponent<PlayerAnimator>();
            Debug.Log("PlayerAnimator component added to Player");
        }
        
        // 애니메이션 데이터 설정 (예시)
        if (animatedSprite.animations == null || animatedSprite.animations.Length == 0)
        {
            animatedSprite.animations = new AnimatedSprite.AnimationData[5];
            
            // Idle
            animatedSprite.animations[0] = new AnimatedSprite.AnimationData
            {
                name = "Idle",
                frameRate = 12f,
                sprites = new Sprite[0] // 실제 스프라이트는 수동으로 할당 필요
            };
            
            // Run
            animatedSprite.animations[1] = new AnimatedSprite.AnimationData
            {
                name = "Run",
                frameRate = 12f,
                sprites = new Sprite[0]
            };
            
            // Jump
            animatedSprite.animations[2] = new AnimatedSprite.AnimationData
            {
                name = "Jump",
                frameRate = 12f,
                sprites = new Sprite[0]
            };
            
            // Duck
            animatedSprite.animations[3] = new AnimatedSprite.AnimationData
            {
                name = "Duck",
                frameRate = 12f,
                sprites = new Sprite[0]
            };
            
            // Death
            animatedSprite.animations[4] = new AnimatedSprite.AnimationData
            {
                name = "Death",
                frameRate = 12f,
                sprites = new Sprite[0]
            };
            
            EditorUtility.SetDirty(animatedSprite);
            Debug.Log("Animation data structure created. Please assign sprites manually in the Inspector.");
        }
        
        Debug.Log("Player animation setup completed!");
    }
}
