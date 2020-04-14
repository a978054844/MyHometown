using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;
using UnityEngine.UI;

public class KBEEvent_BattleSpace : MonoBehaviour {

    public GameObject avatarPrefab;
    public GameObject goldPrefab;
    public GameObject xCYDPrefab;
    public GameObject nSJNPrefab;

    public Text startSecond;

    private Dictionary<int, KBEngine.Avatar> avatarsDict = new Dictionary<int, KBEngine.Avatar>();

    private void Awake()
    {
        RegisterEventOfSpace();
        //通知服务器切换场景成功
        (KBEngineApp.app.player() as Account).baseCall("reqGiveClientToAvatar");
        
    }

    #region 注册的事件

    void RegisterEventOfSpace() {
        KBEngine.Event.registerOut(KBEngine.EventOutTypes.onEnterWorld, this, "Event_onEnterWorld");
        KBEngine.Event.registerOut(KBEngine.EventOutTypes.onLeaveWorld, this, "Event_onLeaveWorld");

        KBEngine.Event.registerOut("startGameSecond", this, "Event_startGameSecond");
        KBEngine.Event.registerOut("onAnimStateChanged", this, "Event_onAnimStateChanged");
        KBEngine.Event.registerOut("jumpToPosition", this, "Event_jumpToPosition");
        KBEngine.Event.registerOut("backToMain", this, "Event_backToMain");
    }

    public void Event_onEnterWorld(KBEngine.Entity entity)
    {
        Debug.Log("类型为 : " + entity.className +  "的实体进入世界  实体ID为 : " + entity.id);
        //if(entity.renderObj != null) {
        //    Debug.LogWarning("实体已经有renderObj, 无法再次进入");
        //    return;
        //}
        switch (entity.className) {
            case "Avatar":
                AvatarEnterWorld(entity);
                break;
            case "Gold":
                GoldEnterWorld(entity);
                break;
            case "XiaCiYiDingProp":
                XiaCiYiDingPropEnterWorld(entity);
                break;
            case "NanShangJiaNanProp":
                NanShangJiaNanPropEnterWorld(entity);
                break;
                
        }
    }

    public void Event_onLeaveWorld(KBEngine.Entity entity) {
        Debug.Log("类型为 : " + entity.className + "的实体离开世界  实体ID为 : " + entity.id);
        switch (entity.className) {
            case "Avatar":
                break;
            case "Gold":
                Destroy((GameObject)entity.renderObj);
                break;
            case "XiaCiYiDingProp":
                Destroy((GameObject)entity.renderObj);
                break;
            case "NanShangJiaNanProp":
                Destroy((GameObject)entity.renderObj);
                break;
        }
    }

    public void Event_startGameSecond(byte arg1) {

        Debug.Log("倒计时时间 : " + arg1);
        if (arg1 == 0)
        {
            startSecond.gameObject.SetActive(false);
            Vector3 moveDir = Vector3.zero - PlayerController.SelfPlayer.KeepAvatar.position;
            PlayerController.SelfPlayer.KeepAvatar.cellCall("changeAnimState", AnimStateType.drive.ToString());
            PlayerController.SelfPlayer.ControlContinueMove(true);
        }
        else
        {
            if (!startSecond.gameObject.activeSelf)
                startSecond.gameObject.SetActive(true);
            startSecond.text = "比赛倒计时 : " + arg1;
        }

    }

    public void Event_onAnimStateChanged(KBEngine.Entity entity) {
        GameObject gameObject = entity.renderObj as GameObject;
        gameObject.GetComponent<PlayerController>().ChangeAnimState();
    }

    public void Event_jumpToPosition(Vector3 pos, KBEngine.Entity entity) {
        GameObject gameObject = entity.renderObj as GameObject;
        //KBEngine.Avatar avatar = entity as KBEngine.Avatar;
        //avatar.position = pos;
        gameObject.transform.position = pos;
    }

