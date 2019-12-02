
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class RandomMap
    {
        static List<Vector3> mapList;
        public static List<Vector3> MapList {
            get { return mapList; }
        }


        static List<Vector3> RandomMapList() {
            Vector3 pos_1 = GetBackVec(1);
            Vector3 pos_3 = GetFrontVec(3);
            Vector3 pos_5 = GetBackVec(5);
            Vector3 pos_8 = GetBackVec(8);
            
            InitMapVec(ref mapList);
            return null;
        }

        static Vector3 GetFrontVec(int num_y) {
            int x = Random.Range(1, 3) * 10;
            return new Vector3(x, num_y, 0);
        }

        static Vector3 GetBackVec(int num_y) {
            int x = Random.Range(7, 9) * 10;
            return new Vector3(x, num_y, 0);
        }

        static void InitMapVec(ref List<Vector3> needList) {
            if (needList == null)
                needList = new List<Vector3>();
            else
                needList.Clear();
            for (int x = 0; x < 100; x *= 10) {
                for (int y = 0; y < 100; y *= 10) {
                    
                }
            }
        }
    }
}
