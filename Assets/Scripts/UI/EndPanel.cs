using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndPanel : MonoBehaviour
{

    const int MissionMoney = 1000000;
    [SerializeField] TextMeshProUGUI[] Texts;
    private void Start()
    {
        if (GameManager.Instance.Player.Money >= MissionMoney)
        {
            Texts[0].text = "�̼� ����!";
        }
        else
        {
            Texts[0].text = "�̼� ����";
        }

        Texts[1].text = $"���� ��: {GameManager.Instance.Player.Money}";
    }

    


}
