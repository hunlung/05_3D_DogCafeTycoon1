using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RemainingPanel : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI[] DessertTexts;
    [SerializeField] TextMeshProUGUI[] DrinkTexts;
    [SerializeField] TextMeshProUGUI[] Others;

    [SerializeField] TextMeshProUGUI[] profitTexts;


    private void Start()
    {
        GameManager.Instance.onDayEnd += SetOff;
        GameManager.Instance.Player.onRecorded += UpdatePanel;
        GameManager.Instance.Player.onSell += UpdatePanel;
    }

    private void UpdatePanel(int a)
    {
        OnEnable();
    }
    private void SetOff()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        for (int i = 0; i < DessertTexts.Length; i++)
        {
            DessertTexts[i].text = $"���: {ItemManager.Instance.GetItemByIndex<Item_Dessert>(i).remaining}��";
            DrinkTexts[i].text = $"���: {ItemManager.Instance.GetItemByIndex<Item_Drink>(i).remaining}��";
        }
        Others[0].text = $"���: {ItemManager.Instance.GetItemByIndex<Item_Goods>(0).remaining}��";
        Others[1].text = $"���: {ItemManager.Instance.GetItemByIndex<Item_Medicine>(0).remaining}��";

        for (int i = 0; profitTexts.Length > i; i++)
        {//����� ������
            if (i < GameManager.Instance.Player.profits.Length)
            {
                profitTexts[i].text = $"{GameManager.Instance.Player.recorededTime[i]}�� ����: {GameManager.Instance.Player.profits[i]}";
            }//����� ������
            else
            {
                profitTexts[i].text = "";
            }

        }




    }
}
