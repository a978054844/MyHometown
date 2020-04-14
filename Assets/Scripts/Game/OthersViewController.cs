using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OthersViewController : MonoBehaviour {

    private Transform _followTran;
    public Transform FollowTran {
        get { return _followTran; }
        set {
            _followTran = value;
            ChangeParent();
        }
    }


    void ChangeParent() {
        
        transform.SetParent(_followTran);
        transform.localPosition = _followTran.Find("Original").localPosition;
        transform.localEulerAngles = _followTran.Find("Original").localEulerAngles;
        if (!GetComponent<Camera>().enabled)
            GetComponent<Camera>().enabled = true;
    }

}
