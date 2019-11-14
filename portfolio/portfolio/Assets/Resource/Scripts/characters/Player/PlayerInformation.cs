using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    public int Hp;
    public int Shield;
    public int Atk;
    public int Def;
    public int Money;
    static PlayerInformation instance;
    public static PlayerInformation Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerInformation>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<PlayerInformation>();
                }
            }
            return instance;
        }
    }
}
