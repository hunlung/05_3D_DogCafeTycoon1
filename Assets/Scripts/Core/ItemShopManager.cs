using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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



        itemShopButtons[0].onClick.AddListener(GotoDessertPanel);
        itemShopButtons[1].onClick.AddListener(GotoDrinkPanel);
        itemShopButtons[2].onClick.AddListener(GotoGoodsPanel);
        itemShopButtons[3].onClick.AddListener(GotoMedicinePanel);
        itemShopButtons[4].onClick.AddListener(GotoNextDay);


        //for(int i = 0; i < ReturnButtons.Length; i++)
        //{
        //    ReturnButtons[i].onClick.AddListener(GotoItemShop);
        //}

        itemBuySellButtons[1].onClick.AddListener(CloseItemBuySellPanel);
    }

    private void OnEnable()
    {
        itemShopInput.Enable();
        //itemShopInput.ItemShop._1.performed += GotoDessertPanel;
    }

    private void OnDisable()
    {
        //itemShopInput.ItemShop._1.performed -= GotoDessertPanel;
        itemShopInput.Disable();
    }

    public void PrepareStore()
    {
        ItemShopPanel.SetActive(true);
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
    }

    //private void GotoItemShop(Button returnbutton)
    //{
    //    returnbutton.transform.parent.gameObject.SetActive(false);
    //    ItemShopPanel.SetActive(true);
    //}


    private void CloseItemBuySellPanel()
    {
        ItemBuySellPanel.SetActive(false);
    }




}
