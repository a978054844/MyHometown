using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameUIcontroller : MonoBehaviour {


    public static List<float> DrivePercent = new List<float>() { 0, 0, 0, 0, 0 };
    public static List<Transform> ViewParent = new List<Transform>() { null, null, null, null, null };

    public Text GoldNumText;

    public Camera OthersCamera;

    private List<Vector3> onePercentVec;
    private Vector3 destinationPos;


    private static Transform _smallMap;

    private bool curIsFirstView = true;

    public static Transform SmallMap {
        get { return _smallMap; }
        set { _smallMap = value; }
    }

    private void Start()
    {

        KBEngine.Event.registerOut("onGoldNumChanged", this, "Event_onGoldNumChanged");
        KBEngine.Event.registerOut("ArriveSuccess", this, "Event_ArriveSuccess");
        KBEngine.Event.registerOut("ArriveDefeat", this, "Event_ArriveDefeat");

        SmallMapSetting();

    }

    private void Update()
    {
        RefreshSmallMap();
    }

    void SmallMapSetting() {
        SmallMap = transform.Find("SmallMap");
        for (int i = 0; i < 5; i++) {
            int index = i;
            SmallMap.Find(i + "/View").GetComponent<Button>().onClick.AddListener(delegate {
                LookOthersView(index);
            });
        }
        SetOnePercentList();
    }

    void SetOnePercentList() {
        destinationPos = SmallMap.Find("Destination").position;
        onePercentVec = new List<Vector3>();
        for (int i = 0; i < DrivePercent.Count; i++)
        {
            Vector3 vector = (SmallMap.Find(i + "").position - destinationPos) / 100f;
            onePercentVec.Add(vector);
        }
    }

    void RefreshSmallMap() {
        for (int i = 0; i < DrivePercent.Count; i++) {
            Transform curTran = SmallMap.Find(i + "/Percent");
            curTran.GetComponent<Text>().text = (int)(DrivePercent[i] * 100) + "%";
            float curPercent = DrivePercent[i];
            if (curPercent < 0)
                curPercent = 0;
            curTran.parent.position = destinationPos + (1 - curPercent) * 100 * onePercentVec[i];
        }
    }

    void ShowBackMain() {
        transform.Find("MatchEnd").gameObject.SetActive(true);
    }

    #region UIButton事件
    
    public void DirButtonDown(bool isLeft) {
        Animator animator = PlayerController.SelfPlayer.gameObject.GetComponent<Animator>();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Vector3 dir;
        AnimStateType animState;
        
        if (isLeft)
        {
            if (!stateInfo.IsName(AnimStateType.drive.ToString()) &&
                !stateInfo.IsName(AnimStateType.driveRight.ToString()))
                return;
            dir = Vector3.Normalize(new Vector3(-1, 0, 1));
            animState = AnimStateType.driveLeft;
        }
        else {
            if (!stateInfo.IsName(AnimStateType.drive.ToString()) &&
                !stateInfo.IsName(AnimStateType.driveLeft.ToString()))
                return;
            dir = Vector3.Normalize(new Vector3(1, 0, 1));
            animState = AnimStateType.driveRight;
        }
        PlayerController.SelfPlayer.KeepAvatar.cellCall("changeAnimState", animState.ToString());
        PlayerController.SelfPlayer.MoveSelf_Continue(dir, true);
    }

    public void DirButtonUp() {
        if (PlayerController.SelfPlayer.KeepAvatar == null)
            return;

        Animator animator = PlayerController.SelfPlayer.gameObject.GetComponent<Animator>();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("idle"))
            return;

        PlayerController.SelfPlayer.KeepAvatar.cellCall("changeAnimState", AnimStateType.drive.ToString());
        PlayerController.SelfPlayer.MoveSelf_Continue(Vector3.zero, false);
    }


    public void ChangeView(Text curText) {
        if (OthersCamera.enabled)
            OthersCamera.enabled = false;
        curIsFirstView = !curIsFirstView;
        PlayerController.SelfPlayer.transform.Find("CameraView/FirstView").gameObject.SetActive(curIsFirstView);
        PlayerController.SelfPlayer.transform.Find("CameraView/ThirdView").gameObject.SetActive(!curIsFirstView);
        if (curIsFirstView)
            curText.text = "第三视角";
        else
            curText.text = "第一视角";
        
    }

    public void LookOthersView(int index) {
        PlayerController.SelfPlayer.transform.Find("CameraView/FirstView").gameObject.SetActive(false);
        PlayerController.SelfPlayer.transform.Find("CameraView/ThirdView").gameObject.SetActive(false);

        OthersCamera.gameObject.GetComponent<OthersViewController>().FollowTran = ViewParent[index];

    }

    public void ResetView() {
        if(curIsFirstView)
            PlayerController.SelfPlayer.transform.Find("CameraView/FirstView").gameObject.SetActive(true);
        else
            PlayerController.SelfPlayer.transform.Find("CameraView/ThirdView").gameObject.SetActive(true);
        OthersCamera.enabled = false;
        
    }

    public void JiaSuSkill() {
        PlayerController.SelfPlayer.KeepAvatar.cellCall("jiaSuSkill");
    }

    public void AnYeYiYangSkill() {
        OperateController.CurrentState = OperateController.OperationType.AnYeYiYang;
    }

    public void XiaCiYiDingSkill() {
        OperateController.CurrentState = OperateController.OperationType.XiaCiYiDing;
    }

    public void NanShangJiaNanSkill() {
        OperateController.CurrentState = OperateController.OperationType.NanShangJiaNan;
    }

    public void BackToMain(bool isContinue) {
        
        PlayerController.SelfPlayer.KeepAvatar.baseCall("reqBackToMain", isContinue);
    }

    #endregion

    #region 注册的事件

    public void Event_onGoldNumChanged() {
        GoldNumText.text = PlayerController.SelfPlayer.KeepAvatar.goldNum + "";
    }

    public void Event_ArriveSuccess()
    {
        PlayerController.SelfPlayer.KeepAvatar.cellCall("changeAnimState", AnimStateType.driveSuccess.ToString());
        Text desText = transform.Find("MatchEnd/Description").GetComponent<Text>();
        desText.text = "恭喜了!!!";
        desText.color = new Color(218 / 255f, 50 / 255f, 125 / 255f, 1f);
        Invoke("ShowBackMain", 3);
    }

    public void Event_ArriveDefeat(uint id)
    {
        PlayerController.SelfPlayer.KeepAvatar.cellCall("changeAnimState", AnimStateType.driveFaild.ToString());
        Text desText = transform.Find("MatchEnd/Description").GetComponent<Text>();
        desText.text = "菜鸡???";
        desText.color = new Color(173 / 255f, 222 / 255f, 0 / 255f, 1f);
        Invoke("ShowBackMain", 3);
    }

    #endregion

    private void OnDestroy()
    {
        //解除所有注册out事件
        KBEngine.Event.deregisterOut(this);
    }
}
