# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *


class Animation:

    def __init__(self):
        pass

    def changeAnimState(self, animName):
        DEBUG_MSG("changeAnimState : %s" % animName)
        self.animState = animName