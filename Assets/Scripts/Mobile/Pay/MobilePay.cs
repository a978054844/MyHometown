using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PayInfo
{
    public string subject;  // 显示在按钮上的内容,跟支付无关系
    public float money;     // 商品价钱
    public string title;    // 商品描述
}


public class MobilePay : MonoBehaviour {

    #region 支付宝_Android支付
    AndroidJavaObject jo;

    void Start () {
        if (Application.platform == RuntimePlatform.Android)
            InitAndroid();
	}

    private void InitAndroid() {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
    }


    public void Alipay() {
        try
        {
            jo.Call("Pay", 0.01f, "测试的支付宝支付");
        }
        catch (System.Exception ex)
        {
            GUI.Label(new Rect(50, 50, 500, 500), ex.ToString());
        }
    }
    #endregion
}
