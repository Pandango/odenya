using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBtnController : MonoBehaviour {
    public AudioSource coinSound;

    [Header("Menu Information")]
    public string menuNameKey;
    public string menuName;
    public int purchasePrice;
    public int PurchaseMenuPrice;

    [Header("UI setter")]
    public bool isUnlock = true;
    public Sprite menuUnlockBG;
    public Sprite menuLockedBG;
    public Sprite odenUnlockPic;
    public Sprite odenLockPic;

    [Header("Component Setter")]
    public Image OdenImageArea;
    public Text MenuNameTxt;
    public Text PurchaseMenuPriceTxt;

    [Header("UnlockLocked Panel")]
    public GameObject LockedPanel;
    public GameObject UnLockPanel;

    [Header("Dependency Gameobject")]
    Text WarningContent;
    Text PurchaseDialogPriceTxt;
    Button PurchaseBtn;
    public GameObject PurchaseDialog;
    public GameObject WarningDialog;

    [Header("ETC")]
    [SerializeField]
    int maxUnit;
    int minUnit = 0;

    public GameObject unitTextObj;
    Text unitText;

    public GameObject selectedItemManagerPanel;
    SelectedItemManager selectedItemManager;
    PlayerResourceManager playerResourceManager;

    // Use this for initialization
    void Start () {
        playerResourceManager = GameObject.Find("PlayerResourceManager").GetComponent<PlayerResourceManager>();

        unitText = unitTextObj.GetComponent<Text>();
        selectedItemManager = selectedItemManagerPanel.GetComponent<SelectedItemManager>();

        SetMenuInfo();
        SetUnlockedMode(isUnlock);
    }

    public void DecreaseUnit()
    {
        int currentUnit = int.Parse(unitText.text);
        if(currentUnit > minUnit)
        {
            currentUnit--;
            unitText.text = currentUnit.ToString();
        }
        UpdateSelectedItemViewData(menuNameKey, currentUnit, purchasePrice);
        selectedItemManager.UpdateTotalUnitText();
        selectedItemManager.UpdateTotalBuyPrice();
    }

    public void IncreaseUnit()
    {
        int currentUnit = int.Parse(unitText.text);
        if (currentUnit < maxUnit)
        {
            currentUnit++;
            unitText.text = currentUnit.ToString();
        }
        UpdateSelectedItemViewData(menuNameKey, currentUnit, purchasePrice);
        selectedItemManager.UpdateTotalUnitText();
        selectedItemManager.UpdateTotalBuyPrice();
   
    }

    public void UpdateSelectedItemViewData(string itemName, int unit, int price)
    {
        selectedItemManager.odenListModel.UpdateSpecificUnitInSelectedItem(itemName, unit);
        selectedItemManager.odenListModel.CalculateTotalPurchase(itemName, unit, price);
    }

    public void updateUnitOfSelectedItem(string itemName, int unit)
    {
        selectedItemManager.odenListModel.UpdateSpecificUnitInSelectedItem(itemName, unit);
    }

    public void updatePurchasePriceOfSelectedItem(string itemName, int unit, int price)
    {
        selectedItemManager.odenListModel.CalculateTotalPurchase(itemName, unit, price);
    }

    void OnPurchaseMenu()
    {
        ClosePurchaseDialog();
        coinSound.Play();
        int income = PurchaseMenuPrice * -1;
        playerResourceManager.UpdateTotalCoin(income);
        SetUnlockedMode(true);

        Image ItemPicInDialog = GameObject.Find("ItemPic").GetComponent<Image>();
        ItemPicInDialog.sprite = odenUnlockPic;

        Text ItemNameTxt = GameObject.Find("ItemNameTxt").GetComponent<Text>();
        ItemNameTxt.text = menuName;

        UnlockedMenuModel.UpdateSpecificUnlockMenu(menuNameKey, true);
        SavePlayerDataManager.SavePlayerDataCollection();
    }

    void OnOpenWarningDialog()
    {
        WarningDialog.SetActive(true);

        SetupWarningDialog();
    }

    void SetupWarningDialog()
    {
        Text WarningContent = GameObject.Find("WarningContent").GetComponent<Text>();
        WarningContent.text = "Your coin is not enough";

    }

    public void OnOpenPurchaseDialog()
    {
        if (PlayerDataModel.TotalCoin >= PurchaseMenuPrice)
        {
            
            PurchaseDialog.SetActive(true);

            Text PurchaseDialogDescription = GameObject.Find("PurchaseDialogDescriptionTxt").GetComponent<Text>();
            PurchaseDialogDescription.text = "Do you want to purchase '" + menuName + "' ?";

            Text PurchaseDialogPriceTxt = GameObject.Find("PurchaseDialogPriceTxt").GetComponent<Text>();
            PurchaseDialogPriceTxt.text = PurchaseMenuPrice.ToString();

            GameObject PurchaseBtn = GameObject.Find("PurchaseBtn");
            PurchaseBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            PurchaseBtn.GetComponent<Button>().onClick.AddListener(OnPurchaseMenu);
        }
        else
        {
            OnOpenWarningDialog();
        }       
    }

    void ClosePurchaseDialog()
    {
        PurchaseDialog.SetActive(false);
    }
    //////////////////////////UI Setter////////////////////////////////////

    void SetMenuInfo()
    {
        MenuNameTxt.text = menuName;
        gameObject.name = menuNameKey;

        PurchaseMenuPriceTxt.text = PurchaseMenuPrice.ToString();
    }

    public void SetUnlockedMode(bool isUnlock)
    {
        UnLockPanel.SetActive(isUnlock);
        LockedPanel.SetActive(!isUnlock);

        UnlockedMenuModel.UpdateSpecificUnlockMenu(menuNameKey, isUnlock);

        if (isUnlock)
        {
            SetUnlockedUI();
        }
        else
        {
            SetLockedUI();
        }
    }

    void SetUnlockedUI()
    {
        Image menuBg = gameObject.GetComponent<Image>();

        OdenImageArea.sprite = odenUnlockPic;
        menuBg.sprite = menuUnlockBG;
    }

    void SetLockedUI()
    {
        Image menuBg = gameObject.GetComponent<Image>();

        OdenImageArea.sprite = odenLockPic;
        menuBg.sprite = menuLockedBG;
    }
}
