/*
	Generated by KBEngine!
	Please do not modify this file!
	Please inherit this module, such as: (class Account : AccountBase)
	tools = kbcmd
*/

namespace KBEngine
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	// defined in */scripts/entity_defs/Account.def
	public abstract class AccountBase : Entity
	{
		public EntityBaseEntityCall_AccountBase baseEntityCall = null;
		public EntityCellEntityCall_AccountBase cellEntityCall = null;

		public FRIEND_INFO_LIST Friend_list = new FRIEND_INFO_LIST();
		public virtual void onFriend_listChanged(FRIEND_INFO_LIST oldValue) {}
		public UInt16 Icon = 0;
		public virtual void onIconChanged(UInt16 oldValue) {}
		public Byte Level = 0;
		public virtual void onLevelChanged(Byte oldValue) {}
		public string Name = "";
		public virtual void onNameChanged(string oldValue) {}

		public abstract void backToMain(); 
		public abstract void enterBattleSpace(); 
		public abstract void reqAddFriendMessage(UInt32 arg1, string arg2); 
		public abstract void reqChangeNameCall(string arg1); 
		public abstract void reqEnterMatchRoomMessage(UInt32 arg1, string arg2); 
		public abstract void reqMessageCall(string arg1); 
		public abstract void reqShowMatching(string arg1); 
		public abstract void reqUpdateFriendChatting(UInt64 arg1, string arg2); 
		public abstract void reqUpdateFriendListUI(); 
		public abstract void reqUpdateMatchRoomChatting(string arg1, string arg2); 
		public abstract void reqUpdateMatchRoomUI(FRIEND_INFO_LIST arg1); 

		public override void onGetBase()
		{
			baseEntityCall = new EntityBaseEntityCall_AccountBase();
			baseEntityCall.id = id;
			baseEntityCall.className = className;
		}

		public override void onGetCell()
		{
			cellEntityCall = new EntityCellEntityCall_AccountBase();
			cellEntityCall.id = id;
			cellEntityCall.className = className;
		}

		public override void onLoseCell()
		{
			cellEntityCall = null;
		}

		public override EntityCall getBaseEntityCall()
		{
			return baseEntityCall;
		}

		public override EntityCall getCellEntityCall()
		{
			return cellEntityCall;
		}

		public override void onRemoteMethodCall(Method method, MemoryStream stream)
		{
			switch(method.methodUtype)
			{
				case 26:
					backToMain();
					break;
				case 25:
					enterBattleSpace();
					break;
				case 18:
					UInt32 reqAddFriendMessage_arg1 = stream.readUint32();
					string reqAddFriendMessage_arg2 = stream.readUnicode();
					reqAddFriendMessage(reqAddFriendMessage_arg1, reqAddFriendMessage_arg2);
					break;
				case 17:
					string reqChangeNameCall_arg1 = stream.readUnicode();
					reqChangeNameCall(reqChangeNameCall_arg1);
					break;
				case 21:
					UInt32 reqEnterMatchRoomMessage_arg1 = stream.readUint32();
					string reqEnterMatchRoomMessage_arg2 = stream.readUnicode();
					reqEnterMatchRoomMessage(reqEnterMatchRoomMessage_arg1, reqEnterMatchRoomMessage_arg2);
					break;
				case 16:
					string reqMessageCall_arg1 = stream.readUnicode();
					reqMessageCall(reqMessageCall_arg1);
					break;
				case 24:
					string reqShowMatching_arg1 = stream.readUnicode();
					reqShowMatching(reqShowMatching_arg1);
					break;
				case 20:
					UInt64 reqUpdateFriendChatting_arg1 = stream.readUint64();
					string reqUpdateFriendChatting_arg2 = stream.readUnicode();
					reqUpdateFriendChatting(reqUpdateFriendChatting_arg1, reqUpdateFriendChatting_arg2);
					break;
				case 19:
					reqUpdateFriendListUI();
					break;
				case 23:
					string reqUpdateMatchRoomChatting_arg1 = stream.readUnicode();
					string reqUpdateMatchRoomChatting_arg2 = stream.readUnicode();
					reqUpdateMatchRoomChatting(reqUpdateMatchRoomChatting_arg1, reqUpdateMatchRoomChatting_arg2);
					break;
				case 22:
					FRIEND_INFO_LIST reqUpdateMatchRoomUI_arg1 = ((DATATYPE_FRIEND_INFO_LIST)method.args[0]).createFromStreamEx(stream);
					reqUpdateMatchRoomUI(reqUpdateMatchRoomUI_arg1);
					break;
				default:
					break;
			};
		}

		public override void onUpdatePropertys(Property prop, MemoryStream stream)
		{
			switch(prop.properUtype)
			{
				case 4:
					FRIEND_INFO_LIST oldval_Friend_list = Friend_list;
					Friend_list = ((DATATYPE_FRIEND_INFO_LIST)EntityDef.id2datatypes[23]).createFromStreamEx(stream);

					if(prop.isBase())
					{
						if(inited)
							onFriend_listChanged(oldval_Friend_list);
					}
					else
					{
						if(inWorld)
							onFriend_listChanged(oldval_Friend_list);
					}

					break;
				case 3:
					UInt16 oldval_Icon = Icon;
					Icon = stream.readUint16();

					if(prop.isBase())
					{
						if(inited)
							onIconChanged(oldval_Icon);
					}
					else
					{
						if(inWorld)
							onIconChanged(oldval_Icon);
					}

					break;
				case 2:
					Byte oldval_Level = Level;
					Level = stream.readUint8();

					if(prop.isBase())
					{
						if(inited)
							onLevelChanged(oldval_Level);
					}
					else
					{
						if(inWorld)
							onLevelChanged(oldval_Level);
					}

					break;
				case 1:
					string oldval_Name = Name;
					Name = stream.readUnicode();

					if(prop.isBase())
					{
						if(inited)
							onNameChanged(oldval_Name);
					}
					else
					{
						if(inWorld)
							onNameChanged(oldval_Name);
					}

					break;
				case 40001:
					Vector3 oldval_direction = direction;
					direction = stream.readVector3();

					if(prop.isBase())
					{
						if(inited)
							onDirectionChanged(oldval_direction);
					}
					else
					{
						if(inWorld)
							onDirectionChanged(oldval_direction);
					}

					break;
				case 40000:
					Vector3 oldval_position = position;
					position = stream.readVector3();

					if(prop.isBase())
					{
						if(inited)
							onPositionChanged(oldval_position);
					}
					else
					{
						if(inWorld)
							onPositionChanged(oldval_position);
					}

					break;
					case 40002:
						stream.readUint32();
						break;
				default:
					break;
			};
		}

		public override void callPropertysSetMethods()
		{
			ScriptModule sm = EntityDef.moduledefs[className];
			Dictionary<UInt16, Property> pdatas = sm.idpropertys;

			FRIEND_INFO_LIST oldval_Friend_list = Friend_list;
			Property prop_Friend_list = pdatas[3];
			if(prop_Friend_list.isBase())
			{
				if(inited && !inWorld)
					onFriend_listChanged(oldval_Friend_list);
			}
			else
			{
				if(inWorld)
				{
					if(prop_Friend_list.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onFriend_listChanged(oldval_Friend_list);
					}
				}
			}

			UInt16 oldval_Icon = Icon;
			Property prop_Icon = pdatas[4];
			if(prop_Icon.isBase())
			{
				if(inited && !inWorld)
					onIconChanged(oldval_Icon);
			}
			else
			{
				if(inWorld)
				{
					if(prop_Icon.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onIconChanged(oldval_Icon);
					}
				}
			}

			Byte oldval_Level = Level;
			Property prop_Level = pdatas[5];
			if(prop_Level.isBase())
			{
				if(inited && !inWorld)
					onLevelChanged(oldval_Level);
			}
			else
			{
				if(inWorld)
				{
					if(prop_Level.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onLevelChanged(oldval_Level);
					}
				}
			}

			string oldval_Name = Name;
			Property prop_Name = pdatas[6];
			if(prop_Name.isBase())
			{
				if(inited && !inWorld)
					onNameChanged(oldval_Name);
			}
			else
			{
				if(inWorld)
				{
					if(prop_Name.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onNameChanged(oldval_Name);
					}
				}
			}

			Vector3 oldval_direction = direction;
			Property prop_direction = pdatas[1];
			if(prop_direction.isBase())
			{
				if(inited && !inWorld)
					onDirectionChanged(oldval_direction);
			}
			else
			{
				if(inWorld)
				{
					if(prop_direction.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onDirectionChanged(oldval_direction);
					}
				}
			}

			Vector3 oldval_position = position;
			Property prop_position = pdatas[0];
			if(prop_position.isBase())
			{
				if(inited && !inWorld)
					onPositionChanged(oldval_position);
			}
			else
			{
				if(inWorld)
				{
					if(prop_position.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onPositionChanged(oldval_position);
					}
				}
			}

		}
	}
}