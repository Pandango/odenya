using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;

public class InviteFriendFBScript : MonoBehaviour {

	//private const string IOS_URL = "https://itunes.apple.com/us/app/goat-strike/id1112188421?ls=1&mt=8";
	//private const string ANDROID_URL = "https://play.google.com/store/apps/details?id=th.ac.kmutt.media.mdt.goatstrike";
	private const string IOS_URL = "https://fb.me/1837242233208216";
	private const string ANDROID_URL = "https://fb.me/1837242233208216";
	private const string IMAGE_FOR_FB = "http://www.imi.co.th/apps/goatstrike/GoatStrikeFB.jpg";

	public void CallInviteFriend()
	{		
		if (FB.IsLoggedIn) {
			#if UNITY_IOS 
			FB.Mobile.AppInvite(new Uri(IOS_URL), new Uri(IMAGE_FOR_FB), this.HandleResult);
			#endif

			#if UNITY_ANDROID
			FB.Mobile.AppInvite(new Uri(ANDROID_URL), new Uri(IMAGE_FOR_FB), this.HandleResult);
			#endif
		} else {
			Debug.Log("Need to login first");
		}
	}

	protected void HandleResult(IResult result)
	{
		if (result == null){
			Debug.Log("Null response");
			return;
		}
		// Some platforms return the empty string instead of null.
		if (!string.IsNullOrEmpty(result.Error)){
			Debug.Log("Error response");
		}
		else if (result.Cancelled){
			Debug.Log("User cancelled");
		}
		else if (!string.IsNullOrEmpty(result.RawResult)){
			Debug.Log("Successfully invited friends");
		}
		else{
			Debug.Log("Empty Response");
		}
	}
		
}
