using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Head : MonoBehaviour
{
    [SerializeField]
    public List<Item> items = new List<Item>();
    Dictionary<int, Item> find = new Dictionary<int, Item>();

    static Item_Head instance;
    public static Item_Head Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Item_Head>();

                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<Item_Head>();
                }
            }
            return instance;
        }
    }
}
