using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Weapon : MonoBehaviour
{
    [SerializeField]
    public List<Item> items = new List<Item>();
    Dictionary<int, Item> find = new Dictionary<int, Item>();

    static Item_Weapon instance;
    public static Item_Weapon Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Item_Weapon>();

                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<Item_Weapon>();
                }
            }
            return instance;
        }
    }
}
