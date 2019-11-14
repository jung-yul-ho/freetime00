using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public enum ItemType
{
    ITEM_NULL,
    ITEM_HEAD,
    ITEM_BODY,
    ITEM_WEAPON,
    ITEM_CONSUME,
    ITEM_BOOK
};

[System.Serializable]
public class ItemInformation
{
    public int itemnumber;
    public string itemname;
    public ItemType itemtype;
    public Sprite itemsprite;
    public string itemexplain;
    public int ItemCost;
}

public class ItemData : MonoBehaviour
{
    public Item Itemprefab;
    public Dictionary<int, Item> find = new Dictionary<int, Item>();
    public List<Sprite> ItemImage;

    static ItemData instance;
    public static ItemData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemData>();

                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<ItemData>();
                }
            }
            return instance;
        }
    }

    //private void Start()
    //{
    //    foreach (var el in Item_Consume.Instance.items)
    //    {
    //        find[el.iteminformation.itemnumber] = el;
    //    }
    //    foreach (var el in Item_Head.Instance.items)
    //    {
    //        find[el.iteminformation.itemnumber] = el;
    //    }
    //    foreach (var el in Item_Weapon.Instance.items)
    //    {
    //        find[el.iteminformation.itemnumber] = el;
    //    }
    //    foreach (var el in Item_Body.Instance.items)
    //    {
    //        find[el.iteminformation.itemnumber] = el;
    //    }
    //}

    //public void Awake()
    //{
    //    foreach (var el in Item_Consume.Instance.items)
    //    {
    //        find[el.iteminformation.itemnumber] = el;
    //    }
    //    foreach (var el in Item_Head.Instance.items)
    //    {
    //        find[el.iteminformation.itemnumber] = el;
    //    }
    //    foreach (var el in Item_Weapon.Instance.items)
    //    {
    //        find[el.iteminformation.itemnumber] = el;
    //    }
    //    foreach (var el in Item_Body.Instance.items)
    //    {
    //        find[el.iteminformation.itemnumber] = el;
    //    }
    //}

    public Item finditem(int number)
    {
        if (find.ContainsKey(number))
        {
            return find[number];
        }
        return null;
    }
}