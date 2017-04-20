using System;
using System.Collections;
using System.Collections.Generic;

public class OdenListModel {

    Dictionary<string, int> _unitOfSelectedItems = new Dictionary<string, int>();
    Dictionary<string, int> _purchasePriceOfSelectedItems = new Dictionary<string, int>();

    //Dictionary<int, Dictionary<string, int>> _selectedItemsWithIndex = new Dictionary<int, Dictionary<string, int>>();

    public void UpdateSpecificUnitInSelectedItem(string itemName, int unit)
    {
        _unitOfSelectedItems[itemName] = unit;     
    }
    
    public Dictionary<string, int> getSelectedItems()
    {
        return _unitOfSelectedItems;
    }

    public void resetUnitOfSelectedItems()
    {
        _unitOfSelectedItems.Clear();
    }

    public void resetPurchasePriceOfSelectedItems()
    {
        _purchasePriceOfSelectedItems.Clear();
    }

    public int getUnitOfItems()
    {
        int totalUnit = 0;
        foreach(KeyValuePair<string, int> pair in _unitOfSelectedItems)
        {
            totalUnit += pair.Value;
        }
        return totalUnit;
    }

    public void CalculateTotalPurchase(string itemName, int unit, int price)
    {
        _purchasePriceOfSelectedItems[itemName] = unit * price;
    }

    public int getTotalPurchasePrice()
    {
        int total = 0;
        foreach (KeyValuePair<string, int> pair in _purchasePriceOfSelectedItems)
        {
            total += pair.Value;
        }
        return total;
    }


    ///////////////////////Oden In pot location model//////////////

    static Dictionary<string, OdenTimeStampInfo> _BoiledOdenInfoModel = new Dictionary<string, OdenTimeStampInfo>();

    public static void UpdateBoiledOdenInfoModel(string slotId, string odenKeyname, DateTime timeStamp)
    {
        _BoiledOdenInfoModel[slotId] = new OdenTimeStampInfo { OdenName = odenKeyname, TimeStamp = timeStamp };
        SavePlayerDataManager.SaveBoiledInfoCollection();
    }

    public static void RemoveEmptyBoiledOdenInfoModel(string slotId)
    {
        _BoiledOdenInfoModel.Remove(slotId);
        SavePlayerDataManager.SaveBoiledInfoCollection();
    }

    public static Dictionary<string, OdenTimeStampInfo> GetBoiledOdenInfoModel
    {
        get { return _BoiledOdenInfoModel; }
    }

    public static void ResetBoilOdenInfoModel()
    {
        _BoiledOdenInfoModel.Clear();
    }
}

public class OdenTimeStampInfo
{
    public string OdenName { get; set; }
    public DateTime TimeStamp { get; set; }
}