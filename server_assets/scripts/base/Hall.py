
# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *

import Math

class Hall(KBEngine.Entity):
    def __init__(self):
        DEBUG_MSG('Hall Init')
        KBEngine.Entity.__init__(self)

        #存储全局变量大厅
        KBEngine.globalData["Hall"] = self

        #初始化正在匹配所有玩家字典  最多为五人
        self.MatchingPlayerDict = {1 : [], 2 : [], 3 : [], 4 : [], 5 : []}

        #初始化正在匹配的玩家列表
        self.MatchingPlayerList = []

        #添加定时器
        self.addTimer(10, 5, 0)

        self.maxNum = 2

    def onTimer(self, id, userArg):
        """
        KBEngine method.
        使用addTimer后， 当时间到达则该接口被调用
        @param id		: addTimer 的返回值ID
        @param userArg	: addTimer 最后一个参数所给入的数据
        """
        self.matchRule()

    def reqAddMatch(self, matchList):
        self.MatchingPlayerDict[len(matchList)].append(matchList)
        self.MatchingPlayerList.append(matchList)

        nameStr = ""
        for player in matchList:
            nameStr += player.Name + " | "
        DEBUG_MSG("Hall[%i].reqAddMatch : %s" % (self.id, nameStr))

    def reqDelMatch(self, matchList):
        self.MatchingPlayerDict[len(matchList)].remove(matchList)
        self.MatchingPlayerList.remove(matchList)

        nameStr = ""
        for player in matchList:
            nameStr += player.Name + " | "
        DEBUG_MSG("Hall[%i].reqDelMatch : %s" % (self.id, nameStr))

    def matchRule(self):
        #DEBUG_MSG("Hall[%i].matchRule : 检测匹配" % (self.id))
        if len(self.MatchingPlayerList) == 0:
            return
        curLen = len(self.MatchingPlayerList[0])
        needLen = self.maxNum - curLen
        if needLen >= self.maxNum:
            return
        #没有switch 暴毙
        if needLen == 4:
            self.matchNeedFour()
        elif needLen == 3:
            self.matchNeedThree()
        elif  needLen == 2:
            self.matchNeedTwo()
        elif needLen == 1:
            self.matchNeedOne()
        elif needLen == 0:
            self.matchNeedZero()

    def matchNeedFour(self):
        cur_players = self.MatchingPlayerList[0]
        if len(self.MatchingPlayerDict[4]) > 0:  # 1 + 4
            needList1 = self.MatchingPlayerDict[4][0]
            self.matchSuccess([needList1[0], needList1[1], needList1[2], needList1[3], cur_players[0]])
            self.reqDelMatch(needList1)
            self.reqDelMatch(cur_players)
        elif len(self.MatchingPlayerDict[3]) > 0 and len(self.MatchingPlayerDict[1]) > 1:  # 1 + 3 + 1
            needList1 = self.MatchingPlayerDict[3][0]
            needList2 = self.MatchingPlayerDict[1][1]
            self.matchSuccess([needList1[0], needList1[1], needList1[2], needList2[0], cur_players[0]])
            self.reqDelMatch(needList1)
            self.reqDelMatch(needList2)
            self.reqDelMatch(cur_players)
        elif len(self.MatchingPlayerDict[2]) > 1:  # 1 + 2 + 2
            needList1 = self.MatchingPlayerDict[2][0]
            needList2 = self.MatchingPlayerDict[2][1]
            self.matchSuccess([needList1[0], needList1[1], needList2[0], needList2[1], cur_players[0]])
            self.reqDelMatch(needList1)
            self.reqDelMatch(needList2)
            self.reqDelMatch(cur_players)
        elif len(self.MatchingPlayerDict[2]) > 0 and len(self.MatchingPlayerDict[1]) > 2:  # 1 + 2 + 1 + 1
            needList1 = self.MatchingPlayerDict[2][0]
            needList2 = self.MatchingPlayerDict[1][1]
            needList3 = self.MatchingPlayerDict[1][2]
            self.matchSuccess([needList1[0], needList1[1], needList2[0], needList3[0], cur_players[0]])
            self.reqDelMatch(needList1)
            self.reqDelMatch(needList2)
            self.reqDelMatch(needList3)
            self.reqDelMatch(cur_players)
        elif len(self.MatchingPlayerDict[1]) > 4:  # 1 + 1 + 1 + 1 + 1
            needList1 = self.MatchingPlayerDict[1][1]
            needList2 = self.MatchingPlayerDict[1][2]
            needList3 = self.MatchingPlayerDict[1][3]
            needList4 = self.MatchingPlayerDict[1][4]
            self.matchSuccess([needList1[0], needList2[0], needList3[0], needList4[0], cur_players[0]])
            self.reqDelMatch(needList1)
            self.reqDelMatch(needList2)
            self.reqDelMatch(needList3)
            self.reqDelMatch(needList4)
            self.reqDelMatch(cur_players)

    def matchNeedThree(self):
        cur_players = self.MatchingPlayerList[0]
        if len(self.MatchingPlayerDict[3]) > 0: # 2 + 3
            needList1 = self.MatchingPlayerDict[3][0]
            self.matchSuccess([needList1[0], needList1[1], needList1[2], cur_players[0], cur_players[1]])
            self.reqDelMatch(needList1)
            self.reqDelMatch(cur_players)
        elif len(self.MatchingPlayerDict[2]) > 1 and len(self.MatchingPlayerDict[1]) > 0: # 2 + 2 + 1
            needList1 = self.MatchingPlayerDict[2][1]
            needList2 = self.MatchingPlayerDict[1][0]
            self.matchSuccess([needList1[0], needList1[1], needList2[0], cur_players[0], cur_players[1]])
            self.reqDelMatch(needList1)
            self.reqDelMatch(needList2)
            self.reqDelMatch(cur_players)
        elif len(self.MatchingPlayerDict[1]) > 2: # 2 + 1 + 1 + 1
            needList1 = self.MatchingPlayerDict[1][0]
            needList2 = self.MatchingPlayerDict[1][1]
            needList3 = self.MatchingPlayerDict[1][2]
            self.matchSuccess([needList1[0], needList2[0], needList3[0], cur_players[0], cur_players[1]])
            self.reqDelMatch(needList1)
            self.reqDelMatch(needList2)
            self.reqDelMatch(needList3)
            self.reqDelMatch(cur_players)

    def matchNeedTwo(self):
        cur_players = self.MatchingPlayerList[0]

        # if len(self.MatchingPlayerDict[2]) > 0: # 3 + 2
        #     needList1 = self.MatchingPlayerDict[2][0]
        #     self.matchSuccess([needList1[0], needList1[1], cur_players[0], cur_players[1], cur_players[2]])
        #     self.reqDelMatch(needList1)
        #     self.reqDelMatch(cur_players)
        # elif len(self.MatchingPlayerDict[1]) > 1: # 3 + 1 + 1
        #     needList1 = self.MatchingPlayerDict[1][0]
        #     needList2 = self.MatchingPlayerDict[1][1]
        #     self.matchSuccess([needList1[0], needList2[0], cur_players[0], cur_players[1], cur_players[2]])
        #     self.reqDelMatch(needList1)
        #     self.reqDelMatch(needList2)
        #     self.reqDelMatch(cur_players)

        #测试使用
        if len(self.MatchingPlayerDict[2]) > 0: # 1 + 2
            needList1 = self.MatchingPlayerDict[2][0]
            self.matchSuccess([needList1[0], needList1[1], cur_players[0]])
            self.reqDelMatch(needList1)
            self.reqDelMatch(cur_players)
        elif len(self.MatchingPlayerDict[1]) > 2: # 1 + 1 + 1
            needList1 = self.MatchingPlayerDict[1][1]
            needList2 = self.MatchingPlayerDict[1][2]
            self.matchSuccess([needList1[0], needList2[0], cur_players[0]])
            self.reqDelMatch(needList1)
            self.reqDelMatch(needList2)
            self.reqDelMatch(cur_players)

    def matchNeedOne(self):
        cur_players = self.MatchingPlayerList[0]

        # if len(self.MatchingPlayerDict[1]) > 0:  # 3 + 1 + 1
        #     needList1 = self.MatchingPlayerDict[1][0]
        #     self.matchSuccess([needList1[0], cur_players[0], cur_players[1], cur_players[2], cur_players[3]])
        #     self.reqDelMatch(needList1)
        #     self.reqDelMatch(cur_players)

        # 测试使用
        if len(self.MatchingPlayerDict[1]) > 1:  # 1 + 1
            needList1 = self.MatchingPlayerDict[1][1]
            self.matchSuccess([needList1[0], cur_players[0]])
            self.reqDelMatch(needList1)
            self.reqDelMatch(cur_players)

    def matchNeedZero(self):

        cur_players = self.MatchingPlayerList[0]

        self.matchSuccess(cur_players)
        self.reqDelMatch(cur_players)

    def matchSuccess(self, players):
        # DEBUG_MSG("Hall[%i].matchSuccess : teamPlayerName [%s]  [%s]  [%s]  [%s]  [%s]"
        #           % (self.id, players[0].Name, players[1].Name, players[2].Name, players[3].Name, players[4].Name))

        DEBUG_MSG("Hall[%i].matchSuccess : teamPlayerName [%s]  [%s]"
                   % (self.id, players[0].Name, players[1].Name))

        params = {
            "AccountList": players,
        }
        battleSpace = KBEngine.createEntityAnywhere("BattleSpace", params)
