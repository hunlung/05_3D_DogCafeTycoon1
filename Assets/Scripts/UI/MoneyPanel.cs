using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyPanel : MonoBehaviour
{
    Player player;

    TextMeshProUGUI text;

    [SerializeField] private GameObject moneyChangeTextPrefab; // �� ��ȭ �ؽ�Ʈ ������
    [SerializeField] private float fadeOutDuration = 1f; // ���̵� �ƿ� ���� �ð�
    [SerializeField] private float moveUpDistance = 100f; // ���� �̵��� �Ÿ�

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
        SetMoneyText(currentMoney); // ���� �� ǥ��

        if (diff != 0)
        {
            string changeText = (diff > 0 ? "+" : "") + diff.ToString(); // ��ȭ�� �ؽ�Ʈ ����
            Color textColor = diff > 0 ? Color.green : Color.red; // ��ȭ���� ���� �ؽ�Ʈ ���� ����

            GameObject changeTextObj = Instantiate(moneyChangeTextPrefab, transform); // �ؽ�Ʈ ������ ����
            TextMeshProUGUI changeTextComponent = changeTextObj.GetComponent<TextMeshProUGUI>(); // TextMeshProUGUI ������Ʈ ��������
            changeTextComponent.text = changeText; // ��ȭ�� �ؽ�Ʈ ����
            changeTextComponent.color = textColor; // �ؽ�Ʈ ���� ����

            // �� ��ȭ �ؽ�Ʈ�� ���� �̵���Ű�� ���̵� �ƿ��ϴ� �ڷ�ƾ ����
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

            // �ؽ�Ʈ�� ���� �̵�
            textObj.transform.position = originalPosition + Vector3.up * moveUpDistance * t;

            // �ؽ�Ʈ�� ���̵� �ƿ�
            textComponent.color = Color.Lerp(originalColor, Color.clear, t);

            yield return null;
        }

        Destroy(textObj); // �ؽ�Ʈ ������Ʈ �ı�
    }

    private void SetMoneyText(int currentMoney)
    {
        text.text = currentMoney.ToString();
    }

}
