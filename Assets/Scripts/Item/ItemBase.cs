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
    //아이템 분류
    public ItemType ItemType;
    
    //아이템 정보
    public Sprite Icon;
    public string ItemName;
    public int remaining;
    public int purchasePrice;
    public int sellPrice;
    public int satisfaction;

    //업그레이드
    public int upgradePrice;
    public float upgradeEfficiency;
    public bool upgradeRequirement;
    public string requirementinfo;
    //설명
    public string ItemInfo;

    //업그레이드
    public void UpGrade()
    {
        this.sellPrice += (int)(sellPrice * upgradeEfficiency);
        this.satisfaction += (int)(satisfaction * upgradeEfficiency * 2);
        this.upgradePrice += (int)(upgradePrice * upgradeEfficiency);
    }



}
