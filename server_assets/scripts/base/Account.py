
# -*- coding: utf-8 -*-
import math

import KBEngine
from KBEDebug import *
from FRIEND_INFO import *
import BattleSpacePosConfigs

class Account(KBEngine.Proxy):
	def __init__(self):
		KBEngine.Proxy.__init__(self)


		if self.Level == 0:
			self.Level = 1
		if self.Icon == 0:
			self.Icon = 1

		self.DbidToIndex = {} #好友列表dbid对应index下标

		self.OnlineFriends = [] #在线好友实体数组
		self.battleSpace= None #当前加载空间

		self.readyGetClient = False #是否准备从avatar获得客户端
		self.isContinueRoom = False #从avatar回来后是否继续房间

		
	def onTimer(self, id, userArg):
		"""
		KBEngine method.
		使用addTimer后， 当时间到达则该接口被调用
		@param id		: addTimer 的返回值ID
		@param userArg	: addTimer 最后一个参数所给入的数据
		"""
		DEBUG_MSG(id, userArg)
		
	def onClientEnabled(self):
		"""
		KBEngine method.
		该entity被正式激活为可使用， 此时entity已经建立了client对应实体， 可以在此创建它的
		cell部分。
		"""
		INFO_MSG("account[%i] entities enable. entityCall:%s" % (self.id, self.client))
		if self.readyGetClient == True:
			DEBUG_MSG("account[%i].base::onGetClient" % self.id)
			self.client.backToMain()
			self.battleSpace.ReturnClient()
			#重置好友数据
			self.DbidToIndex = {}
			self.OnlineFriends = []

			
	def onLogOnAttempt(self, ip, port, password):
		"""
		KBEngine method.
		客户端登陆失败时会回调到这里
		"""
		INFO_MSG(ip, port, password)
		return KBEngine.LOG_ON_ACCEPT
		
	def onClientDeath(self):
		"""
		KBEngine method.
		客户端对应实体已经销毁
		"""
		DEBUG_MSG("Account[%i].onClientDeath:" % self.id)

		if len(self.OnlineFriends) != 0:
			for onlineFriend in self.OnlineFriends:
				DEBUG_MSG(onlineFriend.Name)
				onlineFriend.updateFriendStatus(self.databaseID, False, self)

		self.destroy()


	def mainSceneLoaded(self):
		DEBUG_MSG("Account[%i].mainSceneLoaded:" % self.id)
		self.updateFriendsMessage()
		if self.readyGetClient == False:
			return
		DEBUG_MSG(self.isContinueRoom)
		self.isContinueRoom = bool(self.isContinueRoom)
		if self.isContinueRoom:
			self.client.reqUpdateMatchRoomUI(self.getAllRoomPlayerInfo())
		else:
			self.reqExitMatchRoom()


	def reqChangeName(self, name):

		"""
		数据库查询语句
		"SELECT * FROM tbl_account WHERE sm_Name = '" + name + "'"
		"SELECT * FROM kbe.tbl_account WHERE sm_Name = '" + name + "'"
		"""
		sqlCommond = "SELECT * FROM tbl_account WHERE sm_Name = '" + name + "'"

		#回调函数
		def sqlcallback(result, row, insertid, errstr):
			if errstr:#输出错误信息
				ERROR_MSG("Account[%i].reqChangeName:[%s]" % (self.id, errstr))
				self.client.reqChangeNameCall(errstr)
			elif len(result) == 0:#不存在则注册名字
				if name == "":
					return
				self.Name = name
				DEBUG_MSG("Account[%i].reqChangeName:[%s]" % (self.id, name))
				# 写入数据库  回调函数暂无
				self.writeToDB()
				self.client.reqChangeNameCall("success")
			elif len(result) != 0:#告诉客户端 名字已存在
				DEBUG_MSG("该名字已存在")
				self.client.reqChangeNameCall("repetition")
		#查询数据库
		KBEngine.executeRawDatabaseCommand(sqlCommond, sqlcallback)

	def addFriend(self, name):
		sqlCommond = "SELECT * FROM tbl_account WHERE sm_Name = '" + name + "'"

		# 数据库查询回调
		def sqlcallback(result, row, insertid, errstr):
			if errstr:  # 输出错误信息
				ERROR_MSG("Account[%i].reqChangeName:[%s]" % (self.id, errstr))
			elif len(result) == 0:  # 不存在角色名字
				self.client.reqMessageCall("名字为  " + name + "  的角色不存在")
				DEBUG_MSG("Account[%i].addFriend:[%s] || 该角色名不存在" % (self.id, name))
			elif len(result) != 0:  # 名字存在
				dbid = int(result[0][0])
				level = int(result[0][3])

				# 检测在线回调
				def lookUpEntityCallBack(result):
					if result == True:
						self.client.reqMessageCall("名字为  " + name + "  的角色不在线")
						DEBUG_MSG("lookUpEntityCallBack : 玩家不在线")
					elif result == False:
						DEBUG_MSG("lookUpEntityCallBack : 其他原因")
					else:
						DEBUG_MSG("lookUpEntityCallBack : 玩家在线")
						#像该id实体的客户端发送好友请求
						KBEngine.entities[result.id].client.reqAddFriendMessage(self.id, self.Name)

				# 检查该DBID对应实体是否检出  也就是是否在线
				KBEngine.lookUpEntityByDBID("Account", dbid, lookUpEntityCallBack)

		# 查询数据库
		KBEngine.executeRawDatabaseCommand(sqlCommond, sqlcallback)

	def reqAddFriendMessageCall(self, needId, isAccept):

		needEntity = KBEngine.entities[needId]

		if isAccept == 'True':# 同意 则互相存入信息
			# 从新赋值 否则不更新数据
			self.Friend_list.append({'dbid': needEntity.databaseID, "name": needEntity.Name,
									 "level": needEntity.Level, "status": True, "icon": needEntity.Icon})
			list = self.Friend_list
			self.Friend_list = list
			needEntity.Friend_list.append({'dbid': self.databaseID, "name": self.Name,
										   "level": self.Level, "status": True, "icon": self.Icon})
			list1 = needEntity.Friend_list
			needEntity.Friend_list = list1
			# 写入数据库
			def selfWriteToDBCall(isSuccess, baseEntity):
				if isSuccess:
					baseEntity.client.reqUpdateFriendListUI()
				else:
					ERROR_MSG("reqAddFriendMessageCall : self存入数据库失败")
			def needEntityWriteToDBCall(isSuccess, baseEntity):
				if isSuccess:
					baseEntity.client.reqUpdateFriendListUI()
				else:
					ERROR_MSG("reqAddFriendMessageCall : needEntity存入数据库失败")

			DEBUG_MSG(self.Friend_list)
			DEBUG_MSG(needEntity.Friend_list)
			self.writeToDB(selfWriteToDBCall)
			needEntity.writeToDB(needEntityWriteToDBCall)
			# 更新客户端好友列表UI

		elif isAccept == 'False':# 不同意 返回提示
			needEntity.client.reqMessageCall(self.Name + "  拒绝了您的好友请求")
			DEBUG_MSG("拒绝互加好友")
		DEBUG_MSG("reqAddFriendMessageCall | id : %i  isAccept : %s" % (needId, isAccept))

	def delFriend(self, name):
		return

	def sendChattingMessage(self, dbid, message):
		# 检测在线回调
		def lookUpEntityCallBack(result):
			if result == True:
				# 应设置数据库暂存  先空着
				DEBUG_MSG("sendChattingMessage : 玩家不在线")
			elif result == False:
				DEBUG_MSG("sendChattingMessage : 其他原因")
			else:
				DEBUG_MSG("sendChattingMessage : 玩家 %s 在线" % result.Name)
				# 像该id实体的客户端发送聊天信息
				KBEngine.entities[result.id].client.reqUpdateFriendChatting(self.databaseID, message)

		# 检查该DBID对应实体是否检出  也就是是否在线
		KBEngine.lookUpEntityByDBID("Account", dbid, lookUpEntityCallBack)

	def getAllRoomPlayerInfo(self):
		allInfo = []
		# 人员信息获取
		for player in self.MatchRoomAllPlayers:
			#将字典转换成obj
			obj = friend_info_inst.createObjFromDict({'dbid': player.databaseID, "name": player.Name,
													  "level": player.Level, "status": True, "icon": player.Icon})
			allInfo.append(obj)

		return allInfo

	def reqCreateMatchRoom(self):
		DEBUG_MSG("Account[%i].reqCreateMatchRoom:" % self.id)
		self.MatchRoomOwner = self
		self.MatchRoomAllPlayers = [self]
		list1 = self.getAllRoomPlayerInfo()

		self.client.reqUpdateMatchRoomUI(list1)
		return

	def reqAddFriendToMatchRoom(self, reqDBID):

		def lookUpEntityCallBack(result):

			if result == True:
				DEBUG_MSG("reqAddFriendToMatchRoom : 玩家不在线")
			elif result == False:
				DEBUG_MSG("reqAddFriendToMatchRoom : 其他原因")
			else:
				DEBUG_MSG("reqAddFriendToMatchRoom : 玩家 %s 在线" % result.Name)
				# 像该id实体的客户端发送显示邀请信息
				KBEngine.entities[result.id].client.reqEnterMatchRoomMessage(self.id, self.Name)

		# 检查该DBID对应实体是否检出  也就是是否在线
		KBEngine.lookUpEntityByDBID("Account", reqDBID, lookUpEntityCallBack)


	def reqEnterMatchRoom(self, reqID, reqName):
		DEBUG_MSG("Account[%i].reqEnterMatchRoom : reqID:%i | reqName:%s" % (self.id, reqID, reqName))
		entityCall = KBEngine.entities[reqID]
		if entityCall.Name != reqName:
			DEBUG_MSG("reqEnterMatchRoom : 请求名字与实体id名字不匹配, 请求失效")
			return

		players = entityCall.MatchRoomAllPlayers
		for player in players:
			if player == self:
				DEBUG_MSG("reqEnterMatchRoom : 该房间已有玩家 " + self.Name)
				return
		players.append(self)
		self.MatchRoomAllPlayers = players
		self.MatchRoomOwner = entityCall.MatchRoomOwner

		allInfo = self.getAllRoomPlayerInfo()

		for player in players:
			if players != self:
				player.MatchRoomAllPlayers = players
			player.client.reqUpdateMatchRoomUI(allInfo)


	def reqExitMatchRoom(self):
		DEBUG_MSG("Account[%i].reqExitMatchRoom" % self.id)
		#退出房间的不是房主
		if self.MatchRoomOwner != self:
			# 删除自己
			self.MatchRoomAllPlayers.remove(self)
			# 获取没有自己后的信息
			allInfo = self.getAllRoomPlayerInfo()
			for play in self.MatchRoomAllPlayers:
				play.MatchRoomAllPlayers = self.MatchRoomAllPlayers
				#客户端没有准备好
				if play.clientEnabled == True:
					play.client.reqUpdateMatchRoomUI(allInfo)

			# 修改自己的信息
			self.MatchRoomOwner = None
			self.MatchRoomAllPlayers = []
			self.client.reqUpdateMatchRoomUI([])
		else:
			self.reqDeleteMatchRoom()

	def reqDeleteMatchRoom(self):
		DEBUG_MSG("Account[%i].reqDeleteMatchRoom" % self.id)
		oldPlayers = self.MatchRoomAllPlayers

		for player in oldPlayers:
			player.MatchRoomOwner = None
			player.MatchRoomAllPlayers = []

			if player.clientEnabled == True:
				player.client.reqUpdateMatchRoomUI([])

	def sendMessageToMatchRoom(self, playerName, sendMess):
		DEBUG_MSG("Account[%i].sendMessageToMatchRoom : playerName:%s | sendMess:%s" % (self.id, playerName, sendMess))
		for player in self.MatchRoomAllPlayers:
			player.client.reqUpdateMatchRoomChatting(playerName, sendMess)

	def reqStartMatch(self):
		DEBUG_MSG("Account[%i].reqStartMatch" % self.id)

		dontReadyName = ""
		for player in self.MatchRoomAllPlayers:
			if player.clientEnabled == False:
				dontReadyName = player.Name
				break
		if dontReadyName == "":
			hall = KBEngine.globalData["Hall"]
			hall.reqAddMatch(self.MatchRoomAllPlayers)
			for player in self.MatchRoomAllPlayers:
				player.client.reqShowMatching('true')
		else:
			tips = "玩家 : " + dontReadyName + " 没有退回到房间"
			for player in self.MatchRoomAllPlayers:
				if player.clientEnabled == True:
					player.client.reqMessageCall(tips)




	def reqStopMatch(self):
		DEBUG_MSG("Account[%i].reqStopMatch" % self.id)
		hall = KBEngine.globalData["Hall"]
		hall.reqDelMatch(self.MatchRoomAllPlayers)
		for player in self.MatchRoomAllPlayers:
			player.client.reqShowMatching('false')

	def updateFriendsMessage(self):
		DEBUG_MSG("Account[%i].updateFriendsMessage" % self.id)
		global newInfoList
		newInfoList = []
		global  curNum
		curNum = 0
		def finishCallBack(num = 1):
			global newInfoList
			global curNum
			curNum += num

			if curNum == len(newInfoList):
				conversionList = []
				for i in range(0, len(newInfoList)):
					self.Friend_list[i] = friend_info_inst.createObjFromDict(newInfoList[i])
				list1 = self.Friend_list
				self.Friend_list = list1
				self.client.reqUpdateFriendListUI()

		# 检测在线回调
		def lookUpEntityCallBack(result):
			global newInfoList

			if result == True:
				finishCallBack()
				return
			elif result == False:
				DEBUG_MSG("updateFriendsMessage : 其他原因")
				return
			else:
				DEBUG_MSG("在线玩家姓名 : " + result.Name)
				newInfoList[self.DbidToIndex[result.databaseID]]["status"] = True
				#通知好友已经上线
				KBEngine.entities[result.id].updateFriendStatus(self.databaseID, True, self)
				#存入在线好友
				self.OnlineFriends.append(KBEngine.entities[result.id])
				finishCallBack()

		# 数据库操作回调函数
		def sqlcallback(result, row, insertid, errstr):
			global newInfoList
			if errstr:  # 输出错误信息
				ERROR_MSG("updateFriendsMessage[%i].reqChangeName:[%s]" % (self.id, errstr))
				self.client.reqChangeNameCall(errstr)
			elif len(result) > 0:
				newInfoList.append({'dbid': int(result[0][0]), "name": result[0][2].decode("utf-8"), "level": int(result[0][3]),
									"status": False, "icon": int(result[0][4])})
				self.DbidToIndex[int(result[0][0])] = len(newInfoList) - 1

			#回调完毕
			if len(newInfoList) == len(self.Friend_list):
				for oneInfo in newInfoList:
					# 检查该DBID对应实体是否检出  也就是是否在线
					KBEngine.lookUpEntityByDBID("Account", oneInfo['dbid'], lookUpEntityCallBack)


		for friendInfo in self.Friend_list:
			needDict = friend_info_inst.getDictFromObj(friendInfo)
			sqlCommond = "SELECT * FROM tbl_account WHERE id = '" + str(needDict["dbid"]) + "'"
			KBEngine.executeRawDatabaseCommand(sqlCommond, sqlcallback)


	def updateFriendStatus(self, fri_dbid, fri_status, fri_entity):
		DEBUG_MSG("Account[%i].updateFriendStatus:: fri_dbid:%i | fri_status:%i " % (self.id, fri_dbid, fri_status))
		#dbid对应下标3  不转成字典了
		self.Friend_list[self.DbidToIndex[fri_dbid]][3] = fri_status
		newList = self.Friend_list
		self.Friend_list = newList

		#客户端实体不可用时 返回
		if self.clientEnabled == True:
			self.client.reqUpdateFriendListUI()

		if fri_entity == None:
			return

		# 修改在线好友
		if fri_status == True:
			if fri_entity not in self.OnlineFriends:
				self.OnlineFriends.append(fri_entity)
		elif fri_status == False:
			if fri_entity not in self.OnlineFriends:
				return
			self.OnlineFriends.remove(fri_entity)

	def matchSuccess(self, ballteSpace, index):
		DEBUG_MSG("Account[%i].matchSuccess battleSpace[%i]" % (self.id, ballteSpace.id))
		self.track = index
		#方向为弧度 用角度换算弧度
		params = {
			"Account": self,
			"BattleFieldCell": ballteSpace.cell,
			"name": self.Name,
			"dbid": self.databaseID,
			"position": BattleSpacePosConfigs.getInitAvatarPos(index),
			"direction": [0, (index * 72 / 360) * (2 * math.pi), 0],
			"track": index,
		}

		#回调函数
		def callBack(result):
			if result == None:
				ERROR_MSG(result)
			else:
				DEBUG_MSG("Create Avatar[%i] Success" % result.id)
				ballteSpace.baseAvatars[result.id] = result
				self.battleSpace = ballteSpace
				self.client.enterBattleSpace()
				self.Avatar = result

		KBEngine.createEntityAnywhere("Avatar", params, callBack)

	def reqGiveClientToAvatar(self):
		DEBUG_MSG("Account[%i]: reqGiveClientToAvatar" % self.id)
		self.giveClientTo(self.Avatar)
		self.battleSpace.StartGame()
		self.battleSpace.cell.InitGold(self.track)



