
# -*- coding: utf-8 -*-
import time

import KBEngine
from KBEDebug import *


class BattleSpace(KBEngine.Entity):
	def __init__(self):
		DEBUG_MSG("BattleSpace:[%s] init" % self.id)
		KBEngine.Entity.__init__(self)

		self.createCellEntityInNewSpace(None)
		self.changeSceneNum = 0
		self.isStarting = False
		self.getClientNum = 0
		self.secondNum = 5


	def StartGame(self):
		"""
		开始游戏数秒
		:return:
		"""

		self.getClientNum += 1
		DEBUG_MSG("BattleSpace[%i].base::StartGame : getClientNum: %i" % (self.id, self.getClientNum))
		if self.getClientNum < len(self.AccountList):
			return
		self.addTimer(3, 1, 0)

	def ReturnClient(self):
		'''
		归还客户端
		:return:
		'''
		self.getClientNum -= 1
		DEBUG_MSG("BattleSpace[%i].base::ReturnClient : getClientNum: %i" % (self.id, self.getClientNum))
		if self.getClientNum == 0:
			for avatar in self.baseAvatars.values():
				avatar.destroySelf()
			if self.cell != None:
				self.destroyCellEntity()
			else:
				self.destroy()

	# --------------------------------------------------------------------------------------------
	#                              Call Back
	# --------------------------------------------------------------------------------------------

	def onGetCell(self):
		"""
		entity的cell部分被创建成功
		"""
		DEBUG_MSG("BattleSpace[%i]::onGetCell" % self.id)
		index = 0
		for player in self.AccountList:
			player.matchSuccess(self, index)
			index += 1

	def onTimer( self, timerHandle, userData ):

		DEBUG_MSG("BattleSpace[%i] : secondNum : %i" % (self.id, self.secondNum))
		for cur_avatar in self.baseAvatars.values():
			if cur_avatar.hasClient:
				cur_avatar.client.startGameSecond(self.secondNum)

		if self.secondNum == 0:
			self.delTimer(timerHandle)
			self.isStarting = True

		self.secondNum -= 1

	def onLoseCell( self ):
		DEBUG_MSG("BattleSpace[%i].base::onLoseCell" % self.id)
		self.destroy(True, False)





