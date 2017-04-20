using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using Facebook.MiniJSON;
using UnityEngine.UI;

public class LogingFB : MonoBehaviour {
    public string lastResponse, fbname;
    public Image FBProfilePicture;
    public Text FBNameTxt;

    public Sprite FBProfileDefault;

    public Sprite SpriteLogOutBtn;
    public Sprite SpriteLogInBtn;

    [Header("Logout Dialog Setter")]
    public GameObject LogoutDialog;
    public Image FBPicInDialog;
    public Text FBnameInDialog;

    public AudioSource clickSound;

    public void CallFBlogin()
    {
        clickSound.Play();
        if (!FB.IsLoggedIn)
        {
             FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, AuthCallback);
        }
        else
        {
            FB.LogOut();

            LogoutDialog.SetActive(true);
            FBPicInDialog.sprite = FBProfilePicture.sprite;
            FBnameInDialog.text = PlayerDataModel.playerName;
        }
    }

    public void OnLogout()
    {
        clickSound.Play();

        FBNameTxt.text = "Unknow Oden";
        FBProfilePicture.sprite = FBProfileDefault;

        PlayerDataModel.playerName = string.Empty;
        this.gameObject.GetComponent<Image>().sprite = SpriteLogInBtn;

        LogoutDialog.SetActive(false);
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
            FB.API("/me?fields=first_name", HttpMethod.GET, LoginCallBack2);
            FB.API("me/picture?type=square&height=128&width=128", HttpMethod.GET, GetProfilePicture);

            //set to log out btn sprite
            this.gameObject.GetComponent<Image>().sprite = SpriteLogOutBtn;
        }
        else {
            Debug.Log("User cancelled login");
        }
    }

    void LoginCallBack2(IGraphResult result)
    {
        if (result.Error != null)
            lastResponse = "Error Response\n" + result.Error;
        else if (!FB.IsLoggedIn)
            lastResponse = "Login cancelled by player";
        else
        {
            IDictionary dict = Json.Deserialize(result.RawResult) as IDictionary;
            fbname = dict["first_name"].ToString();

            SetFBPlayerName(fbname);
        }
    }

    void GetProfilePicture(IGraphResult result)
    {
        if(result.Texture != null)
        {
            FBProfilePicture.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
        }
        else
        {
            FBProfilePicture.sprite = FBProfileDefault;
        }
       
    }

    void SetFBPlayerName(string fBname)
    {
        FBNameTxt.text = fbname;
        PlayerDataModel.playerName = fbname;
        SavePlayerDataManager.SavePlayerDataCollection();
    }
}
