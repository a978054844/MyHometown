using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBEngine
{
    public class Avatar : AvatarBase
    {
        
        public override void __init__()
        {
            base.__init__();
        }

        public override void onGoldNumChanged(ushort oldValue)
        {
            base.onGoldNumChanged(oldValue);
            KBEngine.Event.fireOut("onGoldNumChanged");
        }

        public override void onAnimStateChanged(string oldValue)
        {
            base.onAnimStateChanged(oldValue);
            KBEngine.Event.fireOut("onAnimStateChanged", this);
        }

        public override void onMoveSpeedChanged(float oldValue)
        {
            base.onMoveSpeedChanged(oldValue);
            Debug.Log(name + " || moveSpeed变化为 : " + moveSpeed);
        }


        #region ClientMethods

        //开始游戏数秒  base
        public override void startGameSecond(byte arg1)
        {
            KBEngine.Event.fireOut("startGameSecond", arg1);
        }

        //直接改变位置
        public override void jumpToPosition(Vector3 arg1)
        {
            KBEngine.Event.fireOut("jumpToPosition", arg1, this);
        }

        public override void ArriveDefeat(uint arg1)
        {
            KBEngine.Event.fireOut("ArriveDefeat", arg1);
            
        }

        public override void ArriveSuccess()
        {
            KBEngine.Event.fireOut("ArriveSuccess");
        }

        #endregion
    }
}
