using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SavePlayerDataManager : MonoBehaviour {

    public static void SavePlayerDataCollection()
    {
        string playerName = PlayerDataModel.playerName;
        int coin = PlayerDataModel.TotalCoin;
        int maximumPotSlot = PlayerDataModel.TotalPotSlot;
        int usedPotSlot = PlayerDataModel.UsedPotSlot;
      
        PlayerPrefs.SetString("PlayerNameSave", playerName);
        PlayerPrefs.SetInt("CoinSave", coin);
        PlayerPrefs.SetInt("MaxPotSlotSave", maximumPotSlot);
        PlayerPrefs.SetInt("UsedPotSlotSave", usedPotSlot);

        PlayerPrefs.SetString("UnlockMenusSave", GetMenuUnlockCollection());

        PlayerPrefs.Save();
    }

    public static void SaveBoiledInfoCollection()
    {
        PlayerPrefs.SetString("BoiledOdensInfoSave", GetBoiledInfoCollection());
        PlayerPrefs.Save();
    }

    public static void LoadPlayerDataCollection()
    {
       //PlayerPrefs.DeleteAll();

        if (PlayerPrefs.HasKey("PlayerNameSave") && PlayerPrefs.GetString("PlayerNameSave").Length > 0)
        {
            PlayerDataModel.playerName = PlayerPrefs.GetString("PlayerNameSave");
            PlayerDataModel.TotalCoin = PlayerPrefs.GetInt("CoinSave");
            PlayerDataModel.TotalPotSlot = PlayerPrefs.GetInt("MaxPotSlotSave");
            PlayerDataModel.UsedPotSlot = PlayerPrefs.GetInt("UsedPotSlotSave");
        }
        else
        {
            CreateNewDataCollection();
        }

        LoadUnlockMenuDataCollection();
        LoadBoiledOdenInfoCollection();

    }

    static void LoadBoiledOdenInfoCollection()
    {
        bool isBoiledOdensInfoEmpty = PlayerPrefs.GetString("BoiledOdensInfoSave").Length <= 0;
        if (!isBoiledOdensInfoEmpty)
        {
            ConvertBoiledOdensDataToStoreInModel(PlayerPrefs.GetString("BoiledOdensInfoSave"));
        }
    }

    static void LoadUnlockMenuDataCollection()
    {
        bool isUnlockMenusSaveEmpty = PlayerPrefs.GetString("UnlockMenusSave").Length <= 0;
        if (!isUnlockMenusSaveEmpty)
        {
            ConvertUnlockMenuDataToStoreInModel(PlayerPrefs.GetString("UnlockMenusSave"));
        }
    }

    static void CreateNewDataCollection()
    {
        PlayerDataModel.playerName = "Unknow Oden";
        PlayerDataModel.TotalCoin = 5000;
        PlayerDataModel.TotalPotSlot = 3;
        PlayerDataModel.UsedPotSlot = 0;


    }

    static string GetMenuUnlockCollection()
    {
        string unlockMenusSave = string.Empty;

        Dictionary<string, bool> unlockedMenuModel = UnlockedMenuModel.UnlockMenuList;

        foreach (KeyValuePair<string, bool> menu in unlockedMenuModel)
        {
            unlockMenusSave += string.Format("Menu:{0},IsUnlock:{1}|", menu.Key, menu.Value);
        }

        print("unlockcollection:" + unlockMenusSave);
        return unlockMenusSave;
    }

    static string GetBoiledInfoCollection()
    {
        string boiledOdensInfoSave = string.Empty;

        Dictionary<string, OdenTimeStampInfo> boiledOdendInfo = OdenListModel.GetBoiledOdenInfoModel;

        if(boiledOdendInfo.Count >= 0)
        {
            foreach (KeyValuePair<string, OdenTimeStampInfo> slotId in boiledOdendInfo)
            {
                boiledOdensInfoSave += string.Format("SlotId:{0},OdenName:{1},TimeStamp:{2}|", slotId.Key, slotId.Value.OdenName, slotId.Value.TimeStamp);
            }
        }

        print("boilcollection:" + boiledOdensInfoSave);
        return boiledOdensInfoSave;
    }

    static void ConvertUnlockMenuDataToStoreInModel(string stringData)
    {
        string[] unlockMenuList = stringData.Split('|');

        foreach (string menu in unlockMenuList)
        {
            if(menu != string.Empty)
            {
                string[] formatData = menu.Split(',');
                string odenName = formatData[0].Replace("Menu:", string.Empty);
                bool isUnlock = bool.Parse(formatData[1].Replace("IsUnlock:", string.Empty));

                UnlockedMenuModel.UpdateSpecificUnlockMenu(odenName, isUnlock);
            }         
        }
    }

    private static void ConvertBoiledOdensDataToStoreInModel(string stringData)
    {
        string[] boiledOdensDataList = stringData.Split('|');

        foreach (string menu in boiledOdensDataList)
        {
            if(menu.Length > 0)
            {
                string[] formatData = menu.Split(',');
                string slotId = formatData[0].Replace("SlotId:", string.Empty);
                string odenName = formatData[1].Replace("OdenName:", string.Empty);
                DateTime timeStamp = DateTime.Parse(formatData[2].Replace("TimeStamp:", string.Empty));

                OdenListModel.UpdateBoiledOdenInfoModel(slotId, odenName, timeStamp);
            }  
        }
    }

}
