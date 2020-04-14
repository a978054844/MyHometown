# -*- coding: utf-8 -*-

import Math
import math
import random

GoldNum = 10

def getInitAvatarPos(index):
    if index == 0:
        return [0, 0.5, -109.02]
    elif index == 1:
        return [-103.68, 0.5, -33.69]
    elif index == 2:
        return [-64.08, 0.5, 88.2]
    elif index == 3:
        return [64.08, 0.5, 88.2]
    elif index == 4:
        return [103.68, 0.5, -33.69]

def getInitGoldPos(index):
    initPos = Math.Vector3(getInitAvatarPos(index))
    butterPosNum = 3
    allPos = []
    oneGoldVec = (Math.Vector3(0, initPos.y, 0) - initPos).scale(1 / (GoldNum + butterPosNum))
    horizontalVec = getRotateVector3WithY(Math.Vector3(1, 0, 0), 72 * index)
    horizontalVec.normalise()
    for i in range(0, GoldNum):
        normalPos = initPos + oneGoldVec.scale(i + 1)
        normalPos += horizontalVec.scale((random.random() * 2 - 1) * 4)
        allPos.append(normalPos)
    return allPos

def getRotateVector3WithY(originalVec, angle):
    getRadians = math.radians(angle)
    #需要的为左手坐标系
    #需要将y 和 z互换  x取反
    #进行右手坐标系的z轴旋转
    #最后将y 和 z换回  x取反
    converVec = Math.Vector3(-originalVec.x, originalVec.z, originalVec.y)
    # getX = originalVec.x * math.cos(getRadians) + 0 - originalVec.x * math.sin(getRadians)
    # getY = originalVec.y
    # getZ = originalVec.z * math.sin(getRadians) + 0 + originalVec.z * math.cos(getRadians)

    getX = converVec.x * math.cos(getRadians) - converVec.y * math.sin(getRadians) + 0
    getY = converVec.x * math.sin(getRadians) + converVec.y * math.cos(getRadians) + 0
    getZ = 0 + 0 + converVec.z * 1
    return Math.Vector3(-getX, getZ, getY)