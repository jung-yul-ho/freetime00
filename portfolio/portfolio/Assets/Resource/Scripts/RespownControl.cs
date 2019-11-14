using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RespownPoint
{
    public Transform EnermyRespown;
    public bool RespownPointUsingCheck;
}

public class RespownControl : MonoBehaviour
{
    public List<Transform> PlayerRespown;
    public List<RespownPoint> EnermyRespown;

    static RespownControl instance;
    public static RespownControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RespownControl>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<RespownControl>();
                }
            }
            return instance;
        }
    }
}
