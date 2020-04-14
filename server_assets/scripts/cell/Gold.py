# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
from interfaces.Motion import Motion
from interfaces.GameObject import GameObject

class Gold(KBEngine.Entity, GameObject):
	def __init__(self):
		DEBUG_MSG("Gold[%i].cell::__init__" % self.id)
		KBEngine.Entity.__init__(self)
		GameObject.__init__(self)