    public void Event_backToMain() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");

    }
    #endregion

    #region 进入世界
    void AvatarEnterWorld(KBEngine.Entity entity) {
        KBEngine.Avatar selfAvatar = entity as KBEngine.Avatar;
        GameObject selfGO = Instantiate(avatarPrefab);
        selfAvatar.renderObj = selfGO;

        selfGO.transform.eulerAngles = selfAvatar.direction;
        selfGO.transform.Find("Name").GetComponent<TextMesh>().text = selfAvatar.name;
        selfGO.name = selfAvatar.name;
        selfGO.transform.position = selfAvatar.position;
        if (selfAvatar.isPlayer())
        {
            selfGO.transform.Find("Name").GetComponent<TextMesh>().color = Color.red;
            selfGO.AddComponent<Rigidbody>();
            selfGO.GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            selfGO.transform.Find("Name").GetComponent<TextMesh>().color = Color.green;
            Destroy(selfGO.transform.Find("CameraView/FirstView").gameObject);
            Destroy(selfGO.transform.Find("CameraView/ThirdView").gameObject);
        }
        selfGO.GetComponent<PlayerController>().KeepAvatar = selfAvatar;

        //UI小地图
        GameUIcontroller.SmallMap.Find(selfAvatar.track + "").gameObject.SetActive(true);
        
        GameUIcontroller.SmallMap.Find(selfAvatar.track + "/Name").GetComponent<Text>().text = selfAvatar.name;
        GameUIcontroller.SmallMap.Find(selfAvatar.track + "/Percent").GetComponent<Text>().text = "0%";
        GameUIcontroller.ViewParent[selfAvatar.track] = selfGO.transform.Find("CameraView/OthersView");
        if (selfAvatar.isPlayer())
        {
            GameUIcontroller.SmallMap.Find(selfAvatar.track + "/View").gameObject.SetActive(false);
            GameUIcontroller.SmallMap.Find(selfAvatar.track + "/Name").GetComponent<Text>().color = Color.red;
            GameUIcontroller.SmallMap.Find(selfAvatar.track + "/Percent").GetComponent<Text>().color = Color.red;
        }

        avatarsDict.Add(selfAvatar.id, selfAvatar);
    }

    void GoldEnterWorld(KBEngine.Entity entity) {
        KBEngine.Gold selfGold = entity as KBEngine.Gold;
        GameObject selfGO = Instantiate(goldPrefab);
        selfGO.name = selfGold.id + "";
        selfGold.renderObj = selfGO;
        selfGO.transform.position = selfGold.position;
        selfGO.transform.eulerAngles = selfGold.direction;
        selfGO.transform.localScale = new Vector3(1 * selfGold.goldValue, 1 * selfGold.goldValue, 1.5f);
        goldPrefab.GetComponent<GoldController>().KeepGold = selfGold;
    }

    void XiaCiYiDingPropEnterWorld(KBEngine.Entity entity) {
        KBEngine.XiaCiYiDingProp prop = entity as KBEngine.XiaCiYiDingProp;
        
        GameObject propGO = Instantiate(xCYDPrefab);
        GameObject acceptGO = avatarsDict[(int)prop.acceptID].renderObj as GameObject;
        propGO.transform.SetParent(acceptGO.transform);
        propGO.transform.localPosition = Vector3.forward * 2;
        propGO.transform.localEulerAngles = Vector3.zero;
        prop.renderObj = propGO;
        propGO.GetComponent<EntityUpdatePosAndDir>().KeepEntity = prop;
    }

    void NanShangJiaNanPropEnterWorld(KBEngine.Entity entity) {
        KBEngine.NanShangJiaNanProp prop = entity as KBEngine.NanShangJiaNanProp;

        GameObject propGO = Instantiate(nSJNPrefab);
        propGO.transform.position = prop.position;
        propGO.transform.eulerAngles = prop.direction;
        prop.renderObj = propGO;
    }
    #endregion

    private void OnDestroy()
    {
        //解除所有注册out事件
        KBEngine.Event.deregisterOut(this);
    }
}
