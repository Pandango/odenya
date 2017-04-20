using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdsRewardedBtn : MonoBehaviour {
    public AudioSource clickSound;

    public int AdsReloadCoundown;
    public string zoneId;
    public PlayerResourceManager playerResourceManager;

    [Header("Visible/Invisible Btn Setter")]
    public Sprite spriteVisibleBtn;
    public Sprite spriteInVisibleBtn;

    [Header("Congrants Dialog Setter")]
    public GameObject CongrantsDialog;
    public Text ItemName;
    public Image Itempic;
    public Sprite coinPic;

    GameBuffModel gameBuffModel = new GameBuffModel();
    public int[] moneyRewardRate;

    int _timeCounter = 0, _focusCounter = 0;
    DateTime _lastMinimize;
    double minimizedSeconds;

    void Start()
    {
        Application.runInBackground = true;

    }

    IEnumerator StartCounter()
    {
        yield return new WaitForSeconds(1f);   
        if(_timeCounter <= AdsReloadCoundown)
        {
            _timeCounter++;
            StartCoroutine("StartCounter");
        }
        else
        {
            IsBtnVisible(true);
        }      
    }

    void OnApplicationPause(bool isGamePuase)
    {
        if (isGamePuase)
        {
            _lastMinimize = DateTime.Now;
           
        }
    }

    void OnApplicationFocus(bool isGameFocus)
    {
        if (isGameFocus)
        {
            _focusCounter++;

            if(_focusCounter >= 2)
            {
                minimizedSeconds = (DateTime.Now - _lastMinimize).TotalSeconds;
             
                _timeCounter += (Int32)minimizedSeconds;
            }
        }
    }


    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                getRandomReward();
                clickSound.Play();
                break;
            case ShowResult.Skipped:
                Debug.LogWarning("Video was shipped");
                break;
            case ShowResult.Failed:
                Debug.LogError("Videl fail to show");
                break;
        }
    }

    public void getRandomReward()
    {
        string selectedReward = gameBuffModel.getRandomBuff();
        //if (selectedReward == "EXTRA_MONEY")
        //{
        int rate = UnityEngine.Random.Range(0, moneyRewardRate.Length);
        int moneyReward = moneyRewardRate[rate];
        playerResourceManager.UpdateTotalCoin(moneyReward);

        CongrantsDialog.SetActive(true);
        ItemName.text = moneyReward + " Coins";
        Itempic.sprite = coinPic;

        //}
        IsBtnVisible(false);

        SetReloadAdsBtn();
    }
    
    public void ShowAds()
    {
        
        if (string.IsNullOrEmpty(zoneId)) zoneId = null;

        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show(zoneId, options);
        
    }

    void SetReloadAdsBtn()
    {
        print("Text");
        _timeCounter = 0;
        StartCoroutine("StartCounter");    
    }

    void IsBtnVisible(bool isVisible)
    {
        if (isVisible)
        {
            this.gameObject.GetComponent<Image>().sprite = spriteVisibleBtn;
            this.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = spriteInVisibleBtn;
            this.gameObject.GetComponent<Button>().interactable = false;
        }
    }
}
