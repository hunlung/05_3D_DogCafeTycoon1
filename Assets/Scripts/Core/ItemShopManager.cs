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
    Player player;

    PlayerInput itemShopInput;
    [SerializeField] Button[] itemShopButtons;
    [SerializeField] Button[] dessertButtons;
    [SerializeField] Button[] drinkButtons;
    [SerializeField] Button[] goodsButtons;
    [SerializeField] Button[] medicineButtons;
    [SerializeField] Button[] returnButtons;
    [SerializeField] Button[] itemBuyButtons;
    [SerializeField] Button requirementButton;
    [SerializeField] Button[] upgradeButtons;
    Button lackMoneyButton;

    GameObject ItemShopPanel;
    GameObject dessertPanel;
    GameObject drinkPanel;
    GameObject goodsPanel;
    GameObject medicinePanel;
    GameObject ItemBuyPanel;
    GameObject RequirementPanel;
    GameObject LackMoneyImage;

    [SerializeField] Item_Dessert[] item_Dessert;
    [SerializeField] Item_Drink[] item_Drink;
    [SerializeField] Item_Goods[] item_Goods;
    [SerializeField] Item_Medicine[] item_Medicine;

    //아이템 구매패널 관련
    ItemBase currentItem;
    Image itemImage;
    TextMeshProUGUI itemInfoText;
    Scrollbar itemScrollbar;
    TMP_InputField itemInputField;
    TextMeshProUGUI itemPriceText;
    int itemPrice;
    int currentItemCount;
    int currentItemPrice;
    int maxitemCount;
    public Action<int> onChangedRemaining;


    //업그레이드 패널 관련
    GameObject upgradeItemPanel;
    TextMeshProUGUI upgradePriceText;
    TextMeshProUGUI upgradeNowSellPrice;
    TextMeshProUGUI upgradeTooSellPrice;
    TextMeshProUGUI upgradeNowSatisfaction;
    TextMeshProUGUI upgradeTooSatisfaction;


    private void Awake()
    {
        itemShopInput = new PlayerInput();
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
        ItemShopPanel = Canvas.transform.GetChild(0).gameObject;
        dessertPanel = Canvas.transform.GetChild(1).gameObject;
        drinkPanel = Canvas.transform.GetChild(2).gameObject;
        goodsPanel = Canvas.transform.GetChild(3).gameObject;
        medicinePanel = Canvas.transform.GetChild(4).gameObject;
        ItemBuyPanel = Canvas.transform.GetChild(5).gameObject;
        RequirementPanel = Canvas.transform.GetChild(6).gameObject;
        LackMoneyImage = Canvas.transform.GetChild(7).gameObject;
        lackMoneyButton = LackMoneyImage.GetComponentInChildren<Button>();
        //아이템 구매패널
        Transform itemBuyTransform;
        itemBuyTransform = ItemBuyPanel.transform.GetChild(1).transform;
        itemImage = itemBuyTransform.GetChild(1).GetComponent<Image>();
        itemInfoText = itemBuyTransform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemScrollbar = itemBuyTransform.GetChild(3).GetComponent<Scrollbar>();
        itemInputField = itemBuyTransform.GetChild(4).GetComponent<TMP_InputField>();
        itemPriceText = itemBuyTransform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>();
        itemInputField.onValueChanged.AddListener(UpdateBuyPanel);
        itemScrollbar.onValueChanged.AddListener(UpdateBuyPanelBySlider);

        //업그레이드패널
        Transform UpgradeTransform = ItemBuyPanel.transform.GetChild(2).GetChild(0).transform;
        upgradeItemPanel = ItemBuyPanel.transform.GetChild(2).gameObject;
        upgradeNowSatisfaction = UpgradeTransform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
        upgradeNowSellPrice = UpgradeTransform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        upgradeTooSatisfaction = UpgradeTransform.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>();
        upgradeTooSellPrice = UpgradeTransform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        upgradePriceText = UpgradeTransform.GetChild(2).GetComponent<TextMeshProUGUI>();

        lackMoneyButton.onClick.AddListener(CloseLackMoneyImage);
        //아이템 패널의 버튼들
        itemShopButtons[0].onClick.AddListener(GotoDessertPanel);
        itemShopButtons[1].onClick.AddListener(GotoDrinkPanel);
        itemShopButtons[2].onClick.AddListener(GotoGoodsPanel);
        itemShopButtons[3].onClick.AddListener(GotoMedicinePanel);
        itemShopButtons[4].onClick.AddListener(GotoNextDay);

        //아이템 구매 패널의 버튼들
        itemBuyButtons[0].onClick.AddListener(BuyItem);
        itemBuyButtons[1].onClick.AddListener(CloseItemBuyPanel);
        itemBuyButtons[2].onClick.AddListener(OpenUpgradeItemPanel);

        //업그레이드 패널의 버튼들
        upgradeButtons[0].onClick.AddListener(UpgradeItem);
        upgradeButtons[1].onClick.AddListener(CloseUpgradeItemPanel);

        //각 세부 패널들의 버튼들
        foreach (Button dessertButton in dessertButtons)
        {
            dessertButton.onClick.AddListener(() => ShowItemOnBuyPanel(dessertButton));
        }

        foreach (Button drinkButton in drinkButtons)
        {
            drinkButton.onClick.AddListener(() => ShowItemOnBuyPanel(drinkButton));
        }

        foreach (Button goodsButton in goodsButtons)
        {
            goodsButton.onClick.AddListener(() => ShowItemOnBuyPanel(goodsButton));
        }

        foreach (Button medicineButton in medicineButtons)
        {
            medicineButton.onClick.AddListener(() => ShowItemOnBuyPanel(medicineButton));
        }


        //아이템 샵으로 돌아가는 리턴 버튼들
        foreach (Button returnButton in returnButtons)
        {
            returnButton.onClick.AddListener(() => GotoItemShop(returnButton));
        }




    }



    private void OnEnable()
    {
        itemShopInput.Enable();
        itemShopInput.ItemShop._1.performed += PressOneButton;
        itemShopInput.ItemShop._2.performed += PressTwoButton;
        itemShopInput.ItemShop._3.performed += PressThreeButton;
        itemShopInput.ItemShop._4.performed += PressFourButton;
        itemShopInput.ItemShop._5.performed += PressFiveButton;

    }


    private void OnDisable()
    {
        itemShopInput.ItemShop._1.performed -= PressOneButton;
        itemShopInput.ItemShop._2.performed -= PressTwoButton;
        itemShopInput.ItemShop._3.performed -= PressThreeButton;
        itemShopInput.ItemShop._4.performed -= PressFourButton;
        itemShopInput.ItemShop._5.performed -= PressFiveButton;
        itemShopInput.Disable();
    }


    // -----------------아이템샵의 번호에 적힌 숫자를 누르면 해당 버튼을 누르는 함수들

    private void PressOneButton(InputAction.CallbackContext context)
    {
        if (ItemShopPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ItemShopPanel.gameObject.SetActive(false);
            GotoDessertPanel();
        }
        else if (dessertPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (dessertButtons[0] != null)
                ShowItemOnBuyPanel(dessertButtons[0]);
        }
        else if (drinkPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (drinkButtons[0] != null)
                ShowItemOnBuyPanel(drinkButtons[0]);
        }
        else if (goodsPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (goodsButtons[0] != null)
                ShowItemOnBuyPanel(goodsButtons[0]);
        }
        else if (medicinePanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (medicineButtons[0] != null)
                ShowItemOnBuyPanel(medicineButtons[0]);

        }

    }

    private void PressTwoButton(InputAction.CallbackContext context)
    {
        if (ItemShopPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ItemShopPanel.gameObject.SetActive(false);
            GotoDrinkPanel();
        }
        else if (dessertPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (dessertButtons[1] != null)
                ShowItemOnBuyPanel(dessertButtons[1]);
        }
        else if (drinkPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (drinkButtons[1] != null)
                ShowItemOnBuyPanel(drinkButtons[1]);
        }
        else if (goodsPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (goodsButtons[1] != null)
                ShowItemOnBuyPanel(goodsButtons[1]);
        }
        else if (medicinePanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (medicineButtons[1] != null)
                ShowItemOnBuyPanel(medicineButtons[1]);

        }
    }


    private void PressThreeButton(InputAction.CallbackContext context)
    {
        if (ItemShopPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ItemShopPanel.gameObject.SetActive(false);
            GotoGoodsPanel();
        }
        else if (dessertPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (dessertButtons[2] != null)
                ShowItemOnBuyPanel(dessertButtons[2]);
        }
        else if (drinkPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (drinkButtons[2] != null)
                ShowItemOnBuyPanel(drinkButtons[2]);
        }
        else if (goodsPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (goodsButtons[2] != null)
                ShowItemOnBuyPanel(goodsButtons[2]);
        }
        else if (medicinePanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (medicineButtons[2] != null)
                ShowItemOnBuyPanel(medicineButtons[2]);

        };
    }

    private void PressFourButton(InputAction.CallbackContext context)
    {
        if (ItemShopPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ItemShopPanel.gameObject.SetActive(false);
            GotoMedicinePanel();
        }
        else if (dessertPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (dessertButtons[3] != null)
                ShowItemOnBuyPanel(dessertButtons[3]);
        }
        else if (drinkPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (drinkButtons[3] != null)
                ShowItemOnBuyPanel(drinkButtons[3]);
        }
        else if (goodsPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (goodsButtons[3] != null)
                ShowItemOnBuyPanel(goodsButtons[3]);
        }
        else if (medicinePanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (medicineButtons[3] != null)
                ShowItemOnBuyPanel(medicineButtons[3]);

        }
    }
    private void PressFiveButton(InputAction.CallbackContext context)
    {

        if (dessertPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (dessertButtons[4] != null)
                ShowItemOnBuyPanel(dessertButtons[4]);
        }
        else if (drinkPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (drinkButtons[4] != null)
                ShowItemOnBuyPanel(drinkButtons[4]);
        }
        else if (goodsPanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (goodsButtons[4] != null)
                ShowItemOnBuyPanel(goodsButtons[4]);
        }
        else if (medicinePanel.gameObject.activeSelf && !ItemBuyPanel.activeSelf)
        {
            if (medicineButtons[4] != null)
                ShowItemOnBuyPanel(medicineButtons[4]);

        }
    }



    public void PrepareStore()
    {
        ItemShopPanel.SetActive(true);
        itemShopInput.Enable();
    }

    private void CloseLackMoneyImage()
    {
        LackMoneyImage.SetActive(false);
    }


    private void GotoDessertPanel()
    {
        ItemShopPanel.SetActive(false);
        dessertPanel.SetActive(true);
    }
    private void GotoDrinkPanel()
    {
        ItemShopPanel.SetActive(false);
        drinkPanel.SetActive(true);
    }
    private void GotoGoodsPanel()
    {
        ItemShopPanel.SetActive(false);
        goodsPanel.SetActive(true);
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
        itemShopInput.Disable();
    }

    private void GotoItemShop(Button button)
    {
        button.transform.parent.gameObject.SetActive(false);
        ItemShopPanel.SetActive(true);
    }

    private void ShowItemOnBuyPanel(Button itemButton)
    {
        if (itemButton != null)
        {
            if (dessertButtons.Contains(itemButton))
            {
                int index = System.Array.IndexOf(dessertButtons, itemButton);
                if (index >= 0 && index < item_Dessert.Length)
                {
                    Item_Dessert selectedItem = item_Dessert[index];
                    UpdateBuyPanel(selectedItem);
                }
            }
            else if (drinkButtons.Contains(itemButton))
            {
                int index = System.Array.IndexOf(drinkButtons, itemButton);
                if (index >= 0 && index < item_Drink.Length)
                {
                    Item_Drink selectedItem = item_Drink[index];
                    UpdateBuyPanel(selectedItem);
                }
            }
            else if (goodsButtons.Contains(itemButton))
            {
                int index = System.Array.IndexOf(goodsButtons, itemButton);
                if (index >= 0 && index < item_Goods.Length)
                {
                    Item_Goods selectedItem = item_Goods[index];
                    UpdateBuyPanel(selectedItem);
                }
            }
            else if (medicineButtons.Contains(itemButton))
            {
                int index = System.Array.IndexOf(medicineButtons, itemButton);
                if (index >= 0 && index < item_Medicine.Length)
                {
                    Item_Medicine selectedItem = item_Medicine[index];
                    UpdateBuyPanel(selectedItem);
                }
            }
        }
    }
    /// <summary>
    /// 아이템구매창 업데이트
    /// </summary>
    /// <param name="item"></param>
    private void UpdateBuyPanel(ItemBase item)
    {
        currentItem = item;
        currentItemCount = 0;
        currentItemPrice = 0;
        ItemBuyPanel.SetActive(true);
        itemImage.sprite = item.Icon;
        itemInfoText.text = item.ItemInfo;
        itemPrice = item.purchasePrice;
        itemPriceText.text = $"{itemPrice} 원";

        //열 때 최대치 정해놓기
        maxitemCount = 0;
        
        while (player.Money >= currentItemPrice )
        {
            currentItemCount += 1;
            currentItemPrice += itemPrice;
            if(player.Money < currentItemPrice)
            {
                currentItemCount -= 1;
                currentItemPrice -= itemPrice;
                break;
            }
        }
        maxitemCount = currentItemCount;
        currentItemCount = 1;
        currentItemPrice = itemPrice;
        itemScrollbar.value = 0f;
        itemInputField.text = "";
    }

    private void UpdateBuyPanel(string value)
    {
        currentItemCount = int.Parse(value);
         currentItemPrice = itemPrice * currentItemCount;
        //플레이어가 가진돈보다 가격이 높으면 작동
        if (currentItemCount > maxitemCount)
        {
            currentItemCount = maxitemCount;
            itemInputField.text = maxitemCount.ToString();
        }
        itemPriceText.text = $"{currentItemPrice} 원";
        itemInputField.text = $"{currentItemCount}";
    }
    private void UpdateBuyPanelBySlider(float value)
    {
        switch (maxitemCount)
        {
            case <= 10: itemScrollbar.size = 0.3f; break;
            case <= 15: itemScrollbar.size = 0.27f; break;
            case <= 20: itemScrollbar.size = 0.24f; break;
            case <= 30: itemScrollbar.size = 0.2f; break;
            case <= 50: itemScrollbar.size = 0.18f; break;
            case >= 70: itemScrollbar.size = 0.15f; break;
        }
        int sliderValue = (int)(maxitemCount * value);
        UpdateBuyPanel($"{sliderValue}");
    }

    //---------------구매창에서 사고 닫고,업그레이드
    private void BuyItem()
    {
        if(player.Money >= currentItemPrice)
        {
        player.Money -= currentItemPrice;
        currentItem.remaining += currentItemCount;
        onChangedRemaining?.Invoke(currentItem.remaining);
        ItemBuyPanel.SetActive(false);
        }
        else
        {
            LackMoneyImage.SetActive(true);
        }
    }


    private void CloseItemBuyPanel()
    {
        ItemBuyPanel.SetActive(false);
    }

    private void OpenUpgradeItemPanel()
    {
        upgradeNowSellPrice.text = $"{currentItem.sellPrice}원";
        upgradeNowSatisfaction.text = $"{currentItem.satisfaction}";

        upgradeTooSellPrice.text = $"{currentItem.sellPrice + currentItem.sellPrice * currentItem.upgradeEfficiency}원";
        upgradeTooSatisfaction.text = $"{(int)(currentItem.satisfaction + currentItem.satisfaction * currentItem.upgradeEfficiency * 2)}";
        upgradePriceText.text = $"{currentItem.upgradePrice}원";
        upgradeItemPanel.SetActive(true);

    }

    private void UpgradeItem()
    {
        if (player.Money >= currentItem.upgradePrice)
        {
            player.Money -= currentItem.upgradePrice;
            currentItem.UpGrade();
            CloseUpgradeItemPanel();
        }
        else
        {
            LackMoneyImage.SetActive(true);
        }
    }

    private void CloseUpgradeItemPanel()
    {
        upgradeItemPanel.SetActive(false);
    }

    //private void CantUsingButton(ItemBase item)
    //{
    //    item.
    //}


}
