using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESCPanel : MonoBehaviour
{
    [SerializeField] Button[] buttons;

    [SerializeField] GameObject[] Panels;

   private bool isActived = false;


    private void Start()
    {
        buttons[0].onClick.AddListener(Explain);
        buttons[1].onClick.AddListener(CheckExit);
        buttons[2].onClick.AddListener(CloseExit);
        buttons[3].onClick.AddListener(EXIT);
        buttons[4].onClick.AddListener(ClosePanel);
    }

    private void OnEnable()
    {
        if(Time.timeScale > 0f)
        {
        GameManager.Instance.TimeManager.StopTimeToggle();
        }
        isActived = true;
        
    }
    private void OnDisable()
    {
        if(Time.timeScale <= 0f)
        {
        GameManager.Instance.TimeManager.StopTimeToggle();
        }
        isActived = false;
    }


    private void Explain()
    {
        Panels[0].gameObject.SetActive(true);
    }

    private void CheckExit()
    {
        Panels[1].SetActive(true);
    }
    private void CloseExit()
    {
        Panels[1].SetActive(false);
    }

    private void EXIT()
    {
        Application.Quit();
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public bool CheckPanel()
    {
        return isActived;
    }


}
