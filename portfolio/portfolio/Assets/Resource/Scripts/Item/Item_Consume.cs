using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Consume : MonoBehaviour
{
    [SerializeField]
    public List<Item> items = new List<Item>();
    Dictionary<int, Item> find = new Dictionary<int, Item>();

    static Item_Consume instance;
    public static Item_Consume Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Item_Consume>();

                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<Item_Consume>();
                }
            }
            return instance;
        }
    }

    public void Awake()
    {
        foreach (var el in items)
        {
            find[el.iteminformation.itemnumber] = el;
        }
    }

    public Item finditem(int number)
    {
        if (find.ContainsKey(number))
        {
            return find[number];
        }
        return null;
    }
}
