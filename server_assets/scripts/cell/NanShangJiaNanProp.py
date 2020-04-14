# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
from interfaces.Motion import Motion
from interfaces.GameObject import GameObject
import Math

class NanShangJiaNanProp(KBEngine.Entity, GameObject):
	def __init__(self):
		DEBUG_MSG("NanShangJiaNanProp[%i].cell::__init__" % self.id)
		KBEngine.Entity.__init__(self)
		GameObject.__init__(self)

		self.slowSpeedRatio = 0.5

	# --------------------------------------------------------------------------------------------
	#                              Callbacks
	# --------------------------------------------------------------------------------------------

	def onWitnessed( self, isWitnessed ):
		DEBUG_MSG("NanShangJiaNanProp[%i] :: onWitnessed[%i]" % (self.id, isWitnessed))
		if isWitnessed:
			self.addProximity(3, 1, 0)

	def onEnterTrap(self, entityEntering, rangeXZ, rangeY, controllerID, userArg=0):
		DEBUG_MSG("%s::onEnterTrap:%i  entityEntering::%s[%i]" %
				  (self.className, self.id, entityEntering.className, entityEntering.id))
		if entityEntering.className == "Avatar":
			entityEntering.moveSpeed *= self.slowSpeedRatio

	def onLeaveTrap( self, entity, rangeXZ, rangeY, controllerID, userArg ):
		DEBUG_MSG("%s::onLeaveTrap:%i  entityLeaveing::%s[%i]" %
				  (self.className, self.id, entity.className, entity.id))
		if entity.className == "Avatar":
			entity.moveSpeed *= (1 / self.slowSpeedRatio)
			self.destroy()