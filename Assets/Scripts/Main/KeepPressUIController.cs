using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;
using UnityEngine.UI;

public class KeepPressUIController : MonoBehaviour {

    
    public static int friendIndex;

    private void OnEnable()
    {
        Transform reqEnterTr = transform.Find("Main/ReqEnter");
        Transform loogInfo = transform.Find("Main/LookInfo");
        if (MainInfoData.GetInstance().InRoom)
        {
            reqEnterTr.GetComponent<Image>().color = Color.white;
            reqEnterTr.GetComponent<Button>().interactable = true;
        }
        else {
            reqEnterTr.GetComponent<Image>().color = Color.gray;
            reqEnterTr.GetComponent<Button>().interactable = false;
        }

    }

    public void AddFriendToMatchRoom() {
        Account account = KBEngineApp.app.player() as Account;
        account.baseCall("reqAddFriendToMatchRoom", account.Friend_list[friendIndex].dbid);
        LongPressCallBack(gameObject);
    }

    public void LookInfo() {
        TipsOperation.ShowTips("该功能等待实现哦");
        LongPressCallBack(gameObject);
    }

    //长按UI回调
    public void LongPressCallBack(GameObject longPressUI)
    {
        longPressUI.SetActive(false);
    }
}
