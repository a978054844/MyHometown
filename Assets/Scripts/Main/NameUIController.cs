using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KBEngine;

public class NameUIController : MonoBehaviour {

    public GameObject CreateNameGO;

    private void Start()
    {
        //注册命名回调事件
        KBEngine.Event.registerOut("reqChangeNameCall", this, "Event_reqChangeNameCall");
        Account account = KBEngineApp.app.player() as Account;
        if (account.Name.Length != 0)
        {
            OriginalSetting(account);
            CreateNameGO.SetActive(false);
        }

    }

    private void OriginalSetting(Account account) {
        MainInfoData.GetInstance().Name = account.Name;
        MainInfoData.GetInstance().HeadIcon = null;
        MainInfoData.GetInstance().UpdateOnceData();
    }

    public void Event_reqChangeNameCall(string callStr) {
        string tips;
        if (callStr == "success")
        {
            tips = "命名成功";
            Debug.Log(tips);
            TipsOperation.ShowTips(tips);
            MainInfoData.GetInstance().Name = (KBEngineApp.app.player() as Account).Name;
            MainInfoData.GetInstance().UpdateOnceData();

            CreateNameGO.SetActive(false);
        }
        else if (callStr == "repetition")
        {
            tips = "该名字已拥有, 请重新输入";
            TipsOperation.ShowTips(tips);
            Debug.Log(tips);
        }
        else
        {
            tips = "未知错误 : " + callStr;
            TipsOperation.ShowTips(tips);
            Debug.LogError(tips);
        }

    }

    public void SetPlayerName(InputField inputField) {

        if (inputField.text == "")
        {
            Debug.LogWarning("名字不能为空");
            return;
        }
        Account account = KBEngineApp.app.player() as Account;
        account.baseCall("reqChangeName", inputField.text);
    }

    private void OnDestroy()
    {
        //解除所有注册out事件
        KBEngine.Event.deregisterOut(this);
    }
}
