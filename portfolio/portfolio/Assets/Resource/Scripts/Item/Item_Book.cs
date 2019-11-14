using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Book : MonoBehaviour
{
    [SerializeField]
    public List<Item> items = new List<Item>();
    Dictionary<int, Item> find = new Dictionary<int, Item>();

    static Item_Book instance;
    public static Item_Book Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Item_Book>();

                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<Item_Book>();
                }
            }
            return instance;
        }
    }
}
