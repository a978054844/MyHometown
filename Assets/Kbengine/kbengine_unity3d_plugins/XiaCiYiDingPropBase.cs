/*
	Generated by KBEngine!
	Please do not modify this file!
	Please inherit this module, such as: (class XiaCiYiDingProp : XiaCiYiDingPropBase)
	tools = kbcmd
*/

namespace KBEngine
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	// defined in */scripts/entity_defs/XiaCiYiDingProp.def
	public abstract class XiaCiYiDingPropBase : Entity
	{
		public EntityBaseEntityCall_XiaCiYiDingPropBase baseEntityCall = null;
		public EntityCellEntityCall_XiaCiYiDingPropBase cellEntityCall = null;

		public UInt32 acceptID = 0;
		public virtual void onAcceptIDChanged(UInt32 oldValue) {}
		public UInt32 launchID = 0;
		public virtual void onLaunchIDChanged(UInt32 oldValue) {}
		public string name = "";
		public virtual void onNameChanged(string oldValue) {}


		public override void onGetBase()
		{
			baseEntityCall = new EntityBaseEntityCall_XiaCiYiDingPropBase();
			baseEntityCall.id = id;
			baseEntityCall.className = className;
		}

		public override void onGetCell()
		{
			cellEntityCall = new EntityCellEntityCall_XiaCiYiDingPropBase();
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
				default:
					break;
			};
		}

		public override void onUpdatePropertys(Property prop, MemoryStream stream)
		{
			switch(prop.properUtype)
			{
				case 28:
					UInt32 oldval_acceptID = acceptID;
					acceptID = stream.readUint32();

					if(prop.isBase())
					{
						if(inited)
							onAcceptIDChanged(oldval_acceptID);
					}
					else
					{
						if(inWorld)
							onAcceptIDChanged(oldval_acceptID);
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
				case 27:
					UInt32 oldval_launchID = launchID;
					launchID = stream.readUint32();

					if(prop.isBase())
					{
						if(inited)
							onLaunchIDChanged(oldval_launchID);
					}
					else
					{
						if(inWorld)
							onLaunchIDChanged(oldval_launchID);
					}

					break;
				case 30:
					string oldval_name = name;
					name = stream.readUnicode();

					if(prop.isBase())
					{
						if(inited)
							onNameChanged(oldval_name);
					}
					else
					{
						if(inWorld)
							onNameChanged(oldval_name);
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

			UInt32 oldval_acceptID = acceptID;
			Property prop_acceptID = pdatas[3];
			if(prop_acceptID.isBase())
			{
				if(inited && !inWorld)
					onAcceptIDChanged(oldval_acceptID);
			}
			else
			{
				if(inWorld)
				{
					if(prop_acceptID.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onAcceptIDChanged(oldval_acceptID);
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

			UInt32 oldval_launchID = launchID;
			Property prop_launchID = pdatas[4];
			if(prop_launchID.isBase())
			{
				if(inited && !inWorld)
					onLaunchIDChanged(oldval_launchID);
			}
			else
			{
				if(inWorld)
				{
					if(prop_launchID.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onLaunchIDChanged(oldval_launchID);
					}
				}
			}

			string oldval_name = name;
			Property prop_name = pdatas[5];
			if(prop_name.isBase())
			{
				if(inited && !inWorld)
					onNameChanged(oldval_name);
			}
			else
			{
				if(inWorld)
				{
					if(prop_name.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onNameChanged(oldval_name);
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