using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class PlayerController : MonoBehaviour {

    /// <summary>
    /// 本地客户端对应控制器
    /// </summary>
    public static PlayerController SelfPlayer;
    /// <summary>
    /// 持有的Avatar
    /// </summary>
    private KBEngine.Avatar _keepAvatar;
    /// <summary>
    /// 保持移动的持续的单位方向向量
    /// </summary>
    private Vector3 dir_continue;
    /// <summary>
    /// 是否持续移动
    /// </summary>
    private bool isContinueMove;
    /// <summary>
    /// 初始持续方向
    /// </summary>
    private Vector3 dir_original;


    [HideInInspector]
    public KBEngine.Avatar KeepAvatar {
        get { return _keepAvatar; }
        set
        {
            _keepAvatar = value;
            if (_keepAvatar.isPlayer())
            {
                SelfPlayer = this;
                dir_original = Vector3.forward;
            }
            RefreshSmallMap();
        }
    }

    protected virtual void Update()
    {
        
        if (_keepAvatar == null)
            return;
        else
        {
            if (_keepAvatar.position != transform.position)
            {
                if (_keepAvatar.isPlayer())
                {
                    ClampRotation();
                    _keepAvatar.position = transform.position;
                    _keepAvatar.direction = transform.eulerAngles;
                }else
                    RefreshStatus();

                RefreshSmallMap();
            }
        }
    }

    protected virtual void FixedUpdate() {
        if (_keepAvatar == null)
            return;
        if (!_keepAvatar.isPlayer())
            return;
        if (isContinueMove)
        {
            Vector3 dis = dir_continue * (_keepAvatar.moveSpeed / 50f);
            transform.Translate(dis);
        }
    }


    void RefreshStatus() {
        //更新位置
        //更新频率为10Hz
        transform.Translate((_keepAvatar.position - transform.position) / 7f, Space.World);
    }

    void RefreshSmallMap() {
        //更新小地图
        GameUIcontroller.DrivePercent[_keepAvatar.track] =
            1 - (Vector3.Distance(transform.position, new Vector3(0, 0.5f, 0f)) - 12) / 97f;
    }

    void ClampRotation()
    {
        float clampRange = 30;
        float needX = transform.rotation.eulerAngles.x;
        if (needX >= 180)
            needX = Mathf.Clamp(needX, 360 - clampRange, 360);
        else
            needX = Mathf.Clamp(needX, 0, clampRange);
        transform.rotation = Quaternion.Euler(needX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    public virtual void MoveSelf(Vector3 arrivePos, float speed = 0, float dis = 0) {
        //_keepAvatar.cellCall("gotoPosition", new object[] { arrivePos, speed, dis });
        if (speed == 0)
            speed = _keepAvatar.moveSpeed;
        iTween.MoveTo(gameObject, iTween.Hash("position", arrivePos,
            "speed", speed, "easeType", iTween.EaseType.linear));
        Debug.Log("MovePos : " + arrivePos + " || speed : " + speed);
    }

    public virtual void MoveSelf_Continue(Vector3 dir, bool isContinue) {
        if (isContinue)
            dir_continue = dir.normalized;
        else
            dir_continue = dir_original;
    }

    public void ControlContinueMove(bool isStart) {
        dir_continue = dir_original;
        isContinueMove = isStart;
    }

    #region AnimState

    public virtual void ChangeAnimState() {
        Animator animator = transform.GetComponent<Animator>();
        AnimStateType animStateType = (AnimStateType)System.Enum.Parse(typeof(AnimStateType), _keepAvatar.animState);
        switch (animStateType) {
            case AnimStateType.drive:
                animator.SetInteger("driveState", -1);
                animator.SetTrigger("drive");
                break;
            case AnimStateType.driveLeft:
                animator.SetInteger("driveState", 1);
                break;
            case AnimStateType.driveRight:
                animator.SetInteger("driveState", 2);
                break;
            case AnimStateType.driveFaild:
                animator.SetInteger("driveState", 0);
                break;
            case AnimStateType.driveSuccess:
                animator.SetInteger("driveState", 100);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_keepAvatar == null)
            return;
        if (!_keepAvatar.isPlayer())
            return;

        bool isArrive = other.gameObject.CompareTag("portal");
        Debug.Log("碰撞触发 : " + other.gameObject.name);
        if (isArrive)
        {
            other.enabled = false;
            _keepAvatar.cellCall("reqArriveSuccess");
        }
    }

    #endregion
}
