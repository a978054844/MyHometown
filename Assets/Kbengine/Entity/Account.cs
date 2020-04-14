using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBEngine
{
    public class Account : AccountBase
    {
        

        public override void __init__()
        {
            base.__init__();
            //Event.fireOut(EventOutTypes.onLoginFailed, new object[] { ServerErr });
            Event.fireOut("onLoginSuccessfully", new object[] { KBEngineApp.app.entity_uuid, id, this });
        }

 

        //请求信息提示
        public override void reqMessageCall(string arg1)
        {
            
            KBEngine.Event.fireOut("reqMessageCall", arg1);
        }

        //创建名字客户端回调
        public override void reqChangeNameCall(string arg1)
        {
            Event.fireOut("reqChangeNameCall", arg1);
        }

        #region 好友列表
        //申请好友信息
        public override void reqAddFriendMessage(uint arg1, string arg2)
        {
            Event.fireOut("reqAddFriendMessage", arg1, arg2);
        }

        //更新好友列表信息
        public override void reqUpdateFriendListUI()
        {
            Event.fireOut("reqUpdateFriendListUI");
        }

        //更新好友聊天
        public override void reqUpdateFriendChatting(ulong arg1, string arg2)
        {
            Event.fireOut("reqUpdateFriendChatting", arg1, arg2);
        }
        #endregion

        #region 匹配房间
        //进入房间
        public override void reqEnterMatchRoomMessage(uint arg1, string arg2)
        {
            Event.fireOut("reqEnterMatchRoomMessage", arg1, arg2);
        }


        //更新匹配房间聊天
        public override void reqUpdateMatchRoomChatting(string arg1, string arg2)
        {
            Event.fireOut("reqUpdateMatchRoomChatting", arg1, arg2);
        }

        //更新匹配房间人物UI信息
        public override void reqUpdateMatchRoomUI(FRIEND_INFO_LIST arg1)
        {
            Event.fireOut("reqUpdateMatchRoomUI", arg1);
        }

        //显示匹配时间
        public override void reqShowMatching(string arg1)
        {
            Event.fireOut("reqShowMatching", arg1);
        }

        //进入战场
        public override void enterBattleSpace()
        {
            Event.fireOut("enterBattleSpace");
        }

        public override void backToMain()
        {
            KBEngine.Event.fireOut("backToMain");
        }

        #endregion

    }
}
