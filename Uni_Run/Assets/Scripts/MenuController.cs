using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    // Start 버튼 클릭 시 게임 씬(Main)으로 전환
    public void StartGame()
    {
        SceneManager.LoadScene("Main"); // "Main" 씬으로 전환
    }

    public void EnterHelp()
    {
        SceneManager.LoadScene("Help");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Exit 버튼 클릭 시 애플리케이션 종료
    public void ExitGame()
    {
        Application.Quit(); // 게임 종료
    }

}
