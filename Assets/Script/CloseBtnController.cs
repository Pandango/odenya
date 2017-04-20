using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBtnController : MonoBehaviour {
    public GameObject Dialog;

	public void CloseDialog()
    {
        Dialog.SetActive(false);
    }
}
