# -*- coding: utf-8 -*-
import KBEngine
import Math
import math
import BattleSpacePosConfigs

from KBEDebug import *
from interfaces.Motion import Motion
from interfaces.GameObject import GameObject
from interfaces.Animation import Animation
from interfaces.Skills import Skills


class Avatar(KBEngine.Entity, Motion, GameObject, Animation, Skills):
	def __init__(self):
		DEBUG_MSG("Avatar[%i].cell::__init__" % self.id)
		KBEngine.Entity.__init__(self)
		Motion.__init__(self)
		GameObject.__init__(self)

		self.setViewRadius(200, 5)
		self.jiaSuTimerId = []
		self.BattleFieldCell.cellAvatars[self.id] = self

		if self.name == "t1":
			self.keepJiaSuTime = 10

	# --------------------------------------------------------------------------------------------
	#                              Skills
	# --------------------------------------------------------------------------------------------

	def jiaSuSkill(self):
		'''
		重写加速技能
		'''
		if self.BattleFieldCell.isFinished == True:
			return
		Skills.jiaSuSkill(self)
		if self.goldNum >= self.jiaSuGold:
			self.goldNum -= self.jiaSuGold
		else:
			DEBUG_MSG("Avatar[%i] :: jiaSuSkill : 金币(%i)不足" % (self.id, self.goldNum))
			return
		self.moveSpeed += 1
		self.jiaSuTimerId.append(self.addTimer(self.keepJiaSuTime, 100, 0))
		DEBUG_MSG("Avatar[%i] :: jiaSuSkill : 当前速度为%f" % (self.id, self.moveSpeed))

	def jianSu(self, timerId):
		if self.BattleFieldCell.isFinished == False:
			self.moveSpeed -= 1
		self.jiaSuTimerId.remove(timerId)
		self.delTimer(timerId)
		DEBUG_MSG("Avatar[%i] :: jianSu : 当前速度为%f" % (self.id, self.moveSpeed))

	def anYeYiYangSkill(self, entityId):
		'''
		重写俺也一样技能
		'''
		if self.BattleFieldCell.isFinished == True:
			return
		Skills.anYeYiYangSkill(self, entityId)
		if self.goldNum >= self.anYeYiYangGold:
			self.goldNum -= self.anYeYiYangGold
		else:
			DEBUG_MSG("Avatar[%i] :: anYeYiYangSkill : 金币(%i)不足" % (self.id, self.goldNum))
			return

		followAvatarCell = self.BattleFieldCell.cellAvatars[entityId]
		distance = followAvatarCell.position.distTo(Math.Vector3(0.0, self.position.y, 0.0))
		direVec = BattleSpacePosConfigs.getRotateVector3WithY(Math.Vector3(0, 0, -1), 72 * self.track)
		direVec.normalise()
		finalPos = Math.Vector3(0.0, 0.5, 0.0) + direVec.scale(distance)
		self.client.jumpToPosition(finalPos)

		DEBUG_MSG("Avatar[%i] :: anYeYiYangSkill : 计算后的位置为(%f, %f, %f)" %
				  (self.id, finalPos.x, finalPos.y, finalPos.z))

	def xiaCiYiDingSkill(self, entityId):
		'''
		重写下次一定技能
		'''
		if self.BattleFieldCell.isFinished == True:
			return
		Skills.xiaCiYiDingSkill(self, entityId)
		if self.goldNum >= self.xiaCiYiDingGold:
			self.goldNum -= self.xiaCiYiDingGold
		else:
			DEBUG_MSG("Avatar[%i] :: xiaCiYiDingSkill : 金币(%i)不足" % (self.id, self.goldNum))
			return
		acceptAvatarCell = self.BattleFieldCell.cellAvatars[entityId]
		params = {
			"launchID": self.id,
			"acceptID": acceptAvatarCell.id,
			"followAvatarCell": acceptAvatarCell,
		}
		KBEngine.createEntity('XiaCiYiDingProp', self.BattleFieldCell.spaceID, (0.0, 0.0, 0.0),
							  acceptAvatarCell.direction, params)


	def nanShangJiaNanSkill(self, pos, track):
		'''
		重写男上加男技能
		'''
		if self.BattleFieldCell.isFinished == True:
			return
		Skills.nanShangJiaNanSkill(self, pos, track)
		if self.goldNum >= self.nanShangJiaNanGold:
			self.goldNum -= self.nanShangJiaNanGold
		else:
			DEBUG_MSG("Avatar[%i] :: nanShangJiaNanSkill : 金币(%i)不足" % (self.id, self.goldNum))
			return

		params = {
			"launchID": self.id,
			"track": track,
		}
		params.values()
		KBEngine.createEntity('NanShangJiaNanProp', self.BattleFieldCell.spaceID, pos,
							  (0.0, math.radians(72 * track) * 1.0, 0.0), params)

	def reqArriveSuccess(self):

		if self.BattleFieldCell.isFinished == False:
			self.BattleFieldCell.isFinished = True
			DEBUG_MSG("Avatar[%i] :: reqArriveSuccess : 成功的人为 : %s" % (self.id, self.name))
			for avatarCell in self.BattleFieldCell.cellAvatars.values():
				avatarCell.stopMotion()
				if avatarCell == self:
					avatarCell.client.ArriveSuccess()
				else:
					avatarCell.client.ArriveDefeat(self.id)
		else:
			return


	# --------------------------------------------------------------------------------------------
	#                              Callbacks
	# --------------------------------------------------------------------------------------------

	def onGetWitness( self ):
		DEBUG_MSG("AvatarCell[%i] :: onGetWitness" % self.id)
		self.addProximity(1, 1, 0)

	def onEnterTrap(self, entityEntering, rangeXZ, rangeY, controllerID, userArg=0):
		DEBUG_MSG("%s::onEnterTrap:%i  entityEntering::%s[%i]" %
				  (self.className, self.id, entityEntering.className, entityEntering.id))
		if entityEntering.className == "Gold":
			self.goldNum += entityEntering.goldValue
			DEBUG_MSG("Name : %s  的金币变化为 %i" % (self.name, self.goldNum))
			entityEntering.destroy()

	def onLeaveTrap( self, entity, rangeXZ, rangeY, controllerID, userArg ):

		pass

	def onTimer( self, timerHandle, userData ):
		if timerHandle in self.jiaSuTimerId:
			self.jianSu(timerHandle)

