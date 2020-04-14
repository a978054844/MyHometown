using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class RoomUIController : MonoBehaviour {

    public GameObject RoomGO;

    public GameObject chattingGO;

    public GameObject reqMessageGO;

    static MyList<string> roomChattingRecords;

    string oneChatting = null;

    uint reqID;

    string reqName;

    private void Start()
    {

        Init();
    }

    void Init() {

        KBEngine.Event.registerOut("reqEnterMatchRoomMessage", this, "Event_reqEnterMatchRoomMessage");
        KBEngine.Event.registerOut("reqUpdateMatchRoomChatting", this, "Event_reqUpdateMatchRoomChatting");
        KBEngine.Event.registerOut("reqUpdateMatchRoomUI", this, "Event_reqUpdateMatchRoomUI");
        KBEngine.Event.registerOut("reqShowMatching", this, "Event_reqShowMatching");
        KBEngine.Event.registerOut("enterBattleSpace", this, "Event_enterBattleSpace");
        

        roomChattingRecords = new MyList<string>();
        roomChattingRecords.AddEvent += AddOneChatting;
    }


    #region KBE_Event

    //请求加入房间信息
    public void Event_reqEnterMatchRoomMessage(uint arg1, string arg2)
    {
        //显示邀请信息
        reqID = arg1;
        reqName = arg2;

        reqMessageGO.transform.Find("Message").GetComponent<Text>().text = "玩家 " + arg2 + " 邀请你加入房间";
        reqMessageGO.SetActive(true);
    }

    //更新匹配房间聊天
    public void Event_reqUpdateMatchRoomChatting(string arg1, string arg2)
    {
        oneChatting = arg1 + " : " + arg2;
        Debug.Log("添加房间聊天信息 | " + oneChatting);
        roomChattingRecords.Add(oneChatting);

    }

    //更新匹配房间人物UI信息
    public void Event_reqUpdateMatchRoomUI(FRIEND_INFO_LIST friend_info_list)
    {
        Debug.Log("更新房间UI | 人数 : " + friend_info_list.Count);
        if (friend_info_list.Count == 0)
        {
            RoomGO.SetActive(false);
            return;
        }
        if (!RoomGO.activeSelf)
            RoomGO.SetActive(true);

        Account account = KBEngineApp.app.player() as Account;
        Transform playerTr;
        for (int i = 0; i < 5; i++)
        {
            playerTr = RoomGO.transform.Find(i.ToString());
            if (i < friend_info_list.Count)
            {
                playerTr.gameObject.SetActive(true);
                playerTr.Find("Icon").GetComponent<Image>().sprite = null;
                playerTr.Find("Level").GetComponent<Text>().text = friend_info_list[i].level.ToString();
                playerTr.Find("Name").GetComponent<Text>().text = friend_info_list[i].name;

            }
            else
                playerTr.gameObject.SetActive(false);

        }

    }

    //是否展示匹配计时
    public void Event_reqShowMatching(string order) {
        if (order == "true")
        {
            RoomGO.transform.Find("StartMatch").gameObject.SetActive(false);
            RoomGO.transform.Find("MatchingUI").gameObject.SetActive(true);
        }
        else if (order == "false")
        {
            RoomGO.transform.Find("StartMatch").gameObject.SetActive(true);
            RoomGO.transform.Find("MatchingUI").gameObject.SetActive(false);
        }
        else
            Debug.LogError("指令有误");
    }

    public void Event_enterBattleSpace() {
        SceneManager.LoadScene("GameScene");
    }

    #endregion


    #region  UI
    //创建匹配房间
    public void CreateMatchRoom() {
        Account account = KBEngineApp.app.player() as Account;
        account.baseCall("reqCreateMatchRoom");
        MainInfoData.GetInstance().InRoom = true;
    }

    //退出匹配房间
    public void ExitMatchRoom() {
        //清空聊天记录
        roomChattingRecords.Clear();
        //隐藏所有UI
        Transform content = chattingGO.transform.Find("ChattingRecords/Viewport/Content");
        Transform childExample = content.Find("Example");
        foreach (Transform child in content)
            child.gameObject.SetActive(false);

        Account account = KBEngineApp.app.player() as Account;
        account.baseCall("reqExitMatchRoom");
    }

    //接受邀请
    public void AcceptEnterRoom() {
        Account account = KBEngineApp.app.player() as Account;
        account.baseCall("reqEnterMatchRoom", reqID, reqName);
        reqMessageGO.SetActive(false);
        MainInfoData.GetInstance().InRoom = true;
    }

    //拒绝邀请
    public void RefuseEnterRoom() {
        reqMessageGO.SetActive(false);
        Debug.Log("拒绝进入房间");
    }

    //在房间发送聊天
    public void SendChattingToRoom(InputField inputField) {
        Account account = KBEngineApp.app.player() as Account;
        account.baseCall("sendMessageToMatchRoom", account.Name, inputField.text);
        inputField.text = null;
    }

    public void StartMatch() {
        Account account = KBEngineApp.app.player() as Account;
        account.baseCall("reqStartMatch");
    }



    //添加聊天到列表
    void AddOneChatting()
    {
        Transform content = chattingGO.transform.Find("ChattingRecords/Viewport/Content");
        Transform childExample = content.Find("Example");
        int num = roomChattingRecords.Count - 1;
        Transform needTr = content.Find(num + "");
        
        if (!needTr)
        {
            needTr = Instantiate(childExample);
            needTr.SetParent(content);
            needTr.name = num.ToString();
            needTr.Find("Text").GetComponent<Text>().text = roomChattingRecords[num];
            needTr.gameObject.SetActive(true);
        }
        else
        {
            needTr.Find("Text").GetComponent<Text>().text = roomChattingRecords[num];
            if (!needTr.gameObject.activeSelf)
                needTr.gameObject.SetActive(true);
        }

        needTr.Find("Text").GetComponent<ContentSizeFitter>().SetLayoutVertical();
        Vector2 size = needTr.transform.GetComponent<RectTransform>().sizeDelta;
        size.y = needTr.Find("Text").GetComponent<RectTransform>().sizeDelta.y + 10;
        needTr.transform.GetComponent<RectTransform>().sizeDelta = size;

        //强制刷新UI属性
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        //每次设置最底部
        ScrollRect scrollRect = chattingGO.transform.Find("ChattingRecords").GetComponent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 0;
    }
    #endregion

    private void OnDestroy()
    {
        //解除所有注册out事件
        KBEngine.Event.deregisterOut(this);
    }
}
