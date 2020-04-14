using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class LoginUIController : MonoBehaviour {

    public InputField UserNameInput;
    public InputField PasswordInput;

    private void Start()
    {
        KBEngine.Event.registerOut(EventOutTypes.onLoginFailed, this, "Event_onLoginFailed");     //登录失败事件
        KBEngine.Event.registerOut(EventOutTypes.onCreateAccountResult, this, "Event_onCreateAccountResult");//注册回调事件
        KBEngine.Event.registerOut("onLoginSuccessfully", this, "Event_onLoginSuccessfully");//登陆成功事件
    }

    //注册结果回调事件
    public void Event_onCreateAccountResult(UInt16 i, byte[] serverData) {
        TipsOperation.ShowTips(KBEngineApp.app.serverErr(i));
    }

    //登录失败回调事件
    public void Event_onLoginFailed(UInt16 i, byte[] serverData)
    {
        TipsOperation.ShowTips(string.Format("登录失败 : " + KBEngineApp.app.serverErr(i)));
    }

    //登陆成功回调事件
    public void Event_onLoginSuccessfully(UInt64 uuid, Int32 id, Account account)
    {
        Debug.Log("登录成功 || " + "uuid : " + uuid + "id : " + id + "account : " + account);
        //加载主场景
        SceneManager.LoadScene("MainScene");

    }

    //登录事件
    public void LoginEvent() {
        string userName = UserNameInput.text;
        string password = PasswordInput.text;
        if (CheckLoginMessage(userName, password))
            KBEngine.Event.fireIn(EventInTypes.login, userName, password, System.Text.Encoding.UTF8.GetBytes("PC1"));
            
    }

    //注册事件
    public void RegisterEvent() {
        string userName = UserNameInput.text;
        string password = PasswordInput.text;
        if (CheckLoginMessage(userName, password))
            KBEngine.Event.fireIn(EventInTypes.createAccount, userName, password, System.Text.Encoding.UTF8.GetBytes("PC1"));
    }

    bool CheckLoginMessage(string userName, string password) {
        return (CheckUserName(userName) && CheckPassword(password));
    }

    bool CheckUserName(string userName) {
        if (userName.Length > 0)
            return true;
        else
        {
            ShowErrorTips("账号为空");
            return false;
        }
    }

    bool CheckPassword(string password) {
        if (password.Length > 0)
            return true;
        else
        {
            ShowErrorTips("密码为空");
            return false;
        }
    }

    void ShowErrorTips(string errorMessage) {
        Debug.Log(errorMessage);
    }

    private void OnDestroy()
    {
        //解除所有注册out事件
        KBEngine.Event.deregisterOut(this);
    }
}
