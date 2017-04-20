using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlotBtnController : MonoBehaviour {
    public AudioSource clickSound;
    public AudioSource coinSound;

    public List<int> moneyRateForSlotUpgraded = new List<int>();
    public List<Sprite> itemsPicList = new List<Sprite>();

    public PlayerResourceManager playerResourceManager;

    public GameObject UpgradeDialog;
    public Text UpgradeCostTxt;
    public Text RemainCoinTxt;

    [Header("Content Panel")]
    public GameObject MaximumContentPanel;
    public GameObject UpgradeContentPanel;

    [Header("Warning Dialog")]
    public GameObject WarningDialog;
    public Text WarningTxt;

    [Header("Congrants Dialog")]
    public GameObject CongrantsDialog;
    public Text ItemName;
    public Image ItemPic;

    int maxPotLevel;
    int upgradeCost = 0;

    const string WARNING_MONEY_NOT_ENOUGH = "Your money is not enough.";
    const int INCREASE_SLOT_RATE = 3;

    void Start()
    {
        maxPotLevel = moneyRateForSlotUpgraded.Count;
    }

    public void OpenDialog()
    {
        clickSound.Play();
        UpdateDialogContent();
        OpenUpgradeDialog();
    }

    public void CloseDialog()
    {
        ResetValue();
        UpgradeDialog.SetActive(false);
    }

    public void UpdateRemainMoneyAfterUpgraded()
    {
        
        int usedCoin = upgradeCost;
        UpdateTotalCoin(usedCoin);
       
    }

    void UpdateTotalCoin(int usedCoin)
    {
        //update to model
        if(PlayerDataModel.TotalCoin >= usedCoin)
        {
            coinSound.Play();
            int coinIncome = usedCoin * -1;
            playerResourceManager.UpdateTotalCoin(coinIncome);
            CloseDialog();           

            UpdatedPotSlotInDataModel();
            OpenCongrantDialogContent();
            playerResourceManager.UpdateMaximumPotSlot();
        }
        else
        {
            CloseDialog();
            WarningDialog.SetActive(true);
            WarningTxt.text = WARNING_MONEY_NOT_ENOUGH;
        }
    }

    void UpdateDialogContent()
    {
        int currentPotLevel = CalculatePotLevel(PlayerDataModel.TotalPotSlot);

        if (currentPotLevel == maxPotLevel)
        {
            SetActiveUpgradableContentPanel(false);
        }
        else
        {
            SetActiveUpgradableContentPanel(true);
            upgradeCost = moneyRateForSlotUpgraded[currentPotLevel];
            UpgradeCostTxt.text = upgradeCost.ToString();
        }
    }
    
    void UpdatedPotSlotInDataModel()
    {
        PlayerDataModel.TotalPotSlot += INCREASE_SLOT_RATE;
        ResetPotSlot();
    }

    int CalculatePotLevel(int potslot)
    {
        if(potslot == 3)
        {
            return 0;
        }
        else if(potslot == 6)
        {
            return 1;
        }
        else if(potslot == 9)
        {
            return 2;
        }
        else
        {
            return maxPotLevel;
        }

    }

    string getPotslotName(int potslot)
    {
        if (potslot == 3)
        {
            return "Pot with 3 Slots";
        }
        else if (potslot == 6)
        {
            return "Pot with 6 Slots";
        }
        else
        {
            return "Pot with 9 Slots";
        }

    }
    void OpenUpgradeDialog()
    {
        UpgradeDialog.SetActive(true);
    }

    void SetActiveUpgradableContentPanel(bool setActive)
    {
        UpgradeContentPanel.SetActive(setActive);
        MaximumContentPanel.SetActive(!setActive);
    }

    void ResetValue()
    {
        upgradeCost = 0;
    }

    void OpenCongrantDialogContent()
    {
        //set image
        int selectedItemIndex = CalculatePotLevel(PlayerDataModel.TotalPotSlot);

        ItemPic.sprite = itemsPicList[selectedItemIndex];
        ItemName.text = getPotslotName(PlayerDataModel.TotalPotSlot);

        CongrantsDialog.SetActive(true);
    }

    void ResetPotSlot()
    {
        playerResourceManager.ResetSlotStatus();
        OdenListModel.ResetBoilOdenInfoModel();
        SavePlayerDataManager.SaveBoiledInfoCollection();
    }
}
