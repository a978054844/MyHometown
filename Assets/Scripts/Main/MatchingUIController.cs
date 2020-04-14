using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KBEngine;

public class MatchingUIController : MonoBehaviour {

    float cur_duration = 0;

    private void OnEnable()
    {
        transform.Find("Text").GetComponent<Text>().text = "00 : 00 : 00";
    }

    private void Update()
    {
        cur_duration += Time.deltaTime;
        UpdateMatchingTime();
    }

    void UpdateMatchingTime() {
        int allTime = (int)cur_duration;
        string seconds = string.Format("{0:00}", (allTime % 60));
        string minutes = string.Format("{0:00}", (allTime / 60 % 60));
        string hours = string.Format("{0:00}", (allTime / (60 * 60)));
        
        transform.Find("Text").GetComponent<Text>().text = hours + " : " + minutes + " : " + seconds;
    }

    public void StopButtonEvent()
    {
        Account account = KBEngineApp.app.player() as Account;
        account.baseCall("reqStopMatch");
    }
}
