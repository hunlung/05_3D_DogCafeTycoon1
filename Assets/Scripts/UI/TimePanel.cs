using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimePanel : MonoBehaviour
{
     TimeManager timeManager;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;

    //12���� �� �̸� ����ȭ
    private Dictionary<int, string> monthAbbreviations = new Dictionary<int, string>()
    {
        { 1, "Jan" },
        { 2, "Feb" },
        { 3, "Mar" },
        { 4, "Apr" },
        { 5, "May" },
        { 6, "Jun" },
        { 7, "Jul" },
        { 8, "Aug" },
        { 9, "Sep" },
        { 10, "Oct" },
        { 11, "Nov" },
        { 12, "Dec" }
    };

    private void Awake()
    {
        timeText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        dateText = transform.GetChild(2).GetComponent<TextMeshProUGUI>(); 
    }

    private void Start()
    {
        timeManager = GameManager.Instance.TimeManager;
        timeManager.OnTimeChanged += SetTime;
        timeManager.OnDayChanged += SetDay;
        //���۽� ����
        SetTime(timeManager.CurrentHour, timeManager.CurrentMinute);
        SetDay(timeManager.Month, timeManager.Day);
    }

    
    public void SetTime(int Hour ,int Minit) 
    {

        timeText.text = $"{Hour:D2} : {Minit:D2}";
    }

    public void SetDay(int month, int day)
    {
        //������ month�ְ� ��� ���
        if (monthAbbreviations.TryGetValue(month, out string monthAbbreviation))
        {
            dateText.text = $"{monthAbbreviation} {day}";
        }
        else
        {
            Debug.LogWarning($"Month�� 1~12���̰� �ƴմϴ�, month : {month}");
        }
    }
}
