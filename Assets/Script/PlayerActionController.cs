using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour {
    public AudioSource coinSound;

    const string ODEN_ITEM_TAG = "AddedOden";

    Vector3 touchPosWorld;
    TouchPhase touchPhase = TouchPhase.Ended;
    GameObject touchedObject;

    OdenScript odenScript;
    PlayerResourceManager playerResourceManager;

    void Start()
    {
        playerResourceManager = gameObject.GetComponent<PlayerResourceManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == touchPhase)
        {
            touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

            if (hitInformation.collider != null)
            {
                touchedObject = hitInformation.transform.gameObject;
                if (touchedObject.tag == ODEN_ITEM_TAG)
                {
                    InitializePalyerActionOnOdenItems(touchedObject);
                }
            }
            else
            {
                touchedObject = null;
            }

        }
    }

    void InitializePalyerActionOnOdenItems(GameObject targetObject)
    {
        saleOdenItem(targetObject);
        RemoveOdenItems(targetObject);
        UpdateBoiledOdenInfoToModel(targetObject);
    }

    void saleOdenItem(GameObject selectedOden)
    {
        odenScript = selectedOden.GetComponent<OdenScript>();
        bool isDone = odenScript.isDone;
        bool isRoted = odenScript.isRoted;

        if (isDone || isRoted)
        {
            //update coin from selling oden
            //update used slot
            coinSound.Play();
            playerResourceManager.deleteItemsFromSlot();
            playerResourceManager.UpdateTotalCoin(odenScript.SalePrice);
            Destroy(selectedOden);
        }
    }

    void RemoveOdenItems(GameObject selectedOden)
    {
        odenScript = selectedOden.GetComponent<OdenScript>();
        bool isCooking = odenScript.isCooking;

        if (isCooking)
        {
            playerResourceManager.deleteItemsFromSlot();
            Destroy(selectedOden);
        }
    }

    void UpdateBoiledOdenInfoToModel(GameObject selectedOden)
    {
        string slotId = selectedOden.GetComponent<OdenScript>().SlotPositionId;
        OdenListModel.RemoveEmptyBoiledOdenInfoModel(slotId);
        SavePlayerDataManager.SaveBoiledInfoCollection();

    }
}
