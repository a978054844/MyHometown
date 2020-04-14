using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayOperation {

    public enum ReturnType {
        GameObject,
        Position,
    }

    public static object LaunchRayOfScreen(Vector2 screenPoint, ReturnType returnType) {

        
        if (Camera.main == null)
        {
            Debug.LogWarning("主相机无法找到");
            return null;
        }
        else
            return LaunchRayOfScreen(screenPoint, Camera.main, returnType);
    }

    public static object LaunchRayOfScreen(Vector2 screenPoint, Camera cur_Camera, ReturnType returnType) {
        Ray ray;
        ray = cur_Camera.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        
        bool isHit = Physics.Raycast(ray, out hit);

        if (isHit)
        {
            switch (returnType) {
                case ReturnType.GameObject:
                    GameObject rayGO = hit.collider.gameObject;
                    Debug.Log("LaunchRayOfScreen : " + rayGO.name);
                    return rayGO;
                case ReturnType.Position:
                    Vector3 pos = hit.point;
                    Debug.Log("LaunchRayOfScreen : " + pos);
                    return pos;
            }
            
        }
        
        return null;
    }
}
