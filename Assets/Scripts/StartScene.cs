using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startButton;
    public Button exitButton;
    public Image dinoImage;
    
    [Header("Settings")]
    public string gameSceneName = "Game";
    public float dinoBounceDuration = 1.5f;
    public float bounceHeight = 20f;
    
    private Vector3 initialDinoPosition;
    private float bounceTimer;

    private void Awake()
    {
        // 버튼 이벤트 연결
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }
        
        // 공룡 초기 위치 저장
        if (dinoImage != null)
        {
            initialDinoPosition = dinoImage.rectTransform.anchoredPosition;
        }
    }
    
    private void Update()
    {
        // 공룡 애니메이션 업데이트
        if (dinoImage != null)
        {
            bounceTimer += Time.deltaTime;
            float normalizedTime = (bounceTimer % dinoBounceDuration) / dinoBounceDuration;
            float yOffset = Mathf.Sin(normalizedTime * Mathf.PI) * bounceHeight;
            
            dinoImage.rectTransform.anchoredPosition = initialDinoPosition + new Vector3(0, yOffset, 0);
        }
        
        // 스페이스바 입력으로도 게임 시작
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}