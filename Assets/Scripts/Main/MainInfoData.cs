using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class MainInfoData {

    private static MainInfoData mainInfoData;
    private string _name;
    private Sprite _headIcon;
    private bool _inRoom;
    //dbid对应所有聊天信息
    private Dictionary<ulong, List<string>> _chattingRecords;
    //聊天dbid对应名字
    private Dictionary<ulong, string> _chattingDBIDtoName;

    private MainInfoData() {
        
        _chattingRecords = new Dictionary<ulong, List<string>>();
        _chattingDBIDtoName = new Dictionary<ulong, string>();
    }

    public static MainInfoData GetInstance() {

        if (mainInfoData == null)
            mainInfoData = new MainInfoData();
        
        return mainInfoData;
    }

    public string Name {
        get {return _name; }
        set {_name = value; }
    }

    public Sprite HeadIcon {
        get { return _headIcon; }
        set { _headIcon = value; }
    }

    public bool InRoom { get { return _inRoom; } set { _inRoom = value; } }

    public List<string> GetAllRecordsWithDBID(ulong DBID) {
        if (!_chattingRecords.ContainsKey(DBID))
            _chattingRecords.Add(DBID, new List<string>());
        return _chattingRecords[DBID];
    }

    public void SetAllRecordsWithDBID(ulong DBID, List<string> allRecords)
    {
        
        if (!_chattingRecords.ContainsKey(DBID))
            _chattingRecords.Add(DBID, allRecords);
        else
            _chattingRecords[DBID] = allRecords;
    }

    public string GetChattingNameToDBID(ulong DBID) {
        if (!_chattingDBIDtoName.ContainsKey(DBID))
        {
            Account account = KBEngineApp.app.player() as Account;
            foreach (FRIEND_INFO info in account.Friend_list)
                if (info.dbid == DBID)
                {
                    _chattingDBIDtoName.Add(DBID, info.name);
                    break;
                }

        }
        return _chattingDBIDtoName[DBID];
    }

    public void SetChattingNameToDBID(ulong DBID, string name)
    {
        if (!_chattingDBIDtoName.ContainsKey(DBID))
            _chattingDBIDtoName.Add(DBID, name);
        else
            _chattingDBIDtoName[DBID] = name;
    }

    public void UpdateOnceData() {
        UpdateData();
    }

    public delegate void UpdateDataHandler();
    public event UpdateDataHandler UpdateData;
}
