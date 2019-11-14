using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public bool Open;

    static MonoBehaviour instance;
    public static MonoBehaviour Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MonoBehaviour>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<MonoBehaviour>();
                }
            }
            return instance;
        }
    }
}
