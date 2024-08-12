using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainPanel : MonoBehaviour
{

    [SerializeField] Button[] buttons;
   [SerializeField] GameObject[] pages;

    int number;

    private void Awake()
    {
        pages = new GameObject[3];
        number = 0;
        for (int i = 0; i < 3; i++) 
        {
            pages[i] = gameObject.transform.GetChild(i + 2).gameObject;
        }

        buttons[0].onClick.AddListener(Close);
        buttons[1].onClick.AddListener(Next);
        
    }

    private void Start()
    {
        
        gameObject.SetActive(false); 
    }

    private void OnEnable()
    {
        for(int i =0; i < 3; i++)
        {
            pages[i].gameObject.SetActive(false);
        }
        pages[0].gameObject.SetActive(true);
        number = 0;
    }


    private void Close()
    {
        gameObject.SetActive(false);
    }
    private void Next()
    {
        switch (number)
        {
            case 0:
                pages[0].gameObject.SetActive(false);
                pages[1].gameObject.SetActive(true);
                break;
                case 1:
                pages[1].gameObject.SetActive(false);
                pages[2].gameObject.SetActive(true); break;
            case 2:
                Close();
                break;
        }
        number++;

    }


}
