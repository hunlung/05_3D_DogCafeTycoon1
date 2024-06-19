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
    [Header("�����ۼ��� ��ư��")]
    [SerializeField] Button[] itemShopButtons;

    [Header("�����ۼ� ���� ��ư��")]
    [SerializeField] Button[] dessertButtons;
    [SerializeField] Button[] drinkButtons;
    [SerializeField] Button[] goodsButtons;
    [SerializeField] Button[] medicineButtons;
    [SerializeField] Button[] returnButtons;
    [SerializeField] Button[] itemBuyButtons;
    [SerializeField] Button requirementButton;
    [SerializeField] Button[] upgradeButtons;
    Button lackMoneyButton;
    [Header("�������� ��ư��")]
    [SerializeField] Button[] furnitureDownButtons;
    [SerializeField] Button[] furnitureLeftButtons;

    GameObject ItemShopPanel;
    GameObject dessertPanel;
    GameObject drinkPanel;
    GameObject goodsPanel;
    GameObject medicinePanel;
    GameObject ItemBuyPanel;
    GameObject requirementPanel;
    GameObject lackMoneyImage;


    //������ �����г� ����
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

    //���� ���� �ؽ�Ʈ
    TextMeshProUGUI requirementText;

    //���׷��̵� �г� ����
    GameObject upgradeItemPanel;
    TextMeshProUGUI upgradePriceText;
    TextMeshProUGUI upgradeNowSellPrice;
    TextMeshProUGUI upgradeTooSellPrice;
    TextMeshProUGUI upgradeNowSatisfaction;
    TextMeshProUGUI upgradeTooSatisfaction;
    //��ũ�ѹ�
    private const float MinScrollbarSize = 0.15f;
    private const float MaxScrollbarSize = 0.3f;

    //�������� 
    private GameObject storeFurniturePanel;
    private Image[] storeFurnitureImages;
    [Header("���� ������ �̹�����")]
    [SerializeField] private Sprite[] colorImages;
    [SerializeField] private Sprite[] furnitureImages;
    [SerializeField] private Sprite[] decorationImages;
    [Header("���� ������ ��ǰ��")]
    [SerializeField] private Material[] colorMaterials;
    [SerializeField] private GameObject[] furniturelObjects;
    [SerializeField] private GameObject[] decorationObjects;

    private int FurnitureiPanelController = 1;
    private TextMeshProUGUI[] furnitureLeftBarTexts;
    private int previousControllerNum = 0;

    public GameObject store;
    public MeshRenderer storeColor;
    public MeshRenderer storeDoorColor;
    public MeshRenderer CounterColor;
    
    

    private void Awake()
    {
        itemShopInput = new PlayerInput();
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        SetupUI();
        SetupButtonListeners();
    }


    private void SetupUI()
    {
        GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
        ItemShopPanel = Canvas.transform.GetChild(0).gameObject;
        dessertPanel = Canvas.transform.GetChild(1).gameObject;
        drinkPanel = Canvas.transform.GetChild(2).gameObject;
        goodsPanel = Canvas.transform.GetChild(3).gameObject;
        medicinePanel = Canvas.transform.GetChild(4).gameObject;
        ItemBuyPanel = Canvas.transform.GetChild(5).gameObject;
        requirementPanel = Canvas.transform.GetChild(6).gameObject;
        lackMoneyImage = Canvas.transform.GetChild(7).gameObject;
        lackMoneyButton = lackMoneyImage.GetComponentInChildren<Button>();
        storeFurniturePanel = Canvas.transform.GetChild(9).gameObject;
        //������ �����г�
        Transform itemBuyTransform;
        itemBuyTransform = ItemBuyPanel.transform.GetChild(1).transform;
        itemImage = itemBuyTransform.GetChild(1).GetComponent<Image>();
        itemInfoText = itemBuyTransform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemScrollbar = itemBuyTransform.GetChild(3).GetComponent<Scrollbar>();
        itemInputField = itemBuyTransform.GetChild(4).GetComponent<TMP_InputField>();
        itemPriceText = itemBuyTransform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>();
        itemInputField.onValueChanged.AddListener(UpdateBuyPanelByInput);
        itemScrollbar.onValueChanged.AddListener(UpdateBuyPanelBySlider);

        //���� �ؽ�Ʈ
        requirementText = requirementPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        //���׷��̵��г�
        Transform UpgradeTransform = ItemBuyPanel.transform.GetChild(2).GetChild(0).transform;
        upgradeItemPanel = ItemBuyPanel.transform.GetChild(2).gameObject;
        upgradeNowSatisfaction = UpgradeTransform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
        upgradeNowSellPrice = UpgradeTransform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        upgradeTooSatisfaction = UpgradeTransform.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>();
        upgradeTooSellPrice = UpgradeTransform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        upgradePriceText = UpgradeTransform.GetChild(2).GetComponent<TextMeshProUGUI>();


        //���� �г�
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

    }

    private void SetupButtonListeners()
    {
        lackMoneyButton.onClick.AddListener(CloseLackMoneyImage);
        //������ �г��� ��ư��
        itemShopButtons[0].onClick.AddListener(GotoDessertPanel);
        itemShopButtons[1].onClick.AddListener(GotoDrinkPanel);
        itemShopButtons[2].onClick.AddListener(GotoGoodsPanel);
        itemShopButtons[3].onClick.AddListener(GotoMedicinePanel);
        itemShopButtons[4].onClick.AddListener(GotoFurniturePanel);
        itemShopButtons[5].onClick.AddListener(GotoNextDay);

        //������ ���� �г��� ��ư��
        itemBuyButtons[0].onClick.AddListener(BuyItem);
        itemBuyButtons[1].onClick.AddListener(CloseItemBuyPanel);
        itemBuyButtons[2].onClick.AddListener(OpenUpgradeItemPanel);

        //���׷��̵� �г��� ��ư��
        upgradeButtons[0].onClick.AddListener(UpgradeItem);
        upgradeButtons[1].onClick.AddListener(CloseUpgradeItemPanel);

        //���� �г� ��ư
        requirementButton.onClick.AddListener(OffRequirementPanel);

        //�����г� ��ư
        furnitureLeftButtons[0].onClick.AddListener(ChangeCafeThemePanel);
        furnitureLeftButtons[1].onClick.AddListener(ChangeFurniturePanel);
        furnitureLeftButtons[2].onClick.AddListener(ChangeDecorationPanel);

        furnitureDownButtons[0].onClick.AddListener(FirstButtonOnFurniturePanel);
        furnitureDownButtons[1].onClick.AddListener(SecondButtonOnFurniturePanel);
        furnitureDownButtons[2].onClick.AddListener(ThirdButtonOnFurniturePanel);
        furnitureDownButtons[3].onClick.AddListener(FourthButtonOnFurniturePanel);
        furnitureDownButtons[4].onClick.AddListener(FifthButtonOnFurniturePanel);

        //�� ���� �гε��� ��ư��
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

        //������ ������ ���ư��� ���� ��ư��
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


    // -----------------�����ۼ��� ��ȣ�� ���� ���ڸ� ������ �ش� ��ư�� ������ �Լ���


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
        ItemShopPanel.SetActive(true);
        itemShopInput.Enable();
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
        ChangeCafeThemePanel();
        storeFurniturePanel.SetActive(true);

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
    /// �����۱���â ������Ʈ
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
            itemPriceText.text = $"{itemPrice} ��";

            //�� �� �ִ�ġ ���س���
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
        //���� ������ �� ���� �г��� �������� ��
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

    // ��ǲ�ʵ�� ������ ���� ��ȯ
    private void UpdateBuyPanelByInput(string value)
    {
        currentItemCount = int.Parse(value);
        currentItemPrice = itemPrice * currentItemCount;
        //�÷��̾ ���������� ������ ������ �۵�
        if (currentItemCount > maxItemCount)
        {
            currentItemCount = maxItemCount;
            itemInputField.text = maxItemCount.ToString();
        }
        itemPriceText.text = $"{currentItemPrice} ��";
        itemInputField.text = $"{currentItemCount}";
    }

    //�����̴��� �����̴� ũ�� �� ���� ��ȯ
    private void UpdateBuyPanelBySlider(float value)
    {
        float scrollbarSize = Mathf.Lerp(MaxScrollbarSize, MinScrollbarSize, (float)maxItemCount / 70f);
        itemScrollbar.size = scrollbarSize;

        int sliderValue = Mathf.RoundToInt(maxItemCount * value);
        UpdateBuyPanelByInput(sliderValue.ToString());
    }

    //---------------����â���� ��� �ݰ�,���׷��̵�
    private void BuyItem()
    {
        if (player.Money >= currentItemPrice)
        {
            player.Money -= currentItemPrice;
            currentItem.remaining += currentItemCount;
            onChangedRemaining?.Invoke(currentItem.remaining);
            ItemBuyPanel.SetActive(false);
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
        upgradeNowSellPrice.text = $"{currentItem.sellPrice}��";
        upgradeNowSatisfaction.text = $"{currentItem.satisfaction}";

        upgradeTooSellPrice.text = $"{currentItem.sellPrice + currentItem.sellPrice * currentItem.upgradeEfficiency}��";
        upgradeTooSatisfaction.text = $"{(int)(currentItem.satisfaction + currentItem.satisfaction * currentItem.upgradeEfficiency * 2)}";
        upgradePriceText.text = $"{currentItem.upgradePrice}��";
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

    //------------------�������� ����,�����г�
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

            furnitureLeftBarTexts[0].color = Color.cyan;
            furnitureLeftBarTexts[previousControllerNum].color = Color.black;
        }
        FurnitureiPanelController = 1;
        previousControllerNum = 0;
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

    }

    //------------------������������, �Ʒ��� �г�
    private void FirstButtonOnFurniturePanel()
    {

        switch (FurnitureiPanelController)
        {
            case 1:
                storeColor.material = colorMaterials[0];
                storeDoorColor.material = colorMaterials[0];
                CounterColor.material = colorMaterials[5];
                break;
            case 2: break;
            case 3: break;
        }
    }
    private void SecondButtonOnFurniturePanel()
    {
        switch (FurnitureiPanelController)
        {
            case 1:
                storeColor.material = colorMaterials[1];
                storeDoorColor.material = colorMaterials[1];
                CounterColor.material = colorMaterials[0];
                break;
            case 2: break;
            case 3: break;
        }
    }
    private void ThirdButtonOnFurniturePanel()
    {
        switch (FurnitureiPanelController)
        {
            case 1:
                storeColor.material = colorMaterials[2];
                storeDoorColor.material = colorMaterials[2];
                CounterColor.material = colorMaterials[6];
                break;
            case 2: break;
            case 3: break;
        }
    }
    private void FourthButtonOnFurniturePanel()
    {
        switch (FurnitureiPanelController)
        {
            case 1:
                storeColor.material = colorMaterials[3];
                storeDoorColor.material = colorMaterials[3];
                CounterColor.material = colorMaterials[2];
                break;
            case 2: break;
            case 3: break;
        }
    }
    private void FifthButtonOnFurniturePanel()
    {
        switch (FurnitureiPanelController)
        {
            case 1:
                storeColor.material = colorMaterials[4];
                storeDoorColor.material = colorMaterials[4];
                CounterColor.material = colorMaterials[5];
                break;
            case 2: break;
            case 3: break;
        }
    }

}
