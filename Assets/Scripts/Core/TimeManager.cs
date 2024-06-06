using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Material nightSkybox; // 밤 하늘을 표현할 스카이박스 머티리얼
    private Button stopSpeedButton; // 정지 버튼
    private Button normalSpeedButton; // 일반 속도 버튼
    private Button doubleSpeedButton; // 2배속 버튼
    private Button tripleSpeedButton; // 3배속 버튼
    private Image CurrentSpeedImage;

    [SerializeField] private Sprite stopSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite doubleSprite;
    [SerializeField] private Sprite trippleSprite;

    private int currentTimeSpeed = 1;
    private int previousTimeSpeed = 1;

    public Action<int, int> OnTimeChanged; // 시간이 변경될 때 발생하는 이벤트

    private int currentHour = 0; // 현재 시간 (시)
    public int CurrentHour
    {
        get { return currentHour; }
        set
        {
            if (currentHour != value)
            {
                currentHour = value;
                OnTimeChanged?.Invoke(CurrentHour, CurrentMinute); // 시간 변경 이벤트 발생
                if (currentHour >= nightStartHour && !isNight)
                {
                    RenderSettings.skybox = nightSkybox; // 밤 하늘 스카이박스로 변경
                    isNight = true;
                }
                if (currentHour >= endHour)
                {
                    // 영업 시간이 끝났음을 알림
                }
            }
        }
    }

    private int currentMinute; // 현재 시간 (분)
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
                    CurrentHour += 1; // 60분이 되면 시간을 1시간 증가
                }
                OnTimeChanged?.Invoke(CurrentHour, CurrentMinute); // 시간 변경 이벤트 발생
            }
        }
    }

    private readonly int startHour = 12; // 시작 시간 (시)
    private readonly int endHour = 20; // 종료 시간 (시)
    private int nightStartHour; // 밤 시작 시간 (시)

    private bool isNight = false; // 현재 밤인지 여부


    private void Awake()
    {
        nightStartHour = endHour - 2; // 밤 시작 시간 설정 (종료 시간 2시간 전)
    }

    private void Start()
    {

        GameObject gameSpeedButton = GameObject.FindGameObjectWithTag("GameSpeedButton");
        // 각 버튼 컴포넌트 가져오기
        stopSpeedButton = gameSpeedButton.transform.GetChild(0).GetComponent<Button>();
        normalSpeedButton = gameSpeedButton.transform.GetChild(1).GetComponent<Button>();
        doubleSpeedButton = gameSpeedButton.transform.GetChild(2).GetComponent<Button>();
        tripleSpeedButton = gameSpeedButton.transform.GetChild(3).GetComponent<Button>();

        //현재 게임속도 이미지 가져오기
        CurrentSpeedImage = GameObject.FindGameObjectWithTag("CurrentSpeedImage").GetComponent<Image>();

        // 버튼 클릭 이벤트 핸들러 등록
        stopSpeedButton.onClick.AddListener(SetStopTimeSpeed);
        normalSpeedButton.onClick.AddListener(SetNormalTimeSpeed);
        doubleSpeedButton.onClick.AddListener(SetDoubleTimeSpeed);
        tripleSpeedButton.onClick.AddListener(SetTripleTimeSpeed);
    }

    // 시간 주기 시작
    public void StartTimeCycle()
    {
        StopAllCoroutines();
        StartCoroutine(TimeCycleCoroutine());
    }

    // 시간 주기 코루틴
    private IEnumerator TimeCycleCoroutine()
    {
        CurrentHour = startHour; // 시작 시간으로 설정
        CurrentMinute = 0; // 분 초기화
        while (CurrentHour <= endHour)
        {
            CurrentMinute += 1; // 1분씩 증가
            yield return new WaitForSeconds(1f); // 1초 대기
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

    // 일반 속도로 설정
    private void SetNormalTimeSpeed()
    {
        Time.timeScale = 1f;
        currentTimeSpeed = 1;
        SetTimeSpeedImage(currentTimeSpeed);

    }

    // 2배속으로 설정
    private void SetDoubleTimeSpeed()
    {
        Time.timeScale = 2f;
        currentTimeSpeed = 2;
        SetTimeSpeedImage(currentTimeSpeed);

    }

    // 3배속으로 설정
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