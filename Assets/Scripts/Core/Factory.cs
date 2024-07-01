using UnityEngine;

public class Factory : Singleton<Factory>
{

    CorgiPool corgiPool;
    CurPool curPool;
    ShepherdPool shepherdPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        corgiPool = GetComponentInChildren<CorgiPool>();
        corgiPool?.Initialize();
        curPool = GetComponentInChildren<CurPool>();
        curPool?.Initialize();
        shepherdPool = GetComponentInChildren<ShepherdPool>();
        shepherdPool?.Initialize();

    }

    public DogBase GetCorgi()
    {
        DogBase dog = corgiPool?.GetObject();


        return dog;

    }
    public DogBase GetCur()
    {
        DogBase dog = curPool?.GetObject();
        return dog;
    }
    public DogBase GetShephered()
    {
        DogBase dog = shepherdPool?.GetObject();
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
