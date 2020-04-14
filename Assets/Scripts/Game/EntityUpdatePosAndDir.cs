using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityUpdatePosAndDir : MonoBehaviour {

    KBEngine.Entity _keepEntity;
    public KBEngine.Entity KeepEntity{
        get { return _keepEntity; }
        set {_keepEntity = value; }
    }

    public bool isUpdate = true;

	// Update is called once per frame
	void Update () {
        if (!isUpdate)
            return;
        if (_keepEntity == null)
            return;
        UpdatePosAndDir();
	}

    void UpdatePosAndDir() {
        if (_keepEntity.position != transform.position)
        {
            transform.position = _keepEntity.position;
        }
        if (_keepEntity.direction != transform.eulerAngles) {
            transform.eulerAngles = _keepEntity.direction;
        }

    }
}
