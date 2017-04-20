using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class InitialFBscript : MonoBehaviour {
    public Image FBProfilePicture;
    public GameObject LogoutBtn;

    public Sprite SpriteLogOutBtn;
    public Sprite FBProfileDefault;
    /// Include Facebook namespace
    // Awake function from Unity's MonoBehavior
    void Awake()
        {
            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            }
            else {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();    
            }
             
        }

        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                // Signal an app activation App Event
                FB.ActivateApp();
            // Continue with Facebook SDK
            // ...

                if (FB.IsLoggedIn)
                {
                    FB.API("me/picture?type=square&height=128&width=128", HttpMethod.GET, GetProfilePicture);
                    LogoutBtn.GetComponent<Image>().sprite = SpriteLogOutBtn;
                }
            }
            else {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }

        void GetProfilePicture(IGraphResult result)
        {
            if (result.Texture != null)
            {
                FBProfilePicture.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
            }
            else
            {
                FBProfilePicture.sprite = FBProfileDefault;
            }

        }

}
