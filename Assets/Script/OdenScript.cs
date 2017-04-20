using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class OdenScript : MonoBehaviour {

    public int rotedPrice;
    public int normalPrice;
    public Text ExpireTimerTxt;
    public GameObject BubbleBGImage;

    [Header("Bubble Bg")]
    public List<Sprite> BubbleBg = new List<Sprite>();

    int _salePrice;

    [Header("Oden expire time")]
    public int expireTimeSecond;
    public int expireTimeMinute;
    public int expireTimeHour;

    [Header("Oden cooking time")]
    public int cookingTimeSecond;
    public int cookingTimeMinute;
    public int cookingTimeHour;

    [SerializeField]
    public bool isRoted = false;
    public bool isCooking= true;
    public bool isDone = false;

    float timeLeft = 0.0f;
    float rotTime;
    float cookingTime;
    string displayStatus;

    private int _counterValue, _pauseCounter, _focusCounter;
    private DateTime _lastMinimize;
    private double _minimizedSeconds;

    [Header("Specific Info")]
    private DateTime createTime;
    public DateTime TimeStamp;
    public string SlotPositionId;
    public string OdenKeyName;

    void OnApplicationPause(bool isGamePause)
    {
        if (isGamePause)
        {
            _pauseCounter++;

            GoToMinimize();
        }
    }

    void OnApplicationFocus(bool isGameFocus)
    {
        if (isGameFocus)
        {
            _focusCounter++;
            GoToMaximize();
        }
    }

    void Start()
    {
        TimeStamp = DateTime.Now;

        rotTime = calculateTimeUsage(expireTimeSecond, expireTimeMinute, expireTimeHour);
        cookingTime = calculateTimeUsage(cookingTimeSecond, cookingTimeMinute, cookingTimeHour);
        timeLeft = rotTime;

        if(createTime != DateTime.MinValue)
        {
            int boiledTimeSeconds = (Int32)(TimeStamp - createTime).TotalSeconds;
            timeLeft -= boiledTimeSeconds;
        }

        GenerateSpecificInfo();
        UpdateBoiledOdenInfoToModel();

        StartCoroutine("StartCounter");
        Application.runInBackground = true;
    }

    IEnumerator StartCounter()
    {
        yield return new WaitForSeconds(1f);
        _counterValue++;
        timeLeft--;
        StartCoroutine("StartCounter");
    }

    public void GoToMinimize()
    {
        _lastMinimize = DateTime.Now;
    }

    public void GoToMaximize()
    {
        _minimizedSeconds = (DateTime.Now - _lastMinimize).TotalSeconds;
        _counterValue += (Int32)_minimizedSeconds;
        timeLeft -= (Int32)_minimizedSeconds;
    }

    void Update()
    {
        
        OnUpdateOdenBoiledTimer();

        if (timeLeft >= cookingTime)
        {
            updatedOdenState("COOKING");
        }
        else if (timeLeft <= cookingTime && timeLeft >= 0)
        {
            updatedOdenState("DONE");
            UpdateSalePrice("DONE");
            SetBubbleBg("DONE");
        }
        else
        {
            updatedOdenState("ROTED");
            UpdateSalePrice("ROTED");
            SetBubbleBg("ROTED");
        }
    }

    void OnUpdateOdenBoiledTimer()
    {
        if (isCooking)
        {
            float remainTime = (timeLeft - cookingTime);
            displayStatus = FormatOdenTimer((int)remainTime);
        }
        else
        {
            displayStatus = string.Empty;
        }

        ExpireTimerTxt.text = displayStatus.ToString();
    }

    int calculateTimeUsage(int second, int min, int hour )
    {
        int convertFromMin = min * 60;
        int convertFromHour = hour * 60 * 60;
        return second + convertFromMin + convertFromHour;
    }

    void updatedOdenState(string state)
    {
        switch (state)
        {
            case "COOKING":
                isRoted = false;
                isCooking = true;
                isDone = false;
                break;
            case "DONE":
                isRoted = false;
                isCooking = false;
                isDone = true;
                break;
            case "ROTED":
                isRoted = true;
                isCooking = false;
                isDone = false;
                break;
            default:
                break;
        }
    }

    public int SalePrice
    {
        get
        {
            return _salePrice;
        }
    }

    void UpdateSalePrice(string state)
    {
        if(state == "ROTED")
        {
            _salePrice = rotedPrice;
        }
        else if(state == "DONE")
        {
            _salePrice = normalPrice;
        }
    }

    string FormatOdenTimer(int timeLeft)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeLeft);
        return time.ToString();
    }

    void SetBubbleBg(string state)
    {
        Sprite selectedBg;
        switch (state)
        {
            case "DONE":
                selectedBg = BubbleBg[0];
                break;
            case "ROTED":
                selectedBg = BubbleBg[1];
                break;
            default:
                selectedBg = BubbleBg[3];
                break;
        }

        BubbleBGImage.GetComponent<SpriteRenderer>().sprite = selectedBg;
    }

    void GenerateSpecificInfo()
    {
        OdenKeyName = this.gameObject.name;
        SlotPositionId = gameObject.transform.parent.name;
    }

    public void ReGenerateSpecificInfo(string slotId, DateTime createDateTime)
    {
        createTime = createDateTime;;

        SlotPositionId = slotId;
        OdenKeyName = this.gameObject.name;
    }

    void UpdateBoiledOdenInfoToModel()
    {
        OdenListModel.UpdateBoiledOdenInfoModel(SlotPositionId, OdenKeyName, TimeStamp);
    }
}
