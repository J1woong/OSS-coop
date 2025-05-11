using UnityEngine;

// 게임 오브젝트를 계속 왼쪽으로 움직이는 스크립트
public class ScrollingObject : MonoBehaviour {
    public float speed = 10f; // 이동 속도

    private void Update() { 
        // GameManager.instance가 null인지 확인
        if (GameManager.instance == null)
        {
            
            return; // GameManager가 없으면 더 이상 실행되지 않도록 return
        }

        // 게임 오버 상태가 아니면 계속 왼쪽으로 스크롤
        if (!GameManager.instance.isGameover)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }
}