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
    [Header("아이템샵의 버튼들")]
    [SerializeField] Button[] itemShopButtons;

    [Header("아이템샵 하위 버튼들")]
    [SerializeField] Button[] dessertButtons;
    [SerializeField] Button[] drinkButtons;
    [SerializeField] Button[] goodsButtons;
    [SerializeField] Button[] medicineButtons;
    [SerializeField] Button[] returnButtons;
    [SerializeField] Button[] itemBuyButtons;
    [SerializeField] Button requirementButton;
    [SerializeField] Button[] upgradeButtons;
    Button lackMoneyButton;
    [Header("가구상점 버튼들")]
    [SerializeField] Button[] furnitureDownButtons;
    [SerializeField] Button[] furnitureLeftButtons;
    [SerializeField] Button[] furnitureBuyButtons;

    GameObject ItemShopPanel;
    GameObject dessertPanel;
    GameObject drinkPanel;
    GameObject goodsPanel;
    GameObject medicinePanel;
    GameObject ItemBuyPanel;
    GameObject requirementPanel;
    GameObject lackMoneyImage;
    GameObject furnitureBuyPanel;

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
    int maxItemCount;
    public Action<int> onChangedRemaining;

    //구매 조건 텍스트
    TextMeshProUGUI requirementText;
    public TextMeshProUGUI[] dessertRemainingTexts;
    public TextMeshProUGUI[] drinkRemainingTexts;
    public TextMeshProUGUI[] goodsRemainingTexts;
    public TextMeshProUGUI[] medicineRemainingTexts;

    //업그레이드 패널 관련
    GameObject upgradeItemPanel;
    TextMeshProUGUI upgradePriceText;
    TextMeshProUGUI upgradeNowSellPrice;
    TextMeshProUGUI upgradeTooSellPrice;
    TextMeshProUGUI upgradeNowSatisfaction;
    TextMeshProUGUI upgradeTooSatisfaction;
    //스크롤바
    private const float MinScrollbarSize = 0.15f;
    private const float MaxScrollbarSize = 0.3f;

    //가구상점 
    private GameObject storeFurniturePanel;
    private Image[] storeFurnitureImages;
    [Header("가구 상점의 이미지들")]
    [SerializeField] private Sprite[] colorImages;
    [SerializeField] private Sprite[] furnitureImages;
    [SerializeField] private Sprite[] decorationImages;
    [Header("가구 상점의 상품들")]
    [SerializeField] private Material[] colorMaterials;
    [SerializeField] private GameObject[] furniturelObjects;
    [SerializeField] private GameObject[] furnitureles;
    [SerializeField] private GameObject[] decorationObjects;
    private TextMeshProUGUI furnitureBuyText;
    private int FurnitureiPanelController = 0;
    private TextMeshProUGUI[] furnitureLeftBarTexts;
    private int previousControllerNum = 0;
    private bool[] furnitureActivated = new bool[5];
    private bool[] decorationActivated = new bool[5];
    private int currentButtonIndex;
    private int furnitureCusionCount = 0;
    [Header("쿠션개수")]
    [SerializeField] private int furnitureCusionMAX = 6;
    private GameObject chooseFurniture;
    [Header("상점관련")]
    public GameObject store;
    public MeshRenderer storeColor;
    public MeshRenderer storeDoorColor;
    public MeshRenderer CounterColor;



    private void Awake()
    {
        itemShopInput = new PlayerInput();
    }



    public void SetupUI()
    {

        player = GameManager.Instance.Player;
        GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
        ItemShopPanel = Canvas.transform.GetChild(0).gameObject;
        dessertPanel = Canvas.transform.GetChild(1).gameObject;
        drinkPanel = Canvas.transform.GetChild(2).gameObject;
        goodsPanel = Canvas.transform.GetChild(3).gameObject;
        medicinePanel = Canvas.transform.GetChild(4).gameObject;
        ItemBuyPanel = Canvas.transform.GetChild(5).gameObject;
        requirementPanel = Canvas.transform.GetChild(6).gameObject;
        lackMoneyImage = Canvas.transform.GetChild(10).gameObject;
        lackMoneyButton = lackMoneyImage.GetComponentInChildren<Button>();
        storeFurniturePanel = Canvas.transform.GetChild(9).gameObject;
        furnitureBuyPanel = Canvas.transform.GetChild(8).gameObject;
        //아이템 구매패널
        Transform itemBuyTransform;
        itemBuyTransform = ItemBuyPanel.transform.GetChild(1).transform;
        itemImage = itemBuyTransform.GetChild(1).GetComponent<Image>();
        itemInfoText = itemBuyTransform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemScrollbar = itemBuyTransform.GetChild(3).GetComponent<Scrollbar>();
        itemInputField = itemBuyTransform.GetChild(4).GetComponent<TMP_InputField>();
        itemPriceText = itemBuyTransform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>();
        itemInputField.onValueChanged.AddListener(UpdateBuyPanelByInput);
        itemScrollbar.onValueChanged.AddListener(UpdateBuyPanelBySlider);

        //조건 텍스트
        requirementText = requirementPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        dessertRemainingTexts = new TextMeshProUGUI[dessertButtons.Length];
        for (int i = 0; i < dessertButtons.Length; i++)
        {
            dessertRemainingTexts[i] = dessertButtons[i].transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>();
        }
        drinkRemainingTexts = new TextMeshProUGUI[drinkButtons.Length];
        for (int i = 0; i < drinkButtons.Length; i++)
        {
            drinkRemainingTexts[i] = drinkButtons[i].transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>();
        }
        goodsRemainingTexts = new TextMeshProUGUI[goodsButtons.Length];
        for (int i = 0; i < goodsButtons.Length; i++)
        {
            goodsRemainingTexts[i] = goodsButtons[i].transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>();
        }
        medicineRemainingTexts = new TextMeshProUGUI[medicineButtons.Length];
        for (int i = 0; i < medicineButtons.Length; i++)
        {
            medicineRemainingTexts[i] = medicineButtons[i].transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        //업그레이드패널
        Transform UpgradeTransform = ItemBuyPanel.transform.GetChild(2).GetChild(0).transform;
        upgradeItemPanel = ItemBuyPanel.transform.GetChild(2).gameObject;
        upgradeNowSatisfaction = UpgradeTransform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
        upgradeNowSellPrice = UpgradeTransform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        upgradeTooSatisfaction = UpgradeTransform.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>();
        upgradeTooSellPrice = UpgradeTransform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        upgradePriceText = UpgradeTransform.GetChild(2).GetComponent<TextMeshProUGUI>();


        //가구 패널
        storeFurnitureImages = new Image[furnitureDownButtons.Length];
        for (int i = 0; i < furnitureDownButtons.Length; i++)
        {
            storeFurnitureImages[i] = furnitureDownButtons[i].transform.GetChild(0).GetComponent<Image>();
        }
        furnitureLeftBarTexts = new TextMeshProUGUI[3];
        for (int i = 0; i < furnitureLeftButtons.Length; i++)
        {
            furnitureLeftBarTexts[i] = furnitureLeftButtons[i].GetComponentInChildren<TextMeshProUGUI>();
        }
        furnitureBuyText = furnitureBuyPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        SetupButtonListeners();
    }

    private void SetupButtonListeners()
    {
        lackMoneyButton.onClick.AddListener(CloseLackMoneyImage);
        //아이템 패널의 버튼들
        itemShopButtons[0].onClick.AddListener(GotoDessertPanel);
        itemShopButtons[1].onClick.AddListener(GotoDrinkPanel);
        itemShopButtons[2].onClick.AddListener(GotoGoodsPanel);
        itemShopButtons[3].onClick.AddListener(GotoMedicinePanel);
        itemShopButtons[4].onClick.AddListener(GotoFurniturePanel);
        itemShopButtons[5].onClick.AddListener(GotoNextDay);

        //아이템 구매 패널의 버튼들
        itemBuyButtons[0].onClick.AddListener(BuyItem);
        itemBuyButtons[1].onClick.AddListener(CloseItemBuyPanel);
        itemBuyButtons[2].onClick.AddListener(OpenUpgradeItemPanel);

        //업그레이드 패널의 버튼들
        upgradeButtons[0].onClick.AddListener(UpgradeItem);
        upgradeButtons[1].onClick.AddListener(CloseUpgradeItemPanel);

        //조건 패널 버튼
        requirementButton.onClick.AddListener(OffRequirementPanel);

        //가구패널 버튼
        furnitureLeftButtons[0].onClick.AddListener(ChangeCafeThemePanel);
        furnitureLeftButtons[1].onClick.AddListener(ChangeFurniturePanel);
        furnitureLeftButtons[2].onClick.AddListener(ChangeDecorationPanel);

        furnitureDownButtons[0].onClick.AddListener(FirstButtonOnFurniturePanel);
        furnitureDownButtons[1].onClick.AddListener(SecondButtonOnFurniturePanel);
        furnitureDownButtons[2].onClick.AddListener(ThirdButtonOnFurniturePanel);
        furnitureDownButtons[3].onClick.AddListener(FourthButtonOnFurniturePanel);
        furnitureDownButtons[4].onClick.AddListener(FifthButtonOnFurniturePanel);

        furnitureBuyButtons[0].onClick.AddListener(BuyFurnitrues);
        furnitureBuyButtons[1].onClick.AddListener(CloseBuyFurniturePanel);

        //각 세부 패널들의 버튼들
        for (int i = 0; i < dessertButtons.Length; i++)
        {
            int index = i;
            dessertButtons[i].onClick.AddListener(() => ShowItemOnBuyPanel<Item_Dessert>(index));
        }

        for (int i = 0; i < drinkButtons.Length; i++)
        {
            int index = i;
            drinkButtons[i].onClick.AddListener(() => ShowItemOnBuyPanel<Item_Drink>(index));
        }

        for (int i = 0; i < goodsButtons.Length; i++)
        {
            int index = i;
            goodsButtons[i].onClick.AddListener(() => ShowItemOnBuyPanel<Item_Goods>(index));
        }

        for (int i = 0; i < medicineButtons.Length; i++)
        {
            int index = i;
            medicineButtons[i].onClick.AddListener(() => ShowItemOnBuyPanel<Item_Medicine>(index));
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
        itemShopInput.ItemShop.ReturnItemShop.performed += ReturnItemShop;

    }



    private void OnDisable()
    {
        itemShopInput.ItemShop._1.performed -= PressOneButton;
        itemShopInput.ItemShop._2.performed -= PressTwoButton;
        itemShopInput.ItemShop._3.performed -= PressThreeButton;
        itemShopInput.ItemShop._4.performed -= PressFourButton;
        itemShopInput.ItemShop._5.performed -= PressFiveButton;
        itemShopInput.ItemShop.ReturnItemShop.performed -= ReturnItemShop;
        itemShopInput.Disable();
    }


    // -----------------아이템샵의 번호에 적힌 숫자를 누르면 해당 버튼을 누르는 함수들


    private void PressOneButton(InputAction.CallbackContext context)
    {
        if (ItemShopPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ItemShopPanel.SetActive(false);
            GotoDessertPanel();
        }
        else if (dessertPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Dessert>(0);
        }
        else if (drinkPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Drink>(0);
        }
        else if (goodsPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Goods>(0);
        }
        else if (medicinePanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Medicine>(0);
        }
        else if (storeFurniturePanel.activeSelf)
        {
            ChangeCafeThemePanel();
        }
    }

    private void PressTwoButton(InputAction.CallbackContext context)
    {
        if (ItemShopPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ItemShopPanel.SetActive(false);
            GotoDrinkPanel();
        }
        else if (dessertPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Dessert>(1);
        }
        else if (drinkPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Drink>(1);
        }
        else if (goodsPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Goods>(1);
        }
        else if (medicinePanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Medicine>(1);
        }
        else if (storeFurniturePanel.activeSelf)
        {
            ChangeFurniturePanel();
        }
    }


    private void PressThreeButton(InputAction.CallbackContext context)
    {
        if (ItemShopPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ItemShopPanel.SetActive(false);
            GotoGoodsPanel();
        }
        else if (dessertPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Dessert>(2);
        }
        else if (drinkPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Drink>(2);
        }
        else if (goodsPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Goods>(2);
        }
        else if (medicinePanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Medicine>(2);
        }
        else if (storeFurniturePanel.activeSelf)
        {
            ChangeDecorationPanel();
        }
    }

    private void PressFourButton(InputAction.CallbackContext context)
    {
        if (ItemShopPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ItemShopPanel.SetActive(false);
            GotoMedicinePanel();
        }
        else if (dessertPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Dessert>(3);
        }
        else if (drinkPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Drink>(3);
        }
        else if (goodsPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Goods>(3);
        }
        else if (medicinePanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Medicine>(3);
        }
    }
    private void PressFiveButton(InputAction.CallbackContext context)
    {
        if (ItemShopPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ItemShopPanel.SetActive(false);
            GotoFurniturePanel();
        }
        if (dessertPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Dessert>(4);
        }
        else if (drinkPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Drink>(4);
        }
        else if (goodsPanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Goods>(4);
        }
        else if (medicinePanel.activeSelf && !ItemBuyPanel.activeSelf)
        {
            ShowItemOnBuyPanel<Item_Medicine>(4);
        }
    }

    private void ReturnItemShop(InputAction.CallbackContext context)
    {
        GameManager.Instance.PlayerControl.DisableAction();
        if (!ItemShopPanel.activeSelf && !ItemBuyPanel.activeSelf && !requirementPanel.activeSelf)
        {
            Button returnbutton = GameObject.FindGameObjectWithTag("ReturnButton").GetComponent<Button>();
            GotoItemShop(returnbutton);
        }
        else if (requirementPanel.activeSelf)
        {
            requirementPanel.SetActive(false);
        }
        else if (upgradeItemPanel.activeSelf)
        {
            upgradeItemPanel.SetActive(false);
        }
        else if (ItemBuyPanel.activeSelf)
        {
            ItemBuyPanel.SetActive(false);
        }

    }

    public void PrepareStore()
    {
        Debug.Log("상점준비 시작");
        ItemShopPanel.SetActive(true);
        itemShopInput.Enable();
        GameManager.Instance.PlayerControl.DisableAction();
        UpdateRemainingTexts();
    }

    private void CloseLackMoneyImage()
    {
        lackMoneyImage.SetActive(false);
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
    private void GotoFurniturePanel()
    {
        ItemShopPanel.SetActive(false);
        player.transform.position = new Vector3(0, 10, 0);
        storeFurniturePanel.SetActive(true);
        GameManager.Instance.PlayerControl.EnableWASD();

        ChangeCafeThemePanel();

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

    private void ShowItemOnBuyPanel<T>(int index) where T : ItemBase
    {
        ItemBase item = ItemManager.Instance.GetItemByIndex<T>(index);
        if (item != null)
        {
            UpdateBuyPanel(item);
        }
    }


    /// <summary>
    /// 아이템구매창 업데이트
    /// </summary>
    /// <param name="item"></param>
    private void UpdateBuyPanel(ItemBase item)
    {
        if (!item.itemCantBuy && !requirementPanel.activeSelf)
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
            maxItemCount = 0;

            while (player.Money >= currentItemPrice)
            {
                currentItemCount += 1;
                currentItemPrice += itemPrice;
                if (player.Money < currentItemPrice)
                {
                    currentItemCount -= 1;
                    currentItemPrice -= itemPrice;
                    break;
                }
            }
            maxItemCount = currentItemCount;
            currentItemCount = 1;
            currentItemPrice = itemPrice;
            itemScrollbar.value = 0f;
            itemInputField.text = "";
        }
        //조건 불충족 및 조건 패널이 닫혀있을 때
        else if (item.itemCantBuy && !requirementPanel.activeSelf)
        {
            requirementPanel.SetActive(true);
            requirementText.text = item.requirementinfo;
        }
    }
    private void OffRequirementPanel()
    {
        requirementPanel.SetActive(false);
    }

    // 인풋필드로 아이템 개수 변환
    private void UpdateBuyPanelByInput(string value)
    {
        currentItemCount = int.Parse(value);
        currentItemPrice = itemPrice * currentItemCount;
        //플레이어가 가진돈보다 가격이 높으면 작동
        if (currentItemCount > maxItemCount)
        {
            currentItemCount = maxItemCount;
            itemInputField.text = maxItemCount.ToString();
        }
        itemPriceText.text = $"{currentItemPrice} 원";
        itemInputField.text = $"{currentItemCount}";
    }

    //슬라이더로 슬라이더 크기 및 개수 변환
    private void UpdateBuyPanelBySlider(float value)
    {
        float scrollbarSize = Mathf.Lerp(MaxScrollbarSize, MinScrollbarSize, (float)maxItemCount / 70f);
        itemScrollbar.size = scrollbarSize;

        int sliderValue = Mathf.RoundToInt(maxItemCount * value);
        UpdateBuyPanelByInput(sliderValue.ToString());
    }

    //---------------구매창에서 사고 닫고,업그레이드
    private void BuyItem()
    {
        if (player.Money >= currentItemPrice)
        {
            player.Money -= currentItemPrice;
            currentItem.remaining += currentItemCount;
            onChangedRemaining?.Invoke(currentItem.remaining);
            ItemBuyPanel.SetActive(false);

            //재고 업데이트
            if (currentItem is Item_Dessert)
            {
                int index = System.Array.IndexOf(ItemManager.Instance.runtimeDessertItems, currentItem);
                if (index >= 0)
                {
                    dessertRemainingTexts[index].text = $"재고 : {currentItem.remaining}개";
                }
            }
            else if (currentItem is Item_Drink)
            {
                int index = System.Array.IndexOf(ItemManager.Instance.runtimeDrinkItems, currentItem);
                if (index >= 0)
                {
                    drinkRemainingTexts[index].text = $"재고 : {currentItem.remaining}개";
                }
            }
            else if (currentItem is Item_Goods)
            {
                int index = System.Array.IndexOf(ItemManager.Instance.runtimeGoodsItems, currentItem);
                if (index >= 0)
                {
                    goodsRemainingTexts[index].text = $"재고 : {currentItem.remaining}개";
                }
            }
            else if (currentItem is Item_Medicine)
            {
                int index = System.Array.IndexOf(ItemManager.Instance.runtimeMedicineItems, currentItem);
                if (index >= 0)
                {
                    medicineRemainingTexts[index].text = $"재고 : {currentItem.remaining}개";
                }
            }
        }

        else
        {
            lackMoneyImage.SetActive(true);
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
            lackMoneyImage.SetActive(true);
        }
    }

    private void CloseUpgradeItemPanel()
    {
        upgradeItemPanel.SetActive(false);
    }

    //-------------------------------------상점 재고 업데이트
    private void UpdateRemainingTexts()
    {
        for (int i = 0; i < ItemManager.Instance.runtimeDessertItems.Length; i++)
        {
            dessertRemainingTexts[i].text = $"재고 : {ItemManager.Instance.runtimeDessertItems[i].remaining}개";
        }

        for (int i = 0; i < ItemManager.Instance.runtimeDrinkItems.Length; i++)
        {
            drinkRemainingTexts[i].text = $"재고 : {ItemManager.Instance.runtimeDrinkItems[i].remaining}개";
        }

        for (int i = 0; i < ItemManager.Instance.runtimeGoodsItems.Length; i++)
        {
            goodsRemainingTexts[i].text = $"재고 : {ItemManager.Instance.runtimeGoodsItems[i].remaining}개";
        }

        for (int i = 0; i < ItemManager.Instance.runtimeMedicineItems.Length; i++)
        {
            medicineRemainingTexts[i].text = $"재고 : {ItemManager.Instance.runtimeMedicineItems[i].remaining}개";
        }
    }

    //------------------가구상점 관련,왼쪽패널
    private void ChangeCafeThemePanel()
    {
        if (store == null)
        {
            store = GameObject.FindWithTag("MergedStore");
            storeColor = store.transform.GetChild(0).GetComponent<MeshRenderer>();
            storeDoorColor = store.transform.GetChild(2).GetComponent<MeshRenderer>();
            CounterColor = store.transform.GetChild(3).GetComponent<MeshRenderer>();

        }
        if (FurnitureiPanelController != 1)
        {
            for (int i = 0; i < furnitureDownButtons.Length; i++)
            {
                if (colorImages[i] == null)
                {
                    furnitureDownButtons[i].gameObject.SetActive(false);
                }
                else
                {

                    furnitureDownButtons[i].gameObject.SetActive(true);
                    storeFurnitureImages[i].sprite = colorImages[i];
                }
            }

            FurnitureiPanelController = 1;
            furnitureLeftBarTexts[0].color = Color.cyan;
            if (FurnitureiPanelController - 1 != previousControllerNum)
            {
                furnitureLeftBarTexts[previousControllerNum].color = Color.black;
            }
        }
        previousControllerNum = 0;
        UpdateButtonStates();
    }

    private void ChangeFurniturePanel()
    {
        if (FurnitureiPanelController != 2)
        {
            for (int i = 0; i < furnitureDownButtons.Length; i++)
            {
                if (furnitureImages[i] == null)
                {
                    furnitureDownButtons[i].gameObject.SetActive(false);
                }
                else
                {

                    furnitureDownButtons[i].gameObject.SetActive(true);
                    storeFurnitureImages[i].sprite = furnitureImages[i];
                }
            }
            furnitureLeftBarTexts[1].color = Color.cyan;
            furnitureLeftBarTexts[previousControllerNum].color = Color.black;
        }
        FurnitureiPanelController = 2;
        previousControllerNum = 1;
        UpdateButtonStates();

    }

    private void ChangeDecorationPanel()
    {
        if (FurnitureiPanelController != 3)
        {
            for (int i = 0; i < furnitureDownButtons.Length; i++)
            {
                if (decorationImages[i] == null)
                {
                    furnitureDownButtons[i].gameObject.SetActive(false);
                }
                else
                {

                    furnitureDownButtons[i].gameObject.SetActive(true);
                    storeFurnitureImages[i].sprite = decorationImages[i];
                }
            }
            furnitureLeftBarTexts[2].color = Color.cyan;
            furnitureLeftBarTexts[previousControllerNum].color = Color.black;
        }
        FurnitureiPanelController = 3;
        previousControllerNum = 2;
        UpdateButtonStates();
    }

    //------------------가구상점관련, 아래쪽 패널
    private void FirstButtonOnFurniturePanel()
    {
        currentButtonIndex = 0;
        switch (FurnitureiPanelController)
        {
            case 1:
                storeColor.material = colorMaterials[0];
                storeDoorColor.material = colorMaterials[0];
                CounterColor.material = colorMaterials[5];
                break;
            case 2:
                if (!furnitureActivated[0])
                {
                    OpenFurniturePanel(ItemManager.Instance.GetFurniturePrice(0));
                    chooseFurniture = furnitureles[furnitureles.Length - 2];
                }
                break;
            case 3:
                if (!decorationActivated[0])
                {
                    OpenFurniturePanel(ItemManager.Instance.GetDecorationPrice(0));
                    chooseFurniture = furnitureles[furnitureles.Length - 1];
                }
                break;
        }
    }

    private void SecondButtonOnFurniturePanel()
    {
        currentButtonIndex = 1;
        switch (FurnitureiPanelController)
        {
            case 1:
                storeColor.material = colorMaterials[1];
                storeDoorColor.material = colorMaterials[1];
                CounterColor.material = colorMaterials[0];
                break;
            case 2:
                if (furnitureCusionCount < furnitureCusionMAX)
                {
                    OpenFurniturePanel(ItemManager.Instance.GetFurniturePrice(1));
                    chooseFurniture = furnitureles[furnitureCusionCount];
                }
                break;
            case 3:
                break;


        }
    }

    private void ThirdButtonOnFurniturePanel()
    {
        currentButtonIndex = 2;

        switch (FurnitureiPanelController)
        {
            case 1:
                storeColor.material = colorMaterials[2];
                storeDoorColor.material = colorMaterials[2];
                CounterColor.material = colorMaterials[6];
                break;

            case 2:
                if (furnitureCusionCount < furnitureCusionMAX)
                {
                    OpenFurniturePanel(ItemManager.Instance.GetFurniturePrice(2));
                    chooseFurniture = furnitureles[furnitureCusionCount];
                }
                break;

            case 3:

                break;
        }
    }
    private void FourthButtonOnFurniturePanel()
    {
        currentButtonIndex = 3;
        switch (FurnitureiPanelController)
        {
            case 1:
                storeColor.material = colorMaterials[3];
                storeDoorColor.material = colorMaterials[3];
                CounterColor.material = colorMaterials[2];
                break;
            case 2:
                if (furnitureCusionCount < furnitureCusionMAX)
                {
                    OpenFurniturePanel(ItemManager.Instance.GetFurniturePrice(3));
                    chooseFurniture = furnitureles[furnitureCusionCount];
                }
                break;

            case 3:

                break;
        }
    }
    private void FifthButtonOnFurniturePanel()
    {
        currentButtonIndex = 4;
        switch (FurnitureiPanelController)
        {
            case 1:
                storeColor.material = colorMaterials[4];
                storeDoorColor.material = colorMaterials[4];
                CounterColor.material = colorMaterials[5];
                break;

            case 2:
                OpenFurniturePanel(ItemManager.Instance.GetFurniturePrice(4));
                break;
                
            case 3: break;

        }
    }

    //------------------------가구구매창 관련(열기,닫기,구매)
    private void OpenFurniturePanel(int price)
    {
        furnitureBuyPanel.SetActive(true);
        furnitureBuyText.text = $"{price}원";
        currentItemPrice = price;
    }


    private void CloseBuyFurniturePanel()
    {
        furnitureBuyPanel?.SetActive(false);
    }

    private void BuyFurnitrues()
    {
        if (player.Money >= currentItemPrice)
        {
            player.Money -= currentItemPrice;

            switch (FurnitureiPanelController)
            {
                case 2: // 기능가구

                    ActivateFurniture(currentButtonIndex);
                    break;
                case 3: // 장식가구
                    ActivateDecoration(currentButtonIndex);
                    break;
            }

            CloseBuyFurniturePanel();
            UpdateButtonStates();
        }
        else
        {
            lackMoneyImage.SetActive(true);
        }
    }

    private void ActivateFurniture(int index)
    {
        if (index == 0 && !furnitureActivated[0])
        {
            furniturelObjects[0].SetActive(true);
            furnitureActivated[0] = true;
            chooseFurniture.gameObject.SetActive(true);
            ItemManager.Instance.GetItemByIndex<Item_Drink>(2).itemCantBuy = false;
            ItemManager.Instance.GetItemByIndex<Item_Drink>(4).itemCantBuy = false;
            Transform CanbuyTransform = drinkButtons[2].gameObject.transform.parent.GetChild(2).transform;
            CanbuyTransform.gameObject.SetActive(false);
            CanbuyTransform = drinkButtons[4].gameObject.transform.parent.GetChild(2).transform;
            CanbuyTransform.gameObject.SetActive(false);
        }
        else if (index >= 1 && index <= 3)
        {
            furnitureCusionCount++;
            UpgradeCushions(index);

            if (furnitureCusionCount >= furnitureCusionMAX)
            {
                for (int i = 1; i <= 3; i++)
                {
                    furniturelObjects[i].SetActive(true);
                    furnitureActivated[i] = true;
                }
            }
        }
    }

    private void UpgradeCushions(int level)
    {
        Vector3 position = chooseFurniture.transform.position;
        Quaternion rotation = chooseFurniture.transform.rotation;
        Transform parent = chooseFurniture.transform.parent;
        switch (level)
        { 
            case 1:
                chooseFurniture.SetActive(true);
                break;
            case 2:
                Destroy(chooseFurniture);
                GameObject newSuperCushion = Instantiate(furniturelObjects[2], position,rotation ,parent);
                break;
            case 3:
                Destroy(chooseFurniture);
                GameObject newPerfectCushion =  Instantiate(furniturelObjects[3], position,rotation, parent);
                break;
        }
    }


    private void ActivateDecoration(int index)
    {
        if (index >= 0 && index < decorationObjects.Length && !decorationActivated[index])
        {
            decorationObjects[index].SetActive(true);
            decorationActivated[index] = true;
            chooseFurniture.gameObject.SetActive(true);
        }
    }

    private void UpdateButtonStates()
    {
        for (int i = 0; i < furnitureDownButtons.Length; i++)
        {
            if (FurnitureiPanelController == 2)
            {
                if (i == 0 || i == 4)
                {
                    furnitureDownButtons[i].interactable = !furnitureActivated[i];
                }
                else // i가 1, 2, 3 일 때
                {
                    furnitureDownButtons[i].interactable = furnitureCusionCount < furnitureCusionMAX;
                }
            }
            else if (FurnitureiPanelController == 3)
            {
                furnitureDownButtons[i].interactable = !decorationActivated[i];
            }
            else
            {
                furnitureDownButtons[i].interactable = true;
            }
        }
    }


    public void GoldDogGumActivate()
    {
        dessertButtons[3].gameObject.transform.parent.GetChild(2).gameObject.SetActive(false);
    }

}
