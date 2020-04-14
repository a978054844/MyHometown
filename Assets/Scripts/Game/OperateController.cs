using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperateController : MonoBehaviour {

    public enum OperationType {
        Normal,
        AnYeYiYang,
        XiaCiYiDing,
        NanShangJiaNan,
    }


    public static OperationType CurrentState = OperationType.Normal;

    void Update() {
        switch (Application.platform) {
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                //点击到UI上
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    break;
                DetectComputerOperate();
                break;
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                //点击到UI上
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) ||
                    UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId))
                    break;

                DetectMobilePhoneOperate();
                break;
        }
        
    }

    void DetectComputerOperate() {
        
        //点击鼠标左键
        if (Input.GetMouseButtonDown(0)) {
            switch (CurrentState) {
                case OperationType.AnYeYiYang:
                    OperateAnYeYiYangState(Input.mousePosition);
                    break;
                case OperationType.XiaCiYiDing:
                    OperateXiaCiYiDingState(Input.mousePosition);
                    break;
                case OperationType.NanShangJiaNan:
                    OperateNanShangJiaNanState(Input.mousePosition);
                    break;
            }
        }
    } 

    void DetectMobilePhoneOperate() {

    }

    #region 非正常状态
    void OperateAnYeYiYangState(Vector2 downPos) {
        object ob = RayOperation.LaunchRayOfScreen(downPos, RayOperation.ReturnType.GameObject);
        if (ob == null)
            return;

        GameObject needGO = (GameObject)ob;
        PlayerController playerController = needGO.GetComponent<PlayerController>();
        if (!playerController)
            return;
        PlayerController.SelfPlayer.KeepAvatar.cellCall("anYeYiYangSkill", playerController.KeepAvatar.id);

        //还原状态
        CurrentState = OperationType.Normal;
    }

    void OperateXiaCiYiDingState(Vector2 downPos)
    {
        object ob = RayOperation.LaunchRayOfScreen(downPos, RayOperation.ReturnType.GameObject);
        if (ob == null)
            return;

        GameObject needGO = (GameObject)ob;
        PlayerController playerController = needGO.GetComponent<PlayerController>();
        if (!playerController)
            return;
        PlayerController.SelfPlayer.KeepAvatar.cellCall("xiaCiYiDingSkill", playerController.KeepAvatar.id);

        //还原状态
        CurrentState = OperationType.Normal;
    }

    void OperateNanShangJiaNanState(Vector2 downPos) {
        object ob = RayOperation.LaunchRayOfScreen(downPos, RayOperation.ReturnType.Position);
        object ob1 = RayOperation.LaunchRayOfScreen(downPos, RayOperation.ReturnType.GameObject);
        if (ob == null || ob1 == null)
            return;
        
        GameObject needGO = (GameObject)ob1;
        if (!needGO.CompareTag("track"))
            return;

        Vector3 pos = (Vector3)ob;
        PlayerController.SelfPlayer.KeepAvatar.cellCall("nanShangJiaNanSkill", pos, int.Parse(needGO.name));

        //还原状态
        CurrentState = OperationType.Normal;
    }
    #endregion

}
