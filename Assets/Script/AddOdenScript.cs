using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AddOdenScript : MonoBehaviour {

    public AudioSource toggleSound;

    int screenH = Screen.height;
    int screenW = Screen.width;
    bool isAddOdenListHide = true;

    public GameObject OdenListObj;
    public GameObject AddOdenPanel;
    RectTransform _addOdenBtnPos;
    
    // Use this for initialization
    void Start () {
        OdenListObj.SetActive(false);
        hideAddOdenListBtn();
    }
	
    void showAddOdenBtnPos()
    {
        _addOdenBtnPos = AddOdenPanel.GetComponent<RectTransform>();
        _addOdenBtnPos.anchorMin = new Vector2(0, 0.85f);
        _addOdenBtnPos.anchorMax = new Vector2(1, 1);
        _addOdenBtnPos.pivot = new Vector2(0.5f, 1);
        _addOdenBtnPos.anchoredPosition = new Vector2(0, 0);

    }

    void hideAddOdenListBtn()
    {
        _addOdenBtnPos = AddOdenPanel.GetComponent<RectTransform>();
        _addOdenBtnPos.anchorMax = new Vector2(1, 0.15f);
        _addOdenBtnPos.anchorMin = new Vector2(0, 0);
        _addOdenBtnPos.pivot = new Vector2(1, 0);
        _addOdenBtnPos.anchoredPosition = new Vector2(0, 0);
    }

    public void toggleShowHideOdenBtn()
    {
        toggleSound.Play();
        if (isAddOdenListHide)
        {
            showAddOdenBtnPos();
            OdenListObj.SetActive(true);
            isAddOdenListHide = false;
        }
        else
        {
            hideAddOdenListBtn();
            OdenListObj.SetActive(false);
            isAddOdenListHide = true;
        }
        
    }
}
