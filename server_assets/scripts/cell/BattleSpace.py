# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
import BattleSpacePosConfigs
import Math
import math
import random

TRACKGOLDVALUE = 20

class BattleSpace(KBEngine.Entity):
	def __init__(self):
		KBEngine.Entity.__init__(self)
		self.isFinished = False


	def InitGold(self, index):
		allPos = BattleSpacePosConfigs.getInitGoldPos(index)
		curValue = 0
		isMin = False
		curTrackGoldValue = TRACKGOLDVALUE
		for i in range(0, int(BattleSpacePosConfigs.GoldNum)):
			if isMin:
				curValue = 1
			else:
				curValue = random.randint(1, 3)
				if (curTrackGoldValue - curValue) == (BattleSpacePosConfigs.GoldNum - (i + 1)):
					isMin = True
				elif (curTrackGoldValue - curValue) + 1 == (BattleSpacePosConfigs.GoldNum - (i + 1)):
					curValue -= 1
					isMin = True
			curTrackGoldValue -= curValue
			KBEngine.createEntity('Gold', self.spaceID, allPos[i], (0.0, math.radians(72 * index) * 1.0, 0.0),
								  {"goldValue" : curValue})
			DEBUG_MSG("InitGold | pos : (%f, %f, %f)  index : %f" % (allPos[i].x, allPos[i].y, allPos[i].z, index))