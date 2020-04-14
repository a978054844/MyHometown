using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsController : MonoBehaviour {

    

    // Use this for initialization
    void Start() {
        TipsOperation.TipsTr = transform;
        transform.Find("Text").GetComponent<Text>().color = new Color(0, 0, 0, 0);
        transform.Find("BG").GetComponent<Image>().color = new Color(1, 1, 1, 0);

        KBEngine.Event.registerOut("reqMessageCall", this, "Event_reqMessageCall");
    }

    public void Event_reqMessageCall(string tips) {
        TipsOperation.ShowTips(tips);
    }


}

public class TipsOperation {

    public static Transform TipsTr;

    public static void ShowTips(string tips)
    {
        TipsTr.SetAsLastSibling();
        TipsTr.Find("Text").GetComponent<Text>().color = new Color(0, 0, 0, 1);
        TipsTr.Find("BG").GetComponent<Image>().color = new Color(1, 1, 1, 1);
        TipsTr.Find("Text").GetComponent<Text>().text = tips;
        iTween.ColorTo(TipsTr.Find("Text").gameObject, iTween.Hash("color", new Color(0, 0, 0, 0), "time", 2f,
            "easetype", iTween.EaseType.linear));
        iTween.ColorTo(TipsTr.Find("BG").gameObject, iTween.Hash("color", new Color(1, 1, 1, 0), "time", 2f,
            "easetype", iTween.EaseType.linear));

    }
}
