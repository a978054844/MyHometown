using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;
using UnityEngine.UI;
using System;

public class FriendUIController : MonoBehaviour {

    //好友请求通知物体
    public GameObject addFriendMessGO;
    //好友列表物体
    public Transform friendListTr;
    //聊天物体
    public GameObject chattingGO;

    uint needId;

    ulong cur_chattingDBID;

    //按下的好友cell
    Transform pressTr;

	void Start () {
        
        KBEngine.Event.registerOut("reqAddFriendMessage", this, "Event_reqAddFriendMessage");
        KBEngine.Event.registerOut("reqUpdateFriendListUI", this, "Event_reqUpdateFriendListUI");
        KBEngine.Event.registerOut("reqUpdateFriendChatting", this, "Event_reqUpdateFriendChatting");

        (KBEngineApp.app.player() as Account).baseCall("mainSceneLoaded");
        //Invoke("UpdateFriendUI", 0.2f);
    }

    #region 服务器回调事件

    public void Event_reqAddFriendMessage(uint id, string needName)
    {
        needId = id;
        addFriendMessGO.transform.Find("Message").GetComponent<Text>().text = "玩家  " + needName + "  申请添加好友";
        addFriendMessGO.SetActive(true);
    }

    public void Event_reqUpdateFriendListUI() {
        UpdateFriendUI();
    }

    public void Event_reqUpdateFriendChatting(ulong dbid, string message) {
        Debug.Log("新接收的消息来自 : " + dbid + "  内容为 : " + message);
        List<string> list = MainInfoData.GetInstance().GetAllRecordsWithDBID(dbid);

        string mess_name = MainInfoData.GetInstance().GetChattingNameToDBID(dbid);
        message = mess_name + ": " + message;
        list.Add(message);

        MainInfoData.GetInstance().SetAllRecordsWithDBID(dbid, list);

        //来消息提示
        if (cur_chattingDBID != dbid)
        {
            foreach (Transform child in friendListTr)
                if (child.Find("Name").GetComponent<Text>().text == mess_name)
                {
                    child.Find("ChattingTips").gameObject.SetActive(true);
                    break;
                }
        }else
            UpdateChattingRecords(dbid);

    }

    #endregion

    //刷新好友列表
    void UpdateFriendUI()
    {
        Account account = KBEngineApp.app.player() as Account;

        Transform originalFriendContent = friendListTr.Find("Example");
        FRIEND_INFO friend_info;
        int listCount = account.Friend_list.Count;
        int onlineNum = 1;
        for (int i = 0; i < listCount; i++)
        {
            friend_info = account.Friend_list[i];
            Transform needTr = friendListTr.Find(i + "");
            if (needTr)
            {
                needTr.Find("Name").GetComponent<Text>().text = friend_info.name;
                needTr.Find("Level").GetComponent<Text>().text = friend_info.level + "";
                if (!needTr.gameObject.activeSelf)
                    needTr.gameObject.SetActive(true);
                //在线
                if (Convert.ToBoolean(friend_info.status))
                {
                    needTr.SetSiblingIndex(onlineNum++);
                    needTr.GetComponent<Image>().color = Color.white;
                    needTr.GetComponent<Image>().raycastTarget = true;
                }
                else
                {
                    needTr.GetComponent<Image>().color = Color.gray;
                    needTr.GetComponent<Image>().raycastTarget = false;
                }
            }
            else
            {
                needTr = Instantiate(originalFriendContent);
                needTr.SetParent(friendListTr);
                needTr.name = i.ToString();
                needTr.Find("Name").GetComponent<Text>().text = friend_info.name;
                needTr.Find("Level").GetComponent<Text>().text = friend_info.level + "";
                needTr.gameObject.SetActive(true);
                //在线
                if (Convert.ToBoolean(friend_info.status))
                {
                    needTr.SetSiblingIndex(onlineNum++);
                    needTr.GetComponent<Image>().color = Color.white;
                    needTr.GetComponent<Image>().raycastTarget = true;
                }
                else
                {
                    needTr.GetComponent<Image>().color = Color.gray;
                    needTr.GetComponent<Image>().raycastTarget = false;
                }
            }
        }

        //隐藏剩余好友框
        int num = friendListTr.childCount - listCount - 1;
        for (int i = 0; i < num; i++)
            friendListTr.transform.Find((i + listCount).ToString()).gameObject.SetActive(false);

    }

