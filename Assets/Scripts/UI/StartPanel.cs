using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField]Button[] buttons;


    private void Start()
    {
        buttons[0].onClick.AddListener(GameStart);
        buttons[1].onClick.AddListener(ExplainGame);
        buttons[2].onClick.AddListener(GameQuit);
    }

    public void GameStart()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void ExplainGame()
    {

    }

    private void GameQuit()
    {
        Application.Quit();
    }

}
