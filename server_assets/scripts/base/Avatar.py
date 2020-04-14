
# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *

class Avatar(KBEngine.Proxy):
	def __init__(self):
		KBEngine.Proxy.__init__(self)

		DEBUG_MSG("Avatar[%i].base::__init__" % self.id)

		self.ballteSpaceCell = self.cellData['BattleFieldCell']

	def reqBackToMain(self, isContinue):
		DEBUG_MSG("Avatar[%i].base::reqBackToMain  isContinue: %i" % (self.id, isContinue))
		self.Account.isContinueRoom = isContinue
		self.Account.readyGetClient = True
		self.giveClientTo(self.Account)

	def destroySelf(self):
		DEBUG_MSG("Avatar[%i].base::destroySelf" % self.id)
		if self.cell != None:
			self.destroyCellEntity()
		else:
			self.destroy()

	# --------------------------------------------------------------------------------------------
	#                              Call Back
	# --------------------------------------------------------------------------------------------
		
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
		INFO_MSG("Avatar[%i] entities enable. entityCall:%s" % (self.id, self.client))
		DEBUG_MSG("Avatar[%i].base::onGetClient" % self.id)
		self.createCellEntity(self.ballteSpaceCell)
			
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
		DEBUG_MSG("Avatar[%i].onClientDeath:" % self.id)



	def onLoseCell(self):
		DEBUG_MSG("Avatar[%i].base::onLoseCell" % self.id)
		self.destroy()



