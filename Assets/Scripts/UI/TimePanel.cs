using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimePanel : MonoBehaviour
{
     TimeManager timeManager;
     TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        timeManager = GameManager.Instance.TimeManager;
        timeManager.OnTimeChanged += SetTime;
    }

    
    public void SetTime(int Hour ,int Minit) 
    {

        text.text = $"{Hour:D2} : {Minit:D2}";
    }



}
