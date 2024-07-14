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
            DessertTexts[i].text = $"재고: {ItemManager.Instance.GetItemByIndex<Item_Dessert>(i).remaining}개";
            DrinkTexts[i].text = $"재고: {ItemManager.Instance.GetItemByIndex<Item_Drink>(i).remaining}개";
        }
        Others[0].text = $"재고: {ItemManager.Instance.GetItemByIndex<Item_Goods>(0).remaining}개";
        Others[1].text = $"재고: {ItemManager.Instance.GetItemByIndex<Item_Medicine>(0).remaining}개";

        for (int i = 0; profitTexts.Length > i; i++)
        {//기록이 있으면
            if (i < GameManager.Instance.Player.profits.Length)
            {
                profitTexts[i].text = $"{GameManager.Instance.Player.recorededTime[i]}시 수익: {GameManager.Instance.Player.profits[i]}";
            }//기록이 없으면
            else
            {
                profitTexts[i].text = "";
            }

        }




    }
}
