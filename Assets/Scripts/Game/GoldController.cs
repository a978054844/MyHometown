using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class GoldController : MonoBehaviour {

    private KBEngine.Gold _keepGold;

    public KBEngine.Gold KeepGold {
        set {
            _keepGold = value;
        }

    }

}
