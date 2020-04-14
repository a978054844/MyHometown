using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LongPress : MonoBehaviour {

    public float pressTime = 1f;

    [Serializable]
    public class longPressEvent : UnityEvent { }
    //[FormerlySerializedAs("MyEvent")]
    [SerializeField]
    private longPressEvent myEvent = new longPressEvent();

    public longPressEvent MyEvent { get { return myEvent; } set { myEvent = value; } }



    public void StartPress() {

    }


    private void OnEnable()
    {
        keepTime = 0;
    }

    float keepTime = 0f;
    private void Update()
    {
        keepTime += Time.deltaTime;
        if (keepTime >= pressTime)
        {
            MyEvent.Invoke();
            enabled = false;
        }
    }
}
