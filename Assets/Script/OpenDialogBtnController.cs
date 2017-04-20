using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDialogBtnController : MonoBehaviour {
    public GameObject Dialog;

    public void OpenCongrantsDialog()
    {
        Dialog.SetActive(true);
    }
}
