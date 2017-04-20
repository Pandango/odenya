using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceManager : MonoBehaviour {

    public OdenItemDatabase odenItemDatabase;
    public SelectedItemManager selectedItemManager;
    OdenListModel odenListModel;

    public Text totalCoinTxt;
    public Text slotStatusTxt;
    public Text playerName;

    public GameObject[] SlotTemps;
    GameObject SelectedSlotTemp;

    public GameObject[] MenuCards;
    GameObject SelectedMenuCard;

    //Dictionary<int, Vector3> ChildrenSpawnPos = new Dictionary<int, Vector3>();
    [SerializeField]
    Vector3[] ChildrenSpawnPos;

    Transform[] ChildrenSpawnTrans;
    [SerializeField]
    Transform[] children;


    // Use this for initialization
    void Start()
    {
        InitailizeModel();
        InitializeModelFromSavedCollection();
        LoadUnlockMenusDataFromSavedCollection();

        CreatePotPosition();

        AddItemFromLoadCollectionToSpecificSlot();
    }

    void InitailizeModel()
    {
        odenListModel = selectedItemManager.odenListModel;
    }

    void InitializeModelFromSavedCollection()
    {
        SavePlayerDataManager.LoadPlayerDataCollection();

        playerName.text = PlayerDataModel.playerName;
        totalCoinTxt.text = PlayerDataModel.TotalCoin.ToString();

        string slotTextFormat = PlayerDataModel.UsedPotSlot.ToString() + "/" + PlayerDataModel.TotalPotSlot.ToString();
        slotStatusTxt.text = slotTextFormat;    
    }

    public void CreatePotPosition()
    {
        int userMaxSlot = PlayerDataModel.TotalPotSlot;
        int slotTempId = (userMaxSlot / 3) - 1; //get Id of Selected Slottemp that store in SlotTemps 

        SelectedSlotTemp = GameObject.Instantiate(getSlotTemp(slotTempId));
        SelectedSlotTemp.name = getSlotTemp(slotTempId).name;
        SelectedSlotTemp.transform.parent = GameObject.Find("PotSlotSpawn").transform;

        CreateSpawnPos();
    }

    void CreateSpawnPos()
    {
        ChildrenSpawnTrans = GameObject.FindGameObjectWithTag("Slot").transform.GetComponentsInChildren<Transform>(true);

        int dataSize = Convert.ToInt32(PlayerDataModel.TotalPotSlot);
        ChildrenSpawnPos = new Vector3[dataSize];
        for (int index = 1; index < ChildrenSpawnTrans.Length; index++)
        {
            ChildrenSpawnPos[index - 1] = ChildrenSpawnTrans[index].position;
        }
        PlayerDataModel.spawnAddedOdenPositions = ChildrenSpawnPos; //add to playerDateModel
    }

    void DetroyPot()
    {
        GameObject PotSlotSpawnObj = GameObject.Find("PotSlotSpawn").transform.GetChild(0).gameObject;
        DestroyImmediate(PotSlotSpawnObj);
    }

    public void UpdateMaximumPotSlot()
    {
        DetroyPot();
        CreatePotPosition();
    }

    public void UpdateItemsInPotSlot(Dictionary<string,int> selectedItemDic, int totalSelectedItemsUnit )
    {
        UpdateSlotStatus(totalSelectedItemsUnit);
        addItemToEachSlot();
    }

    void addItemToEachSlot()
    {
        Dictionary<string, int> selectedItems = odenListModel.getSelectedItems();
 
        foreach(KeyValuePair<string, int> pair in selectedItems)
        {
            for(int unit = 1; unit <= pair.Value; unit++)
            {
                CreateOdenInAvailableSlot(pair.Key);
            }
        }
    }

    void AddItemFromLoadCollectionToSpecificSlot()
    {
        Dictionary<string, OdenTimeStampInfo> ItemsCollection = OdenListModel.GetBoiledOdenInfoModel;

        foreach (KeyValuePair<string, OdenTimeStampInfo> itemsInfo in ItemsCollection)
        {
            CreateOdenInSpecificSlot(itemsInfo.Value.OdenName, itemsInfo.Key, itemsInfo.Value.TimeStamp);
        }
    }

    public void deleteItemsFromSlot()
    {
        int updatedUsedPotSlot = - 1;
        UpdateSlotStatus(updatedUsedPotSlot);
    }

    void CreateOdenInAvailableSlot(string odenName)
    {
        GameObject selectedOdenPrefab;
        GameObject instantOdenObj;
        for (int slotIndex = 1; slotIndex <= PlayerDataModel.TotalPotSlot; slotIndex++)
        {
            if (IsSlotParentEmpty(getSlotId(slotIndex)))
            {
                selectedOdenPrefab = odenItemDatabase.getSelectedOdenPrefab(odenName);
                instantOdenObj = Instantiate(selectedOdenPrefab, ChildrenSpawnPos[slotIndex - 1], Quaternion.identity);
                instantOdenObj.name = selectedOdenPrefab.name;
                instantOdenObj.transform.parent = GameObject.Find(getSlotId(slotIndex)).transform;
                break;
            }
        }
    }

    void CreateOdenInSpecificSlot(string odenName, string slotId, DateTime createTime)
    {
        GameObject selectedOdenPrefab;
        GameObject instantOdenObj;

        int slotIndex = int.Parse(slotId.ToLower().Replace("slot", string.Empty));
        print("slotId:" + slotIndex);

        selectedOdenPrefab = odenItemDatabase.getSelectedOdenPrefab(odenName);
        instantOdenObj = Instantiate(selectedOdenPrefab, ChildrenSpawnPos[slotIndex - 1], Quaternion.identity);     
        instantOdenObj.name = selectedOdenPrefab.name;
        instantOdenObj.transform.parent = GameObject.Find(getSlotId(slotIndex)).transform;
        instantOdenObj.GetComponent<OdenScript>().ReGenerateSpecificInfo(slotId, createTime);
    }

    bool IsSlotParentEmpty(string slotId)
    {
        children = GameObject.Find(slotId).GetComponentsInChildren<Transform>();

        if(children.Length > 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void UpdateSlotStatus(int usedSlot)
    {
        //update to model

        PlayerDataModel.UsedPotSlot += usedSlot;

        string slotTextFormat = PlayerDataModel.UsedPotSlot.ToString() + "/" + PlayerDataModel.TotalPotSlot.ToString();
        slotStatusTxt.text = slotTextFormat;

        SavePlayerDataManager.SavePlayerDataCollection();
    }


    public void ResetSlotStatus()
    {
        //update to model

        PlayerDataModel.UsedPotSlot = 0;

        string slotTextFormat = PlayerDataModel.UsedPotSlot.ToString() + "/" + PlayerDataModel.TotalPotSlot.ToString();
        slotStatusTxt.text = slotTextFormat;

        SavePlayerDataManager.SavePlayerDataCollection();
    }

    public void UpdateTotalCoin(int coinIncome)
    {
        //update to model
        PlayerDataModel.TotalCoin += coinIncome;

        totalCoinTxt.text = PlayerDataModel.TotalCoin.ToString();
        SavePlayerDataManager.SavePlayerDataCollection();
    }

    GameObject getSlotTemp(int id)
    {
        if (id <= SlotTemps.Length)
        {
            return SlotTemps[id];
        }
        return SlotTemps[0]; //if index wrong will go to 3 slot
    }

    string getSlotId(int id)
    {
        return "Slot" + id;
    }


    void LoadUnlockMenusDataFromSavedCollection()
    {
        Dictionary<string, bool> unlockMenuList = UnlockedMenuModel.UnlockMenuList;

        foreach (GameObject card in MenuCards)
        {
            ItemBtnController cardController = card.GetComponent<ItemBtnController>();
            cardController.SetUnlockedMode(unlockMenuList[cardController.menuNameKey]);
        }
    }
}
