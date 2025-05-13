using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 게임 오버 상태를 표현하고, 게임 점수와 UI를 관리하는 게임 매니저
// 씬에는 단 하나의 게임 매니저만 존재할 수 있다.
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤을 할당할 전역 변수

    public bool isGameover = false; // 게임 오버 상태
    public Text scoreText; // 점수를 출력할 UI 텍스트
    public Text highScoreText; // 최고 점수를 출력할 UI 텍스트
    public GameObject gameoverUI; // 게임 오버시 활성화 할 UI 게임 오브젝트

    private int score = 0; // 현재 점수
    private int highScore = 0; // 최고 점수

    public float scoreIncreaseInterval = 1f; // 점수 증가 간격 (초)
    private float timeSinceLastIncrease = 0f; // 지난 점수 증가 이후 경과 시간

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }

        // 최고 점수 불러오기
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    void Start()
    {
        scoreText.text = "Score : " + score;
        highScoreText.text = "High Score : " + highScore;
    }

    void Update()
    {
        // 게임 오버 상태에서 게임을 재시작할 수 있게 하는 처리
        if (isGameover && Input.GetKeyDown(KeyCode.UpArrow))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (isGameover && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu"); // "Menu" 씬으로 전환
        }

        // 게임이 진행 중일 때 점수 자동 증가
        if (!isGameover)
        {
            timeSinceLastIncrease += Time.deltaTime;

            if (timeSinceLastIncrease >= scoreIncreaseInterval)
            {
                AddScore(1); // 1점 증가
                timeSinceLastIncrease = 0f;
            }
        }
    }

    // 점수를 증가시키는 메서드
    public void AddScore(int newScore)
    {
        if (!isGameover)
        {
            score += newScore;
            scoreText.text = "Score : " + score;

            // 최고 점수 갱신
            if (score > highScore)
            {
                highScore = score;
                highScoreText.text = "High Score : " + highScore;
            }
        }
    }

    // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
    public void OnPlayerDead()
    {
        isGameover = true;
        gameoverUI.SetActive(true);

        // 최고 점수 저장
        PlayerPrefs.SetInt("HighScore", highScore);
    }
}
