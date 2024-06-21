using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{



    [SerializeField] private Item_Dessert[] originalDessertItems;
    [SerializeField] private Item_Drink[] originalDrinkItems;
    [SerializeField] private Item_Goods[] originalGoodsItems;
    [SerializeField] private Item_Medicine[] originalMedicineItems;

    public Item_Dessert[] runtimeDessertItems;
    public Item_Drink[] runtimeDrinkItems;
    public Item_Goods[] runtimeGoodsItems;
    public Item_Medicine[] runtimeMedicineItems;

    [Header("기능용가구 가격")]
    private int[] furniturePrices;
    [SerializeField] private int CuffePort = 3000;
    [SerializeField] private int cushionPrice = 2500;
    [SerializeField] private int highCushionPrice = 3500;
    [SerializeField] private int perfectCushionPrice = 4500;
    [Header("장식용가구 가격")]
    private int[] decorationPrices;
    [SerializeField] private int clockPrice = 500;


    private void Awake()
    {
 
        furniturePrices = new int[4];
        furniturePrices[0] = CuffePort;
        furniturePrices[1] = cushionPrice;
        furniturePrices[2] = highCushionPrice;
        furniturePrices[3] = perfectCushionPrice;

        decorationPrices = new int[1];
        decorationPrices[0] = clockPrice;
    }

    private void InitializeItems()
    {
        runtimeDessertItems = new Item_Dessert[originalDessertItems.Length];
        runtimeDrinkItems = new Item_Drink[originalDrinkItems.Length];
        runtimeGoodsItems = new Item_Goods[originalGoodsItems.Length];
        runtimeMedicineItems = new Item_Medicine[originalMedicineItems.Length];

        for (int i = 0; i < originalDessertItems.Length; i++)
        {
            runtimeDessertItems[i] = Instantiate(originalDessertItems[i]);
        }

        for (int i = 0; i < originalDrinkItems.Length; i++)
        {
            runtimeDrinkItems[i] = Instantiate(originalDrinkItems[i]);
        }

        for (int i = 0; i < originalGoodsItems.Length; i++)
        {
            runtimeGoodsItems[i] = Instantiate(originalGoodsItems[i]);
        }

        for (int i = 0; i < originalMedicineItems.Length; i++)
        {
            runtimeMedicineItems[i] = Instantiate(originalMedicineItems[i]);
        }
    }

    public T GetItemByIndex<T>(int index) where T : ItemBase
    {
        if (typeof(T) == typeof(Item_Dessert))
        {
            return runtimeDessertItems[index] as T;
        }
        else if (typeof(T) == typeof(Item_Drink))
        {
            return runtimeDrinkItems[index] as T;
        }
        else if (typeof(T) == typeof(Item_Goods))
        {
            return runtimeGoodsItems[index] as T;
        }
        else if (typeof(T) == typeof(Item_Medicine))
        {
            return runtimeMedicineItems[index] as T;
        }

        return null;
    }

    public int GetFurniturePrice(int i)
    {

        return furniturePrices[i];
    }
    public int GetDecorationPrice(int i)
    {
        return decorationPrices[i];
    }

}
