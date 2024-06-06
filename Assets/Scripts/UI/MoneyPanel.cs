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
        //TODO:: 천천히 상승하는 코드 필요
        text.text = amount.ToString();
    }

}
