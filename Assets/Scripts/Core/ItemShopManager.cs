using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemShopManager : MonoBehaviour
{
    PlayerInput itemShopInput;
    [SerializeField] Button[] itemShopButtons;
    [SerializeField] Button[] dessertButtons;
    [SerializeField] Button[] drinkButtons;
    [SerializeField] Button[] goodsButtons;
    [SerializeField] Button[] medicineButtons;
    [SerializeField] Button[] returnButtons;
    [SerializeField] Button[] itemBuySellButtons;
    [SerializeField] Button requirementButton;

    GameObject ItemShopPanel;
    GameObject dessertPanel;
    GameObject drinkPanel;
    GameObject goodsPanel;
    GameObject medicinePanel;
    GameObject ItemBuySellPanel;
    GameObject RequirementPanel;


   [SerializeField] Item_Dessert[] item_Dessert;
    [SerializeField] Item_Drink[] item_Drink;
    [SerializeField] Item_Goods[] item_Goods;
    [SerializeField] Item_Medicine[] item_Medicine;

    public Image itemImage;
    public TextMeshProUGUI itemInfoText;

    private void Awake()
    {
        itemShopInput = new PlayerInput();
    }

    private void Start()
    {
        GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
        ItemShopPanel = Canvas.transform.GetChild(0).gameObject;
        dessertPanel = Canvas.transform.GetChild(1).gameObject;
        drinkPanel = Canvas.transform.GetChild(2).gameObject;
        goodsPanel = Canvas.transform.GetChild(3).gameObject;
        medicinePanel = Canvas.transform.GetChild(4).gameObject;
        ItemBuySellPanel = Canvas.transform.GetChild(5).gameObject;
        RequirementPanel = Canvas.transform.GetChild(6).gameObject;

        Transform itemBuySellTransform;
        itemBuySellTransform = ItemBuySellPanel.transform.GetChild(1).transform;
        itemImage = itemBuySellTransform.GetChild(1).GetComponent<Image>();
        itemInfoText = itemBuySellTransform.GetChild(2).GetComponent<TextMeshProUGUI>();

        itemShopButtons[0].onClick.AddListener(GotoDessertPanel);
        itemShopButtons[1].onClick.AddListener(GotoDrinkPanel);
        itemShopButtons[2].onClick.AddListener(GotoGoodsPanel);
        itemShopButtons[3].onClick.AddListener(GotoMedicinePanel);
        itemShopButtons[4].onClick.AddListener(GotoNextDay);

        foreach (Button dessertButton in dessertButtons)
        {
            dessertButton.onClick.AddListener(() => ShowItemOnBuySellPanel(dessertButton));
        }

        foreach (Button drinkButton in drinkButtons)
        {
            drinkButton.onClick.AddListener(() => ShowItemOnBuySellPanel(drinkButton));
        }

        foreach (Button goodsButton in goodsButtons)
        {
            goodsButton.onClick.AddListener(() => ShowItemOnBuySellPanel(goodsButton));
        }

        foreach (Button medicineButton in medicineButtons)
        {
            medicineButton.onClick.AddListener(() => ShowItemOnBuySellPanel(medicineButton));
        }



        foreach (Button returnButton in returnButtons)
        {
            returnButton.onClick.AddListener(() => GotoItemShop(returnButton));
        }

        itemBuySellButtons[1].onClick.AddListener(CloseItemBuySellPanel);
        


    }

    private void OnEnable()
    {
        itemShopInput.Enable();
        itemShopInput.ItemShop._1.performed += GotoDessertPanel;
        itemShopInput.ItemShop._2.performed += GotoDrinkPanel;
        itemShopInput.ItemShop._3.performed += GotoGoodsPanel;
        itemShopInput.ItemShop._4.performed += GotoMedicinePanel;

    }




    private void OnDisable()
    {
        itemShopInput.ItemShop._1.performed -= GotoDessertPanel;
        itemShopInput.Disable();
    }


    public void PrepareStore()
    {
        ItemShopPanel.SetActive(true);
    }



    private void GotoDessertPanel(InputAction.CallbackContext context)
    {
        GotoDessertPanel();
    }
    private void GotoDessertPanel()
    {
        ItemShopPanel.SetActive(false);
        dessertPanel.SetActive(true);
    }
    private void GotoDrinkPanel(InputAction.CallbackContext context)
    {
        GotoDrinkPanel();
    }
    private void GotoDrinkPanel()
    {
        ItemShopPanel.SetActive(false);
        drinkPanel.SetActive(true);
    }
    private void GotoGoodsPanel(InputAction.CallbackContext context)
    {
        GotoGoodsPanel();    
    }
    private void GotoGoodsPanel()
    {
        ItemShopPanel.SetActive(false);
        goodsPanel.SetActive(true);
    }
    private void GotoMedicinePanel(InputAction.CallbackContext context)
    {
        GotoMedicinePanel();
    }
    private void GotoMedicinePanel()
    {
        ItemShopPanel.SetActive(false);
        medicinePanel.SetActive(true);
    }
    private void GotoNextDay()
    {
        ItemShopPanel.SetActive(false);
        GameManager.Instance.DayStart();
    }

    private void GotoItemShop(Button button)
    {
        button.transform.parent.gameObject.SetActive(false);
        ItemShopPanel.SetActive(true);
    }

    private void ShowItemOnBuySellPanel(Button itemButton)
    {
        if (itemButton != null)
        {
            if (dessertButtons.Contains(itemButton))
            {
                int index = System.Array.IndexOf(dessertButtons, itemButton);
                if (index >= 0 && index < item_Dessert.Length)
                {
                    Item_Dessert selectedItem = item_Dessert[index];
                    UpdateBuySellPanel(selectedItem);
                }
            }
            else if (drinkButtons.Contains(itemButton))
            {
                int index = System.Array.IndexOf(drinkButtons, itemButton);
                if (index >= 0 && index < item_Drink.Length)
                {
                    Item_Drink selectedItem = item_Drink[index];
                    UpdateBuySellPanel(selectedItem);
                }
            }
            else if (goodsButtons.Contains(itemButton))
            {
                int index = System.Array.IndexOf(goodsButtons, itemButton);
                if (index >= 0 && index < item_Goods.Length)
                {
                    Item_Goods selectedItem = item_Goods[index];
                    UpdateBuySellPanel(selectedItem);
                }
            }
            else if (medicineButtons.Contains(itemButton))
            {
                int index = System.Array.IndexOf(medicineButtons, itemButton);
                if (index >= 0 && index < item_Medicine.Length)
                {
                    Item_Medicine selectedItem = item_Medicine[index];
                    UpdateBuySellPanel(selectedItem);
                }
            }
        }
    }
    private void UpdateBuySellPanel(ItemBase item)
    {
        ItemBuySellPanel.SetActive(true);

        // Update item image
        itemImage.sprite = item.Icon;

        // Update item info text
        itemInfoText.text = item.ItemInfo;

    }

    private void CloseItemBuySellPanel()
    {
        ItemBuySellPanel.SetActive(false);
    }




}
