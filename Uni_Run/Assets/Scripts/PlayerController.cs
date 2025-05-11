using UnityEngine;
using UnityEngine.UI; // UI 하트 이미지 사용을 위해 추가
using System.Collections;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour {
   public AudioClip deathClip; // 사망시 재생할 오디오 클립
   public float jumpVelocity = 5f;   // 바로 적용할 점프 속도
   public float fallVelocity = -20f;  // 빠른 하강 속도

   public int maxLives = 3; // 최대 목숨 수 (하트 개수)
   private int currentLives; // 현재 남은 목숨 수

   public float invincibleDuration = 1.5f; // 무적 시간 (초)
   private bool isInvincible = false; // 현재 무적인지 여부
   private float invincibleTimer = 0f; // 남은 무적 시간

   private SpriteRenderer spriteRenderer;

   public Image[] heartImages; // UI 상의 하트 이미지 배열 (최대 3개)

   private int jumpCount = 0; // 누적 점프 횟수
   private bool isGrounded = false; // 바닥에 닿았는지 나타냄
   private bool isDead = false; // 사망 상태

   private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
   private Animator animator; // 사용할 애니메이터 컴포넌트
   private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트

   private void Start() { // 초기화
       playerRigidbody = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
       playerAudio = GetComponent<AudioSource>();
       spriteRenderer = GetComponent<SpriteRenderer>();

       currentLives = maxLives; // 시작 시 최대 목숨 설정
       UpdateHeartsUI(); // 하트 UI 초기 상태로 반영
    }

   private void Update() { // 사용자 입력을 감지하고 점프하는 처리
       if (isDead)
       {
           return;
       }
       if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCount < 2)
        {
           jumpCount++;
           playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpVelocity);
           playerAudio.Play(); // 점프 사운드 재생
       }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, -100f);
        }
        else if (isGrounded && playerRigidbody.velocity.y < 0)
        {
            // 땅에 닿으면 velocity를 0으로 만드는 코드 (부자연스러워 보이지 않게 처리)
            playerRigidbody.velocity = Vector2.zero;
        }

        if (isInvincible) // 깜박거림
       {
           invincibleTimer -= Time.deltaTime;
           if (invincibleTimer <= 0f)
           {
               isInvincible = false; // 무적 해제
           }
       }

       // 애니메이터에 현재 바닥 상태 전달
       animator.SetBool("Grounded", isGrounded);
    }

   private void Die() { // 사망 처리
        animator.SetTrigger("Die"); // 죽는 애니메이션 재생
        playerAudio.clip = deathClip; // 사망 사운드 클립 지정
        playerAudio.Play(); // 사망 사운드 재생
        playerRigidbody.velocity = Vector2.zero; // 움직임 정지
        isDead = true; // 사망 상태 설정
        GameManager.instance.OnPlayerDead(); // 게임 매니저에 사망 통보
   }

   private void TakeDamage()
    { // 데미지를 받아 하트를 하나 줄인다
        if (isInvincible) return; // 무적 중이면 무시
        currentLives--; // 남은 하트 수 감소
        UpdateHeartsUI(); // UI에 반영

        if (currentLives <= 0)
        {
            Die();
        }
        else
        {
            isInvincible = true;
            invincibleTimer = invincibleDuration;
            StartCoroutine(FlickerEffect()); // 깜빡이기 시작!
        }
    }

   private IEnumerator FlickerEffect()
    {
        float elapsed = 0f;
        float flickerSpeed = 0.1f;

        while (elapsed < invincibleDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flickerSpeed);
            elapsed += flickerSpeed;
        }

        spriteRenderer.enabled = true; // 마지막에 다시 켜줌
    }

    private void UpdateHeartsUI()
    { // 현재 남은 하트 수에 따라 UI 갱신
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = i < currentLives; // 남은 개수까지만 표시
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
        if (other.tag == "Dead" && !isDead)
        {
            Die();
        }
        else if (other.CompareTag("Obstacle") && !isDead)
        {
            TakeDamage(); // 일반 장애물은 하트 1개 감소
        }
    }

   private void OnCollisionEnter2D(Collision2D collision) { // 바닥에 닿았음을 감지하는 처리
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
        }
   }

   private void OnCollisionExit2D(Collision2D collision) { // 바닥에서 벗어났음을 감지하는 처리
        isGrounded = false;
   }
}