# -*- coding: utf-8 -*-
import KBEngine
import math
import time
import random
from KBEDebug import *

class Motion:

    def __init__(self):

        pass

    def stopMotion(self):
        """
        停止移动
        """
        # if self.isMoving:
        #     self.cancelController("Movement")
        #     self.isMoving = False
        self.moveSpeed = 0.0

    def gotoPosition(self, position, speed = 0.0, dist = 0.0):

        DEBUG_MSG("Motion::gotoPosition.%i:isMoving = %i,position(%f,%f,%f),dist=%f,speed=%f" %
            (self.id,self.isMoving,position.x,position.y,position.z,dist,speed))

        if self.isMoving:
            self.stopMotion()

        self.isMoving = True

        if speed == 0.0:
            speed = self.moveSpeed
        DEBUG_MSG("speed : %i" % speed)

        self.moveToPoint(position, speed, dist, None, False, False)


    # --------------------------------------------------------------------------------------------
    #                              Callbacks
    # --------------------------------------------------------------------------------------------

    def onMove(self, controllerId, userarg):
        """
        KBEngine method.
        使用引擎的任何移动相关接口， 在entity一次移动完成时均会调用此接口
        """
        #DEBUG_MSG("%s::onMove: %i controllerId =%i, userarg=%s" % \
        #               (self.getScriptName(), self.id, controllerId, userarg))
        DEBUG_MSG("OnMoveFinish: %i" % controllerId)
        self.isMoving = True

    def onMoveFailure(self, controllerId, userarg):
        """
        KBEngine method.
        使用引擎的任何移动相关接口， 在entity一次移动完成时均会调用此接口
        """
        DEBUG_MSG("%s::onMoveFailure: %i controllerId =%i, userarg=%s" % \
                        (self.getScriptName(), self.id, controllerId, userarg))

        self.isMoving = False

    def onMoveOver(self, controllerId, userarg):
        """
        KBEngine method.
        使用引擎的任何移动相关接口， 在entity移动结束时均会调用此接口
        """
        self.isMoving = False