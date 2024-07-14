using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyPanel : MonoBehaviour
{
    Player player;

    TextMeshProUGUI text;

    [SerializeField] private GameObject moneyChangeTextPrefab; // 돈 변화 텍스트 프리팹
    [SerializeField] private float fadeOutDuration = 1f; // 페이드 아웃 지속 시간
    [SerializeField] private float moveUpDistance = 100f; // 위로 이동할 거리

    private void Start()
    {
        SetMoneyPanel();
    }

    public void SetMoneyPanel()
    {
        player = GameManager.Instance.Player;
        player.OnMoneyChange += ShowMoneyChange;
        text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        SetMoneyText(player.Money);
    }

    private void ShowMoneyChange(int currentMoney, int diff)
    {
        SetMoneyText(currentMoney); // 현재 돈 표시

        if (diff != 0)
        {
            string changeText = (diff > 0 ? "+" : "") + diff.ToString(); // 변화량 텍스트 생성
            Color textColor = diff > 0 ? Color.green : Color.red; // 변화량에 따른 텍스트 색상 설정

            GameObject changeTextObj = Instantiate(moneyChangeTextPrefab, transform); // 텍스트 프리팹 생성
            TextMeshProUGUI changeTextComponent = changeTextObj.GetComponent<TextMeshProUGUI>(); // TextMeshProUGUI 컴포넌트 가져오기
            changeTextComponent.text = changeText; // 변화량 텍스트 설정
            changeTextComponent.color = textColor; // 텍스트 색상 설정

            // 돈 변화 텍스트를 위로 이동시키고 페이드 아웃하는 코루틴 실행
            StartCoroutine(MoveUpAndFadeOut(changeTextObj));
        }
    }
    private IEnumerator MoveUpAndFadeOut(GameObject textObj)
    {
        TextMeshProUGUI textComponent = textObj.GetComponent<TextMeshProUGUI>();
        Color originalColor = textComponent.color;
        Vector3 originalPosition = textObj.transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / fadeOutDuration;

            // 텍스트를 위로 이동
            textObj.transform.position = originalPosition + Vector3.up * moveUpDistance * t;

            // 텍스트를 페이드 아웃
            textComponent.color = Color.Lerp(originalColor, Color.clear, t);

            yield return null;
        }

        Destroy(textObj); // 텍스트 오브젝트 파괴
    }

    private void SetMoneyText(int currentMoney)
    {
        text.text = currentMoney.ToString();
    }

}
