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
            Texts[0].text = "미션 성공!";
        }
        else
        {
            Texts[0].text = "미션 실패";
        }

        Texts[1].text = $"모은 돈: {GameManager.Instance.Player.Money}";
    }

    


}
