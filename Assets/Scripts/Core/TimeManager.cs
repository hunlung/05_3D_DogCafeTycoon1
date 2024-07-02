using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Material nightSkybox; // �� �ϴ��� ǥ���� ��ī�̹ڽ� ��Ƽ����
    [SerializeField] private Material daySkybox; //  �ϴ��� ǥ���� ��ī�̹ڽ� ��Ƽ����

    private Button stopSpeedButton; // ���� ��ư
    private Button normalSpeedButton; // �Ϲ� �ӵ� ��ư
    private Button doubleSpeedButton; // 2��� ��ư
    private Button tripleSpeedButton; // 3��� ��ư
    private Image CurrentSpeedImage; //�ð� �� ��� �̹���

    [SerializeField] private Sprite stopSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite doubleSprite;
    [SerializeField] private Sprite trippleSprite;

    private int currentTimeSpeed = 1;
    private int previousTimeSpeed = 1;

    public Action<int, int> OnTimeChanged; // �ð��� ����� �� �߻��ϴ� �̺�Ʈ
    public Action<int, int> OnDayChanged;

    private int currentHour = 0; // ���� �ð� (��)
    
    /// <summary>
    /// TODO:: �ð��� ������ �ٷ� ���� �Ѱ����� ��, ���� ���� �� �ڵ���� �ʿ�
    /// </summary>
    public int CurrentHour
    {
        get { return currentHour; }
        set
        {
            if (currentHour != value)
            {
                currentHour = value;
                OnTimeChanged?.Invoke(CurrentHour, CurrentMinute); // �ð� ���� �̺�Ʈ �߻�
                if (currentHour >= nightStartHour && !isNight)
                {
                    RenderSettings.skybox = nightSkybox; // �� �ϴ� ��ī�̹ڽ��� ����
                    isNight = true;
                }
                if (currentHour >= endHour)
                {
                    //�ٷ� �������� ������ �մ��� ������ ���� �������� �ٲ� �ʿ䰡 ���� + ����.
                    StopAllCoroutines();
                    //���� ���� �غ� ���� + ������ ����
                    GameManager.Instance.StoreLastOrder();

                }
            }
        }
    }

    private int currentMinute; // ���� �ð� (��)
    public int CurrentMinute
    {
        get { return currentMinute; }
        set
        {
            if (currentMinute != value)
            {
                currentMinute = value;
                if (currentMinute >= 60)
                {
                    currentMinute = 0;
                    CurrentHour += 1; // 60���� �Ǹ� �ð��� 1�ð� ����
                }
                OnTimeChanged?.Invoke(CurrentHour, CurrentMinute); // �ð� ���� �̺�Ʈ �߻�
            }
        }
    }

    //4����ŸƮ
    private int month = 4;
    /// <summary>
    /// 12������ ���� ���� ��
    /// </summary>
    public int Month
    {
        get { return month; }
        set
        {
            if (month != value)
                month = value;
            month %= 13;
            month = (int)Mathf.Max(1f, month);
            OnDayChanged?.Invoke(Month, Day);
        }
    }
    //5�� ��ŸƮ
    private int day = 5;
    //�޷��� 31��,30��,28�� ����
    public int Day
    {
        get { return day; }
        set
        {
            if (day != value)
            {
                day = value;
                if(Month == 2)
                {
                    if(Day >= 29)
                    {
                        Month += 1;
                        day = 0;
                    }
                }
                else if(Month == 1 || Month == 3 || Month == 5 || Month == 7 || Month == 8 || Month == 10 || Month == 12)
                {
                    if(Day >= 32)
                    {
                        Month += 1;
                        day = 0;
                    }
                }
                else
                {
                    if(day >= 31)
                    {
                        Month += 1;
                        day = 0;
                    }
                }
                OnDayChanged?.Invoke(Month, Day);
            }
        }
    }

    private readonly int startHour = 12; // ���� �ð� (��)
    private readonly int endHour = 20; // ���� �ð� (��)
    private int nightStartHour; // �� ���� �ð� (��)

    private bool isNight = false; // ���� ������ ����


    private void Awake()
    {
        nightStartHour = endHour - 2; // �� ���� �ð� ���� (���� �ð� 2�ð� ��)
    }

    private void Start()
    {

        GameObject gameSpeedButton = GameObject.FindGameObjectWithTag("GameSpeedButton");
        // �� ��ư ������Ʈ ��������
        stopSpeedButton = gameSpeedButton.transform.GetChild(0).GetComponent<Button>();
        normalSpeedButton = gameSpeedButton.transform.GetChild(1).GetComponent<Button>();
        doubleSpeedButton = gameSpeedButton.transform.GetChild(2).GetComponent<Button>();
        tripleSpeedButton = gameSpeedButton.transform.GetChild(3).GetComponent<Button>();

        //���� ���Ӽӵ� �̹��� ��������
        CurrentSpeedImage = GameObject.FindGameObjectWithTag("CurrentSpeedImage").GetComponent<Image>();

        // ��ư Ŭ�� �̺�Ʈ �ڵ鷯 ���
        stopSpeedButton.onClick.AddListener(SetStopTimeSpeed);
        normalSpeedButton.onClick.AddListener(SetNormalTimeSpeed);
        doubleSpeedButton.onClick.AddListener(SetDoubleTimeSpeed);
        tripleSpeedButton.onClick.AddListener(SetTripleTimeSpeed);
    }
    public void NextDay()
    {
        Day += 1;
        CurrentSpeedImage.gameObject.SetActive(false);
        RenderSettings.skybox = daySkybox;
        isNight = false;
    }

    // �ð� �ֱ� ����
    public void StartTimeCycle()
    {
        StopAllCoroutines();
        StartCoroutine(TimeCycleCoroutine());
    }

    // �ð� �ֱ� �ڷ�ƾ
    private IEnumerator TimeCycleCoroutine()
    {
        CurrentHour = startHour; // ���� �ð����� ����
        CurrentMinute = 0; // �� �ʱ�ȭ
        CurrentSpeedImage.gameObject.SetActive(true); // ���� ��� ������ Ű��
        SetNormalTimeSpeed(); // 1������� �����ϱ�
        while (CurrentHour <= endHour)
        {
            CurrentMinute += 2; // �ʴ� 2�о� ����
            yield return new WaitForSeconds(1f); // 1�� ���
        }
    }

    private void SetStopTimeSpeed()
    {
        if (currentTimeSpeed != 0)
        {
            previousTimeSpeed = (int)Time.timeScale;
            Time.timeScale = 0f;
            currentTimeSpeed = 0;
            SetTimeSpeedImage(currentTimeSpeed);
        }
        else
        {
            currentTimeSpeed = previousTimeSpeed;
            Time.timeScale = previousTimeSpeed;
            SetTimeSpeedImage(currentTimeSpeed);
        }
    }

    // �Ϲ� �ӵ��� ����
    private void SetNormalTimeSpeed()
    {
        Time.timeScale = 1f;
        currentTimeSpeed = 1;
        SetTimeSpeedImage(currentTimeSpeed);

    }

    // 2������� ����
    private void SetDoubleTimeSpeed()
    {
        Time.timeScale = 2f;
        currentTimeSpeed = 2;
        SetTimeSpeedImage(currentTimeSpeed);

    }

    // 3������� ����
    private void SetTripleTimeSpeed()
    {
        Time.timeScale = 3f;
        currentTimeSpeed = 3;
        SetTimeSpeedImage(currentTimeSpeed);

    }

    public void TimeSpeedChange()
    {
        switch (currentTimeSpeed)
        {
            case 0:
                SetStopTimeSpeed(); break;
            case 1:
                SetDoubleTimeSpeed(); break;
            case 2:
                SetTripleTimeSpeed(); break;
            case 3:
                SetNormalTimeSpeed(); break;
        }
    }

    public void StopTimeToggle()
    {

        SetStopTimeSpeed();
    }

    private void SetTimeSpeedImage(int timeSpeed)
    {
        if (CurrentSpeedImage.gameObject.activeSelf)
        {

            switch (timeSpeed)
            {
                case 0:
                    CurrentSpeedImage.sprite = stopSprite; break;
                case 1:
                    CurrentSpeedImage.sprite = normalSprite; break;
                case 2:
                    CurrentSpeedImage.sprite = doubleSprite; break;
                case 3:
                    CurrentSpeedImage.sprite = trippleSprite; break;
            }
        }
    }

}