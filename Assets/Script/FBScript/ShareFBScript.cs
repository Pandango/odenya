using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Facebook.Unity;
using UnityEngine.UI;

public class ShareFBScript : MonoBehaviour {

	private const string IMAGE_FOR_FB = "http://imgur.com/a/coEQO";
	private const string LINK_FOR_FB =	"http://www.imi.co.th/apps/goatstrike/";

    string username;

    public void OnFBShared()
    {
        username = PlayerDataModel.playerName;

        if(username == string.Empty)
        {
            username = "My Oden cart";
        }

        string itemName = GameObject.Find("ItemNameTxt").GetComponent<Text>().text;

        string shareTitle = "Congrantulations! " + username + " has unlock new item";
        string description = username + "has unlocked " + itemName + ". Did you got this item?";

        Uri photo = new Uri(IMAGE_FOR_FB);
        FB.ShareLink(new Uri(LINK_FOR_FB), shareTitle, description, photo, callback: ShareCallback);
    }

    private void ShareCallback(IShareResult result)
    {
        if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink Error: " + result.Error);
        }
        else if (!String.IsNullOrEmpty(result.PostId))
        {
            Debug.Log(result.PostId);
        }
        else {
            Debug.Log("ShareLink success!");
        }
    }
}
