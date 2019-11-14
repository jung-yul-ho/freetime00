using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Body : MonoBehaviour
{
    [SerializeField]
    public List<Item> items = new List<Item>();
    Dictionary<int, Item> find = new Dictionary<int, Item>();

    static Item_Body instance;
    public static Item_Body Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Item_Body>();

                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<Item_Body>();
                }
            }
            return instance;
        }
    }
}
