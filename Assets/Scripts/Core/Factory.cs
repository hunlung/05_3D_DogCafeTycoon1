using UnityEngine;

public class Factory : Singleton<Factory>
{

    CorgiPool corgiPool;


    protected override void OnInitialize()
    {
        base.OnInitialize();

        corgiPool = GetComponentInChildren<CorgiPool>();
        corgiPool?.Initialize();

    }

    public DogBase GetCorgi()
    {
        DogBase dog = corgiPool?.GetObject();


        return dog;

    }


    //public GunItem GetAssaultRifleItem(Vector3 position)
    //{
    //    GunItem item = assaultRiflePool?.GetObject();
    //    item.transform.position = position;
    //    return item;
    //}



    //public ItemBase GetDropItem(Enemy.ItemTable table, Vector3 position)
    //{
    //    ItemBase item = null;
    //    switch (table)
    //    {
    //        case Enemy.ItemTable.AssaultRifle:
    //            item = GetAssaultRifleItem(position);
    //            break;
    //        case Enemy.ItemTable.Shotgun:
    //            item = GetShotgunItem(position);
    //            break;
    //        case Enemy.ItemTable.Heal:
    //        default:
    //            item = GetHealPackItem(position);
    //            break;
    //    }

    //    return item;
    //}



}
