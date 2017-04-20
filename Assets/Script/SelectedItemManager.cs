using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedItemManager : MonoBehaviour {
    public AudioSource clickSound;
    public AudioSource coinSound;

    const int EMPTY_ITEM = 0;
    const int EMPTY_COIN = 0;
    public OdenListModel odenListModel = new OdenListModel();
    public GameObject playerResourceManagerObj;

    [Header("Warning Dialog Setter")]
    public GameObject WarningDialog;
    public Text WarningContentTxt;

    PlayerResourceManager playerResourceManager;

    public Text totalPuchasePriceTxt;
    public Text totalUnitTxt;

    public Button AddToPot;
    public Button ClearSelectedItem;

    public GameObject[] itemsUnitTag;

    public void UpdateTotalUnitText()
    {
        int totalUnit = odenListModel.getUnitOfItems();
        totalUnitTxt.text = totalUnit.ToString();
    }

    public void UpdateTotalBuyPrice()
    {
        int purchasePrice = odenListModel.getTotalPurchasePrice();
        totalPuchasePriceTxt.text = purchasePrice.ToString();

    }
    public void ClearSelectedItems()
    {
        clickSound.Play();

        totalUnitTxt.text = EMPTY_ITEM.ToString();
        totalPuchasePriceTxt.text = EMPTY_COIN.ToString();

        itemsUnitTag = GameObject.FindGameObjectsWithTag("ItemUnit");
        for(int index = 0; index < itemsUnitTag.Length; index++)
        {
            itemsUnitTag[index].GetComponent<Text>().text = EMPTY_ITEM.ToString();
        }

        odenListModel.resetUnitOfSelectedItems();
        odenListModel.resetPurchasePriceOfSelectedItems();
    }

    public void OnAddItemsToPot()
    {
        clickSound.Play();
       
        //SelectetITems
        Dictionary<string, int> selectedItemDict =  odenListModel.getSelectedItems();
        int totalUnitOfSelectedItem = odenListModel.getUnitOfItems();
        int totalPurchasePrice = odenListModel.getTotalPurchasePrice();

        playerResourceManager = playerResourceManagerObj.GetComponent<PlayerResourceManager>();

        bool isSlotEnough = totalUnitOfSelectedItem <= PlayerDataModel.RemainPotSlot;
        bool isMoneyEnough = totalPurchasePrice <= PlayerDataModel.TotalCoin;

        if (isSlotEnough && isMoneyEnough)
        {
            coinSound.Play();
            playerResourceManager.UpdateItemsInPotSlot(selectedItemDict, totalUnitOfSelectedItem);
            playerResourceManager.UpdateTotalCoin(totalPurchasePrice * -1);
            ClearSelectedItems();
        }
        else
        {
            if (!isMoneyEnough)
            {
                WarningContentTxt.text = "Your coin is not enough";
            }
            else
            {
                WarningContentTxt.text = "Slot id not enough";
            }

            WarningDialog.SetActive(true);
        }

        
    }

    public bool IsSelectedOdenOverThanUserRemainPotSlot()
    {
        return true;
    }
}
