# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
from interfaces.Motion import Motion
from interfaces.GameObject import GameObject
import Math

class XiaCiYiDingProp(KBEngine.Entity, GameObject):
	def __init__(self):
		DEBUG_MSG("XiaCiYiDingProp[%i].cell::__init__" % self.id)
		KBEngine.Entity.__init__(self)
		GameObject.__init__(self)
		self.keepTime = 5
		self.destroyTimerId = self.addTimer(self.keepTime, 100, 0)
		self.updateTimerId = self.addTimer(0, 0.1, 0)

	# --------------------------------------------------------------------------------------------
	#                              Callbacks
	# --------------------------------------------------------------------------------------------

	def onWitnessed( self, isWitnessed ):
		DEBUG_MSG("XiaCiYiDingProp[%i] :: onWitnessed[%i]" % (self.id, isWitnessed))
		if isWitnessed:
			self.addProximity(2, 1, 0)

	def onEnterTrap(self, entityEntering, rangeXZ, rangeY, controllerID, userArg=0):
		DEBUG_MSG("%s::onEnterTrap:%i  entityEntering::%s[%i]" %
				  (self.className, self.id, entityEntering.className, entityEntering.id))
		if entityEntering.className == "Gold":
			entityEntering.destroy()

	def onTimer( self, timerHandle, userData ):
		DEBUG_MSG("XiaCiYiDingProp.pos(%f, %f, %f)" %(self.position.x, self.position.y, self.position.z))
		if timerHandle == self.updateTimerId:
			oneVec = Math.Vector3(0.0, 0.0, 0.0) - self.followAvatarCell.position
			oneVec.normalise()
			self.position = self.followAvatarCell.position + oneVec.scale(2)
		if timerHandle == self.destroyTimerId:
			self.delTimer(self.updateTimerId)
			self.delTimer(timerHandle)
			self.destroy()