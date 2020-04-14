using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KBEngine;

public class MainUIController : MonoBehaviour {

    public Text nameText;
    public Image headIcon;

    private void Awake()
    {
        MainInfoData.GetInstance().UpdateData += UpdateName;
        MainInfoData.GetInstance().UpdateData += UpdateHeadIcon;
    }


    public void UpdateName()
    {
        nameText.text = MainInfoData.GetInstance().Name;
    }

    public void UpdateHeadIcon()
    {
        headIcon.sprite = MainInfoData.GetInstance().HeadIcon;
    }

    private void OnDestroy()
    {
        MainInfoData.GetInstance().UpdateData -= UpdateName;
        MainInfoData.GetInstance().UpdateData -= UpdateHeadIcon;
    }
}