    //刷新聊天记录
    void UpdateChattingRecords(ulong dbid) {
        Transform content = chattingGO.transform.Find("ChattingRecords/Viewport/Content");
        Transform childExample = content.Find("Example");
        List<string> list = MainInfoData.GetInstance().GetAllRecordsWithDBID(dbid);
        int listCount = list.Count;

        for (int i = 0; i < listCount; i++)
        {
            Transform needTr = content.Find(i + "");
            Color color;
            if (list[i].Split(':')[0] == "自己")
                color = Color.red;
            else
                color = Color.white;

            if (needTr)
            {
                needTr.Find("Text").GetComponent<Text>().text = list[i];
                if (!needTr.gameObject.activeSelf)
                    needTr.gameObject.SetActive(true);
            }
            else
            {
                needTr = Instantiate(childExample);
                needTr.SetParent(content);
                needTr.name = i.ToString();
                needTr.Find("Text").GetComponent<Text>().text = list[i];
                needTr.gameObject.SetActive(true);
            }
            
            needTr.GetComponent<Image>().color = color;

            //聊天框自适应文字
            needTr.Find("Text").GetComponent<ContentSizeFitter>().SetLayoutVertical();
            Vector2 size = needTr.transform.GetComponent<RectTransform>().sizeDelta;
            size.y = needTr.Find("Text").GetComponent<RectTransform>().sizeDelta.y + 10;
            needTr.transform.GetComponent<RectTransform>().sizeDelta = size;

        }

        //隐藏剩余聊天框
        int num = content.childCount - listCount - 1;
        for (int i = 0; i < num; i++)
            content.Find((i + listCount).ToString()).gameObject.SetActive(false);

        //强制刷新UI属性
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        //每次设置最底部
        ScrollRect scrollRect = chattingGO.transform.Find("ChattingRecords").GetComponent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 0;
    }


    #region UI执行事件

    //添加好友按钮事件
    public void AddFriend(InputField inputField) {
        string addName = inputField.text;
        Account account = KBEngineApp.app.player() as Account;
        if (addName == account.Name)
        {
            TipsOperation.ShowTips("输入的是自己的名字, 请重新输入");
            return;
        }
        foreach (FRIEND_INFO friend_info in account.Friend_list)
            if (friend_info.name == addName) {
                TipsOperation.ShowTips("该好友已拥有, 请重新输入");
                return;
            }

        account.baseCall("addFriend", inputField.text);
        inputField.text = "";
    }

    //点击好友申请信息按钮事件
    public void ClickFriendMessBut(bool accept) {
        Account account = KBEngineApp.app.player() as Account;
        account.baseCall("reqAddFriendMessageCall", needId, accept.ToString());
        addFriendMessGO.SetActive(false);
    }

    //点击好友单元
    public void ClickFriendCell(GameObject go) {
        Account account = KBEngineApp.app.player() as Account;
        ulong dbid = account.Friend_list[int.Parse(go.name)].dbid;

        cur_chattingDBID = dbid;
        string needName = account.Friend_list[int.Parse(go.name)].name;


        Debug.Log("选择玩家 : " + needName);
        MainInfoData.GetInstance().SetChattingNameToDBID(dbid, needName);
        go.transform.Find("ChattingTips").gameObject.SetActive(false);
        //加载所有记录
        if (!chattingGO.activeSelf)
            chattingGO.SetActive(true);
        UpdateChattingRecords(cur_chattingDBID);
    }

    //发送聊天信息给客户端
    public void SendChattingRecordsToFriend(InputField inputField) {
        string message = inputField.text;
        if (message == "")
        {
            Debug.Log("未输入内容");
            return;
        }

        List<string> list = MainInfoData.GetInstance().GetAllRecordsWithDBID(cur_chattingDBID);
        list.Add("自己: " + message);
        MainInfoData.GetInstance().SetAllRecordsWithDBID(cur_chattingDBID, list);
        Account account = KBEngineApp.app.player() as Account;
        account.baseCall("sendChattingMessage", cur_chattingDBID, message);

        inputField.text = "";
        UpdateChattingRecords(cur_chattingDBID);
    }

    //按下
    public void StartPress(LongPress longPress) {
        pressTr = longPress.transform;
        longPress.enabled = true;
    }

    public void ExitPress(LongPress longPress) {
        if (longPress.enabled == true)
            longPress.enabled = false;
    }

    public void ShowLongPressUI(GameObject longPressUI) {
        longPressUI.SetActive(true);
        Transform main = longPressUI.transform.Find("Main");
        main.transform.SetParent(pressTr);
        main.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -20, 0);
        main.SetParent(longPressUI.transform);
        //赋值好友列表序号
        KeepPressUIController.friendIndex = int.Parse(pressTr.name);
    }

    #endregion

    private void OnDestroy()
    {
        //解除所有注册out事件
        KBEngine.Event.deregisterOut(this);
    }
}
