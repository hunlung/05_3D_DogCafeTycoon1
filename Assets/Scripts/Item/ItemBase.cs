using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType : byte
{
    medicine = 0,
    drink,
    dessert,
    goods
}


[CreateAssetMenu(fileName = "New Food", menuName = "ScriptableObject/ItemData", order = 1)]
public class ItemBase : ScriptableObject
{
    //������ �з�
    public int itemCode;
    public ItemType ItemType;
    
    //������ ����
    [SerializeField] protected Sprite Icon;
    public string ItemName;
    public int purchasePrice;
    public int sellPrice;
    public int quantity;
    public int satisfaction;

    //���׷��̵�
    public int upgradePrice;
    public float upgradeEfficiency;
    //����
    public string ItemInfo;

    //���׷��̵�
    public void UpGrade()
    {
        this.sellPrice += (int)(sellPrice * upgradeEfficiency);
        this.satisfaction += (int)(satisfaction * upgradeEfficiency * 2);
        this.upgradePrice += (int)(upgradePrice * upgradeEfficiency);
    }



}