using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyPanel : MonoBehaviour
{
    Player player;

    TextMeshProUGUI text;

    private void Start()
    {
        player = GameManager.Instance.Player;
        player.OnMoneyChange += SetMoneyText;
        text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void SetMoneyText(int amount)
    {
        //TODO:: õõ�� ����ϴ� �ڵ� �ʿ�
        text.text = amount.ToString();
    }

}
