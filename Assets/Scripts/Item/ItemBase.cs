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
    public ItemType ItemType;
    
    //������ ����
    public Sprite Icon;
    public string ItemName;
    public int remaining;
    public int purchasePrice;
    public int sellPrice;
    public int satisfaction;

    //���׷��̵�
    public int upgradePrice;
    public float upgradeEfficiency;
    public bool upgradeRequirement;
    public string requirementinfo;
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
